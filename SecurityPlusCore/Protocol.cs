using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SecurityPlusCore
{
    internal class Protocol
    {
        private int headerSize;

        private byte[] header;

        private SynchronizedCollection<IProcessValidationCommand> commands;
        
        internal event EventHandler<ProcessValidationRequest> PreProcessValidation;

        internal event EventHandler<Tuple<ProcessValidationRequest, ProcessValidationResponse>> PostProcessValidation;

        internal Protocol()
        {
            this.headerSize = sizeof(int);

            this.header = new byte[sizeof(int)];

            this.commands = new SynchronizedCollection<IProcessValidationCommand>();
        }

        internal async Task ProcessRequest(Stream stream)
        {
            var data = this.ReadData2(stream);
            var formattedData = data.ToProcessValidationRequest();

            this.PreProcessValidation?.Invoke(this, formattedData);
            // var responseData = this.ExecuteValidationCommands(formattedData);
            var responseData = new ProcessValidationResponse(ProcessOperationResultType.Allow);
            this.PostProcessValidation?.Invoke(this, new Tuple<ProcessValidationRequest, ProcessValidationResponse>(formattedData, responseData));

            var response = responseData.ToByteArray();
            await this.WriteData(stream, response);
        }

        internal void SendProtectedProcessId(Stream stream)
        {
            var processId = Process.GetCurrentProcess().Id;
            var responseData = new ProcessValidationResponse(ProcessOperationResultType.Deny, new IntPtr(processId));
            var response = responseData.ToByteArray();
            this.WriteData(stream, response).ConfigureAwait(true);
        }

        internal void RegisterProcessValidationCommand(IProcessValidationCommand processValidationCommand)
        {
            this.commands.Add(processValidationCommand);
        }

        internal void RegisterProcessValidationCommands(IEnumerable<IProcessValidationCommand> processValidationCommands)
        {
            foreach (var command in processValidationCommands)
            {
                this.commands.Add(command);
            }
        }

        internal void ClearRegisteredProcessValidationCommands()
        {
            this.commands.Clear();
        }

        private async Task<byte[]> ReadData(Stream stream)
        {
            byte[] buffer = null;

            if (0 != await stream.ReadAsync(this.header, 0, this.headerSize))
            {
                var structureSize = BitConverter.ToInt32(this.header, 0);

                buffer = new byte[structureSize];
                Buffer.BlockCopy(this.header, 0, buffer, 0, this.header.Length);

                var offset = this.headerSize;
                var bodySize = structureSize - offset;

                do
                {
                    offset += await stream.ReadAsync(buffer, offset, bodySize);
                    bodySize -= offset;
                }
                while (0 != bodySize);

                File.WriteAllBytes(Guid.NewGuid().ToString(), buffer);
            }

            return buffer;
        }

        private byte[] ReadData2(Stream stream)
        {
            byte[] buffer = null;

            if (0 != stream.Read(this.header, 0, this.headerSize))
            {
                var structureSize = BitConverter.ToInt32(this.header, 0);

                buffer = new byte[structureSize];
                Buffer.BlockCopy(this.header, 0, buffer, 0, this.header.Length);

                var offset = this.headerSize;
                var bodySize = structureSize - offset;

                do
                {
                    var bytesRead = stream.Read(buffer, offset, bodySize);
                    offset += bytesRead;
                    bodySize -= bytesRead;
                }
                while (0 != bodySize);

                // File.WriteAllBytes(Guid.NewGuid().ToString(), buffer);
            }

            return buffer;
        }

        private ProcessValidationResponse ExecuteValidationCommands(ProcessValidationRequest request)
        {
            var result = new ProcessValidationResponse(ProcessOperationResultType.Allow);

            for (var index = 0; index < this.commands.Count; index++)
            {
                var command = this.commands[index];
                if (command.Enabled)
                {
                    var success = true;

                    try
                    {
                        success = ProcessOperationResultType.Allow == command.Validate(request.Operation, request.GetProcessPath());
                    }
                    catch (Exception exception)
                    {
                        success = false;
                    }

                    if (!success)
                    {
                        result = new ProcessValidationResponse(ProcessOperationResultType.Deny);
                        break;
                    }
                }
            }

            return result;
        }

        private ProcessValidationResponse ExecuteValidationCommandsInParallel(ProcessValidationRequest request)
        {
            var result = new ProcessValidationResponse(ProcessOperationResultType.Allow);

            var success = this.commands.AsParallel()
                                       .Where(c => c.Enabled)
                                       .Select(cmd => cmd.Validate(request.Operation, request.GetProcessPath()))
                                       .All(r => ProcessOperationResultType.Allow == r);
            if (!success)
            {
                result = new ProcessValidationResponse(ProcessOperationResultType.Deny);
            }

            return result;
        }

        private async Task WriteData(Stream stream, byte[] buffer)
        {
            await stream.WriteAsync(buffer, 0, buffer.Length);
        }
    }
}
