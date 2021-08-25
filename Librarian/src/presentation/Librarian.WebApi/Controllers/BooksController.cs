using Librarian.Application.Books.Commands.CreateBook;
using Librarian.Application.Books.Commands.DeleteBook;
using Librarian.Application.Books.Commands.UpdateBook;
using Librarian.Application.Books.Queries.GetBooks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Librarian.WebApi.Controllers
{
    /*
     * BooksController, kitap ekleme, silme, güncelleme ve kitap listesini ViewModel ölçütünde çekme işlemlerini üstleniyor.
     * 
     * Constructor üstünden MediatR modülünün enjekte edildiğini görebiliyoruz.
     * 
     * Önceki versiyonuna göre en büyük fark elbetteki metotlarda Query/Command nesnelerinin kullanılması.
     * 
     * Ayrıca fonksiyon içeriklerine bakıldığında yapılan tek şey Send ile ilgili komutu MeditaR'a yönlendirmek. O gerisini halleder
     * 
     */
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BooksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<BooksViewModel>> Get()
        {
            return await _mediator.Send(new GetBooksQuery());
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateBookCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteBookCommand { BookId = id });
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateBookCommand command)
        {
            if (id != command.BookId)
                return BadRequest();
            await _mediator.Send(command);

            return NoContent();
        }
    }
}
