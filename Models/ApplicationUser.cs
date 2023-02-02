using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

namespace VidFluentAI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
           Jobs = new List<Job>();
        }
        public bool IsSubscriptionActive { get; set; }
        public string CustomerId { get; set; } = string.Empty;
        public string SubscriptionId { get; set; } = string.Empty;
        public List<Job>? Jobs { get; set; }
    }
}
