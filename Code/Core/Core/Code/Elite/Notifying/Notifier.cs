using LightHouse.Core.Caching;
using LightHouse.Core.Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Elite.Notifying
{
    /// <summary>
    ///  Officer responsible for notifying events.
    /// </summary>
    public class Notifier
    {
        private DataCache cache = new DataCache();

        /// <summary>
        /// Subscribe to the specified event handler.
        /// </summary>
        /// <typeparam name="T">Type of the event arguments.</typeparam>
        /// <param name="handler">Event handler to be subscribed to.</param>
        public void AddHandler<T>(EventHandler<T> handler) where T : EventArgs
        {
            String name = typeof(T).FullName;

            EventHandler<T> eventHandler = cache.Get<EventHandler<T>>(name);

            eventHandler += new WeakEventHandler<T>(handler, delegate(EventHandler<T> addingEventHandler)
            {
                eventHandler -= addingEventHandler;
            }); 

            cache.Add(name, eventHandler);
        }

        /// <summary>
        /// Triggers the notification.
        /// </summary>
        /// <typeparam name="T">Type of the arguments of the notification.</typeparam>
        /// <param name="sender">Sender that is triggering the notification.</param>
        /// <param name="args">Arguments of the notification.</param>
        public void Notify<T>(object sender, T args) where T : EventArgs
        {
            String name = typeof(T).FullName;
            EventHandler<T> eventHandler = cache.Get<EventHandler<T>>(name);

            if (eventHandler != null)
            {
                eventHandler(sender, args);
            }
        }
    }
}
