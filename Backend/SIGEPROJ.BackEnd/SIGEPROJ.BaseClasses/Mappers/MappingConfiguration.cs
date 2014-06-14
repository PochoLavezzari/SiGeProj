using System;
using System.Collections.Generic;

namespace SIGEPROJ.BaseClasses.Mappers
{
    /// <summary>
    /// Base class that stores the mapping configuration
    /// </summary>
    public abstract class MappingConfiguration
    {
        #region Extensiones
        /// <summary>
        /// Lista de extensiones que puede contener esta configuración.
        /// <para>Permite agregar más información sin necesidad de heredar la clase.</para>
        /// </summary>
        private readonly IDictionary<Type, object> extensions = new Dictionary<Type, object>();

        /// <summary>
        /// Obtiene una extensión. Devuelve null si no existe.
        /// </summary>
        /// <typeparam name="TExtensionType">The type of the extension type.</typeparam>
        /// <returns></returns>
        public virtual TExtensionType GetExtension<TExtensionType>()
            where TExtensionType : class
        {
            Type extensionType = typeof (TExtensionType);
            if (extensions.ContainsKey(extensionType))
                return (TExtensionType)extensions[extensionType];
            return null;
        }

        /// <summary>
        /// Agrega una extensión. No puede haber más de una extensión del mismo tipo.
        /// </summary>
        /// <typeparam name="TExtensionType">The type of the extension type.</typeparam>
        /// <param name="extension">The extension.</param>
        public virtual void AddExtension<TExtensionType>(TExtensionType extension)
        {
            extensions[typeof(TExtensionType)] = extension;
        }
        #endregion

        #region Properties

        private PropInfo sourceProperty;
        private PropInfo targetProperty;

        /// <summary>
        /// Orden en el que se evaluarán los Mappings
        /// </summary>
        public virtual int Order { get; set; }
        
        /// <summary>
        /// Gets or sets the source property.
        /// </summary>
        /// <value>
        /// The source property.
        /// </value>
        // ReSharper disable ConvertToAutoProperty
        public virtual PropInfo SourceProperty
        // ReSharper restore ConvertToAutoProperty
        {
            get { return sourceProperty; }
            set { sourceProperty = value; }
        }

        /// <summary>
        /// Gets or sets the target property.
        /// </summary>
        /// <value>
        /// The target property.
        /// </value>
        public virtual PropInfo TargetProperty
        {
            get { return targetProperty; }
            set { targetProperty = value; }
        }
        #endregion

        #region ctor
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="MappingConfiguration"/>.
        /// </summary>
        /// <param name="sourceProperty">The source property.</param>
        /// <param name="targetProperty">The target property.</param>
        protected MappingConfiguration(PropInfo sourceProperty, PropInfo targetProperty)
        {
            this.sourceProperty = sourceProperty;
            this.targetProperty = targetProperty;
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="MappingConfiguration"/> .
        /// </summary>
        protected MappingConfiguration()
            : this(new PropInfo(), new PropInfo())
        {
        }
        #endregion

        /// <summary>
        /// Permite convertir desde un valor a otro. Sirve para cambiar los valores de los filtros.
        /// </summary>
        /// <param name="targetValue"></param>
        /// <returns></returns>
        public virtual object ConvertTargetValueToSourceValue(object targetValue)
        {
            // Por defecto se devuelve el mismo valor
            return targetValue;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            string source = "source";
            string target = "target";
            if (TargetProperty != null)
                target += "." + TargetProperty.Name;
            if (SourceProperty != null)
                source += "." + SourceProperty.Name;
            return "MappingConfiguration[" + source + " => " + target + "]";
        }
    }
}
