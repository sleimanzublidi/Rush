namespace Rush.Data
{
    public interface IStoreRepository : IRushRepository<RushObject, string>
    {
        string Resource { get; set; }
    }

    public abstract class StoreRepository : IStoreRepository
    {
        public string Resource { get; set; }

        public abstract RushObject[] GetAll();
        public abstract RushObject Get(string id);
        public abstract RushObject Insert(RushObject document);
        public abstract RushObject Update(RushObject document, string id);
        public abstract void Delete(string id);
    }
}
