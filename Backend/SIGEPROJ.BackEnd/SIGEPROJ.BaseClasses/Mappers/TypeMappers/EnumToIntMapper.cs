using System;
using System.ComponentModel;
using System.Reflection;

namespace SIGEPROJ.BaseClasses.Mappers.TypeMappers
{
    /// <summary>
    /// Clase <see cref="EnumToIntMapper"/>
    /// </summary>
    public class EnumToIntMapper : IMemberInjection
    {
         /// <summary>
        /// 
        /// </summary>
        private static readonly EnumToIntMapper instance = new EnumToIntMapper();
        /// <summary>
        /// Gets the default.
        /// </summary>
        public static EnumToIntMapper Default
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="EnumToStringMapper"/> class from being created.
        /// </summary>
        private EnumToIntMapper() { }

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
            return IntValueOf((Enum) source);
        }

        /// <summary>
        /// Lanza una excepción si es que algún mapping es incorrecto.
        /// </summary>
        public void AssertConfigurationIsValid()
        {
        }

        /// <summary>
        /// Retorna el valor Int de un enumerado
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int IntValueOf(Enum value)
        {
            if (value == null)
                return default(int);
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
            {
                return int.Parse(attributes[0].Description);
            }
            return Convert.ToInt32(value);
        }
    }
}