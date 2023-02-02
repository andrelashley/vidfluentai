namespace VidFluentAI.Models
{
    public class Job
    {
        public int Id { get; set; }
        public string Input { get; set; } = string.Empty;
        public string Output { get; set; } = string.Empty;
        public JobType JobType { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public string? ApplicationUserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
