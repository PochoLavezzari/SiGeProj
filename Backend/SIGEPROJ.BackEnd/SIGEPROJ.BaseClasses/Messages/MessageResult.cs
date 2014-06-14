using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace SIGEPROJ.BaseClasses.Messages
{
    /// <summary>
    /// Clase serializable que se utiliza para pasar mensajes entre los servicios y los clientes.
    /// </summary>
    [Serializable]
    [DataContract]
    public class MessageResult
    {
        /// <summary>
        /// Constructor por defecto. Tipo por defecto = "Info"
        /// </summary>
        public MessageResult()
        {
            TimeStamp = DateTime.Now;
            Kind = Messages.MessageKind.Info;
        }

        /// <summary>
        /// Constructor. Recibe el mensaje a mostrar.
        /// </summary>
        /// <param name="message"></param>
        public MessageResult(string message)
            : this()
        {
            Message = message;
        }

        /// <summary>
        /// Constructor de Excepci�n. Recibe como par�metro una excepci�n.
        /// </summary>
        /// <param name="ex"></param>
        public MessageResult(Exception ex)
            : this(ex.Message)
        {
            StackTrace = ex.ToString();
            Source = ex.Source;
            Kind = Messages.MessageKind.Error;
        }

        /// <summary>
        /// Tipo de mensaje. No se serializa. Para eso se utiliza la propiedad de tipo String: MessageKind
        /// </summary>
        [XmlIgnore]
        [IgnoreDataMember]
        public MessageKind Kind { get; set; }

        /// <summary>
        /// N�mero relativo de mensaje ocurrido en el contexto de Logging. (0, 1, 2... etc)
        /// </summary>
        [DataMember]
        public int MessageNum { get; set; }

        /// <summary>
        /// Tipo de Mensaje
        /// </summary>
        [DataMember]
        public String MessageKind
        {
            get { return Kind.ToString(); }
            set { Kind = (MessageKind)Enum.Parse(typeof(MessageKind), value); }
        }

        /// <summary>
        /// Mensaje a mostrar
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// Clase donde se origin� el mensaje
        /// </summary>
        [DataMember]
        public string Caller { get; set; }

        /// <summary>
        /// C�digo de Error de DataDic
        /// </summary>
        [DataMember]
        public int ErrorCode { get; set; }

        /// <summary>
        /// StackTrace del Error si es que existe
        /// </summary>
        [DataMember]
        public string StackTrace { get; set; }

        /// <summary>
        /// Fecha y hora en el que ocurri� el mensaje
        /// </summary>
        [DataMember]
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// C�digo donde ocurri� el Error si es que existe
        /// </summary>
        [DataMember]
        public string Source { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Message + StackTrace;
        }
    }
}