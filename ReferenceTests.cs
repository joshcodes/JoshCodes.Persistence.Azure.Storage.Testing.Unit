using System;
using System.Linq;

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
            var exampleStore = new Testing.Unit.ExampleStore(tableClient);
            var example = exampleStore.Create(Guid.NewGuid().ToString(), "ConcurrentModification", -1, 0.0, "foo", null);
            var exampleReferenceStore = new Testing.Unit.ExampleReference.Store(tableClient);
            var referencedExample = exampleReferenceStore.Create(example);

            var referencedEntityWrapper = referencedExample.Example;
            Assert.AreEqual(example.Int, referencedEntityWrapper.Int);
        }

        [TestMethod]
        public void QueryOn()
        {
            var tableClient = JoshCodes.Persistence.Azure.Storage.Settings.StorageAccount().CreateCloudTableClient();
            var exampleStore = new Testing.Unit.ExampleStore(tableClient);
            var example = exampleStore.Create(Guid.NewGuid().ToString(), "ConcurrentModification", -1, 0.0, "foo", null);
            var exampleReferenceStore = new Testing.Unit.ExampleReference.Store(tableClient);
            var referencedExample = exampleReferenceStore.Create(example);

            var referencingExamples = exampleReferenceStore.FindByExample(example);
            Assert.AreEqual(1, referencingExamples.Count());
            var referencedExampleFromQuery = referencingExamples.First();
            Assert.AreEqual(referencedExample.IdGuid, referencedExampleFromQuery.IdGuid);
        }
    }
}
