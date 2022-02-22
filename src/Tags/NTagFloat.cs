using System;
using System.Linq;
using System.Text;

namespace Aadev.NBT.Tags
{



    public class NTagFloat : NTag
    {
        public override NTagType Type => NTagType.Float;

        public override bool HasValue => true;

        public float Value { get; set; }



        public override byte? ByteValue { get => (byte?)Value; set => Value = value ?? 0; }
        public override short? ShortValue { get => (short?)Value; set => Value = value ?? 0; }
        public override int? IntValue { get => (int?)Value; set => Value = value ?? 0; }
        public override long? LongValue { get => (long?)Value; set => Value = value ?? 0; }
        public override float? FloatValue { get => Value; set => Value = value ?? 0; }
        public override double? DoubleValue { get => Value; set => Value = (float)(value ?? 0); }
        public override string? StringValue => $"{Value}f";

        public override int Size => sizeof(float) + (IsArraysChild ? 0 : 1) + (HasName ? 2 + Encoding.UTF8.GetByteCount(Name) : 0);

        public NTagFloat() { }
        public NTagFloat(string? name) : this(name, 0) { }
        public NTagFloat(float value) : this(null, value) { }
        public NTagFloat(string? name, float value)
        {
            Name = name;
            Value = value;
        }
        public NTagFloat(NTagFloat other)
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
            if (endianness is Endianness.Big)
                BitConverter.GetBytes(Value).Reverse().ToArray().CopyTo(bytes.Slice(offset, sizeof(float)));
            else
                BitConverter.GetBytes(Value).CopyTo(bytes.Slice(offset, sizeof(float)));
            offset += sizeof(float);
        }
        public static explicit operator NTagByte(NTagFloat tag) => new NTagByte(tag.Name, (byte)tag.Value);

        public static explicit operator NTagShort(NTagFloat tag) => new NTagShort(tag.Name, (short)tag.Value);

        public static explicit operator NTagInt(NTagFloat tag) => new NTagInt(tag.Name, (int)tag.Value);

        public static explicit operator NTagLong(NTagFloat tag) => new NTagLong(tag.Name, (long)tag.Value);

        public static explicit operator NTagDouble(NTagFloat tag) => new NTagDouble(tag.Name, tag.Value);
    }
}