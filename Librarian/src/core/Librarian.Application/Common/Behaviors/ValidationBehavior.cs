using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Librarian.Application.Common.Behaviors
{
    /*
     * Bir Command çalışıp ekleme, güncelleme gibi işlemler yapıldığı sırada devreye giren doğrulama fonksiyonunu sağlıyoruz.
     * 
     * Eğer talebin geldiği komuta bir Validator uygulanmışsa buradaki süreç çalışıp doğrulama ihlalleri toplanacak.
     * Eğer ihlaller varsa bunu da ValidationException isimli bizim yazdığımı bir nesnede toplayıp sisteme exception basacağız.
     * Doğal olarak talep işlenmeyecek.
     */
    public class ValidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (!_validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var errors = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

            if (errors.Count != 0)
                throw new ValidationException(errors);

            return await next();
        }
    }
}
