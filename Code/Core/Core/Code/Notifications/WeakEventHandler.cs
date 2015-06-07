using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Notifications
{
    /// <summary>
    /// Weak event handler to avoid problems with memory leaks.
    /// </summary>
    /// <typeparam name="T">Type of the event arguments.</typeparam>
    public class WeakEventHandler<T> where T : EventArgs
    {
        /// <summary>
        /// Target as weak reference.
        /// </summary>
        private WeakReference target;

        /// <summary>
        /// Method information of the event handler.
        /// </summary>
        private MethodInfo method;

        /// <summary>
        /// Event handler of the weak reference.
        /// </summary>
        private EventHandler<T> handler;

        /// <summary>
        /// Callback for unregistering.
        /// </summary>
        private UnregisterCallback<T> unregister;

        /// <summary>
        /// Initializes a new instance, use in private.
        /// </summary>
        private WeakEventHandler()
        {
            this.handler = Invoke;
        }

        /// <summary>
        /// Initializes a new instance of WeakEventHandler.
        /// </summary>
        /// <param name="eventHandler">Event handler to be used for the weak event handler.</param>
        /// <param name="unregister">Callback for unregistering the event handler.</param>
        public WeakEventHandler(EventHandler<T> eventHandler, UnregisterCallback<T> unregister)
        {
            this.target = new WeakReference(eventHandler.Target);
            this.method = eventHandler.GetMethodInfo();
            this.handler = Invoke;
            this.unregister = unregister;
        }

        /// <summary>
        /// Creates a weak event handler from the provided delegate.
        /// </summary>
        /// <param name="delegate">Delegate to be used for creating a new weak event handler.</param>
        /// <param name="unregister">Callback to unregister the event handler.</param>
        /// <returns>A weak event handler for the provided delegate.</returns>
        public static WeakEventHandler<T> MakeFromDelegate(Delegate @delegate, UnregisterCallback<T> unregister)
        {
            WeakEventHandler<T> weakEventHandler = new WeakEventHandler<T>();

            weakEventHandler.target = new WeakReference(@delegate.Target);
            weakEventHandler.method = @delegate.GetMethodInfo();
            weakEventHandler.unregister = unregister;

            return weakEventHandler;
        }

        /// <summary>
        /// Invokes the current target by the given arguments.
        /// </summary>
        /// <param name="sender">Sender that is invoking this event handler.</param>
        /// <param name="arguments">Argumetns for invoking the event handler.</param>
        public void Invoke(object sender, T arguments)
        {
            object target = this.target.Target;

            if (target != null)
            {
                this.method.Invoke(target, new object[] { sender, arguments });
            }
            else if (this.unregister != null)
            {
                this.unregister(this.handler);
                this.unregister = null;
            } 
        }

        /// <summary>
        /// Checks if the delegate has the same target.
        /// </summary>
        /// <param name="delegate">Delegate to compare to the current weak event handler.</param>
        /// <returns>True if the share the same target; otherwise false.</returns>
        public bool IsSameTarget(Delegate @delegate)
        {
            return this.method == @delegate.GetMethodInfo();
        }

        /// <summary>
        /// Operator for the weak event handler.
        /// </summary>
        /// <param name="eventHandler">Weak event handler.</param>
        /// <returns>Operator for the weak event handler.</returns>
        public static implicit operator EventHandler<T>(WeakEventHandler<T> eventHandler)
        {
            return eventHandler.handler;
        }
    }
}
