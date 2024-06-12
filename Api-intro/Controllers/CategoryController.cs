using Api_intro.Data;
using Api_intro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_intro.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    List<string> categories = new List<string>()
        //    {
        //        "Web design",
        //        "Programming",
        //        "Digital marketing"
        //    };

        //    return Ok(categories);
        //}

        //[HttpGet("{id}")]
        //public IActionResult GetById([FromRoute] int id)
        //{
        //    return Ok("Programming" + id);
        //}

        //[HttpGet]
        //public IActionResult GetAllWithParameters([FromQuery] int id, [FromQuery] string name)
        //{
        //    return Ok();
        //}

        //[HttpPost]
        //public IActionResult FileUpload([FromForm] Test test)
        //{
        //    return Ok();
        //}

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Categories.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var data = await _context.Categories.FindAsync(id);

            if (data is null) return NotFound();

            return Ok(data);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int? id)
        {
            if (id is null) return BadRequest();

            var data = await _context.Categories.FindAsync(id);

            if (data is null) return NotFound();

            _context.Categories.Remove(data);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Category category)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Created", category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] Category category)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var data = await _context.Categories.FindAsync(id);

            if (data is null) return NotFound();

            data.Name = category.Name;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string? search)
        {
            return Ok(search == null ? await _context.Categories.ToListAsync() : await _context.Categories.Where(m => m.Name.Contains(search)).ToListAsync());
        }
    }
}
