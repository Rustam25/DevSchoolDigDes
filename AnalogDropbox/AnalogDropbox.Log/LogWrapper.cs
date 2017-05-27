using NLog;
using System;
using System.Reflection;
using System.Diagnostics;

namespace AnalogDropbox.Log
{
    public class LogWrapper : IDisposable
    {
        NLog.Logger _logger;
        MethodBase methodBase;

        public LogWrapper()
        {
            _logger = LogManager.GetCurrentClassLogger();
            methodBase = new StackTrace().GetFrame(1).GetMethod();
            _logger.Info($"Start of method {methodBase.ReflectedType.Name}.{methodBase.Name}");
        }

        public void Dispose()
        {
            _logger.Info($"Finish of method {methodBase.ReflectedType.Name}.{methodBase.Name}");
        }

        public void Trace(string message)
        {
            _logger.Trace(message);
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Warn(string message)
        {
            _logger.Warn(message);
        }

        public void Error(string exeptionMessage)
        {
            _logger.Error($"Method {methodBase.ReflectedType.Name}.{methodBase.Name}|Error message: {exeptionMessage}");
        }

        public void Fatal(string exeptionMessage)
        {
            _logger.Fatal($"Method {methodBase.ReflectedType.Name}.{methodBase.Name}|Error message: {exeptionMessage}");
        }
    }
}
