using Maru.MCore;

namespace BrassSparrow.Scripts.Core {
    public abstract class BrassSparrowBehavior : VentBehavior {
        protected override string VentLocatorKey => SceneManager.VentKey;
    }
}
