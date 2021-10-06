using FluentAssertions;
using Librarian.Application.Books.Commands.CreateBook;
using Librarian.Application.Books.Queries.GetBooks;
using Librarian.Domain.Enums;
using System.Threading.Tasks;
using Xunit;

namespace Librarian.IntegrationTests.Books.Queries
{
    using static DbFixture;

    [Collection("DbCollection")]
    public class GetBooksTests
    {
        public GetBooksTests()
        {
            ResetState().GetAwaiter().GetResult();
        }

        [Fact]
        public async Task ShouldGetBooks()
        {
            var command = new CreateBookCommand
            {
                Title = "İnsanlığın Geleceği",
                Authors = "Michio Kaku",
                Publisher = "ODTÜ Yayıncılık",
                Column = 1,
                Row = 1,
                Language = Language.Turkish
            };

            await SendAsync(command);

            command = new CreateBookCommand
            {
                Title = "Saatleri Ayarlama Enstitüsü",
                Authors = "Ahmet Hamdi Tanpınar",
                Publisher = "Dergah Yayınları",
                Column = 1,
                Row = 3,
                Language = Language.Turkish
            };

            await SendAsync(command);

            var query = new GetBooksQuery();
            var books = await SendAsync(query);
            books.BookList.Should().NotBeEmpty();
            books.BookList.Count.Should().Be(2);
        }
    }
}