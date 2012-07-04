namespace Brandy.Trees.Tests.Integration.FluentNHibernate
{
    using Infrastructure;
    using NHibernate;
    using global::FluentNHibernate.Cfg;
    using global::FluentNHibernate.Cfg.Db;
    using global::NHibernate;
    using global::NHibernate.Cfg;
    using global::NHibernate.Tool.hbm2ddl;

    public class NHUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private static readonly ISessionFactory sessionFactory;

        static NHUnitOfWorkFactory()
        {
            var config = MsSqlCeConfiguration.Standard
                .ConnectionString("Data Source=TestDb.sdf")
                .ShowSql();

            var cfg = Fluently.Configure()
                .Database(config)
                .ExposeConfiguration(ExtendConfiguration)
                .Mappings(m => m.FluentMappings
                                   .Add<TestTreeEntryMap>()
                                   .ExportTo(@"."));

            var configuration = cfg.BuildConfiguration();
            BuildSchema(configuration);

            sessionFactory = configuration.BuildSessionFactory();
        }

        public IUnitOfWork Create()
        {
            return new NHUnitOfWork(sessionFactory.OpenSession());
        }

        private static void BuildSchema(Configuration configuration)
        {
            new SchemaExport(configuration).Execute(true, true, false);
        }

        private static void ExtendConfiguration(Configuration c)
        {
            c.SetProperty("generate_statistics", "true");
        }
    }
}