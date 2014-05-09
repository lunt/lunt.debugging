using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunt.IO;
using Lunt.Tests;
using Xunit;

namespace Lunt.Debugging.Tests
{
    public sealed class DebuggerOptionsTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_The_Build_Configuration_Path_Is_Null()
            {
                // Given, When, Then
                Assert.Throws<ArgumentNullException>(() => new DebuggerOptions(null))
                    .ShouldHaveParameter("buildConfigurationPath");
            }
        }

        public sealed class TheBuildConfigurationPathProperty
        {
            [Fact]
            public void Should_Return_The_Provided_Build_Configuration_Path()
            {
                // Given
                var path = new FilePath("/file.xml");
                var options = new DebuggerOptions(path);

                // When, Then
                Assert.Equal(path, options.BuildConfigurationPath);
            }
        }
    }
}
