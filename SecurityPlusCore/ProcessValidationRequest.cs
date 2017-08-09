using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SecurityPlusCore
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ProcessValidationRequest
    {
        public int RequestLength;
        public ProcessOperationType Operation;
        public IntPtr ProcessId;
        public StringData ProcessPath;

        public string GetProcessPath()
        {
            if (0 == this.ProcessPath.Length)
            {
                return Process.GetProcessById(this.ProcessId.ToInt32())?.ProcessName;
            }

            return this.ProcessPath.ToString();
        }
    }
}
