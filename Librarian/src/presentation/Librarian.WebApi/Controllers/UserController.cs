using Librarian.Application.Common.Interfaces;
using Librarian.Application.Dtos.User;
using Microsoft.AspNetCore.Mvc;

namespace Librarian.WebApi.Controllers
{
    /*
     * Kullanıcı doğrulama işini üstlenen controller tipidir.
     * Authenticat fonksiyonu /auth talebi ile çalışır ve Body içeriği ile gelen model nesnesindeki kullanıcı adı şifre üstünden UserService'e gidilir.
     * Geçerli bir kullanıcı ise tamam ama değilse HTTP 400 Bad Request hatası basarız.
     */
    public class UserController
        : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService) => _userService = userService;

        [HttpPost("auth")]
        public IActionResult Authenticate([FromBody] AuthenticationRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new
                {
                    message = "Kullanıcı adın ya da şifren hatalı!"
                });

            return Ok(response);
        }
    }
}
