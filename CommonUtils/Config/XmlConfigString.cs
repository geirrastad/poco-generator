using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtils.Config
{
    public class XmlConfigString : ConfigImpl
    {
        public static XmlConfigString GetXmlConfig(String xml)
        {
            XmlConfigString xmlConfig = new XmlConfigString(xml);
            return xmlConfig;
        }

        public XmlConfigString(String xml)
        {
            LoadConfig(xml);
        }
    }
}
