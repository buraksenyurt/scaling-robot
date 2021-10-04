using System.Threading.Tasks;
using FluentAssertions;
using Librarian.Application.Common.Exceptions;
using Librarian.Application.Books.Commands.CreateBook;
using Xunit;
using Librarian.Domain.Entities;
using Librarian.Domain.Enums;

namespace Librarian.IntegrationTests.Books.Commands
{
    using static DbFixture;

    [Collection("DbCollection")]
    public class CreateBookTests
    {
        public CreateBookTests()
        {
            ResetState().GetAwaiter().GetResult();
        }

        [Fact]
        public void ShouldRequireMinimumFields()
        {
            var command = new CreateBookCommand();
            FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public void ShouldRequirePublisher()
        {
            var command = new CreateBookCommand
            {
                 Title= "Uygulamalı Lineer Cebir",
                 Authors= "Bernard Kolman"
            };

            FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task ShouldCreateBook()
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

            var id = await SendAsync(command);
            var book = await FindAsync<Book>(id);

            book.Should().NotBeNull();
            book.Title.Should().Be(command.Title);
            book.Authors.Should().Be(command.Authors);
            book.Publisher.Should().Be(command.Publisher);
            book.Row.Should().Be(command.Row);
            book.Column.Should().Be(command.Column);
            book.Language.Should().Be(command.Language);
        }
    }
}