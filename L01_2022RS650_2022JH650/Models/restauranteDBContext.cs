using Microsoft.EntityFrameworkCore;
namespace L01_2022RS650_2022JH650.Models
{
    public class restauranteDBContext : DbContext
    {
        public restauranteDBContext(DbContextOptions<restauranteDBContext> options) : base(options)
        {
        }

    }
}
