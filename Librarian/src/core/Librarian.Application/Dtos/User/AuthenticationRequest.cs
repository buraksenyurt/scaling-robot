using System.ComponentModel.DataAnnotations;

namespace Librarian.Application.Dtos.User
{
    /*
     * Authentication aşamasında gelen talebin içinde Username ve Password bilgisinin taşınmasını sağlayan sınıfımız
     */
    public class AuthenticationRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
