using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SecurityPlusCore
{
    public static class BufferHelper
    {
        public static T ToStructure<T>(this byte[] buffer) where T : struct
        {
            T result = default(T);

            if (null == buffer)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            var ptr = handle.AddrOfPinnedObject();

            result = Marshal.PtrToStructure<T>(ptr);

            handle.Free();

            return result;
        }

        private static int Read1 = sizeof(int);
        private static int Read2 = Read1 + sizeof(byte);
        private static int Read3 = Read2 + IntPtr.Size;
        private static int Read4 = Read3 + sizeof(short);

        public static ProcessValidationRequest ToProcessValidationRequest(this byte[] buffer)
        {
            var result = default(ProcessValidationRequest);

            if (null == buffer)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            result.RequestLength = buffer.Length;
            result.Operation = (ProcessOperationType)BitConverter.ToChar(buffer, BufferHelper.Read1);
            if (IntPtr.Size == sizeof(int))
            {
                result.ProcessId = new IntPtr(BitConverter.ToInt32(buffer, Read2));
            }
            else
            {
                result.ProcessId = new IntPtr(BitConverter.ToInt64(buffer, Read2));
            }

            result.ProcessPath.Length = BitConverter.ToInt16(buffer, BufferHelper.Read3);
            result.ProcessPath.Buffer = Encoding.Unicode.GetString(buffer, BufferHelper.Read4, buffer.Length - BufferHelper.Read4);

            return result;
        }

        public static byte[] ToByteArray<T>(this T item) where T : struct
        {
            var size = Marshal.SizeOf(item);
            var result = new byte[size];

            var pointer = Marshal.AllocHGlobal(size);

            Marshal.StructureToPtr(item, pointer, true);

            Marshal.Copy(pointer, result, 0, result.Length);

            Marshal.FreeHGlobal(pointer);

            return result;
        }
    }
}
