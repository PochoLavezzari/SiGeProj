using SIGEPROJ.BaseClasses.Mappers.ValueInjecter;

namespace SIGEPROJ.BaseClasses.Mappers
{
    /// <summary>
    /// Indica que es una configuración que contiene configuraciones Child
    /// </summary>
    //public interface ICompositeConfiguration
    //{
    //    /// <summary>
    //    /// Gets the mapping configurations.
    //    /// </summary>
    //    Dictionary<string, MappingConfiguration> MappingConfigurations { get; }
    //}

    /// <summary>
    /// Clase <see cref="SetValueConfiguration"/>
    /// </summary>
    public abstract class SetValueConfiguration : MappingConfiguration, IValueInjection
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="SetValueConfiguration"/> .
        /// </summary>
        /// <param name="sourceProperty">The source property.</param>
        /// <param name="targetProperty">The target property.</param>
        protected SetValueConfiguration(PropInfo sourceProperty, PropInfo targetProperty)
            :base(sourceProperty,targetProperty)
        {
            
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="SetValueConfiguration"/> .
        /// </summary>
        protected SetValueConfiguration()
        {
        }

        // TODO: Debe recibir los 2 objetos y setear su propiedad de alguna manera
        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public virtual object SetValue(object source, object target)
        {
            return null;
        }

        /// <summary>
        /// Map desde un objeto source a un objeto target.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns>
        /// El objeto target
        /// </returns>
        public object Map(object source, object target)
        {
            return SetValue(source, target);
        }


    }
}