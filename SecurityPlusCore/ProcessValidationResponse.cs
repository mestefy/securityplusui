using System;
using System.Runtime.InteropServices;

namespace SecurityPlusCore
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ProcessValidationResponse
    {
        // [FieldOffset(0)]
        public ProcessOperationResultType Result;
        // [FieldOffset(0)]
        public IntPtr ProtectedProcessId;

        public ProcessValidationResponse(ProcessOperationResultType result) : this(result, IntPtr.Zero)
        {
        }

        public ProcessValidationResponse(ProcessOperationResultType result, IntPtr protectedProcessId)
        {
            this.Result = result;
            this.ProtectedProcessId = protectedProcessId;
        }
    }
}
