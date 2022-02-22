
namespace Aadev.NBT
{


    public interface INTag
    {
        /// <summary>
        /// 1 bit value of current tag; If current tag dosn't have numeric value, it return <see langword="null"/>.
        /// </summary>
        byte? ByteValue { get; set; }
        /// <summary>
        /// Double precision value of current tag; If current tag dosn't have numeric value, it return <see langword="null"/>.
        /// </summary>
        double? DoubleValue { get; set; }
        /// <summary>
        /// Single precision value of current tag; If current tag dosn't have numeric value, it return <see langword="null"/>.
        /// </summary>
        float? FloatValue { get; set; }
        /// <summary>
        /// Returns true, if current tag has name and isn't chlid of <see cref="INTagArray"/>.
        /// </summary>
        bool HasName { get; }
        /// <summary>
        /// Returns true, if current tag has value.
        /// </summary>
        bool HasValue { get; }
        /// <summary>
        /// 4 bit value of current tag; If current tag dosn't have numeric value, it return <see langword="null"/>.
        /// </summary>
        int? IntValue { get; set; }
        /// <summary>
        /// Returns true, if current tag is child of <see cref="INTagArray"/>.
        /// </summary>
        bool IsArraysChild { get; }
        /// <summary>
        /// Returns true, if current tag is root of nbt file and dosn't have parent.
        /// </summary>
        bool IsRoot { get; }
        /// <summary>
        /// 8 bit value of current tag; If current tag dosn't have numeric value, it return <see langword="null"/>.
        /// </summary>
        long? LongValue { get; set; }
        /// <summary>
        /// Name of current tag. The value is <see langword="null"/>, if HasName is false.
        /// </summary>
        string? Name { get; set; }
        /// <summary>
        /// Parent tag of current tag.
        /// </summary>
        INTagParent? Parent { get; set; }
        /// <summary>
        /// 2 bit value of current tag; If current tag dosn't have numeric value, it return <see langword="null"/>.
        /// </summary>
        short? ShortValue { get; set; }
        /// <summary>
        /// String representaion of value. It is null, when current tag dosn't have value.
        /// </summary>
        string? StringValue { get; set; }
        /// <summary>
        /// The type of current tag
        /// </summary>
        NTagType Type { get; }
        /// <summary>
        /// Size in bytes that is taken up by current tag in NBT data.
        /// </summary>
        int Size { get; }

        /// <summary>
        /// Returns SNBT representaion of current tag
        /// </summary>
        string ToSNBT();
        /// <summary>
        /// Returns tree of tags.
        /// </summary>
        string ToDisplayTree();
        /// <summary>
        /// Convert to NBT data
        /// </summary>
        byte[] ToByteArray(Endianness endianness);
    }
}