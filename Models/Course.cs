
namespace mla.projectSchool.Models
{
    public class Course
    {
        public required string Title { get; set; }
        public ICollection<StudentCourse>? StudentCourses { get; set; } 
    }
}

