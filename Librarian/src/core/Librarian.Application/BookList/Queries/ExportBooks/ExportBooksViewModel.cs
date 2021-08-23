namespace Librarian.Application.BookList.Queries.ExportBooks
{
    /*
     * Kitapların bir çıktısını CSV olarak verdiğimizde kullanılan ViewModel nesnemiz.
     * Bunu ExportBooksQuery ve Handler tipi kullanmakta.
     * 
     * Dosyanın adını, içeriğin tipini ve byte[] cinsinden içeriği tutuyor.
     * Belki byte[] yerine 64 bit encode edilmiş string içerik de verebiliriz.
     * 
     */
    public class ExportBooksViewModel
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
    }
}
