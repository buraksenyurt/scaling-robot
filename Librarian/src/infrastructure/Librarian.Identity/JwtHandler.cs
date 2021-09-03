using Librarian.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Librarian.Identity
{
    public class JwtHandler
    {
        private readonly RequestDelegate _next;
        private readonly AuthenticationSettings _authenticationSettings;

        public JwtHandler(RequestDelegate next, IOptions<AuthenticationSettings> appSettings)
        {
            _next = next;
            _authenticationSettings = appSettings.Value;
        }

        /*
         * Gelen talep işlenirken invoke çağrısında araya giriyoruz.
         * Burada token kontrolü yapıp akışın devam etmesine izin vereceğiz
         */
        public async Task Invoke(HttpContext context, IUserService userService)
        {
            //Hele sen bi gel Header diyip içinden Authorization kısmını ayıklıyor
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            // Ayrıştırılan kısmın token bilgisini içerdiğini düşünüyoruz ve eğer null değilse
            if (token != null)
            {
                try
                {
                    // Settings üstünden gelecek secret değerini baz alarak Header üstünden gelen token bir kontrol ediliyor
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_authenticationSettings.Secret);
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    }, out var validatedToken);

                    // Bu satıra exception oluşmadan gelindiyse token geçerli diyebiliriz.
                    var jwtToken = (JwtSecurityToken)validatedToken;
                    // token parse edilince içinde userId bilgisini yakalıyoruz
                    var userId = int.Parse(jwtToken.Claims.First(c => c.Type == "userId").Value);
                    // userId bilgisinden kullanıcıya ait tanımlı kimlik bilgilerini istiyoruz ve bunu akan Http Context içerisine User isimli öğe olarak ekliyoruz
                    context.Items["User"] = userService.GetById(userId);
                }
                catch
                {
                }
            }

            await _next(context); // Akış sonraki adımla devam ediyor
        }
    }
}
