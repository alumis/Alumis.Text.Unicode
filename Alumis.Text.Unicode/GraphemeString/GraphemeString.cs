using Alumis.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alumis.Text.Unicode
{
    public partial class GraphemeString : IEnumerable<string>
    {
        public GraphemeString(string value)
        {
            Value = value;
        }

        public string Value;

        public RedBlackTreeNode<GraphemeSurrogate> Surrogates;
        public RedBlackTreeNode<GraphemeCluster> Clusters;

        public GraphemeString this[int index]
        {
            get
            {
                return Substring(index, 1);
            }
        }

        public override string ToString()
        {
            return Value;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return new GraphemeStringEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator GraphemeString(string str)
        {
            return new GraphemeString(str);
        }
    }
}
