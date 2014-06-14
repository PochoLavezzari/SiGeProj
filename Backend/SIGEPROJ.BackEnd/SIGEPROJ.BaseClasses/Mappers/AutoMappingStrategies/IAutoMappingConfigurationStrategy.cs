namespace SIGEPROJ.BaseClasses.Mappers.AutoMappingStrategies
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAutoMappingConfigurationStrategy
    {
        /// <summary>
        /// Matches el info especificado.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        bool Match(AutoMappingConventionInfo info, out MappingConfiguration configuration);
    }
}