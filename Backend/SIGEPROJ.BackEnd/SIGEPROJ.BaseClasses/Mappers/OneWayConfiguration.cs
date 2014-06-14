namespace SIGEPROJ.BaseClasses.Mappers
{
    /// <summary>
    /// Clase <see cref="OneWayConfiguration&lt;TConfiguration&gt;"/>
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    public abstract class OneWayConfiguration<TConfiguration>
        : SetValueConfiguration
        where TConfiguration : OneWayConfiguration<TConfiguration>
        
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="OneWayConfiguration&lt;TConfiguration&gt;"/> .
        /// </summary>
        protected OneWayConfiguration()
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="OneWayConfiguration&lt;TConfiguration&gt;"/>.
        /// </summary>
        /// <param name="sourceProperty">The source property.</param>
        /// <param name="targetProperty">The target property.</param>
        protected OneWayConfiguration(PropInfo sourceProperty, PropInfo targetProperty)
            : base(sourceProperty, targetProperty)
        {
        }

        /// <summary>
        /// true si la propiedad debe setearse únicamente en 1 sentido.
        /// </summary>
        public bool IsOneWay { get; private set; }

        /// <summary>
        /// Indica que la propiedad debe setearse únicamente en 1 sentido.
        /// </summary>
        /// <returns></returns>
        public TConfiguration OneWayOnly()
        {
            IsOneWay = true;
            return (TConfiguration) this;
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
            return "MappingConfiguration[" + source + " => " + target + (IsOneWay ? ", IsOneWay" : "") + "]";
        }
    }
}