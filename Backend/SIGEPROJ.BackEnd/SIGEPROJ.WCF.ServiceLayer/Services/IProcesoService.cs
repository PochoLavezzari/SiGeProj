using System.Net.Security;
using System.ServiceModel;
using SIGEPROJ.BaseClasses.Services;
using SIGEPROJ.WCF.ServiceLayer.DTOs;

namespace SIGEPROJ.WCF.ServiceLayer.Services
{
    [ServiceContract(ProtectionLevel = ProtectionLevel.EncryptAndSign)]
    public interface IProcesoService : IServiceBase<ProcesoDTO, long>
    {
    }
}
