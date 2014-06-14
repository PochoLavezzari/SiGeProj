namespace SIGEPROJ.BaseClasses.Mappers
{
    /// <summary>
    /// Interfaz que permite indicar los mappings que pueden transformarse en
    /// su inverso. Esto es, que se pase desde un Target a un Source.
    /// </summary>
    public interface ITwoWayMappingConverter
    {
        /// <summary>
        /// True si puede convertirse al inverso.
        /// </summary>
        /// <returns></returns>
        bool CanConvert();

        /// <summary>
        /// Convierte el Mapping actual a su inverso.
        /// </summary>
        /// <returns></returns>
        MappingConfiguration ConvertToInverse();
    }
}