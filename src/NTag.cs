using Aadev.NBT.Tags;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Aadev.NBT
{



    public abstract class NTag : INTag
    {
        private string? name;

        /// <summary>
        /// Returns true, if current tag is child of <see cref="INTagArray"/>.
        /// </summary>
#if NET5_0_OR_GREATER
        [MemberNotNullWhen(true, nameof(Parent))]
#endif
        public bool IsArraysChild => Parent is NTagList || Parent is NTagByteArray || Parent is NTagIntArray || Parent is NTagLongArray;
        /// <summary>
        /// Returns true, if current tag has name and isn't chlid of <see cref="INTagArray"/>.
        /// </summary>
#if NET5_0_OR_GREATER
        [MemberNotNullWhen(true, nameof(Name))]
#endif
        public virtual bool HasName => !IsArraysChild;
        /// <summary>
        /// Returns true, if current tag is root of nbt file and dosn't have parent.
        /// </summary>
#if NET5_0_OR_GREATER
        [MemberNotNullWhen(false, nameof(Parent))]
#endif
        public bool IsRoot => Parent is null;

        /// <summary>
        /// Returns true, if current tag has value.
        /// </summary>
        public abstract bool HasValue { get; }


        /// <summary>
        /// The type of current tag
        /// </summary>
        public abstract NTagType Type { get; }
        /// <summary>
        /// Size in bytes that is taken up by current tag in NBT data.
        /// </summary>
        public abstract int Size { get; }



        /// <summary>
        /// Name of current tag. The value is <see langword="null"/>, if HasName is false.
        /// </summary>
        public string? Name { get => HasName ? name : null; set => name = HasName ? (value ?? string.Empty) : null; }
        /// <summary>
        /// Parent tag of current tag.
        /// </summary>
        public INTagParent? Parent { get; set; }

        /// <summary>
        /// 1 bit value of current tag; If current tag dosn't have numeric value, it return <see langword="null"/>.
        /// </summary>
        public virtual byte? ByteValue { get => null; set => _ = value; }
        /// <summary>
        /// String representaion of value. It is null, when current tag dosn't have value.
        /// </summary>
        public virtual short? ShortValue { get => null; set => _ = value; }
        /// <summary>
        /// 4 bit value of current tag; If current tag dosn't have numeric value, it return <see langword="null"/>.
        /// </summary>
        public virtual int? IntValue { get => null; set => _ = value; }
        /// <summary>
        /// 8 bit value of current tag; If current tag dosn't have numeric value, it return <see langword="null"/>.
        /// </summary>
        public virtual long? LongValue { get => null; set => _ = value; }
        /// <summary>
        /// Single precision value of current tag; If current tag dosn't have numeric value, it return <see langword="null"/>.
        /// </summary>
        public virtual float? FloatValue { get => null; set => _ = value; }
        /// <summary>
        /// Double precision value of current tag; If current tag dosn't have numeric value, it return <see langword="null"/>.
        /// </summary>
        public virtual double? DoubleValue { get => null; set => _ = value; }
        /// <summary>
        /// String representaion of value. It is null, when current tag dosn't have value.
        /// </summary>
        public virtual string? StringValue { get => null; set => _ = value; }


        internal abstract void ToSNBT(StringBuilder sb);
        internal virtual void ToDisplayTree(StringBuilder sb, int depth, char depthChar) { }
        internal abstract void ToByteArray(ref int offset, Span<byte> bytes, Endianness endianness);
        /// <summary>
        /// Returns SNBT representaion of current tag
        /// </summary>
        public virtual string ToSNBT()
        {
            StringBuilder sb = new StringBuilder();
            ToSNBT(sb);
            return sb.ToString();

        }


        /// <summary>
        /// Convert to NBT data
        /// </summary>
        public byte[] ToByteArray(Endianness endianness)
        {
            Span<byte> span = stackalloc byte[Size];
            int offset = 0;
            ToByteArray(ref offset, span, endianness);

            return span.ToArray();
        }

        /// <summary>
        /// Returns tree of tags.
        /// </summary>
        public virtual string ToDisplayTree()
        {
            StringBuilder sb = new StringBuilder();
            ToDisplayTree(sb, 0, '\t');
            return sb.ToString();
        }
    }

}