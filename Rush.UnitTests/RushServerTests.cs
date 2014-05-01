namespace Rush
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rush.Controllers;
    using Rush.Data;

    [TestClass]
    public class RushServerTests
    {
        [TestMethod]
        public void TestCreateRushObjectUsingIndexer()
        {
            var obj = new RushObject("User");
            obj["Name"] = "John Doe";
            Assert.IsNotNull(obj["Name"]);
        }

        [TestMethod]
        public void TestCreateRushObjectAsDynamic()
        {
            dynamic obj = new RushObject("User");
            obj.Name = "John Doe";
            Assert.IsNotNull(obj.Name);
        }

        [TestMethod]
        public void TestGetFromStoreMongoDbRepository()
        {
            var obj = new RushObject("Player");
            obj["Name"] = "John Doe";

            var store = new StoreMongoDbRepository();
            store.Resource = obj.ClassName;
            store.Insert(obj);

            var fromDb = store.Get(obj.ObjectId);
            Assert.IsNotNull(fromDb);
        }

        [TestMethod]
        public void TestInsertIntoStoreMongoDbRepository()
        {
            var obj = new RushObject("Player");
            obj["Name"] = "John Doe";
            obj["Score"] = 1000;
            obj["Array"] = new string[] { "A", "B", "C" };

            var store = new StoreMongoDbRepository();
            store.Resource = obj.ClassName;
            store.Insert(obj);
            
            Assert.IsNotNull(obj.ObjectId);
            Assert.IsNotNull(obj.CreatedAt);
        }

        [TestMethod]
        public void TestUpdateStoreMongoDbRepository()
        {
            var obj = new RushObject("Player");
            obj["Name"] = "John Doe";

            var store = new StoreMongoDbRepository();
            store.Resource = obj.ClassName;
            store.Insert(obj);

            obj["UpdateWorks"] = true;
            store.Update(obj.ObjectId, obj);
                        
            Assert.IsNotNull(obj["UpdateWorks"]);
        }

        [TestMethod]
        public void TestDeleteStoreMongoDbRepository()
        {
            var obj = new RushObject("Player");
            obj["Name"] = "John Doe";

            var store = new StoreMongoDbRepository();
            store.Resource = obj.ClassName;
            store.Insert(obj);

            store.Delete(obj.ObjectId);

            var result = store.Get(obj.ObjectId);

            Assert.IsNull(result);
        }
    }
}
