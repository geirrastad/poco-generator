using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonUtils.Logging;
using System.IO;

namespace CommonUtils.Config
{
    public class XmlConfigFile : ConfigImpl
    {
        String _configFile = "config.xml";
        ILogger _log = LoggerFactory.GetLogger(typeof(XmlConfigFile));
        //bool _configFileOK = false;

        public static XmlConfigFile GetXmlConfig(String XMLFile) {
            XmlConfigFile xmlConfig = new XmlConfigFile(XMLFile);
            return xmlConfig;
        }

        private XmlConfigFile() { }

        private XmlConfigFile(String file)
        {
              LoadConfig(file);  
        }

        private FileInfo FindConfigFile(String file)
        {
            if (file != null)
                _configFile = file;

            FileInfo fi = new FileInfo(_configFile);
            if (!fi.Exists)
            {
                return null;
            } 
            return fi;
        }

        protected override bool LoadConfig(string xml)
        {
            FileInfo fi = FindConfigFile(xml);
            if (fi == null)
            {
                //_log.Error("Config file not found");
                throw new System.IO.IOException("Config file not found: " + xml);
            }

            try
            {
                _doc = new System.Xml.XmlDocument();
                _doc.Load(fi.FullName);
                SetXpathRoot( "/" + _doc.DocumentElement.Name + "/" );
            }
            catch (Exception e)
            {
                _log.Error("Cannot load XML: " + e.ToString());
                return false;
            }

            return true;
        }



    }
}
