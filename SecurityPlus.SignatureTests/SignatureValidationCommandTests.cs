using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecurityPlus.Signature;
using SecurityPlusCore;

namespace SecurityPlus.SignatureTests
{
    [TestClass]
    public class SignatureValidationCommandTests
    {
        private SignatureValidationCommand command = new SignatureValidationCommand();

        private string processPath = @"C:\Program Files\Mozilla Firefox\firefox.exe";// @"c:\Program Files (x86)\Windows Kits\10\bin\x64\signtool.exe"; //@"C:\Windows\System32\ntoskrnl.exe";// 

        [TestMethod]
        public void ValidateSignatureTest()
        {
            for (int index = 0; index < 100; index++)
            {
                var result = this.command.Validate(ProcessOperationType.Create, this.processPath);

                Assert.AreEqual(ProcessOperationResultType.Allow, result);
            }
        }
    }
}
