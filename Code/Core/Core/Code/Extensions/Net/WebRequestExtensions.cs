using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Extensions.Net
{
    /// <summary>
    /// Extensions for WebRequest (System.Net.WebRequest).
    /// </summary>
    public static class WebRequestExtensions
    {
        /// <summary>
        /// Returns a response to a web request as an asynchronous operation. Portable implementation of GetResponseAsync. Currently not available in Profile78.
        /// </summary>
        /// <param name="source">WebRequest on which the extensions is based.</param>
        /// <returns>Task containing the WebResponse as a result.</returns>
        static public Task<WebResponse> GetResponseAsync(this WebRequest source)
        {
            return Task.Factory.FromAsync<WebResponse>(source.BeginGetResponse, source.EndGetResponse, null);
        }

        /// <summary>
        /// Returns a request stream for a web request as an asynchronous operation. 
        /// </summary>
        /// <param name="source">WebRequest on which the extensions is based.</param>
        /// <returns>Task containing the RequestStream as a result.</returns>
        static public Task<Stream> GetRequestStreamAsync(this WebRequest source)
        {
            return Task.Factory.FromAsync<Stream>(source.BeginGetRequestStream, source.EndGetRequestStream, null);
        }
    }
}
