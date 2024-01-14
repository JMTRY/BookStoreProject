﻿using bookStoreProject.Models;
using Microsoft.EntityFrameworkCore;

namespace bookStoreProject.DBEFModels
{
    public class BookDBContext : DbContext
    {
        public BookDBContext(DbContextOptions<BookDBContext> options) : base(options) { }
        

        public DbSet<Book> Books { get; set; }
    }
}