using Librarian.Data.Contexts;
using Librarian.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Librarian.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        // CQRS geçişinden sonra aşağıdaki metot kullanımları değişecek.

        private readonly LibrarianDbContext _context;
        public BooksController(LibrarianDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Books);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]Book book)
        {            
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return Ok(book);
        }

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete([FromRoute] int id)
        //{
        //    return Ok(id);
        //}

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update([FromRoute] int id,[FromBody] Book book)
        //{

        //    return Ok(book);
        //}
    }
}
