using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using mla.projectSchool.Data;
using mla.projectSchool.Models;

namespace projectSchool.Services
{
    public class StudentService
    {
        private readonly DataContext _context;

        public StudentService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetAllStudents()
        {
            return await _context.Student.ToListAsync();
        }

        public async Task<Student> GetStudentById(string id)
        {
            return await _context.Student.FindAsync(id);
        }

        public async Task<bool> UpdateStudent(string id, Student student)
        {
            if (id != student.DNI)
            {
                return false;
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }

        public async Task<bool> CreateStudent(Student student)
        {
            _context.Student.Add(student);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StudentExists(student.DNI))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }

        public async Task<bool> DeleteStudent(string id)
        {
            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return false;
            }

            _context.Student.Remove(student);
            await _context.SaveChangesAsync();

            return true;
        }

        private bool StudentExists(string id)
        {
            return _context.Student.Any(e => e.DNI == id);
        }
    }
}
