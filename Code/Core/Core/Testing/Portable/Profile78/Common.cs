using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

using LightHouse.Core.Notifications;

namespace LightHouse.Core.Testing
{
    public class Common
    {
        /// <summary>
        /// Checks if a PropertyChangingEventArgs can be created correctly.
        /// </summary>
        public void CreatePropertyChangingEventArgs()
        {
            PropertyChangingEventArgs propertyChangingEventArgs = new PropertyChangingEventArgs("Property", 1, 7);

            Assert.True(propertyChangingEventArgs.PropertyName == "Property");
            Assert.True(((Int32)propertyChangingEventArgs.OldValue) == 7);
            Assert.True(((Int32)propertyChangingEventArgs.NewValue) == 1);
        }
    }
}
