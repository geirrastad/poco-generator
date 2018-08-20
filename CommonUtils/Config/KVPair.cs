using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtils.Config
{
    public class KVPair
    {
        String _key;

        public String Key
        {
            get { return _key; }
            set { _key = value; }
        }
        String _value;

        public String Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public KVPair() { }

        public KVPair(String key, String value)
        {
            _key = key;
            _value = value; 
        }

        public static String GetValue(KVPair[] kvp, String key)
        {
            try
            {
                if (kvp == null || kvp.Length == 0) return null;
                foreach (KVPair k in kvp)
                    if (k.Key.Equals(key))
                        return k.Value;
            }
            catch (Exception)
            {
            }
            return null;
        }

        public static KVPair[] ParseList(String[] commandLineArgs)
        {
            List<KVPair> array = new List<KVPair>();
            if (commandLineArgs == null || commandLineArgs.Length == 0) return null;

            foreach (String s in commandLineArgs)
            {
                String[] pair = s.Replace("-", "").Split(new char[] { '=' });
                if (pair.Length == 2)
                {
                    KVPair k = new KVPair(pair[0], pair[1]);
                    array.Add(k);
                }
            }

            return array.ToArray();
        }

    }
}
