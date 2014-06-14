using System;
using SIGEPROJ.BaseClasses.Mappers.TypeMappers;

namespace SIGEPROJ.BaseClasses.Mappers.TypeMapperStrategies
{
    /// <summary>
    /// Clase <see cref="ArrayToArrayTypeMapperStrategy"/>
    /// </summary>
    public class ArrayToArrayTypeMapperStrategy : ITypeMapperStrategy
    {
        /// <summary>
        /// True si cumple la condición para utilizar este Strategy
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool Match(StrategyConventionInfo info)
        {
            return info.SourceType.IsArray && info.TargetType.IsArray;
        }

        /// <summary>
        /// Gets the injecter.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <returns></returns>
        public IMemberInjection GetInjecter(object source, object target, Type sourceType, Type targetType)
        {
            Type genericEnumerableType = typeof(ArrayToArrayTypeMapper<,>)
                .MakeGenericType(sourceType, targetType);

            var mapped = (IMemberInjection)Activator.CreateInstance(genericEnumerableType);
            return mapped;
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

            Type genericEnumerableType = typeof(ArrayToArrayTypeMapper<,>)
                .MakeGenericType(sourceType, targetType);

            var mapped = (IMemberInjection)Activator.CreateInstance(genericEnumerableType);
            return mapped;
        }
    }
}