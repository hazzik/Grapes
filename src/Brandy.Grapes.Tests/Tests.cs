using System;
using Xunit;

namespace Brandy.Grapes.Tests
{
    public class Tests
    {
        [Fact]
        public void CanSetParent()
        {
            var parent = new TestTreeEntry {Name = "parent"};
            var child = new TestTreeEntry {Name = "child", Parent = parent};

            parent.IsParentOf(child);
        }

        [Fact]
        public void CanSetParentToNull()
        {
            var parent = new TestTreeEntry {Name = "parent"};
            var child = new TestTreeEntry {Name = "child", Parent = parent};
            child.Parent = null;

            parent.IsNotParentOf(child);
            Assert.Null(child.Parent);
        }

        [Fact]
        public void CanSwitchParent()
        {
            var parent1 = new TestTreeEntry {Name = "parent1"};
            var parent2 = new TestTreeEntry {Name = "parent1"};
            var child = new TestTreeEntry {Name = "child", Parent = parent1};
            child.Parent = parent2;

            parent1.IsNotParentOf(child);

            parent2.IsParentOf(child);
        }

        [Fact]
        public void CanAddChild()
        {
            var parent = new TestTreeEntry {Name = "parent"};
            var child = new TestTreeEntry {Name = "child"};

            parent.AddChild(child);

            parent.IsParentOf(child);
        }

        [Fact]
        public void CanAddGrandChild()
        {
            var parent = new TestTreeEntry {Name = "parent"};
            var child = new TestTreeEntry {Name = "child"};
            var grandChild = new TestTreeEntry {Name = "grandChild"};

            parent.AddChild(child);
            child.AddChild(grandChild);

            parent.IsParentOf(child);
            child.IsParentOf(grandChild);
            parent.IsAncestorOf(grandChild);
        }

        [Fact]
        public void CanRemoveChild()
        {
            var parent = new TestTreeEntry {Name = "parent"};
            var child = new TestTreeEntry {Name = "child", Parent = parent};

            parent.RemoveChild(child);

            parent.IsNotParentOf(child);
            Assert.Null(child.Parent);
        }

        [Fact]
        public void WhenRemovingGrandChild()
        {
            var parent = new TestTreeEntry {Name = "parent"};
            var child = new TestTreeEntry {Name = "child", Parent = parent};
            var grandChild = new TestTreeEntry {Name = "grandChild", Parent = child};

            child.RemoveChild(grandChild);

            parent.IsParentOf(child);
            child.IsNotParentOf(grandChild);
            parent.IsNotAncestorOf(grandChild);
        }
    }
}