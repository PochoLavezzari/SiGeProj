using System.Runtime.Serialization;

namespace SIGEPROJ.BaseClasses.DTOs
{
    /// <summary>
    /// Objeto de transferencia de datos base
    /// </summary>
    public interface IDTOBase
    {
    }

    /// <summary>
    /// Objeto de transferencia de datos base
    /// </summary>
    /// <typeparam name="TIdDTO">Tipo del Id del DTO</typeparam>
    public interface IDTOBase<TIdDTO> : IDTOBase
        where TIdDTO : struct
    {
        /// <summary>
        /// Identificador del DTO
        /// </summary>
        [DataMember]
        TIdDTO Id { get; set; }
    }
}
