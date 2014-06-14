namespace SIGEPROJ.BaseClasses.Mappers.TypeMappers
{
    /// <summary>
    /// Interfaz base que deben cumplir los mappers de un Type a otro Type
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TTarget"></typeparam>
    public abstract class BaseTypeMapper<TSource,TTarget> : IMemberInjection
    {
        /// <summary>
        /// Maps el source especificado.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public abstract TTarget Map(TSource source, TTarget target);

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
            return Map((TSource)source, (TTarget)target);
        }

        /// <summary>
        /// Lanza una excepción si es que algún mapping es incorrecto.
        /// </summary>
        public void AssertConfigurationIsValid() { }
    }
}