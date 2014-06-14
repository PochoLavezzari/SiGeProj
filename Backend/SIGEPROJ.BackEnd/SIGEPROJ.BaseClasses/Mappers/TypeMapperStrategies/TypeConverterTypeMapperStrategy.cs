using System;
using System.ComponentModel;
using SIGEPROJ.BaseClasses.Mappers.TypeMappers;

namespace SIGEPROJ.BaseClasses.Mappers.TypeMapperStrategies
{
    /// <summary>
    /// Clase <see cref="TypeConverterTypeMapperStrategy"/>
    /// </summary>
    public class TypeConverterTypeMapperStrategy : ITypeMapperStrategy
    {
        /// <summary>
        /// Gets the type converter.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <returns></returns>
        private static TypeConverter GetTypeConverter(Type sourceType)
        {
            return TypeDescriptor.GetConverter(sourceType);
        }

        /// <summary>
        /// True si cumple la condición para utilizar este Strategy
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool Match(StrategyConventionInfo info)
        {
            return GetTypeConverter(info.SourceType).CanConvertTo( info.TargetType);
        }

        /// <summary>
        /// Devuelve el Mapper correcto.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public IMemberInjection GetInjecter(StrategyConventionInfo info)
        {
            Type sourceType = info.SourceType;
            Type targetType = info.TargetType;
            Type genericEnumerableType = typeof(TypeConverterMapper<,>)
                .MakeGenericType(sourceType, targetType);

            var mapped = (IMemberInjection)Activator.CreateInstance(genericEnumerableType);
            return mapped;
        }
    }
}