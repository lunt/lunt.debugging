using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace Lunt.Debugging.Tests
{
    public sealed class DebuggerBootstrapperTests
    {
        public sealed class TheGetEngineMethod
        {
            [Fact]
            public void Should_Resolve_Build_Engine()
            {
                // Given
                var bootstrapper = new DebuggerBootstrapper();
                bootstrapper.Initialize();

                // When
                var engine = bootstrapper.GetEngine();

                // Then
                Assert.IsType<BuildEngine>(engine);
            }   
        }
    }
}
