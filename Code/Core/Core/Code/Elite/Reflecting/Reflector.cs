using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

using LightHouse.Core.Caching;
using LightHouse.Core.Collections;

namespace LightHouse.Core.Elite.Reflecting
{
    /// <summary>
    /// Provides functionality for getting information about types and assemblies. The Reflector tries to improve the performance of System.Reflection. If that isn't possible the standard functionality of the .NET Framework is used.
    /// </summary>
    public class Reflector
    {
        /// <summary>
        /// Gets all properties of a given type.
        /// </summary>
        /// <param name="type">Type of properties.</param>
        /// <returns>List of propertyies</returns>
        public IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            return type.GetRuntimeProperties();
        }

        /// <summary>
        /// Gets all fields of a given type.
        /// </summary>
        /// <param name="type">Type of properties.</param>
        /// <returns>List of propertyies</returns>
        public IEnumerable<FieldInfo> GetFields(Type type)
        {
            return type.GetRuntimeFields();
        }

        /// <summary>
        /// Gets a propertyinfo from specified type with specified property name.
        /// </summary>
        /// <param name="type">Type to get.</param>
        /// <param name="property">Name of property.</param>
        /// <returns></returns>
        public PropertyInfo GetProperty(Type type, String property)
        {
            return type.GetRuntimeProperty(property);
        }

        /// <summary>
        /// Returns the property value of the specified target object by given propertyinfo.
        /// </summary>
        /// <param name="propertyInfo">Property to get.</param>
        /// <param name="target">Target of property to get.</param>
        /// <returns>Property value.</returns>
        public Object GetPropertyValue(PropertyInfo propertyInfo, Object target)
        {
            if (propertyInfo != null)
            {
                return propertyInfo.GetValue(target);
            }

            return null;
        }

        /// <summary>
        /// Returns the property value of the specified target object by the given property name.
        /// </summary>
        /// <param name="property">Name of property.</param>
        /// <param name="target">Target of property to get.</param>
        /// <returns>Property value.</returns>
        public Object GetPropertyValue(String property, Object target)
        {
            return GetPropertyValue(GetProperty(target.GetType(), property), target);
        }

        /// <summary>
        /// Sets the property value of the specified target object by the given PropertyInfo.
        /// </summary>
        /// <param name="propertyInfo">Property to set.</param>
        /// <param name="target">Target of property to set.</param>
        /// <param name="value">Property value.</param>
        public void SetPropertyValue(PropertyInfo propertyInfo, Object target, Object value)
        {
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(target, value);
            }
        }

        /// <summary>
        /// Sets the property value of the specified target object by given property name.
        /// </summary>
        /// <param name="property">Name of property.</param>
        /// <param name="target">Target of property to set.</param>
        /// <param name="value">Property value.</param>
        public void SetPropertyValue(String property, Object target, Object value)
        {
            SetPropertyValue(GetProperty(target.GetType(), property), target, value);
        }

        /// <summary>
        /// Gets the value based on a provided path.
        /// </summary>
        /// <param name="target">Target object on which to evaluate the path.</param>
        /// <param name="path">Path to be returned from the target object.</param>
        /// <returns>Value of the path on the target object.</returns>
        public Object GetPathValue(IObject target, String path)
        {
            Object value = target;

            IList<String> components = ParsePath(path);

            foreach (String component in components)
            {
                value = GetValue(value, component);
            }

            return value;
        }

        /// <summary>
        /// Sets the value base on a provided path.
        /// </summary>
        /// <param name="target">Target object on which to set the path value.</param>
        /// <param name="path">Path to be set in the target object.</param>
        /// <param name="value">Value of the path on the target object.</param>
        public void SetPathValue(IObject target, String path, Object value)
        {
            Object targetValue = target;

            IList<String> components = ParsePath(path);

            for (int i = 0; i < components.Count - 1; i++)
            {
                targetValue = GetValue(value, components[i]);
            }

            String lastComponent = components.Last();

            if (targetValue is IObject)
            {
                if (lastComponent.StartsWith("."))
                {
                    ((IObject)targetValue).SetProperty(lastComponent.Substring(1), value);
                }
            }
        }

        /// <summary>
        /// Splits the provided path in smaller components.
        /// </summary>
        /// <param name="path">Path to be splitted in smaller components.</param>
        /// <returns>Compoents of the path.</returns>
        private IList<String> ParsePath(String path)
        {
            String remainingPath = String.Format(".{0}", path);
            IList<String> components = new List<String>();

            Boolean componentFound = true;

            while (componentFound)
            {
                componentFound = false;

                Int32 nextDot = remainingPath.IndexOf(".", 1);

                if (nextDot != -1)
                {
                    components.Add(remainingPath.Substring(0, nextDot));
                    remainingPath = remainingPath.Substring(nextDot);
                    componentFound = true;
                }
                else
                {
                    components.Add(remainingPath);
                }
            }

            return components;
        }

        /// <summary>
        /// Gets the value of the component in an object.
        /// </summary>
        /// <param name="parent">Object to evaluate the component on.</param>
        /// <param name="component">Component to evaluate on the object.</param>
        /// <returns>Value of component in the provided object.</returns>
        private Object GetValue(Object parent, String component)
        {
            if (component.StartsWith("."))
            {
                if (parent is IObject)
                {
                    return ((IObject)parent).GetProperty<Object>(component.Substring(1));
                }
            }

            return default(Object);
        }
    }
}
