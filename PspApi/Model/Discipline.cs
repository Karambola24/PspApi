using System.ComponentModel.DataAnnotations;

namespace PspApi.Model
{
    public class Discipline
    {
        [Key]
        public int ID_Discipline { get; set; }
        public string Name { get; set; }
    }
}
