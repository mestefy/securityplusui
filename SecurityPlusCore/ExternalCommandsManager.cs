using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SecurityPlusCore
{
    public class ExternalCommandsManager
    {        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerable<Type> GetProcessValidationCommandTypes(Assembly assembly)
        {
            var commandInterfaceType = typeof(IProcessValidationCommand);

            return assembly.GetTypes().Where(t => t.IsClass && t.IsPublic && commandInterfaceType.IsAssignableFrom(t));
        }

        public IEnumerable<IProcessValidationCommand> GetExternalValidationCommands(string assemblyDirectoryPath)
        {
            var commands = new List<IProcessValidationCommand>();

            if (null == assemblyDirectoryPath)
            {
                throw new ArgumentNullException(nameof(assemblyDirectoryPath));
            }

            if (string.IsNullOrWhiteSpace(assemblyDirectoryPath))
            {
                throw new ArgumentException(@"Assembly directory path cannot be empty", nameof(assemblyDirectoryPath));
            }

            if (!Directory.Exists(assemblyDirectoryPath))
            {
                throw new DirectoryNotFoundException(string.Format(@"Could not find directory {0}", assemblyDirectoryPath));
            }

            var directoryInfo = new DirectoryInfo(assemblyDirectoryPath);

            foreach (var file in directoryInfo.GetFiles("*.dll"))
            {
                var assembly = Assembly.LoadFile(file.FullName);
                var assemblyCommands = this.GetProcessValidationCommandTypes(assembly).ToList();
                if (null != assemblyCommands && assemblyCommands.Any())
                {
                    foreach(var type in assemblyCommands)
                    {
                        var command = Activator.CreateInstance(type) as IProcessValidationCommand;
                        if (null != command)
                        {
                            commands.Add(command);
                        }
                    }
                }
            }

            return commands;
        }
    }
}
