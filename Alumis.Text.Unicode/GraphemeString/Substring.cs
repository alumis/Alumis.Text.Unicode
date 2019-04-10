using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alumis.Text.Unicode
{
    partial class GraphemeString
    {
        public GraphemeString Substring(int startIndex, int length)
        {
            var codeUnits = GetSubstringCodeUnits(startIndex, length);
            var str = new GraphemeString(_value.Substring(codeUnits.Index, codeUnits.Length));

            return str;
        }

        public string NativeSubstring(int startIndex, int length)
        {
            var codeUnits = GetSubstringCodeUnits(startIndex, length);
            var size = codeUnits.Length;
            var chars = new char[size];

            _value.CopyTo(codeUnits.Index, chars, 0, size);

            return new string(chars);
        }

        Interval GetSubstringCodeUnits(int startIndex, int length)
        {
            Index();

            var indexUpper = startIndex + length;
            var graphemeClusters = _clusters;

            if (graphemeClusters == null)
                return new Interval(startIndex, indexUpper);

            int codeUnitsIndex = 0, codeUnitsIndexUpper;

            for (; ; )
            {
                if (startIndex < graphemeClusters.Value.Interval.Index)
                {
                    if (graphemeClusters.Left != null)
                        graphemeClusters = graphemeClusters.Left;

                    else
                    {
                        codeUnitsIndex = startIndex;

                        if (indexUpper <= graphemeClusters.Value.Interval.Index)
                            return new Interval(codeUnitsIndex, indexUpper);

                        break;
                    }
                }
                else if (graphemeClusters.Value.Interval.IndexUpper <= startIndex)
                    graphemeClusters = graphemeClusters.Right;

                else
                {

                    codeUnitsIndex = graphemeClusters.Value.CodeUnitsInterval.Index + (startIndex - graphemeClusters.Value.Interval.Index);

                    if (indexUpper <= graphemeClusters.Value.Interval.IndexUpper)
                    {
                        if (length == 0)
                            return new Interval(codeUnitsIndex, codeUnitsIndex);

                        else if (graphemeClusters.Value.Interval.Length == 1)
                            codeUnitsIndexUpper = graphemeClusters.Value.CodeUnitsInterval.IndexUpper;

                        else codeUnitsIndexUpper = graphemeClusters.Value.CodeUnitsInterval.Index + (indexUpper - graphemeClusters.Value.Interval.Index);

                        return new Interval(codeUnitsIndex, codeUnitsIndexUpper);
                    }

                    break;
                }
            }

            for (graphemeClusters = _clusters; ;)
            {
                if (indexUpper < graphemeClusters.Value.Interval.Index)
                    graphemeClusters = graphemeClusters.Left;

                else if (graphemeClusters.Value.Interval.IndexUpper < indexUpper)
                    graphemeClusters = graphemeClusters.Right;

                else
                {
                    if (length == 0)
                        return new Interval(codeUnitsIndex, codeUnitsIndex);

                    else if (graphemeClusters.Value.Interval.Length == 1)
                        codeUnitsIndexUpper = graphemeClusters.Value.CodeUnitsInterval.IndexUpper;

                    else codeUnitsIndexUpper = graphemeClusters.Value.CodeUnitsInterval.Index + (indexUpper - graphemeClusters.Value.Interval.Index);

                    return new Interval(codeUnitsIndex, codeUnitsIndexUpper);
                }
            }
        }
    }
}
