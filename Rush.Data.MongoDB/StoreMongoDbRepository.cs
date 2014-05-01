namespace Rush.Data
{
    using System;
    using System.Linq;
    using System.Collections;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.Conventions;
    using MongoDB.Bson.Serialization.IdGenerators;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using Rush.Data.Serialization;

    public class StoreMongoDbRepository : IStoreRepository
    {
        private const string DocumentCollectionName = "Store";

        public string Resource { get; set; }

        static StoreMongoDbRepository()
        {
            BsonSerializer.RegisterSerializationProvider(new RushObjectSerializationProvider());
        }

        public StoreMongoDbRepository()
        { }

        public RushObject[] GetAll()
        {
            var collection = GetDatabase().GetCollection<RushObject>(Resource);
            return collection.FindAll().ToArray();
        }

        public RushObject Get(string id)
        {
            var collection = GetDatabase().GetCollection<RushObject>(Resource);
            var document = collection.FindOneById(BsonValue.Create(id));
            return document;
        }

        public RushObject Insert(RushObject document)
        {
            document.CreatedAt = document.UpdatedAt = DateTime.Now;

            var collection = GetDatabase().GetCollection<RushObject>(Resource);
            var result = collection.Insert(document);
            if (result.Ok) return document;

            return null;
        }

        public RushObject Update(string id, RushObject document)
        {
            document.ObjectId = id;
            document.UpdatedAt = DateTime.Now;            

            var collection = GetDatabase().GetCollection<RushObject>(Resource);
            var result = collection.Save(document);
            if (result.Ok) return document;

            return null;
        }

        public void Delete(string id)
        {
            var collection = GetDatabase().GetCollection<RushObject>(Resource);
            var query = Query<RushObject>.EQ(e => e.ObjectId, id);
            collection.Remove(query);
        }

        #region Helper Methods

        private const string MongoDbUrl = "mongodb://localhost:27017";
        private MongoDatabase database = null;

        private MongoDatabase GetDatabase()
        {
            if (database == null)
            {
                var client = new MongoClient(MongoDbUrl);
                var server = client.GetServer();
                database = server.GetDatabase("rushdb");
            }
            return database;
        }

        #endregion
    }
}
