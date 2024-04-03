
namespace mla.projectSchool.Models
{
    public class Student
    {
        public required string DNI { get; set; }
        public required string Name { get; set; }
        public ICollection<StudentCourse>? StudentCourses { get; set; }
    }
}