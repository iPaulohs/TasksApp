using Domain.Enum;

namespace Domain.Entity
{
    public class Card
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public StatusCardEnum Status { get; set; }
        public DateTime? DeadLine { get; set; }
        public ListCard? List { get; set; }
        public bool Arquived { get; set; }
    }
}
