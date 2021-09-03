using Librarian.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Librarian.Identity
{
    /*
     * Servis çağrısının yetkilendirilmesi ile ilgili kontrolü sağlayan nitelik tipi
     * 
     * Sadece sınıf ve metot seviyesinde uygulanabilir.
     * 
     * Fonksiyon içinde o anki Http Context'i içinden User içeriği çekilir eğer bu içerik null ise
     * yetkisiz erişim olduğuna kanaat getirilir ve çağıran taraf HTTP 401 ile cezalandırılır.
     * 
     * Attribute'un kullanıldığı Controller tipleri yetki kontrolüne tabi olur.
     * 
     */
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (User)context.HttpContext.Items["User"];
            if (user == null)
            {
                context.Result = new JsonResult(new
                {
                    message = "Unauthorized Access Detected"
                })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
        }
    }
}
