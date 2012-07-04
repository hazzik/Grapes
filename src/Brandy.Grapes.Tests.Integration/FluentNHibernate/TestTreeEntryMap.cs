namespace Brandy.Trees.Tests.Integration.FluentNHibernate
{
    using Grapes.Tests;
    using Grapes.FluentNHibernate;
    using global::FluentNHibernate.Mapping;

    public class TestTreeEntryMap : ClassMap<TestTreeEntry>
    {
        public TestTreeEntryMap()
        {
            Id(x => x.Id)
                .GeneratedBy.Identity();

            Map(x => x.Name)
                .Default("'The name'");

            this.MapTree("TestTreeClass_HIERARCHY");

            DynamicInsert();
            DynamicUpdate();
        }
    }
}