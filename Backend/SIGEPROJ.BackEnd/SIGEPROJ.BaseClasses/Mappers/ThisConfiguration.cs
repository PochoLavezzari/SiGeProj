using System;
using System.Collections.Generic;
using SIGEPROJ.BaseClasses.Mappers.ValueInjecter;

namespace SIGEPROJ.BaseClasses.Mappers
{
    /// <summary>
    /// Clase que mantiene todas las referencias a This
    /// </summary>
    public class ThisConfiguration : SetValueConfiguration//, IConfigurableMemberInjection
    {
        private readonly Type thisType;
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ThisConfiguration"/> .
        /// </summary>
        public ThisConfiguration(Type thisType)
        {
            this.thisType = thisType;
            InnerMappingConfigurations = new Dictionary<Type, MappingConfiguration>();
        }

        /// <summary>
        /// Gets the inner mapping configurations.
        /// </summary>
        public Dictionary<Type, MappingConfiguration> InnerMappingConfigurations { get; private set; }

        private Dictionary<MappingConfiguration, Dictionary<string, MappingConfiguration>> mappingConfigurationsCached;
        /// <summary>
        /// Gets the mapping configurations.
        /// </summary>
        public Dictionary<MappingConfiguration, Dictionary<string, MappingConfiguration>> MappingConfigurations
        {
            get
            {
                if (mappingConfigurationsCached != null)
                    return mappingConfigurationsCached;
                Dictionary<MappingConfiguration, Dictionary<string, MappingConfiguration>> mappings =
                    new Dictionary<MappingConfiguration, Dictionary<string, MappingConfiguration>>();

                // Recorre todos los mappers internos
                foreach (var inheritedMapper in InnerMappingConfigurations)
                {
                    // Obtiene un injection
                    var injection = Mapper.GetInjection(inheritedMapper.Key, thisType);
                    MemberInjection memberInjection = injection as MemberInjection;
                    if (memberInjection != null)
                    {
                        Dictionary<string, MappingConfiguration> memberConfigs = 
                            new Dictionary<string, MappingConfiguration>();

                        // Por cada mappingConfiguration lo agrega a la lista
                        foreach (var mappingConfiguration in memberInjection.MappingConfigurations)
                        {
                            memberConfigs.Add(mappingConfiguration.Key, mappingConfiguration.Value);
                        }

                        mappings.Add(inheritedMapper.Value, memberConfigs);
                    }
                        
                }
                mappingConfigurationsCached = mappings;
                return mappings;
            }
        }

        /// <summary>
        /// Setea el valor en todos los Mappers
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public override object SetValue(object source, object target)
        {
            foreach (var mappingConfiguration in InnerMappingConfigurations.Values)
            {
                IValueInjection valueInjection = mappingConfiguration as IValueInjection;
                if (valueInjection != null)
                    target = valueInjection.Map(source, target);
            }
            return target;
        }
    }
}