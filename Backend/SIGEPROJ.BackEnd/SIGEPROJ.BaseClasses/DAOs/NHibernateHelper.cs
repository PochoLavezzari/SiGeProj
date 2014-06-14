using NHibernate;
using NHibernate.Cfg;

namespace SIGEPROJ.BaseClasses.DAOs
{
    /// <summary>
    /// Helper de NHibernate que se utiliza para levantar las configuraciones del XML y generar un SessionFactory
    /// </summary>
    public class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    var configuration = new Configuration();
                    configuration.Configure();
                    configuration.AddAssembly("SIGEPROJ.Persistence");
                    _sessionFactory = configuration.BuildSessionFactory();
                }
                return _sessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}
