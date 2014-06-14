using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SIGEPROJ.BaseClasses.Mappers.ValueInjecter
{
    /// <summary>
    /// this is for caching the PropertyDescriptorCollection and PropertyInfo[] for each Type
    /// </summary>
    internal static class PropertyInfosStorage
    {
        private static readonly IDictionary<Type, PropertyDescriptorCollection> Storage = new Dictionary<Type, PropertyDescriptorCollection>();
        private static readonly IDictionary<Type, IEnumerable<PropertyInfo>> InfosStorage = new Dictionary<Type, IEnumerable<PropertyInfo>>();

        private static readonly object PropsLock = new object();
        private static readonly object InfosLock = new object();

        private static readonly IList<Action<Type>> Actions = new List<Action<Type>>();


        public static void RegisterActionForEachType(Action<Type> action)
        {
            Actions.Add(action);
        }

        public static PropertyDescriptorCollection GetProps(Type type)
        {
            if (!Storage.ContainsKey(type))
            {
                lock (PropsLock)
                {
                    if (!Storage.ContainsKey(type))
                    {
                        if (!type.IsAnonymousType())
                            foreach (var action in Actions)
                                action(type);

                        var props = TypeDescriptor.GetProperties(type);
                        Storage.Add(type, props);
                    }
                }
            }

            return Storage[type];
        }

        public static PropertyDescriptorCollection GetProps(this object o)
        {
            return GetProps(o.GetType());
        }

        public static IEnumerable<PropertyInfo> GetInfos(this Type type)
        {
            if (!InfosStorage.ContainsKey(type))
            {
                lock (InfosLock)
                {
                    if (!InfosStorage.ContainsKey(type))
                    {
                        if (!type.IsAnonymousType())
                            foreach (var action in Actions)
                                action(type);

                        var props = type.GetProperties()
                            .Union(type
                                       .GetInterfaces()
                                       .SelectMany(t => t.GetProperties()));
                        InfosStorage.Add(type, props);
                    }
                }
            }
            return InfosStorage[type];
        }

        public static IEnumerable<PropertyInfo> GetInfos(this object o)
        {
            return GetInfos(o.GetType());
        }

        public static Boolean IsAnonymousType(this Type type)
        {
            var hasCompilerGeneratedAttribute = type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Length > 0;
            var nameContainsAnonymousType = type.FullName.Contains("AnonymousType");
            var isAnonymousType = hasCompilerGeneratedAttribute && nameContainsAnonymousType;

            return isAnonymousType;
        }
    }
}