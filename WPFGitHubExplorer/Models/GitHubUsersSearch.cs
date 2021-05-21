using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFGitHubExplorer.Models
{
    public class GitHubUsersSearch
    {
        public string message { get; set; }
        public string documentation_url { get; set; }
        public long total_count { get; set; }
        public bool incomplete_results { get; set; }
        public List<GitHubRepositoryUser> items { get; set; }
    }
}
