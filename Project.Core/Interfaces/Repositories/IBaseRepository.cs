using Project.Core.Entities;

namespace Project.Core.Interfaces.Repositories
{
    //Unit of Work Pattern
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(string query);
        Task<PaginatedDataViewModel<T>> GetPaginatedData(string query, int pageNumber, int pageSize);
        Task<T> GetById<Tid>(Tid id, string query);
        Task<bool> IsExists<Tvalue>(string key, Tvalue value);
        Task<bool> IsExistsForUpdate<Tid>(Tid id, string key, string value);
        Task<T> Create(T model);
        Task Update(T model);
        Task Delete(T model);
        Task<IEnumerable<T>> ExecuteQuery(string query);
        Task SaveChangeAsync();
    }
}
