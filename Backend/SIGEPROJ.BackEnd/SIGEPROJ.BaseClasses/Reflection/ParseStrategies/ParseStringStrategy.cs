using System;

namespace SIGEPROJ.BaseClasses.Reflection.ParseStrategies
{
    /// <summary>
    /// Parsea un string
    /// </summary>
    public class ParseStringStrategy : AbstractSimpleParseStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParseStringStrategy"/> class.
        /// </summary>
        public ParseStringStrategy() : base(typeof(string)){ }

        /// <summary>
        /// Parsea el valor y lo devuelve
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object Parse(Type type, object value)
        {
            if (value != null && value.GetType() != appliesToType)
            {
                return value.ToString();
            }
            return null;
        }
    }
}