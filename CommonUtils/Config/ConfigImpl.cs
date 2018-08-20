
// Type: CommonUtils.Config.ConfigImpl
// Assembly: CommonUtils, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 633D58C8-273F-4F1E-A0EB-8F8C30A8EDB9
// Assembly location: C:\Users\geir\Desktop\POCO\CommonUtils.dll

using CommonUtils.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace CommonUtils.Config
{
    public abstract class ConfigImpl
    {
        private ILogger _log = LoggerFactory.GetLogger(typeof(ConfigImpl));
        private bool inited = false;
        private string _xpathRoot = "/";
        protected XmlDocument _doc;

        public ConfigImpl SpawnConfigFromHere(string newRootNode)
        {
            try
            {
                return ConfigFactory.GetConfigFromString(this._doc.SelectNodes((this._xpathRoot + "/" + newRootNode).Replace("//", "/"))[0].OuterXml);
            }
            catch (Exception ex)
            {
                return (ConfigImpl)null;
            }
        }

        public string GetXML()
        {
            MemoryStream memoryStream = new MemoryStream();
            this._doc.Save((Stream)memoryStream);
            memoryStream.Flush();
            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }

        public void Save(string path)
        {
            if (this._doc == null)
                return;
            this._doc.Save(path);
        }

        private string GetEncoding(string xmlSource)
        {
            if (xmlSource == null || xmlSource.Length < 5)
                return (string)null;
            string[] strArray1 = xmlSource.Split(new char[1]
            {
        '?'
            }, StringSplitOptions.None);
            if (strArray1 == null || strArray1.Length < 3)
                return (string)null;
            string str1 = strArray1[1];
            char[] separator = new char[1] { ' ' };
            int num = 0;
            foreach (string str2 in str1.Split(separator, (StringSplitOptions)num))
            {
                if (str2.ToLower().IndexOf("encoding=") >= 0)
                {
                    string[] strArray2 = str2.Split(new char[1] { '=' }, StringSplitOptions.None);
                    if (strArray2 != null && strArray2.Length == 2)
                        return strArray2[1].Replace("'", "").Replace("\"", "");
                }
            }
            return (string)null;
        }

        protected virtual bool LoadConfig(string xml)
        {
            try
            {
                this._doc = new XmlDocument();
                string name = this.GetEncoding(xml) ?? "UTF-8";
                Encoding encoding = Encoding.GetEncoding(name);
                if (encoding == null)
                {
                    this._log.Error("Cannot load XML encoder: " + name);
                    this.inited = false;
                    return false;
                }
                MemoryStream memoryStream = new MemoryStream(encoding.GetBytes(xml));
                memoryStream.Flush();
                memoryStream.Position = 0L;
                this._doc.Load((Stream)memoryStream);
                this._xpathRoot = "/" + this._doc.DocumentElement.Name + "/";
                this.inited = true;
            }
            catch (Exception ex)
            {
                this._log.Error("Cannot parse XML: " + ex.ToString());
                this.inited = false;
            }
            return this.inited;
        }

        public void SetXpathRoot(string xpath)
        {
            this._xpathRoot = xpath;
        }

        public virtual KVPair[] GetConfigKVPair(string section)
        {
            return this.GetConfigKVPair(section, false);
        }

        public virtual KVPair[] GetConfigKVPair(string section, bool useNameAttribute)
        {
            List<KVPair> kvPairList = new List<KVPair>();
            XmlNodeList xmlNodeList = this._doc.SelectNodes((this._xpathRoot + section).Replace("//", "/") + "/*");
            if (xmlNodeList == null || xmlNodeList.Count == 0)
                return new KVPair[0];
            foreach (XmlNode xmlNode in xmlNodeList)
                kvPairList.Add(new KVPair()
                {
                    Key = !useNameAttribute ? xmlNode.Name : xmlNode.Attributes["name"].Value,
                    Value = xmlNode.InnerText
                });
            return kvPairList.ToArray();
        }

        public virtual string GetConfigString(string setting, string attribute)
        {
            try
            {
                string xpath = (this._xpathRoot + setting).Replace("//", "/").Replace("/%ROOT%", "");
                if (this._doc == null)
                    return (string)null;
                XmlNode xmlNode = this._doc.SelectSingleNode(xpath);
                if (xmlNode == null)
                    return (string)null;
                return attribute == null ? xmlNode.InnerText : xmlNode.Attributes[attribute].Value;
            }
            catch (Exception ex)
            {
                return (string)null;
            }
        }

        public virtual int GetConfigInt(string setting, string attribute)
        {
            string configString = this.GetConfigString(setting, attribute);
            if (configString == null)
                return 0;
            try
            {
                return int.Parse(configString);
            }
            catch (Exception ex)
            {
                this._log.Debug("Wrong format");
                return 0;
            }
        }

        public virtual double GetConfigDouble(string setting, string attribute)
        {
            string configString = this.GetConfigString(setting, attribute);
            if (configString == null)
                return 0.0;
            try
            {
                return double.Parse(configString);
            }
            catch (Exception ex)
            {
                this._log.Debug("Wrong format");
                return 0.0;
            }
        }

        public virtual DateTime GetConfigDateTime(string setting, string attribute)
        {
            string configString = this.GetConfigString(setting, attribute);
            if (configString == null)
                return new DateTime();
            try
            {
                return DateTime.Parse(configString);
            }
            catch (Exception ex)
            {
                this._log.Debug("Wrong format");
                return new DateTime();
            }
        }

        public virtual bool GetConfigBool(string setting, string attribute)
        {
            string configString = this.GetConfigString(setting, attribute);
            return configString != null && !configString.Trim().Equals("0") && !configString.Trim().ToUpper().Equals("FALSE") && (configString.Trim().Equals("1") || configString.Trim().Equals("-1") || configString.Trim().ToUpper().Equals("TRUE"));
        }

        public virtual bool GetConfigBool(string setting)
        {
            return this.GetConfigBool(setting, (string)null);
        }

        public virtual string GetConfigString(string setting)
        {
            return this.GetConfigString(setting, (string)null);
        }

        public virtual DateTime GetConfigDateTime(string setting)
        {
            return this.GetConfigDateTime(setting, (string)null);
        }

        public virtual double GetConfigDouble(string setting)
        {
            return this.GetConfigDouble(setting, (string)null);
        }

        public virtual int GetConfigInt(string setting)
        {
            return this.GetConfigInt(setting, (string)null);
        }

        public virtual void SetConfigString(string configPath, string value)
        {
            string str = (this._xpathRoot + configPath).Replace("//", "/");
            if (this.Exists(str))
            {
                this._doc.SelectSingleNode(str).InnerText = value;
            }
            else
            {
                string[] strArray = str.Split(new char[1] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                string path = "";
                string xpath = "";
                int index;
                for (index = 0; index < strArray.Length - 1; ++index)
                {
                    path = path + "/" + strArray[index];
                    if (!this.Exists(path))
                    {
                        XmlNode element = (XmlNode)this._doc.CreateElement(strArray[index]);
                        this._doc.SelectSingleNode(xpath).AppendChild(element);
                    }
                    xpath = path;
                }
                if (!this.Exists(path + "/" + strArray[index]))
                {
                    XmlNode element = (XmlNode)this._doc.CreateElement(strArray[index]);
                    if (value != null && value.Trim().Length > 0)
                        element.InnerText = value.Trim();
                    this._doc.SelectSingleNode(xpath).AppendChild(element);
                }
            }
        }

        public bool Exists(string path)
        {
            if (this._doc == null)
                return false;
            if (path.StartsWith("/"))
                return this._doc.SelectSingleNode(path) != null;
            return this._doc.SelectSingleNode(this._xpathRoot + "/" + path) != null;
        }
    }
}
