using System;
using System.Collections.Generic;
using System.Text;

namespace CloudOpsDashboard.Core.Entities
{
    public class CloudAlert
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Severity { get; set; } = string.Empty;

        public string? Instance { get; set; }

        public string? Region { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsResolved { get; set; }
    }
}