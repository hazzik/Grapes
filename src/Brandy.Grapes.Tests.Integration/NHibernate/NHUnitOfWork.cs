namespace Brandy.Trees.Tests.Integration.NHibernate
{
    using System.Linq;

    using Infrastructure;

    using global::NHibernate;
    using global::NHibernate.Linq;

    public class NHUnitOfWork : IUnitOfWork
    {
        private ISession session;
        private ITransaction tx;

        public NHUnitOfWork(ISession session)
        {
            this.session = session;
            tx = session.BeginTransaction();
        }

        public void Dispose()
        {
            if (tx != null) tx.Dispose();
            tx = null;
            if (session != null) session.Dispose();
            session = null;
        }

        public void Commit()
        {
            tx.Commit();
        }

        public void Delete<T>(T entity) where T : class
        {
            session.Delete(entity);
        }

        public T Get<T>(int id) where T : class
        {
            return session.Get<T>(id);
        }

        public IQueryable<T> Query<T>() where T : class
        {
            return session.Query<T>();
        }

        public void Save<T>(T entity) where T : class
        {
            session.Save(entity);
        }
    }
}