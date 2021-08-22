namespace Librarian.Application.BookList.Queries.ExportBooks
{
    public class ExportBooksViewModel
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
    }
}
