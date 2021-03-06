﻿using System;
using Xunit;

namespace Lunt.Tests
{
    public static class ShouldExtensions
    {
        public static ArgumentException ShouldHaveParameter(this ArgumentException exception, string parameterName)
        {
            Assert.Equal(parameterName, exception.ParamName);
            return exception;
        }
    }
}
