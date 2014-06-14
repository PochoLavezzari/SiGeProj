using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;
using System.ComponentModel;
using SIGEPROJ.BaseClasses.Reflection.ParseStrategies;

namespace SIGEPROJ.BaseClasses.Reflection
{
    /// <summary>
    /// Clase que permite hacer obtener datos de propiedades
    /// </summary>
    public static class Util
    {
        #region Tipos Cacheados
        internal readonly static Type objectType = typeof(object);
        internal readonly static Type stringType = typeof(string);
        internal readonly static Type intType = typeof(int);
        internal readonly static Type decimalType = typeof(decimal);
        internal readonly static Type floatType = typeof(float);
        internal readonly static Type dateTimeType = typeof(DateTime);
        internal readonly static Type boolType = typeof(bool);
        internal readonly static Type int64Type = typeof(Int64); //JCR
        #endregion

        #region Strategies de obtención de valores simples
        /// <summary>
        /// 
        /// </summary>
        static IList<ISimpleParseStrategy> strategies = new List<ISimpleParseStrategy>();
        #endregion

        /// <summary>
        /// Initializes the <see cref="Util"/> class.
        /// </summary>
        static Util()
        {
            strategies.Add(new ParseStringStrategy());
            strategies.Add(new ParseIntegerStrategy());
            strategies.Add(new ParseDecimalStrategy());
            strategies.Add(new ParseFloatStrategy());
            strategies.Add(new ParseBoolStrategy());
            strategies.Add(new ParseInt64Strategy());
        }

        /// <summary>
        /// Obtiene el valor de una propiedad
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="atributo"></param>
        /// <returns></returns>
        public static object GetObjectValue(object objeto, string atributo)
        {
            if (objeto == null || string.IsNullOrEmpty(atributo))
                return null;

            // Si el atributo es compuesto: Ej : Automovil.Marca
            string[] atributos = atributo.Split('.');

            PropertyDescriptor prop = PropertyDescriptorUtil.GetPropertyDescriptorIgnoreCase(objeto, atributos[0]);
            if (prop == null)
            {
                throw new Exception(String.Format("Atributo '{0}' no encontrado en objeto '{1}'.", atributo, objeto));
            }

            object s = atributos.Length > 1
                           ? GetObjectValue(prop.GetValue(objeto),
                                            string.Join(".", atributos, 1, atributos.Length - 1))
                           : prop.GetValue(objeto);
            
            return s;
        }

        /// <summary>
        /// Setea el valor de una propiedad
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="atributo"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static void SetObjectValue(object objeto, string atributo, object valor)
        {
            if (objeto == null || string.IsNullOrEmpty(atributo))
                return;

            // Si el atributo es compuesto: Ej : Automovil.Marca
            string[] atributos = atributo.Split('.');

            PropertyDescriptor prop = PropertyDescriptorUtil.GetPropertyDescriptorIgnoreCase(objeto, atributos[0]);
            if (prop == null)
            {
                throw new Exception(String.Format("Atributo '{0}' no encontrado en objeto '{1}'.", atributo, objeto));
            }

            if (atributos.Length > 1)
            {
                SetObjectValue(prop.GetValue(objeto), string.Join(".", atributos, 1, atributos.Length - 1), valor);
            }
            else
            {
                prop.SetValue(objeto, ParseValue(prop.PropertyType, valor));
            }
        }

        /// <summary>
        /// Convierte "valor" en el tipo correcto
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static object ParseValue(Type tipo, object valor)
        {
            // Si el valor que llega es DBNull lo convierto a null para no tener que 
            // preguntar si no es null o DBNull.
            if ((valor is DBNull))
            {
                valor = null;
            }
            if (valor != null)
            {
                // Si el tipo recibido es es mismo que se espera lo devuelve
                Type valueReceived = valor.GetType();
                if (tipo == valueReceived)
                    return valor;
            }
            // Para ver si es de tipo Nullable
            if(tipo.IsGenericType)
            {
                Type auxType = tipo.GetGenericTypeDefinition();
                if (auxType == typeof(Nullable<>))
                {
                    // Obtiene el tipo concreto
                    Type valueType = tipo.GetGenericArguments()[0];
                    if (valor == null || string.IsNullOrEmpty(valor.ToString()))
                    {
                        return null;
                    }
                    return ParseSimpleValue(valueType, valor);
                }
            }

            return ParseSimpleValue(tipo, valor);
            
        }

        /// <summary>
        /// Parsea Valores simples, ValueObjects No nullables u otros objetos
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static object ParseSimpleValue(Type tipo, object valor)
        {
            // TODO: Ver cómo mejorar esta implementación utilizando Builders o lo mismo que se utilizó en Wrappers
            foreach (var strategy in strategies)
            {
                if (strategy.Applies(tipo))
                {
                    return strategy.Parse(tipo, valor);
                }
            }
            // Si no está contemplado devuelve el mismo objeto
            return valor;
        }

        /// <summary>
        /// Devuelve una lista de propiedades encadenadas para obtener un valor
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="atributo"></param>
        /// <returns></returns>
        public static List<PropertyInfo> GetPropertyInfoList(object objeto, string atributo)
        {
            if (objeto == null || atributo == null)
                return null;
            return GetPropertyInfoList(objeto.GetType(), atributo);
        }

        /// <summary>
        /// Devuelve una lista de propiedades encadenadas para obtener un valor
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="atributo"></param>
        /// <returns></returns>
        public static List<PropertyInfo> GetPropertyInfoList(Type objeto, string atributo)
        {
            var properties = new List<PropertyInfo>();
            if (objeto == null || atributo == null)
                return null;

            // Si el atributo es compuesto: Ej : Automovil.Marca
            string[] atributos = atributo.Split('.');

            PropertyInfo prop = GetPropertyInfoIgnoreCase(objeto, atributos[0]);
            if (prop == null)
            {
                throw new Exception(String.Format("Atributo '{0}' no encontrado en objeto '{1}'.", atributo, objeto));
            }
            properties.Add(prop);
            if (atributos.Length > 1)
            {
                List<PropertyInfo> innerProps = GetPropertyInfoList(prop.PropertyType,
                                                                    string.Join(".", atributos, 1, atributos.Length - 1));
                properties.AddRange(innerProps);
            }
            return properties;
        }

        /// <summary>
        /// Devuelve el property info correspondiente al objeto y propiedad dados
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="atributo"></param>
        /// <param name="throwIfNoExist">true si debe lanzar una excepción si es que no existe el atributo en el objeto</param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo(object objeto, string atributo, bool throwIfNoExist)
        {
            if (objeto == null || atributo == null)
                return null;

            // Si el atributo es compuesto: Ej : Automovil.Marca
            string[] atributos = atributo.Split('.');

            Type objType = objeto.GetType();

            PropertyInfo prop = GetPropertyInfoIgnoreCase(objType, atributos[0]);
            if (prop == null)
            {
                if (throwIfNoExist)
                    throw new Exception(String.Format("Atributo '{0}' no encontrado en objeto '{1}'.", atributo, objeto));
                return null;
            }

            return atributos.Length > 1
                       ? GetPropertyInfo(prop.GetValue(objeto, null),
                                         string.Join(".", atributos, 1, atributos.Length - 1))
                       : prop;
        }

        /// <summary>
        /// Devuelve el property info correspondiente al objeto y propiedad dados
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="atributo"></param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo(object objeto, string atributo)
        {
            return GetPropertyInfo(objeto, atributo, false);
        }

        /// <summary>
        /// Obtiene el valor de una propiedad
        /// Tiene error si se intenta obtener el valor de un objeto
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static object GetObjectValue(object objeto, PropertyInfo prop)
        {
            if (objeto == null || prop == null)
                return null;
            return prop.GetValue(objeto, null);
        }

        /// <summary>
        /// Obtiene el valor de una propiedad
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public static object GetObjectValue(object objeto, List<PropertyInfo> properties)
        {
            object value = null;
            object auxObjeto = objeto;
            if (objeto == null || properties == null)
                return null;
            foreach (PropertyInfo prop in properties)
            {
                //value = prop.GetValue(auxObjeto, null);
                value = GetObjectValue(auxObjeto, prop);
                if (value == null) return null;
                auxObjeto = value;
            }
            return value;
        }

        /// <summary>
        /// Setea el valor de una propiedad
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="prop"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static void SetObjectValue(object objeto, PropertyInfo prop, object valor)
        {
            if (objeto == null || prop == null)
                return;

            prop.SetValue(objeto, ParseValue(prop.PropertyType, valor), null);
        }

        /// <summary>
        /// Setea el valor de una propiedad
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="prop"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static void SetObjectValue(object objeto, PropertyDescriptor prop, object valor)
        {
            if (objeto == null || prop == null)
                return;

            prop.SetValue(objeto, ParseValue(prop.PropertyType, valor));
        }

        /// <summary>
        /// Setea el Valor de un objeto
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="properties"></param>
        /// <param name="valor"></param>
        public static void SetObjectValue(object objeto, List<PropertyInfo> properties, object valor)
        {
            object auxObjeto = objeto;
            int i = 1;
            if (objeto == null || properties == null || properties.Count == 0)
                return;

            foreach (PropertyInfo propAux in properties)
            {
                if (properties.Count == i++)
                    break;
                object value = GetObjectValue(auxObjeto, propAux);

                if (value == null) return;
                auxObjeto = value;
            }

            PropertyInfo prop = properties[properties.Count - 1];
            prop.SetValue(auxObjeto, ParseValue(prop.PropertyType, valor), null);
        }

        /// <summary>
        /// Devuelve un PropertyInfo sin importar si está en minúsculas o mayúsculas
        /// el nombre de la propiedad (En DataDic se guardan siempre en mayúsculas)
        /// </summary>
        /// <param name="tipoObjeto"></param>
        /// <param name="atributo"></param>
        /// <returns></returns>
        internal static PropertyInfo GetPropertyInfoIgnoreCase(Type tipoObjeto, string atributo)
        {
            PropertyInfo prop = tipoObjeto.GetProperty(atributo);
            if (prop == null)
            {
                foreach (PropertyInfo propertyInfo in tipoObjeto.GetProperties())
                {
                    if (string.Compare(propertyInfo.Name, atributo, StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        prop = propertyInfo;
                        break;
                    }
                }
            }
            return prop;
        }

        /// <summary>
        /// Devuelve el default de un Tipo especificado.
        /// Ej: string : null
        ///     int    : 0
        ///  </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static object DefaultForType(Type targetType)
        {
            return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
        }

        /// <summary>
        /// Obtiene una lista genérica
        /// </summary>
        /// <param name="typeX"></param>
        /// <param name="lista"></param>
        /// <returns></returns>
        public static IList CreateGenericList(Type typeX, IEnumerable lista)
        {
            Type listType = typeof(List<>);
            Type[] typeArgs = { typeX };
            Type genericType = listType.MakeGenericType(typeArgs);
            IList o = Activator.CreateInstance(genericType) as IList;
            if (o != null && lista != null)
                foreach (var l in lista)
                {
                    o.Add(l);
                }
            return o;
        }

        #region Reflection Methods

        /// <summary>
        /// Obtiene el nombre de un método especificado en una Expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string GetMethodName<T, TReturn>(Expression<Func<T, TReturn>> expression)
        {
            var memberExpression = expression.Body as MethodCallExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException("expression must be in the form: (Foo instance) => instance.Method");
            }
            return memberExpression.Method.Name;
        }

        /// <summary>
        /// Obtiene el nombre de un método especificado en una Expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string GetMethodName<T>(Expression<Action<T>> expression)
        {
            var memberExpression = expression.Body as MethodCallExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException("expression must be in the form: (Foo instance) => instance.Method");
            }
            return memberExpression.Method.Name;
        }

        /// <summary>
        /// Obtiene el nombre de una property especificada en una expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string GetPropertyName<T>(Expression<Func<T, object>> expression)
        {
            return GetPropertyName<T, object>(expression);
        }

        /// <summary>
        /// Obtiene el nombre de una property especificada en una expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        //[DebuggerStepThrough]
        public static string GetPropertyName<T, TReturn>(Expression<Func<T, TReturn>> expression)
        {
            MemberExpression memberExpression = null;
            // FIX: Cuando se utiliza un Struct viene una unary Expression con el método Convert
            UnaryExpression unaryExpression = expression.Body as UnaryExpression;
            if (unaryExpression != null)
            {
                if (unaryExpression.Method == null)
                    memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                // Caso especial en el que en lugar se obtenerse una property, se obtiene el mismo objeto
                ParameterExpression paramExpression = expression.Body as ParameterExpression;
                if(paramExpression != null)
                {
                    return "this";
                }
                memberExpression = expression.Body as MemberExpression;
            }
            if (memberExpression == null)
            {
                throw new ArgumentException(
                    "expression must be in the form: (Thing instance) => instance.Property[.Optional.Other.Properties.In.Chain]");
            }
            string name = memberExpression.Member.Name;

            //while (memberExpression.Expression as MemberExpression != null)
            Expression expr;
            Expression newExpresion = memberExpression;
            while ((expr = NextExpression(newExpresion)) != null)
            {
                if (expr.NodeType == ExpressionType.MemberAccess)
                {
                    newExpresion = (MemberExpression)expr;
                    name = ((MemberExpression)newExpresion).Member.Name + "." + name;
                }
                else if (expr.NodeType == ExpressionType.Call)
                {
                    MethodCallExpression methodCall = (MethodCallExpression)expr;
                    string separators = "()";
                    bool isEnumerable = false;
                    if (methodCall.Object.Type.IsEnumerable())
                    {
                        isEnumerable = true;
                    }
                    string auxname = string.Empty;

                    if (isEnumerable)
                    {
                        if (methodCall.Method.Name.StartsWith("get_"))
                        {
                            separators = "[]";
                            if (methodCall.Method.Name != "get_Item")
                                auxname += methodCall.Method.Name.Substring(4);
                        }
                        else
                        {
                            auxname += methodCall.Method.Name;
                        }
                    }
                    else
                    {
                        auxname += methodCall.Method.Name;
                    }

                    auxname += separators[0];

                    foreach (Expression argument in methodCall.Arguments)
                    {
                        if (argument.NodeType == ExpressionType.Constant)
                        {
                            auxname += ((ConstantExpression)argument).Value;
                        }
                        else
                            auxname += argument.ToString();

                    }
                    name = auxname + separators[1] + "." + name;
                    newExpresion = methodCall;
                }
                else
                {
                    newExpresion = null;
                }
            }
            return name;
        }

        /// <summary>
        /// Nexts the expression.
        /// </summary>
        /// <param name="memberExpression">The member expression.</param>
        /// <returns></returns>
        private static Expression NextExpression(Expression memberExpression)
        {
            if (memberExpression == null)
                return null;
            if (memberExpression.NodeType == ExpressionType.MemberAccess)
            {
                return ((MemberExpression)memberExpression).Expression;
            }
            if (memberExpression.NodeType == ExpressionType.Call)
            {
                return ((MethodCallExpression)memberExpression).Object;
            }
            return null;
        }

        /// <summary>
        /// Obtiene el nombre del Namespace
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string GetNamespaceName<T>()
        {
            return typeof(T).Namespace;
        }

        /// <summary>
        /// Obtiene el nombre de la clase
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string GetClassName<T>()
        {
            return typeof(T).Name;
        }

        /// <summary>
        /// Indica true si el Type contiene la interfaz pasada por parámetro
        /// </summary>
        /// <param name="this"></param>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public static bool HasInterface(this Type @this, Type interfaceType)
        {
            return @this.GetInterfaces().Contains(interfaceType);
        }

        /// <summary>
        /// Indica true si el Type t hereda de la clase base
        /// </summary>
        /// <param name="tBase">Type de la que sería la clase base</param>
        /// <param name="t">Type de la que sería la clase child</param>
        /// <returns>True si tBase es un Type base de t</returns>
        public static bool Inherits(Type tBase, Type t)
        {
            if (t == null)
                return false;

            if (t == tBase)
                return true;

            if (tBase.IsInterface)
            {
                Type[] interfaces = t.GetInterfaces();
                if (interfaces.Length > 0)
                {
                    List<Type> listInterfaces = new List<Type>(interfaces);
                    if (listInterfaces.Contains(tBase))
                    {
                        return true;
                    }
                }
            }

            if (t.IsGenericType)
            {
                Type tDefinition = t.GetGenericTypeDefinition();
                if (tDefinition == tBase)
                    return true;
            }

            return Inherits(tBase, t.BaseType);
        }
        #endregion


        #region Getters y Setters para Bindeo Custom
        /// <summary>
        /// Obtiene un Getter
        /// </summary>
        /// <param name="objeto">Objeto</param>
        /// <param name="atributo">Atributo del objeto, o path completo del atributo</param>
        /// <returns></returns>
        public static Func<object, object> GetGetter(object objeto, string atributo)
        {
            if (objeto == null || string.IsNullOrEmpty(atributo))
                return null;
            return GetGetter(objeto, atributo.Split('.'));
        }

        /// <summary>
        /// Obtiene un Getter
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="atributo"></param>
        /// <returns></returns>
        internal static Func<object, object> GetGetter(object objeto, string[] atributo)
        {
            // Si es un type dinámico
            if (objeto is CustomTypeDescriptor)
            {
                return PropertyDescriptorUtil.GetPropertyDescriptorGetter(objeto, atributo);
            }
            return PropertyLambdaExpressionUtil.GetPropertyLambdaGetter(objeto, atributo);
        }

        /// <summary>
        /// Obtiene un Setter
        /// </summary>
        /// <param name="objeto">Objeto</param>
        /// <param name="atributo">Atributo del objeto, o path completo del atributo</param>
        /// <returns></returns>
        public static Action<object, object> GetSetter(object objeto, string atributo)
        {
            if (objeto == null || string.IsNullOrEmpty(atributo))
                return null;
            return GetSetter(objeto, atributo.Split('.'));

        }

        /// <summary>
        /// Obtiene un Setter
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="atributo"></param>
        /// <returns></returns>
        internal static Action<object, object> GetSetter(object objeto, string[] atributo)
        {
            // Si es un type dinámico
            if (objeto is CustomTypeDescriptor)
            {
                return PropertyDescriptorUtil.GetPropertyDescriptorSetter(objeto, atributo);
            }
            return PropertyLambdaExpressionUtil.GetPropertyLambdaSetter(objeto, atributo);
        }

        /// <summary>
        /// Obtiene un descriptor de propiedades para un objeto y propiedad específicos
        /// </summary>
        /// <param name="objectToBind"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static ICustomPropertyDescriptor GetCustomPropertyDescriptor(
            object objectToBind,
            string propertyName)
        {
            if (objectToBind == null || string.IsNullOrEmpty(propertyName))
                return null;

            // Obtiene el Setter para la propiedad
            Action<object, object> setter = GetSetter(objectToBind, propertyName);

            // Obtiene el Getter para la propiedad
            Func<object, object> getter = GetGetter(objectToBind, propertyName);
            
            CustomPropertyDescriptor cpd =
                new CustomPropertyDescriptor(propertyName, setter, getter);
            return cpd;
        }

        internal static readonly MethodInfo ParseValueMethod = typeof(Util).GetMethod("ParseValue");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actualPropertyDescriptor"></param>
        /// <param name="objectToBind"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static ICustomPropertyDescriptor GetCustomPropertyDescriptor
            (
            ICustomPropertyDescriptor actualPropertyDescriptor,
            object objectToBind,
            string propertyName
            )
        {
            return actualPropertyDescriptor ?? GetCustomPropertyDescriptor(objectToBind, propertyName);
        }
        #endregion

        /// <summary>
        /// Gets the type of the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="expr">The expr.</param>
        /// <returns></returns>
        public static Type GetObjectType<T, TProperty>(Expression<Func<T, TProperty>> expr)
        {
            if ((expr.Body.NodeType == ExpressionType.Convert) ||
                (expr.Body.NodeType == ExpressionType.ConvertChecked))
            {
                var unary = expr.Body as UnaryExpression;
                if (unary != null)
                    return unary.Operand.Type;
            }
            return expr.Body.Type;
        }

        /// <summary>
        /// Determines whether the specified type is enumerable.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the specified type is enumerable; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEnumerable(this Type type)
        {
            if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition().GetInterfaces().Contains(typeof(IEnumerable)))
                    return true;
            }
            return false;
        }


        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        public static PropertyInfo GetProperty(LambdaExpression selector)
        {
            Expression body = selector;
            if (body is LambdaExpression)
            {
                body = ((LambdaExpression)body).Body;
            }
            switch (body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return (PropertyInfo)((MemberExpression)body).Member;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}