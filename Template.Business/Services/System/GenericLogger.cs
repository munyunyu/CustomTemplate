using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Business.Interfaces.System;

namespace Template.Business.Services.System
{
    public class GenericLogger<T> : IGenericLogger<T>
    {
        private readonly ILogger<T> _logger;

        public GenericLogger(ILogger<T> logger)
        {
            _logger = logger;
        }
        public void LogError(Exception exception, string message, params object[]? args)
        {
            if (args == null) _logger.LogError(exception, message);
            else _logger.LogError(exception, message, args);
        }

        public void LogInformation(string message, params object[]? args)
        {
            if(args == null) _logger.LogInformation(message);
            else _logger.LogInformation(message, args);

        }

        public void LogWarning(string message, params object[]? args)
        {
            if (args == null) _logger.LogWarning(message);
            else _logger.LogWarning(message, args);
        }
    }
}
