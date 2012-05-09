using System;
using Xunit;

namespace Brandy.Grapes.Tests
{
    public static class TestTreeEntryAsserts
    {
        public static void IsParentOf(this TestTreeEntry parent, TestTreeEntry child)
        {
            Assert.Equal(parent, child.Parent);
            Assert.Contains(child, parent.Children);
            parent.IsAncestorOf(child);
        }

        public static void IsNotParentOf(this TestTreeEntry parent, TestTreeEntry child)
        {
            Assert.NotEqual(parent, child);
            Assert.DoesNotContain(child, parent.Children);
            parent.IsNotAncestorOf(child);
        }

        public static void IsNotAncestorOf(this TestTreeEntry parent, TestTreeEntry child)
        {
            Assert.DoesNotContain(parent, child.Ancestors);
            Assert.DoesNotContain(child, parent.Descendants);
        }

        public static void IsAncestorOf(this TestTreeEntry parent, TestTreeEntry child)
        {
            Assert.Contains(parent, child.Ancestors);
            Assert.Contains(child, parent.Descendants);
        }
    }
}