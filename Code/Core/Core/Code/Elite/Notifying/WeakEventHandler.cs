using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Elite.Notifying
{
    public class WeakEventHandler<E> where E : EventArgs
    {
        public delegate void UnregisterCallback(EventHandler<E> eventHandler);

        private WeakReference targetRef;
        private MethodInfo method;
        private EventHandler<E> handler;
        private UnregisterCallback unregister;

        public WeakEventHandler(EventHandler<E> eventHandler, UnregisterCallback unregister)
        {
            this.targetRef = new WeakReference(eventHandler.Target);
            this.method = eventHandler.GetMethodInfo();
            this.handler = Invoke;
            this.unregister = unregister;
        }

        public void Invoke(object sender, E e)
        {
            object target = this.targetRef.Target;

            if (target != null)
            {
                this.method.Invoke(target, new object[] { sender, e });
            }
            else if (this.unregister != null)
            {
                this.unregister(this.handler);
                this.unregister = null;
            } 
        }

        public static implicit operator EventHandler<E>(WeakEventHandler<E> eventHandler)
        {
            return eventHandler.handler;
        }
    }
}
