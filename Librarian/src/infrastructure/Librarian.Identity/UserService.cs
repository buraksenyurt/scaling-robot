using Librarian.Application.Common.Interfaces;
using Librarian.Application.Dtos.User;
using Librarian.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Librarian.Identity
{
    public class UserService
        : IUserService
    {
        /*
         * Sembolik bir kullanıcı listesi
         * Normalde kullanıcıların tutulduğu farklı bir ortamdan (Identity Server gibi) sağlanabilir.
         * Sanırım örneğin basitliği için bu şekilde sabit bir kullanıcı listesi söz konusu
         */
        private readonly List<User> _users = new()
        {
            new User
            {
                UserId = 1,
                Name = "Jan Claud",
                LastName = "Van Damme",
                Username = "onlyVan",
                Password = "P@ssw0rd"
            }
        };

        private readonly AuthenticationSettings _authSettings;
        public UserService(IOptions<AuthenticationSettings> appSettings) => _authSettings = appSettings.Value;
        public User GetById(int id) => _users.FirstOrDefault(u => u.UserId == id);

        /* 
         * Kullanıcı adı ve şifresini kontrol edip geriye üretilen JWT token ile birlikte dönen fonksiyon
         */
        public AuthenticationResponse Authenticate(AuthenticationRequest model)
        {
            var user = _users.SingleOrDefault(u => u.Username == model.Username && u.Password == model.Password);

            if (user == null)
                return null;

            var token = GenerateJwtToken(user);
            return new AuthenticationResponse(user, token);
        }
        private string GenerateJwtToken(User user)
        {
            // 1 saat süreyle geçerli olacak bir token üretiliyor
            var key = Encoding.ASCII.GetBytes(_authSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("userId", user.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
