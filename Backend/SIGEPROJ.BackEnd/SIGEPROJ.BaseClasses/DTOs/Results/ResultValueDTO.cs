using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using SIGEPROJ.BaseClasses.Messages;

namespace SIGEPROJ.BaseClasses.DTOs.Results
{
    /// <summary>
    /// Devuelve un valor del tipo <typeparamref name="TValue"/>
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    [Serializable]
    [DataContract]
    public class ResultValueDTO<TValue> : ResultDTO
    {
        /// <summary>
        /// Constructor para serialización
        /// </summary>
        public ResultValueDTO()
        {
        }

        /// <summary>
        /// Constructor recibiendo el valor
        /// </summary>
        /// <param name="dto"></param>
        public ResultValueDTO(TValue dto)
        {
            Result = dto;
        }

        /// <summary>
        /// Constructor recibiendo el valor y los mensajes de resultados
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="messages"></param>
        public ResultValueDTO(TValue dto, List<MessageResult> messages)
            : base(messages)
        {
            Result = dto;
        }

        /// <summary>
        /// Valor de resultado
        /// </summary>
        [XmlElement(ElementName="result")]
        [DataMember]
        public TValue Result { get; set; }
         
    }
}