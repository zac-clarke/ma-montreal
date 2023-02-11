namespace MaMontreal.Repositories
{
    public interface IRepo<T>
    {
        public Task<T> Get(int id);
        public Task<IEnumerable<T>> GetAll();
        public Task<T> Add(T entity);
        public Task<T> Update(T entity);
        public Task<T> Delete(int id);
        public Task<T> Delete(T entity);


        public Task<T> GetDeleted(string id);
        public Task<T> GetAllDeleted(string id);
    }
}