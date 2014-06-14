using System;
using System.ComponentModel;
using System.Reflection;
using SIGEPROJ.BaseClasses.Reflection;

namespace SIGEPROJ.BaseClasses.Mappers.TypeMappers
{
    /// <summary>
    /// Clase <see cref="IntToEnumMapper"/>
    /// </summary>
    public class StringToEnumNullableMapper : IMemberInjection
    {
        private readonly Type enumType;
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="IntToEnumMapper"/>.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        public StringToEnumNullableMapper(Type enumType)
        {
            this.enumType = enumType;
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
                return Util.DefaultForType(enumType);

            return EnumValueOf((string)source, enumType);
        }

        /// <summary>
        /// Lanza una excepción si es que algún mapping es incorrecto.
        /// </summary>
        public void AssertConfigurationIsValid()
        {
        }


        /// <summary>
        /// Retorna el enumerado de un string
        /// Si Type es <code>Nullable</code> y el parámetro value es String Vacío o <code>null</code>, el método retorna <code>null</code>
        /// </summary>
        /// <param name="value">valor string</param>
        /// <param name="type">Tipo del enumerado</param>
        /// <returns>el enumerado de un string</returns>
        public static object EnumValueOf(string value, Type type)
        {
            var enumType = type;
            //Esto es porque un Nullable no es un Enumerado. 
            //Entonces se extrae el argumento del Generic que debe ser un tipo enumerado
            if (IsNullableType(type))
            {
                if (String.IsNullOrEmpty(value))
                    return null;

                //Obtengo el argumento del tipo nullable, se espera que se un Enumerado
                enumType = new NullableConverter(type).UnderlyingType;

            }

            string[] names = Enum.GetNames(enumType);
            foreach (string name in names)
            {
                if (StringValueOf((Enum)Enum.Parse(enumType, name)).Equals(value))
                {
                    return Enum.Parse(enumType, name);
                }
            }

            throw new ArgumentException("The string is not a description or value of the specified enum.");
        }

        /// <summary>
        /// Determines whether [is nullable type] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if [is nullable type] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsNullableType(Type type)
        {
            return (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
        }

        /// <summary>
        /// Retorna el valor string de un enumerado
        /// </summary>
        /// <param name="value">enumerado</param>
        /// <returns>el valor string de un enumerado</returns>
        private static string StringValueOf(Enum value)
        {
            if (value == null)
                return null;
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            return value.ToString();
        }
    }
}