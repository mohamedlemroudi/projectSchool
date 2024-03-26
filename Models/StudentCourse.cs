
namespace mla.projectSchool.Models
{
    public class StudentCourse
    {
        public required string StudentDNI { get; set; } 
        public Student? Student { get; set; }
        public required string CourseTitle { get; set; }
        public Course? Course { get; set; }
    }
}
