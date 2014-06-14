using System;

namespace SIGEPROJ.BaseClasses.Mappers
{
    /// <summary>
    /// Contiene la información necesaria para poder saber si es posible mapear un type contra otro
    /// </summary>
    public class StrategyConventionInfo
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="StrategyConventionInfo"/> .
        /// </summary>
        public StrategyConventionInfo()
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="StrategyConventionInfo"/>.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="targetType">Type of the target.</param>
        public StrategyConventionInfo(Type sourceType, Type targetType)
        {
            SourceType = sourceType;
            TargetType = targetType;
        }

        /// <summary>
        /// Gets the type of the source.
        /// </summary>
        /// <value>
        /// The type of the source.
        /// </value>
        public Type SourceType { get; private set; }
        /// <summary>
        /// Gets the type of the target.
        /// </summary>
        /// <value>
        /// The type of the target.
        /// </value>
        public Type TargetType { get; private set; }
    }
}