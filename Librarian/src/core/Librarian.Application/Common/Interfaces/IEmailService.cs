using Librarian.Application.Dtos.Email;
using System.Threading.Tasks;

namespace Librarian.Application.Common.Interfaces
{
    /*
     * Email gönderim işlemini tanımlayan servis sözleşmesi.
     * 
     * Sistemde buna benzer işlevsel fonksiyonlar içeren servisleri birer arayüz ile tanımlıyoruz.
     * Böylece bağımlılıkları kolayca çözümletebiliriz. Dependency Injection uygulamak kolay olacaktır.
     * Bu servisler Application katmanında toplanıyorlar. 
     * Interfaces isimli bir klasör içerisinde konuşlandırmak oldukça mantıklı.
     * 
     */
    public interface IEmailService
    {
        Task SendAsync(EmailDto mailDto);
    }
}
