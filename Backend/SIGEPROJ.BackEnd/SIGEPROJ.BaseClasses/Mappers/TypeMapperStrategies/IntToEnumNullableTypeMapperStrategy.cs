using System;
using SIGEPROJ.BaseClasses.Mappers.TypeMappers;

namespace SIGEPROJ.BaseClasses.Mappers.TypeMapperStrategies
{
    /// <summary>
    /// Clase <see cref="IntToEnumNullableTypeMapperStrategy"/>
    /// </summary>
    public class IntToEnumNullableTypeMapperStrategy : ITypeMapperStrategy
    {
        /// <summary>
        /// True si cumple la condición para utilizar este Strategy
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool Match(StrategyConventionInfo info)
        {
            Type nullableTargetType = Nullable.GetUnderlyingType(info.TargetType);
            if (nullableTargetType == null) return false;
            return info.SourceType == typeof(int) && nullableTargetType.IsSubclassOf(typeof(Enum));
        }

        /// <summary>
        /// Devuelve el Mapper correcto.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public IMemberInjection GetInjecter(StrategyConventionInfo info)
        {
            Type targetType = info.TargetType;
            return new IntToEnumNullableMapper(targetType);
        }
    }
}