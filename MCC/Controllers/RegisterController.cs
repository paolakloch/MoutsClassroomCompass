using MCC.Data.Dtos;
using MCC.Services;
using Microsoft.AspNetCore.Mvc;

namespace MCC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly RegisterService _registerService;

        public RegisterController(RegisterService registerService)
        {
            _registerService = registerService;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(CreateUserDto dto)
        {
            await _registerService.RegisterAsync(dto);
            return Ok("Usuário Cadastrado!");
        }
    }
}
