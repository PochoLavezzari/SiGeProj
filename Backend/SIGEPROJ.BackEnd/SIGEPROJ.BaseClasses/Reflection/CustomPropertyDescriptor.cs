using System;

namespace SIGEPROJ.BaseClasses.Reflection
{
    /// <summary>
    /// Obtiene una representación de una property
    /// </summary>
    public class CustomPropertyDescriptor : ICustomPropertyDescriptor
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CustomPropertyDescriptor(
            string propertyName,
            Action<object,object> setter,
            Func<object,object>   getter)
        {
            PropertyName = propertyName;
            Setter = setter;
            Getter = getter;
        }
        /// <summary>
        /// Nombre de la Propiedad
        /// </summary>
        public virtual string PropertyName { get; set; }

        /// <summary>
        /// Acción que permite setear una propiedad
        /// </summary>
        public virtual Action<object, object> Setter { get; set; }

        /// <summary>
        /// Función que permite obtener una propiedad
        /// </summary>
        public virtual Func<object, object> Getter { get; set; }

        #region Actualización de Objetos o propiedades
        /// <summary>
        /// Actualiza el objeto con el valor pasado por parámetro
        /// </summary>
        /// <param name="customPropertyDescriptor"></param>
        /// <param name="objectToBind"></param>
        /// <param name="value"></param>
        public static void UpdateObjectValue(
            ICustomPropertyDescriptor customPropertyDescriptor,
            object objectToBind,
            object value)
        {
            if (customPropertyDescriptor != null)
            {
                customPropertyDescriptor.Setter(objectToBind, value);
            }
        }

        /// <summary>
        /// Actualiza el valor de la variable pasada por parámetro con el valor que 
        /// contiene el objeto
        /// </summary>
        /// <param name="customPropertyDescriptor"></param>
        /// <param name="propertyToBind"></param>
        /// <param name="objectToBind"></param>
        public static void UpdatePropertyValue(
            ICustomPropertyDescriptor customPropertyDescriptor,
            ref object propertyToBind,
            object objectToBind)
        {
            if (customPropertyDescriptor != null)
            {
                propertyToBind = customPropertyDescriptor.Getter(objectToBind);
            }
        }
        #endregion
    }
}