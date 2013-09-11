using System;
using System.Threading;

using Microsoft.WindowsAzure.StorageClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JoshCodes.Persistence.Azure.Storage.Testing.Unit
{
    [TestClass]
    public class Concurrency
    {
        [TestMethod]
        [TestCategory("JoshCodes/Persistence/Azure/Storage")]
        public void BruteForce()
        {
            var tableClient = JoshCodes.Persistence.Azure.Storage.Settings.StorageAccount().CreateCloudTableClient();
            var entityStore = new ExampleStore(tableClient);
            var entityWrapper1 = entityStore.Create(Guid.NewGuid().ToString(), "ConcurrentModification", 0, 0.0, "foo", null);
            var entityWrapper2 = (Example)entityStore.FindByUrn(entityWrapper1.IdUrn);

            int totalMods = 0;

            Thread t1 = new Thread(new ThreadStart(delegate
                {
                    int i = 0;
                    while (i < 100)
                    {
                        int cv;
                        if(entityWrapper1.ChangeIntAtomic(i, i+1, out cv))
                        {
                            i++;
                            lock(this)
                            {
                                totalMods++;
                            }
                        } else
                        {
                            i = cv;
                        }
                    }
                }));
            Thread t2 = new Thread(new ThreadStart(delegate
                {
                    int i = 0;
                    while (i < 100)
                    {
                        int cv;
                        if(entityWrapper2.ChangeIntAtomic(i, i+1, out cv))
                        {
                            i++;
                            lock(this)
                            {
                                totalMods++;
                            }
                        } else
                        {
                            i = cv;
                        }
                    }
                }));
            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();

            Assert.AreEqual(totalMods, 100);
        }

        [TestMethod]
        public void DoesUpdate()
        {
            var tableClient = JoshCodes.Persistence.Azure.Storage.Settings.StorageAccount().CreateCloudTableClient();
            var entityStore = new ExampleStore(tableClient);
            var key = Guid.NewGuid().ToString();
            var entityWrapper = entityStore.Create(key, "Duplicate", -1, 0.0, "foo", null);

            int currentValue;
            entityWrapper.ChangeIntAtomic(-1, 0, out currentValue);
            Assert.AreEqual(0, currentValue);
            Assert.AreEqual(0, entityWrapper.Int);

        }
    }
}
