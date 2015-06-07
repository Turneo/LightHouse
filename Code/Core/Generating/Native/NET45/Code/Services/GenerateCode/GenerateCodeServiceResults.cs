using LightHouse.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Generating.Native.Services.GenerateCode
{
    /// <summary>
    /// Results for the GenerateCode service.
    /// </summary>
    public class GenerateCodeServiceResults : ServiceResults
    {
        /// <summary>
        /// Initializes a new instance of GenerateCodeServiceResults.
        /// </summary>
        public GenerateCodeServiceResults() : base() { }

        /// <summary>
        /// Initializes a new instance of GenerateCodeServiceResults using the provided proxy configuration.
        /// </summary>
        /// <param name="isProxy">Proxy configuration to be used in the initialization.</param>
        public GenerateCodeServiceResults(Boolean isProxy) : base(isProxy) { }

        /// <summary>
        /// Initializes a new instance of GenerateCodeServiceResults using the provided DataObject.
        /// </summary>
        /// <param name="dataObject">DataObject to be used in the initialization.</param>
        public GenerateCodeServiceResults(DataObject dataObject) : base(dataObject) { }

        /// <summary>
        /// Initializes a new instance of GenerateCodeServiceResults using the provided ContractObject.
        /// </summary>
        /// <param name="contractObject">ContractObject to be used in the initialization.</param>
        public GenerateCodeServiceResults(ContractObject contractObject) : base(contractObject) { }

        /// <summary>
        /// Generated code as string.
        /// </summary>
        public virtual String GeneratedCode
        {
            get { return GetContractProperty<String>(String.Format("{0}.{1}", typeof(GenerateCodeServiceResults).FullName, "GeneratedCode")); }
            set { SetContractProperty(String.Format("{0}.{1}", typeof(GenerateCodeServiceResults).FullName, "GeneratedCode"), value); }
        }
    }
}
