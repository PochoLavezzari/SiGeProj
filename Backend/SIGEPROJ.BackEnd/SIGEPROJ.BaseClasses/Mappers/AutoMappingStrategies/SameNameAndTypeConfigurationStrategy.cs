using System;
using System.Reflection;

namespace SIGEPROJ.BaseClasses.Mappers.AutoMappingStrategies
{
    /// <summary>
    /// Clase <see cref="SameNameAndTypeConfigurationStrategy"/>
    /// </summary>
    public class SameNameAndTypeConfigurationStrategy : IAutoMappingConfigurationStrategy
    {
        /// <summary>
        /// Matches el info especificado.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public bool Match(AutoMappingConventionInfo info, out MappingConfiguration configuration)
        {
            configuration = null;
            PropertyInfo s = info.SourceType.GetProperty(info.TargetPropertyInfo.Name);
            if (s == null)
                return false;

            if (info.TargetPropertyInfo.PropertyType != s.PropertyType)
                return false;

            // Si no puede sobreescribirse, no se agrega
            if (!info.TargetPropertyInfo.CanWrite) return false;

            var genericTargetType = typeof(DefaultFunctionMapping<,,,>).MakeGenericType(
                info.SourceType,
                info.TargetType,
                info.TargetPropertyInfo.PropertyType,
                info.TargetPropertyInfo.PropertyType);
            var funcMapping = Activator.CreateInstance(genericTargetType, new object[] { s, info.TargetPropertyInfo });

            configuration = funcMapping as MappingConfiguration;
            return configuration != null;
        }
    }

}