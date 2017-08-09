using System;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SecurityPlusCore;

namespace SecurityPlusCoreTests
{
    [TestClass]
    public class ExternalCommandsManagerTests
    {
        [TestMethod]
        public void GetProcessValidationCommandTypesTest()
        {
            var path = @"d:\Work\SecurityPlusUI\SecurityPlus.Hash\bin\Debug\SecurityPlus.Hash.dll";
            var manager = new ExternalCommandsManager();

            var assembly = Assembly.LoadFile(path);
            var items = manager.GetProcessValidationCommandTypes(assembly).ToList();

            Assert.IsTrue(items.Any());
        }

        [TestMethod]
        public void GetExternalValidationCommandsTest()
        {
            var path = @"d:\Work\SecurityPlusUI\SecurityPlus.Hash\bin\Debug";
            var manager = new ExternalCommandsManager();

           var items = manager.GetExternalValidationCommands(path).ToList();

            Assert.IsTrue(items.Any());
        }
    }
}
