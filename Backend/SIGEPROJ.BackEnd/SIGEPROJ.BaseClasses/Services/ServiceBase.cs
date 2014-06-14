using System;
using SIGEPROJ.BaseClasses.BE;
using SIGEPROJ.BaseClasses.DAOs;
using SIGEPROJ.BaseClasses.DTOs;
using SIGEPROJ.BaseClasses.DTOs.Results;
using SIGEPROJ.BaseClasses.Mappers;

namespace SIGEPROJ.BaseClasses.Services
{
    public class ServiceBase<TDTO, TBe, TIdDTO, TIdBe> : IServiceBase<TDTO, TIdDTO>
        where TDTO : class, IDTOBase<TIdDTO>
        where TBe: class, IBusinessEntityBase<TIdBe>
        where TIdDTO : struct
        where TIdBe : struct 
    {
        public readonly IDAOBase<TBe, TIdBe> _dao;

        public readonly IDTOMapperBase<TBe, TDTO, TIdBe, TIdDTO> _dtoMapper;

        #region IServiceBase<TDTO,TIdDTO> Members

        public ResultObjectDTO<TDTO> Insert(TDTO dto)
        {
            try
            {
                //TODO: Ver como obtener los errores de cada uno de los métodos.
                var be = _dtoMapper.GetBe(dto);
                var beResult = _dao.Insert(be);
                var dtoResult = _dtoMapper.GetDTO(beResult);
                return new ResultObjectDTO<TDTO>(dtoResult);
            }
            catch (Exception e)
            {
                //TODO: Loguear los errores
                throw;
            }
        }

        public ResultObjectDTO<TDTO> Update(TDTO dto)
        {
            try
            {
                //TODO: Ver como obtener los errores de cada uno de los métodos.
                var be = _dtoMapper.GetBe(dto);
                var beResult = _dao.Insert(be);
                var dtoResult = _dtoMapper.GetDTO(beResult);
                return new ResultObjectDTO<TDTO>(dtoResult);
            }
            catch (Exception e)
            {
                //TODO: Loguear los errores
                throw;
            }
        }

        public ResultVoidDTO Delete(TDTO dto)
        {
            try
            {
                //TODO: Ver como obtener los errores de cada uno de los métodos.
                var be = _dtoMapper.GetBe(dto);
                _dao.Delete(be);
                return new ResultVoidDTO();
            }
            catch (Exception e)
            {
                //TODO: Loguear los errores
                throw;
            }
        }

        public ResultObjectDTO<TDTO> GetById(TIdDTO id)
        {
            try
            {
                //TODO: Ver como obtener los errores de cada uno de los métodos.
                var idBe = _dtoMapper.GetBeIdFromDTOId(id);
                var beResult = _dao.GetById(idBe);
                var dtoResult = _dtoMapper.GetDTO(beResult);
                return new ResultObjectDTO<TDTO>(dtoResult);
            }
            catch (Exception e)
            {
                //TODO: Loguear los errores
                throw;
            }
        }

        #endregion
    }
}
