using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Mail;
using System.Xml;
using System.Diagnostics;

namespace CommonUtils.Logging
{
    public class InternalLogManager
    {
        private const int LOG_STDOUT         = 1;
        private const int LOG_EVENT_LOGGER   = 2;
        private const int LOG_EMAIL          = 4;
        private const int LOG_FILE           = 8;

        private static int currentLogLevel   = LogLevel.LOG_LEVEL_INFO;
        private static int logLevelConsole = LogLevel.LOG_LEVEL_INHERIT;
        private static int logLevelFile = LogLevel.LOG_LEVEL_INHERIT;
        private static int logLevelMail = LogLevel.LOG_LEVEL_INHERIT;
        private static int logLevelEvent = LogLevel.LOG_LEVEL_INHERIT;
        private static int logMask = LOG_STDOUT;


        private static String logFile;
        private static String mailHost;
        private static int mailPort;
        private static String mailFromAddress;
        private static String mailFromDisplayName;
        private static String mailToAddress;
        private static String mailToDisplayName;
        private static String mailSubject;
        private static String eventKey;
        private static String eventSection;

        private static String logFormat      = "%TS% [%LEVEL%] - %NAME%: %MESSAGE%%NL%";

        private static object lockObject = new object();

        public static void InitializeConsoleLogger(int logLevel)
        {
            logMask = LOG_STDOUT;
            logLevelConsole = logLevel;
            currentLogLevel = logLevel;
        }

        public static void InitializeEventLogger(int logLevel, String section, String key)
        {
            currentLogLevel = logLevel;
            
            logMask = LOG_EVENT_LOGGER;
            eventKey = key;
            eventSection = section;
        }

        public static void InitializeLogger(String confFile)
        {
            FileInfo fi = new FileInfo(confFile);
            if (!fi.Exists)
            {
                return;
            }

            XmlDocument conf = new XmlDocument();
            conf.Load(confFile);

            XmlNode node = conf.SelectSingleNode("/logger/email");
            if (node != null)
            {
                mailHost = conf.SelectSingleNode("/logger/email/host").InnerText;
                mailPort = int.Parse(conf.SelectSingleNode("/logger/email/port").InnerText);
                mailFromAddress = conf.SelectSingleNode("/logger/email/from_address").InnerText;
                mailToAddress = conf.SelectSingleNode("/logger/email/to_address").InnerText;
                mailFromDisplayName = conf.SelectSingleNode("/logger/email/from_displayname").InnerText;
                mailToDisplayName = conf.SelectSingleNode("/logger/email/to_displayname").InnerText;
                mailSubject = conf.SelectSingleNode("/logger/email/subject").InnerText;
            }

            node = conf.SelectSingleNode("/logger/file");
            if (node != null)
            {
                logFile = conf.SelectSingleNode("/logger/file/name").InnerText.Replace("%APP_DIR%", AppDomain.CurrentDomain.BaseDirectory);
            }


            node = conf.SelectSingleNode("/logger/event");
            if (node != null)
            {
                eventKey = conf.SelectSingleNode("/logger/event/key").InnerText;
                eventSection = conf.SelectSingleNode("/logger/event/section").InnerText;
            }

            logMask = 0;

            logMask += (conf.SelectSingleNode("/logger/appenders/console") != null) ? LOG_STDOUT : 0;
            logMask += (conf.SelectSingleNode("/logger/appenders/event") != null) ? LOG_EVENT_LOGGER : 0;
            logMask += (conf.SelectSingleNode("/logger/appenders/email") != null) ? LOG_EMAIL : 0;
            logMask += (conf.SelectSingleNode("/logger/appenders/file") != null) ? LOG_FILE : 0;

            logFormat = conf.SelectSingleNode("/logger/appenders").Attributes["format"].InnerText;
            String level = conf.SelectSingleNode("/logger/appenders").Attributes["level"].InnerText.ToUpper();
            currentLogLevel = MapLogLevel(level);

            node = conf.SelectSingleNode("/logger/email/threshold");
            if (node != null)
                logLevelMail = MapLogLevel(node.InnerText.ToUpper());

            node = conf.SelectSingleNode("/logger/event/threshold");
            if (node != null)
                logLevelEvent = MapLogLevel(node.InnerText.ToUpper());

            node = conf.SelectSingleNode("/logger/console/threshold");
            if (node != null)
                logLevelConsole = MapLogLevel(node.InnerText.ToUpper());

            node = conf.SelectSingleNode("/logger/file/threshold");
            if (node != null)
                logLevelFile = MapLogLevel(node.InnerText.ToUpper());

            if ((logMask & LOG_EVENT_LOGGER) != 0)
            {
                if (!EventLog.SourceExists(eventSection))
                    EventLog.CreateEventSource(eventSection, eventKey);
            }
        }

        private static int MapLogLevel(String level)
        {
            switch (level)
            {
                case "DEBUG":
                    return LogLevel.LOG_LEVEL_DEBUG;
                case "INFO":
                    return LogLevel.LOG_LEVEL_INFO;
                case "WARN":
                    return LogLevel.LOG_LEVEL_WARN;
                case "ERROR":
                    return LogLevel.LOG_LEVEL_ERROR;
                case "CRITICAL":
                    return LogLevel.LOG_LEVEL_CRITICAL;
            }

            return LogLevel.LOG_LEVEL_INHERIT;
        }

        public static bool IsDebug()
        {
            return (currentLogLevel >= LogLevel.LOG_LEVEL_DEBUG) ? true : false;
        }

        public static void Log(int level, String name, String message)
        {
            // Generic threshold - ovverrides all appender speciffics
            if (level > currentLogLevel) return;
            DateTime ts = DateTime.Now;
            lock (lockObject)
            {
                if ((logMask & LOG_STDOUT) != 0)
                    LogStdOut(level, name, message, ts);
                if ((logMask & LOG_FILE) != 0)
                    LogFile(level, name, message, ts);
                if ((logMask & LOG_EMAIL) != 0)
                    LogMail(level, name, message, ts);
                if ((logMask & LOG_EVENT_LOGGER) != 0)
                    LogEvent(level, name, message, ts);
            }
        }

        private static String ParseLogString(int level, String name, String message, DateTime ts)
        {
            String result = logFormat.Replace("%LEVEL%", GetLevel(level));
            result = result.Replace("%NAME%", name);
            result = result.Replace("%MESSAGE%", message);
            result = result.Replace("%TS%", ts.ToShortDateString() + " " + ts.ToLongTimeString());
            result = result.Replace("%NL%", Environment.NewLine);
            return result;
        }

        private static String GetLevel(int level)
        {
            switch (level)
            {
                case LogLevel.LOG_LEVEL_CRITICAL:
                    return "CRITICAL";
                case LogLevel.LOG_LEVEL_DEBUG:
                    return "DEBUG";
                case LogLevel.LOG_LEVEL_ERROR:
                    return "ERROR";
                case LogLevel.LOG_LEVEL_INFO:
                    return "INFO";
                case LogLevel.LOG_LEVEL_WARN:
                    return "WARN";
            }
            return "UNKNOWN";
        }

        private static void LogStdOut(int level, String name, String message, DateTime ts)
        {
            if (level > logLevelConsole) return;
            String logString = ParseLogString(level, name, message, ts);
            if (level <= LogLevel.LOG_LEVEL_ERROR)
                Console.Error.Write(logString);
            else
                Console.Write(logString);
        }

        private static void LogEvent(int level, String name, String message, DateTime ts)
        {
            if (level > logLevelEvent) return;
            String logString = ParseLogString(level, name, message, ts);
            EventLogEntryType et = EventLogEntryType.Information;
            switch (level)
            {
                case LogLevel.LOG_LEVEL_CRITICAL:
                case LogLevel.LOG_LEVEL_ERROR:
                    et = EventLogEntryType.Error;
                    break;
                case LogLevel.LOG_LEVEL_WARN:
                    et = EventLogEntryType.Warning;
                    break;
            }

            EventLog.WriteEntry(eventKey, logString, et); 
        }

        private static void LogFile(int level, String name, String message, DateTime ts)
        {
            if (level > logLevelFile) return;
            String logString = ParseLogString(level, name, message, ts);
            //FileInfo fi = new FileInfo(logFile);
            try
            {
                File.AppendAllText(logFile, logString);
            }
            catch (Exception) { }
        }

        private static void LogMail(int level, String name, String message, DateTime ts)
        {
            if (level > logLevelMail) return;
            try
            {
                SmtpClient smtp = new SmtpClient(mailHost, mailPort);
                MailAddress from = new MailAddress(mailFromAddress, mailFromDisplayName);
                MailAddress to = new MailAddress(mailToAddress, mailToDisplayName);
                MailMessage mm = new MailMessage(from, to);
                mm.Subject = GetLevel(level) + ": " + mailSubject;
                mm.Body = ParseLogString(level, name, message, ts);
                smtp.Send(mm);
            }
            catch (Exception)
            {
                // Don't log ...
            }
        }



    }
}
