using System;
using SIGEPROJ.BaseClasses.BE;

namespace SIGEPROJ.BusinessLayer
{
    /// <summary>
    /// Clase de Proceso Judicial
    /// </summary>
    public class Proceso : IBusinessEntityBase<long>
    {
        public virtual DateTime FechaProceso { get; set; }
        public virtual string Descripcion { get; set; }
        public virtual string Caratula { get; set; }

        public override bool Equals(object obj)
        {
            var proceso = (Proceso)obj;
            if (this == proceso) return true;
            if (proceso == null) return false; 

            if (Id != proceso.Id) return false;
            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Id.GetHashCode();
            }
        }

        public virtual long Id { get; set; }
    }
}
