using System;

using Microsoft.WindowsAzure.StorageClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JoshCodes.Persistence.Azure.Storage.Testing.Unit
{
    [TestClass]
    public class MutationsTests
    {
        [TestMethod]
        public void UpdateExampleInt()
        {
            var tableClient = JoshCodes.Persistence.Azure.Storage.Settings.StorageAccount().CreateCloudTableClient();
            var exampleStore = new Testing.Unit.ExampleStore(tableClient);
            var example = exampleStore.Create(Guid.NewGuid(), -1, 0.0, "ConcurrentModification", null, Guid.Empty);
            example.Update(23);

            Assert.AreEqual(23, example.Int);
        }
    }
}
