using Librarian.Application.Books.Queries.ExportBooks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Librarian.WebApi.Controllers
{
    /*
     * Kitap listesini CSV formatında export etmemizi sağlayan Controller.
     * 
     * BooksController'da olduğu gibi MediatR kullanıyor ve CSV çıktısı için ilgili Query komutunu işletiyor.
     */
    [Route("api/[controller]")]
    [ApiController]
    public class ExportController 
        : ControllerBase
    {
        private readonly IMediator _mediator;
        public ExportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<FileResult> Get()
        {
            var model = await _mediator.Send(new ExportBooksQuery());
            return File(model.Content, model.ContentType, model.FileName);
        }
    }
}
