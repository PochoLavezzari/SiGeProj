using System.Collections.Generic;

namespace SIGEPROJ.BaseClasses.Mappers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IConfigurableMemberInjection// : IMemberInjection
    {
        /// <summary>
        /// Gets the mapping configurations.
        /// </summary>
        Dictionary<string, MappingConfiguration> MappingConfigurations { get; }
    }
}