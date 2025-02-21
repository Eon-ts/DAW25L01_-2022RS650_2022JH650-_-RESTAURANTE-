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
            var listadopedidos = (from e in _restauranteDBContexto.pedidos
                                  join c in _restauranteDBContexto.clientes
                                  on e.clienteid equals c.clienteid
                                  join m in _restauranteDBContexto.motoristas
                                  on e.motoristaid equals m.motoristaid
                                  join p in _restauranteDBContexto.platos
                                  on e.platoid equals p.platoid
                                  select new
                                  {
                                      e.pedidoid,
                                      e.motoristaid,
                                      Motorista = m.nombremotorista,
                                      e.clienteid,
                                      NombreCliente = c.nombrecliente,
                                      e.platoid,
                                      Plato = p.nombreplato,
                                      e.cantidad,
                                      e.precio
                                  }).ToList();


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
                                  join m in _restauranteDBContexto.motoristas
                                  on e.motoristaid equals m.motoristaid
                                  join p in _restauranteDBContexto.platos
                                  on e.platoid equals p.platoid
                                  where c.nombrecliente.Contains(cliente)
                                  select new
                                  {
                                      e.pedidoid,
                                      e.motoristaid,
                                      Motorista = m.nombremotorista,
                                      e.clienteid,
                                      NombreCliente = c.nombrecliente,
                                      e.platoid,
                                      Plato = p.nombreplato,
                                      e.cantidad,
                                      e.precio
                                  }).ToList();




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
                                    join m in _restauranteDBContexto.motoristas on e.motoristaid equals m.motoristaid
                                    join c in _restauranteDBContexto.clientes
                                    on e.clienteid equals c.clienteid
                                    join p in _restauranteDBContexto.platos
                                    on e.platoid equals p.platoid
                                    where m.nombremotorista.Contains(motorista)
                                    select new
                                    {
                                        e.pedidoid,
                                        e.motoristaid,
                                        Motorista = m.nombremotorista,
                                        e.clienteid,
                                        NombreCliente = c.nombrecliente,
                                        e.platoid,
                                        Plato = p.nombreplato,
                                        e.cantidad,
                                        e.precio
                                    }).ToList();

            if (pedidosMotorista.Count == 0)
            {
                return NotFound();
            }

            return Ok(pedidosMotorista);
        }

        [HttpGet]
        [Route("GetByTopN/{cantidad}")]
        public IActionResult GetCantidad(int cantidad)
        {

            //La única solución que encontramos es agrupar pedidos por plato para ordenarlo descendente (Top N)
            var topPlatos = (from e in _restauranteDBContexto.pedidos group e by e.platoid into platoGroup orderby platoGroup.Count() descending
                             select new
                             {
                                 PlatoId = platoGroup.Key,
                                 CantidadDePedidos = platoGroup.Count()
                             }).Take(cantidad).ToList();

            if (topPlatos.Count == 0)
            {
                return NotFound();
            }

            //Para mostrar con nombre de plato en lugar de id
            var topPlatosNombre = (from t in topPlatos
                                     join p in _restauranteDBContexto.platos on t.PlatoId equals p.platoid
                                     select new
                                     {
                                         PlatoId = t.PlatoId,
                                         Plato = p.nombreplato,
                                         t.CantidadDePedidos
                                     }).ToList();

            return Ok(topPlatosNombre);

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
