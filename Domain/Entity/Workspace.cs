namespace Domain.Entity
{
    public class Workspace
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public ICollection<ListCard>? List { get; set; }
        public bool Arquived { get; set; }
        public User? User { get; set; }
    }
}
