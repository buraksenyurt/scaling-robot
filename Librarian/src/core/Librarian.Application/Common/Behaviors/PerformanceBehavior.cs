using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Librarian.Application.Common.Behaviors
{
    /*
     * Taleplerin ne kadar sürede işlendiğini tespit etmek ve belli bir değerin altına indiğinde uyarı logu basmak için bu uyarlama oldukça kullanışlı.
     * 
     * LoggingBehavior'un uyguladığı arayüzden farklı bir arayüz uygulandığına dikkat etmek lazım.
     * 
     * Handle metodunda mesajı hat üzerindeki bir sonraki noktaya taşıdığımız next fonksiyonunun önüne ve sonrasına zamanlayıcı çağrıları koyduk.
     * Sonrasında cevaplama süresinin 250 milisaniye altında olup olmadığına bakıp uyarı logu basıyoruz.
     * 
     */
    public class PerformanceBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger _logger;
        private readonly Stopwatch _hgwells;
        public PerformanceBehavior(ILogger logger)
        {
            _logger = logger;
            _hgwells = new Stopwatch();
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _hgwells.Start();
            var response = await next();
            _hgwells.Stop();

            var responseDuration = _hgwells.ElapsedMilliseconds;
            if (responseDuration < 250)
                return response;

            _logger.LogWarning($"{typeof(TRequest).Name} normal çalışma süresini aştı. {responseDuration}. İçerik {request}");
            return response;

        }
    }
}
