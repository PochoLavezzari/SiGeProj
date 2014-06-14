using System;

namespace SIGEPROJ.BaseClasses.Reflection.ParseStrategies
{
    /// <summary>
    /// Parsea un valor Float
    /// </summary>
    public class ParseFloatStrategy : AbstractSimpleParseStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParseFloatStrategy"/> class.
        /// </summary>
        public ParseFloatStrategy() : base(typeof(float)) { }

        /// <summary>
        /// Parsea el valor y lo devuelve
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object Parse(Type type, object value)
        {
            float res;
            if (value is float)
            {
                return value;
            }
            if (value == null || !float.TryParse(value.ToString(), out res))
            {
                return 0f;
            }
            return res;
        }
    }
}