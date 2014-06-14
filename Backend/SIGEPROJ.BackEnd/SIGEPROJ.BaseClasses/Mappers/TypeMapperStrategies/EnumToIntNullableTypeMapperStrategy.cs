using System;
using SIGEPROJ.BaseClasses.Mappers.TypeMappers;

namespace SIGEPROJ.BaseClasses.Mappers.TypeMapperStrategies
{
    /// <summary>
    /// Clase <see cref="EnumToIntTypeMapperStrategy"/>
    /// </summary>
    public class EnumToIntNullableTypeMapperStrategy : ITypeMapperStrategy
    {
        /// <summary>
        /// True si cumple la condición para utilizar este Strategy
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool Match(StrategyConventionInfo info)
        {
            Type nullableType = Nullable.GetUnderlyingType(info.TargetType);
            if (nullableType == null) return false;
            return info.SourceType.IsSubclassOf(typeof(Enum)) && nullableType == typeof(int);
        }

        /// <summary>
        /// Devuelve el Mapper correcto.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public IMemberInjection GetInjecter(StrategyConventionInfo info)
        {
            return new EnumToIntNullableMapper();
        }
    }
}