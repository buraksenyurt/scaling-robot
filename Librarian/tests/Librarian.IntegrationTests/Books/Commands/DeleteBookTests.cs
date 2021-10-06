using FluentAssertions;
using Librarian.Application.Books.Commands.CreateBook;
using Librarian.Application.Books.Commands.DeleteBook;
using Librarian.Application.Common.Exceptions;
using Librarian.Domain.Entities;
using Librarian.Domain.Enums;
using System.Threading.Tasks;
using Xunit;

namespace Librarian.IntegrationTests.Books.Commands
{
    using static DbFixture;

    [Collection("DbCollection")]
    public class DeleteBookTests
    {
        public DeleteBookTests()
        {
            ResetState().GetAwaiter().GetResult();
        }

        [Fact]
        public void ShouldRequiredAValidBook()
        {
            var command = new DeleteBookCommand
            {
                BookId = 4
            };
            FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<BookNotFoundException>();
        }

        [Fact]
        public async Task ShouldDeleteBook()
        {
            var command = new CreateBookCommand
            {
                Title = "İnsanlığın Gelecği",
                Authors = "Michio",
                Publisher = "ODTÜ Yayıncılık",
                Column = 1,
                Row = 1,
                Language = Language.Turkish
            };

            var id = await SendAsync(command);

            var delCommand = new DeleteBookCommand
            {
                BookId = id
            };
            await SendAsync(delCommand);

            var book = await FindAsync<Book>(id);
            book.Should().BeNull();
        }
    }
}