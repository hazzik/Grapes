namespace Brandy.Trees.Tests.Integration.NHibernate
{
    using Brandy.Grapes.NHibernate;
    using Brandy.Grapes.Tests;

    using FluentNHibernate.Mapping;

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