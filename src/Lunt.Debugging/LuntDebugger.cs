using System;
using System.Reflection;
using Lunt.IO;

namespace Lunt.Debugging
{
    /// <summary>
    /// The Lunt pipeline debugger.
    /// </summary>
    public sealed class LuntDebugger
    {
        private readonly Assembly _assembly;

        /// <summary>
        /// Initializes a new instance of the <see cref="LuntDebugger"/> class.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public LuntDebugger(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            _assembly = assembly;
        }

        /// <summary>
        /// Runs the debugger using the specified build configuration buildConfigurationPath.
        /// </summary>
        /// <param name="buildConfigurationPath">The build configuration path.</param>
        /// <returns>The result of the build.</returns>
        public BuildManifest Run(FilePath buildConfigurationPath)
        {
            var settings = new BuildEngineSettings(buildConfigurationPath);
            return Run(settings);
        }

        /// <summary>
        /// Runs the debugger using the specified build engine settings.
        /// </summary>
        /// <param name="settings">The build engine settings.</param>
        /// <returns>The result of the build.</returns>
        public BuildManifest Run(BuildEngineSettings settings)
        {
            var engine = new BuildEngine(new LuntDebuggerConfiguration(_assembly));
            return engine.Build(settings);
        }
    }
}
