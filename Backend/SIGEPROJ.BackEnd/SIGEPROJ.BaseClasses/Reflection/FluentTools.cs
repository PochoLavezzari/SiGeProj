using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace SIGEPROJ.BaseClasses.Reflection
{
    /// <summary>
    /// Based on http://stackoverflow.com/questions/7723744/expressionfunctmodel-string-to-expressionactiontmodel-getter-to-sette
    /// </summary>
    public static class FluentTools
    {
        /// <summary>
        /// Convierte una expression Getter a una función setter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="getter"></param>
        /// <returns></returns>
        public static Action<T, TValue> GetterToSetter<T, TValue>(Expression<Func<T, TValue>> getter)
        {
            ParameterExpression parameter;
            Expression instance;
            MemberExpression propertyOrField;

            GetMemberExpression(getter, out parameter, out instance, out propertyOrField);

            if ((parameter == null) && (instance == null) && (propertyOrField == null))
                return null;

            // Very simple case: p => p.Property or p => p.Field
            if (parameter == instance)
            {
                if (propertyOrField.Member.MemberType == MemberTypes.Property)
                {
                    // This is FASTER than Expression trees! (5x on my benchmarks) but works only on properties
                    PropertyInfo property = propertyOrField.Member as PropertyInfo;
                    if (property != null && property.CanWrite && property.GetSetMethod() != null)
                    {
                        MethodInfo setter = property.GetSetMethod();

                        //Delegate.CreateDelegate(typeof(Action<T, TValue>), setter);
                        //Delegate.CreateDelegate(typeof (ActionReference<T, TValue>), setter);
                        

                        Action<T, TValue> action;

                        if(typeof(T).IsValueType)
                        {
                            // Si es un reference type, hay que pasarlo por parámetro ref.
                            var actionReference = (ActionReference<T, TValue>)Delegate.CreateDelegate(typeof(ActionReference<T, TValue>), setter);
                            action = (t, val) =>
                                         {
                                             actionReference(ref t, val);
                                         };

                        }
                        else
                            action = (Action<T, TValue>)Delegate.CreateDelegate(typeof(Action<T, TValue>), setter);
                        return action;
                    }
                    return null;
                }
                #region .NET 3.5
                else // if (propertyOrField.Member.MemberType == MemberTypes.Field)
                {
                    // 1.2x slower than 4.0 method, 5x faster than 3.5 method
                    FieldInfo field = propertyOrField.Member as FieldInfo;
                    var action = FieldSetter<T, TValue>(field);
                    return action;
                }
                #endregion
            }

            ParameterExpression value = Expression.Parameter(typeof(TValue), "val");

            Expression expr = null;

            #region .NET 3.5
            if (propertyOrField.Member.MemberType == MemberTypes.Property)
            {
                PropertyInfo property = propertyOrField.Member as PropertyInfo;
                // Debe permitir el seteo de valores
                if (property != null && property.CanWrite && property.GetSetMethod() != null)
                {
                    MethodInfo setter = property.GetSetMethod();
                    expr = Expression.Call(instance, setter, value);
                }
                else
                {
                    return null;
                }
            }
            else // if (propertyOrField.Member.MemberType == MemberTypes.Field)
            {
                expr = FieldSetter(propertyOrField, value);
            }
            #endregion

            //#region .NET 4.0
            //// For field access it's 5x faster than the 3.5 method and 1.2x than "simple" method. For property access nearly same speed (1.1x faster).
            //expr = Expression.Assign(propertyOrField, value);
            //#endregion

            return Expression.Lambda<Action<T, TValue>>(expr, parameter, value).Compile();
        }

        /// <summary>
        /// Convierte una expression Getter a una función setter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="getter"></param>
        /// <returns></returns>
        public static ActionReference<T, TValue> GetterToSetterByReference<T, TValue>(Expression<Func<T, TValue>> getter)
        {
            ParameterExpression parameter;
            Expression instance;
            MemberExpression propertyOrField;

            GetMemberExpression(getter, out parameter, out instance, out propertyOrField);

            if ((parameter == null) && (instance == null) && (propertyOrField == null))
                return null;

            // Very simple case: p => p.Property or p => p.Field
            if (parameter == instance)
            {
                if (propertyOrField.Member.MemberType == MemberTypes.Property)
                {
                    // This is FASTER than Expression trees! (5x on my benchmarks) but works only on properties
                    PropertyInfo property = propertyOrField.Member as PropertyInfo;
                    if (property != null && property.CanWrite && property.GetSetMethod() != null)
                    {
                        MethodInfo setter = property.GetSetMethod();

                        //Delegate.CreateDelegate(typeof(Action<T, TValue>), setter);
                        //Delegate.CreateDelegate(typeof (ActionReference<T, TValue>), setter);


                        ActionReference<T, TValue> action;

                        //if (typeof(T).IsValueType)
                        //{
                        //    // Si es un reference type, hay que pasarlo por parámetro ref.
                        //    var actionReference = (ActionReference<T, TValue>)Delegate.CreateDelegate(typeof(ActionReference<T, TValue>), setter);
                        //    action = (t, val) =>
                        //    {
                        //        actionReference(ref t, val);
                        //    };

                        //}
                        //else
                            action = (ActionReference<T, TValue>)Delegate.CreateDelegate(typeof(ActionReference<T, TValue>), setter);
                        return action;
                    }
                    return null;
                }
                #region .NET 3.5
                else // if (propertyOrField.Member.MemberType == MemberTypes.Field)
                {
                    // 1.2x slower than 4.0 method, 5x faster than 3.5 method
                    FieldInfo field = propertyOrField.Member as FieldInfo;
                    var action = FieldSetterReference<T, TValue>(field);
                    return action;
                }
                #endregion
            }

            ParameterExpression value = Expression.Parameter(typeof(TValue), "val");

            Expression expr = null;

            #region .NET 3.5
            if (propertyOrField.Member.MemberType == MemberTypes.Property)
            {
                PropertyInfo property = propertyOrField.Member as PropertyInfo;
                // Debe permitir el seteo de valores
                if (property != null && property.CanWrite && property.GetSetMethod() != null)
                {
                    MethodInfo setter = property.GetSetMethod();
                    expr = Expression.Call(instance, setter, value);
                }
                else
                {
                    return null;
                }
            }
            else // if (propertyOrField.Member.MemberType == MemberTypes.Field)
            {
                expr = FieldSetter(propertyOrField, value);
            }
            #endregion

            //#region .NET 4.0
            //// For field access it's 5x faster than the 3.5 method and 1.2x than "simple" method. For property access nearly same speed (1.1x faster).
            //expr = Expression.Assign(propertyOrField, value);
            //#endregion

            return Expression.Lambda<ActionReference<T, TValue>>(expr, parameter, value).Compile();
        }



        /// <summary>
        /// Obtiene el parámetro principal de la función.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="expression"></param>
        /// <param name="parameter"></param>
        /// <param name="instance"></param>
        /// <param name="propertyOrField"></param>
        private static void GetMemberExpression<T, U>(Expression<Func<T, U>> expression, out ParameterExpression parameter, out Expression instance, out MemberExpression propertyOrField)
        {
            Expression current = expression.Body;

            while (current.NodeType == ExpressionType.Convert || current.NodeType == ExpressionType.TypeAs)
            {
                current = (current as UnaryExpression).Operand;
            }

            // Caso obj => obj // No tiene Setter
            if(current.NodeType == ExpressionType.Parameter)
            {
                instance = null;
                parameter = null;
                propertyOrField = null;
                return;
            }

            if (current.NodeType != ExpressionType.MemberAccess)
            {
                throw new ArgumentException();
            }

            propertyOrField = current as MemberExpression;
            current = propertyOrField.Expression;

            instance = current;

            while (current.NodeType != ExpressionType.Parameter)
            {
                if (current.NodeType == ExpressionType.Convert || current.NodeType == ExpressionType.TypeAs)
                {
                    current = (current as UnaryExpression).Operand;
                }
                else if (current.NodeType == ExpressionType.MemberAccess)
                {
                    current = (current as MemberExpression).Expression;
                }
                else if (current.NodeType == ExpressionType.Call)
                {
                    current = (current as MethodCallExpression).Object;
                    //current = null;
                }
                else if (current.NodeType == ExpressionType.Invoke)
                {
                    current = (current as InvocationExpression).Expression;
                    //current = null;
                }
                else
                {
                    throw new ArgumentException();
                }
            }

            parameter = current as ParameterExpression;
        }

        #region .NET 3.5

        /// <summary>
        /// Based on http://stackoverflow.com/questions/321650/how-do-i-set-a-field-value-in-an-c-expression-tree/321686#321686
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        private static Action<T, TValue> FieldSetter<T, TValue>(FieldInfo field)
        {
            DynamicMethod m = new DynamicMethod("setter", typeof(void), new Type[] { typeof(T), typeof(TValue) }, typeof(FluentTools));
            ILGenerator cg = m.GetILGenerator();

            // arg0.<field> = arg1
            cg.Emit(OpCodes.Ldarg_0);
            cg.Emit(OpCodes.Ldarg_1);
            cg.Emit(OpCodes.Stfld, field);
            cg.Emit(OpCodes.Ret);

            return (Action<T, TValue>)m.CreateDelegate(typeof(Action<T, TValue>));
        }

        /// <summary>
        /// Based on http://stackoverflow.com/questions/321650/how-do-i-set-a-field-value-in-an-c-expression-tree/321686#321686
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        private static ActionReference<T, TValue> FieldSetterReference<T, TValue>(FieldInfo field)
        {
            DynamicMethod m = new DynamicMethod("setter", typeof(void), new Type[] { typeof(T), typeof(TValue) }, typeof(FluentTools));
            ILGenerator cg = m.GetILGenerator();

            // arg0.<field> = arg1
            cg.Emit(OpCodes.Ldarg_0);
            cg.Emit(OpCodes.Ldarg_1);
            cg.Emit(OpCodes.Stfld, field);
            cg.Emit(OpCodes.Ret);

            return (ActionReference<T, TValue>)m.CreateDelegate(typeof(ActionReference<T, TValue>));
        }

        /// <summary>
        /// Based on http://stackoverflow.com/questions/208969/assignment-in-net-3-5-expression-trees/3972359#3972359
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static Expression FieldSetter(Expression left, Expression right)
        {
            return
                Expression.Call(
                    null,
                    typeof(FluentTools)
                        .GetMethod("AssignTo", BindingFlags.NonPublic | BindingFlags.Static)
                        .MakeGenericMethod(left.Type),
                    left,
                    right);
        }

        /// <summary>
        /// asigna un valor
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        private static void AssignTo<T>(ref T left, T right)  // note the 'ref', which is
        {                                                     // important when assigning
            left = right;                                     // to value types!
        }

        #endregion
    }

    /// <summary>
    /// Action utilizado para elementos de tipo ValueType que deben ser referenciados.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="target">The target.</param>
    /// <param name="value">The value.</param>
    public delegate void ActionReference<T, TValue>(ref T target, TValue value);

    /// <summary>
    /// Action utilizado para elementos de tipo ValueType que deben ser referenciados.
    /// </summary>
    /// <param name="target">The target.</param>
    /// <param name="value">The value.</param>
    public delegate void ActionReference(ref object target, object value);
}