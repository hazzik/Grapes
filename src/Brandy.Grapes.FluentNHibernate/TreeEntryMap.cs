using FluentNHibernate.Mapping;

namespace Brandy.Grapes.FluentNHibernate
{
    public static class TreeEntryMap
    {
        public static void MapTree<T>(this ClasslikeMapBase<T> map, string hierarchyTableName)
            where T : TreeEntry<T>
        {
            map.References(x => x.Parent)
                .Access.CamelCaseField()
                .Nullable()
                .Column("PARENT_ID");

            map.HasMany(x => x.Children)
                .Access.CamelCaseField()
                .Cascade.All()
                .Inverse()
                .AsSet()
                .LazyLoad()
                .BatchSize(250)
                .KeyColumn("PARENT_ID");

            map.HasManyToMany(x => x.Ancestors)
                .Access.CamelCaseField()
                .Cascade.None()
                .AsSet()
                .LazyLoad()
                .BatchSize(250)
                .Table(hierarchyTableName)
                .ParentKeyColumn("CHILD_ID")
                .ChildKeyColumn("PARENT_ID")
                .ForeignKeyConstraintNames(string.Format("FK_{0}_CHILD", hierarchyTableName), null);

            map.HasManyToMany(x => x.Descendants)
                .Access.CamelCaseField()
                .Cascade.All()
                .Inverse()
                .AsSet()
                .LazyLoad()
                .BatchSize(250)
                .Table(hierarchyTableName)
                .ParentKeyColumn("PARENT_ID")
                .ChildKeyColumn("CHILD_ID")
                .ForeignKeyConstraintNames(string.Format("FK_{0}_PARENT", hierarchyTableName), null);
        }
    }
}