using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Xunit;

using LightHouse.Core.Collections;
using LightHouse.Localization;
using LightHouse.Ontology.Social.Organization;
using LightHouse.Ontology.Social.Demography;
using LightHouse.Ontology.Social.Organization.Projects;
using LightHouse.Core.Caching;

namespace LightHouse.Core.Testing
{
    public class DataObject
    {
        public DataObject()
        {
            
        }

        /// <summary>
        /// The time for creating 10000 persons shouldn't be more than 0.05 second.
        /// </summary>
        public void Create10000DynamicPerson()
        {
            DateTime startTime = DateTime.Now;

            IList<IDataObject> people = new List<IDataObject>();

            for (int i = 0; i < 10000; i++)
            {
                IDataObject person = new LightHouse.Core.DataObject()
                {                    
                    DynamicType = typeof(Person).ToString()
                };

                people.Add(person);
            }

            TimeSpan timeDifference = DateTime.Now.Subtract(startTime);

            Assert.True(timeDifference.TotalSeconds < 0.05);
            Assert.Equal(people.Count, 10000);
        }

        /// <summary>
        /// This test attemps to assign a value to the property which type is standard .net types.
        /// </summary>
        public void SetPropertyStandardNetTypes()
        {
            Person person = new Person();
            DateTime now = DateTime.Now;

            LightHouse.Storage.EntityObject entityObject = (LightHouse.Storage.EntityObject)person.ConvertTo<IDataObject>();

            entityObject.Reference = "Reference1";
            entityObject.CreatedOn = now;

            DataCache dynamicCache = (DataCache)entityObject.GetType().GetRuntimeFields().Where(x => x.Name == "dynamicCache").First().GetValue(entityObject);
            dynamicCache.Add("Birthday", now, "Properties");

            Assert.NotNull(person.Reference);
            Assert.IsType(typeof(String), person.Reference);
            Assert.True(person.Reference == "Reference1");

            Assert.NotNull(person.CreatedOn);
            Assert.IsType(typeof(DateTime), person.CreatedOn);
            Assert.True(person.CreatedOn == now);

            Assert.NotNull(person.Birthday);
            Assert.IsType(typeof(DateTime), person.Birthday);
            Assert.True(person.Birthday == now);
        }

        /// <summary>
        /// This test attemps to assign a value to the property which type is subclass of DataObject.
        /// </summary>
        public void SetPropertyOfDataObject()
        {
            Project project = new Project();
            IDataObject dataObject = project.ConvertTo<IDataObject>();

            Person projectLeader = new Person();
            projectLeader.Reference = "Person1";

            DateTime now = DateTime.Now;

            DataCache dynamicCache = (DataCache)dataObject.GetType().GetRuntimeFields().Where(x => x.Name == "dynamicCache").First().GetValue(dataObject);
            dynamicCache.Add("Leader", projectLeader.ConvertTo<IDataObject>(), "Properties");

            Assert.NotNull(project.Leader);
            Assert.True(project.Leader.Reference == "Person1");
        }


        /// <summary>
        /// This test attemps to assign a value to the property which type is subclass of DataObject.
        /// </summary>
        public void SetPropertyOfLocalString()
        {
            Project project = new Project();
            IDataObject dataObject = project.ConvertTo<IDataObject>();

            LocalString name = new LocalString("LightHouse");

            DataCache dynamicCache = (DataCache)dataObject.GetType().GetRuntimeFields().Where(x => x.Name == "dynamicCache").First().GetValue(dataObject);
            dynamicCache.Add("Name", name.ConvertTo<IDataObject>(), "Properties");

            Assert.NotNull(project.Name);
            Assert.True(project.Name.GetContent() == "LightHouse");
        }

        /// <summary>
        /// This test attemps to assign a value to the property which type is subclass of DataList.
        /// </summary>
        public void SetPropertyOfDataList()
        {
            Project project = new Project();
            IDataObject dataObject = project.ConvertTo<IDataObject>();

            IContractList<Activity> activities = new ContractList<Activity>()
            {
                new Activity() { Reference = "Activity1" }, 
                new Activity() { Reference = "Activity2" }
            };

            DataCache dynamicCache = (DataCache)dataObject.GetType().GetRuntimeFields().Where(x => x.Name == "dynamicCache").First().GetValue(dataObject);
            dynamicCache.Add("DefaultActivities", activities.ToDataList(), "Properties");

            Assert.NotNull(project.DefaultActivities);
            Assert.True(project.DefaultActivities[0].Reference == "Activity1");
            Assert.True(project.DefaultActivities[1].Reference == "Activity2");
        }

        /// <summary>
        /// This test attemps to assign a value to the property which type is standard .net types by SetProperty.
        /// </summary>
        public void SetPropertyMethodStandardNetTypes()
        {
            Person person = new Person();

            IDataObject dataObject = person.ConvertTo<IDataObject>();

            dataObject.SetProperty("Reference", "Reference1");

            Assert.NotNull(person.Reference);
            Assert.IsType(typeof(String), person.Reference);
            Assert.True(person.Reference == "Reference1");

            DateTime now = DateTime.Now;

            dataObject.SetProperty("CreatedOn", now);

            Assert.NotNull(person.CreatedOn);
            Assert.IsType(typeof(DateTime), person.CreatedOn);
            Assert.True(person.CreatedOn == now);
        }

        /// <summary>
        /// This test attemps to assign a value to the property which type is LocalString by SetProperty.
        /// </summary>
        public void SetPropertyMethodOfLocalString()
        {
            Project project = new Project();
            IDataObject dataObject = project.ConvertTo<IDataObject>();

            LocalString name = new LocalString("LightHouse");

            dataObject.SetProperty("Name", name.ConvertTo<IDataObject>());

            Assert.NotNull(project.Name);
            Assert.True(project.Name.GetContent() == "LightHouse");
        }

        /// <summary>
        /// This test attemps to assign a value to the property which type is subclass of DataObject by SetProperty.
        /// </summary>
        public void SetPropertyMethodOfDataObject()
        {
            Project project = new Project();
            IDataObject dataObject = project.ConvertTo<IDataObject>();

            Person projectLeader = new Person();
            projectLeader.Reference = "Person1";

            DateTime now = DateTime.Now;

            dataObject.SetProperty("Leader", projectLeader.ConvertTo<IDataObject>());

            Assert.NotNull(project.Leader);
            Assert.True(project.Leader.Reference == "Person1");
        }

        /// <summary>
        /// This test attemps to assign a value to the property which type is subclass of DataList by SetProperty.
        /// </summary>
        public void SetPropertyMethodOfDataList() 
        {
            Project project = new Project();
            IDataObject dataObject = project.ConvertTo<IDataObject>();

            IContractList<Activity> activities = new ContractList<Activity>()
            {
                new Activity() { Reference = "Activity1" }, 
                new Activity() { Reference = "Activity2" }
            };

            dataObject.SetProperty("DefaultActivities", activities.ToDataList());

            Assert.NotNull(project.DefaultActivities);
            Assert.True(project.DefaultActivities[0].Reference == "Activity1");
            Assert.True(project.DefaultActivities[1].Reference == "Activity2");
        }

        /// <summary>
        /// This test attemps to get a value to the property which type is LocalString.
        /// </summary>
        public void GetPropertyOfLocalString()
        {
            Project project = new Project();
            IDataObject dataObject = project.ConvertTo<IDataObject>();

            project.Name = new LocalString("LightHouse");

            DataCache dynamicCache = (DataCache)dataObject.GetType().GetRuntimeFields().Where(x => x.Name == "dynamicCache").First().GetValue(dataObject);
            IDataObject name = (IDataObject)dynamicCache.Get("Name", "Properties");

            Assert.NotNull(name);
            Assert.True(((IDataObject)name.GetProperty<IDataList>("Contents")[0]).GetProperty<String>("Content") == "LightHouse");
        }

        /// <summary>
        /// This test attemps to get a value to the property which type is standard .net types.
        /// </summary>
        public void GetPropertyStandardNetTypes()
        {
            Person person = new Person();
            LightHouse.Storage.EntityObject dataObject = (LightHouse.Storage.EntityObject)person.ConvertTo<IDataObject>();

            DateTime now = DateTime.Now;
            person.Reference = "Reference1";
            person.CreatedOn = now;
            person.Birthday = now;

            DataCache dynamicCache = (DataCache)dataObject.GetType().GetRuntimeFields().Where(x => x.Name == "dynamicCache").First().GetValue(dataObject);
            DateTime birthday = (DateTime)dynamicCache.Get("Birthday", "Properties");
         
            Assert.NotNull(dataObject.Reference);
            Assert.True(dataObject.Reference == "Reference1");

            Assert.NotNull(dataObject.CreatedOn);
            Assert.True(dataObject.CreatedOn == now);

            Assert.NotNull(birthday);
            Assert.True(birthday == now);
        }

        /// <summary>
        /// This test attemps to get a value to the property which type is subclass of DataObject.
        /// </summary>
        public void GetPropertyOfDataObject()
        {
            Project project = new Project();
            IDataObject dataObject = project.ConvertTo<IDataObject>();

            Person person = new Person();
            person.Reference = "Person1";

            project.Leader = person;

            DataCache dynamicCache = (DataCache)dataObject.GetType().GetRuntimeFields().Where(x => x.Name == "dynamicCache").First().GetValue(dataObject);
            IDataObject leaderDataObject = (IDataObject)dynamicCache.Get("Leader", "Properties");

            Assert.NotNull(leaderDataObject);
            Assert.True(leaderDataObject.GetProperty<String>("Reference") == "Person1");
        }

        /// <summary>
        /// This test attemps to get a value to the property which type is subclass of DataList.
        /// </summary>
        public void GetPropertyOfDataList()
        {
            Project project = new Project();
            IDataObject dataObject = project.ConvertTo<IDataObject>();

            IContractList<Activity> activities = new ContractList<Activity>()
            {
                new Activity() { Reference = "Activity1" }, 
                new Activity() { Reference = "Activity2" }
            };

            project.DefaultActivities = activities;

            DataCache dynamicCache = (DataCache)dataObject.GetType().GetRuntimeFields().Where(x => x.Name == "dynamicCache").First().GetValue(dataObject);
            IDataList activitiesDataList = (IDataList)dynamicCache.Get("DefaultActivities", "Properties");

            Assert.NotNull(activitiesDataList);
            Assert.True(((IDataObject)activitiesDataList[0]).GetProperty<String>("Reference") == "Activity1");
            Assert.True(((IDataObject)activitiesDataList[1]).GetProperty<String>("Reference") == "Activity2");
        }

        /// <summary>
        /// This test attemps to get a value to the property which type is LocalString by GetProperty.
        /// </summary>
        public void GetPropertyMethodOfLocalString()
        {
            Project project = new Project();
            IDataObject dataObject = project.ConvertTo<IDataObject>();

            project.Name = new LocalString("LightHouse");

            Assert.NotNull(dataObject.GetProperty<IDataObject>("Name"));
            Assert.True(((IDataObject)dataObject.GetProperty<IDataObject>("Name").GetProperty<IDataList>("Contents")[0]).GetProperty<String>("Content") == "LightHouse");
        }

        /// <summary>
        /// This test attemps to get a value to the property which type is standard .net types by GetProperty.
        /// </summary>
        public void GetPropertyMethodStandardNetTypes()
        {
            Person person = new Person();
            IDataObject dataObject = person.ConvertTo<IDataObject>();

            person.Reference = "Reference1";

            Assert.NotNull(dataObject.GetProperty<String>("Reference"));
            Assert.True(dataObject.GetProperty<String>("Reference") == "Reference1");

            DateTime now = DateTime.Now;

            person.CreatedOn = now;

            Assert.NotNull(dataObject.GetProperty<DateTime>("CreatedOn"));
            Assert.True(dataObject.GetProperty<DateTime>("CreatedOn") == now);
        }

        /// <summary>
        /// This test attemps to get a value to the property which type is subclass of DataObject by GetProperty.
        /// </summary>
        public void GetPropertyMethodOfDataObject()
        {
            Project project = new Project();
            IDataObject dataObject = project.ConvertTo<IDataObject>();

            Person person = new Person();
            person.Reference = "Person1";

            project.Leader = person;

            Assert.NotNull(dataObject.GetProperty<LightHouse.Core.DataObject>("Leader"));
            Assert.True(dataObject.GetProperty<LightHouse.Core.DataObject>("Leader").GetProperty<String>("Reference") == "Person1");
        }

        /// <summary>
        /// This test attemps to get a value to the property which type is subclass of DataList by GetProperty.
        /// </summary>
        public void GetPropertyMethodOfDataList()
        {
            Project project = new Project();
            IDataObject dataObject = project.ConvertTo<IDataObject>();

            IContractList<Activity> activities = new ContractList<Activity>()
            {
                new Activity() { Reference = "Activity1" }, 
                new Activity() { Reference = "Activity2" }
            };

            project.DefaultActivities = activities;

            Assert.NotNull(dataObject.GetProperty<IDataList>("DefaultActivities"));
            Assert.True(((LightHouse.Core.DataObject)dataObject.GetProperty<IDataList>("DefaultActivities")[0]).GetProperty<String>("Reference") == "Activity1");
            Assert.True(((LightHouse.Core.DataObject)dataObject.GetProperty<IDataList>("DefaultActivities")[1]).GetProperty<String>("Reference") == "Activity2");
        }

        /// <summary>
        /// This test attemps to assign a value to the property which type is standard .net types by using SetContractProperty.
        /// </summary>
        public void SetContractPropertyMethodStandardNetTypes()
        {
            Person person = new Person();

            IDataObject dataObject = person.ConvertTo<IDataObject>();

            dataObject.SetContractProperty(String.Format("{0}.Reference", typeof(EntityObject).FullName), "Reference1");

            Assert.NotNull(person.Reference);
            Assert.IsType(typeof(String), person.Reference);
            Assert.True(person.Reference == "Reference1");

            DateTime now = DateTime.Now;

            dataObject.SetContractProperty(String.Format("{0}.CreatedOn", typeof(ElementObject).FullName), now);

            Assert.NotNull(person.CreatedOn);
            Assert.IsType(typeof(DateTime), person.CreatedOn);
            Assert.True(person.CreatedOn == now);
        }

        /// <summary>
        /// This test attemps to assign a value to the property which type is LocalString by SetContractProperty.
        /// </summary>
        public void SetContractPropertyMethodOfLocalString()
        {
            Project project = new Project();
            IDataObject dataObject = project.ConvertTo<IDataObject>();

            LocalString name = new LocalString("LightHouse");

            dataObject.SetContractProperty(String.Format("{0}.Name", typeof(Project).FullName), name.ConvertTo<IDataObject>());

            Assert.NotNull(project.Name);
            Assert.True(project.Name.GetContent() == "LightHouse");
        }

        /// <summary>
        /// This test attemps to assign a value to the property which type is subclass of DataObject by SetContractProperty.
        /// </summary>
        public void SetContractPropertyMethodOfDataObject()
        {
            Project project = new Project();
            IDataObject dataObject = project.ConvertTo<IDataObject>();

            Person projectLeader = new Person();
            projectLeader.Reference = "Person1";

            DateTime now = DateTime.Now;

            dataObject.SetContractProperty(String.Format("{0}.Leader", typeof(Project).FullName), projectLeader.ConvertTo<IDataObject>());

            Assert.NotNull(project.Leader);
            Assert.True(project.Leader.Reference == "Person1");
        }

        /// <summary>
        /// This test attemps to assign a value to the property which type is subclass of DataList by SetContractProperty.
        /// </summary>
        public void SetContractPropertyMethodOfDataList()
        {
            Project project = new Project();
            IDataObject dataObject = project.ConvertTo<IDataObject>();

            IContractList<Activity> activities = new ContractList<Activity>()
            {
                new Activity() { Reference = "Activity1" }, 
                new Activity() { Reference = "Activity2" }
            };

            dataObject.SetContractProperty(String.Format("{0}.DefaultActivities", typeof(Project).FullName), activities.ToDataList());

            Assert.NotNull(project.DefaultActivities);
            Assert.True(project.DefaultActivities[0].Reference == "Activity1");
            Assert.True(project.DefaultActivities[1].Reference == "Activity2");
        }

        /// <summary>
        /// This test attemps to get a value to the property which type is LocalString by GetContractProperty.
        /// </summary>
        public void GetContractPropertyMethodOfLocalString()
        {
            Project project = new Project();
            IDataObject dataObject = project.ConvertTo<IDataObject>();

            project.Name = new LocalString("LightHouse");

            Assert.NotNull(dataObject.GetContractProperty<IDataObject>(String.Format("{0}.Name", typeof(Project).FullName)));
            Assert.True(dataObject.GetContractProperty<LocalString>(String.Format("{0}.Name", typeof(Project).FullName)).GetContent() == "LightHouse");
        }

        /// <summary>
        /// This test attemps to get a value to the property which type is standard .net types by GetContractProperty.
        /// </summary>
        public void GetContractPropertyMethodStandardNetTypes()
        {
            Person person = new Person();
            IDataObject dataObject = person.ConvertTo<IDataObject>();

            person.Reference = "Reference1";

            Assert.NotNull(dataObject.GetContractProperty<String>(String.Format("{0}.Reference", typeof(EntityObject).FullName)));
            Assert.True(dataObject.GetContractProperty<String>(String.Format("{0}.Reference", typeof(EntityObject).FullName)) == "Reference1");

            DateTime now = DateTime.Now;

            person.CreatedOn = now;

            Assert.NotNull(dataObject.GetContractProperty<DateTime>(String.Format("{0}.CreatedOn", typeof(ElementObject).FullName)));
            Assert.True(dataObject.GetContractProperty<DateTime>(String.Format("{0}.CreatedOn", typeof(ElementObject).FullName)) == now);
        }

        /// <summary>
        /// This test attemps to get a value to the property which type is subclass of DataObject by GetContractProperty.
        /// </summary>
        public void GetContractPropertyMethodOfDataObject()
        {
            Project project = new Project();
            IDataObject dataObject = project.ConvertTo<IDataObject>();

            Person person = new Person();
            person.Reference = "Person1";

            project.Leader = person;

            Assert.NotNull(dataObject.GetContractProperty<IDataObject>(String.Format("{0}.Leader", typeof(Project).FullName)));
            Assert.True(dataObject.GetContractProperty<IDataObject>(String.Format("{0}.Leader", typeof(Project).FullName)).GetContractProperty<String>(String.Format("{0}.Reference", typeof(EntityObject).FullName)) == "Person1");
        }

        /// <summary>
        /// This test attemps to get a value to the property which type is subclass of DataList by GetContractProperty.
        /// </summary>
        public void GetContractPropertyMethodOfDataList()
        {
            Project project = new Project();
            IDataObject dataObject = project.ConvertTo<IDataObject>();

            IContractList<Activity> activities = new ContractList<Activity>()
            {
                new Activity() { Reference = "Activity1" }, 
                new Activity() { Reference = "Activity2" }
            };

            project.DefaultActivities = activities;

            Assert.NotNull(dataObject.GetContractProperty<IContractList<Activity>>(String.Format("{0}.DefaultActivities", typeof(Project).FullName)));
            Assert.True(dataObject.GetContractProperty<IContractList<Activity>>(String.Format("{0}.DefaultActivities", typeof(Project).FullName))[0].Reference == "Activity1");
            Assert.True(dataObject.GetContractProperty<IContractList<Activity>>(String.Format("{0}.DefaultActivities", typeof(Project).FullName))[1].Reference == "Activity2");
        }
    }
}
