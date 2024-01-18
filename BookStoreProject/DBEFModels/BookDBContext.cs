using bookStoreProject.Models;
using BookStoreProject.DBEFModels;
using Microsoft.EntityFrameworkCore;

namespace bookStoreProject.DBEFModels
{
    // class used to manage the interactions with a database containing a set of "Book"
    // DbContext is the main class responsible for interacting with the database and performing CRUD (Create, Read, Update, Delete) operations.
    public class BookDBContext : DbContext
    {
        // DbContext manages the connection to the database.
        // It opens the connection when needed, executes commands, and closes the connection when the work is done
        // It uses change tracking to keep track of changes made to entities.

        public BookDBContext(DbContextOptions<BookDBContext> options) : base(options) { }
        

        // Represents a table named "Books" in the database.
        // DbSet is used to query and save instances of Book.
        // LINQ queries against a DbSet will be translated into queries against the database.
        public DbSet<Book> Books { get; set; }


    }
}
