using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using L01_2022RS650_2022JH650.Models;
using Microsoft.EntityFrameworkCore;

namespace L01_2022RS650_2022JH650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class motoristasController : ControllerBase
    {

        private readonly restauranteDBContext _restauranteDBContexto;
        public motoristasController(restauranteDBContext restauranteDBContexto)
        {
            _restauranteDBContexto = restauranteDBContexto;
        }
        /// <summary>
        ///  Endpoint que retorna
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<motoristas> listadomotoristas = (from e in _restauranteDBContexto.motoristas
                                           select e).ToList();
            if (listadomotoristas.Count == 0)
            {
                return NotFound();
            }
            return Ok(listadomotoristas);
        }
        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id)
        {
            motoristas? motorista = (from e in _restauranteDBContexto.motoristas
                               where e.motoristaid == id
                               select e).FirstOrDefault();
            if (motorista == null)
            {
                return NotFound();
            }
            return Ok(motorista);
        }
        [HttpGet]
        [Route("FindByNombre/{filtro}")]
        public IActionResult FindByNombre(string filtro)
        {
            List<motoristas?> equipo = (from e in _restauranteDBContexto.motoristas
                               where e.nombremotorista.Contains(filtro)
                               select e).ToList();
            if (equipo == null)
            {
                return NotFound();
            }
            return Ok(equipo);
        }
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarMotorista([FromBody] motoristas motorista)
        {
            try
            {
                //para controlar el identity en BD 
                motorista.motoristaid = 0;
                _restauranteDBContexto.motoristas.Add(motorista);
                _restauranteDBContexto.SaveChanges();
                return Ok(motorista);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarMotorista(int id, [FromBody] motoristas motoristaModificar)
        {
            motoristas? motoristaActual = (from e in _restauranteDBContexto.motoristas
                                     where e.motoristaid == id
                                     select e).FirstOrDefault();
            if (motoristaActual == null)
            {
                return NotFound();
            }
            motoristaActual.nombremotorista = motoristaModificar.nombremotorista;

            _restauranteDBContexto.Entry(motoristaActual).State = EntityState.Modified;
            _restauranteDBContexto.SaveChanges();
            return Ok(motoristaModificar);
        }
        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarMotorista(int id)
        {
            motoristas? motorista = (from e in _restauranteDBContexto.motoristas
                               where e.motoristaid == id
                               select e).FirstOrDefault();
            if (motorista == null)
            {
                return NotFound();

            }
            _restauranteDBContexto.motoristas.Attach(motorista);
            _restauranteDBContexto.motoristas.Remove(motorista);
            _restauranteDBContexto.SaveChanges();

            return Ok(motorista);
        }

    }
}
