namespace Rush.Data
{
    using System;
    using System.IO;
    using Raven.Client.Document;

    public class StoreRavenDbRepository : IStoreRepository
    {
        private const string RavenDbUrl = "http://localhost:8080";
        private const string DocumentCollectionName = "Store";

        public string Resource { get; set; }

        public RushObject[] GetAll()
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

        public RushObject Get(string id)
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

        public RushObject Insert(RushObject obj)
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

        public RushObject Update(string id, RushObject obj)
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

        public void Delete(string id)
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
