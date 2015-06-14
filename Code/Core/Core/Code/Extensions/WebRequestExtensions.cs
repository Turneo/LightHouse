using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Extensions
{
    /// <summary>
    /// Extensions for WebRequest (System.Net.WebRequest).
    /// </summary>
    public static class WebRequestExtensions
    {
        /// <summary>
        /// Returns a response to an Internet request as an asynchronous operation. Portable implementation of GetResponseAsync. Currently not available in Profile78.
        /// </summary>
        /// <param name="this">WebRequest on which the extensions is based.</param>
        /// <returns>Task containing the WebResponse as a result.</returns>
        static public Task<WebResponse> GetResponseAsync(this WebRequest @this)
        {
            return Task.Factory.FromAsync(
                (asyncCallback, state) => @this.BeginGetResponse(asyncCallback, state), 
                (asyncResult) => @this.EndGetResponse(asyncResult), null);
        }
    }
}
