using Domain.Abstract;
using Domain.Entity;
using Infra.Persistence;

namespace Infra.Repository
{
    public class UserRepository(TasksDbContext context) : RepositoryBase<User>(context), IUserRepository
    {
    }
}
