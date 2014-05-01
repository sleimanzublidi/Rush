namespace Rush.Data
{
    public interface IRushRepository<TEntity, TId>
    {
        TEntity[] GetAll();
        TEntity Get(TId id);
        TEntity Insert(TEntity document);
        TEntity Update(TId id, TEntity document);
        void Delete(TId id);
    }
}
