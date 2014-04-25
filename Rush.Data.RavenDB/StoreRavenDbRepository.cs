namespace Rush.Data
{
    using System;
    using System.IO;
    using Raven.Client.Document;

    public class StoreRavenDbRepository : StoreRepository
    {
        private const string RavenDbUrl = "http://localhost:8080";
        private const string DocumentCollectionName = "Store";

        public override RushObject[] GetAll()
        {
            using (var store = new DocumentStore { Url = RavenDbUrl }.Initialize())
            {
                using (var session = store.OpenSession())
                {
                    string key = Path.Combine(DocumentCollectionName, Resource);
                    var result = session.Load<dynamic>(key);
                    if (result != null)
                    {

                    }
                }
            }
            return null;
        }

        public override RushObject Get(string id)
        {
            using (var store = new DocumentStore { Url = RavenDbUrl }.Initialize())
            {
                using (var session = store.OpenSession())
                {
                    string key = Path.Combine(DocumentCollectionName, Resource, id);
                    var result = session.Load<RushObject>(key);
                    if (result != null)
                    {

                    }
                }
            }
            return null;
        }

        public override RushObject Insert(RushObject obj)
        {
            using (var store = new DocumentStore { Url = RavenDbUrl }.Initialize())
            {
                using (var session = store.OpenSession("Rush"))
                {
                    string key = Path.Combine(DocumentCollectionName, Resource);
                    session.Store(obj, key);
                    session.SaveChanges();
                }                
            }

            return obj;
        }

        public override RushObject Update(RushObject obj, string id)
        {
            using (var store = new DocumentStore { Url = RavenDbUrl }.Initialize())
            {
                using (var session = store.OpenSession())
                {
                    session.Store(obj, id);
                    session.SaveChanges();
                }
            }

            return obj;
        }

        public override void Delete(string id)
        {
            using (var store = new DocumentStore { Url = RavenDbUrl }.Initialize())
            {
                using (var session = store.OpenSession())
                {
                    session.Advanced.DocumentStore.DatabaseCommands.Delete(DocumentCollectionName + id, null);
                }
            }
        }
    }
}
