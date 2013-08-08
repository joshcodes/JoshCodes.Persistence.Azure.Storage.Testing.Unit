using System;

using Microsoft.WindowsAzure.StorageClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using JoshCodes.Persistence.Azure.Sql.Extensions;

namespace JoshCodes.Persistence.Azure.Sql.Testing.Unit
{
    [TestClass]
    public class Conflict
    {
        [TestMethod]
        public void Duplicate()
        {
            var tableClient = JoshCodes.Persistence.Azure.Sql.Settings.StorageAccount().CreateCloudTableClient();
            var key = Guid.NewGuid().ToString();
            var entityWrapper1 = new Example(tableClient, key, "Duplicate", -1, 0.0, "foo", null);

            bool isDuplicate = false;
            try
            {
                var entityWrapper2 = new Example(tableClient, key, "Duplicate", -1, 0.0, "foo", null);
            } catch(Exception ex)
            {
                isDuplicate = ex.IsProblemResourceAlreadyExists();
            }

            Assert.AreEqual(true, isDuplicate);
        }
    }
}
