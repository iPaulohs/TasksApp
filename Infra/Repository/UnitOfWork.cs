using Domain.Abstract;
using Infra.Persistence;

namespace Infra.Repository
{
    public class UnitOfWork(TasksDbContext context, IWorkspaceRepository workspaceRepository, IListRepository listRepository, ICardRepository cardRepository, IUserRepository userRepository) : IUnitOfWork
    {
        private readonly TasksDbContext _context = context;

        public IWorkspaceRepository IWorkspaceRepository => workspaceRepository ?? new WorkspaceRepository(_context);

        public IListRepository IListRepository => listRepository ?? new ListRepository(_context);

        public ICardRepository ICardRepository => cardRepository ?? new CardRepository(_context);

        public IUserRepository IUserRepository => userRepository ?? new UserRepository(_context);

        public async void CommitAsync()
        {
            await _context.SaveChangesAsync();
            await _context.DisposeAsync();
        }
    }
}
