namespace Rush.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using Newtonsoft.Json;
    using Rush.Data;

    public class StoreController : RushController
    {
        private IStoreRepository repository;

        public StoreController(RushContext context, IStoreRepository repository)
            : base(context)
        {
            this.repository = repository;
            this.repository.Resource = context.Resource;
        }

        public override void Get()
        {
            string json = "[]";

            if (String.IsNullOrWhiteSpace(Context.Id))
            {
                var objs = repository.GetAll();
                if (objs != null) 
                    json = JsonConvert.SerializeObject(objs);
            }
            else
            {
                var obj = repository.Get(Context.Id);
                if (obj == null)
                    throw new RushException(HttpStatusCode.NoContent, "requested resource was not found");
                json = JsonConvert.SerializeObject(obj);
            }

            Context.HttpContext.Response.Write(json);
        }

        public override void Post()
        {
            string json = null;

            using (var reader = new StreamReader(Context.HttpContext.Request.InputStream))
            {
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                json = reader.ReadToEnd();
            }

            var obj = JsonConvert.DeserializeObject<RushObject>(json);
            obj["CreatedAt"] = DateTime.Now;            
            obj["UpdatedAt"] = DateTime.Now;

            var x = repository.Insert(obj);

            Context.HttpContext.Response.Write(json);
        }
    }
}
