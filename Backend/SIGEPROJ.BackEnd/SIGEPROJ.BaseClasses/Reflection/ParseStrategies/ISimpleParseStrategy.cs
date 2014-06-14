using System;

namespace SIGEPROJ.BaseClasses.Reflection.ParseStrategies
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISimpleParseStrategy
    {
        /// <summary>
        /// Indica si aplica esta estrategia
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        bool Applies(Type type);

        /// <summary>
        /// Parsea el valor y lo devuelve
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        object Parse(Type type, object value);
    }
}