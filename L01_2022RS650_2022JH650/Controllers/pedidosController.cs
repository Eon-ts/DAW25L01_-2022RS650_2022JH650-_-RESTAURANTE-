using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using L01_2022RS650_2022JH650.Models;
using Microsoft.EntityFrameworkCore;

namespace L01_2022RS650_2022JH650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class pedidosController : ControllerBase
    {
        private readonly restauranteDBContext _restauranteDBContexto;
        public pedidosController(restauranteDBContext restauranteDBContexto)
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
            List<pedidos> listadopedidos = (from e in _restauranteDBContexto.pedidos
                                           select e).ToList();
            if (listadopedidos.Count == 0)
            {
                return NotFound();
            }
            return Ok(listadopedidos);
        }
        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id)
        {
            pedidos? pedido = (from e in _restauranteDBContexto.pedidos
                               where e.pedidoid == id
                               select e).FirstOrDefault();
            if (pedido == null)
            {
                return NotFound();
            }
            return Ok(pedido);
        }
        [HttpGet]
        [Route("FindByCliente/{cliente}")]
        public IActionResult FindByCliente(string cliente)
        {
            var pedidosCliente = (from e in _restauranteDBContexto.pedidos
                                  join c in _restauranteDBContexto.clientes on e.clienteid equals c.clienteid
                                  where c.nombrecliente.Contains(cliente)
                                  select e).ToList();

            if (pedidosCliente.Count == 0)
            {
                return NotFound();
            }

            return Ok(pedidosCliente);
        }

        [HttpGet]
        [Route("FindByMotorista/{motorista}")]
        public IActionResult FindByMotorista(string motorista)
        {
            var pedidosMotorista = (from e in _restauranteDBContexto.pedidos
                                    join c in _restauranteDBContexto.motoristas on e.motoristaid equals c.motoristaid
                                    where c.nombremotorista.Contains(motorista)
                                    select e).ToList();

            if (pedidosMotorista.Count == 0)
            {
                return NotFound();
            }

            return Ok(pedidosMotorista);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarEquipo([FromBody] pedidos pedido)
        {
            try
            {
                //para controlar el identity en BD 
                pedido.pedidoid = 0;
                _restauranteDBContexto.pedidos.Add(pedido);
                _restauranteDBContexto.SaveChanges();
                return Ok(pedido);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarEquipo(int id, [FromBody] pedidos pedidoModificar)
        {
            pedidos? pedidoActual = (from e in _restauranteDBContexto.pedidos
                                     where e.pedidoid == id
                                     select e).FirstOrDefault();
            if (pedidoActual == null)
            {
                return NotFound();
            }
            pedidoActual.motoristaid = pedidoModificar.motoristaid;
            pedidoActual.clienteid = pedidoModificar.clienteid;
            pedidoActual.platoid = pedidoModificar.platoid;
            pedidoActual.cantidad = pedidoModificar.cantidad;
            pedidoActual.precio = pedidoModificar.precio;

            _restauranteDBContexto.Entry(pedidoActual).State = EntityState.Modified;
            _restauranteDBContexto.SaveChanges();
            return Ok(pedidoModificar);
        }
        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarEquipo(int id)
        {
            pedidos? pedido = (from e in _restauranteDBContexto.pedidos
                               where e.pedidoid == id
                               select e).FirstOrDefault();
            if (pedido == null)
            {
                return NotFound();

            }
            _restauranteDBContexto.pedidos.Attach(pedido);
            _restauranteDBContexto.pedidos.Remove(pedido);
            _restauranteDBContexto.SaveChanges();

            return Ok(pedido);
        }

    }
}
