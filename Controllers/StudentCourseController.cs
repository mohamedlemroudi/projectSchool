using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mla.projectSchool.Data;
using mla.projectSchool.Models;

namespace projectSchool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentCourseController : ControllerBase
    {
        private readonly DataContext _context;

        public StudentCourseController(DataContext context)
        {
            _context = context;
        }

        // GET: api/StudentCourse
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentCourse>>> GetStudentCourse()
        {
            return await _context.StudentCourse.ToListAsync();
        }

        // GET: api/StudentCourse/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentCourse>> GetStudentCourse(string id)
        {
            var studentCourse = await _context.StudentCourse.FindAsync(id);

            if (studentCourse == null)
            {
                return NotFound();
            }

            return studentCourse;
        }

        // GET: api/StudentCourse/GetStudentCourses
        [HttpGet("GetStudentCourses")]
        public async Task<ActionResult<IEnumerable<StudentCourse>>> GetStudentCourses()
        {
            // Incluir datos relacionados al recuperar StudentCourses
            var studentCourses = await _context.StudentCourse
                .Include(sc => sc.Student)
                .Include(sc => sc.Course)
                .ToListAsync();

            return studentCourses;
        }

        // POST: api/StudentCourse/AddCourseToStudent
        [HttpPost("AddCourseToStudent")]
        public async Task<ActionResult<StudentCourse>> AddStudentCourse([FromBody] StudentCourse request)
        {
            // 1. Load existing Student and Course entities
            var student = await _context.Student.FindAsync(request.StudentDNI);
            var course = await _context.Course.FindAsync(request.CourseTitle);

            if (student == null || course == null)
            {
                return NotFound("Student or Course not found.");
            }

            // 2. Check for existing enrollment
            var existingStudentCourse = await _context.StudentCourse
                .FirstOrDefaultAsync(sc => sc.StudentDNI == request.StudentDNI && sc.CourseTitle == request.CourseTitle);

            if (existingStudentCourse != null)
            {
                return Conflict("Student already enrolled in the course.");
            }

            // 3. Create StudentCourse entity and set relationships
            var studentCourse = new StudentCourse
            {
                StudentDNI = request.StudentDNI,
                CourseTitle = request.CourseTitle,
                Student = student,
                Course = course
            };

            _context.StudentCourse.Add(studentCourse);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Handle database-related errors
                throw; // Rethrow for appropriate handling in a higher layer
            }

            return CreatedAtAction("GetStudentCourse", new { id = request.StudentDNI }, studentCourse);
        }



        // PUT: api/StudentCourse/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentCourse(string id, StudentCourse studentCourse)
        {
            if (id != studentCourse.StudentDNI)
            {
                return BadRequest();
            }

            _context.Entry(studentCourse).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentCourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/StudentCourse
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StudentCourse>> PostStudentCourse(StudentCourse studentCourse)
        {
            _context.StudentCourse.Add(studentCourse);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StudentCourseExists(studentCourse.StudentDNI))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStudentCourse", new { id = studentCourse.StudentDNI }, studentCourse);
        }

        // DELETE: api/StudentCourse/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentCourse(string id)
        {
            var studentCourse = await _context.StudentCourse.FindAsync(id);
            if (studentCourse == null)
            {
                return NotFound();
            }

            _context.StudentCourse.Remove(studentCourse);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentCourseExists(string id)
        {
            return _context.StudentCourse.Any(e => e.StudentDNI == id);
        }
    }
}
