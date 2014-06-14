using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SIGEPROJ.BaseClasses.Reflection
{
    /// <summary>
    /// Clase con métodos útiles para obtener expressiones compiladas de Getters y Setters
    /// para optimizar el seteo y la obtención de datos sin utilizar Reflection
    /// </summary>
    public static class PropertyLambdaExpressionUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private static Func<object, object> CreateLambdaPropertyGetter(PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException("property");

            if (!property.CanRead)
                return x => { throw new InvalidOperationException("Property cannot be readen"); };

            // Objeto sobre el que se quiere obtener un Getter
            var objParam = Expression.Parameter(Util.objectType, "obj");

            // Se convierte en (x) => Convert(x).Getter()
            var body = Expression.Convert(Expression.Call(
                Expression.Convert(objParam, property.ReflectedType),
                property.GetGetMethod()), Util.objectType);

            return Expression.Lambda<Func<object, object>>(
                body, objParam).Compile();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private static Action<object, object> CreateLambdaPropertySetter(PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException("property");
            if (!property.CanWrite)
                return (x,y) => { throw new InvalidOperationException("Property cannot be written"); };

            var objParam = Expression.Parameter(Util.objectType, "obj");
            var valueParam = Expression.Parameter(Util.objectType, "value");

            // Debería ser así: (x,y) => x.Set( Convertir( y ) )
            var body = Expression.Call(
                // Obtiene el type 
                Expression.Convert(objParam, property.ReflectedType),
                property.GetSetMethod(),
                Expression.Convert(
                    Expression.Call(Util.ParseValueMethod, Expression.Constant(property.PropertyType), valueParam),
                    property.PropertyType)
                );
            return Expression.Lambda<Action<object, object>>(
                body, objParam, valueParam).Compile();

        }

        #region Metódos públicos
        /// <summary>
        /// Obtiene un Getter
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="atributo"></param>
        /// <returns></returns>
        public static Func<object, object> GetPropertyLambdaGetter(object objeto, string atributo)
        {
            return GetPropertyLambdaGetter(objeto, atributo.Split('.'));
        }

        /// <summary>
        /// Obtiene un Setter
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="atributo"></param>
        /// <returns></returns>
        public static Action<object, object> GetPropertyLambdaSetter(object objeto, string atributo)
        {
            return GetPropertyLambdaSetter(objeto, atributo.Split('.'));
        }
        #endregion

        #region Métodos privados
        /// <summary>
        /// Obtiene un Getter
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="atributos"></param>
        /// <returns></returns>
        public static Func<object, object> GetPropertyLambdaGetter(object objeto, string [] atributos)
        {
            Type type = objeto.GetType();
            PropertyInfo info = Util.GetPropertyInfoIgnoreCase(type, atributos[0]);
            Type propertyType = info.PropertyType;
            // Obtiene la función Get del primer término
            Func<object, object> func = CreateLambdaPropertyGetter(info);
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
                return x =>
                {
                    object value = func(x);
                    return value == null ? Util.DefaultForType(propertyType) : innerFunc(value);
                };
            }
            return func;
        }

        /// <summary>
        /// Obtiene un Setter
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="atributos"></param>
        /// <returns></returns>
        public static Action<object, object> GetPropertyLambdaSetter(object objeto, string [] atributos)
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

                Action<object, object> action = GetPropertyLambdaSetter(innerFunc(objeto), atributos.Last());

                // Devuelve el Getter interno 
                return (obj, value) => {
                    object innerValue = innerFunc(obj);
                    if (innerValue != null)
                        action(innerValue, value);
                };
            }

            Type type = objeto.GetType();
            PropertyInfo info = Util.GetPropertyInfoIgnoreCase(type, atributos[0]);
            // Obtiene el setter simple
            return CreateLambdaPropertySetter(info);
            
        }
        #endregion
    }
}