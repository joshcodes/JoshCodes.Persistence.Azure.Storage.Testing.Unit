using System;

using Microsoft.WindowsAzure.StorageClient;

namespace JoshCodes.Persistence.Azure.Storage.Testing.Unit
{
    public class Example : AzureObjectWrapper<Example.Entity>, IDefineExample
    {
        internal const string EntityTableName = "ExampleTableTest";

        public Example(CloudTableClient tableClient, Example.Entity storage)
            : base(storage, tableClient, EntityTableName)
        {
        }

        private Example(Entity definition, CloudTableClient tableClient)
            : base(definition, tableClient, EntityTableName)
        {
        }

        public class Entity : Storage.Entity
        {
            public Entity()
            {
            }

            public Entity(Guid key, DateTime lastModified, int i, double d, string s, Uri uri, Guid g)
                : base(key, lastModified)
            {
                this.Int = i;
                this.Double = d;
                this.String = s;
                this.Uri = uri;
                this.guid = g;
            }

            public int Int { get; set; }
            public double Double { get; set; }
            public string String { get; set; }
            public Uri Uri { get; set; }
            public Guid guid { get; set; }
        }

        public bool ChangeIntAtomic(int requiredValue, int newValue, out int currentValue)
        {
            return this.AtomicModification<int>(requiredValue, newValue, out currentValue,
                (entity) => entity.Int);
        }

        public int Int
        {
            get
            {
                return Storage.Int;
            }
        }


        public DateTime UpdatedAt
        {
            get { throw new NotImplementedException(); }
        }

        public DateTime CreatedAt
        {
            get { throw new NotImplementedException(); }
        }
    }
}
