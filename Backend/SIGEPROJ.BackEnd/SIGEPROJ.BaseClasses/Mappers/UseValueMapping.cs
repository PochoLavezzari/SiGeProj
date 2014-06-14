using System;
using SIGEPROJ.BaseClasses.Mappers.ValueInjecter;

namespace SIGEPROJ.BaseClasses.Mappers
{
    /// <summary>
    /// This mapping sets the given target value to the mapped object
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    /// <typeparam name="TTargetProperty">Value to use for the target property</typeparam>
    public class UseValueMapping<TSource,TTarget,TTargetProperty> : MappingConfiguration, IValueInjection
    {

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="UseValueMapping&lt;TSource, TTarget, TTargetProperty&gt;"/>.
        /// </summary>
        /// <param name="targetValue">The target value.</param>
        /// <param name="targetProperty">The target property.</param>
        public UseValueMapping(TTargetProperty targetValue, PropInfo targetProperty)
            : base(new PropInfo(), targetProperty)
        {
            TargetValue = targetValue;

        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="UseValueMapping&lt;TSource, TTarget, TTargetProperty&gt;"/>.
        /// </summary>
        /// <param name="targetValue">The target value.</param>
        /// <param name="targetProperty">The target property.</param>
        public UseValueMapping(Func<TSource,TTargetProperty> targetValue, PropInfo targetProperty)
            : base(new PropInfo(), targetProperty)
        {
            TargetFuncValue = targetValue;

        }

        /// <summary>
        /// Gets or sets the target value.
        /// </summary>
        /// <value>The target value.</value>
        public TTargetProperty TargetValue { get; private set; }

        /// <summary>
        /// Gets the target func value.
        /// </summary>
        public Func<TSource, TTargetProperty> TargetFuncValue { get; private set; }


        private TTargetProperty GetTargetValue(TSource source)
        {
            if (TargetFuncValue != null)
                return TargetFuncValue(source);
            return TargetValue;
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
            TTargetProperty targetValue = GetTargetValue((TSource) source);
            TargetProperty.Setter(target, targetValue);
            return targetValue;
        }
    }
}