using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alumis.Text.Unicode
{
    partial class GraphemeString
    {
        internal class GraphemeStringEnumerator : IEnumerator<string>
        {
            public GraphemeStringEnumerator(GraphemeString str)
            {
                (_string = str).Index();
                Reset();
            }

            GraphemeString _string;

            internal struct Position
            {
                public RedBlackTreeNode<GraphemeCluster> GraphemeCluster;
                public int Index;
            }

            internal Position _position;

            public string _current;

            //RedBlackTreeNode<GraphemeCluster> _state.CurrentGraphemeCluster;
            //int _state.CurrentIndex;
            //string _current;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Reset()
            {
                _position.GraphemeCluster = _string._clusters?.Leftmost;
                _position.Index = 0;
            }

            public bool MoveNext()
            {
                if (_position.GraphemeCluster == null)
                {
                    if (_position.Index < _string._value.Length)
                    {
                        _current = _string._value[_position.Index++].ToString();
                        return true;
                    }

                    return false;
                }

                if (_position.Index < _position.GraphemeCluster.Value.Interval.Index)
                {
                    _current = _string._value[_position.Index++].ToString();
                    return true;
                }

                if (_position.GraphemeCluster.Value.Interval.IndexUpper <= _position.Index)
                {
                    if ((_position.GraphemeCluster = _position.GraphemeCluster.Next) == null)
                        return false;
                }

                if (_position.GraphemeCluster.Value.Interval.Length == 1)
                {
                    _current = _string._value.Substring(_position.GraphemeCluster.Value.CodeUnitsInterval.Index, _position.GraphemeCluster.Value.CodeUnitsInterval.Length);
                    _position.Index++;

                    return true;
                }

                _current = _string._value[_position.GraphemeCluster.Value.CodeUnitsInterval.Index + (_position.Index++ - _position.GraphemeCluster.Value.Interval.Index)].ToString();
                return true;
            }

            public string Current => _current;

            object IEnumerator.Current => _current;

            public void Dispose()
            {
            }
        }
    }
}