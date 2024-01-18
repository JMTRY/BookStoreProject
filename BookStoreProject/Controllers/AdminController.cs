using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bookStoreProject.DBEFModels;
using bookStoreProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BookStoreProject.DBEFModels;
using static System.Collections.Specialized.BitVector32;

namespace BookStoreProject.Controllers
{
    [Authorize] 
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly BookDBContext _context;
        private readonly IdentityDbContext _identityContext;

        public AdminController(BookDBContext context, IdentityDbContext identityContext)
        {
            // The context represents the session with the database and is responsible for tracking changes,
            // managing connections, and performing database operations.
            // DbContext keeps track of changes made to entities.
            _context = context;
            _identityContext = identityContext;
        }

        // GET: api/Admin
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
          if (_context.Books == null)
          {
              return NotFound(); // Returns 404 Not Found response 
          }
          // _context.Books represents the query that selects the "Books" entity set from the database.
          // It doen's execute the query.
            return await _context.Books.ToListAsync();
          // ToListAsync() execute the query asynchronously and convert the result into a list.
        }

        // GET: api/Admin/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
          if (_context.Books == null)
          {
              return NotFound();
          }
            // FindAsync method is a convenient method provided by Entity Framework for retrieving an entity based on its primary key asynchronously.
            // It is a part of the DbSet<T> class
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // PUT: api/Admin/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.BookID)
            {
                return BadRequest();
            }

            // inform Entity Framework that the book entity is in a modified state
            _context.Entry(book).State = EntityState.Modified;

            try
            {
                // save the changes made to the book.
                // This method will call the .DetectChanges() method to discover any changes to the entity instances before saving them in the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Admin
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
          if (_context.Books == null)
          {
              return Problem("Entity set 'BookDBContext.Books'  is null.");
          }
            // Add method is used to add an entity to the DbSet.
            // This does not immediately insert the record into the database.
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            // CreatedAtAction represents an HTTP response with a 201 status code (Created)
            return CreatedAtAction("GetBook", new { id = book.BookID }, book);
        }

        // DELETE: api/Admin/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            if (_context.Books == null)
            {
                return NotFound();
            }
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            // Remove method is used to mark an entity for deletion.
            _context.Books.Remove(book);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Admin
        [HttpGet("GetUsers")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            if (_context.Books == null)
            {
                return NotFound();
            }

            var users = _identityContext.ApiUsers
                                     .Select(user => new UserDTO
                                     {
                                        id = user.Id,
                                        Email = user.Email,
                                        PhoneNumber = user.PhoneNumber

                                     }).ToList();

            return await Task.FromResult(users);
        }

        private bool BookExists(int id)
        {
            return (_context.Books?.Any(e => e.BookID == id)).GetValueOrDefault();
        }
    }
}
