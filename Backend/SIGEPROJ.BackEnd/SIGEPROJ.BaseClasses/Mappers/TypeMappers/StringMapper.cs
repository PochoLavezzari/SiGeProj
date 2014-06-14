namespace SIGEPROJ.BaseClasses.Mappers.TypeMappers
{
    /// <summary>
    /// Clase <see cref="StringMapper"/>
    /// </summary>
    public class StringMapper : IMemberInjection
    {
        /// <summary>
        /// 
        /// </summary>
        public static StringMapper DefaultMapper = new StringMapper();

        /// <summary>
        /// Prevents a default instance of the <see cref="StringMapper"/> class from being created.
        /// </summary>
        private StringMapper()
        {
            
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
            if (source == null)
                return null;
            if(source is string)
            {
                return source;
            }
            return source.ToString();
        }

        /// <summary>
        /// Lanza una excepción si es que algún mapping es incorrecto.
        /// </summary>
        public void AssertConfigurationIsValid()
        {
        }
    }
}