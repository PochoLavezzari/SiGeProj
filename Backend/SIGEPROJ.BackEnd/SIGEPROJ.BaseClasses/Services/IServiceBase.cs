using System;
using System.Net.Security;
using System.ServiceModel;
using SIGEPROJ.BaseClasses.DTOs;
using SIGEPROJ.BaseClasses.DTOs.Results;

namespace SIGEPROJ.BaseClasses.Services
{
    /// <summary>
    /// Interfaz de servicio base
    /// </summary>
    /// <typeparam name="TDTO">Tipo del DTO con el que trabaja el servicio</typeparam>
    /// <typeparam name="TIdDTO">Tipo de Id del DTO</typeparam>
    [ServiceContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Name = "IServiceBase")]
    public interface IServiceBase<TDTO, in TIdDTO>
        where TDTO : IDTOBase<TIdDTO>
        where TIdDTO : struct
    {
        /// <summary>
        /// Servicio de Inserción de un DTO
        /// </summary>
        /// <param name="dto">DTO a insertar</param>
        /// <returns>Un DTO persistido</returns>
        [OperationContract(Action = "Insert", ProtectionLevel = ProtectionLevel.EncryptAndSign)]
        ResultObjectDTO<TDTO> Insert(TDTO dto);

        /// <summary>
        /// Servicio de actualización de un DTO
        /// </summary>
        /// <param name="dto">DTO a actualizar</param>
        /// <returns>Un DTO actualizado</returns>
        [OperationContract(Action = "Update", ProtectionLevel = ProtectionLevel.EncryptAndSign)]
        ResultObjectDTO<TDTO> Update(TDTO dto);

        /// <summary>
        /// Servicio de eliminación de un DTO
        /// </summary>
        /// <param name="dto">DTO a eliminar</param>
        [OperationContract(Action="Delete", ProtectionLevel = ProtectionLevel.EncryptAndSign)]
        ResultVoidDTO Delete(TDTO dto);

        /// <summary>
        /// Servicio que recupera un DTO, para un Id. dado
        /// </summary>
        /// <param name="id">Id. del DTO a buscar</param>
        /// <returns>Un DTO</returns>
        [OperationContract(Action="GetById", ProtectionLevel = ProtectionLevel.EncryptAndSign)]
        ResultObjectDTO<TDTO> GetById(TIdDTO id);
    }
}
