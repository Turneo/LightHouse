using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LightHouse.Core.Bindings;
using LightHouse.Core.Queries;

namespace LightHouse.Core.Collections
{
    /// <summary>
    /// A  collection of SurrogateObjects.
    /// </summary>
    /// <typeparam name="T">Type of the ContractObjects included in the collection.</typeparam>
    public class SurrogateList<T> : QueryableList<T>, ISurrogateList<T>
    {
        /// <summary>
        /// Query that is used for executing the SurrogateList as queryable.
        /// </summary>
        protected IQuery query;

        /// <summary>
        /// ObjectPath that the SurrogateList is bound to.
        /// </summary>
        protected ObjectPath objectPath;

        /// <summary>
        /// Internal list containing the SurrogateObjects.
        /// </summary>
        private IList<T> dataList = new List<T>();

        /// <summary>
        /// Initializes a new instance of a SurrogateList.
        /// </summary>
        public SurrogateList()
        {
        }

        /// <summary>
        /// Initializes a new instance of a SurrogateList based on the provided query.
        /// </summary>
        /// <param name="query">IQuery with the required querying information.</param>
        public SurrogateList(IQuery query)
        {
            this.query = query;            
        }

        /// <summary>
        /// Initializes a new instance of a SurrogateList based on the provided ObjectPath.
        /// </summary>
        /// <param name="objectPath">ObjectPath with the required binding information.</param>
        public SurrogateList(ObjectPath objectPath)            
        {
            this.objectPath = objectPath;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection. 
        /// </summary>
        /// <returns>A enumerator that can be used to iterate through the collection.</returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            if (this.query != null)
            {
                return GetQueryable().GetEnumerator();
            }

            return dataList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection. 
        /// </summary>
        /// <returns>A enumerator that can be used to iterate through the collection.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            if (this.query != null)
            {
                return GetQueryable().GetEnumerator();
            }

            return dataList.GetEnumerator();
        }

        /// <summary>
        /// Gets the type of the element(s) that are returned when the expression tree associated the queryable of this SurrogateList.
        /// </summary>
        public override Type ElementType
        {
            get
            {
                return GetQueryable().ElementType;
            }
        }

        /// <summary>
        /// Gets the expression tree that is associated with the instance of the queryable of this SurrogateList.
        /// </summary>
        public override System.Linq.Expressions.Expression Expression
        {
            get
            {
                return GetQueryable().Expression;
            }
        }

        /// <summary>
        /// Gets the query provider that is associated with this data source.
        /// </summary>
        public override IQueryProvider Provider
        {
            get 
            {
                return GetQueryable().Provider;
            }
        }

        /// <summary>
        /// Returns the Queryable for the current collection.
        /// </summary>
        /// <returns>Queryable for the current collection.</returns>
        private IQueryable<T> GetQueryable()
        {
            if (this.query != null)
            {
                return (IQueryable<T>)this.query;
            }

            IList<T> list = new List<T>();

            if ((dataList != null) && (dataList.Count > 0))
            {
                foreach (IDataObject item in dataList)
                {
                    list.Add(item.As<T>());
                }
            }

            return list.AsQueryable<T>();
        }
    }
}
