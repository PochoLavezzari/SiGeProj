using System;
using System.Collections;
using SIGEPROJ.BaseClasses.Mappers.TypeMappers;
using SIGEPROJ.BaseClasses.Reflection;

namespace SIGEPROJ.BaseClasses.Mappers.TypeMapperStrategies
{
    /// <summary>
    /// Clase <see cref="DictionaryToDictionaryTypeMapperStrategy"/>
    /// </summary>
    public class DictionaryToDictionaryTypeMapperStrategy : ITypeMapperStrategy
    {
        /// <summary>
        /// True si cumple la condición para utilizar este Strategy
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool Match(StrategyConventionInfo info)
        {
            return (info.SourceType.IsEnumerable() && info.TargetType.IsEnumerable() &&
                    info.SourceType.IsGenericType && typeof(IDictionary).IsAssignableFrom(info.SourceType) &&
                    info.TargetType.IsGenericType && typeof(IDictionary).IsAssignableFrom(info.TargetType)
                   );
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
            Type genericEnumerableType = typeof(DictionaryToDictionaryTypeMapper<,>)
                .MakeGenericType(sourceType, targetType);

            var mapped = (IMemberInjection)Activator.CreateInstance(genericEnumerableType);
            return mapped;
        }
    }
}