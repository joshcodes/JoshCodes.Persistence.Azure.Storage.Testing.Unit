﻿using System;
using System.Collections.Generic;

using Microsoft.WindowsAzure.StorageClient;

using JoshCodes.Persistence.Azure.Storage;

namespace JoshCodes.Persistence.Azure.Storage.Testing.Unit
{
    class ExampleReference : AzureObjectWrapper<ExampleReference.Entity>, IDefineExample
    {
        public ExampleReference(ExampleReference.Entity entity, CloudTableClient tableClient)
            : base(entity, tableClient, "ExampleReferenceTable")
        {
        }

        public Example Example
        {
            get
            {
                var example = new ExampleStore(_tableClient).GetReferencedObject(Storage.GetExampleIdRef());
                return example;
            }
        }

        public class Entity : Storage.Entity
        {
            public string ExampleIdRef { get; set; }

            public Entity()
            {
            }

            public Entity(AzureObjectReference exampleIdRef, Guid key, DateTime lastModified)
                : base(key, lastModified)
            {
                ExampleIdRef = Encode(exampleIdRef);
            }

            public AzureObjectReference GetExampleIdRef()
            {
                return Decode<AzureObjectReference>(this.ExampleIdRef);
            }
        }

        public class Store : AzureObjectStore<IDefineExample, ExampleReference, Entity>
        {
            public Store(CloudTableClient tableClient)
                : base(tableClient, "ExampleReferenceTable")
            {
            }

            public ExampleReference Create(Example example)
            {
                var idExample = example.GetAzureObjectReference<Example.Entity>();
                var entity = new ExampleReference.Entity(idExample, Guid.NewGuid(), DateTime.Now);

                Create(entity);

                return new ExampleReference(entity, _tableClient);
            }

            protected override ExampleReference CreateObjectStore(Entity entity)
            {
                return new ExampleReference(entity, _tableClient);
            }

            internal IEnumerable<ExampleReference> FindByExample(Example example)
            {
                var results = QueryOn<Example.Entity>(example, (entity) => entity.ExampleIdRef);
                return results;
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
