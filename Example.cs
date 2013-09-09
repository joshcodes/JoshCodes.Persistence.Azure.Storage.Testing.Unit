using System;

using Microsoft.WindowsAzure.StorageClient;

namespace JoshCodes.Persistence.Azure.Storage.Testing.Unit
{
    public class Example : AzureObjectWrapper<Example.Entity>
    {
        private const string EntityTableName = "ExampleTableTest";

        public Example(CloudTableClient tableClient, Example.Entity storage)
            : base(storage, tableClient, EntityTableName)
        {
        }

        public Example(CloudTableClient tableClient, string rowKey, string partitionKey)
            : base(tableClient, EntityTableName, new CreateEntity((out string pk, out string rk) =>
            {
                rk = rowKey;
                pk = partitionKey;
                return new Entity();
            }))
        {
        }

        public Example(CloudTableClient tableClient, string rowKey, string partitionKey,
            int number, double scalar, string text, Uri uri)
            : base(tableClient, EntityTableName, new CreateEntity((out string pk, out string rk) =>
            {
                rk = rowKey;
                pk = partitionKey;
                return new Entity()
                {
                    Int = number,
                    Double = scalar,
                    String = text,
                    Uri = uri,
                    UpdatedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                };
            }))
        {
        }

        private Example(Entity definition, CloudTableClient tableClient)
            : base(definition, tableClient, EntityTableName)
        {
        }

        public class Entity : Storage.Entity
        {
            public int Int { get; set; }
            public double Double { get; set; }
            public string String { get; set; }
            public Uri Uri { get; set; }
        }

        public bool ChangeIntAtomic(int requiredValue, int newValue, out int currentValue)
        {
            return this.AtomicModification<int>(requiredValue, newValue, out currentValue,
                (entity) => entity.Int);
        }

        public class Store : AzureObjectStore<Example, Example, Entity>
        {
            public Store(CloudTableClient tableClient)
                : base(tableClient, EntityTableName, (entity) => new Example(entity, tableClient))
            {
            }
        }

        public int Int
        {
            get
            {
                return Storage.Int;
            }
        }
    }
}
