using System.ComponentModel.DataAnnotations;

namespace PspApi.Model
{
    public class Grade
    {
        [Key]
        public int ID_Grade { get; set; }
        public int ID_Student { get; set; }
        public int Grade_value { get; set; }
        public int ID_Discipline { get; set; }

    }
}
