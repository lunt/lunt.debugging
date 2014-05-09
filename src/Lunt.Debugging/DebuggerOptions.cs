using System;
using Lunt.IO;

namespace Lunt.Debugging
{
    /// <summary>
    /// Options for the debugger.
    /// </summary>
    public sealed class DebuggerOptions
    {
        private readonly FilePath _buildConfigurationPath;

        /// <summary>
        /// Gets the build configuration path.
        /// </summary>
        /// <value>The build configuration path.</value>
        public FilePath BuildConfigurationPath
        {
            get { return _buildConfigurationPath; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DebuggerOptions"/> class.
        /// </summary>
        /// <param name="buildConfigurationPath">The build configuration path.</param>
        public DebuggerOptions(FilePath buildConfigurationPath)
        {
            if (buildConfigurationPath == null)
            {
                throw new ArgumentNullException("buildConfigurationPath");
            }
            _buildConfigurationPath = buildConfigurationPath;
        }

        /// <summary>
        /// Gets or sets the input path.
        /// </summary>
        /// <value>The input path.</value>
        public DirectoryPath InputPath { get; set; }

        /// <summary>
        /// Gets or sets the output path.
        /// </summary>
        /// <value>The output path.</value>
        public DirectoryPath OutputPath { get; set; }
    }
}