using System;

namespace SIGEPROJ.BaseClasses.Reflection.ParseStrategies
{
    /// <summary>
    /// Parsea un Boolean
    /// </summary>
    public class ParseBoolStrategy : AbstractSimpleParseStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParseFloatStrategy"/> class.
        /// </summary>
        public ParseBoolStrategy() : base(typeof(bool)) { }

        /// <summary>
        /// Parsea el valor y lo devuelve
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object Parse(Type type, object value)
        {
            bool res;
            if (value is bool)
            {
                return value;
            }
            if (value == null || !bool.TryParse(value.ToString(), out res))
            {
                return false;
            }
            return res;
        }
    }
}