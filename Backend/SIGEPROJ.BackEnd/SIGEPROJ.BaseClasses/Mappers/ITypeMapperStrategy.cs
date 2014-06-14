namespace SIGEPROJ.BaseClasses.Mappers
{
    /// <summary>
    /// En base a los Types de los objetos, busca el mapper correcto
    /// </summary>
    public interface ITypeMapperStrategy
    {
        /// <summary>
        /// True si cumple la condición para utilizar este Strategy
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        bool Match(StrategyConventionInfo info);

        /// <summary>
        /// Devuelve el Mapper correcto.
        /// </summary>
        /// <returns></returns>
        IMemberInjection GetInjecter(StrategyConventionInfo info);
    }
}