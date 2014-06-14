namespace SIGEPROJ.BaseClasses.Mappers.TypeMapperStrategies
{
    /// <summary>
    /// Permite mapear desde clases del mismo tipo
    /// </summary>
    public class ValueTypeTypeMapperStrategy : SameTypeTypeMapperStrategy
    {
        /// <summary>
        /// Matches el info especificado.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <returns></returns>
        public override bool Match(StrategyConventionInfo info)
        {
            return info.SourceType.IsValueType && base.Match(info);
        }
    }
}