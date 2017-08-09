using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SecurityPlus.Hash;
using SecurityPlusCore;

namespace SecurityPlus.HashTests
{
    [TestClass]
    public class HashValidationCommandTests
    {
        private HashValidationCommand command = new HashValidationCommand();

        private string processPath = @"C:\Windows\System32\notepad.exe";

        [TestMethod]
        public void ValidateTest()
        {
            var result = this.command.Validate(ProcessOperationType.Create, this.processPath);

            Assert.AreEqual(ProcessOperationResultType.Allow, result);
        }
        
        [TestMethod]
        public void GenerateSHA256HashTest()
        {            
            var result1 = this.command.GenerateSHA256Hash(this.processPath);
            var result2 = this.command.GenerateSHA256Hash(this.processPath);
            
            Assert.IsTrue(result1.SequenceEqual(result2));
        }
    }
}
