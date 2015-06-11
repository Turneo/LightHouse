using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LightHouse.Core.Queries
{
    /// <summary>
    /// Functionality that needs to be implemented by all query objects.
    /// </summary>
    public interface IQuery
    {
        /// <summary>
        /// Query convertor, for casting the query object to another type.
        /// </summary>
        /// <typeparam name="U">New type of the query.</typeparam>
        /// <returns>Modified query object.</returns>
        IQuery<U> As<U>();        
    }
}
