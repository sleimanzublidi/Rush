namespace Rush
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public static class RushClient
    {
        private const string storeBaseUrl = "http://localhost:38110/rush/store/";

        public static void Initialize(string applicationId, string applicationKey)
        {

        }

        public static Task SaveAsync(this RushObject obj)
        {
            return SaveAsync(obj, CancellationToken.None);
        }

        public static Task SaveAsync(this RushObject obj, CancellationToken cancellationToken)
        {
            var json = JsonConvert.SerializeObject(obj.AsDictionary());
            var content = new StringContent(json);

            var client = new HttpClient();
            return client.PostAsync(storeBaseUrl + obj.ClassName, content, cancellationToken);
        } 
    }
}
