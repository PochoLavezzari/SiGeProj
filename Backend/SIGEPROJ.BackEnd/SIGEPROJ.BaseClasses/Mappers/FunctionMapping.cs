using System;
using System.Linq.Expressions;

namespace SIGEPROJ.BaseClasses.Mappers
{
    /// <summary>
    /// This mapping is used to map a property against a user defined function
    /// </summary>
    /// <typeparam name="TSource">Source type</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    /// <typeparam name="TSourceProperty">The type of the source property.</typeparam>
    /// <typeparam name="TTargetProperty">Target property Type</typeparam>
    public class FunctionMapping<TSource, TTarget, TSourceProperty, TTargetProperty> :
        OneWayConfiguration<FunctionMapping<TSource, TTarget, TSourceProperty, TTargetProperty>>
        , ITwoWayMappingConverter
    {
        #region Inverse for TwoWayBinding
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="FunctionMapping&lt;TSource, TTarget, TSourceProperty, TTargetProperty&gt;"/>.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="t">The t.</param>
        protected FunctionMapping(PropInfo s, PropInfo t)
            : base(s, t)
        {
        }

        /// <summary>
        /// Convierte el Mapping actual a su inverso.
        /// </summary>
        /// <returns></returns>
        public virtual MappingConfiguration ConvertToInverse()
        {
            return new FunctionMapping<TTarget, TSource, TTargetProperty,TSourceProperty>(TargetProperty, SourceProperty);
        }
        #endregion

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="FunctionMapping&lt;TSource, TTarget, TSourceProperty, TTargetProperty&gt;"/>.
        /// </summary>
        /// <param name="targetProperty">The target property.</param>
        protected FunctionMapping(PropInfo targetProperty)
            : base(new PropInfo(), targetProperty)
        {

        }


        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="FunctionMapping&lt;TSource, TTarget, TSourceProperty, TTargetProperty&gt;"/>.
        /// </summary>
        /// <param name="sourceMappingFunction">The source mapping function.</param>
        /// <param name="targetProperty">The target property.</param>
        public FunctionMapping(
            Expression<Func<TSource, TTargetProperty>> sourceMappingFunction,
            PropInfo targetProperty
            )
            : base(
            PropInfo.FillProperty(sourceMappingFunction)
            , targetProperty)
        {
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public override object SetValue(object source, object target)
        {
            object value = SourceProperty.Getter(source);
            TargetProperty.Setter(target, value);
            return value;
        }

        /// <summary>
        /// True si puede convertirse al inverso.
        /// </summary>
        /// <returns></returns>
        public virtual bool CanConvert()
        {
            return !IsOneWay;
        }
    }
}