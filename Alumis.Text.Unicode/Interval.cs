using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alumis.Text.Unicode
{
    struct Interval
    {
        public int Index, IndexUpper;

        public Interval(int index, int indexUpper)
        {
            Index = index;
            IndexUpper = indexUpper;
        }

        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return IndexUpper - Index;
            }
        }

        public override string ToString()
        {
            return $"[{Index}, {IndexUpper})";
        }
    }
}
