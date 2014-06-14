using System;
using System.Linq.Expressions;

namespace SIGEPROJ.BaseClasses.Mappers
{
    /// <summary>
    /// Mapping Configuration Factory, will be used to define what to do with the property
    /// </summary>
    public class MappingFactory<TSource, TTarget, TTargetProperty>
    {
        #region ctor
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="MappingFactory&lt;TSource, TTarget, TTargetProperty&gt;"/> .
        /// </summary>
        public MappingFactory()
            : this(new PropInfo())
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="MappingFactory&lt;TSource, TTarget, TTargetProperty&gt;"/>.
        /// </summary>
        /// <param name="propInfo">The prop info.</param>
        public MappingFactory(PropInfo propInfo)
        {
            TargetProperty = propInfo;
        }
        #endregion

        /// <summary>
        /// Gets or sets the target property.
        /// </summary>
        /// <value>
        /// The target property.
        /// </value>
        public PropInfo TargetProperty { get; set; }

        /// <summary>
        /// Call if you want to ignore the property
        /// </summary>
        public IgnoreMapping Ignore()
        {
            return new IgnoreMapping();
        }

        /// <summary>
        /// Maps the property with the given mapping function
        /// TTargetProperty == TTSourceProperty
        /// </summary>
        /// <param name="sourceMappingFunction"></param>
        public FunctionMapping<TSource, TTarget, TTargetProperty, TTargetProperty> MapFrom(
            Expression<Func<TSource, TTargetProperty>> sourceMappingFunction)
        {
            var func = new FunctionMapping<TSource, TTarget, TTargetProperty, TTargetProperty>(
                sourceMappingFunction,
                TargetProperty);
            return func;
        }

        /// <summary>
        /// Maps from.
        /// </summary>
        /// <typeparam name="TSourceProperty">The type of the source property.</typeparam>
        /// <param name="sourceMappingFunction">The source mapping function.</param>
        /// <returns></returns>
        public ConvertFunctionMapping<TSource, TTarget, TSourceProperty, TTargetProperty> MapFrom<TSourceProperty>(
            Expression<Func<TSource, TSourceProperty>> sourceMappingFunction)
        {
            // Acá se debe pasar el Function mapping que sabe como convertir de un lado a otro
            var func = new ConvertFunctionMapping<TSource, TTarget, TSourceProperty,TTargetProperty>(
                sourceMappingFunction, TargetProperty
                );
            return func;
        }

        /// <summary>
        /// Use a defined value to map the source property
        /// </summary>
        /// <param name="value">The target value.</param>
        public UseValueMapping<TSource,TTarget,TTargetProperty> UseValue(TTargetProperty value)
        {
            return new UseValueMapping<TSource, TTarget, TTargetProperty>(value, TargetProperty);
        }

        /// <summary>
        /// Use a defined function to map the source property
        /// </summary>
        /// <param name="sourceValueFunction">The source value function.</param>
        /// <returns></returns>
        public UseValueMapping<TSource, TTarget, TTargetProperty> UseValue(Func<TSource,TTargetProperty> sourceValueFunction)
        {
            return new UseValueMapping<TSource, TTarget, TTargetProperty>(sourceValueFunction, TargetProperty);
        }
    }
}