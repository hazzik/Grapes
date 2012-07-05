namespace Brandy.Grapes
{
    using System;
    using System.Collections.Generic;

    public abstract class TreeEntry<T>
        where T : TreeEntry<T>
    {
        protected bool UpdateHierarchy;
        readonly ICollection<T> ancestors = new List<T>();
        readonly ICollection<T> children = new List<T>();
        readonly ICollection<T> descendants = new List<T>();
        T parent;

        protected TreeEntry()
            : this(true)
        {
        }

        protected TreeEntry(bool updateHierarchy)
        {
            UpdateHierarchy = updateHierarchy;
        }

        public virtual T Parent
        {
            get { return parent; }
            set
            {
                if (UpdateHierarchy && parent != null)
                {
                    var collection = parent.children;
                    collection.Remove(This);
                    UpdateAncestorDescendantRelation(parent, This, false);
                }
                parent = value;
                if (UpdateHierarchy && parent != null)
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

        static void UpdateAncestorDescendantRelation(T ancestor, T descendant, bool addRelation)
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
