using System;
using Maru.MCore;

namespace ShelleyStudio.Events {
    public struct EnumDataChangedEvent<T,V> where T : Enum {
        public T Type;
        public V Value;
    }

    public interface IEnumDataTrigger<V> {
        void TriggerEvent(IMessageBus vent, V value);
        int EnumValue();
    }

    public class EnumDataTrigger<T, V> : IEnumDataTrigger<V> where T : Enum {
        private string ventKey;
        private T type;
        
        public EnumDataTrigger(T type, string ventKey) {
            this.type = type;
            this.ventKey = ventKey;
        }
        
        public void TriggerEvent(IMessageBus vent, V value) {
            vent.Trigger(ventKey, new EnumDataChangedEvent<T, V> {
                Type = type,
                Value = value
            });
        }

        public int EnumValue() {
            return (int) (object) type;
        }
    }
}
