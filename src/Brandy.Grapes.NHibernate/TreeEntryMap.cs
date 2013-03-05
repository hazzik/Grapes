namespace Brandy.Grapes.NHibernate
{
    using global::NHibernate.Mapping.ByCode;

    public static class TreeEntryMap
    {
        public static void MapTree<T>(this IClassMapper<T> map)
            where T : TreeEntry<T>
        {
            MapTree(map, typeof (T).Name + "_HIERARCHY");
        }

        public static void MapTree<T>(this IClassMapper<T> map, string hierarchyTableName)
            where T : TreeEntry<T>
        {
            map.ManyToOne(x => x.Parent, m =>
                {
                    m.Access(Accessor.Field);
                    m.NotNullable(false);
                    m.Column("PARENT_ID");
                });

            map.Set(x => x.Children, m =>
                {
                    m.Access(Accessor.Field);
                    m.Cascade(Cascade.All);
                    m.Inverse(true);
                    m.Lazy(CollectionLazy.Lazy);
                    m.BatchSize(250);
                    m.Key(k => k.Column("PARENT_ID"));
                }, m => m.OneToMany());

            map.Set(x => x.Ancestors, m =>
                {
                    m.Access(Accessor.Field);
                    m.Cascade(Cascade.None);
                    m.Lazy(CollectionLazy.Lazy);
                    m.BatchSize(250);
                    m.Table(hierarchyTableName);
                    m.Key(k =>
                        {
                            k.Column("CHILD_ID");
                            k.ForeignKey(string.Format("FK_{0}_CHILD", hierarchyTableName));
                        });
                }, m => m.ManyToMany(x => x.Column("PARENT_ID")));

            map.Set(x => x.Descendants, m =>
                {
                    m.Access(Accessor.Field);
                    m.Cascade(Cascade.All);
                    m.Inverse(true);
                    m.Lazy(CollectionLazy.Lazy);
                    m.BatchSize(250);
                    m.Table(hierarchyTableName);
                    m.Key(k =>
                        {
                            k.Column("PARENT_ID");
                            k.ForeignKey(string.Format("FK_{0}_PARENT", hierarchyTableName));
                        });
                }, m => m.ManyToMany(x => x.Column("CHILD_ID")));
        }
    }
}
