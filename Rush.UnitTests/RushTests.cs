using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rush
{
    [TestClass]
    public class RushTests
    {
        [TestMethod]
        public async Task TestSaveRushObjectAsync()
        {
            RushObject obj = new RushObject("GameScore");
            obj["PlayerName"] = "John Doe";
            obj["Score"] = 12000;
            obj["Array"] = new string[] { "A", "B", "C" };
            
            await obj.SaveAsync();
            
            Assert.IsNotNull(obj.ObjectId);
        }
    }
}
