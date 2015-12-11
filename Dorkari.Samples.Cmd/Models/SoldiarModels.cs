using System;
using System.Collections.Generic;

namespace Dorkari.Samples.Cmd.Models
{
    public class SoldierDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public List<int> YearsFought { get; set; }
    }

    public class Officer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string BaseGroup { get; set; }
        public DateTime ActiveFrom { get; set; }
        public string Type { get; set; }
    }
}
