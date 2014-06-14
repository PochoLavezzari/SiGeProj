using System;

namespace SIGEPROJ.BaseClasses.Reflection
{
    /// <summary>
    /// Obtiene una representación de una property
    /// </summary>
    public interface ICustomPropertyDescriptor
    {
        /// <summary>
        /// Nombre de la Propiedad
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Acción que permite setear una propiedad
        /// </summary>
        Action<object, object> Setter { get; }

        /// <summary>
        /// Función que permite obtener una propiedad
        /// </summary>
        Func<object, object> Getter { get; }
    }
}