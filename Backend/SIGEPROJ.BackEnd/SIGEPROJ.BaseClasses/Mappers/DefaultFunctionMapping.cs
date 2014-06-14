using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SIGEPROJ.BaseClasses.Mappers
{
    /// <summary>
    /// This mapping is used to map a property against a user defined function
    /// </summary>
    /// <typeparam name="TSource">Source type</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    /// <typeparam name="TSourceProperty">The type of the source property.</typeparam>
    /// <typeparam name="TTargetProperty">Target property Type</typeparam>
    public class DefaultFunctionMapping<TSource, TTarget, TSourceProperty, TTargetProperty> :
        OneWayConfiguration<DefaultFunctionMapping<TSource, TTarget, TSourceProperty, TTargetProperty>>,
        ITwoWayMappingConverter
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="DefaultFunctionMapping&lt;TSource, TTargetProperty&gt;"/>.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="t">The t.</param>
        protected DefaultFunctionMapping(PropInfo s, PropInfo t)
        {
            SourceProperty = s;
            TargetProperty = t;
        }

        /// <summary>
        /// Creates the inverse.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="t">The t.</param>
        /// <returns></returns>
        protected static DefaultFunctionMapping<TSource, TTarget, TSourceProperty, TTargetProperty> CreateInverse(PropInfo s, PropInfo t)
        {
            return new DefaultFunctionMapping<TSource, TTarget, TSourceProperty, TTargetProperty>(s, t);
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="DefaultFunctionMapping&lt;TSource, TTarget, TSourceProperty, TTargetProperty&gt;"/>.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="t">The t.</param>
        public DefaultFunctionMapping(PropertyInfo s, PropertyInfo t)
        {
            var origSource = Expression.Parameter(typeof(TSource), "source");
            var origTarget = Expression.Parameter(typeof(TTarget), "target");
            SourceProperty = 
                PropInfo.FillProperty(
                Expression.Lambda<Func<TSource, TSourceProperty>>(
                    Expression.Property(origSource, s), origSource));

            TargetProperty =
                PropInfo.FillProperty(
                Expression.Lambda<Func<TTarget, TTargetProperty>>(
                    Expression.Property(origTarget, t), origTarget));
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
            //if(TargetProperty.SetterReference != null)
            //    TargetProperty.SetterReference(ref target, value);
            return value;
        }

        /// <summary>
        /// True si puede convertirse al inverso.
        /// </summary>
        /// <returns></returns>
        public virtual bool CanConvert()
        {
            // No debe ser OneWay y debe contener un Setter válido.
            return !IsOneWay && SourceProperty.Setter != null;
        }

        /// <summary>
        /// Convierte el Mapping actual a su inverso.
        /// </summary>
        /// <returns></returns>
        public virtual MappingConfiguration ConvertToInverse()
        {
            return new DefaultFunctionMapping<TTarget, TSource, TTargetProperty, TSourceProperty>(TargetProperty, SourceProperty);
        }

        /// <summary>
        /// Permite convertir desde un valor a otro. Sirve para cambiar los valores de los filtros.
        /// </summary>
        /// <param name="targetValue"></param>
        /// <returns></returns>
        public override object ConvertTargetValueToSourceValue(object targetValue)
        {
            Type sourceType = typeof(TSourceProperty);
            if (targetValue == null)
                return Reflection.Util.DefaultForType(sourceType);
            if (targetValue is TSourceProperty)
                return targetValue;
            if (targetValue is TTargetProperty)
                return Mapper.Map<TTargetProperty, TSourceProperty>((TTargetProperty)targetValue);

            object sourceValue = ObjectCreator.Create(sourceType);
            return Mapper.Map(targetValue, sourceValue, targetValue.GetType(), sourceType);
        }
    }
}