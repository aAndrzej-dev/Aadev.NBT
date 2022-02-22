using Aadev.NBT.Tags;
using System;

namespace Aadev.NBT
{



    public class NTagType : IEquatable<NTagType?>
    {
        private readonly Func<NTag> instanceFactory;


        private NTagType(string name, byte id, string tagName, Func<NTag> instanceFactory)
        {
            Name = name;
            Id = id;
            TagName = tagName;
            this.instanceFactory = instanceFactory;
        }

        public static readonly NTagType End = new NTagType(nameof(End), 0, "TAG_End", () => new NTagEnd());
        public static readonly NTagType Byte = new NTagType(nameof(Byte), 1, "TAG_Byte", () => new NTagByte());
        public static readonly NTagType Short = new NTagType(nameof(Short), 2, "TAG_Short", () => new NTagShort());
        public static readonly NTagType Int = new NTagType(nameof(Int), 3, "TAG_Int", () => new NTagInt());
        public static readonly NTagType Long = new NTagType(nameof(Long), 4, "TAG_Long", () => new NTagLong());
        public static readonly NTagType Float = new NTagType(nameof(Float), 5, "TAG_Float", () => new NTagFloat());
        public static readonly NTagType Double = new NTagType(nameof(Double), 6, "TAG_Double", () => new NTagDouble());
        public static readonly NTagType ByteArray = new NTagType(nameof(ByteArray), 7, "TAG_Byte_Array", () => new NTagByteArray());
        public static readonly NTagType String = new NTagType(nameof(String), 8, "TAG_String", () => new NTagString());
        public static readonly NTagType List = new NTagType(nameof(List), 9, "TAG_List", () => new NTagList());
        public static readonly NTagType Compound = new NTagType(nameof(Compound), 10, "TAG_Compound", () => new NTagCompound());
        public static readonly NTagType IntArray = new NTagType(nameof(IntArray), 11, "TAG_Int_Array", () => new NTagIntArray());
        public static readonly NTagType LongArray = new NTagType(nameof(LongArray), 12, "TAG_Long_Array", () => new NTagLongArray());

        public string Name { get; }
        public byte Id { get; }
        public string TagName { get; }
        public NTag Instance => instanceFactory();


        public static NTagType GetTagTypeById(byte id)
        {
            return id switch
            {
                0 => NTagType.End,
                1 => NTagType.Byte,
                2 => NTagType.Short,
                3 => NTagType.Int,
                4 => NTagType.Long,
                5 => NTagType.Float,
                6 => NTagType.Double,
                7 => NTagType.ByteArray,
                8 => NTagType.String,
                9 => NTagType.List,
                10 => NTagType.Compound,
                11 => NTagType.IntArray,
                12 => NTagType.LongArray,
                _ => throw new IndexOutOfRangeException($"NBT Tag with id `{id}` dosn't exist"),
            };
        }

        public override bool Equals(object? obj) => Equals(obj as NTagType);
        public bool Equals(NTagType? other) => other != null && Id == other.Id;
        public override int GetHashCode() => HashCode.Combine(Id);

        public static bool operator ==(NTagType? left, NTagType? right) => left?.Id == right?.Id;
        public static bool operator !=(NTagType? left, NTagType? right) => !(left == right);
    }
}