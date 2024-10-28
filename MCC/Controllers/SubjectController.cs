using MCC.Models;
using MCC.Services;
using Microsoft.AspNetCore.Mvc;

namespace MCC.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class SubjectController : ControllerBase
    {
        private SubjectService _service;
        public SubjectController(SubjectService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_service.GetAll());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                return Ok(_service.Get(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public IActionResult Post([FromBody] Subject subject)
        {
            try
            {
                return StatusCode(201, _service.Create(subject));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _service.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] Subject subject)
        {
            try
            {
                _service.Update(subject);
                return Ok("Subject successfully updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
