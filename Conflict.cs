using System;

using Microsoft.WindowsAzure.StorageClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using JoshCodes.Persistence.Azure.Storage.Extensions;

namespace JoshCodes.Persistence.Azure.Storage.Testing.Unit
{
    [TestClass]
    public class Conflict
    {
        [TestMethod]
        public void Duplicate()
        {
            var tableClient = JoshCodes.Persistence.Azure.Storage.Settings.StorageAccount().CreateCloudTableClient();
            var entityStore = new ExampleStore(tableClient);
            var key = Guid.NewGuid().ToString();
            var entityWrapper1 = entityStore.Create(key, "Duplicate", -1, 0.0, "foo", null);

            bool isDuplicate = false;
            try
            {
                var entityWrapper2 = entityStore.Create(key, "Duplicate", -1, 0.0, "foo", null);
            } catch(Exception ex)
            {
                isDuplicate = ex.IsProblemResourceAlreadyExists();
            }

            Assert.AreEqual(true, isDuplicate);
        }
    }
}
