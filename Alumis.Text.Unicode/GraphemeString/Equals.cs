using System;
using System.Collections.Generic;
using System.Text;

namespace Alumis.Text.Unicode
{
    partial class GraphemeString
    {
        public override bool Equals(object obj)
        {
            var str = obj as GraphemeString;

            if (str == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            Normalize();
            str.Normalize();

            if (_hashCode != null && str._hashCode != null && _hashCode.Value != str._hashCode.Value)
                return false;

            return _value.Equals(str._value);
        }
    }
}