using System;
using System.Reflection;

namespace SIGEPROJ.BaseClasses.Mappers.AutoMappingStrategies
{
    /// <summary>
    /// Clase <see cref="SameNameAndTypeWithConverterConfigurationStrategy"/>
    /// </summary>
    public class SameNameAndTypeWithConverterConfigurationStrategy : IAutoMappingConfigurationStrategy
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
            {
                // Si no puede sobreescribirse, no se agrega
                if (!info.TargetPropertyInfo.CanWrite) return false;

                var injection = Mapper.GetInjection(s.PropertyType, info.TargetPropertyInfo.PropertyType);

                // Si existe algún tipo de conversión entre esos tipo, entonces lo mapea.
                if(injection != null)
                {
                    var genericTargetType = typeof(DefaultConvertFunctionMapping<,,,>).MakeGenericType(
                        info.SourceType,
                        info.TargetType,
                        s.PropertyType,
                        info.TargetPropertyInfo.PropertyType);
                    var funcMapping = Activator.CreateInstance(genericTargetType, new object[] { s, info.TargetPropertyInfo });

                    configuration = funcMapping as MappingConfiguration;
                }
            }
            return configuration != null;
        }
    }
}