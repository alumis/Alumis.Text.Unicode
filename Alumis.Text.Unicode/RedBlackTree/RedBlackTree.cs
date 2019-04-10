using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Alumis.Text.Unicode
{
    static class RedBlackTree
    {
        public static void Insert<TValue>(RedBlackTreeNode<TValue> node, ref RedBlackTreeNode<TValue> root, Comparison<RedBlackTreeNode<TValue>> comparer, RedBlackTreeNode<TValue> nil = null)
        {
            node.Left = node.Right = nil;

            var parent = root;

            if (parent == null)
            {
                (root = node).Parent = null;
                return;
            }

            for (; ; )
            {
                var c = comparer(parent, node);

                Debug.Assert(c != 0);

                if (0 < c)
                {
                    if (parent.Left == nil)
                    {
                        (parent.Left = node).Parent = parent;
                        return;
                    }

                    parent = parent.Left;
                }

                else
                {
                    if (parent.Right == nil)
                    {
                        (parent.Right = node).Parent = parent;
                        return;
                    }

                    parent = parent.Right;
                }
            }
        }

        public static void InsertLeft<TValue>(RedBlackTreeNode<TValue> node, ref RedBlackTreeNode<TValue> root, RedBlackTreeNode<TValue> nil = null)
        {
            node.Left = node.Right = nil;

            var parent = root;

            if (parent == null)
            {
                (root = node).Parent = null;
                return;
            }

            for (; ; )
            {
                if (parent.Left == nil)
                {
                    (parent.Left = node).Parent = parent;
                    return;
                }

                parent = parent.Left;
            }
        }

        public static void InsertRight<TValue>(RedBlackTreeNode<TValue> node, ref RedBlackTreeNode<TValue> root, RedBlackTreeNode<TValue> nil = null)
        {
            node.Left = node.Right = nil;

            var parent = root;

            if (parent == null)
            {
                (root = node).Parent = null;
                return;
            }

            for (; ; )
            {
                if (parent.Right == nil)
                {
                    (parent.Right = node).Parent = parent;
                    return;
                }

                parent = parent.Right;
            }
        }

        public static void RotateLeft<TValue>(RedBlackTreeNode<TValue> node, ref RedBlackTreeNode<TValue> root)
        {
            var parent = node.Parent;
            var right = node.Right;

            Debug.Assert(right != null);

            if (parent != null)
            {
                if (node == parent.Left)
                    parent.Left = right;

                else parent.Right = right;

                right.Parent = parent;
            }

            else
            {
                root = right;
                right.Parent = null;
            }

            node.Parent = right;
            parent = right.Left;
            right.Left = node;
            node.Right = parent;

            if (parent != null)
                parent.Parent = node;
        }

        public static void RotateRight<TValue>(RedBlackTreeNode<TValue> node, ref RedBlackTreeNode<TValue> root)
        {
            var parent = node.Parent;
            var left = node.Left;

            Debug.Assert(left != null);

            if (parent != null)
            {
                if (node == parent.Right)
                    parent.Right = left;

                else parent.Left = left;

                left.Parent = parent;
            }

            else
            {
                root = left;
                left.Parent = null;
            }

            node.Parent = left;
            parent = left.Right;
            left.Right = node;
            node.Left = parent;

            if (parent != null)
                parent.Parent = node;
        }

        public static void Balance<TValue>(RedBlackTreeNode<TValue> node, ref RedBlackTreeNode<TValue> root, RedBlackTreeNode<TValue> nil = null)
        {
            var parent = node.Parent;

            if (parent == null)
            {
                node.Color = RedBlackTreeNodeColor.Black;
                return;
            }

            node.Color = RedBlackTreeNodeColor.Red;

            // Case 2

            if (parent.Color == RedBlackTreeNodeColor.Black)
                return;

            case3:

            var grandparent = parent.Parent;

            grandparent = ((parent == grandparent.Left) ? grandparent.Right : grandparent.Left);

            if (grandparent != null && grandparent.Color == RedBlackTreeNodeColor.Red)
            {
                parent.Color = grandparent.Color = RedBlackTreeNodeColor.Black;
                node = grandparent.Parent;
                node.Color = RedBlackTreeNodeColor.Red;
                parent = node.Parent;

                // Case 1

                if (parent == null)
                {
                    node.Color = RedBlackTreeNodeColor.Black;
                    return;
                }

                // Case 2

                if (parent.Color == RedBlackTreeNodeColor.Black)
                    return;

                goto case3;
            }

            else grandparent = parent.Parent;

            // Case 4

            if (node == parent.Right)
            {
                if (parent == grandparent.Left)
                {
                    RotateLeft(parent, ref root);

                    parent = node;
                    grandparent = parent.Parent;
                    node = node.Left;

                }
            }

            else if (parent == grandparent.Right)
            {
                RotateRight(parent, ref root);

                parent = node;
                grandparent = parent.Parent;
                node = node.Right;
            }

            // Case 5

            parent.Color = RedBlackTreeNodeColor.Black;
            grandparent.Color = RedBlackTreeNodeColor.Red;

            if (node == parent.Left)
                RotateRight(grandparent, ref root);

            else RotateLeft(grandparent, ref root);
        }

        public static RedBlackTreeNode<TValue> Copy<TValue>(RedBlackTreeNode<TValue> root)
        {
            if (root == null)
                return null;

            return root.Copy();
        }
    }
}
