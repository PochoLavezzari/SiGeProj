using System;
using System.ComponentModel;
using System.Reflection;

namespace SIGEPROJ.BaseClasses.Mappers.TypeMappers
{
    /// <summary>
    /// Clase <see cref="EnumToIntMapper"/>
    /// </summary>
    public class EnumNullableToStringMapper : IMemberInjection
    {
        /// <summary>
        /// 
        /// </summary>
        private static readonly EnumNullableToStringMapper instance = new EnumNullableToStringMapper();
        /// <summary>
        /// Gets the default.
        /// </summary>
        public static EnumNullableToStringMapper Default
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="EnumToStringMapper"/> class from being created.
        /// </summary>
        private EnumNullableToStringMapper() { }
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
            return StringValueOf((Enum)source);
        }

        /// <summary>
        /// Lanza una excepción si es que algún mapping es incorrecto.
        /// </summary>
        public void AssertConfigurationIsValid()
        {
        }

        /// <summary>
        /// Retorna el valor string de un enumerado
        /// </summary>
        /// <param name="value">enumerado</param>
        /// <returns>el valor string de un enumerado</returns>
        public static string StringValueOf(Enum value)
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