namespace SIGEPROJ.BaseClasses.Mappers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IOneWayConfiguration
    {
        /// <summary>
        /// true si la propiedad debe setearse �nicamente en 1 sentido.
        /// </summary>
        bool IsOneWay { get; }
    }
}