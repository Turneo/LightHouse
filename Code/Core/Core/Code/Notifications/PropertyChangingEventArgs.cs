using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Notifications
{
    /// <summary>
    /// Event arguments for the PropertyChangingEvent.
    /// </summary>
    public class PropertyChangingEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the PropertyChangedEventArgs class.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        public PropertyChangingEventArgs(string propertyName)
        {
            this.propertyName = propertyName;
        }

        /// <summary>
        /// Initializes a new instance of the PropertyChangedEventArgs class.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        /// <param name="newValue">The new value of the property that changed.</param>
        /// <param name="oldValue">The old value of the property that changed.</param>
        public PropertyChangingEventArgs(string propertyName, Object newValue, Object oldValue) : this(propertyName)
        {
            this.newValue = newValue;
            this.oldValue = oldValue;
        }

        private String propertyName;

        /// <summary>
        /// Gets the name of the property that changed.
        /// </summary>
        public virtual string PropertyName { get { return propertyName; } }

        private Object newValue;

        /// <summary>
        /// Gets the new value from the property that changed.
        /// </summary>

        public virtual Object NewValue { get { return newValue; } }

        private Object oldValue;

        /// <summary>
        /// Gets the old value from the property that changed.
        /// </summary>
        public virtual Object OldValue { get { return oldValue; } }
    }
}
