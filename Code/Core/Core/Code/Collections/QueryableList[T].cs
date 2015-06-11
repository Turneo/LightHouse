using LightHouse.Core.Bindings;
using LightHouse.Core.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LightHouse.Core.Collections
{
    /// <summary>
    /// Provides the base for a QueryableList wherein the type of the data is known.
    /// </summary>
    /// <typeparam name="T">The type of the QueryableList.</typeparam>
    public abstract class QueryableList<T> : IQueryableList<T>
    {
        /// <summary>
        /// Paths to be included in the returned objects of the QueryableList.
        /// </summary>
        protected IList<String> paths = new List<String>();

        /// <summary>
        /// DynamicType of the QueryableList. To be used in the case the objects are dynamic and not yet generated.
        /// </summary>
        protected String dynamicType = "";

        /// <summary>
        /// Initializes a new instance of a QueryableList.
        /// </summary>
        public QueryableList() { }        

        /// <summary>
        /// Gets the type of the element(s) that are returned when the expression tree associated with this instance of the queryable list is executed.
        /// </summary>
        public abstract Type ElementType { get; }

        /// <summary>
        /// Gets the expression tree that is associated with the instance of the queryable list.
        /// </summary>
        public abstract System.Linq.Expressions.Expression Expression { get; }

        /// <summary>
        /// Gets the query provider that is associated with this data source.
        /// </summary>
        public abstract IQueryProvider Provider { get; }

        /// <summary>
        /// Returns an enumerator that iterates through the collection. 
        /// </summary>
        /// <returns>A enumerator that can be used to iterate through the collection.</returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection. 
        /// </summary>
        /// <returns>A enumerator that can be used to iterate through the collection.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the dynamic type of the QueryableList and returns itself to support fluent calls.
        /// </summary>
        /// <param name="dynamicType">Dynamic type to be used by the QueryableList.</param>
        /// <returns>The current QueryableList to support fluent calls.</returns>
        public IQueryableList<T> OfDynamicType(String dynamicType)
        {
            this.dynamicType = dynamicType;

            return this;
        }

        /// <summary>
        /// Adds a path to be included in the query and returns itself to support fluent calls. The included path is returned for each response object and as such isn't proxied.
        /// </summary>
        /// <param name="path">Path to be included in the query.</param>
        /// <returns>The current QueryableList to support fluent calls.</returns>
        public IQueryableList<T> Include(String path)
        {
            if (!this.paths.Contains(path))
            {
                this.paths.Add(path);
            }

            return this;
        }

        /// <summary>
        /// Adds a collection of paths to be included in the query and returns itself to support fluent calls. The included paths are returned for each response object and as such aren't proxied.
        /// </summary>
        /// <param name="paths">Paths to be included in the query.</param>
        /// <returns>The current QueryableList to support fluent calls.</returns>
        public IQueryableList<T> Include(ICollection<String> paths)
        {
            this.paths = this.paths.Union(paths).ToList<String>();

            return this;
        }
    }
}
