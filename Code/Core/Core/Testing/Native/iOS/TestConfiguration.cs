using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

using LightHouse.Execution.Provider.Embedded;
using LightHouse.Testing.Scenarios;
using System.Diagnostics;

namespace LightHouse.Core.Testing
{
    public static class TestConfiguration
    {
        private static Boolean initialized = false;
        private static Object locker = new Object();

        private static LightHouse.Core.Testing.DataObject dataObject;
        public static LightHouse.Core.Testing.DataObject DataObject
        {
            get
            {
                if (dataObject == null)
                {
                    dataObject = new LightHouse.Core.Testing.DataObject();
                }

                return dataObject;
            }
        }

        private static LightHouse.Core.Testing.ContractObject contractObject;
        public static LightHouse.Core.Testing.ContractObject ContractObject
        {
            get
            {
                if (contractObject == null)
                {
                    contractObject = new LightHouse.Core.Testing.ContractObject();
                }

                return contractObject;
            }
        }

        private static LightHouse.Core.Caching.Testing.DataCache dataCache;
        public static LightHouse.Core.Caching.Testing.DataCache DataCache
        {
            get
            {
                if (dataCache == null)
                {
                    dataCache = new LightHouse.Core.Caching.Testing.DataCache();
                }

                return dataCache;
            }
        }

        private static LightHouse.Core.Elite.Reflecting.Testing.Reflector reflector;
        public static LightHouse.Core.Elite.Reflecting.Testing.Reflector Reflector
        {
            get
            {
                if (reflector == null)
                {
                    reflector = new LightHouse.Core.Elite.Reflecting.Testing.Reflector();
                }

                return reflector;
            }
        }

        private static LightHouse.Core.Elite.Locating.Testing.Locator locator;
        public static LightHouse.Core.Elite.Locating.Testing.Locator Locator
        {
            get
            {
                if (locator == null)
                {
                    locator = new LightHouse.Core.Elite.Locating.Testing.Locator();
                }

                return locator;
            }
        }

        private static LightHouse.Core.Elite.Merging.Testing.Merger merger;
        public static LightHouse.Core.Elite.Merging.Testing.Merger Merger
        {
            get
            {
                if (merger == null)
                {
                    merger = new LightHouse.Core.Elite.Merging.Testing.Merger();
                }

                return merger;
            }
        }

        private static LightHouse.Core.Elite.Building.Testing.Builder builder;
        public static LightHouse.Core.Elite.Building.Testing.Builder Builder
        {
            get
            {
                if (builder == null)
                {
                    builder = new LightHouse.Core.Elite.Building.Testing.Builder();
                }

                return builder;
            }
        }

        private static LightHouse.Core.Elite.Cloning.Testing.Cloner cloner;
        public static LightHouse.Core.Elite.Cloning.Testing.Cloner Cloner
        {
            get
            {
                if (cloner == null)
                {
                    cloner = new LightHouse.Core.Elite.Cloning.Testing.Cloner();
                }

                return cloner;
            }
        }
        
        private static LightHouse.Core.Collections.Testing.SurrogateList surrogateList;
        public static LightHouse.Core.Collections.Testing.SurrogateList SurrogateList
        {
            get
            {
                if (surrogateList == null)
                {
                    surrogateList = new LightHouse.Core.Collections.Testing.SurrogateList();
                }

                return surrogateList;
            }
        }

        private static LightHouse.Core.Testing.DataEnumerator dataEnumerator;
        public static LightHouse.Core.Testing.DataEnumerator DataEnumerator
        {
            get
            {
                if (dataEnumerator == null)
                {
                    dataEnumerator = new LightHouse.Core.Testing.DataEnumerator();
                }

                return dataEnumerator;
            }
        }

        private static LightHouse.Core.Testing.ContractEnumerator contractEnumerator;
        public static LightHouse.Core.Testing.ContractEnumerator ContractEnumerator
        {
            get
            {
                if (contractEnumerator == null)
                {
                    contractEnumerator = new LightHouse.Core.Testing.ContractEnumerator();
                }

                return contractEnumerator;
            }
        }

        public static void SetEnvironment()
        {
            lock (locker)
            {
                if (!initialized)
                {
                    LightHouse.Base.Testing.Operator.ClearEnvironments();

                    LightHouse.Base.Execution.Manager.AddProvider(new EmbeddedExecutionProvider());

                    LightHouse.Base.Testing.Operator.AddEnvironment("Standard", new TestEnvironment()
                    {

                    });

                    initialized = true;
                }
            }
        }
    }
}
