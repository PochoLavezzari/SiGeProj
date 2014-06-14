using System;
using System.ComponentModel;
using System.Linq;

namespace SIGEPROJ.BaseClasses.Reflection
{
    /// <summary>
    /// Clase con métodos útiles para CustomPropertyDescriptors
    /// </summary>
    public static class PropertyDescriptorUtil
    {
        #region Metódos públicos
        /// <summary>
        /// Obtiene un Getter
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="atributo"></param>
        /// <returns></returns>
        public static Func<object, object> GetPropertyDescriptorGetter(object objeto, string atributo)
        {
            return GetPropertyDescriptorGetter(objeto, atributo.Split('.'));
        }

        /// <summary>
        /// Obtiene un Setter
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="atributo"></param>
        /// <returns></returns>
        public static Action<object, object> GetPropertyDescriptorSetter(object objeto, string atributo)
        {
            return GetPropertyDescriptorSetter(objeto, atributo.Split('.'));
        }
        #endregion

        #region Métodos privados
        /// <summary>
        /// Obtiene un Getter
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="atributos"></param>
        /// <returns></returns>
        internal static Func<object, object> GetPropertyDescriptorGetter(object objeto, string[] atributos)
        {
            // Obtiene la función Get del primer término
            Func<object, object> func = GetPropertyDescriptorGetterIgnoreCase(objeto, atributos[0]);
            // Si hay más obtiene el path completo
            if (atributos.Length > 1)
            {
                // Obtiene el Getter interno
                Func<object, object> innerFunc =
                    Util.GetGetter(
                        func(objeto),                   // Ejecuta la función con el objeto actual
                        atributos.Skip(1).ToArray());   // Devuelve la lista de atributos salteandose la primera

                if (innerFunc == null)
                {
                    throw new Exception(String.Format("Atributo no encontrado en objeto '{0}'.", objeto));
                }

                // Devuelve el Getter interno 
                return delegate(object x)
                           {
                               object value = func(x);
                               return value == null ? null : innerFunc(value);
                           };
            }
            return func;
        }

        /// <summary>
        /// Obtiene un Getter
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="atributos"></param>
        /// <returns></returns>
        internal static Action<object, object> GetPropertyDescriptorSetter(object objeto, string[] atributos)
        {
            // Si hay más obtiene el path completo
            if (atributos.Length > 1)
            {
                // Obtiene el Getter interno
                Func<object, object> innerFunc =
                    Util.GetGetter(
                        objeto,
                        atributos.Take(atributos.Length - 1).ToArray());

                if (innerFunc == null)
                {
                    throw new Exception(String.Format("Atributo no encontrado en objeto '{0}'.", objeto));
                }

                Action<object, object> action = GetPropertyDescriptorSetterIgnoreCase(innerFunc(objeto), atributos.Last());
                // Devuelve el Getter interno 
                return delegate(object obj, object value)
                           {
                               object innerValue = innerFunc(obj);
                               if (innerValue != null)
                                   action(innerFunc(obj), value);
                           };
            }
            
            // Obtiene el setter simple
            return GetPropertyDescriptorSetterIgnoreCase(objeto, atributos[0]);
            
        }

        /// <summary>
        /// Devuelve un PropertyInfo sin importar si está en minúsculas o mayúsculas
        /// el nombre de la propiedad (En DataDic se guardan siempre en mayúsculas)
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="atributo"></param>
        /// <returns></returns>
        internal static Action<object, object> GetPropertyDescriptorSetterIgnoreCase(object objeto, string atributo)
        {
            PropertyDescriptorCollection collection = TypeDescriptor.GetProperties(objeto);
            foreach (PropertyDescriptor propertyDescriptor in collection)
            {
                if (string.Compare(propertyDescriptor.Name,
                    atributo,
                    StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    if (propertyDescriptor.IsReadOnly)
                        return
                            delegate
                                {
                                    throw new InvalidOperationException(string.Format("Property {0} cannot be written",
                                                                                      atributo));
                                };
                    return propertyDescriptor.SetValue;
                }
            }
            return
                (obj, value) => { throw new InvalidOperationException(string.Format("Property {0} does not exist", atributo)); };
        }

        /// <summary>
        /// Devuelve un PropertyInfo sin importar si está en minúsculas o mayúsculas
        /// el nombre de la propiedad (En DataDic se guardan siempre en mayúsculas)
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="atributo"></param>
        /// <returns></returns>
        internal static Func<object, object> GetPropertyDescriptorGetterIgnoreCase(object objeto, string atributo)
        {
            PropertyDescriptorCollection collection = TypeDescriptor.GetProperties(objeto);
            foreach (PropertyDescriptor propertyDescriptor in collection)
            {
                if (string.Compare(propertyDescriptor.Name,
                    atributo,
                    StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    return propertyDescriptor.GetValue;
                }
            }
            return x => { throw new InvalidOperationException(string.Format("Property {0} does not exist", atributo)); };
        }

        /// <summary>
        /// Devuelve un PropertyInfo sin importar si está en minúsculas o mayúsculas
        /// el nombre de la propiedad (En DataDic se guardan siempre en mayúsculas)
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="atributo"></param>
        /// <returns></returns>
        internal static PropertyDescriptor GetPropertyDescriptorIgnoreCase(object objeto, string atributo)
        {
            PropertyDescriptorCollection collection = TypeDescriptor.GetProperties(objeto);
            return collection
                .Cast<PropertyDescriptor>()
                        .FirstOrDefault
                        (propertyDescriptor => string.Compare(propertyDescriptor.Name, atributo, StringComparison.CurrentCultureIgnoreCase) == 0);
        }
        #endregion
    }
}