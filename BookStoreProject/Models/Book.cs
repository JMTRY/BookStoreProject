namespace bookStoreProject.Models
{
    public class Book
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public string? Author { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
        public string? Publisher { get; set; }
        public DateTime? FirstPublish { get; set; }
        public string? Language { get; set; }
        public int? Pages { get; set; }
        public decimal? Price { get; set; }
        public string? CoverFileName { get; set; }
        public string? AuthorImage { get; set; }
        public decimal? Rating { get; set; }

        public bool Featured { get; set; }
        public int Discount { get; set; }
        public bool IsNew { get; set; }


    }

}

