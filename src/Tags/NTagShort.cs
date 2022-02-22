using System;
using System.Text;

namespace Aadev.NBT.Tags
{



    public class NTagShort : NTag
    {
        public override NTagType Type => NTagType.Short;
        public override bool HasValue => true;
        public short Value { get; set; }


        public override byte? ByteValue { get => (byte?)Value; set => Value = value ?? 0; }
        public override short? ShortValue { get => Value; set => Value = value ?? 0; }
        public override int? IntValue { get => Value; set => Value = (short)(value ?? 0); }
        public override long? LongValue { get => Value; set => Value = (short)(value ?? 0); }
        public override float? FloatValue { get => Value; set => Value = (short)(value ?? 0); }
        public override double? DoubleValue { get => Value; set => Value = (short)(value ?? 0); }
        public override string? StringValue => $"{Value}s";

        public override int Size => sizeof(short) + (IsArraysChild ? 0 : 1) + (HasName ? 2 + Encoding.UTF8.GetByteCount(Name) : 0);

        public NTagShort() { }
        public NTagShort(string? name) : this(name, 0) { }
        public NTagShort(short value) : this(null, value) { }
        public NTagShort(string? name, short value)
        {
            Name = name;
            Value = value;
        }
        public NTagShort(NTagShort other)
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
            sb.Append($"{Value}s");
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

            if (endianness is Endianness.Big)
            {
                bytes[offset++] = (byte)(Value >> 8);
                bytes[offset++] = (byte)Value;
            }
            else
            {
                bytes[offset++] = (byte)Value;
                bytes[offset++] = (byte)(Value >> 8);
            }

        }

        public static explicit operator NTagByte(NTagShort tag) => new NTagByte(tag.Name, (byte)tag.Value);

        public static explicit operator NTagInt(NTagShort tag) => new NTagInt(tag.Name, tag.Value);

        public static explicit operator NTagLong(NTagShort tag) => new NTagLong(tag.Name, tag.Value);

        public static explicit operator NTagFloat(NTagShort tag) => new NTagFloat(tag.Name, tag.Value);

        public static explicit operator NTagDouble(NTagShort tag) => new NTagDouble(tag.Name, tag.Value);
    }
}