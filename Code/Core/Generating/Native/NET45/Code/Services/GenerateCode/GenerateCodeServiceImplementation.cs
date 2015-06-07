using LightHouse.Core;
using LightHouse.Core.Generating.Native.Services.Templates;
using LightHouse.Execution;
using LightHouse.Execution.Services.Attributes;
using LightHouse.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Generating.Native.Services.GenerateCode
{
    /// <summary>
    /// Implementation of the GenerateCode service.
    /// </summary>
	[LightHouse.Execution.Services.Attributes.ServiceImplementation]
    public static class GenerateCodeServiceImplementation
    {
        /// <summary>
        /// Static implementation of the GenerateCode service.
        /// </summary>
        /// <param name="service">GenerateCode service to be invoked.</param>
        /// <returns>GenerateCode service results.</returns>
        [Service(typeof(GenerateCodeService))]
        public static ServiceResults Invoke_GenerateCodeService(GenerateCodeService service)
        {
            return new GenerateCodeServiceResults()
            {
                GeneratedCode = GenerateCodeFromElement(service.Element)
            };
        }

        /// <summary>
        /// Generats code for the provided element.
        /// </summary>
        /// <param name="element">Element to be used for generating code.</param>
        /// <returns>Generated code as string.</returns>
        private static string GenerateCodeFromElement(Element element)
        {
            if (element.IsContract)
            {
                ContractObjectTemplate template = new ContractObjectTemplate();
                template.Session = new Dictionary<string, object>();
                template.Session.Add("Element", element);
                template.Initialize();
                return template.TransformText();
            }
            else
            {
                DataObjectTemplate template = new DataObjectTemplate();
                template.Session = new Dictionary<string, object>();
                template.Session.Add("Element", element);
                template.Initialize();
                return template.TransformText();
            }
        }
    }
}
