using System;
using System.Reflection;

namespace SIGEPROJ.BaseClasses.Mappers.AutoMappingStrategies
{
    /// <summary>
    /// Clase que se utiliza para descubrir todas las propiedades que se pueden mapear de
    /// forma automática.
    /// </summary>
    public class AutoMappingConventionInfo
    {
        /// <summary>
        /// Gets or sets the target property info.
        /// </summary>
        /// <value>
        /// The target property info.
        /// </value>
        public PropertyInfo TargetPropertyInfo { get; set; }
        /// <summary>
        /// Gets or sets the type of the target.
        /// </summary>
        /// <value>
        /// The type of the target.
        /// </value>
        public Type TargetType { get; set; }
        /// <summary>
        /// Gets or sets the type of the source.
        /// </summary>
        /// <value>
        /// The type of the source.
        /// </value>
        public Type SourceType { get; set; }
    }
}