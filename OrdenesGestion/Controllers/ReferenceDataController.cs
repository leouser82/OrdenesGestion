using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GestionOrdenes.Interfaces;

namespace GestionOrdenes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReferenceDataController : ControllerBase
    {
        private readonly IReferenceDataRepository _referenceDataRepository;

        public ReferenceDataController(IReferenceDataRepository referenceDataRepository)
        {
            _referenceDataRepository = referenceDataRepository;
        }

        /// <summary>
        /// Obtiene todos los estados de orden disponibles
        /// </summary>
        /// <returns>Lista de estados de orden</returns>
        [HttpGet("estados-orden")]
        public async Task<IActionResult> GetEstadosOrden()
        {
            var estados = await _referenceDataRepository.GetAllEstadosOrdenAsync();
            return Ok(estados);
        }

        /// <summary>
        /// Obtiene todos los tipos de activo disponibles
        /// </summary>
        /// <returns>Lista de tipos de activo</returns>
        [HttpGet("tipos-activo")]
        public async Task<IActionResult> GetTiposActivo()
        {
            var tipos = await _referenceDataRepository.GetAllTiposActivoAsync();
            return Ok(tipos);
        }


        /// <summary>
        /// Obtiene todos los activos financieros disponibles
        /// </summary>
        /// <returns>Lista de activos financieros</returns>
        [HttpGet("activos-financieros")]
        public async Task<IActionResult> GetActivosFinancieros()
        {
            var activos = await _referenceDataRepository.GetAllActivosFinancierosAsync();
            return Ok(activos);
        }

    }
}


