using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Server.Shared
{
    public interface ILoggerManager
    {
        void LogInfo(string message);
        void LogWarn(string message);
        void LogError(string message, Exception ex = null);
        void LogFatal(string message, Exception ex = null);
    }

    public class Logger : ILoggerManager
    {
        private readonly ILog _logger;

        public Logger(Type type)
        {
            _logger = LogManager.GetLogger(type);
        }

        public Logger()
        {
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        public void LogInfo(string message)
        {
            _logger.Info(message);
        }

        public void LogWarn(string message)
        {
            _logger.Warn(message);
        }

        public void LogError(string message, Exception ex = null)
        {
            if (ex == null)
            {
                _logger.Error(message);
            }
            else
            {
                _logger.Error(message, ex);
            }
        }

        public void LogFatal(string message, Exception ex = null)
        {
            if (ex == null)
            {
                _logger.Fatal(message);
            }
            else
            {
                _logger.Fatal(message, ex);
            }
        }
    }
}
