using LightHouse.Core.Generating.Native.Services;
using LightHouse.Core.Generating.Native.Services.GenerateCode;
using LightHouse.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Generating.Native
{
    /// <summary>
    /// Officer responsible for generating code.
    /// </summary>
    public class Generator : LightHouse.Model.Elite.Generating.Generator
    {
        /// <summary>
        /// Initializes a new instance of Generator.
        /// </summary>
        public Generator() : base() { }

        /// <summary>
        /// Initializes a new instance of Generator using the provided proxy configuration.
        /// </summary>
        /// <param name="isProxy">Proxy configuration to be used in the initialization.</param>
        public Generator(Boolean isProxy) : base(isProxy) { }

        /// <summary>
        /// Initializes a new instance of Generator using the provided DataObject.
        /// </summary>
        /// <param name="dataObject">DataObject to be used in the initialization.</param>
        public Generator(DataObject dataObject) : base(dataObject) { }

        /// <summary>
        /// Initializes a new instance of Generator using the provided ContractObject.
        /// </summary>
        /// <param name="contractObject">ContractObject to be used in the initialization.</param>
        public Generator(ContractObject contractObject) : base(contractObject) { }

        /// <summary>
        /// Generats code for the provided element.
        /// </summary>
        /// <param name="element">Element to be used for generating code.</param>
        /// <returns>Generated code as string.</returns>
        public override string GenerateCode(Element element)
        {
            GenerateCodeService generateCodeService = new GenerateCodeService()
            {
                Element = element
            };

            GenerateCodeServiceResults serviceResults = generateCodeService.Invoke().As<GenerateCodeServiceResults>();

            return serviceResults.GeneratedCode;
        }

    }
}
