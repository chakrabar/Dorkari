using System;
using System.Collections.Generic;

namespace Dorkari.Samples.Cmd.Models
{
    //NO! I do NOT write many classes in a file
    public class SoldierDTO : IEquatable<SoldierDTO>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public List<int> YearsFought { get; set; }

        public bool Equals(SoldierDTO other)
        {
            return this.Id == other.Id;
        }
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
