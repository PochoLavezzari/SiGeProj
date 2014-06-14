using SIGEPROJ.BaseClasses.BE;
using SIGEPROJ.BaseClasses.DTOs;

namespace SIGEPROJ.BaseClasses.Mappers
{
    /// <summary>
    /// Clase base para los mappers entre Entidades de Negocio y Objetos de Transferencia de Datos
    /// </summary>
    /// <typeparam name="TBe">Tipo de la Entidad de Negocio</typeparam>
    /// <typeparam name="TDTO">Tipo del Objeto de Transferencia de Datos</typeparam>
    /// <typeparam name="TIdBe">Tipo del Id de la BE</typeparam>
    /// <typeparam name="TIdDTO">Tipo del Id del DTO</typeparam>
    public interface IDTOMapperBase<TBe, TDTO, TIdBe, TIdDTO>
        where TDTO : class, IDTOBase<TIdDTO>
        where TBe : class, IBusinessEntityBase<TIdBe>
        where TIdDTO : struct
        where TIdBe : struct
    {
        /// <summary>
        /// Obtiene una BE a partir de un DTO
        /// </summary>
        /// <param name="dto">DTO de partida</param>
        /// <returns>Una BE</returns>
        TBe GetBe(TDTO dto);

        /// <summary>
        /// Obtiene un DTO a partir de una BE.
        /// </summary>
        /// <param name="be">BE de partida</param>
        /// <returns>Un DTO</returns>
        TDTO GetDTO(TBe be);

        /// <summary>
        /// Transforma un Id de DTO en un Id de BE
        /// </summary>
        /// <param name="idDTO">Id. del DTO</param>
        /// <returns>Id. de la BE</returns>
        TIdBe GetBeIdFromDTOId(TIdDTO idDTO);

        /// <summary>
        /// Transforma un Id de BE en un Id de DTO
        /// </summary>
        /// <param name="idBe">Id. de la BE</param>
        /// <returns>Id. del DTO</returns>
        TIdDTO GetDTOIdFromBeId(TIdBe idBe);
    }
}
