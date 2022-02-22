using System;
using System.Linq;
using System.Text;

namespace Aadev.NBT.Tags
{


    public class NTagDouble : NTag
    {
        public override NTagType Type => NTagType.Double;
        public override bool HasValue => true;

        public double Value { get; set; }


        public override byte? ByteValue { get => (byte?)Value; set => Value = value ?? 0; }
        public override short? ShortValue { get => (short?)Value; set => Value = value ?? 0; }
        public override int? IntValue { get => (int?)Value; set => Value = value ?? 0; }
        public override long? LongValue { get => (long?)Value; set => Value = value ?? 0; }
        public override float? FloatValue { get => (float?)Value; set => Value = value ?? 0; }
        public override double? DoubleValue { get => Value; set => Value = value ?? 0; }
        public override string? StringValue => $"{Value}d";

        public override int Size => sizeof(double) + (IsArraysChild ? 0 : 1) + (HasName ? 2 + Encoding.UTF8.GetByteCount(Name) : 0);

        public NTagDouble() { }
        public NTagDouble(string? name) : this(name, 0) { }
        public NTagDouble(double value) : this(null, value) { }
        public NTagDouble(string? name, double value)
        {
            Name = name;
            Value = value;
        }
        public NTagDouble(NTagDouble other)
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
            sb.Append($"{Value}d");
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
                BitConverter.GetBytes(Value).Reverse().ToArray().CopyTo(bytes.Slice(offset, sizeof(double)));
            else
                BitConverter.GetBytes(Value).CopyTo(bytes.Slice(offset, sizeof(double)));

            offset += sizeof(double);
        }
        public static explicit operator NTagByte(NTagDouble tag) => new NTagByte(tag.Name, (byte)tag.Value);

        public static explicit operator NTagShort(NTagDouble tag) => new NTagShort(tag.Name, (short)tag.Value);

        public static explicit operator NTagInt(NTagDouble tag) => new NTagInt(tag.Name, (int)tag.Value);

        public static explicit operator NTagLong(NTagDouble tag) => new NTagLong(tag.Name, (long)tag.Value);

        public static explicit operator NTagFloat(NTagDouble tag) => new NTagFloat(tag.Name, (float)tag.Value);
    }
}