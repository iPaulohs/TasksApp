using System.Linq.Expressions;

namespace Domain.Abstract
{
    public interface IRepositoryBase<T> where T : class
    {
        T? Get(Expression<Func<T, bool>> expression);
        IEnumerable<T?> GetAll(Expression<Func<T, bool>> expression);
        Task<T> Create(T command);
        Task<T> Update(T command);
        Task Delete(Guid id);
    }
}
