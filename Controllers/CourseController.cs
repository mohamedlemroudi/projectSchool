using Microsoft.AspNetCore.Mvc;
using mla.projectSchool.Models;
using projectSchool.Services;

namespace projectSchool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly CourseService _CourseService;

        public CourseController(CourseService CourseService)
        {
            _CourseService = CourseService;
        }

        // GET: api/Course
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourse()
        {
            return await _CourseService.ObtenerTodosLosCursos();
        }

        // GET: api/Course/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(string id)
        {
            var course = await _CourseService.ObtenerCursoPorId(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // PUT: api/Course/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(string id, Course course)
        {
            var result = await _CourseService.ActualizarCurso(id, course);
            if (!result)
            {
                return BadRequest();
            }

            return NoContent();
        }

        // POST: api/Course
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            var result = await _CourseService.CrearCurso(course);
            if (!result)
            {
                return Conflict();
            }

            return CreatedAtAction("GetCourse", new { id = course.Title }, course);
        }

        // DELETE: api/Course/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(string id)
        {
            var result = await _CourseService.EliminarCurso(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
