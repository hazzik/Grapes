namespace Brandy.Grapes
{
    using System;
    using System.Collections.Generic;

    public abstract class TreeEntry<T> where T : TreeEntry<T>
    {
        private readonly ICollection<T> ancestors;
        private readonly ICollection<T> children;
        private readonly ICollection<T> descendants;
        private T parent;

        protected TreeEntry()
        {
            children = new List<T>();
            ancestors = new List<T>();
            descendants = new List<T>();
        }

        public virtual T Parent
        {
            get { return parent; }
            set
            {
                if (parent != null)
                {
                    var collection = parent.children;
                    collection.Remove(This);
                    UpdateAncestorDescendantRelation(parent, This, false);
                }
                parent = value;
                if (parent != null)
                {
                    var collection = parent.children;
                    collection.Add(This);
                    UpdateAncestorDescendantRelation(parent, This, true);
                }
            }
        }

        public virtual IEnumerable<T> Children
        {
            get { return children; }
        }

        public virtual IEnumerable<T> Ancestors
        {
            get { return ancestors; }
        }

        public virtual IEnumerable<T> Descendants
        {
            get { return descendants; }
        }

        protected T This
        {
            get { return (T) this; }
        }

        public virtual void AddChild(T child)
        {
            if (child == null) throw new ArgumentNullException("child");
            child.Parent = This;
        }

        public virtual void RemoveChild(T child)
        {
            if (child == null) throw new ArgumentNullException("child");
            child.Parent = null;
        }

        private static void UpdateAncestorDescendantRelation(T ancestor, T descendant, bool addRelation)
        {
            if (ancestor.parent != null)
                UpdateAncestorDescendantRelation(ancestor.parent, descendant, addRelation);
            foreach (var grandDescendant in descendant.children)
                UpdateAncestorDescendantRelation(ancestor, grandDescendant, addRelation);
            var ancestorDescendants = ancestor.descendants;
            var descendantAncestors = descendant.ancestors;
            if (addRelation)
            {
                ancestorDescendants.Add(descendant);
                descendantAncestors.Add(ancestor);
            }
            else
            {
                ancestorDescendants.Remove(descendant);
                descendantAncestors.Remove(ancestor);
            }
        }
    }
}