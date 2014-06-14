using System.Runtime.Serialization;

namespace SIGEPROJ.BaseClasses.Messages
{
    /// <summary>
    /// Tipos de mensaje entre los activites y el host
    /// </summary>
    [DataContract]
    public enum MessageKind
    {
        /// <summary>
        /// Mensaje de error Fatal
        /// </summary>
        [EnumMember]
        Fatal,
        /// <summary>
        /// Mensaje de Error
        /// </summary>
        [EnumMember]
        Error,
        /// <summary>
        /// Mensaje de Información
        /// </summary>
        [EnumMember]
        Info,
        /// <summary>
        /// Mensaje de alerta
        /// </summary>
        [EnumMember]
        Warning,
        /// <summary>
        /// Mensaje de Debug
        /// </summary>
        [EnumMember]
        Debug,
        /// <summary>
        /// Para otro tipo de error
        /// </summary>
        [EnumMember]
        Other,
    }
}