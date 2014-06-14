using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using SIGEPROJ.BaseClasses.Messages;

namespace SIGEPROJ.BaseClasses.DTOs.Results
{
    /// <summary>
    /// Clase base para devolver resultados desde un DTO
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName="result")]
    [DataContract]
    public class ResultDTO
    {
        /// <summary>
        /// Lista de mensajes
        /// </summary>
        private List<MessageResult> _messages;

        /// <summary>
        /// Constructor
        /// </summary>
        public ResultDTO()
        {
            _messages = new List<MessageResult>();
        }

        /// <summary>
        /// Constructor recibiendo la lista de mensajes
        /// </summary>
        /// <param name="messages"></param>
        public ResultDTO(List<MessageResult> messages)
        {
            Messages = messages;
        }

        /// <summary>
        /// Lista de mensajes serializables
        /// </summary>
        [XmlArray(ElementName="messages",IsNullable=false)]
        [XmlArrayItem(ElementName="msg")]
        [DataMember]
        public List<MessageResult> Messages
        {
            get { return _messages; }
            set { _messages = value; }
        }

        /// <summary>
        /// Cantidad de errores
        /// </summary>
        [XmlAttribute(AttributeName = "countErrors")]
        [DataMember]
        public int CountErrors
        {
            get
            {
                return _messages.Count(message => message.Kind == MessageKind.Error);
            }
            private set { }
        }

        /// <summary>
        /// Cantidad de Warnings
        /// </summary>
        [XmlAttribute(AttributeName = "countWarnings")]
        [DataMember]
        public int CountWarnings
        {
            get
            {
                return _messages.Count(message => message.Kind == MessageKind.Warning);
            }
            private set { }
        }

        /// <summary>
        /// Cantidad de Mensajes de información
        /// </summary>
        [XmlAttribute(AttributeName = "countInfos")]
        [DataMember]
        public int CountInfos
        {
            get
            {
                return _messages.Count(message => message.Kind == MessageKind.Info);
            }
            private set { }
        }

        /// <summary>
        /// Indica si contiene errores
        /// </summary>
        [XmlAttribute(AttributeName = "hasErrors")]
        [DataMember]
        public bool HasErrors
        {
            get { return CountErrors > 0; }
            private set { }
        }

        /// <summary>
        /// Indica si contiene Warnings
        /// </summary>
        [XmlAttribute(AttributeName = "hasWarnings")]
        [DataMember]
        public bool HasWarnings
        {
            get { return CountWarnings > 0; }
            private set { }
        }

        /// <summary>
        /// Inidica si contiene Mensajes de Información
        /// </summary>
        [XmlAttribute(AttributeName="hasInfos")]
        [DataMember]
        public bool HasInfos
        {
            get { return CountInfos > 0; }
            private set { }
        }
    }
}