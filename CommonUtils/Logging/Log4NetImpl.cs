using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace CommonUtils.Logging
{
    public class Log4NetImpl : ILogger
    {
        private ILog logger;

        public Log4NetImpl(Type logname)
        {
            logger = log4net.LogManager.GetLogger(logname);
        }

        public void Trace(string message)
        {
            logger.Debug(message);
        }

        public void Debug(string message)
        {
            logger.Debug(message);
        }

        public void Warn(string message)
        {
            logger.Warn(message);
        }

        public void Info(string message)
        {
            logger.Info(message);
        }

        public void Error(string message)
        {
            logger.Error(message);
        }

        public void Fatal(string message)
        {
            logger.Fatal(message);
        }

        public bool IsDebug()
        {
            return logger.IsDebugEnabled;
        }



        public void Trace(string message, Exception innerException)
        {
            logger.Debug(message + " Exception: " + innerException.Message + " - StackTrace: " + innerException.StackTrace);
        }

        public void Debug(string message, Exception innerException)
        {
            logger.Debug(message + " Exception: " + innerException.Message + " - StackTrace: " + innerException.StackTrace);
        }

        public void Warn(string message, Exception innerException)
        {
            logger.Warn(message + " Exception: " + innerException.Message + " - StackTrace: " + innerException.StackTrace);
        }

        public void Info(string message, Exception innerException)
        {
            logger.Info(message + " Exception: " + innerException.Message + " - StackTrace: " + innerException.StackTrace);
        }

        public void Error(string message, Exception innerException)
        {
            logger.Error(message + " Exception: " + innerException.Message + " - StackTrace: " + innerException.StackTrace);
        }

        public void Fatal(string message, Exception innerException)
        {
            logger.Fatal(message + " Exception: " + innerException.Message + " - StackTrace: " + innerException.StackTrace);
        }
    }
}
