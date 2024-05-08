using Application.Cities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class CitiesController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetCities()
        {
            return HandleResult(await Mediator.Send(new List.Query()));
        }

        [HttpPost]
        public async Task<IActionResult> AddCity(Add.Command command)
        {
            return HandleResult(await Mediator.Send(command));
        }
    }
}
