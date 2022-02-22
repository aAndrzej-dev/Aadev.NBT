using System.Collections.Generic;

namespace Aadev.NBT
{

    public interface INTagParent : INTag
    {
        IList<NTag> Children { get; }
        void AddChild(NTag nTag);
        void RemoveChild(NTag nTag);
    }
}