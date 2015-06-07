using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LightHouse.Core.Notifications;

namespace LightHouse.Core
{
    /// <summary>
    /// Functionality to be implemented by all SurrogateObjects.
    /// </summary>
    public interface ISurrogateObject : IObject, INotifyPropertyChanged, INotifyPropertyChanging
    {
        /// <summary>
        /// Information about the ContractType or DataType that represents the SurrogageObject.
        /// </summary>
        LightHouse.Core.Elite.Locating.TypeInfo TypeInfo { get; set; }
    }
}
