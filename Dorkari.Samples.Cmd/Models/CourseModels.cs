using System;
using System.Collections.Generic;

namespace Dorkari.Samples.Cmd.Models
{
    //NO! I do NOT write many classes in a file for real code
    class CourseDomainModel
    {
        public CourseDomainModel(int id, string name, string desciption, TutorDomainModel tutor)
        {
            Id = id;
            Name = name;
            Description = desciption;
            CourseTutor = tutor;
        }

        public CourseDomainModel(int id, string name, TutorDomainModel tutor)
        {
            Id = id;
            Name = name;
            CourseTutor = tutor;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public Double Duration { get; set; }
        public string Description { get; set; }
        public virtual TutorDomainModel CourseTutor { get; set; }
    }

    class TutorDomainModel
    {
        internal TutorDomainModel(ICollection<CourseDomainModel> courses)
        {
            Courses = courses;
        }
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime TutorDOB { get; set; }
        public ICollection<CourseDomainModel> Courses { get; set; }
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
