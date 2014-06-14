using System;
using System.Linq.Expressions;

namespace SIGEPROJ.BaseClasses.Mappers
{
    /// <summary>
    /// This mapping is used to map a property against a user defined function
    /// </summary>
    /// <typeparam name="TSource">Source type</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    /// <typeparam name="TSourceProperty">Source property Type</typeparam>
    /// <typeparam name="TTargetProperty">Target property Type</typeparam>
    /// ///
    public class ConvertFunctionMapping<TSource, TTarget, TSourceProperty,TTargetProperty> :
        FunctionMapping<TSource, TTarget, TSourceProperty, TTargetProperty>
    {
        #region Inverse for TwoWay

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ConvertFunctionMapping&lt;TSource, TTarget, TSourceProperty, TTargetProperty&gt;"/> .
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="t">The t.</param>
        public ConvertFunctionMapping(PropInfo s, PropInfo t)
            : base(s, t)
        {
            Converter = GetConverter();
        }


        /// <summary>
        /// Convierte el Mapping actual a su inverso.
        /// </summary>
        /// <returns></returns>
        public override MappingConfiguration ConvertToInverse()
        {
            return new ConvertFunctionMapping<TTarget, TSource, TTargetProperty, TSourceProperty>(
                TargetProperty,
                SourceProperty
                );
        }
        #endregion

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ConvertFunctionMapping&lt;TSource, TTarget, TSourceProperty, TTargetProperty&gt;"/>.
        /// </summary>
        /// <param name="mappingFunction">The mapping function.</param>
        /// <param name="targetProperty">The target property.</param>
        public ConvertFunctionMapping(
            Expression<Func<TSource, TSourceProperty>> mappingFunction,
            PropInfo targetProperty)
            : base(PropInfo.FillProperty(mappingFunction), targetProperty)
        {
            // Si es una propiedad "this", se debe mapear 
            if (targetProperty.Setter == null && targetProperty.Name == "this")
            {
                targetProperty.Setter =
                    (target, value) => Mapper.Map(value, target,
                        typeof(TSourceProperty), TargetProperty.ParentType);
            }
            Converter = GetConverter();
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
            object convertedValue;

            if (TargetProperty.Name == "this")
            {
                convertedValue = target;
                TargetProperty.Setter(convertedValue, value);
            }
            else
            {

                convertedValue = Converter(value);

                if (TargetProperty.Setter != null)
                    TargetProperty.Setter(target, convertedValue);
                else
                {
                    throw new NullReferenceException(string.Format("La property \"{0}\" de la clase {1} no contiene un Setter válido", TargetProperty.Name, typeof(TTarget)));
                }
            }
            return convertedValue;
        }

        /// <summary>
        /// Gets or sets the converter.
        /// </summary>
        /// <value>
        /// The converter.
        /// </value>
        protected Func<object, object> Converter { get; set; }

        /// <summary>
        /// Gets the converter.
        /// </summary>
        /// <returns></returns>
        protected Func<object, object> GetConverter()
        {
            Type targetProperty = typeof(TTargetProperty);
            //Type sourceProperty = typeof(TSourceProperty);
            // Si es de otro tipo, se debe utilizar el mapper
            Func<object, object> convertedFunction =
                source =>
                {
                    TSourceProperty sourcePropertyValue = (TSourceProperty)source;
                    //if (Equals(sourcePropertyValue, default(TSourceProperty)))
                    if( sourcePropertyValue == null)
                        return Reflection.Util.DefaultForType(targetProperty);
                    return Mapper.Map<TSourceProperty, TTargetProperty>(sourcePropertyValue);


                };
            return convertedFunction;

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