using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SIGEPROJ.BaseClasses.Messages;

namespace SIGEPROJ.BaseClasses.DTOs.Results
{
    /// <summary>
    /// Devuelve solo resultados de la operación
    /// </summary>
    [Serializable]
    [DataContract]
    public class ResultVoidDTO : ResultDTO
    {
        /// <summary>
        /// Constructor para serialización
        /// </summary>
        public ResultVoidDTO()
        {
        }

        /// <summary>
        /// Constructor recibiendo una lista de mensajes
        /// </summary>
        /// <param name="messages"></param>
        public ResultVoidDTO(List<MessageResult> messages)
            : base(messages)
        {
        }

    }
}