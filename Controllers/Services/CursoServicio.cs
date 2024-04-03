using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using mla.projectSchool.Data;
using mla.projectSchool.Models;

namespace projectSchool.Services
{
    public class CursoServicio
    {
        private readonly DataContext _context;

        public CursoServicio(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Course>> ObtenerTodosLosCursos()
        {
            return await _context.Course.ToListAsync();
        }

        public async Task<Course> ObtenerCursoPorId(string id)
        {
            return await _context.Course.FindAsync(id);
        }

        public async Task<bool> ActualizarCurso(string id, Course course)
        {
            if (id != course.Title)
            {
                return false;
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
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

        public async Task<bool> CrearCurso(Course course)
        {
            _context.Course.Add(course);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CourseExists(course.Title))
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

        public async Task<bool> EliminarCurso(string id)
        {
            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return false;
            }

            _context.Course.Remove(course);
            await _context.SaveChangesAsync();

            return true;
        }

        private bool CourseExists(string id)
        {
            return _context.Course.Any(e => e.Title == id);
        }
    }
}
