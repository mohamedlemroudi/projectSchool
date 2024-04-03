using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mla.projectSchool.Models;
using projectSchool.Services;

namespace projectSchool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentCourseController : ControllerBase
    {
        private readonly StudentCourseService _studentCourseService;

        public StudentCourseController(StudentCourseService studentCourseService)
        {
            _studentCourseService = studentCourseService;
        }

        // GET: api/StudentCourse
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentCourse>>> GetStudentCourse()
        {
            return await _studentCourseService.GetAllStudentCourses();
        }

        // GET: api/StudentCourse/5
        [HttpGet("{studentDNI}/{courseTitle}")]
        public async Task<ActionResult<StudentCourse>> GetStudentCourse(string studentDNI, string courseTitle)
        {
            try
            {
                var studentCourse = await _studentCourseService.GetStudentCourseById(studentDNI, courseTitle);
                if (studentCourse == null)
                {
                    return NotFound();
                }

                return studentCourse;
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpGet("GetStudentCourses")]
        public async Task<ActionResult<IEnumerable<StudentCourse>>> GetStudentCourses()
        {
            try
            {
                var studentCourses = await _studentCourseService.GetStudentCoursesIncludingRelations();
                return Ok(studentCourses);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        // POST: api/StudentCourse/AddCourseToStudent
        [HttpPost("AddCourseToStudent")]
        public async Task<ActionResult<StudentCourse>> AddStudentCourse(StudentCourse studentCourse)
        {
            try
            {
                var result = await _studentCourseService.AddStudentCourse(studentCourse);
                return CreatedAtAction("GetStudentCourse", new { studentDNI = studentCourse.StudentDNI, courseTitle = studentCourse.CourseTitle }, studentCourse);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        // DELETE: api/StudentCourse/5
        [HttpDelete("{studentDNI}/{courseTitle}")]
        public async Task<IActionResult> DeleteStudentCourse(string studentDNI, string courseTitle)
        {
            try
            {
                var result = await _studentCourseService.DeleteStudentCourse(studentDNI, courseTitle);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
