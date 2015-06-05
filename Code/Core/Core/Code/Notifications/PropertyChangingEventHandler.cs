using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Notifications
{
    /// <summary>
    /// Represents the method that will handle the System.ComponentModel.INotifyPropertyChanging.PropertyChanging
    /// event of an System.ComponentModel.INotifyPropertyChanging interface.
    /// </summary>
    /// <param name="sender">Sender of the event handler.</param>
    /// <param name="e">Arguments of the event handler.</param>
    public delegate void PropertyChangingEventHandler(object sender, PropertyChangingEventArgs e);
}
