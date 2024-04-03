using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using mla.projectSchool.Data;
using mla.projectSchool.Models;

namespace projectSchool.Services
{
    public class StudentCourseService
    {
        private readonly DataContext _context;

        public StudentCourseService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<StudentCourse>> GetAllStudentCourses()
        {
            return await _context.StudentCourse.ToListAsync();
        }

        public async Task<List<StudentCourse>> GetStudentCoursesIncludingRelations()
        {
            // Incluir datos relacionados al recuperar StudentCourses
            return await _context.StudentCourse
                .Include(sc => sc.Student)
                .Include(sc => sc.Course)
                .ToListAsync();
        }

        public async Task<List<Course>> GetCoursesByStudentDNI(string studentDNI)
        {
            // Consultar los cursos asociados al estudiante con el DNI especificado
            return await _context.StudentCourse
                .Where(sc => sc.StudentDNI == studentDNI)
                .Select(sc => sc.Course)
                .ToListAsync();
        }


        public async Task<StudentCourse> GetStudentCourseById(string studentDNI, string courseTitle)
        {
            return await _context.StudentCourse.FindAsync(studentDNI, courseTitle);
        }

        public async Task<bool> AddStudentCourse(StudentCourse studentCourse)
        {
            // 1. Load existing Student and Course entities
            var student = await _context.Student.FindAsync(studentCourse.StudentDNI);
            var course = await _context.Course.FindAsync(studentCourse.CourseTitle);

            if (student == null || course == null)
            {
                throw new Exception("Student or Course not found.");
            }

            // 2. Check for existing enrollment
            var existingStudentCourse = await _context.StudentCourse
                .FirstOrDefaultAsync(sc => sc.StudentDNI == studentCourse.StudentDNI && sc.CourseTitle == studentCourse.CourseTitle);

            if (existingStudentCourse != null)
            {
                throw new Exception("Student already enrolled in the course.");
            }

            // 3. Create StudentCourse entity and set relationships
            var newStudentCourse = new StudentCourse
            {
                StudentDNI = studentCourse.StudentDNI,
                CourseTitle = studentCourse.CourseTitle,
                Student = student,
                Course = course
            };

            _context.StudentCourse.Add(newStudentCourse);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Database error occurred while adding student course.", ex);
            }

            return true;
        }

        public async Task<bool> DeleteStudentCourse(string studentDNI, string courseTitle)
        {
            var studentCourse = await _context.StudentCourse.FindAsync(studentDNI, courseTitle);
            if (studentCourse == null)
            {
                throw new Exception("Student course not found.");
            }

            _context.StudentCourse.Remove(studentCourse);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
