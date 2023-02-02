namespace VidFluentAI.Models
{
    public class DemoJob
    {
        public int Id { get; set; }
        public string Input { get; set; } = string.Empty;
        public string Output { get; set; } = string.Empty;
        public JobType JobType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string EmailAddress { get; set; } = string.Empty;
    }
}
