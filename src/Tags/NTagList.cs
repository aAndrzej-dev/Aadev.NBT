namespace Aadev.NBT.Tags
{
    public class NTagList : NTag, INTagArray
    {
        private readonly IList<NTag> children = new List<NTag>();




        public override NTagType Type => NTagType.List;


        /// <summary>
        /// Type of each child element in array
        /// </summary>
        public NTagType? ChildType { get; set; }

        public override bool HasValue => false;
        public IList<NTag> Children => children;


        public override int Size
        {
            get
            {
                int sum = sizeof(int) + sizeof(byte);

                for (int i = 0; i < children.Count; i++)
                {
                    sum += children[i].Size;
                }
                if (!IsArraysChild)
                    sum += 1;
                if (HasName)
                    sum += 2 + Encoding.UTF8.GetByteCount(Name);

                return sum;
            }

        }
        internal override void ToDisplayTree(StringBuilder sb, int depth, char depthChar)
        {
            sb.Append(new string(depthChar, depth));

            sb.Append(Type.TagName);
            if (!string.IsNullOrEmpty(Name))
            {
                sb.Append($"(\"{Name}\")");
            }

            sb.Append($": {children.Count} entries {{");
            if (children.Count > 0)
            {
                sb.Append('\n');

                foreach (NTag tag in children)
                {
                    tag.ToDisplayTree(sb, depth + 1, depthChar);
                    sb.Append('\n');
                }

                sb.Append(new string(depthChar, depth));
            }
            sb.Append('}');
        }
        internal override void ToSNBT(StringBuilder sb)
        {
            if (HasName)
            {
                sb.Append($"\"{Name}\":");
            }
            sb.Append('[');

            for (int i = 0; i < children.Count; i++)
            {
                NTag? item = children[i];
                if (i > 0)
                    sb.Append(',');
                item.ToSNBT(sb);

            }

            sb.Append(']');
        }
#if NET6_0_OR_GREATER
        public override string ToSNBT()
        {
            DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler();

            if (HasName)
            {
                defaultInterpolatedStringHandler.AppendLiteral("\"");
                defaultInterpolatedStringHandler.AppendFormatted(Name);
                defaultInterpolatedStringHandler.AppendLiteral("\":");
            }

            defaultInterpolatedStringHandler.AppendLiteral("[");

            for (int i = 0; i < children.Count; i++)
            {
                if (i > 0)
                {
                    defaultInterpolatedStringHandler.AppendLiteral(",");
                }
                defaultInterpolatedStringHandler.AppendFormatted(children[i].ToSNBT());
            }

            defaultInterpolatedStringHandler.AppendLiteral("]");

            return defaultInterpolatedStringHandler.ToStringAndClear();

        }
#endif
        public void AddChild(NTag nTag)
        {
            if (nTag is null)
            {
                throw new ArgumentNullException(nameof(nTag));
            }
            if (nTag == this)
            {
                throw new Exception("Cannot add self to array");
            }


            nTag.Parent = this;
            children.Add(nTag);

        }

        public void RemoveChild(NTag nTag)
        {
            nTag.Parent = null;
            children.Remove(nTag);

        }


        internal override void ToByteArray(ref int offset, Span<byte> bytes, Endianness endianness)
        {
            if (!IsArraysChild)
                bytes[offset++] = Type.Id;

            if (HasName)
            {
                int nameLenght = Encoding.UTF8.GetByteCount(Name);

                if (endianness is Endianness.Big)
                {
                    bytes[offset++] = (byte)(nameLenght >> 8);
                    bytes[offset++] = (byte)nameLenght;
                }
                else
                {
                    bytes[offset++] = (byte)nameLenght;
                    bytes[offset++] = (byte)(nameLenght >> 8);
                }
                offset += Encoding.UTF8.GetBytes(Name, bytes.Slice(offset, nameLenght));
            }

            bytes[offset++] = ChildType!.Id;
            if (endianness is Endianness.Big)
            {
                bytes[offset++] = (byte)(children.Count >> 24);
                bytes[offset++] = (byte)(children.Count >> 16);
                bytes[offset++] = (byte)(children.Count >> 8);
                bytes[offset++] = (byte)children.Count;
            }
            else
            {
                bytes[offset++] = (byte)children.Count;
                bytes[offset++] = (byte)(children.Count >> 8);
                bytes[offset++] = (byte)(children.Count >> 16);
                bytes[offset++] = (byte)(children.Count >> 24);
            }

            foreach (NTag? item in children)
            {
                item.ToByteArray(ref offset, bytes, endianness);
            }

        }
    }
}