using SIGEPROJ.BaseClasses.BE;

namespace SIGEPROJ.BaseClasses.DAOs
{
    /// <summary>
    /// Interfaz de acceso a datos
    /// </summary>
    public interface IDAO
    {

    }
    /// <summary>
    /// Interfaz de acceso a datos base, que define los metodos CRUD para una entidad
    /// </summary>
    /// <typeparam name="T">Tipo de la clase</typeparam>
    /// <typeparam name="TId">Tipo del Id de la clase</typeparam>
    public interface IDAOBase <T, in TId> : IDAO 
        where T : class, IBusinessEntityBase<TId> 
        where TId : struct
    {
        /// <summary>
        /// Hace un insert de un objeto en la base de datos
        /// </summary>
        /// <param name="entity">Entidad con la que se trabaja</param>
        /// <returns>Una entidad modificada</returns>
        T Insert(T entity);

        /// <summary>
        /// Hace un insert de un objeto en la base de datos
        /// </summary>
        /// <param name="entity">Entidad con la que se trabaja</param>
        /// <returns>Una entidad modificada</returns>
        T Update(T entity);

        /// <summary>
        /// Hace un delete de un objeto en la base de datos
        /// </summary>
        /// <param name="entity">Entidad con la que se trabaja</param>
        void Delete(T entity);

        /// <summary>
        /// Obtiene un objeto de la base de datos a partir de un Id
        /// </summary>
        /// <param name="entityId">Id. de la entidad a obtener</param>
        /// <returns>Una entidad</returns>
        T GetById(TId entityId);
    }
}
