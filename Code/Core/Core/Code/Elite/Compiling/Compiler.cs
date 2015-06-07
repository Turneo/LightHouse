using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Elite.Compiling
{
    /// <summary>
    /// Officer responsible for compiling source code.
    /// </summary>
    public class Compiler
    {
        /// <summary>
        /// Invokes a method in the provided source code.
        /// </summary>
        /// <param name="source">Source code to be compiled.</param>
        /// <param name="type">Type of method to be invoked.</param>
        /// <param name="method">Method to be invoked.</param>
        /// <returns>Object returned from the invoked method.</returns>
        public Object InvokeMethodFromSource(String source, String type, String method)
        {
            return LightHouse.Bootstrap.Compiler.InvokeMethodFromSource(source, type, method);
        }
    }
}
