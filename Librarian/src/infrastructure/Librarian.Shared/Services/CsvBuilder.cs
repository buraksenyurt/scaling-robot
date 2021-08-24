using CsvHelper;
using Librarian.Application.Books.Queries.ExportBooks;
using Librarian.Application.Common.Interfaces;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Librarian.Shared.Services
{
    /*
     * Kitap kaytılarını CSV dosyada geriye veren fonksiyonelliği içeren sınıfımız.
     * Application içerisinde tanımladığımız servis sözleşmesini uyguladığına dikkat edelim.
     * Yani servisin sözleşmesi Core katmanındaki Application projesinde, uyarlaması Infrastructure içerisindeki Shared projesinde.
     */
    public class CsvBuilder
        : ICsvBuilder
    {
        public byte[] BuildFile(IEnumerable<BookRecord> bookRecords)
        {
            using (var ms = new MemoryStream())
            {
                using (var writer = new StreamWriter(ms))
                {
                    using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
                    csvWriter.WriteRecords(bookRecords);
                }
                return ms.ToArray();
            }
        }
    }
}
