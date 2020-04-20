using System.Runtime.CompilerServices;
using UnityEngine;

namespace Maru.MCore
{
    public static class MaruUtils
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LayerMaskContains(LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }
    }
}
