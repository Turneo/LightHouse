using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LightHouse.Core.Collections
{
    /// <summary>
    /// Represents a queryable list of objects.
    /// </summary>
    /// <typeparam name="T">Generic type of the queryable list.</typeparam>
    public interface IQueryableList<T> : IQueryable<T>
    {
        /// <summary>
        /// Sets the dynamic type of the QueryableList and returns itself to support fluent calls.
        /// </summary>
        /// <param name="dynamicType">Dynamic type to be used by the QueryableList.</param>
        /// <returns>The current QueryableList to support fluent calls.</returns>
        IQueryableList<T> OfDynamicType(String dynamicType);
        
        /// <summary>
        /// Adds a path to be included in the query and returns itself to support fluent calls. The included path is returned for each response object and as such isn't proxied.
        /// </summary>
        /// <param name="path">Path to be included in the query.</param>
        /// <returns>The current QueryableList to support fluent calls.</returns>
        IQueryableList<T> Include(String path);

        /// <summary>
        /// Adds a collection of paths to be included in the query and returns itself to support fluent calls. The included paths are returned for each response object and as such aren't proxied.
        /// </summary>
        /// <param name="paths">Paths to be included in the query.</param>
        /// <returns>The current QueryableList to support fluent calls.</returns>
        IQueryableList<T> Include(ICollection<String> paths);
    }
}
