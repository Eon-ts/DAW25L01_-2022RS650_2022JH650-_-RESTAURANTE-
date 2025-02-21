using L01_2022RS650_2022JH650.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_2022RS650_2022JH650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class platosController : ControllerBase
    {
        private readonly restauranteDBContext _restauranteDBContexto;
        public platosController(restauranteDBContext restauranteDBContexto)
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
            List<platos> listadoEquipo = (from e in _restauranteDBContexto.platos
                                           select e).ToList();
            if (listadoEquipo.Count == 0)
            {
                return NotFound();
            }
            return Ok(listadoEquipo);
        }//No
        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id)
        {
            platos? platos = (from e in _restauranteDBContexto.platos
                               where e.platoid == id
                               select e).FirstOrDefault();
            if (platos == null)
            {
                return NotFound();
            }
            return Ok(platos);
        }
        //Noo
        [HttpGet]
        [Route("Find/{precio}")]
        public IActionResult FindByDescription(decimal precio)
        {
            List<platos> platos = (from e in _restauranteDBContexto.platos
                               where e.precio < precio
                               select e).ToList();
            if (platos == null)
            {
                return NotFound();
            }
            return Ok(platos);
        }


        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarEquipo([FromBody] platos platos)
        {
            try
            {
                //para controlar el identity en BD 
                platos.platoid = 0;
                _restauranteDBContexto.platos.Add(platos);
                _restauranteDBContexto.SaveChanges();
                return Ok(platos);
            }//Esto es para entender mejor el error(solo da mas info)
            catch (DbUpdateException dbEx)
            {
                return BadRequest(dbEx.InnerException?.Message ?? dbEx.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarEquipo(int id, [FromBody] platos platosModificar)
        {
            platos? platosActual = (from e in _restauranteDBContexto.platos
                                     where e.platoid == id
                                     select e).FirstOrDefault();
            if (platosActual == null)
            {
                return NotFound();
            }

            platosActual.nombreplato = platosModificar.nombreplato;
            platosActual.precio = platosModificar.precio;


            _restauranteDBContexto.Entry(platosActual).State = EntityState.Modified;
            _restauranteDBContexto.SaveChanges();
            return Ok(platosModificar);
        }
        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarEquipo(int id)
        {
            platos? equipo = (from e in _restauranteDBContexto.platos
                               where e.platoid == id
                               select e).FirstOrDefault();
            if (equipo == null)
            {
                return NotFound();

            }
            _restauranteDBContexto.platos.Attach(equipo);
            _restauranteDBContexto.platos.Remove(equipo);
            _restauranteDBContexto.SaveChanges();

            return Ok(equipo);
        }
    }
}
