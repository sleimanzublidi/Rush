namespace Rush.Data
{
    public interface IRushRepository<TEntity, TId>
    {
        TEntity[] GetAll();
        TEntity Get(TId id);
        TEntity Insert(TEntity document);
        TEntity Update(TEntity document, TId id);
        void Delete(TId id);
    }
}
