using System;
using System.Collections.Generic;
using System.Text;

namespace Alumis.Text.Unicode
{
    partial class GraphemeString
    {
        int? _length;

        public int Length
        {
            get
            {
                if (_length != null)
                    return _length.Value;

                Index();

                var length = Clusters == null ?
                    Value.Length :
                    Clusters.Rightmost.Value.Interval.IndexUpper;

                _length = length;

                return length;
            }
        }
    }
}