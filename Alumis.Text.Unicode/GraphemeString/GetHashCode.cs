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

            var hashCode = Value.GetHashCode();

            _hashCode = hashCode;

            return hashCode;
        }
    }
}