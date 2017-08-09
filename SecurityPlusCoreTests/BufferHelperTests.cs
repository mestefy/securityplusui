using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecurityPlusCore;

namespace SecurityPlusCoreTests
{
    [TestClass]
    public class BufferHelperTests
    {
        [TestMethod]
        public void ToStructureTest()
        {
            ProcessValidationResponse response = new ProcessValidationResponse(ProcessOperationResultType.Allow);

            var myData = response.ToByteArray();


            var data = File.ReadAllBytes(@"87f36a50-0166-4da4-a0d5-d73975d37cb8");

            var length = BitConverter.ToUInt32(data, 0);

            var copy = new byte[length];
            Array.Copy(data, copy, length);

            // var result = new UnicodeEncoding(false, false).GetString(copy);

            var index = 0;
            while (index++ < 10000)
            {
                // var formattedData = copy.ToStructure<ProcessValidationRequest>();
                var formattedData = copy.ToProcessValidationRequest();

                var size = Marshal.SizeOf(formattedData);
                var s1 = Marshal.SizeOf(formattedData.ProcessPath);
            }

            // var formattedData = copy.ToProcessValidationRequest();

            /*var str = new UnicodeString();
            var buffer = Encoding.Unicode.GetBytes("ananas");

            str.Length = (short)buffer.Length;
            // str.MaxLength = str.Length;

            var data = str.ToByteArray();

            var all = new byte[data.Length + buffer.Length];
            Buffer.BlockCopy(data, 0, all, 0, data.Length);
            Buffer.BlockCopy(buffer, 0, all, data.Length, buffer.Length);

            var formattedData = all.ToStructure<StringData>();*/

            Assert.IsTrue(true);
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct UnicodeString
        {
            public short Length;
            // public short MaxLength;
        }
    }
}
