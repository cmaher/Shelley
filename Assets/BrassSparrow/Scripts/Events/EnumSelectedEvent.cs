using System;
using Maru.MCore;

namespace BrassSparrow.Scripts.Events {
    public struct EnumSelectedEvent<T> where T : Enum {
        public T Type;
    }
    
    public interface IEnumSelection {
        void TriggerEvent(IMessageBus vent);
        int EnumValue();
    }

    public class EnumSelection<T> : IEnumSelection where T : Enum {
        private EnumSelectedEvent<T> evt;
        private string key;

        public EnumSelection(T type, string ventKey) {
            key = ventKey;
            evt = new EnumSelectedEvent<T> {Type = type};
        }

        public int EnumValue() {
            return (int) (object) evt.Type;
        }

        public void TriggerEvent(IMessageBus vent) {
            vent.Trigger(key, evt);
        }
    }
}
