using System;
using System.IO;
using SecurityPlusCore;

namespace SecurityPlus.Signature
{
    public class SignatureValidationCommand : IProcessValidationCommand
    {
        public SignatureValidationCommand()
        {
        }

        public bool Enabled { get; set; }

        public string Name { get; set; } = @"Signature validation";

        public ProcessOperationResultType Validate(ProcessOperationType operation, string processPath)
        {
            ProcessOperationResultType result;

            result = ProcessOperationResultType.Allow;

            if (null == processPath)
            {
                throw new ArgumentNullException(nameof(processPath));
            }

            if (!File.Exists(processPath))
            {
                throw new FileNotFoundException("Executable file could not be found", processPath);
            }

            /*var cert = X509Certificate2.CreateFromSignedFile(processPath);
            var certificate = new X509Certificate2(cert); //new X509Certificate2(processPath);*/
            if (!AuthenticodeTools.IsTrusted(processPath))
            {
                result = ProcessOperationResultType.Deny;
            }

            return result;
        }
    }
}
