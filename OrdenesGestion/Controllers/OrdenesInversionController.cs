using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GestionOrdenes.DTOs;
using GestionOrdenes.Interfaces;

namespace GestionOrdenes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdenesInversionController : ControllerBase
    {
        private readonly IOrdenInversionService _ordenService;//sss

        public OrdenesInversionController(IOrdenInversionService ordenService)
        {
            _ordenService = ordenService;
        }

        /// <summary>
        /// Obtiene todas las órdenes de inversión
        /// </summary>
        /// <returns>Lista de órdenes de inversión</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllOrdenes()
        {
            var ordenes = await _ordenService.GetAllOrdenesAsync();
            return Ok(ordenes);
        }

        /// <summary>
        /// Obtiene todas las órdenes de inversión
        /// </summary>
        /// <returns>Lista de órdenes de inversión</returns>
        [HttpGet("{nombre}")]
        public async Task<IActionResult> GetAllOrdenesByNombre(string nombre)
        {
            var ordenes = await _ordenService.GetAllOrdenesAsync();
            return Ok(ordenes);
        }

        /// <summary>
        /// Endpoint para probar el middleware de excepciones
        /// </summary>
        /// <param name="tipoExcepcion">Tipo de excepción a lanzar (1-6)</param>
        /// <returns>Nunca retorna, siempre lanza excepción</returns>
        [HttpGet("test-exception/{tipoExcepcion}")]
        public IActionResult TestException(int tipoExcepcion)
        {
            switch (tipoExcepcion)
            {
                case 1:
                    throw new ArgumentNullException("testParam", "Parámetro requerido es nulo");
                case 2:
                    throw new ArgumentException("Argumento inválido proporcionado");
                case 3:
                    throw new InvalidOperationException("Operación no válida en el estado actual");
                case 4:
                    throw new UnauthorizedAccessException("Acceso no autorizado a este recurso");
                case 5:
                    throw new KeyNotFoundException("El recurso solicitado no fue encontrado");
                case 6:
                    throw new Exception("Error interno del servidor - excepción genérica");
                default:
                    throw new NotImplementedException("Tipo de excepción no implementado");
            }
        }

        /// <summary>
        /// Obtiene una orden de inversión por ID
        /// </summary>
        /// <param name="id">ID de la orden</param>
        /// <returns>Orden de inversión</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrdenById(int id)
        {
            var orden = await _ordenService.GetOrdenByIdAsync(id);
            if (orden == null)
            {
                return NotFound(new { message = "Orden no encontrada" });
            }
            return Ok(orden);
        }

        /// <summary>
        /// Crea una nueva orden de inversión
        /// </summary>
        /// <param name="createDto">Datos para crear la orden</param>
        /// <returns>Orden de inversión creada</returns>
        [HttpPost]
        public async Task<IActionResult> CreateOrden([FromBody] CreateOrdenInversionDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orden = await _ordenService.CreateOrdenAsync(createDto);
            return CreatedAtAction(nameof(GetOrdenById), new { id = orden.Id }, orden);
        }

        /// <summary>
        /// Ejecutar orden de inversión (cambia estado a "Ejecutada")
        /// </summary>
        /// <param name="id">ID de la orden</param>
        /// <returns>Orden ejecutada</returns>
        [HttpPut("{id}/ejecutar")]
        public async Task<IActionResult> EjecutarOrden(int id)
        {
            if (id == 0)
            {
                return BadRequest(new { message = "El ID de la orden no puede ser 0" });
            }
            
            var orden = await _ordenService.EjecutarOrdenAsync(id);
            if (orden == null)
            {
                return NotFound(new { message = "Orden no encontrada" });
            }

            return Ok(new { 
                message = "Orden ejecutada exitosamente", 
                orden = orden,
                nuevoEstado = "Ejecutada"
            });
        }

        /// <summary>
        /// Cancelar orden de inversión (cambia estado a "Cancelada")
        /// </summary>
        /// <param name="id">ID de la orden</param>
        /// <returns>Resultado de la operación</returns>
        [HttpDelete("{id}/cancelar")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CancelarOrden(int id)
        {
            var result = await _ordenService.CancelarOrdenAsync(id);
            if (!result)
            {
                return NotFound(new { message = "Orden no encontrada" });
            }

            return Ok(new { message = "Orden cancelada exitosamente", id = id, nuevoEstado = "Cancelada" });
        }
    }
}