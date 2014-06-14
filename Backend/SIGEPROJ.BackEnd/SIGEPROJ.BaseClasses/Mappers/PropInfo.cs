using System;
using System.Linq.Expressions;
using SIGEPROJ.BaseClasses.Reflection;

namespace SIGEPROJ.BaseClasses.Mappers
{
    /// <summary>
    /// Clase que contiene la información necesaria para mapear una property.
    /// </summary>
    public class PropInfo : ICloneable
    {
        /// <summary>
        /// Type del objeto que permite obtener esta property
        /// </summary>
        public Type ParentType { get; set; }

        /// <summary>
        /// Nombre de la property.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type de la property.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Función que permite obtener el valor actual.
        /// </summary>
        public Func<object, object> Getter { get; set; }

        /// <summary>
        /// Action que permite setearle un valor.
        /// </summary>
        public Action<object, object> Setter { get; set; }

        /// <summary>
        /// Action que permite setearle un valor a un elemento de tipo ValueType.
        /// </summary>
        /// <value>
        /// The setter reference.
        /// </value>
        public ActionReference<object, object> SetterReference { get; set; }

        /// <summary>
        /// Expression Inicial.
        /// </summary>
        public LambdaExpression Expression { get; set; }

        /// <summary>
        /// Fills the property.
        /// </summary>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <typeparam name="TPropertyTarget">The type of the property target.</typeparam>
        /// <param name="targetPropExpr">The target prop expr.</param>
        /// <returns></returns>
        public static PropInfo FillProperty<TTarget, TPropertyTarget>(
            Expression<Func<TTarget, TPropertyTarget>> targetPropExpr)
        {
            PropInfo targetProp = new PropInfo();

            targetProp.ParentType = typeof (TTarget);

            targetProp.Expression = targetPropExpr;

            // Obtiene el nombre de la propiedad
            string targetPropertyName = Util.GetPropertyName(targetPropExpr);
            // Obtiene el Type de la propiedad
            Type targetPropertyType = Util.GetObjectType(targetPropExpr);

            // Obtiene el Getter
            var targetGetter = targetPropExpr.Compile();
            Func<object, object> getter = target => (object) targetGetter((TTarget) target);

            // Carga la info de la TargetProperty
            targetProp.Name = targetPropertyName;
            targetProp.Type = targetPropertyType;
            targetProp.Getter = getter;

            // Convierte el Getter en Setter
            if (typeof(TTarget).IsValueType)
            {
                var targetSetter = FluentTools.GetterToSetterByReference(targetPropExpr);
                if(targetSetter != null)
                {
                    targetProp.SetterReference = delegate(ref object target, object value)
                                                     {
                                                         TTarget newTarget = (TTarget) target;
                                                         targetSetter(ref newTarget, (TPropertyTarget)value);
                                                         target = newTarget;
                                                     };
                }
            }
            else
            {
                var targetSetter = FluentTools.GetterToSetter(targetPropExpr);

                if (targetSetter != null)
                    targetProp.Setter = (target, value) => targetSetter((TTarget)target, (TPropertyTarget)value);
            }
            

            return targetProp;
        }

        public object Clone()
        {
            PropInfo propInfo = new PropInfo();
            propInfo.Expression = Expression;
            propInfo.Getter = Getter;
            propInfo.Name = Name;
            propInfo.ParentType = ParentType;
            propInfo.Setter = Setter;
            propInfo.SetterReference = SetterReference;
            propInfo.Type = Type;
            return propInfo;
        }
    }
}