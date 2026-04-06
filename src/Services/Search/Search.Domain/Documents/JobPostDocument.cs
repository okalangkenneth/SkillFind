namespace Search.Domain.Documents
{
    public class JobPostDocument
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string ExperienceLevel { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public DateTime PostedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
