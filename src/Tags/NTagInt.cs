using System;
using System.Text;

namespace Aadev.NBT.Tags
{


    public class NTagInt : NTag
    {
        public override NTagType Type => NTagType.Int;
        public override bool HasValue => true;




        public int Value { get; set; }




        public override byte? ByteValue { get => (byte?)Value; set => Value = value ?? 0; }
        public override short? ShortValue { get => (short?)Value; set => Value = value ?? 0; }
        public override int? IntValue { get => Value; set => Value = value ?? 0; }
        public override long? LongValue { get => Value; set => Value = (int)(value ?? 0); }
        public override float? FloatValue { get => Value; set => Value = (int)(value ?? 0); }
        public override double? DoubleValue { get => Value; set => Value = (int)(value ?? 0); }
        public override string? StringValue => $"{Value}";

        public override int Size => sizeof(int) + (IsArraysChild ? 0 : 1) + (HasName ? 2 + Encoding.UTF8.GetByteCount(Name) : 0);

        public NTagInt() { }
        public NTagInt(string? name) : this(name, 0) { }
        public NTagInt(int value) : this(null, value) { }
        public NTagInt(string? name, int value)
        {
            Name = name;
            Value = value;
        }
        public NTagInt(NTagInt other)
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
            sb.Append($"{Value}");

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
            }

        }

        public static explicit operator NTagByte(NTagInt tag) => new NTagByte(tag.Name, (byte)tag.Value);

        public static explicit operator NTagShort(NTagInt tag) => new NTagShort(tag.Name, (short)tag.Value);

        public static explicit operator NTagLong(NTagInt tag) => new NTagLong(tag.Name, tag.Value);

        public static explicit operator NTagFloat(NTagInt tag) => new NTagFloat(tag.Name, tag.Value);

        public static explicit operator NTagDouble(NTagInt tag) => new NTagDouble(tag.Name, tag.Value);
    }
}