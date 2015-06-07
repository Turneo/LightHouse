using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Collections
{
    /// <summary>
    /// A thread safe collection of SurrogateObjects.
    /// </summary>
    /// <typeparam name="T">Type of the SurrogateObjects included in the collection.</typeparam>
    public interface ISurrogateList<T> : IQueryableList<T>
    {
    }
}
