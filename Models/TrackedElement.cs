using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STAG.Models
{
    internal class TrackedElement
    {
        public Guid Id { get; set; }
        public Dictionary<string, string> UserObjectAttributes { get; set; } = new Dictionary<string, string>();

        // List to store change history
        public List<string> ChangeHistory { get; set; } = new List<string>();

        // Optionally, add methods to log changes
        public void LogChange(string change)
        {
            ChangeHistory.Add($"{DateTime.Now}: {change}");
        }
    }
}