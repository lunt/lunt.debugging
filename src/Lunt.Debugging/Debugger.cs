using System;
using Lunt.IO;

namespace Lunt.Debugging
{
    /// <summary>
    /// The Lunt pipeline debugger.
    /// </summary>
    public sealed class Debugger : IDisposable
    {
        private readonly DebuggerBootstrapper _bootstrapper;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Debugger"/> class.
        /// </summary>
        public Debugger()
            : this(null)
        {            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Debugger"/> class.
        /// </summary>
        /// <param name="bootstrapper">The bootstrapper.</param>
        public Debugger(DebuggerBootstrapper bootstrapper)
        {
            _bootstrapper = bootstrapper ?? new DebuggerBootstrapper();
            _bootstrapper.Initialize();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                _bootstrapper.Dispose();
                _disposed = true;
            }
        }

        /// <summary>
        /// Runs the specified path.
        /// </summary>
        /// <param name="buildConfigurationPath">The build configuration path.</param>
        /// <returns>The build manifest that is the result of the build.</returns>
        public BuildManifest Run(FilePath buildConfigurationPath)
        {
            if (buildConfigurationPath == null)
            {
                throw new ArgumentNullException("buildConfigurationPath");
            }
            return Run(new DebuggerOptions(buildConfigurationPath));
        }

        /// <summary>
        /// Runs the debugger with the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>The build manifest that is the result of the build.</returns>
        public BuildManifest Run(DebuggerOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            var environment = _bootstrapper.Resolve<IBuildEnvironment>();
            var workingDirectory = environment.GetWorkingDirectory();

            // Get the build configuration path.
            var buildConfigurationPath = options.BuildConfigurationPath;
            if (buildConfigurationPath.IsRelative)
            {
                buildConfigurationPath = workingDirectory.Combine(buildConfigurationPath);
            }

            // Read and fix the build configuration.
            var reader = _bootstrapper.Resolve<IBuildConfigurationReader>();
            var configuration = reader.Read(buildConfigurationPath);
            configuration.Incremental = false;
            configuration.InputDirectory = options.InputPath;
            configuration.OutputDirectory = options.OutputPath;

            // Set default directories.
            if (configuration.InputDirectory == null)
            {
                configuration.InputDirectory = buildConfigurationPath.GetDirectory();
            }
            if (configuration.OutputDirectory == null)
            {
                configuration.OutputDirectory = "Output";
            }

            // Make relative paths absolute.
            if (configuration.InputDirectory.IsRelative)
            {
                configuration.InputDirectory = workingDirectory.Combine(configuration.InputDirectory);
            }
            if (configuration.OutputDirectory.IsRelative)
            {
                configuration.OutputDirectory = workingDirectory.Combine(configuration.OutputDirectory);
            }

            // Build the configuration and return the result.
            using (var engine = _bootstrapper.GetEngine())
            {
                return engine.Build(configuration);
            }
        }
    }
}
