using System;
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
    public class DefaultConvertFunctionMapping<TSource, TTarget, TSourceProperty, TTargetProperty> :
        DefaultFunctionMapping<TSource,TTarget, TSourceProperty, TTargetProperty>
    {
        private readonly Type sourcePropertyType;
        private readonly Type targetPropertyType;
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="DefaultConvertFunctionMapping&lt;TSource, TTarget, TSourceProperty, TTargetProperty&gt;"/> .
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="t">The t.</param>
        protected DefaultConvertFunctionMapping(PropInfo s, PropInfo t)
            : base(s, t)
        {
            sourcePropertyType = s.Type;
            targetPropertyType = t.Type;
        }

        /// <summary>
        /// True si puede convertirse al inverso.
        /// </summary>
        /// <returns></returns>
        public override bool CanConvert()
        {
            // No debe ser OneWay y debe contener un Setter válido.
            return !IsOneWay && SourceProperty.Setter != null;
        }

        /// <summary>
        /// Creates the inverse.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="t">The t.</param>
        /// <returns></returns>
        protected new static DefaultConvertFunctionMapping<TSource, TTarget, TSourceProperty, TTargetProperty> CreateInverse(PropInfo s, PropInfo t)
        {
            return new DefaultConvertFunctionMapping<TSource, TTarget, TSourceProperty, TTargetProperty>(s, t);
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="DefaultConvertFunctionMapping&lt;TSource, TTarget, TSourceProperty, TTargetProperty&gt;"/>.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="t">The t.</param>
        public DefaultConvertFunctionMapping(PropertyInfo s, PropertyInfo t)
            : base(s, t)
        {
            sourcePropertyType = s.PropertyType;
            targetPropertyType = t.PropertyType;
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public override object SetValue(object source, object target)
        {
            object propSourceValue = SourceProperty.Getter(source);
            object propTargetValue = TargetProperty.Getter(target);
            if (propSourceValue == null)
                return Reflection.Util.DefaultForType(targetPropertyType);
            if (propTargetValue == null)
                propTargetValue = ObjectCreator.Create(targetPropertyType);
            object convertedValue = Mapper.Map(propSourceValue, propTargetValue, sourcePropertyType, targetPropertyType);
            if (TargetProperty.Setter == null)
                throw new NullReferenceException(string.Format("La propiedad \"{0}\" de la clase \"{1}\" no puede setearse.", TargetProperty.Name, typeof(TTarget).FullName));
            TargetProperty.Setter(target, convertedValue);
            return convertedValue;
        }


        /// <summary>
        /// Convierte el Mapping actual a su inverso.
        /// </summary>
        /// <returns></returns>
        public override MappingConfiguration ConvertToInverse()
        {
            var genericTargetType =
                typeof(DefaultConvertFunctionMapping<,,,>)
                    .MakeGenericType(
                        typeof(TTarget),
                        typeof(TSource),
                        TargetProperty.Type,
                        SourceProperty.Type);
            // evaluate delegate
            var createInverseMethod = genericTargetType.GetMethod("CreateInverse",
                                                                  BindingFlags.Static | BindingFlags.NonPublic);

            var mapper = (MappingConfiguration)createInverseMethod.Invoke(null, new object[]
                                                                                    { 
                                                                                        TargetProperty, SourceProperty
                                                                                    });

            return mapper;
        }
    }
}