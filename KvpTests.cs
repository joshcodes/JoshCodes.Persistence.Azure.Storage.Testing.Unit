using System;

using Microsoft.WindowsAzure.StorageClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using JoshCodes.Persistence.Azure.Storage;

namespace JoshCodes.Persistence.Azure.Storage.Testing.Unit
{
    [TestClass]
    public class KvpTests
    {
        [TestMethod]
        public void TestCreateAndGet()
        {
            // Setup storage
            var tableClient = JoshCodes.Persistence.Azure.Storage.Settings.StorageAccount().CreateCloudTableClient();
            var store = new KvpStore(tableClient);

            // Setup test values
            var container =  "KvpTests.TestGetAndSet";
            var key = Guid.NewGuid().ToString("N");
            var val = Guid.NewGuid().ToString();
            
            // Test empty get
            var getVal = store.Get(container, key);
            Assert.IsNull(getVal);

            // Test Create
            store.Create(container, key, val);

            // Test Get
            getVal = store.Get(container, key);
            Assert.AreEqual(val, getVal);
        }
    }
}
