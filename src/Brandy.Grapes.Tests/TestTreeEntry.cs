using System;

namespace Brandy.Grapes.Tests
{
    public class TestTreeEntry : TreeEntry<TestTreeEntry>
    {
        public virtual string Name { get; set; }

        public virtual int Id { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}