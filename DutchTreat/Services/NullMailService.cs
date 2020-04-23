using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Services
{
    public class NullMailService : IMailService
    {
        public readonly ILogger<NullMailService> _logger;

        public NullMailService(ILogger<NullMailService> _loggr)
        {
            _logger = _loggr;
        }

        public void sendEmail(string To, string subect, string Body)
        {
            _logger.LogInformation($"To:{To} Subject {subect} Body {Body}");
        }
    }
}
