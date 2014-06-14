using System;
using System.Collections;
using System.Collections.Generic;

namespace SIGEPROJ.BaseClasses.Mappers.TypeMappers
{
    /// <summary>
    /// Clase <see cref="DictionaryToDictionaryTypeMapper&lt;TSource, TTarget&gt;"/>
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    public class DictionaryToDictionaryTypeMapper<TSource, TTarget> : IMemberInjection
        where TSource : class, IDictionary
        where TTarget : class, IDictionary
    {
        /// <summary>
        /// Maps el source especificado.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public TTarget Map(TSource source, TTarget target)
        {
            if (source == null) return null;
            var targetArgs = typeof (TTarget).GetGenericArguments();
            var targetArgumentKeyType = targetArgs[0];
            var targetArgumentValueType = targetArgs[1];

            //var sourceArgs = typeof(TSource).GetGenericArguments();
            //var sourceArgumentKeyType = sourceArgs[0];
            //var sourceArgumentValueType = sourceArgs[1];

            var list = (IDictionary)Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(
                targetArgumentKeyType,
                targetArgumentValueType
                                                    ));

            //var sourceKeyValuePairType = typeof(KeyValuePair<,>).MakeGenericType(
            //    sourceArgumentKeyType,
            //    sourceArgumentValueType
            //    );

            //var add = list.GetType().GetMethod("Add");

            //var key = sourceKeyValuePairType.GetProperty("Key").GetGetMethod();
            //var value = sourceKeyValuePairType.GetProperty("Value").GetGetMethod();
            
            
            foreach (DictionaryEntry o in source)
            {
                var oKey = o.Key; // key.Invoke(o, new object[0]);
                var oValue = o.Value; // value.Invoke(o, new object[0]);
                
                var tKey = ObjectCreator.Create(targetArgumentKeyType);
                var tValue = ObjectCreator.Create(targetArgumentValueType);
                //add.Invoke(list, new[]
                //                     {
                //                         Mapper.Map(oKey, tKey, oKey.GetType(), targetArgumentKeyType),
                //                         Mapper.Map(oValue, tValue, oValue.GetType(), targetArgumentValueType)
                //                     });

                list.Add(
                    Mapper.Map(oKey, tKey, oKey.GetType(), targetArgumentKeyType),
                    Mapper.Map(oValue, tValue, oValue.GetType(), targetArgumentValueType)
                    
                    );
            }
            return (TTarget)list;
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
            return Map((TSource)source, (TTarget)target);
        }

        /// <summary>
        /// Lanza una excepción si es que algún mapping es incorrecto.
        /// </summary>
        public void AssertConfigurationIsValid()
        {
        }
    }
}