using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG7312_POE_Part1.Domain
{
    public enum IssueStatus
    {
        Submitted = 0,
        InProgress = 1,
        Resolved = 2
    }

    public class Issue
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Location { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public List<string> Attachments { get; set; } = new List<string>();
        public IssueStatus Status { get; set; } = IssueStatus.Submitted;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

