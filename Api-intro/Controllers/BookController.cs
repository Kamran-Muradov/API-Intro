using Api_intro.Data;
using Api_intro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_intro.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Book book)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            return Created("Created", book);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Books.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _context.Books.FindAsync(id);

            if (data is null) return NotFound();

            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> SearchByName(string? search)
        {
            return Ok(search == null ? await _context.Books.ToListAsync() : await _context.Books.Where(m => m.Name.Contains(search)).ToListAsync());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] Book book)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var data = await _context.Books.FindAsync(id);

            if (data is null) return NotFound();

            data.Name = book.Name;
            data.Author = book.Author;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int? id)
        {
            if (id is null) return BadRequest(ModelState);

            var data = await _context.Books.FindAsync(id);

            if (data is null) return NotFound();

            _context.Books.Remove(data);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
