using Alumis.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alumis.Text.Unicode
{
    public class GraphemeStringEnumerator : IEnumerator<string>
    {
        public GraphemeStringEnumerator(GraphemeString str)
        {
            (_string = str).Index();
            Reset();
        }

        GraphemeString _string;

        public GraphemeStringEnumeratorPosition Position;

        string _current;

        //RedBlackTreeNode<GraphemeCluster> _state.CurrentGraphemeCluster;
        //int _state.CurrentIndex;
        //string _current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            Position.GraphemeCluster = _string.Clusters?.Leftmost;
            Position.Index = 0;
        }

        public bool MoveNext()
        {
            if (Position.GraphemeCluster == null)
            {
                if (Position.Index < _string.Value.Length)
                {
                    _current = _string.Value[Position.Index++].ToString();
                    return true;
                }

                return false;
            }

            if (Position.Index < Position.GraphemeCluster.Value.Interval.Index)
            {
                _current = _string.Value[Position.Index++].ToString();
                return true;
            }

            if (Position.GraphemeCluster.Value.Interval.IndexUpper <= Position.Index)
            {
                if ((Position.GraphemeCluster = Position.GraphemeCluster.Next) == null)
                    return false;
            }

            if (Position.GraphemeCluster.Value.Interval.Length == 1)
            {
                _current = _string.Value.Substring(Position.GraphemeCluster.Value.CodeUnitsInterval.Index, Position.GraphemeCluster.Value.CodeUnitsInterval.Length);
                Position.Index++;

                return true;
            }

            _current = _string.Value[Position.GraphemeCluster.Value.CodeUnitsInterval.Index + (Position.Index++ - Position.GraphemeCluster.Value.Interval.Index)].ToString();
            return true;
        }

        public string Current => _current;

        object IEnumerator.Current => _current;

        public void Dispose()
        {
        }
    }

    public struct GraphemeStringEnumeratorPosition
    {
        public RedBlackTreeNode<GraphemeCluster> GraphemeCluster;
        public int Index;
    }
}