using FluentAssertions;
using Librarian.Application.Books.Commands.CreateBook;
using Librarian.Application.Books.Commands.UpdateBook;
using Librarian.Application.Common.Exceptions;
using Librarian.Domain.Entities;
using Librarian.Domain.Enums;
using System.Threading.Tasks;
using Xunit;

namespace Librarian.IntegrationTests.Books.Commands
{
    using static DbFixture;

    [Collection("DbCollection")]
    public class UpdateBookTests
    {
        public UpdateBookTests()
        {
            ResetState().GetAwaiter().GetResult();
        }

        [Fact]
        public void ShouldRequiredABook()
        {
            var command = new UpdateBookCommand
            {
                BookId = 4,
                Title = "Applied Algebra",
                Authors = "Unkonwn",
                Publisher = "Science And Neature"
            };
            FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<BookNotFoundException>();
        }

        [Fact]
        public async Task ShouldUpdateBook()
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

            var updCommand = new UpdateBookCommand
            {
                BookId = id,
                Title = "İnsanlığın Geleceği",
                Authors = "Michio Kaku",
                Publisher = "ODTÜ Yayınları"
            };
            await SendAsync(updCommand);

            var book = await FindAsync<Book>(id);

            book.Should().NotBeNull();
            book.Title.Should().Be(updCommand.Title);
            book.Authors.Should().Be(updCommand.Authors);
            book.Publisher.Should().Be(updCommand.Publisher);
        }
    }
}