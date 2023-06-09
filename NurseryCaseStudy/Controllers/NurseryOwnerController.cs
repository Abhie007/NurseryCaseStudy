using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NurseryCaseStudy.Models;

namespace NurseryCaseStudy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NurseryOwnerController : ControllerBase
    {
        //doing dependecy injection
        public readonly MyDb _context;
        public readonly ILogger<CustomerController> _logger;

        public NurseryOwnerController(MyDb context, ILogger<CustomerController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //create by owner
        [HttpPost]
        public ActionResult<Plant> Create(Plant plant)
        {
            try
            {
                //plant.Id = _context.plants.Count + 1;
                _context.plants.Add(plant);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500);
            }
        }

        //owner can delete also
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var plant = _context.plants.FirstOrDefault(x => x.Id == id);
                if (plant == null)
                {
                    return NotFound();
                }
                _context.plants.Remove(plant);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500);
            }
        }
    }
}
