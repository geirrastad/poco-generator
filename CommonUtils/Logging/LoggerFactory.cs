using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.IO;
using System.Xml;
using System.Reflection;

namespace CommonUtils.Logging
{
    public class LoggerFactory
    {
        private static bool useLog4Net = false;
        private static bool initialized = false;

        public static void InitEventLogger(int level, String section, String key)
        {
            InternalLogManager.InitializeEventLogger(level, section, key);
            initialized = true;
        }

        public static void InitLogger()
        {
            InitLogger("InternalLogger.xml");
        }

        public static void InitLogger(String confFile)
        {
            if (!File.Exists(confFile))
            {
                InternalLogManager.InitializeConsoleLogger(LogLevel.LOG_LEVEL_INFO);
                initialized = true;
                useLog4Net = false;
                return;
            }
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(confFile);
                XmlNode n = doc.SelectSingleNode("log4net");
                doc = null;
                if (n == null)
                {
                    InternalLogManager.InitializeLogger(confFile);
                    useLog4Net = false;
                }
                else
                {
                    // Config log4net

                    log4net.Config.XmlConfigurator.Configure(LogManager.GetRepository(Assembly.GetEntryAssembly()), new FileInfo(confFile));
                    useLog4Net = true;
                }
                initialized = true;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Cannot configure logger: " + e.Message);
                initialized = false;
            }
        }


        public static ILogger GetLogger(Type logName)
        {
            if (!initialized)
                return null;

            if (useLog4Net)
                return new Log4NetImpl(logName);
            else
                return new InternalLoggerImpl(logName);
        }

        
    }
}
