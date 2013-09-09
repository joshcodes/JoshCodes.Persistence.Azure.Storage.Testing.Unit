using System;
using System.Collections.Generic;

using Microsoft.WindowsAzure.StorageClient;

namespace JoshCodes.Persistence.Azure.Storage.Testing.Unit
{
    class ExampleReference : AzureObjectWrapper<ExampleReference.Entity>
    {
        public ExampleReference(CloudTableClient tableClient, Example example)
            : base(tableClient, "ExampleReference", (out string partitionKey, out string rowKey) =>
                {
                    rowKey = Guid.NewGuid().ToString();
                    partitionKey = example.Int.ToString();
                    return new ExampleReference.Entity()
                    {
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                    };
                })
        {
            Storage.IdExample = SetReferencedObject(example);
        }

        public Example Example
        {
            get
            {
                var example = GetReferencedObject<Example.Entity, Example>(
                    Storage.IdExample,
                    (entity) => new Example(_tableClient, entity));
                return example;
            }
        }

        public class Entity : Storage.Entity
        {
            public string IdExample { get; set; }
        }
    }
}
