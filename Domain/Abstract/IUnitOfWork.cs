namespace Domain.Abstract
{
    public interface IUnitOfWork
    {
        IWorkspaceRepository IWorkspaceRepository { get; }
        IListRepository IListRepository { get; }
        ICardRepository ICardRepository { get; }
        IUserRepository IUserRepository { get; }
        public void CommitAsync();
    }
}
