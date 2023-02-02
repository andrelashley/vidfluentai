using Microsoft.Extensions.Hosting;
using VidFluentAI.Models;

namespace VidFluentAI.Services
{
    public interface IJobsRepository
    {
        public IEnumerable<Job> AllJobs { get; }
    }
}
