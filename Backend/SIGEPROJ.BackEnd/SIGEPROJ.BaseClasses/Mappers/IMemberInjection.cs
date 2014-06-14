using SIGEPROJ.BaseClasses.Mappers.ValueInjecter;

namespace SIGEPROJ.BaseClasses.Mappers
{
    /// <summary>
    /// Member Injection interface
    /// </summary>
    public interface IMemberInjection : IValueInjection
    {
        /// <summary>
        /// Lanza una excepción si es que algún mapping es incorrecto.
        /// </summary>
        void AssertConfigurationIsValid();
    }
}