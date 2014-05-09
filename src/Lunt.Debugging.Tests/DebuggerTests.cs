using System;
using Lunt.Debugging.Tests.Fakes;
using Lunt.IO;
using Lunt.Tests;
using NSubstitute;
using Xunit;

namespace Lunt.Debugging.Tests
{
    public sealed class DebuggerTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Build_Configuration_File_Path_Is_Null()
            {
                // Given
                var debugger = new Debugger();

                // When, Then
                Assert.Throws<ArgumentNullException>(() => debugger.Run((FilePath)null))
                    .ShouldHaveParameter("buildConfigurationPath");
            }

            [Fact]
            public void Should_Throw_If_Options_Is_Null()
            {
                // Given
                var debugger = new Debugger();
                var path = new FilePath("/root/file.xml");

                // When, Then
                Assert.Throws<ArgumentNullException>(() => debugger.Run((DebuggerOptions) null))
                    .ShouldHaveParameter("options");
            }
        }

        public sealed class TheRunMethod
        {
            public sealed class WithPath
            {
                [Fact]
                public void Should_Make_Relative_Build_Configuration_File_Absolute_To_Working_Direcory()
                {
                    // Given
                    var path = new FilePath("file.xml");
                    var bootstrapper = new FakeBootstrapper(path);
                    bootstrapper.BuildConfigurationReader = Substitute.For<IBuildConfigurationReader>();
                    bootstrapper.BuildConfigurationReader.Read(Arg.Any<FilePath>()).Returns(new BuildConfiguration());

                    // When
                    new Debugger(bootstrapper).Run(path);

                    // Then
                    bootstrapper.BuildConfigurationReader.Received(1).Read(
                        Arg.Is<FilePath>(f => f.FullPath == "/working/file.xml"));
                }

                [Fact]
                public void Should_Set_Input_Directory_To_Configuration_Directory_If_Not_Specified()
                {
                    // Given
                    var path = new FilePath("/root/file.xml");
                    var bootstrapper = new FakeBootstrapper(path);
                    bootstrapper.BuildEngine = Substitute.For<IBuildEngine>();

                    // When
                    new Debugger(bootstrapper).Run(path);

                    // Then
                    bootstrapper.BuildEngine.Received(1).Build(
                        Arg.Is<BuildConfiguration>(c => c.InputDirectory.FullPath == "/root"));
                }

                [Fact]
                public void Should_Set_Output_Directory_To_Working_Directory_If_Not_Specified()
                {
                    // Given
                    var path = new FilePath("/root/file.xml");
                    var bootstrapper = new FakeBootstrapper(path);
                    bootstrapper.BuildEngine = Substitute.For<IBuildEngine>();

                    // When
                    new Debugger(bootstrapper).Run(path);

                    // Then
                    bootstrapper.BuildEngine.Received(1).Build(
                        Arg.Is<BuildConfiguration>(c => c.OutputDirectory.FullPath == "/working/Output"));
                }                
            }

            public sealed class WithOptions
            {
                [Fact]
                public void Should_Set_Input_Directory_To_Specified_Directory()
                {
                    // Given
                    var path = new FilePath("/root/file.xml");
                    var bootstrapper = new FakeBootstrapper(path);
                    bootstrapper.BuildEngine = Substitute.For<IBuildEngine>();

                    var options = new DebuggerOptions(path);
                    options.InputPath = "/Input";

                    // When
                    new Debugger(bootstrapper).Run(options);

                    // Then
                    bootstrapper.BuildEngine.Received(1).Build(
                        Arg.Is<BuildConfiguration>(c => c.InputDirectory.FullPath == "/Input"));
                }

                [Fact]
                public void Should_Append_Working_Directory_To_Specified_Relative_Input_Directory()
                {
                    // Given
                    var path = new FilePath("/root/file.xml");
                    var bootstrapper = new FakeBootstrapper(path);
                    bootstrapper.BuildEngine = Substitute.For<IBuildEngine>();

                    var options = new DebuggerOptions(path);
                    options.InputPath = "Input";

                    // When
                    new Debugger(bootstrapper).Run(options);

                    // Then
                    bootstrapper.BuildEngine.Received(1).Build(
                        Arg.Is<BuildConfiguration>(c => c.InputDirectory.FullPath == "/working/Input"));
                }

                [Fact]
                public void Should_Set_Output_Directory_To_Specified_Directory()
                {
                    // Given
                    var path = new FilePath("/root/file.xml");
                    var bootstrapper = new FakeBootstrapper(path);
                    bootstrapper.BuildEngine = Substitute.For<IBuildEngine>();

                    var options = new DebuggerOptions(path);
                    options.OutputPath = "/Output";

                    // When
                    new Debugger(bootstrapper).Run(options);

                    // Then
                    bootstrapper.BuildEngine.Received(1).Build(
                        Arg.Is<BuildConfiguration>(c => c.OutputDirectory.FullPath == "/Output"));
                }

                [Fact]
                public void Should_Append_Working_Directory_To_Specified_Relative_Output_Directory()
                {
                    // Given
                    var path = new FilePath("/root/file.xml");
                    var bootstrapper = new FakeBootstrapper(path);
                    bootstrapper.BuildEngine = Substitute.For<IBuildEngine>();

                    var options = new DebuggerOptions(path);
                    options.OutputPath = "Output";

                    // When
                    new Debugger(bootstrapper).Run(options);

                    // Then
                    bootstrapper.BuildEngine.Received(1).Build(
                        Arg.Is<BuildConfiguration>(c => c.OutputDirectory.FullPath == "/working/Output"));
                }
            }
        }
    }
}
