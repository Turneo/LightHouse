using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Notifications
{
    /// <summary>
    /// Event arguments for the property update.
    /// </summary>
    public class UpdateObjectPropertyEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of UpdateObjectPropertyEventArgs.
        /// </summary>
        /// <param name="source">Source of the event arguments.</param>
        /// <param name="property">Property that is being updated.</param>
        public UpdateObjectPropertyEventArgs(object source, string property)
        {
            this.source = source;
            this.property = property;
        }

        /// <summary>
        /// Source of the event arguments.
        /// </summary>
        private object source;
        public object Source
        {
            get
            {
                return source;
            }
        }

        /// <summary>
        /// Property that is being updated.
        /// </summary>
        private string property;
        public string Property
        {
            get
            {
                return property;
            }
        }

    }
}
