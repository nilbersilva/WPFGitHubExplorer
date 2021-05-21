using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFGitHubExplorer.Models
{
    public class GitHubRepositoryUser
    {
        public string login { get; set; }
        public long id { get; set; }
        public string avatar_url { get; set; }
        public string html_url { get; set; }
    }
}
