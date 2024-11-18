using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PTV.Application.DTOs;
using PTV.Application.Services;

namespace PTV.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreetsController : ControllerBase
    {
        private readonly StreetService _streetService;

        public StreetsController(StreetService streetService)
        {
            _streetService = streetService;
        }
        [HttpGet]
        public async Task<IActionResult> GetStreets()
        {
            var result= await _streetService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddStreet([FromBody] AddStreetRequest request)
        {
            await _streetService.AddStreetAsync(request);
            return Ok("Street added successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStreet(Guid id)
        {
            await _streetService.DeleteStreetAsync(id);
            return Ok("Street deleted successfully.");
        }

        [HttpPatch("{id}/add-point")]
        public async Task<IActionResult> AddPointToStreet(Guid id, [FromBody] AddPointRequest request)
        {
            await _streetService.AddPointToStreet(id, request);
            return Ok("Geometry added successfully.");
        }

        [HttpPatch("{id}/remove-point")]
        public async Task<IActionResult> RemovePointToStreet(Guid id)
        {
            await _streetService.RemovePointToStreet(id);
            return Ok("Geometry removed successfully.");
        }
    }
}
