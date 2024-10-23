using MCC.Data.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace MCC.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class UserController : ControllerBase
    {
        [HttpPost]
        public IActionResult RegisterUser
            (CreateUserDto dto)
        {
           
        }
    }
}
