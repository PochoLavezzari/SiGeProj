namespace SIGEPROJ.BaseClasses.BE
{
    /// <summary>
    /// Interface base para una entidad de negocio
    /// </summary>
    /// <typeparam name="TId">Tipo del Id de la clase</typeparam>
    public interface IBusinessEntityBase<TId>
        where TId : struct
    {
        /// <summary>
        /// Identificador de la entidad de negocio
        /// </summary>
        TId Id { get; set; }
    }
}
