using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace L01_2022RS650_2022JH650.Models
{
    public class platos
    {
        [Key]
        public int platoid { get; set; }
        public string? nombreplato { get; set; }
        public decimal precio { get; set; }
        /*
        create table platos (
        platoId int primary key identity,
        nombrePlato varchar(200),
        precio numeric(18,4)
        )*/
    }
}
