namespace Brandy.Trees.Tests.Integration.Infrastructure
{
    using System;
    using System.Linq;

    public interface IUnitOfWork : IDisposable
    {
        T Get<T>(int id) where T : class;
        IQueryable<T> Query<T>() where T : class; 
        void Save<T>(T entity) where T : class;
        void Commit();
        void Delete<T>(T entity) where T : class;
    }
}