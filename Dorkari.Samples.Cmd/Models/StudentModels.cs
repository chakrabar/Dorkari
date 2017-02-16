using System;
using System.Collections.Generic;

namespace Dorkari.Samples.Cmd.Models
{
    //NO! I do NOT write many classes in a file for real code
    class StudentDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int AdmissionYear { get; set; }
        public DateTime BirthDate { get; set; }
        public string RegNumber { get; set; }
        public List<string> Courses { get; set; }
    }

    class StudentVM
    {
        public string RegistrationNo { get; set; }
        public DateTime BirthDate { get; set; }
        public int AdmissionYear { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<string> Courses { get; set; }
    }
}
