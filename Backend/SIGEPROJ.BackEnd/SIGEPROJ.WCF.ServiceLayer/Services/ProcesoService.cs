using SIGEPROJ.BusinessLayer;
using SIGEPROJ.WCF.ServiceLayer.DTOs;
using SIGEPROJ.BaseClasses.Services;

namespace SIGEPROJ.WCF.ServiceLayer.Services
{
    
    public class ProcesoService : ServiceBase<ProcesoDTO, Proceso, long, long>, IProcesoService
    {
    }
}
