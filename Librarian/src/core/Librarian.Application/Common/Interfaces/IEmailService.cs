using Librarian.Application.Dtos.Email;
using System.Threading.Tasks;

namespace Librarian.Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailDto mailDto);
    }
}
