using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Alumis.Text.Unicode
{
    class RedBlackTreeNode<TValue> : IEnumerable<TValue>
    {
        public TValue Value;
        public RedBlackTreeNode<TValue> Parent, Left, Right;
        public RedBlackTreeNodeColor Color;

        public RedBlackTreeNode<TValue> Leftmost
        {
            get
            {
                var node = this;

                for (; node.Left != null; node = node.Left)
                    ;

                return node;
            }
        }

        public RedBlackTreeNode<TValue> Rightmost
        {
            get
            {
                var node = this;

                for (; node.Right != null; node = node.Right)
                    ;

                return node;
            }
        }

        public RedBlackTreeNode<TValue> Next
        {
            get
            {
                if (Right != null)
                    return Right.Leftmost;

                if (Parent == null)
                    return null;

                if (Parent.Left == this)
                    return Parent;

                else
                {
                    Debug.Assert(Parent.Right == this);

                    var node = Parent;

                    for (; node.Parent != null && node.Parent.Right == node; node = node.Parent)
                        ;

                    return node.Parent;
                }
            }
        }

        public override string ToString()
        {
            return Value != null ? Value.ToString() : null;
        }

        public RedBlackTreeNode<TValue> Copy()
        {
            var node = new RedBlackTreeNode<TValue>() { Value = Value, Color = Color };

            if (Left != null)
                (node.Left = Left.Copy()).Parent = node;

            if (Right != null)
                (node.Right = Right.Copy()).Parent = node;

            return node;
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            for (var next = Leftmost; next != null; next = next.Next)
                yield return next.Value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (var next = Leftmost; next != null; next = next.Next)
                yield return next.Value;
        }
    }
}
