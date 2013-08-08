using System;
using System.Threading;

using Microsoft.WindowsAzure.StorageClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JoshCodes.Persistence.Azure.Sql.Testing.Unit
{
    [TestClass]
    public class Concurrency
    {
        [TestMethod]
        public void BruteForce()
        {
            var tableClient = JoshCodes.Persistence.Azure.Sql.Settings.StorageAccount().CreateCloudTableClient();
            var entityWrapper1 = new Example(tableClient, Guid.NewGuid().ToString(), "ConcurrentModification");
            var entityWrapper2 = new Example.Store(tableClient).FindByUrn(entityWrapper1.IdUrn);

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
            var tableClient = JoshCodes.Persistence.Azure.Sql.Settings.StorageAccount().CreateCloudTableClient();
            var entityWrapper = new Example(tableClient, Guid.NewGuid().ToString(), "ConcurrentModification", -1, 0.0, "foo", null);

            int currentValue;
            entityWrapper.ChangeIntAtomic(-1, 0, out currentValue);
            Assert.AreEqual(-1, currentValue);
            Assert.AreEqual(0, entityWrapper.Int);

        }
    }
}
