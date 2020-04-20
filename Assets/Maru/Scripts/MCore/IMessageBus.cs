using System;

namespace Maru.MCore {
    public interface IMessageBus {
        /**
		 * Subscribe to an event off type TEvent on this message bus.
		 * Returns an action that tears down the listener.
		 */
        Action On<TEvent>(Action<TEvent> handler);
        
        /**
         * Subscribe to an action, and remove the handler after one call
         */
        Action Once<TEvent>(Action<TEvent> handler);

        /**
		 * Tear down the provided listener, ensuring it won't be called again
		 */
        void Unsubscribe<TEvent>(Action<TEvent> handler);

        /**
		 * Send the message to all handlers registered for events of type TEvent
		 */
        void Trigger<TEvent>(TEvent message);
    }
}
