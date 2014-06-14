using System;
using SIGEPROJ.BaseClasses.Mappers.TypeMappers;

namespace SIGEPROJ.BaseClasses.Mappers.TypeMapperStrategies
{
    /// <summary>
    /// Clase <see cref="EnumToIntTypeMapperStrategy"/>
    /// </summary>
    public class IntNullableToEnumNullableTypeMapperStrategy : ITypeMapperStrategy
    {
        /// <summary>
        /// True si cumple la condición para utilizar este Strategy
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool Match(StrategyConventionInfo info)
        {
            Type nullableType = Nullable.GetUnderlyingType(info.SourceType);
            Type nullableTargetType = Nullable.GetUnderlyingType(info.TargetType);
            if (nullableType == null) return false;
            if (nullableTargetType == null) return false;
            return nullableType == typeof(int)
                   && nullableTargetType.IsSubclassOf(typeof(Enum));
        }

        /// <summary>
        /// Devuelve el Mapper correcto.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public IMemberInjection GetInjecter(StrategyConventionInfo info)
        {
            return new IntNullableToEnumNullableMapper(info.TargetType);
        }
    }
}