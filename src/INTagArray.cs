namespace Aadev.NBT
{


    public interface INTagArray : INTagParent
    {
        /// <summary>
        /// Type of each child element in array
        /// </summary>
        NTagType? ChildType { get; }
    }
}