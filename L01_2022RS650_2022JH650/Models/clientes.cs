using System.ComponentModel.DataAnnotations;
namespace L01_2022RS650_2022JH650.Models
{
    public class clientes
    {
        [Key]
        public int clienteid { get; set; }
        public string? nombrecliente { get; set; }
        public string? direccion { get; set; }
    }
}
