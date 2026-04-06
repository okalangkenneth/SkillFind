namespace Notification.Domain.Events
{
    public record JobPostCreatedEvent
    {
        public Guid JobPostId { get; init; }
        public string Title { get; init; } = string.Empty;
        public string CompanyName { get; init; } = string.Empty;
        public string CategoryName { get; init; } = string.Empty;
        public DateTime PostedAt { get; init; }
    }
}
