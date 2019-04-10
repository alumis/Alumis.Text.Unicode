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
            _value = value;
        }

        string _value;

        RedBlackTreeNode<GraphemeSurrogate> _surrogates;
        RedBlackTreeNode<GraphemeCluster> _clusters;

        public GraphemeString this[int index]
        {
            get
            {
                return Substring(index, 1);
            }
        }

        public override string ToString()
        {
            return _value;
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
