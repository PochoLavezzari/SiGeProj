using System;

namespace SIGEPROJ.BaseClasses.Reflection.ParseStrategies
{
    /// <summary>
    /// Parsea un Boolean
    /// </summary>
    public class ParseInt64Strategy : AbstractSimpleParseStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParseFloatStrategy"/> class.
        /// </summary>
        public ParseInt64Strategy() : base(typeof(Int64)) { }

        /// <summary>
        /// Parsea el valor y lo devuelve
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object Parse(Type type, object value)
        {
            long res;
            if (value is Int64)
            {
                return value;
            }
            if (value == null || !Int64.TryParse(value.ToString(), out res))
            {
                return 0;
            }
            return res;
        }
    }
}