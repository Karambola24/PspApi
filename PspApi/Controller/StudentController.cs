using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PspApi.Model;
using System.Text.RegularExpressions;

namespace PspApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Student>> GetStudents()
        {
            // Получаем список студентов из базы данных
            var students = _context.Student.ToList();
            return Ok(students);
        }


        // Добавление студента
        [HttpPost("add")]
        public async Task<IActionResult> AddStudent([FromBody] Student student)
        {
            // Валидация данных студента
            if (string.IsNullOrWhiteSpace(student.Full_name) || student.Full_name.Length > 100)
                return BadRequest("Имя студента не должно быть пустым и не должно превышать 100 символов.");

            if (string.IsNullOrWhiteSpace(student.Group_name) || student.Group_name.Length > 50)
                return BadRequest("Название группы не должно быть пустым и не должно превышать 50 символов.");

            if (string.IsNullOrWhiteSpace(student.Major) || student.Major.Length > 50)
                return BadRequest("Специальность не должна быть пустой и не должна превышать 50 символов.");

            if (student.Course < 1 || student.Course > 5)
                return BadRequest("Курс должен быть от 1 до 5.");

            if (student.Semester < 1 || student.Semester > 10)
                return BadRequest("Семестр должен быть от 1 до 10.");

            if (!Regex.IsMatch(student.Full_name, @"^[А-Яа-яЁёA-Za-z\s]+$"))
                return BadRequest("Имя должно содержать только буквы и пробелы.");

            // Проверка на наличие студента с таким же ФИО
            var existingStudent = await _context.Student
                .FirstOrDefaultAsync(s => s.Full_name == student.Full_name);

            if (existingStudent != null)
                return BadRequest("Студент с таким ФИО уже существует.");

            // Добавление студента в базу данных
            _context.Student.Add(student);
            await _context.SaveChangesAsync();

            return Ok("Студент добавлен успешно.");
        }


        // Удаление студента
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Student.FindAsync(id);
            if (student == null)
                return NotFound("Студент с указанным ID не найден.");

            _context.Student.Remove(student);
            await _context.SaveChangesAsync();

            return Ok("Студент успешно удален.");
        }

        [HttpGet("top-students-percentage")]
        public async Task<IActionResult> GetTopStudentsPercentage()
        {
            var totalStudents = await _context.Student.CountAsync();

            if (totalStudents == 0)
            {
                return Ok("Нет студентов для анализа.");
            }

            var topStudentsCount = await _context.Student
                .Where(student => _context.Grade
                    .Where(g => g.ID_Student == student.ID_Student)
                    .All(g => g.Grade_value >= 9)) 
                .CountAsync();

            var percentageTopStudents = (double)topStudentsCount / totalStudents * 100;

            return Ok(percentageTopStudents);
        }

        [HttpGet("struggling-students-percentage")]
        public async Task<IActionResult> GetStrugglingStudentsPercentage()
        {
            var totalStudents = await _context.Student.CountAsync();

            if (totalStudents == 0)
            {
                return Ok("Нет студентов для анализа.");
            }

            var strugglingStudentsCount = await _context.Student
                .Where(student => _context.Grade
                    .Where(g => g.ID_Student == student.ID_Student)
                    .Any(g => g.Grade_value < 4)) 
                .CountAsync();

            var percentageStrugglingStudents = (double)strugglingStudentsCount / totalStudents * 100;

            return Ok(percentageStrugglingStudents);
        }

    }

}

