using System.ComponentModel.DataAnnotations;

namespace PspApi.Model
{
    public class Student
    {
        [Key]
        public int ID_Student { get; set; }
        public string Full_name { get; set; }
        public string Major { get; set; }
        public int Course { get; set; }
        public int Semester { get; set; }
        public string Group_name { get; set; }
    }
}
