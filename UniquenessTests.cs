using System;

using Microsoft.WindowsAzure.StorageClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using JoshCodes.Persistence.Azure.Storage;

namespace JoshCodes.Persistence.Azure.Storage.Testing.Unit
{
    [TestClass]
    public class UniquenessTests
    {
        [TestMethod]
        public void UniqueValuesDoNotConflict()
        {
            var tableClient = JoshCodes.Persistence.Azure.Storage.Settings.StorageAccount().CreateCloudTableClient();
            tableClient.TryRegisterUnique(Guid.NewGuid().ToString(), "TestEntityUniqueness");
            bool unique = tableClient.TryRegisterUnique(Guid.NewGuid().ToString(), "TestEntityUniqueness");
            Assert.IsTrue(unique);
        }

        [TestMethod]
        public void UniquenessIsUpheld()
        {
            var tableClient = JoshCodes.Persistence.Azure.Storage.Settings.StorageAccount().CreateCloudTableClient();
            var uniqueValue = Guid.NewGuid().ToString();
            tableClient.TryRegisterUnique(uniqueValue, "TestEntityUniqueness");
            bool unique = tableClient.TryRegisterUnique(uniqueValue, "TestEntityUniqueness");
            Assert.IsFalse(unique);
        }

        [TestMethod]
        public void UniquenessInDifferentNamespaces()
        {
            var tableClient = JoshCodes.Persistence.Azure.Storage.Settings.StorageAccount().CreateCloudTableClient();
            var uniqueValue =  Guid.NewGuid().ToString();
            tableClient.TryRegisterUnique(uniqueValue, "TestEntityUniquenessNS1");
            tableClient.TryRegisterUnique(uniqueValue, "TestEntityUniquenessNS2");
        }
    }
}
