using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mla.projectSchool.Models;
using projectSchool.Services;

namespace projectSchool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentService _studentService;

        public StudentController(StudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: api/Student
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudent()
        {
            return await _studentService.GetAllStudents();
        }

        // GET: api/Student/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(string id)
        {
            var student = await _studentService.GetStudentById(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        // PUT: api/Student/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(string id, Student student)
        {
            var result = await _studentService.UpdateStudent(id, student);
            if (!result)
            {
                return BadRequest();
            }

            return NoContent();
        }

        // POST: api/Student
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            var result = await _studentService.CreateStudent(student);
            if (!result)
            {
                return Conflict();
            }

            return CreatedAtAction("GetStudent", new { id = student.DNI }, student);
        }

        // DELETE: api/Student/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(string id)
        {
            var result = await _studentService.DeleteStudent(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
