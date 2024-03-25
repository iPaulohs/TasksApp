using Domain.Entity;
using System.Linq.Expressions;

namespace Domain.Abstract
{
    public interface IWorkspaceRepository : IRepositoryBase<Workspace>
    {
        Workspace? GetWorkspaceIncludeUser(Expression<Func<Workspace, bool>> expression, Expression<Func<Workspace, bool>> expression2);
    }
}
