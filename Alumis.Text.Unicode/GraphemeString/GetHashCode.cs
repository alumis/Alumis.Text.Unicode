using System;
using System.Collections.Generic;
using System.Text;

namespace Alumis.Text.Unicode
{
    partial class GraphemeString
    {
        int? _hashCode;

        public override int GetHashCode()
        {
            if (_hashCode != null)
                return _hashCode.Value;

            Normalize();

            var hashCode = _value.GetHashCode();

            _hashCode = hashCode;

            return hashCode;
        }
    }
}