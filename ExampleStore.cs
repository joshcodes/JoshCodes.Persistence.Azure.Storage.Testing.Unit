using System;

using Microsoft.WindowsAzure.StorageClient;

namespace JoshCodes.Persistence.Azure.Storage.Testing.Unit
{
    class ExampleStore : AzureObjectStore<IDefineExample, Example, Example.Entity>
    {
        public ExampleStore(CloudTableClient tableClient) 
            : base(tableClient, Example.EntityTableName)
        {
        }

        protected override Example CreateObjectStore(Example.Entity entity)
        {
            return new Example(_tableClient, entity);
        }

        public Example Create(string rowKey, string partitionKey)
        {
            var entity = new Example.Entity();
            Create(entity, rowKey, partitionKey);
            return new Example(_tableClient, entity);
        }

        public Example Create(string rowKey, string partitionKey,
            int number, double scalar, string text, Uri uri)
        {
            var entity = new Example.Entity()
            {
                    Int = number,
                    Double = scalar,
                    String = text,
                    Uri = uri,
                    UpdatedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
            };
            Create(entity, rowKey, partitionKey);
            return new Example(_tableClient, entity);
        }
    }
}
