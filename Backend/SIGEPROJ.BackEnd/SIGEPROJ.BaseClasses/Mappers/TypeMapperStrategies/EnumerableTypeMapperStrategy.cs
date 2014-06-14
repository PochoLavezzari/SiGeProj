using System;
using SIGEPROJ.BaseClasses.Reflection;

namespace SIGEPROJ.BaseClasses.Mappers.TypeMapperStrategies
{
    /// <summary>
    /// Clase <see cref="EnumerableTypeMapperStrategy"/>
    /// </summary>
    public class EnumerableTypeMapperStrategy : ITypeMapperStrategy
    {
        /// <summary>
        /// True si cumple la condición para utilizar este Strategy
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool Match(StrategyConventionInfo info)
        {
            // TODO: Faltaría saber si las properties son readonly
            return info.SourceType.IsEnumerable() && info.TargetType.IsEnumerable();
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
            Type genericEnumerableType = typeof(TypeMappers.EnumerableTypeMapper<,>)
                .MakeGenericType(sourceType, targetType);

            var mapped = (IMemberInjection)Activator.CreateInstance(genericEnumerableType);
            return mapped;
        }
    }
}