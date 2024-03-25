using Domain.Abstract;
using Domain.Entity;
using Infra.Persistence;

namespace Infra.Repository
{
    public class CardRepository(TasksDbContext context) : RepositoryBase<Card>(context), ICardRepository
    {
    }
}
