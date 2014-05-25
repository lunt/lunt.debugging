using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lunt.Bootstrapping;
using Lunt.Diagnostics;
using Lunt.Runtime;

namespace Lunt.Debugging
{
    internal sealed class DebuggerConfiguration : IInternalConfiguration
    {
        private readonly Assembly _assembly;

        public DebuggerConfiguration(Assembly assembly)
        {
            _assembly = assembly;
        }

        public IEnumerable<ContainerRegistration> GetRegistrations()
        {
            var configuration = new InternalConfiguration();
            var registrations = configuration.GetRegistrations().ToList();

            // Register the build log.
            ReplaceRegistration(registrations, new InstanceRegistration(typeof(IBuildLog), new ConsoleBuildLog(new ConsoleWriter())));

            // Register the pipeline scanner.
            ReplaceRegistration(registrations, new FactoryRegistration(typeof(IPipelineScanner), context =>
            {
                var log = context.Resolve<IBuildLog>();
                return new AssemblyScanner(log, _assembly);
            }));

            return registrations;
        }

        private static void ReplaceRegistration(List<ContainerRegistration> registrations, ContainerRegistration registration)
        {
            registrations.RemoveAll(x => x.RegistrationType == registration.RegistrationType);
            registrations.Add(registration);
        }
    }

}
