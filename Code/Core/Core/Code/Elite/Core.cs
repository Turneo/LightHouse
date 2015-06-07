using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LightHouse.Core;
using LightHouse.Core.Elite.Building;
using LightHouse.Core.Elite.Cloning;
using LightHouse.Core.Elite.Converting;
using LightHouse.Core.Elite.Compiling;
using LightHouse.Core.Elite.Loading;
using LightHouse.Core.Elite.Locating;
using LightHouse.Core.Elite.Merging;
using LightHouse.Core.Elite.Notifying;
using LightHouse.Core.Elite.Reflecting;

namespace LightHouse.Elite
{
    /// <summary>
    /// Provides easy access to all available officers.
    /// </summary>
    public static class Core
    {
        /// <summary>
        /// Officer responsible for building objects.
        /// </summary>
        private static Builder builder;

        /// <summary>
        /// Officer responsible for building objects.
        /// </summary>
        public static Builder Builder
        {
            get
            {
                if (builder == null)
                {
                    builder = new Builder();
                }

                return builder;
            }
        }

        /// <summary>
        /// Officer responsible for loading proxy properties.
        /// </summary>
        private static Loader loader;

        /// <summary>
        /// Officer responsible for loading proxy properties.
        /// </summary>
        public static Loader Loader
        {
            get
            {
                if (loader == null)
                {
                    loader = new Loader();
                }

                return loader;
            }
        }

        /// <summary>
        /// Officer responsible for locating types and type informations.
        /// </summary>
        private static Locator locator;

        /// <summary>
        /// Officer responsible for locating types and type informations.
        /// </summary>
        public static Locator Locator
        {
            get
            {
                if (locator == null)
                {
                    locator = new Locator();
                }

                return locator;
            }
        }

        /// <summary>
        /// Officer responsible for reflection information of the types.
        /// </summary>
        private static Reflector reflector;
        /// <summary>
        /// Officer responsible for reflection information of the types.
        /// </summary>
        public static Reflector Reflector
        {
            get
            {
                if (reflector == null)
                {
                    reflector = new Reflector();
                }

                return reflector;
            }
        }

        /// <summary>
        /// Officer responsible for compiling code.
        /// </summary>
        private static Compiler compiler;
        /// <summary>
        /// Officer responsible for compiling code.
        /// </summary>
        public static Compiler Compiler
        {
            get
            {
                if (compiler == null)
                {
                    compiler = new Compiler();
                }

                return compiler;
            }
        }

        /// <summary>
        /// Officer responsible for cloning objects.
        /// </summary>
        private static Cloner cloner;
        /// <summary>
        /// Officer responsible for cloning objects.
        /// </summary>
        public static Cloner Cloner
        {
            get
            {
                if (cloner == null)
                {
                    cloner = new Cloner();
                }

                return cloner;
            }
        }

        /// <summary>
        /// Officer responsible for merging objects.
        /// </summary>
        private static Merger merger;
        /// <summary>
        /// Officer responsible for merging objects.
        /// </summary>
        public static Merger Merger
        {
            get
            {
                if (merger == null)
                {
                    merger = new Merger();
                }

                return merger;
            }
        }

        /// <summary>
        /// Officer responsible for converting objects.
        /// </summary>
        private static Converter converter;
        /// <summary>
        /// Officer responsible for converting objects.
        /// </summary>
        public static Converter Converter
        {
            get
            {
                if (converter == null)
                {
                    converter = new Converter();
                }

                return converter;
            }
        }

        /// <summary>
        /// Officer responsible for notifying events.
        /// </summary>
        private static Notifier notifier;
        /// <summary>
        /// Officer responsible for notifying events.
        /// </summary>
        public static Notifier Notifier
        {
            get
            {
                if (notifier == null)
                {
                    notifier = new Notifier();
                }

                return notifier;
            }
        }
    }
}
