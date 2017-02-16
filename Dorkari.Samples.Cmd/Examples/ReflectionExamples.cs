using Dorkari.Helpers.Core.Utilities;
using Dorkari.Samples.Cmd.Models;
using System;
using System.Collections.Generic;

namespace Dorkari.Samples.Cmd.Examples
{
    public class ReflectionExamples
    {
        public static void Show()
        {
            ShowObjectMapping();
            ShowObjectCreation();
        }        

        private static void ShowObjectMapping()
        {
            var student = new StudentDTO
            {
                AdmissionYear = 2015,
                BirthDate = Convert.ToDateTime("2001-11-25"),
                FirstName = "Example",
                LastName = "Kid",
                RegNumber = "15RCS00785",
                Courses = new List<string> { "Maths", "Literature", "Physics", "Music" }
            };
            var studentVM = ReflectionHelper.MapTo<StudentVM>(student);
        }

        private static void ShowObjectCreation()
        {
            var objWithNoCtor = ReflectionHelper.CreateInstance<SoldierDTO>();
            var objWithStaticCtor = ReflectionHelper.CreateInstance<Officer>();
            var objWithComplexCtor = ReflectionHelper.CreateInstance<CourseDomainModel>(true); //populate constructor arguments
            var objWithInterfaceParameter = ReflectionHelper.CreateInstance<TutorDomainModel>();
            var objListOfPrimitive = ReflectionHelper.CreateInstance<List<string>>();
            var objListOfComplex = ReflectionHelper.CreateInstance<List<SoldierDTO>>();
            var objArrayOfComplex = ReflectionHelper.CreateInstance<SoldierDTO[]>();
            var objIEnumOfComplex = ReflectionHelper.CreateInstance<IEnumerable<SoldierDTO>>();
        }
    }
}
