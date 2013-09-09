using System;

using Microsoft.WindowsAzure.StorageClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JoshCodes.Persistence.Azure.Storage
{
    [TestClass]
    public class ReferenceTests
    {
        [TestMethod]
        public void TestReference()
        {
            var tableClient = JoshCodes.Persistence.Azure.Storage.Settings.StorageAccount().CreateCloudTableClient();
            var entityWrapper = new Testing.Unit.Example(tableClient, Guid.NewGuid().ToString(), "ConcurrentModification", -1, 0.0, "foo", null);
            var entityWrapperWithRefernce = new Testing.Unit.ExampleReference(tableClient, entityWrapper);

            var referencedEntityWrapper = entityWrapperWithRefernce.Example;
            Assert.AreEqual(entityWrapper.Int, referencedEntityWrapper.Int);
        }
    }
}
