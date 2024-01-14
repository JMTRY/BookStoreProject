using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bookStoreProject.DBEFModels;
using bookStoreProject.Models;
using BookStoreProject.DBEFModels;

namespace BookStoreProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly BookDBContext _context;

        public UserController(BookDBContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet("GetBooks")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
          if (_context.Books == null)
          {
              return NotFound();
          }
            return await _context.Books.ToListAsync();
        }

        // GET: api/User/5
        [HttpGet("Info/{id}")]
        public async Task<ActionResult<BookInfoDTO>> GetBookInfo(int id)
        {
          if (_context.Books == null)
          {
              return NotFound();
          }

            var book = _context.Books.Where(book => book.BookID == id)
                                     .Select(bk => new BookInfoDTO
                                           {
                                               BookID = bk.BookID,
                                               Title = bk.Title,
                                               Author = bk.Author,
                                               Description = bk.Description,
                                               Publisher = bk.Publisher,
                                               FirstPublish = bk.FirstPublish,
                                               Language = bk.Language,
                                               Pages = bk.Pages,
                                               Price = bk.Price,
                                               CoverFileName = bk.CoverFileName,
                                               AuthorImage = bk.AuthorImage,
                                               Rating = bk.Rating

                                           }).FirstOrDefault();

            if (book == null)
            {
                return NotFound();
            }

            return await Task.FromResult(book);
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
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

            return book;
        }

        // GET: api/User/FeaturedBooks
        [HttpGet("FeaturedBooks")]
        public async Task<ActionResult<IList<Book>>> GetFeaturedBooks()
        {
            var featuredBooks = await _context.Books.Where(book => book.Featured == true)
                                                    .ToListAsync();

            if (featuredBooks == null || featuredBooks.Count == 0)
            {
                return NotFound();
            }

            return featuredBooks;
        }

        // GET: api/User/DiscountedBooks
        [HttpGet("DiscountedBooks")]
        public async Task<ActionResult<IList<Book>>> GetDiscountedBooks()
        {
            var featuredBooks = await _context.Books.Where(book => book.Discount != 0)
                                                    .ToListAsync();

            if (featuredBooks == null || featuredBooks.Count == 0)
            {
                return NotFound();
            }

            return featuredBooks;
        }

        // GET: api/User/NewBooks
        [HttpGet("NewBooks")]
        public async Task<ActionResult<IList<Book>>> GetNewBooks()
        {
            var featuredBooks = await _context.Books.Where(book => book.IsNew == true)
                                                    .ToListAsync();

            if (featuredBooks == null || featuredBooks.Count == 0)
            {
                return NotFound();
            }

            return featuredBooks;
        }

        private bool BookExists(int id)
        {
            return (_context.Books?.Any(e => e.BookID == id)).GetValueOrDefault();
        }
    }
}
