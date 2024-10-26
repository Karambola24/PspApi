using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PspApi.Model;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace PspApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GradeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Grade>> GetGrades()
        {
            var grades = _context.Grade.ToList();
            return Ok(grades);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteGrade(int id)
        {
            var grade = await _context.Grade.FindAsync(id);
            if (grade == null)
                return NotFound("Оценка с указанным ID не найдена.");

            _context.Grade.Remove(grade);
            await _context.SaveChangesAsync();

            return Ok("Оценка успешно удалена.");
        }


        [HttpPost("add")]
        public async Task<IActionResult> AddGrade([FromBody] Grade grade)
        {
            if (grade == null)
                return BadRequest("Данные оценки не могут быть пустыми.");

            // Валидация данных
            if (grade.ID_Student <= 0)
                return BadRequest("ID студента должно быть положительным числом.");

            if (grade.Grade_value < 1 || grade.Grade_value > 10)
                return BadRequest("Оценка должна быть в диапазоне от 1 до 10.");

            if (grade.ID_Discipline <= 0)
                return BadRequest("ID дисциплины должно быть положительным числом.");

            // Проверка существования студента
            var studentExists = await _context.Student.AnyAsync(s => s.ID_Student == grade.ID_Student);
            if (!studentExists)
                return BadRequest("Студент с указанным ID не существует.");

            // Проверка существования дисциплины
            var disciplineExists = await _context.Discipline.AnyAsync(d => d.ID_Discipline == grade.ID_Discipline);
            if (!disciplineExists)
                return BadRequest("Дисциплина с указанным ID не существует.");

            var gradeExists = await _context.Grade
       .AnyAsync(g => g.ID_Student == grade.ID_Student && g.ID_Discipline == grade.ID_Discipline);
            if (gradeExists)
                return BadRequest("У данного студента уже есть оценка по этой дисциплине.");

            // Добавление оценки
            _context.Grade.Add(grade);
            await _context.SaveChangesAsync();

            return Ok("Оценка добавлена успешно.");

        }
    }
}