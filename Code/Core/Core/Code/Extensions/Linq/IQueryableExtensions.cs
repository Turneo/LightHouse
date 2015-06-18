using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Extensions.Linq
{
    /// <summary>
    /// Extensions for IQueryable (System.Linq.IQueryable).
    /// </summary>
    public static class IQueryableExtensions
    {
        /// <summary>
        /// Holds the MethodInfo for the As<U> method.
        /// </summary>
        private static MethodInfo asMethod;

        /// <summary>
        /// Gets the MethodInfo for the As<U> method.
        /// </summary>
        /// <returns></returns>
        private static MethodInfo GetAsMethod()
        {
            if(asMethod == null)
            {
                asMethod = typeof(IQueryableExtensions).GetRuntimeMethods().Where(x => x.Name == "As").FirstOrDefault();
            }

            return asMethod;
        }

        /// <summary>
        /// Casts the query type to a another type.
        /// </summary>
        /// <typeparam name="U">New type of the query.</typeparam>
        /// <param name="source">IQueryable on which the extensions is based.</param>
        /// <returns>Modified query object.</returns>
        public static IQueryable<U> As<U>(this IQueryable source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return source.Provider.CreateQuery<U>(
                Expression.Call(
                    null,
                    GetAsMethod().MakeGenericMethod(typeof(U)),
                    new Expression[] { source.Expression }
                    ));            
        }
    }
}
