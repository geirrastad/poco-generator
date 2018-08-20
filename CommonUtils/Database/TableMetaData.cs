using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace CommonUtils.Database
{
    public class TableMetaData
    {
        private String _name;

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private bool _isPrimary;

        public bool IsPrimary
        {
            get { return _isPrimary; }
            set { _isPrimary = value; }
        }
        private bool _isKey;

        public bool IsKey
        {
            get { return _isKey; }
            set { _isKey = value; }
        }
        private String _type;

        public String Type
        {
            get { return _type; }
            set { _type = value; }
        }
        private int _maxLen;

        public int MaxLen
        {
            get { return _maxLen; }
            set { _maxLen = value; }
        }

        private bool _isNullable = true;

        public bool IsNullable
        {
            get { return _isNullable; }
            set { _isNullable = value; }
        }
    }
}
