using System;

namespace SIGEPROJ.BaseClasses.Mappers.TypeMappers
{
    /// <summary>
    /// Clase <see cref="DefaultValueMapper"/>
    /// </summary>
    public class DefaultValueMapper : IMemberInjection
    {
        private readonly Type targetType;
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="DefaultValueMapper"/>.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        public DefaultValueMapper(Type targetType)
        {
            this.targetType = targetType;
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
            if (source == null)
                return Reflection.Util.DefaultForType(targetType);
            return source;
        }

        /// <summary>
        /// Lanza una excepción si es que algún mapping es incorrecto.
        /// </summary>
        public void AssertConfigurationIsValid()
        {
        }
    }
}