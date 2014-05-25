using System;
using Lunt.Tests;
using Xunit;

namespace Lunt.Debugging.Tests
{
    public sealed class LuntDebuggerTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Assembly_Is_Null()
            {
                // Given, When, Then
                Assert.Throws<ArgumentNullException>(() => new LuntDebugger(null))
                    .ShouldHaveParameter("assembly");
            }
        }
    }
}
