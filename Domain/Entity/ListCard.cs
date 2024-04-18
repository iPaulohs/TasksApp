namespace Domain.Entity
{
    public class ListCard
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public ICollection<Card>? Cards { get; set; }
        public Workspace? Workspace { get; set; }
        public bool Arquived { get; set; }
    }
}
