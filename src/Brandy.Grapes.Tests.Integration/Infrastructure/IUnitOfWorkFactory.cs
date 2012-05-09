namespace Brandy.Trees.Tests.Integration.Infrastructure
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
    }
}