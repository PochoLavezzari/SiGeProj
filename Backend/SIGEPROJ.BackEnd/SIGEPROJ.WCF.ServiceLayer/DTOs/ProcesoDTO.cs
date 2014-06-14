using System;
using System.Runtime.Serialization;
using SIGEPROJ.BaseClasses.DTOs;

namespace SIGEPROJ.WCF.ServiceLayer.DTOs
{
    /// <summary>
    /// Objeto de transferencia de datos de la clase Proceso
    /// </summary>
    [DataContract]
    public class ProcesoDTO : IDTOBase<long>
    {
        #region IDTOBase<long> Members

        [DataMember]
        public long Id { get; set; }

        #endregion

        [DataMember]
        public DateTime FechaProceso { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public string Caratula { get; set; }
    }
}
