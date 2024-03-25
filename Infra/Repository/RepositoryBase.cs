using Domain.Abstract;
using Infra.Persistence;
using System.Linq.Expressions;

namespace Infra.Repository
{
    public class RepositoryBase<T>(TasksDbContext context) : IRepositoryBase<T> where T : class
    {
        protected readonly TasksDbContext _context = context;

        public T? Get(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().FirstOrDefault(expression);
        }

        public IEnumerable<T?> GetAll(Expression<Func<T, bool>> expression)
        {
            return [.. _context.Set<T>().Where(expression)];
        }

        public async Task<T> Create(T command)
        {
            _context.Set<T>().Add(command);
            return command;
        }

        public Task Delete(Guid id)
        {
            _context.Remove(id);
            return Task.CompletedTask;
        }

        public async Task<T> Update(T command)
        {
            _context.Set<T>().Update(command);
            return command;
        }
    }
}
