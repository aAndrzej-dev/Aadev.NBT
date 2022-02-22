using System;
using System.Text;

namespace Aadev.NBT.Tags
{


    public class NTagLong : NTag
    {
        public override NTagType Type => NTagType.Long;
        public override bool HasValue => true;

        public long Value { get; set; }


        public override byte? ByteValue { get => (byte?)Value; set => Value = value ?? 0; }
        public override short? ShortValue { get => (short?)Value; set => Value = value ?? 0; }
        public override int? IntValue { get => (int?)Value; set => Value = value ?? 0; }
        public override long? LongValue { get => Value; set => Value = value ?? 0; }
        public override float? FloatValue { get => Value; set => Value = (long)(value ?? 0); }
        public override double? DoubleValue { get => Value; set => Value = (long)(value ?? 0); }
        public override string? StringValue => $"{Value}l";

        public override int Size => sizeof(long) + (IsArraysChild ? 0 : 1) + (HasName ? 2 + Encoding.UTF8.GetByteCount(Name) : 0);

        public NTagLong() { }
        public NTagLong(string? name) : this(name, 0) { }
        public NTagLong(long value) : this(null, value) { }
        public NTagLong(string? name, long value)
        {
            Name = name;
            Value = value;
        }
        public NTagLong(NTagInt other)
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
            sb.Append($"{Value}l");
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
                bytes[offset++] = (byte)(Value >> 56);
                bytes[offset++] = (byte)(Value >> 48);
                bytes[offset++] = (byte)(Value >> 40);
                bytes[offset++] = (byte)(Value >> 32);

                bytes[offset++] = (byte)(Value >> 24);
                bytes[offset++] = (byte)(Value >> 16);
                bytes[offset++] = (byte)(Value >> 8);
                bytes[offset++] = (byte)Value;
            }
            else
            {
                bytes[offset++] = (byte)Value;
                bytes[offset++] = (byte)(Value >> 8);
                bytes[offset++] = (byte)(Value >> 16);
                bytes[offset++] = (byte)(Value >> 24);

                bytes[offset++] = (byte)(Value >> 32);
                bytes[offset++] = (byte)(Value >> 40);
                bytes[offset++] = (byte)(Value >> 48);
                bytes[offset++] = (byte)(Value >> 56);
            }

        }

        public static explicit operator NTagByte(NTagLong tag) => new NTagByte(tag.Name, (byte)tag.Value);

        public static explicit operator NTagShort(NTagLong tag) => new NTagShort(tag.Name, (short)tag.Value);

        public static explicit operator NTagInt(NTagLong tag) => new NTagInt(tag.Name, (int)tag.Value);

        public static explicit operator NTagFloat(NTagLong tag) => new NTagFloat(tag.Name, tag.Value);

        public static explicit operator NTagDouble(NTagLong tag) => new NTagDouble(tag.Name, tag.Value);
    }
}