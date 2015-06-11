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
    /// <typeparam name="T">Type of the query.</typeparam>
    public interface IQuery<T> : IQueryable<T>, IQuery
    {
        /// <summary>
        /// Add a DynamicType to the query.
        /// </summary>
        /// <param name="dynamicType">Dynamic type to be added.</param>
        /// <returns>Modified query object.</returns>
        IQuery<T> OfDynamicType(String dynamicType);

        /// <summary>
        /// Includes an additional path in the current query.
        /// </summary>
        /// <param name="path">Path to be included in the current query.</param>
        /// <returns>Modified query object.</returns>
        IQuery<T> Include(String path);

        /// <summary>
        /// Includes an additional collection of paths to the current query.
        /// </summary>
        /// <param name="paths">Paths to be included in the current query.</param>
        /// <returns>Modified query object.</returns>
        IQuery<T> Include(ICollection<String> paths);

        /// <summary>
        /// REVIEW: Converts the Query to an IQueryableList of the same type.
        /// </summary>
        /// <typeparam name="T">Type of the new IQueryableList. Must be the same as the IQuery</typeparam>
        /// <returns>IQueryableList of Type T</returns>
        LightHouse.Core.Collections.IQueryableList<T> ToQueryableList<T>();
    }
}
