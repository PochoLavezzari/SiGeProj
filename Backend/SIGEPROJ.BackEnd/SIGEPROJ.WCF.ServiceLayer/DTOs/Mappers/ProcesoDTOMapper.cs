using SIGEPROJ.BaseClasses.Mappers;
using SIGEPROJ.BusinessLayer;

namespace SIGEPROJ.WCF.ServiceLayer.DTOs.Mappers
{
    public class ProcesoDTOMapper : IDTOMapperBase<Proceso, ProcesoDTO, long, long>
    {
        static ProcesoDTOMapper()
        {
            Mapper.CreateMap<ProcesoDTO, Proceso>()
                .MapUnmappedProperties()
                .TwoWayMapping();
        }

        public Proceso GetBe (ProcesoDTO dto)
        {
            return Mapper.Map(dto, new Proceso());
        }

        public ProcesoDTO GetDTO (Proceso be)
        {
            return Mapper.Map(be, new ProcesoDTO());
        }

        #region IDTOMapperBase<Proceso,ProcesoDTO,long,long> Members


        public long GetBeIdFromDTOId(long idDTO)
        {
            return idDTO;
        }

        public long GetDTOIdFromBeId(long idBe)
        {
            return idBe;
        }

        #endregion
    }
}
