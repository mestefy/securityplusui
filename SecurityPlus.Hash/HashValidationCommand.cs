using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SecurityPlusCore;

namespace SecurityPlus.Hash
{
    public class HashValidationCommand : IProcessValidationCommand
    {
        private string settingsPath; // = @"d:\Work\SecurityPlusUI\SecurityPlusUI\bin\x64\Release\HashSettings.xml";

        private PairSettings<string, byte[]> settings;

        private List<Pair<string, byte[]>> records;

        public HashValidationCommand()
        {
            this.settingsPath = Path.Combine(Environment.CurrentDirectory, "HashSettings.xml");
            this.settings = new PairSettings<string, byte[]>(this.settingsPath);

            if (!File.Exists(this.settingsPath))
            {
                this.settings.SaveSettings(new List<Pair<string, byte[]>>());
            }

            this.records = this.settings.LoadSettings();
        }

        public bool Enabled { get; set; } = false;

        public string Name { get; set; } = @"Hash validation";

        public ProcessOperationResultType Validate(ProcessOperationType operation, string processPath)
        {
            ProcessOperationResultType result;

            result = ProcessOperationResultType.Deny;

            if (null == processPath)
            {
                throw new ArgumentNullException(nameof(processPath));
            }

            if (string.IsNullOrWhiteSpace(processPath))
            {
                throw new ArgumentException(@"Process name cannot be empty", nameof(processPath));
            }

            if (!File.Exists(processPath))
            {
                throw new FileNotFoundException(@"Executable file could not be found", processPath);
            }

            /*var records = new List<Pair<string, byte[]>>();
            records.Add(new Pair<string, byte[]> { Key = processPath, Value = this.GenerateSHA256Hash(processPath) });
            this.settings.SaveSettings(records);*/

            var item = this.records.FirstOrDefault(p => p.Key.Equals(processPath, StringComparison.InvariantCultureIgnoreCase));
            if (null != item.Key)
            {
                var hash = this.GenerateSHA256Hash(processPath);
                if (hash.SequenceEqual(item.Value))
                {
                    result = ProcessOperationResultType.Allow;
                }
            }

            return result;
        }

        public byte[] GenerateSHA256Hash(string filePath)
        {
            byte[] result = null;

            var sha256 = new SHA256Managed();
            using (var stream = File.OpenRead(filePath))
            {
                result = sha256.ComputeHash(stream);
            }

            return result;
        }
    }
}
