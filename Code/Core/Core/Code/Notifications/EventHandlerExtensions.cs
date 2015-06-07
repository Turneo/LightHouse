using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LightHouse.Core.Notifications
{
    /// <summary>
    /// Callback for unregistering an event handler.
    /// </summary>
    /// <typeparam name="T">Type of the event arguments.</typeparam>
    /// <param name="eventHandler">Event handler that will be unregistered.</param>
    public delegate void UnregisterCallback<T>(EventHandler<T> eventHandler) where T : EventArgs;

    /// <summary>
    /// Extensions for the EventHandlers.
    /// </summary>
    public static class EventHandlerExtensions
    {
        /// <summary>
        /// Extension method, to check if the handler method already added.
        /// </summary>
        /// <typeparam name="T">Type of the event arguments</typeparam>
        /// <param name="this">Event handler that need to be check for a specified delegate.</param>
        /// <param name="delegate">Delegate that has to be searched within the event handler.</param>
        /// <returns>True if the event handler has been found; otherwise false;</returns>
        public static bool ContainsHandler<T>(this EventHandler<T> @this, Delegate @delegate) where T : EventArgs
        {
            if (@this != null)
            {
                foreach (var invocation in @this.GetInvocationList())
                {
                    WeakEventHandler<T> weakEventHandler = (WeakEventHandler<T>)invocation.Target;

                    if (weakEventHandler.IsSameTarget(@delegate))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Creates a weak event handler for the PropertyChangingEvent.
        /// </summary>
        /// <typeparam name="T">Type of the event arguments.</typeparam>
        /// <param name="this">PropertyChangingEvent for which the extension is provided.</param>
        /// <param name="unregister">Callback for unregistering the event handler.</param>
        /// <returns>A weak event handler for the PropertyChangingEvent.</returns>
        public static WeakEventHandler<T> MakeWeak<T>(this PropertyChangingEventHandler @this, UnregisterCallback<T> unregister) where T : EventArgs
        {
            return WeakEventHandler<T>.MakeFromDelegate(@this, unregister);
        }

        /// <summary>
        /// Creates a weak event handler for the PropertyChangedEvent.
        /// </summary>
        /// <typeparam name="T">Type of the event arguments.</typeparam>
        /// <param name="this">PropertyChangedEvent for which the extension is provided.</param>
        /// <param name="unregister">Callback for unregistering the event handler.</param>
        /// <returns>A weak event handler for the PropertyChangedEvent.</returns>
        public static WeakEventHandler<T> MakeWeak<T>(this PropertyChangedEventHandler @this, UnregisterCallback<T> unregister) where T : EventArgs
        {
            return WeakEventHandler<T>.MakeFromDelegate(@this, unregister);
        }

        /// <summary>
        /// Creates a weak event handler for the ObjectCreatedEvent.
        /// </summary>
        /// <typeparam name="T">Type of the event arguments.</typeparam>
        /// <param name="this">ObjectCreatedEvent for which the extension is provided.</param>
        /// <param name="unregister">Callback for unregistering the event handler.</param>
        /// <returns>A weak event handler for the ObjectCreatedEvent.</returns>
        public static WeakEventHandler<T> MakeWeak<T>(this ObjectCreatedEventHandler @this, UnregisterCallback<T> unregister) where T : EventArgs
        {
            return WeakEventHandler<T>.MakeFromDelegate(@this, unregister);
        }

        /// <summary>
        /// Creates a weak event handler for the UpdateObjectPropertyEvent.
        /// </summary>
        /// <typeparam name="T">Type of the event arguments.</typeparam>
        /// <param name="this">UpdateObjectPropertyEvent for which the extension is provided.</param>
        /// <param name="unregister">Callback for unregistering the event handler.</param>
        /// <returns>A weak event handler for the UpdateObjectPropertyEvent.</returns>
        public static WeakEventHandler<T> MakeWeak<T>(this UpdateObjectPropertyEventHandler @this, UnregisterCallback<T> unregister) where T : EventArgs
        {
            return WeakEventHandler<T>.MakeFromDelegate(@this, unregister);
        }


    }
}
