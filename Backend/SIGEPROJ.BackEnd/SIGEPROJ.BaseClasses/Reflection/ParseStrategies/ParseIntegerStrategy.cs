using System;

namespace SIGEPROJ.BaseClasses.Reflection.ParseStrategies
{
    /// <summary>
    /// Parsea un Integer
    /// </summary>
    public class ParseIntegerStrategy : AbstractSimpleParseStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParseIntegerStrategy"/> class.
        /// </summary>
        public ParseIntegerStrategy() : base(typeof(int)) { }
        
        /// <summary>
        /// Parsea el valor y lo devuelve
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object Parse(Type type, object value)
        {
            int res;
            if (value is Int32)
            {
                return value;
            }
            if (value == null || !Int32.TryParse(value.ToString(), out res))
            {
                return 0;
            }
            return res;
        }
    }
}