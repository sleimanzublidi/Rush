namespace Rush.Data
{
    public interface IStoreRepository : IRushRepository<RushObject, string>
    {
        string Resource { get; set; }
    }
}
