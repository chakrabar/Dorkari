using System;
using System.Collections.Generic;

namespace Dorkari.Samples.Cmd.Models
{
    //NO! I do NOT write many classes in a file
    public class CourseDomainModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Double Duration { get; set; }
        public string Description { get; set; }

        public virtual TutorDomainModel CourseTutor { get; set; }
    }

    public class TutorDomainModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime TutorDOB { get; set; }

        public ICollection<CourseDomainModel> Courses;
    }

    public class CourseDTO
    {
        public string Name { get; set; }
        public Double Duration { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
    }
}
