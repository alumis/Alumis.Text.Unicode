using System;
using System.Collections.Generic;
using System.Text;

namespace Alumis.Text.Unicode
{
    partial class GraphemeString
    {
        bool _hasIndexed;

        void Index()
        {
            if (_hasIndexed)
                return;

            Normalize();

            var codeUnitsIndex = 0;
            var codeUnitsIndexUpper = _value.Length;

            RedBlackTreeNode<GraphemeSurrogate> surrogate = null;
            RedBlackTreeNode<GraphemeCluster> graphemeCluster = null;

            var cpIndex = 0;
            var graphemeClusterIndex = 0;

            uint Read()
            {
                var codeUnit = _value[codeUnitsIndex];

                if ((codeUnit & 0xfffffc00) == 0xd800) // U16_IS_LEAD
                {
                    surrogate = new RedBlackTreeNode<GraphemeSurrogate>() { Value = new GraphemeSurrogate() { CodePointsIndex = cpIndex++, CodePointsIndexUpper = cpIndex, CodeUnits = codeUnitsIndex } };

                    RedBlackTree.InsertRight(surrogate, ref _surrogates);
                    RedBlackTree.Balance(surrogate, ref _surrogates);

                    var cp = ((uint)codeUnit << 10) + _value[codeUnitsIndex + 1] - ((0xd800 << 10) + 0xdc00 - 0x10000); // U16_GET_SUPPLEMENTARY

                    codeUnitsIndex += 2;

                    return cp;
                }

                if (surrogate != null)
                    ++surrogate.Value.CodePointsIndexUpper;

                ++cpIndex;

                return _value[codeUnitsIndex++];
            }

            void Forward()
            {
                if ((_value[codeUnitsIndex] & 0xfffffc00) == 0xd800) // U16_IS_LEAD
                {
                    surrogate = new RedBlackTreeNode<GraphemeSurrogate>() { Value = new GraphemeSurrogate() { CodePointsIndex = cpIndex++, CodePointsIndexUpper = cpIndex, CodeUnits = codeUnitsIndex } };

                    RedBlackTree.InsertRight(surrogate, ref _surrogates);
                    RedBlackTree.Balance(surrogate, ref _surrogates);

                    codeUnitsIndex += 2;
                }

                if (surrogate != null)
                    ++surrogate.Value.CodePointsIndexUpper;

                ++cpIndex;
                ++codeUnitsIndex;
            }

            uint Peek(int index)
            {
                var codeUnit = _value[index];

                if ((codeUnit & 0xfffffc00) == 0xd800) // U16_IS_LEAD
                    return ((uint)codeUnit << 10) + _value[index + 1] - ((0xd800 << 10) + 0xdc00 - 0x10000); // U16_GET_SUPPLEMENTARY

                return codeUnit;
            }

            void IndexGraphemeCluster(int codeUnitsStart, int index)
            {
                if (codeUnitsStart - index == 2)
                {
                    var gc = new RedBlackTreeNode<GraphemeCluster>() { Value = new GraphemeCluster() { Interval = new Interval(graphemeClusterIndex++, graphemeClusterIndex), CodeUnitsInterval = new Interval(index, codeUnitsStart) } };

                    RedBlackTree.InsertRight(gc, ref _clusters);
                    RedBlackTree.Balance(gc, ref _clusters);

                    graphemeCluster = null;
                }

                else if (graphemeCluster == null && _clusters != null)
                {
                    graphemeCluster = new RedBlackTreeNode<GraphemeCluster>() { Value = new GraphemeCluster() { Interval = new Interval(graphemeClusterIndex++, graphemeClusterIndex), CodeUnitsInterval = new Interval(index, codeUnitsStart) } };

                    RedBlackTree.InsertRight(graphemeCluster, ref _clusters);
                    RedBlackTree.Balance(graphemeCluster, ref _clusters);
                }

                else
                {
                    ++graphemeClusterIndex;

                    if (graphemeCluster != null)
                    {
                        ++graphemeCluster.Value.Interval.IndexUpper;
                        ++graphemeCluster.Value.CodeUnitsInterval.IndexUpper;
                    }
                }
            }

            loop:

            while (codeUnitsIndex < codeUnitsIndexUpper)
            {
                var index = codeUnitsIndex;
                var a = Read();

                if (codeUnitsIndex < codeUnitsIndexUpper)
                {
                    var b = Peek(codeUnitsIndex);

                    if (GraphemeCluster.Extends(a, b))
                    {
                        for (Forward(); codeUnitsIndex < codeUnitsIndexUpper;)
                        {
                            a = Peek(codeUnitsIndex);

                            if (GraphemeCluster.Extends(b, a))
                            {
                                Forward();

                                if (codeUnitsIndex < codeUnitsIndexUpper)
                                {
                                    b = Peek(codeUnitsIndex);

                                    if (GraphemeCluster.Extends(a, b))
                                    {
                                        Forward();
                                        continue;
                                    }
                                }

                                else break;
                            }

                            var gc = new RedBlackTreeNode<GraphemeCluster>() { Value = new GraphemeCluster() { Interval = new Interval(graphemeClusterIndex++, graphemeClusterIndex), CodeUnitsInterval = new Interval(index, codeUnitsIndex) } };

                            RedBlackTree.InsertRight(gc, ref _clusters);
                            RedBlackTree.Balance(gc, ref _clusters);

                            graphemeCluster = null;

                            goto loop;
                        }

                        graphemeCluster = new RedBlackTreeNode<GraphemeCluster>() { Value = new GraphemeCluster() { Interval = new Interval(graphemeClusterIndex, graphemeClusterIndex + 1), CodeUnitsInterval = new Interval(index, codeUnitsIndex) } };

                        RedBlackTree.InsertRight(graphemeCluster, ref _clusters);
                        RedBlackTree.Balance(graphemeCluster, ref _clusters);

                        _hasIndexed = true;
                        return;
                    }

                    else IndexGraphemeCluster(codeUnitsIndex, index);
                }

                else
                {
                    IndexGraphemeCluster(codeUnitsIndex, index);
                    _hasIndexed = true;
                    return;
                }
            }

            _hasIndexed = true;
        }
    }
}
