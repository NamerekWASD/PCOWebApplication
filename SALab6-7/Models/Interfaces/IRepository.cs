namespace PCO.Models.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);
        List<T> GetAll();
        void Create(T Object);
        void Update(T Object);
        void Delete(int id);
    }
}
