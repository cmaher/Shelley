using System.Runtime.CompilerServices;
using Maru.MCore;

namespace BrassSparrow.Scripts.Core {
    public abstract class BrassSparrowBehavior : VentBehavior {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override string VentKey() {
            return SceneManager.VentKey;
        }
    }
}
