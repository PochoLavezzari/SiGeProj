using System;
using System.ComponentModel;

namespace SIGEPROJ.BaseClasses.Mappers.TypeMappers
{
    /// <summary>
    /// Clase <see cref="TypeConverterMapper&lt;TSource, TTarget&gt;"/>
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    public class TypeConverterMapper<TSource, TTarget> : BaseTypeMapper<TSource, TTarget>
    {
        /// <summary>
        /// Maps el source especificado.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public override TTarget Map(TSource source, TTarget target)
        {
            if (source == null)
            {
                if(typeof(TTarget).IsValueType)
                    return (TTarget)ObjectCreator.Create(typeof(TTarget));
                return (TTarget)(object)null;
            }
            return (TTarget) GetTypeConverter(typeof(TSource)).ConvertTo(source, typeof(TTarget));
        }

        /// <summary>
        /// Gets the type converter.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <returns></returns>
        private static TypeConverter GetTypeConverter(Type sourceType)
        {
            return TypeDescriptor.GetConverter(sourceType);
        }
    }
}