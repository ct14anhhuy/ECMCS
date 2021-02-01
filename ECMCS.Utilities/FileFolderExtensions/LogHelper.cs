using log4net;
using System;

namespace ECMCS.Utilities.FileFolderExtensions
{
    public class LogHelper
    {
        private static readonly LogHelper _instance = new LogHelper();
        private readonly ILog monitoringLogger;
        private static ILog debugLogger;

        private LogHelper()
        {
            monitoringLogger = LogManager.GetLogger("MonitoringLogger");
            debugLogger = LogManager.GetLogger("DebugLogger");
        }

        public static void Debug(string message)
        {
            debugLogger.Debug(message);
        }

        public static void Debug(string message, Exception exception)
        {
            debugLogger.Debug(message, exception);
        }

        public static void Info(string message)
        {
            _instance.monitoringLogger.Info(message);
        }

        public static void Info(string message, Exception exception)
        {
            _instance.monitoringLogger.Info(message, exception);
        }

        public static void Warn(string message)
        {
            _instance.monitoringLogger.Warn(message);
        }

        public static void Warn(string message, Exception exception)
        {
            _instance.monitoringLogger.Warn(message, exception);
        }

        public static void Error(string message)
        {
            _instance.monitoringLogger.Error(message);
        }

        public static void Error(string message, Exception exception)
        {
            _instance.monitoringLogger.Error(message, exception);
        }

        public static void Fatal(string message)
        {
            _instance.monitoringLogger.Fatal(message);
        }

        public static void Fatal(string message, Exception exception)
        {
            _instance.monitoringLogger.Fatal(message, exception);
        }
    }
}