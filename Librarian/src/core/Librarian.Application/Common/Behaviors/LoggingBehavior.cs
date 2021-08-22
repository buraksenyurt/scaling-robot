using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Librarian.Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest>
        : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger _logger;
        public LoggingBehavior(ILogger logger)
        {
            _logger = logger;
        }
        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            _logger.LogInformation($"Talep geldi {request}", requestName);
        }
    }
}
