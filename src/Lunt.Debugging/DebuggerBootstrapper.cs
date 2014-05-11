using Lunt.Bootstrapping;
using System.Reflection;
using Lunt.Bootstrapping.TinyIoc;
using Lunt.Diagnostics;
using Lunt.Runtime;

namespace Lunt.Debugging
{
    /// <summary>
    /// The debugger bootstrapper.
    /// </summary>
    public class DebuggerBootstrapper : DefaultBootstrapper
    {
        private readonly Assembly _assembly;

        /// <summary>
        /// Initializes a new instance of the <see cref="DebuggerBootstrapper"/> class.
        /// </summary>
        public DebuggerBootstrapper()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DebuggerBootstrapper"/> class.
        /// </summary>
        /// <param name="assembly">The assembly to look for components in.</param>
        public DebuggerBootstrapper(Assembly assembly)
        {
            _assembly = assembly;
        }

        /// <summary>
        /// Registers the build log.
        /// </summary>
        /// <param name="container">The container.</param>
        protected override void RegisterBuildLog(TinyIoCContainer container)
        {
            container.Register(typeof (IBuildLog), typeof (ConsoleBuildLog)).AsSingleton();
        }

        /// <summary>
        /// Registers the pipeline scanner.
        /// </summary>
        /// <param name="container">The container.</param>
        protected override void RegisterPipelineScanner(TinyIoCContainer container)
        {
            if (_assembly == null)
            {
                // Register an app domain scanner.
                container.Register(typeof(IPipelineScanner), typeof(AppDomainScanner)).AsSingleton();
            }
            else
            {
                // Register an assembly scanner.
                container.Register(typeof(IPipelineScanner), (c, np) =>
                {
                    var log = c.Resolve<IBuildLog>();
                    return new AssemblyScanner(log, _assembly);
                });
            }
        }

        /// <summary>
        /// Configures the container.
        /// </summary>
        /// <param name="container">The container.</param>
        protected override void ConfigureContainer(TinyIoCContainer container)
        {
            container.Register(typeof(IBuildConfigurationReader), typeof(BuildConfigurationReader)).AsSingleton();
            container.Register(typeof(IConsoleWriter), typeof(ConsoleWriter)).AsSingleton();
        }

        internal T Resolve<T>()
            where T : class
        {
            return ApplicationContainer.Resolve<T>();
        }
    }
}
