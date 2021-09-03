using Librarian.Application.Dtos.User;
using Librarian.Domain.Entities;

namespace Librarian.Application.Common.Interfaces
{
    /*
     * Kullanıcı servisi response içeriği ile gelen bilgilere göre doğrulama işini üstlenen
     * ve id üstünden kullanıcı bilgisi döndüren fonksiyonları tarifleyen bir sözleşme sunuyor.
     */
    public interface IUserService
    {
        AuthenticationResponse Authenticate(AuthenticationRequest model);
        User GetById(int userId);
    }
}
