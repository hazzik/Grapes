namespace Brandy.Trees.Tests.Integration.Infrastructure
{
    using System;
    using System.Linq;

    using Brandy.Grapes.Tests;

    using Xunit;

    public abstract class TreeEntryMappingTestsBase<T> : IDisposable
        where T : IUnitOfWorkFactory, new()
    {
        private readonly IUnitOfWorkFactory factory = new T();

        [Fact]
        public void AbleToSaveTreeEntrys()
        {
            var testTreeParent = new TestTreeEntry {Name = "parent"};
            var testTreeChild = new TestTreeEntry {Name = "child"};

            testTreeParent.AddChild(testTreeChild);
            using (var uow = factory.Create())
            {
                uow.Save(testTreeParent);
                uow.Commit();
            }

            using (var uow = factory.Create())
            {
                var child = uow.Get<TestTreeEntry>(testTreeChild.Id);
                var parent = uow.Get<TestTreeEntry>(testTreeParent.Id);

                Assert.NotSame(testTreeParent, parent);
                Assert.NotSame(testTreeChild, child);
                Assert.Equal(1, parent.Children.Count());
                Assert.Equal(child, parent.Children.First());
                Assert.NotEmpty(parent.Descendants.ToList());
                Assert.NotEmpty(child.Ancestors.ToList());
            }
        }

        [Fact]
        public void AbleToQueryParent()
        {
            var testTreeParent = new TestTreeEntry {Name = "parent"};
            var testTreeChild = new TestTreeEntry {Name = "child"};

            testTreeParent.AddChild(testTreeChild);
            using (var uow = factory.Create())
            {
                uow.Save(testTreeParent);
                uow.Commit();
            }

            using (var uow = factory.Create())
            {
                var child = uow.Query<TestTreeEntry>()
                    .Single(e => e.Parent.Name == "parent");

                Assert.Equal(testTreeChild.Id, child.Id);
            }
        }

        [Fact]
        public void AbleToQueryChildren()
        {
            var testTreeParent = new TestTreeEntry {Name = "parent"};
            var testTreeChild = new TestTreeEntry {Name = "child"};

            testTreeParent.AddChild(testTreeChild);
            using (var uow = factory.Create())
            {
                uow.Save(testTreeParent);
                uow.Commit();
            }

            using (var uow = factory.Create())
            {
                var parent = uow.Query<TestTreeEntry>()
                    .Single(e => e.Children.Any(a => a.Name == "child"));

                Assert.Equal(testTreeParent.Id, parent.Id);
            }
        }

        [Fact]
        public void AbleToQueryAncestors()
        {
            var testTreeParent = new TestTreeEntry {Name = "parent"};
            var testTreeChild = new TestTreeEntry {Name = "child"};

            testTreeParent.AddChild(testTreeChild);
            using (var uow = factory.Create())
            {
                uow.Save(testTreeParent);
                uow.Commit();
            }

            using (var uow = factory.Create())
            {
                var child = uow.Query<TestTreeEntry>()
                    .Single(e => e.Ancestors.Any(a => a.Name == "parent"));

                Assert.Equal(testTreeChild.Id, child.Id);
            }
        }

        [Fact]
        public void AbleToQueryDescendants()
        {
            var testTreeParent = new TestTreeEntry {Name = "parent"};
            var testTreeChild = new TestTreeEntry {Name = "child"};

            testTreeParent.AddChild(testTreeChild);
            using (var uow = factory.Create())
            {
                uow.Save(testTreeParent);
                uow.Commit();
            }

            using (var uow = factory.Create())
            {
                var parent = uow.Query<TestTreeEntry>()
                    .Single(e => e.Descendants.Any(a => a.Name == "child"));

                Assert.Equal(testTreeParent.Id, parent.Id);
            }
        }

        [Fact]
        public void ClearDescendantsAndAncestorsWhenClearingParent()
        {
            var testTreeParent = new TestTreeEntry {Name = "parent"};
            var testTreeChild = new TestTreeEntry {Name = "child"};
            testTreeParent.AddChild(testTreeChild);
            using (var uow = factory.Create())
            {
                uow.Save(testTreeParent);
                uow.Commit();
            }

            Console.WriteLine("BEFORE CLEAR PARENT");
            using (var uow = factory.Create())
            {
                uow.Get<TestTreeEntry>(testTreeChild.Id).Parent = null;
                uow.Commit();
            }
            Console.WriteLine("AFTER CLEAR PARENT");

            using (var uow = factory.Create())
            {
                var child = uow.Get<TestTreeEntry>(testTreeChild.Id);
                var parent = uow.Get<TestTreeEntry>(testTreeParent.Id);

                Assert.Equal(0, parent.Children.Count());
                Assert.Equal(0, parent.Descendants.Count());
                Assert.Null(child.Parent);
                Assert.Equal(0, parent.Ancestors.Count());
            }
        }

        [Fact]
        public void ManipulateGrandChildren()
        {
            var parent = new TestTreeEntry {Name = "parent"};
            var child = new TestTreeEntry {Name = "child", Parent = parent};
            var grandChild1 = new TestTreeEntry {Name = "gc1", Parent = child};
            var grandChild2 = new TestTreeEntry {Name = "gc2", Parent = child};

            using (var uow = factory.Create())
            {
                uow.Save(parent);
                uow.Commit();
            }

            using (var uow = factory.Create())
            {
                var p = uow.Get<TestTreeEntry>(parent.Id);
                var c = uow.Get<TestTreeEntry>(child.Id);
                var gc1 = uow.Get<TestTreeEntry>(grandChild1.Id);
                var gc2 = uow.Get<TestTreeEntry>(grandChild2.Id);

                p.IsParentOf(c);
                c.IsParentOf(gc1);
                p.IsAncestorOf(gc1);
                c.IsParentOf(gc2);
                p.IsAncestorOf(gc2);

                c.Parent = null;

                gc1.AddChild(p);
                uow.Commit();
            }

            using (var uow = factory.Create())
            {
                var p = uow.Get<TestTreeEntry>(parent.Id);
                var c = uow.Get<TestTreeEntry>(child.Id);
                var gc1 = uow.Get<TestTreeEntry>(grandChild1.Id);
                var gc2 = uow.Get<TestTreeEntry>(grandChild2.Id);

                Assert.Null(c.Parent);
                c.IsParentOf(gc1);
                c.IsParentOf(gc2);
                c.IsAncestorOf(p);
                gc1.IsParentOf(p);
            }
        }

        public void Dispose()
        {
            try
            {
                using (var uow = factory.Create())
                {
                    foreach (var entry in uow.Query<TestTreeEntry>().ToList())
                    {
                        entry.Parent = null;
                        foreach (var child in entry.Children.ToList())
                            entry.RemoveChild(child);
                        uow.Delete(entry);
                    }
                    uow.Commit();
                }

            }
            catch
            {
                //do nothing
            }
        }
    }
}