using System;

namespace SIGEPROJ.BaseClasses.Reflection.ParseStrategies
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstractSimpleParseStrategy : ISimpleParseStrategy
    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly Type appliesToType;
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractSimpleParseStrategy"/> class.
        /// </summary>
        /// <param name="appliesToType">Type of the applies to.</param>
        protected AbstractSimpleParseStrategy(Type appliesToType)
        {
            this.appliesToType = appliesToType;
        }
        /// <summary>
        /// Indica si aplica esta estrategia
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public bool Applies(Type type)
        {
            return type == appliesToType;
        }

        /// <summary>
        /// Parsea el valor y lo devuelve
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract object Parse(Type type, object value);
    }
}