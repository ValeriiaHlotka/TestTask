using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Test.Models;
using System.Threading.Tasks;
using System.Web.Http;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using System.Linq.Dynamic.Core;

namespace Test.Controllers
{
    
    [Route("/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        [System.Web.Http.HttpGet]
        public string Get()
        {
            return "Dogs house service. Version 1.0.1";
        }

    }

    [ApiController]
    [Route("/[controller]")]
    public class DogController : ControllerBase
    {
        DogContext db;
        public DogController(DogContext context)
        {
            db = context;
            if (!db.Dogs.Any())
            {
                db.Dogs.Add(new Dog { name = "Neo", color = "red & amber", tail_length = 22, weight = 32 });
                db.Dogs.Add(new Dog { name = "Jessy", color = "black & white", tail_length = 7, weight = 14 });
                db.SaveChanges();
            }
        }
         
        [System.Web.Http.HttpPost]
        public async Task<ActionResult<Dog>> Post(Dog dog)
        {
            if (dog == null || dog.tail_length <= 0 || dog.weight <= 0)
            {
                return BadRequest();
            } else             
            {
                db.Dogs.Add(dog);
                await db.SaveChangesAsync();
                return Ok(dog);
            }

          
        }

    }

    [ApiController]
    [Route("/[controller]")]
    public class DogsController : ControllerBase
    {
        DogContext db;
        public DogsController(DogContext context)
        {
            db = context;
            if (!db.Dogs.Any())
            {
                db.Dogs.Add(new Dog { name = "Neo", color = "red & amber", tail_length = 22, weight = 32 });
                db.Dogs.Add(new Dog { name = "Jessy", color = "black & white", tail_length = 7, weight = 14 });
                db.SaveChanges();
            }
        }

        [System.Web.Http.HttpGet]
        public async Task<ActionResult<IEnumerable<Dog>>> Get([FromUri] string attribute = null, [FromUri] string order = null, [FromUri] string pageNumber = null, [FromUri] string limit = null)
        {
           IOrderedQueryable<Dog> dogList = null;
            if (!(attribute == null && pageNumber == null))
            {
                
                if (attribute != null && order != null)
                {
                    dogList = db.Dogs.OrderBy($"{attribute} {order}");
                    
                }
                if (pageNumber != null && limit != null)
                {
                    int itemsOnPage = 2;
                    if (int.Parse(pageNumber) <= int.Parse(limit))
                    {
                        if (dogList != null)
                        {
                            return await dogList.Skip((int.Parse(pageNumber) - 1) * itemsOnPage).Take(itemsOnPage).ToListAsync();
                        }
                        else
                        {
                            return await db.Dogs.Skip((int.Parse(pageNumber) - 1) * itemsOnPage).Take(itemsOnPage).ToListAsync();
                        }
                    }
                } else return await dogList.ToListAsync();

            }
            return await db.Dogs.ToListAsync();

        }

    }
}