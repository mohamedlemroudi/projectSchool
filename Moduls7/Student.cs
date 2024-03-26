
namespace mla.projectSchool.Models
{
    public class Student
    {
        public string DNI { get; set; }
        public string Name { get; set; }
        public ICollection<StudentCourse> StudentCourses { get; set; }
    }
}