using Domain.Abstract;
using Domain.Entity;
using Infra.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infra.Repository
{
    public class WorkspaceRepository(TasksDbContext context) : RepositoryBase<Workspace>(context), IWorkspaceRepository
    {
        public Workspace? GetWorkspaceIncludeUser(Expression<Func<Workspace, bool>> expression, Expression<Func<Workspace, bool>> expression2)
        {
            return _context.Workspaces.Where(expression)
                .Include(x => x.User)
                .FirstOrDefault(expression2);
        }
    }
}
