using Lunt.Bootstrapping.TinyIoc;
using Lunt.IO;
using NSubstitute;

namespace Lunt.Debugging.Tests.Fakes
{
    public sealed class FakeBootstrapper : DebuggerBootstrapper
    {
        public IFileSystem FileSystem { get; set; }
        public IBuildEnvironment Environment { get; set; }
        public IBuildConfigurationReader BuildConfigurationReader { get; set; }
        public IBuildEngine BuildEngine { get; set; }

        public FakeBootstrapper(FilePath path)
        {
            FileSystem = Substitute.For<IFileSystem>();
            var file = Substitute.For<IFile>();
            file.Exists.Returns(true);
            FileSystem.GetFile(path).Returns(file);

            Environment = Substitute.For<IBuildEnvironment>();
            Environment.GetWorkingDirectory().Returns("/working");

            BuildConfigurationReader = Substitute.For<IBuildConfigurationReader>();
            BuildConfigurationReader.Read(path).Returns(new BuildConfiguration());

            BuildEngine = Substitute.For<IBuildEngine>();
            BuildEngine.Build(Arg.Any<BuildConfiguration>()).Returns(new BuildManifest());
            BuildEngine.Build(Arg.Any<BuildConfiguration>(), Arg.Any<BuildManifest>()).Returns(new BuildManifest());
        }        

        protected override void RegisterFileSystem(TinyIoCContainer container)
        {
            container.Register(typeof(IFileSystem), FileSystem);
        }

        protected override void RegisterBuildEnvironment(TinyIoCContainer container)
        {
            container.Register(typeof(IBuildEnvironment), Environment);
        }

        protected override void ConfigureContainer(TinyIoCContainer container)
        {
            container.Register(typeof(IBuildConfigurationReader), BuildConfigurationReader);
        }

        protected override IBuildEngine ResolveBuildEngine(TinyIoCContainer container)
        {
            return BuildEngine;
        }
    }
}
