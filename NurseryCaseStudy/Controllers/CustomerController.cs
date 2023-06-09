using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NurseryCaseStudy.Models;

namespace NurseryCaseStudy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        //doing dependecy injection
        public readonly MyDb _context;
        public readonly ILogger<CustomerController> _logger;

        public CustomerController(MyDb context, ILogger<CustomerController> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogInformation("test");
        }

        [HttpGet("ListOfPlants")]
        public ActionResult<IEnumerable<Plant>> Get()
        {
            try
            {
                return _context.plants.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500);
            }
        }

        //adding to cart 
        [HttpPost("AddToCart")]
        public ActionResult<Cart> AddToCart(Cart cart)
        {
            try { 
                    var plant = _context.plants.FirstOrDefault(x => x.Id == cart.Id);
                    if (plant == null)
                    {
                        return NotFound();
                    }
                    if (plant.quantity < cart.quantity)
                    {
                        return BadRequest("Pls try again, stock is less");
                    }
                    //if data already added
                    var doesExist = _context.carts.FirstOrDefault(x => x.Id == cart.Id);
                    if (doesExist == null)
                    {
                        //we add
                        _context.carts.Add(cart);
                        _context.SaveChanges();
                        return Ok();

                    }
                    else
                    {
                        doesExist.quantity = doesExist.quantity + cart.quantity;
                        return Ok();
                    }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500);
            }
        }

        //remove--- if customer removes from cart
        [HttpPost("RemoveFromCart")]
        public ActionResult<Cart> RemoveFromCart(Cart cart)
        {
            try
            {
                var doesExist = _context.carts.FirstOrDefault(x => x.Id == cart.Id);
            if (doesExist == null)
            {
                return NotFound();
            }
            if (doesExist.quantity < cart.quantity)
            {
                return BadRequest("Pls enter valid quality");
            }
            else
            {
                _context.carts.Remove(cart);
                _context.SaveChanges();
                //now if removed from here we add to list?                
                return Ok();
            }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500);
            }

        }


        //when guy is purchasing 
        [HttpPost("Purchase")]
        public ActionResult<Purchase> Purchase(Purchase purchase)
        {
            try
            {

                var cart = _context.carts.ToList();

                foreach (var item in cart)
                {
                    var plant = _context.plants.FirstOrDefault(x => x.Id == item.Id);
                    if (plant == null || plant.quantity < item.quantity)
                    {
                        return BadRequest("Some Items are Out of Stock");
                    }
                    plant.quantity = plant.quantity - item.quantity;
                }



                _context.purchares.Add(purchase);
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
