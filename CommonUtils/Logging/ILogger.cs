using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtils.Logging
{
    public interface ILogger
    {
        void Trace(String message);
        void Debug(String message);
        void Warn(String message);
        void Info(String message);
        void Error(String message);
        void Fatal(String message);
        void Trace(String message, Exception innerException);
        void Debug(String message, Exception innerException);
        void Warn(String message, Exception innerException);
        void Info(String message, Exception innerException);
        void Error(String message, Exception innerException);
        void Fatal(String message, Exception innerException);
        bool IsDebug();
    }
}
