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

                var length = _clusters == null ?
                    _value.Length :
                    _clusters.Rightmost.Value.Interval.IndexUpper;

                _length = length;

                return length;
            }
        }
    }
}