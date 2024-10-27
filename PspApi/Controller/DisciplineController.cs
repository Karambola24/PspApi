using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PspApi.Model;
using System.Text.RegularExpressions;

namespace PspApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisciplineController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DisciplineController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Discipline>> GetDisciplines()
        {
            var disciplines = _context.Discipline.ToList();
            return Ok(disciplines);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteDiscipline(int id)
        {
            var discipline = await _context.Discipline.FindAsync(id);
            if (discipline == null)
                return NotFound("Дисциплина с указанным ID не найдена.");

            _context.Discipline.Remove(discipline);
            await _context.SaveChangesAsync();

            return Ok("Дисциплина успешно удалена.");
        }


        [HttpPost("add")]
        public async Task<IActionResult> AddDiscipline([FromBody] Discipline discipline)
        {
            if (string.IsNullOrWhiteSpace(discipline.Name) || discipline.Name.Length > 100)
                return BadRequest("Название дисциплины не должно быть пустым и не должно превышать 100 символов.");

            if (!Regex.IsMatch(discipline.Name, @"^[А-Яа-яЁёA-Za-z\s]+$"))
                return BadRequest("Название дисциплины должно содержать только буквы и пробелы.");

            var existingDiscipline = await _context.Discipline
                .FirstOrDefaultAsync(d => d.Name == discipline.Name);

            if (existingDiscipline != null)
                return BadRequest("Дисциплина с таким названием уже существует.");

            _context.Discipline.Add(discipline);
            await _context.SaveChangesAsync();

            return Ok("Дисциплина добавлена успешно.");
        }

    }
}
