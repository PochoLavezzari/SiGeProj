using System;
using NHibernate;
using SIGEPROJ.BaseClasses.BE;

namespace SIGEPROJ.BaseClasses.DAOs
{
    /// <summary>
    /// Clase que implementa los métodos base de un objeto de acceso a datos
    /// </summary>
    /// <typeparam name="T">Tipo de la entidad a utililzar</typeparam>
    /// <typeparam name="TId">Tipo del id de la entidad</typeparam>
    public class DAONhBase<T, TId> : IDAOBase<T, TId> 
        where T : class, IBusinessEntityBase<TId>
        where TId : struct
    {
        public T Insert(T entity)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                try
                {
                    var result = session.Save(entity);
                    transaction.Commit();
                    return (T)result;
                }
                catch (Exception e)
                {
                    throw new SessionException(e.Message);
                }
            }
        }

        public T Update(T entity)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                try
                {
                    session.Update(entity);
                    transaction.Commit();
                    var result = session.Get<T>(entity.Id);                    
                    return result;
                }
                catch (Exception e)
                {
                    throw new SessionException(e.Message);
                }
            }
        }

        public void Delete(T entity)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                try
                {
                    session.Delete(entity);
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    throw new SessionException(e.Message);
                }
            }
        }

        public T GetById(TId entityId)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                try
                {
                    return session.Get<T>(entityId);
                }
                catch (Exception e)
                {
                    throw new SessionException(e.Message);
                }
            }
        }
    }    
}
