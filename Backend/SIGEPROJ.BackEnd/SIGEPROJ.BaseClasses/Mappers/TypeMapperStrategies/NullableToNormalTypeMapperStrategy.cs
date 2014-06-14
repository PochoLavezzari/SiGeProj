using System;
using SIGEPROJ.BaseClasses.Mappers.TypeMappers;

namespace SIGEPROJ.BaseClasses.Mappers.TypeMapperStrategies
{
    /// <summary>
    /// Clase <see cref="NullableToNormalTypeMapperStrategy"/>
    /// </summary>
    public class NullableToNormalTypeMapperStrategy : ITypeMapperStrategy
    {
        /// <summary>
        /// True si cumple la condición para utilizar este Strategy
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool Match(StrategyConventionInfo info)
        {
            return Nullable.GetUnderlyingType(info.SourceType) == info.TargetType;
        }

        /// <summary>
        /// Devuelve el Mapper correcto.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public IMemberInjection GetInjecter(StrategyConventionInfo info)
        {
            Type targetType = info.TargetType;
            return new DefaultValueMapper(targetType);
        }
    }
}