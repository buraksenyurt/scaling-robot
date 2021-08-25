using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librarian.Application.Common.Behaviors
{
    /*
     * Pipeline arayüzünden türeyen bu behavior tipi ile Query ve Command'lerin işleyişi sırasında oluşan exception durumlarını loglamaktayız.
     * Yakalanan Exception akışta ele alınmamış bir exception ise burada yakalamamız kolay olacaktır.
     * 
     */
    public class UnhandledExceptionBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<TRequest> _logger;
        public UnhandledExceptionBehavior(ILogger<TRequest> logger)
        {
            _logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next();
            }
            catch (Exception excp)
            {
                _logger.LogError(excp, "Talep geldi ama bir exception oluştu");
                throw;
            }
        }
    }
}
