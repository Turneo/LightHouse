using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Notifications
{
    /// <summary>
    /// Represents the method that will handle the changes of properties.
    /// </summary>
    /// <param name="sender">Sender of the event handler.</param>
    /// <param name="args">Arguments of the event handler.</param>
    public delegate void UpdateObjectPropertyEventHandler(object sender, UpdateObjectPropertyEventArgs args);
}
