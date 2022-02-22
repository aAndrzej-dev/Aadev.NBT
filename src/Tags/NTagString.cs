using System;
using System.Text;

namespace Aadev.NBT.Tags
{


    public class NTagString : NTag
    {
        public override NTagType Type => NTagType.String;
        public override bool HasValue => true;


        public string Value { get; set; }


        public override byte? ByteValue { set => Value = value.ToString() ?? string.Empty; }
        public override short? ShortValue { set => Value = value.ToString() ?? string.Empty; }
        public override int? IntValue { set => Value = value.ToString() ?? string.Empty; }
        public override long? LongValue { set => Value = value.ToString() ?? string.Empty; }
        public override float? FloatValue { set => Value = value.ToString() ?? string.Empty; }
        public override double? DoubleValue { set => Value = value.ToString() ?? string.Empty; }
        public override string? StringValue { get => Value; set => Value = value ?? string.Empty; }

        public override int Size => Encoding.UTF8.GetByteCount(Value) + 2 + (IsArraysChild ? 0 : 1) + (HasName ? 2 + Encoding.UTF8.GetByteCount(Name) : 0);

        public NTagString() : this(null, string.Empty) { }
        public NTagString(string name) : this(name, string.Empty) { }
        public NTagString(string? name, string value)
        {
            Name = name;
            Value = value;
        }
        public NTagString(NTagString other)
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
            sb.Append($"\"{Value}\"");
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
            int valueLenght = Encoding.UTF8.GetByteCount(Value);
            if (endianness is Endianness.Big)
            {
                bytes[offset++] = (byte)(valueLenght >> 8);
                bytes[offset++] = (byte)valueLenght;
            }
            else
            {
                bytes[offset++] = (byte)valueLenght;
                bytes[offset++] = (byte)(valueLenght >> 8);
            }
            offset += Encoding.UTF8.GetBytes(Value, bytes.Slice(offset, valueLenght));
        }
    }
}