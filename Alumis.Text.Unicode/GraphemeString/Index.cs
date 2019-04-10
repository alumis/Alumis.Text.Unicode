using Alumis.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alumis.Text.Unicode
{
    partial class GraphemeString
    {
        bool _hasIndexed;

        public void Index()
        {
            if (_hasIndexed)
                return;

            Normalize();

            var codeUnitsIndex = 0;
            var codeUnitsIndexUpper = Value.Length;

            RedBlackTreeNode<GraphemeSurrogate> surrogate = null;
            RedBlackTreeNode<GraphemeCluster> graphemeCluster = null;

            var cpIndex = 0;
            var graphemeClusterIndex = 0;

            uint Read()
            {
                var codeUnit = Value[codeUnitsIndex];

                if ((codeUnit & 0xfffffc00) == 0xd800) // U16_IS_LEAD
                {
                    surrogate = new RedBlackTreeNode<GraphemeSurrogate>() { Value = new GraphemeSurrogate() { CodePointsIndex = cpIndex++, CodePointsIndexUpper = cpIndex, CodeUnits = codeUnitsIndex } };

                    RedBlackTree.InsertRight(surrogate, ref Surrogates);
                    RedBlackTree.Balance(surrogate, ref Surrogates);

                    var cp = ((uint)codeUnit << 10) + Value[codeUnitsIndex + 1] - ((0xd800 << 10) + 0xdc00 - 0x10000); // U16_GET_SUPPLEMENTARY

                    codeUnitsIndex += 2;

                    return cp;
                }

                if (surrogate != null)
                    ++surrogate.Value.CodePointsIndexUpper;

                ++cpIndex;

                return Value[codeUnitsIndex++];
            }

            void Forward()
            {
                if ((Value[codeUnitsIndex] & 0xfffffc00) == 0xd800) // U16_IS_LEAD
                {
                    surrogate = new RedBlackTreeNode<GraphemeSurrogate>() { Value = new GraphemeSurrogate() { CodePointsIndex = cpIndex++, CodePointsIndexUpper = cpIndex, CodeUnits = codeUnitsIndex } };

                    RedBlackTree.InsertRight(surrogate, ref Surrogates);
                    RedBlackTree.Balance(surrogate, ref Surrogates);

                    codeUnitsIndex += 2;
                }

                if (surrogate != null)
                    ++surrogate.Value.CodePointsIndexUpper;

                ++cpIndex;
                ++codeUnitsIndex;
            }

            uint Peek(int index)
            {
                var codeUnit = Value[index];

                if ((codeUnit & 0xfffffc00) == 0xd800) // U16_IS_LEAD
                    return ((uint)codeUnit << 10) + Value[index + 1] - ((0xd800 << 10) + 0xdc00 - 0x10000); // U16_GET_SUPPLEMENTARY

                return codeUnit;
            }

            void IndexGraphemeCluster(int codeUnitsStart, int index)
            {
                if (codeUnitsStart - index == 2)
                {
                    var gc = new RedBlackTreeNode<GraphemeCluster>() { Value = new GraphemeCluster() { Interval = new UnicodeInterval(graphemeClusterIndex++, graphemeClusterIndex), CodeUnitsInterval = new UnicodeInterval(index, codeUnitsStart) } };

                    RedBlackTree.InsertRight(gc, ref Clusters);
                    RedBlackTree.Balance(gc, ref Clusters);

                    graphemeCluster = null;
                }

                else if (graphemeCluster == null && Clusters != null)
                {
                    graphemeCluster = new RedBlackTreeNode<GraphemeCluster>() { Value = new GraphemeCluster() { Interval = new UnicodeInterval(graphemeClusterIndex++, graphemeClusterIndex), CodeUnitsInterval = new UnicodeInterval(index, codeUnitsStart) } };

                    RedBlackTree.InsertRight(graphemeCluster, ref Clusters);
                    RedBlackTree.Balance(graphemeCluster, ref Clusters);
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

                            var gc = new RedBlackTreeNode<GraphemeCluster>() { Value = new GraphemeCluster() { Interval = new UnicodeInterval(graphemeClusterIndex++, graphemeClusterIndex), CodeUnitsInterval = new UnicodeInterval(index, codeUnitsIndex) } };

                            RedBlackTree.InsertRight(gc, ref Clusters);
                            RedBlackTree.Balance(gc, ref Clusters);

                            graphemeCluster = null;

                            goto loop;
                        }

                        graphemeCluster = new RedBlackTreeNode<GraphemeCluster>() { Value = new GraphemeCluster() { Interval = new UnicodeInterval(graphemeClusterIndex, graphemeClusterIndex + 1), CodeUnitsInterval = new UnicodeInterval(index, codeUnitsIndex) } };

                        RedBlackTree.InsertRight(graphemeCluster, ref Clusters);
                        RedBlackTree.Balance(graphemeCluster, ref Clusters);

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
