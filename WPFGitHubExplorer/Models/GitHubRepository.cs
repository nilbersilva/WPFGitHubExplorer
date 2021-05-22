using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFGitHubExplorer.Models
{
    public class GitHubRepository
    {
        public long id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string html_url { get; set; }
        public GitHubRepositoryUser owner { get; set; }
        public long stargazers_count { get; set; }
    }
}
