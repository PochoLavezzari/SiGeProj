using SIGEPROJ.BaseClasses.Mappers.ValueInjecter;

namespace SIGEPROJ.BaseClasses.Mappers
{
    /// <summary>
    /// Member Injection interface
    /// </summary>
    public interface IMemberInjection : IValueInjection
    {
        /// <summary>
        /// Lanza una excepci�n si es que alg�n mapping es incorrecto.
        /// </summary>
        void AssertConfigurationIsValid();
    }
}