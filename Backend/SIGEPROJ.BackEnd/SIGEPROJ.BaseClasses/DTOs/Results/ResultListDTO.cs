using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using SIGEPROJ.BaseClasses.Messages;

namespace SIGEPROJ.BaseClasses.DTOs.Results
{
    /// <summary>
    /// Devuelve una lista de DTOs
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    [Serializable]
    [DataContract]
    public class ResultListDTO<TDto> : ResultDTO where TDto : IDTOBase
    {
        /// <summary>
        /// Cantidad total de resultados
        /// (puede ser distinta a la cantidad de resultados
        /// devueltos, por ej si está filtrado)
        /// </summary>
        [XmlAttribute(AttributeName="totalListCount")]
        [DataMember]
        public int TotalListCount { get; set; }

        /// <summary>
        /// Página actual de los resultados
        /// </summary>
        [XmlAttribute(AttributeName = "numPage")]
        [DataMember]
        public int NumPage { get; set; }

        /// <summary>
        /// Constructor para serialización
        /// </summary>
        public ResultListDTO()
        {

        }

        /// <summary>
        /// Constructor recibiendo una lista de DTOs
        /// </summary>
        /// <param name="dto"></param>
        public ResultListDTO(List<TDto> dto) : this()
        {
            ResultList = dto;
        }

        /// <summary>
        /// Constructor  recibiendo una lista de DTOs y una lista de mensajes
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="messages"></param>
        public ResultListDTO(List<TDto> dto, List<MessageResult> messages)
            : base(messages)
        {
            ResultList = dto;
        }

        /// <summary>
        /// Lista de resultados
        /// </summary>
        [DataMember]
        public List<TDto> ResultList { get; set; }

        /// <summary>
        /// Convierte un ResultListDTO&lt;<typeparamref name="TDto"/>&gt; a ResultListDTO&lt;IDTO&gt;
        /// para poder ser utilizado en un DataSource
        /// </summary>
        /// <returns></returns>
        public ResultListDTO<IDTOBase> ToResultListOfIDTO()
        {
            // Creo la Lista de resultados a devolver
            var listResult =
                new List<IDTOBase>();

            // Agrego los Items a la lista
            if(ResultList != null)
                ResultList
                    .ForEach(dto => listResult.Add(dto));

            // Creo el resultado pasando los parámetros necesarios
            var res =
                new ResultListDTO<IDTOBase>
                    (
                        listResult,     // Seteo la lista de resultados
                        Messages // Seteo la lista de mensajes
                    )
                {
                    NumPage = NumPage,               // Seteo el número de página
                    TotalListCount = TotalListCount  // Lista total de resultados
                };
            return res;
        }
    }
}