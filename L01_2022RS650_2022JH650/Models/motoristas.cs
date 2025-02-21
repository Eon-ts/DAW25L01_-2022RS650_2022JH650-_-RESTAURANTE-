using System.ComponentModel.DataAnnotations;
namespace L01_2022RS650_2022JH650.Models
{
    public class motoristas
    {
        [Key]
        public int motoristaid { get; set; }
        public string? nombremotorista { get; set; }
    }
}
