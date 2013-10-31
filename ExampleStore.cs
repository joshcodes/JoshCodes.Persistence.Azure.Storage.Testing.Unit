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

        public Example Create()
        {
            return Create(Guid.NewGuid(), -1, 0.0, Guid.NewGuid().ToString(), null, Guid.Empty);
        }

        public Example Create(Guid key,
            int number, double scalar, string text, Uri uri, Guid guid)
        {
            var entity = new Example.Entity(key, DateTime.UtcNow, number, scalar, text, uri, guid);
            Create(entity);
            return new Example(_tableClient, entity);
        }
    }
}
