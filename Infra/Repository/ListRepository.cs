using Domain.Abstract;
using Domain.Entity;
using Infra.Persistence;

namespace Infra.Repository
{
    public class ListRepository(TasksDbContext context) : RepositoryBase<ListCard>(context), IListRepository
    {
    }
}
