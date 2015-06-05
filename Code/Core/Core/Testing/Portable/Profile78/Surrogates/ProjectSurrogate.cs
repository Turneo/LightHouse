using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LightHouse.Core.Attributes;
using LightHouse.Localization;
using LightHouse.Ontology.Social.Organization.Projects;

namespace LightHouse.Core.Testing.Surrogates
{
    [Contract(typeof(Project))]
    public class ProjectSurrogate : SurrogateObject
    {
        [Path("Reference")]
        public String Reference { get; set; }

        [Path("Name")]
        public LocalString Name { get; set; }

        [Path("Leader.FirstName")]
        public LocalString LeaderFirstName { get; set; }

        [Path("Leader.LastName")]
        public LocalString LeaderLastName { get; set; }

        [Path("Leader.Birthday")]
        public DateTime Birthday { get; set; }

    }
}
