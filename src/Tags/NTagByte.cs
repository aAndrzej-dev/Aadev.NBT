namespace Aadev.NBT.Tags
{



    public class NTagByte : NTag
    {
        /// <summary>
        /// The type of current tag
        /// </summary>
        public override NTagType Type => NTagType.Byte;
        /// <summary>
        /// Returns true, if current tag has value.
        /// </summary>
        public override bool HasValue => true;


        public byte Value { get; set; }

        /// <summary>
        /// 1 bit value of current tag; If current tag dosn't have numeric value, it return <see langword="null"/>.
        /// </summary>
        public override byte? ByteValue { get => Value; set => Value = value ?? 0; }
        /// <summary>
        /// 2 bit value of current tag; If current tag dosn't have numeric value, it return <see langword="null"/>.
        /// </summary>
        public override short? ShortValue { get => Value; set => Value = (byte)(value ?? 0); }
        /// <summary>
        /// 4 bit value of current tag; If current tag dosn't have numeric value, it return <see langword="null"/>.
        /// </summary>
        public override int? IntValue { get => Value; set => Value = (byte)(value ?? 0); }
        /// <summary>
        /// 8 bit value of current tag; If current tag dosn't have numeric value, it return <see langword="null"/>.
        /// </summary>
        public override long? LongValue { get => Value; set => Value = (byte)(value ?? 0); }
        /// <summary>
        /// Single precision value of current tag; If current tag dosn't have numeric value, it return <see langword="null"/>.
        /// </summary>
        public override float? FloatValue { get => Value; set => Value = (byte)(value ?? 0); }
        /// <summary>
        /// Double precision value of current tag; If current tag dosn't have numeric value, it return <see langword="null"/>.
        /// </summary>
        public override double? DoubleValue { get => Value; set => Value = (byte)(value ?? 0); }
        /// <summary>
        /// String representaion of value. It is null, when current tag dosn't have value.
        /// </summary>
        public override string? StringValue => $"{Value}b";
        /// <summary>
        /// Size in bytes that is taken up by current tag in NBT data.
        /// </summary>
        public override int Size => sizeof(byte) + (IsArraysChild ? 0 : 1) + (HasName ? 2 + Encoding.UTF8.GetByteCount(Name) : 0);

        public NTagByte() { }
        public NTagByte(string? name) : this(name, 0) { }
        public NTagByte(byte value) : this(null, value) { }
        public NTagByte(string? name, byte value)
        {
            Name = name;
            Value = value;
        }
        public NTagByte(NTagByte other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            Name = other.Name;
            Value = other.Value;
        }

        internal override void ToDisplayTree(StringBuilder sb, int depth, char depthChar)
        {
            sb.Append(new string(depthChar, depth));

            sb.Append(Type.TagName);
            if (!string.IsNullOrEmpty(Name))
            {
                sb.Append($"(\"{Name}\")");
            }

            sb.Append(": ");
            sb.Append(Value);
        }
        internal override void ToSNBT(StringBuilder sb)
        {
            if (HasName)
            {
                sb.Append($"\"{Name}\":");
            }
            sb.Append(StringValue);
        }
        public override string ToSNBT() => $"{(HasName ? $"\"{Name}\":" : string.Empty)}{StringValue}";

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
            bytes[offset++] = Value;
        }


        public static explicit operator NTagShort(NTagByte tag) => new NTagShort(tag.Name, tag.Value);

        public static explicit operator NTagInt(NTagByte tag) => new NTagInt(tag.Name, tag.Value);

        public static explicit operator NTagLong(NTagByte tag) => new NTagLong(tag.Name, tag.Value);

        public static explicit operator NTagFloat(NTagByte tag) => new NTagFloat(tag.Name, tag.Value);

        public static explicit operator NTagDouble(NTagByte tag) => new NTagDouble(tag.Name, tag.Value);
    }
}