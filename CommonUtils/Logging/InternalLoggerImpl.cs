using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtils.Logging
{
    public class InternalLoggerImpl : ILogger
    {
        private static InternalLogManager logManager = new InternalLogManager();
        private static String name;

        public InternalLoggerImpl(Type logname)
        {
            name = logname.Name;
        }

        public void Trace(string message)
        {
            InternalLogManager.Log(LogLevel.LOG_LEVEL_DEBUG, name, message);
        }

        public void Debug(string message)
        {
            InternalLogManager.Log(LogLevel.LOG_LEVEL_DEBUG, name, message);
        }

        public void Warn(string message)
        {
            InternalLogManager.Log(LogLevel.LOG_LEVEL_WARN, name, message);
        }

        public void Info(string message)
        {
            InternalLogManager.Log(LogLevel.LOG_LEVEL_INFO, name, message);
        }

        public void Error(string message)
        {
            InternalLogManager.Log(LogLevel.LOG_LEVEL_ERROR, name, message);
        }

        public void Fatal(string message)
        {
            InternalLogManager.Log(LogLevel.LOG_LEVEL_CRITICAL, name, message);
        }

        public bool IsDebug()
        {
            return InternalLogManager.IsDebug();
        }



        public void Trace(string message, Exception innerException)
        {
            InternalLogManager.Log(LogLevel.LOG_LEVEL_DEBUG, name, message + " Exception: " + innerException.Message + " - StackTrace: " + innerException.StackTrace);
        }

        public void Debug(string message, Exception innerException)
        {
            InternalLogManager.Log(LogLevel.LOG_LEVEL_DEBUG, name, message + " Exception: " + innerException.Message + " - StackTrace: " + innerException.StackTrace);
        }

        public void Warn(string message, Exception innerException)
        {
            InternalLogManager.Log(LogLevel.LOG_LEVEL_WARN, name, message + " Exception: " + innerException.Message + " - StackTrace: " + innerException.StackTrace);
        }

        public void Info(string message, Exception innerException)
        {
            InternalLogManager.Log(LogLevel.LOG_LEVEL_INFO, name, message + " Exception: " + innerException.Message + " - StackTrace: " + innerException.StackTrace);
        }

        public void Error(string message, Exception innerException)
        {
            InternalLogManager.Log(LogLevel.LOG_LEVEL_ERROR, name, message + " Exception: " + innerException.Message + " - StackTrace: " + innerException.StackTrace);
        }

        public void Fatal(string message, Exception innerException)
        {
            InternalLogManager.Log(LogLevel.LOG_LEVEL_CRITICAL, name, message + " Exception: " + innerException.Message + " - StackTrace: " + innerException.StackTrace);
        }
    }
}
