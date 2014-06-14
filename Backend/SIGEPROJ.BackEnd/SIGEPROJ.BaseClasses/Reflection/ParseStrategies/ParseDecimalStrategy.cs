using System;

namespace SIGEPROJ.BaseClasses.Reflection.ParseStrategies
{
    /// <summary>
    /// Parsea un número decimal
    /// </summary>
    public class ParseDecimalStrategy : AbstractSimpleParseStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParseDecimalStrategy"/> class.
        /// </summary>
        public ParseDecimalStrategy() : base(typeof(decimal)) { }

        /// <summary>
        /// Parsea el valor y lo devuelve
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object Parse(Type type, object value)
        {
            decimal res;
            if (value is decimal)
            {
                return value;
            }
            if (value == null || !decimal.TryParse(value.ToString(), out res))
            {
                return 0M;
            }
            return res;
        }
    }
}