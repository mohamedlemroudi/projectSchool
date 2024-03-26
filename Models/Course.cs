
namespace mla.projectSchool.Models
{
    public class Course
    {
        public string Title { get; set; }
        public ICollection<StudentCourse> StudentCourses { get; set; } 
    }
}

