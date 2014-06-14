using System;
using System.Collections;

namespace SIGEPROJ.BaseClasses.Mappers.TypeMappers
{
    /// <summary>
    /// Clase <see cref="ArrayToArrayTypeMapper&lt;TSource, TTarget&gt;"/>
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    public class ArrayToArrayTypeMapper<TSource, TTarget> : BaseTypeMapper<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
        /// <summary>
        /// Maps el source especificado.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public override TTarget Map(TSource source, TTarget target)
        {
            if (source == null) return null;
            var targetArgumentType = typeof(TTarget).GetElementType();
            IList auxObjects = new ArrayList();

            foreach (var o in (Array) (object)source)
            {
                var t = ObjectCreator.Create(targetArgumentType);
                auxObjects.Add(Mapper.Map(o, t, o.GetType(), targetArgumentType));

            }

            var array = ObjectCreator.CreateArray(targetArgumentType, auxObjects.Count);

            for (int i = 0; i < auxObjects.Count; i++)
            {
                array.SetValue(auxObjects[i], i);
            }

            return (TTarget)(object)array;
        }
    }
}