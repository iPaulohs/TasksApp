using Domain.Entity;

namespace Application.WorkspaceCQ.ViewModels
{
    public class WorkspaceViewModel
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public List<ListCard>? List { get; set; }
        public Guid UserId { get; set; }
    }
}
