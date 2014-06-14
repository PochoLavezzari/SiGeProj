namespace SIGEPROJ.BaseClasses.Mappers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IOneWayConfiguration
    {
        /// <summary>
        /// true si la propiedad debe setearse únicamente en 1 sentido.
        /// </summary>
        bool IsOneWay { get; }
    }
}