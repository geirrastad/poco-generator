using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Text;
using System.Collections.Generic;
using System.Xml;
using CommonUtils.Logging;
using System.Data.Common;

namespace CommonUtils.Database
{
    public class DBMetaInfo
    {
        private static ILogger log = LoggerFactory.GetLogger(typeof(DBMetaInfo));

        public static String getTableDefAsXML(String conStr, String table)
        {
            return getTableDefAsXML(conStr, table, Environment.NewLine);
        }

        public static String getTableDefAsXML(String conStr, String table, String newline)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version='1.0' encoding='ISO-8859-1' ?>" + newline);
            TableMetaData[] metas = getTableData(conStr, table);
            if (metas == null)
            {
                sb.Append("<error>Feil under generering av fil</error>");
                return sb.ToString();
            }

            sb.Append("<" + table.Trim() + ">" + newline);
            sb.Append("\t<data>" + newline);

            for (int i = 0; i < metas.Length; i++)
            {
                sb.Append("\t\t<" + metas[i].Name + " datatype='" + metas[i].Type + "' maxlen='" + metas[i].MaxLen + "'>feltverdi</" + metas[i].Name + ">" + newline);
            }

            sb.Append("\t</data>" + newline);
            sb.Append("</" + table.Trim() + ">" + newline);
            return sb.ToString();
        }

        public static TableMetaData[] getTableData(String connStr, String table)
        {
            List<TableMetaData> td = new List<TableMetaData>();
            
            DbConnection c = null;
            DbCommand cmd = null;
            DbDataReader r = null;
            try
            {
                c = DBConnectionFactory.GetSqlConnection(connStr);
                cmd = DBConnectionFactory.GetSqlCommand("desc " + table, c);
                r = cmd.ExecuteReader();
                while( r.Read() )
                {
                    TableMetaData md = new TableMetaData();
                    md.Name = DBTools.GetDBStringValue(r, "Field");
                    String[] dataType = DBTools.GetDBStringValue(r, "Type").Split(new char[] { '(', ')' });
                    if (dataType.Length > 1)
                        md.MaxLen = int.Parse(dataType[1]);
                    else
                        md.MaxLen = 0;
                    md.Type = dataType[0];

                    md.IsNullable = DBTools.GetDBStringValue(r, "Null").Equals("YES") ? true : false;
                    md.IsKey = DBTools.GetDBStringValue(r, "Key").Equals("") ? false : true;
                    if( md.IsKey ) {
                        md.IsPrimary = DBTools.GetDBStringValue(r, "Key").Equals("PRI") ? true : false;
                    }
                    td.Add( md );
                }

                return td.ToArray();
            }
            catch (Exception e)
            {
                log.Error("GetTableData feilet", e);
                return null;
            }
            finally
            {
                if( r != null ) r.Close();
                if( cmd != null ) cmd.Dispose();
                if (c != null) c.Close();
            }
        }


        public static int loadData(String conStr, String data, String table, String format)
        {
            DbConnection con = DBConnectionFactory.GetSqlConnection(conStr);
            if (con == null) return 0;
            DbTransaction trans = con.BeginTransaction();

            TableMetaData[] meta = getTableData(conStr, table);
            if (meta == null)
                return 0;

            if (format.ToLower().Equals("xml"))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(data);

                XmlNodeList nodes = doc.SelectNodes(table.Trim() + "/data");

                if (nodes.Count == 0) return 0;
                // Build the prepared statement
                String sql = "replace into " + table + " set ";

                XmlNode node = nodes.Item(0);
                XmlNodeList nl = node.ChildNodes;

                System.Collections.IEnumerator en = nl.GetEnumerator();
                while (en.MoveNext())
                {
                    node = (XmlNode)en.Current;
                    sql += node.Name + " = @" + node.Name.Trim() + ",";
                }
                sql = sql.TrimEnd(',');

                try
                {

                    for (int n = 0; n < nodes.Count; n++)
                    {
                        // Load the data
                        DbCommand cmd = DBConnectionFactory.GetSqlCommand(sql, con);
                        XmlNode xmlnode = nodes.Item(n);
                        XmlNodeList param = xmlnode.ChildNodes;
                        for (int p = 0; p < param.Count; p++)
                        {
                            xmlnode = param.Item(p);
                            cmd.Parameters. Add("@" + xmlnode.Name.Trim());
                            cmd.Parameters["@" + xmlnode.Name.Trim()].Value = xmlnode.InnerText;
                        }
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }

                    trans.Commit();
                    return nodes.Count;
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    throw e;
                }

            }
            return 0;
        }
    }
}
