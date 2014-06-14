using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using SIGEPROJ.BaseClasses.Messages;

namespace SIGEPROJ.BaseClasses.DTOs.Results
{
    /// <summary>
    /// Devuelve un Objeto DTO
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    [Serializable]
    [DataContract]
    public class ResultObjectDTO<TDto> : ResultDTO where TDto : IDTOBase
    {
        /// <summary>
        /// Constructor para serialización
        /// </summary>
        public ResultObjectDTO()
        {
        }

        /// <summary>
        /// Constructor recibiendo el DTO
        /// </summary>
        /// <param name="dto"></param>
        public ResultObjectDTO(TDto dto)
        {
            Result = dto;
        }

        /// <summary>
        /// Constructor recibiendo el DTO y la lista de mensajes
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="messages"></param>
        public ResultObjectDTO(TDto dto, List<MessageResult> messages)
            : base(messages)
        {
            Result = dto;
        }

        /// <summary>
        /// DTO de resultado
        /// </summary>
        [XmlElement(ElementName="result")]
        [DataMember]
        public TDto Result { get; set; }

        /// <summary>
        /// Convierte un ResultObjectDTO&lt;<typeparamref name="TDto"/>&gt; a ResultObjectDTO&lt;IDTO&gt;
        /// para poder ser utilizado en un DataSource
        /// </summary>
        /// <returns>Objeto transformado</returns>
        public ResultObjectDTO<IDTOBase> ToResultObjectOfIDTO()
        {
            // Objeto obtenido
            IDTOBase objResult = Result;

            // Resultado
            var res = new ResultObjectDTO<IDTOBase>
                (
                    objResult,      // IDTO devuelto
                    Messages // Seteo la lista de mensajes
                );
            return res;
        }

        /// <summary>
        /// Convierte un ResultObjectDTO&lt;<typeparamref name="TDto"/>&gt; a ResultListDTO&lt;IDTO&gt;
        /// para poder ser utilizado en un DataSource.
        /// </summary>
        /// <returns></returns>
        public ResultListDTO<IDTOBase> ToResultListOfIDTO()
        {
            var list = new List<IDTOBase>();
            // Agrego el resultado sólo si no es Null
            if (!Equals(Result, default(TDto)))
                list.Add(Result);
            var res = new ResultListDTO<IDTOBase>
                                          {
                                              ResultList = list,
                                              Messages = Messages,
                                              TotalListCount = list.Count,
                                              NumPage = 0

                                          };
            return res;
        }
    }
}