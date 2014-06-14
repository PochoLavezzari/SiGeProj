namespace SIGEPROJ.BaseClasses.Mappers.TypeMappers
{
    /// <summary>
    /// Clase <see cref="SimpleMapper"/>
    /// </summary>
    public class SimpleMapper : IMemberInjection
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly SimpleMapper Default = new SimpleMapper();

        /// <summary>
        /// Prevents a default instance of the <see cref="SimpleMapper"/> class from being created.
        /// </summary>
        private SimpleMapper()
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
            return source;
        }

        /// <summary>
        /// Lanza una excepción si es que algún mapping es incorrecto.
        /// </summary>
        public void AssertConfigurationIsValid()
        {
        }
    }
}