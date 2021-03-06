﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alumis.Text.Unicode
{
    partial class GraphemeString
    {
        bool _isNormalized;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            if (_isNormalized)
                return;

            Value = Value.Normalize(NormalizationForm.FormD);
            _isNormalized = true;
        }
    }
}
