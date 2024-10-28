using MCC.Services;
using MCC.Models;
using Microsoft.AspNetCore.Mvc;

namespace MCC.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class GradeController : ControllerBase
    {
        private  GradeService _service;
        public GradeController(GradeService service)
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

        [HttpGet("student/{studentId}/subject/{subjectId}")]
        public IActionResult Get(Guid studentId, Guid subjectId)
        {
            try
            {
                var grades = _service.GetGradeByStudentAndSubjectId(studentId, subjectId);
                return Ok(grades);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public IActionResult Post([FromBody] Grade grade)
        {
            try
            {
                return StatusCode(201, _service.Create(grade));
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
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] Grade grade)
        {
            try
            {
                _service.Update(grade);
                return Ok("Grade successfully updated");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
