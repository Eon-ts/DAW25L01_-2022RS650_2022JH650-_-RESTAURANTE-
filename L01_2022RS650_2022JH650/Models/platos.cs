using System.ComponentModel.DataAnnotations;
namespace L01_2022RS650_2022JH650.Models
{
    public class platos
    {
        [Key]
        public int platoid { get; set; }
        public string nombreplato { get; set; }
        public double precio { get; set; }
        /*
        create table platos (
        platoId int primary key identity,
        nombrePlato varchar(200),
        precio numeric(18,4)
        )*/
    }
}
