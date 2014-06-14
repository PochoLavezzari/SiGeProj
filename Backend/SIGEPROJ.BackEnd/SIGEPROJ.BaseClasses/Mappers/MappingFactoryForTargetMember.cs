using System;
using System.Linq.Expressions;

namespace SIGEPROJ.BaseClasses.Mappers
{
    /// <summary>
    /// Permite hacer operaciones contra el objeto Target en lugar de hacerlo contra una propiedad Target
    /// </summary>
    public class MappingFactoryForTargetMember<TSource, TTarget>
    {
        #region ctor
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="MappingFactory&lt;TSource, TTarget, TTargetProperty&gt;"/> .
        /// </summary>
        public MappingFactoryForTargetMember()
            : this(new PropInfo())
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="MappingFactoryForTargetMember&lt;TSource, TTarget&gt;"/>.
        /// </summary>
        /// <param name="propInfo">The prop info.</param>
        public MappingFactoryForTargetMember(PropInfo propInfo)
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
        /// Maps the property with the given mapping function
        /// TTargetProperty == TTSourceProperty
        /// </summary>
        /// <param name="sourceMappingFunction"></param>
        public FunctionMapping<TSource, TTarget, TTarget, TTarget> MapFrom(
            Expression<Func<TSource, TTarget>> sourceMappingFunction)
        {
            var func = new FunctionMapping<TSource, TTarget, TTarget, TTarget>(
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
        public ConvertFunctionMapping<TSource, TTarget, TSourceProperty, TTarget> MapFrom<TSourceProperty>(
            Expression<Func<TSource, TSourceProperty>> sourceMappingFunction)
        {
            // Acá se debe pasar el Function mapping que sabe como convertir de un lado a otro
            var func = new ConvertFunctionMapping<TSource, TTarget, TSourceProperty, TTarget>(
                sourceMappingFunction, TargetProperty
                );
            return func;
        }
    }
}