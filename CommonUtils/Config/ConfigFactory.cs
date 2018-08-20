using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtils.Config
{
    public class ConfigFactory
    {
        public static ConfigImpl GetConfigFromFile(String file)
        {
            return XmlConfigFile.GetXmlConfig(file);
        }

        public static ConfigImpl GetConfigFromString(String xml)
        {
            return XmlConfigString.GetXmlConfig(xml);
        }

        public static ConfigImpl GetConfigFromByteArray(byte[] xml)
        {
            return XmlConfigString.GetXmlConfig( CommonUtils.Strings.Format.ToString(xml) );
        }

    }
}
