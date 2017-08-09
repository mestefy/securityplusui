using System;
using SecurityPlusCore;

namespace SecurityPlusUI.Model
{
    public struct ProcessEvent
    {
        private ProcessValidationRequest request;

        private ProcessValidationResponse response;

        internal ProcessEvent(ProcessValidationRequest request, ProcessValidationResponse response)
        {
            this.request = request;
            this.response = response;
        }

        public ProcessOperationType OperationType { get { return this.request.Operation; } }

        public IntPtr ProcessId { get { return this.request.ProcessId; } }

        public string ProcessPath { get { return this.request.GetProcessPath(); } }

        public ProcessOperationResultType Result { get { return this.response.Result; } }
    }
}
