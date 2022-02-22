using System;
using System.Text;

namespace Aadev.NBT.Tags
{

    public class NTagEnd : NTag
    {
        public override NTagType Type => NTagType.End;
        public override bool HasName => false;
        public override bool HasValue => false;

        public override int Size => 1;

        internal override void ToByteArray(ref int offset, Span<byte> bytes, Endianness endianness) => bytes[offset++] = Type.Id;
        internal override void ToSNBT(StringBuilder sb) => throw new Exception($"Cannot convert {nameof(NTagEnd)} to SNBT");
    }
}