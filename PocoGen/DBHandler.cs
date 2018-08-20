
// Type: POCOGen.DBHandler
// Assembly: POCOGen, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7E14F844-B8BF-4B4C-8F4A-E6B2FD050651
// Assembly location: C:\Users\geir\Desktop\POCO\POCOGen.exe

using CommonUtils.Database;
using POCOGen.DB;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;

namespace POCOGen
{
  public class DBHandler
  {
    private static string NL = Environment.NewLine;
    private static string NL2 = Environment.NewLine + Environment.NewLine;
    private static string _DB_SINGLE_READER_TEMPLATE = "\t\t\tDbConnection conn = null;" + DBHandler.NL + "\t\t\tDbCommand cmd = null;" + DBHandler.NL + "\t\t\tDbDataReader dr = null;" + DBHandler.NL2 + "\t\t\ttry" + DBHandler.NL + "\t\t\t{" + DBHandler.NL + "\t\t\t\tconn = DBConnectionFactory.Get%%DB_DRIVER%%Connection(CONNECTION_STRING);" + DBHandler.NL + "\t\t\t\tcmd = DBConnectionFactory.Get%%DB_DRIVER%%Command(%%QUERY%%, conn);" + DBHandler.NL + "%%SQL_PARAMS%%\t\t\t\tdr = cmd.ExecuteReader();" + DBHandler.NL2 + "\t\t\t\t%%VO_TYPE%% item = null;" + DBHandler.NL2 + "\t\t\t\tif( dr.Read() ) {" + DBHandler.NL + "\t\t\t\t\t%%CONTENT%%" + DBHandler.NL + "\t\t\t\t}" + DBHandler.NL2 + "\t\t\t\treturn item;" + DBHandler.NL + "\t\t\t}" + DBHandler.NL + "\t\t\tcatch( Exception exp ) {" + DBHandler.NL + "\t\t\t\tthrow new DBException(exp.Message, exp);" + DBHandler.NL + "\t\t\t}" + DBHandler.NL + "\t\t\tfinally {" + DBHandler.NL + "\t\t\t\tif( dr != null ) dr.Close();" + DBHandler.NL + "\t\t\t\tif( cmd != null ) cmd = null;" + DBHandler.NL + "\t\t\t\tif( conn != null ) conn.Close();" + DBHandler.NL + "\t\t\t}" + DBHandler.NL;
    private static string _DB_READER_TEMPLATE = "\t\t\tDbConnection conn = null;" + DBHandler.NL + "\t\t\tDbCommand cmd = null;" + DBHandler.NL + "\t\t\tDbDataReader dr = null;" + DBHandler.NL2 + "\t\t\ttry" + DBHandler.NL + "\t\t\t{" + DBHandler.NL + "\t\t\t\tconn = DBConnectionFactory.Get%%DB_DRIVER%%Connection(CONNECTION_STRING);" + DBHandler.NL + "\t\t\t\tcmd = DBConnectionFactory.Get%%DB_DRIVER%%Command(%%QUERY%%, conn);" + DBHandler.NL + "%%SQL_PARAMS%%\t\t\t\tdr = cmd.ExecuteReader();" + DBHandler.NL2 + "\t\t\t\tList<%%VO_TYPE%%> items = new List<%%VO_TYPE%%>();" + DBHandler.NL2 + "\t\t\t\twhile( dr.Read() ) {" + DBHandler.NL + "\t\t\t\t\t%%CONTENT%%" + DBHandler.NL + "\t\t\t\t}" + DBHandler.NL2 + "\t\t\t\treturn items.ToArray();" + DBHandler.NL + "\t\t\t}" + DBHandler.NL + "\t\t\tcatch( Exception exp ) {" + DBHandler.NL + "\t\t\t\tthrow new DBException(exp.Message, exp);" + DBHandler.NL + "\t\t\t}" + DBHandler.NL + "\t\t\tfinally {" + DBHandler.NL + "\t\t\t\tif( dr != null ) dr.Close();" + DBHandler.NL + "\t\t\t\tif( cmd != null ) cmd = null;" + DBHandler.NL + "\t\t\t\tif( conn != null ) conn.Close();" + DBHandler.NL + "\t\t\t}" + DBHandler.NL;
    private static string _DB_UPDATE_TEMPLATE = "\t\t\tDbConnection conn = null;" + DBHandler.NL + "\t\t\tDbCommand cmd = null;" + DBHandler.NL2 + "\t\t\ttry" + DBHandler.NL + "\t\t\t{" + DBHandler.NL + "\t\t\t\tconn = DBConnectionFactory.Get%%DB_DRIVER%%Connection(CONNECTION_STRING);" + DBHandler.NL + "\t\t\t\tcmd = DBConnectionFactory.Get%%DB_DRIVER%%Command(%%QUERY%%, conn);" + DBHandler.NL2 + "\t\t\t\t%%SQL_PARAMS%%" + DBHandler.NL2 + "\t\t\t\tcmd.ExecuteNonQuery();" + DBHandler.NL + "\t\t\t}" + DBHandler.NL + "\t\t\tcatch( Exception exp ) {" + DBHandler.NL + "\t\t\t\tthrow new DBException(exp.Message, exp);" + DBHandler.NL + "\t\t\t}" + DBHandler.NL + "\t\t\tfinally {" + DBHandler.NL + "\t\t\t\tif( cmd != null ) cmd = null;" + DBHandler.NL + "\t\t\t\tif( conn != null ) conn.Close();" + DBHandler.NL + "\t\t\t}" + DBHandler.NL;
    private static string DB_READER_TEMPLATE = (string) null;
    private static string DB_SINGLE_READER_TEMPLATE = (string) null;
    private static string DB_UPDATE_TEMPLATE = (string) null;
    private string _connString = (string) null;
    private const string T = "\t";
    private const string T2 = "\t\t";
    private const string T3 = "\t\t\t";
    private const string T4 = "\t\t\t\t";
    private const string T5 = "\t\t\t\t\t";
    private const string T6 = "\t\t\t\t\t\t";
    private DbConnection _dbconn;
    private string _driver;

    private DBHandler()
    {
    }

    private static string NormalizeFieldName(string input)
    {
      return DBHandler.NormalizeFieldName(input, false);
    }

    private static string NormalizeFieldName(string input, bool isClass)
    {
      string str = "";
      if (input == null)
        return (string) null;
      if (!isClass && input[0] != '_')
        str = "_";
      bool flag = isClass;
      foreach (char ch in input)
      {
        if (ch == '_')
        {
          flag = true;
        }
        else
        {
          str = !flag ? str + ch.ToString().ToLower() : str + ch.ToString().ToUpper();
          flag = false;
        }
      }
      return str;
    }

    private static string NormalizeClassName(string skipPrefix, string input)
    {
      if (input == null)
        return (string) null;
      return DBHandler.NormalizeFieldName(skipPrefix == null || !input.StartsWith(skipPrefix) ? input : input.Substring(skipPrefix.Length), true);
    }

    public void Close()
    {
      this._dbconn.Close();
      this._dbconn.Dispose();
    }

    public string[] GetTableList()
    {
      DbCommand dbCommand = (DbCommand) null;
      DbDataReader dbDataReader = (DbDataReader) null;
      List<string> stringList = new List<string>();
      try
      {
        dbCommand = this.GetDBCommand(this.GetShowTablesQuery());
        dbDataReader = dbCommand.ExecuteReader();
        while (dbDataReader.Read())
        {
          if (!dbDataReader.IsDBNull(0))
            stringList.Add(dbDataReader.GetString(0));
        }
      }
      catch (DbException ex)
      {
      }
      finally
      {
        dbDataReader?.Close();
        dbCommand?.Dispose();
      }
      return stringList.ToArray();
    }

    public DBHandler(string driver, string connString)
    {
      this._connString = connString;
      this._driver = driver;
      if (driver == "MySQL")
      {
        DBHandler.DB_READER_TEMPLATE = DBHandler._DB_READER_TEMPLATE.Replace("%%DB_DRIVER%%", "MySql");
        DBHandler.DB_SINGLE_READER_TEMPLATE = DBHandler._DB_SINGLE_READER_TEMPLATE.Replace("%%DB_DRIVER%%", "MySql");
        DBHandler.DB_UPDATE_TEMPLATE = DBHandler._DB_UPDATE_TEMPLATE.Replace("%%DB_DRIVER%%", "MySql");
        this._dbconn = DBConnectionFactory.GetMySqlConnection(connString);
      }
      else if (driver == "PgSQL")
      {
        DBHandler.DB_READER_TEMPLATE = DBHandler._DB_READER_TEMPLATE.Replace("%%DB_DRIVER%%", "PgSql");
        DBHandler.DB_SINGLE_READER_TEMPLATE = DBHandler._DB_SINGLE_READER_TEMPLATE.Replace("%%DB_DRIVER%%", "PgSql");
        DBHandler.DB_UPDATE_TEMPLATE = DBHandler._DB_UPDATE_TEMPLATE.Replace("%%DB_DRIVER%%", "PgSql");
        this._dbconn = DBConnectionFactory.GetPgSqlConnection(connString);
      }
      else
      {
        if (!(driver == "MSSQL"))
          return;
        DBHandler.DB_READER_TEMPLATE = DBHandler._DB_READER_TEMPLATE.Replace("%%DB_DRIVER%%", "Sql");
        DBHandler.DB_SINGLE_READER_TEMPLATE = DBHandler._DB_SINGLE_READER_TEMPLATE.Replace("%%DB_DRIVER%%", "Sql");
        DBHandler.DB_UPDATE_TEMPLATE = DBHandler._DB_UPDATE_TEMPLATE.Replace("%%DB_DRIVER%%", "Sql");
        this._dbconn = DBConnectionFactory.GetSqlConnection(connString);
      }
    }

    private DbCommand GetDBCommand(string queryString)
    {
      if (this._driver == "MySQL")
        return DBConnectionFactory.GetMySqlCommand(queryString, this._dbconn);
      if (this._driver == "PgSQL")
        return DBConnectionFactory.GetPgSqlCommand(queryString, this._dbconn);
      if (this._driver == "MSSQL")
        return DBConnectionFactory.GetSqlCommand(queryString, this._dbconn);
      return (DbCommand) null;
    }

    private string GetShowTablesQuery()
    {
      if (this._driver == "MySQL")
        return "show tables";
      if (this._driver == "PgSQL")
        return "SELECT relname as table_name from pg_class where reltype != 0 and relkind = 'r' and left(relname,3) != 'pg_' and left(relname,4) != 'sql_'";
      if (this._driver == "MSSQL")
        return "show tables";
      return (string) null;
    }

    private TableDef GetTableDef(string table)
    {
      if (this._driver == "MySQL")
        return this.GetTD_MySQL(table);
      if (this._driver == "PgSQL")
        return this.GetTD_PgSQL(table);
      if (this._driver == "MSSQL")
        return (TableDef) null;
      return (TableDef) null;
    }

    private TableDef GetTD_MySQL(string table)
    {
      string query = "SELECT COLUMN_NAME As field,ORDINAL_POSITION as ordinal, IS_NULLABLE, DATA_TYPE as DataType, CHARACTER_MAXIMUM_LENGTH as data_length,COLUMN_KEY as is_index,COLUMN_COMMENT as description,EXTRA\r\n\t\t\t\t\t\t\tFROM INFORMATION_SCHEMA.COLUMNS\r\n\t\t\t\t\t\t\twhere table_name = @name and table_schema = @schema group by COLUMN_NAME";
      DbCommand cmd = (DbCommand) null;
      DbDataReader reader = (DbDataReader) null;
      TableDef tableDef = new TableDef();
      tableDef.Name = table;
      List<FieldDef> fieldDefList = new List<FieldDef>();
      try
      {
        cmd = DBConnectionFactory.GetMySqlCommand(query, this._dbconn);
        DBTools.AddDbParameter(ref cmd, "@name", (object) table);
        DBTools.AddDbParameter(ref cmd, "@schema", (object) this._dbconn.Database);
        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
          FieldDef fieldDef = new FieldDef();
          fieldDef.Name = DBTools.GetDBStringValue(reader, "field");
          fieldDef.FieldLength = DBTools.GetDBIntValue(reader, "data_length");
          string dbStringValue = DBTools.GetDBStringValue(reader, "description");
          fieldDef.Description = string.IsNullOrEmpty(dbStringValue) ? DBHandler.NormalizeFieldName("_" + fieldDef.Name) : dbStringValue;
          try
          {
            fieldDef.DataType = DBTools.GetCSTypeFromDBType(DBTools.GetDBStringValue(reader, "DataType"), (double) fieldDef.FieldLength);
          }
          catch (Exception ex)
          {
            string message = ex.Message;
          }
          fieldDef.CanBeNull = DBTools.GetDBStringValue(reader, "is_nullable") == "YES";
          fieldDef.IsPrimaryKey = DBTools.GetDBStringValue(reader, "is_index", (string) null) == "PRI";
          fieldDefList.Add(fieldDef);
        }
      }
      catch (DbException ex)
      {
        throw ex;
      }
      finally
      {
        reader?.Close();
        cmd?.Dispose();
      }
      tableDef.Fields = fieldDefList.ToArray();
      return tableDef;
    }

    private TableDef GetTD_PgSQL(string table)
    {
      string query = "SELECT\tpg_attribute.attnum as ordinal,\tpg_attribute.attname AS Field,\tpg_type.typname AS DataType,\tpg_attribute.atttypmod AS data_length,\tpg_attribute.attnotnull AS is_nullable,\tpg_index.indkey is_index\r\n\t\t\t\t\t\t\tFROM\tpg_class\r\n\t\t\t\t\t\t\tINNER JOIN pg_attribute ON pg_attribute.attrelid = pg_class.oid\r\n\t\t\t\t\t\t\tLEFT JOIN pg_index ON pg_index.indrelid = pg_attribute.attrelid and pg_index.indnatts = pg_attribute.attnum\r\n\t\t\t\t\t\t\tINNER JOIN pg_type ON pg_type.oid = pg_attribute.atttypid\r\n\t\t\t\t\t\t\tWHERE\r\n\t\t\t\t\t\t\t\tpg_class.relname = @name\r\n\t\t\t\t\t\t\t\tand pg_attribute.attnum > 0\r\n\t\t\t\t\t\t\tORDER BY pg_attribute.attnum;";
      DbCommand cmd = (DbCommand) null;
      DbDataReader reader = (DbDataReader) null;
      TableDef tableDef = new TableDef();
      tableDef.Name = table;
      List<FieldDef> fieldDefList = new List<FieldDef>();
      try
      {
        cmd = DBConnectionFactory.GetPgSqlCommand(query, this._dbconn);
        DBTools.AddDbParameter(ref cmd, "@name", (object) table);
        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
          FieldDef fieldDef = new FieldDef();
          fieldDef.Name = DBTools.GetDBStringValue(reader, "field");
          fieldDef.FieldLength = DBTools.GetDBIntValue(reader, "data_length");
          fieldDef.DataType = DBTools.GetCSTypeFromDBType(DBTools.GetDBStringValue(reader, "DataType"), (double) fieldDef.FieldLength);
          fieldDef.CanBeNull = DBTools.GetDBBoolValue(reader, "is_nullable");
          fieldDef.IsPrimaryKey = DBTools.GetDBStringValue(reader, "is_index", (string) null) != null;
          fieldDefList.Add(fieldDef);
        }
      }
      catch (DbException ex)
      {
        throw ex;
      }
      finally
      {
        reader?.Close();
        cmd?.Dispose();
      }
      tableDef.Fields = fieldDefList.ToArray();
      return tableDef;
    }

    private string GetSQLQueries(TableDef tdef)
    {
      string str1 = "select ";
      string str2 = "";
      string str3 = "";
      string str4 = "";
      string str5 = "";
      bool flag = true;
      foreach (FieldDef field in tdef.Fields)
      {
        if (flag)
        {
          flag = false;
        }
        else
        {
          str1 += ",";
          str3 += ",";
          str4 += ",";
          str5 += ",";
        }
        str1 += field.Name;
        if (field.IsPrimaryKey)
        {
          if (str2 != "")
            str2 += " and ";
          str2 = str2 + field.Name + " = @" + field.Name;
        }
        str4 = str4 + "@" + field.Name;
        str3 += field.Name;
        str5 = str5 + field.Name + " = @" + field.Name;
      }
      string str6 = str1 + " from " + tdef.OriginalName;
      string str7 = (string) null;
      if (str2 != "")
        str7 = str6 + " where " + str2;
      string str8 = "delete from " + tdef.OriginalName + " where " + str2;
      string str9 = "insert into " + tdef.OriginalName + " (" + str3 + ") values (" + str4 + ")";
      string str10 = "update " + tdef.OriginalName + " set " + str5 + " where " + str2;
      return "\t\tprivate const String SQL_LIST_ALL = \"" + str6 + "\";" + DBHandler.NL + "\t\tprivate const String SQL_LIST_KEY = \"" + str7 + "\";" + DBHandler.NL + "\t\tprivate const String SQL_INSERT = \"" + str9 + "\";" + DBHandler.NL + "\t\tprivate const String SQL_DELETE = \"" + str8 + "\";" + DBHandler.NL + "\t\tprivate const String SQL_UPDATE = \"" + str10 + "\";" + DBHandler.NL2;
    }

    private string GetValueObject(string ns, TableDef tdef)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("using System;" + DBHandler.NL);
      stringBuilder.Append("using System.ComponentModel;" + DBHandler.NL);
      stringBuilder.Append("using System.ComponentModel.DataAnnotations;" + DBHandler.NL);
      stringBuilder.Append("using System.Web.Mvc;" + DBHandler.NL);
      stringBuilder.Append("using System.Collections.Generic;" + DBHandler.NL);
      stringBuilder.Append("using System.Linq;" + DBHandler.NL);
      stringBuilder.Append("using System.Web;" + DBHandler.NL);
      stringBuilder.Append("using System.Text;" + DBHandler.NL2);
      stringBuilder.Append("namespace " + ns.Trim() + DBHandler.NL + "{" + DBHandler.NL + "\tpublic class " + tdef.Name + DBHandler.NL + "\t{" + DBHandler.NL);
      stringBuilder.Append("\t\t#region |--------  Properties  --------|" + DBHandler.NL2);
      foreach (FieldDef field in tdef.Fields)
      {
        string str1 = DBHandler.NormalizeFieldName("_" + field.Name);
        if (str1 == tdef.Name)
          str1 += "Value";
        if (!field.CanBeNull)
          stringBuilder.Append("\t\t[Required]" + DBHandler.NL);
        stringBuilder.Append("\t\t[DisplayName(\"" + field.Description + "\")]" + DBHandler.NL);
        if (field.DataType == "String" && field.FieldLength > 0)
          stringBuilder.Append("\t\t[StringLength(" + (object) field.FieldLength + ")]" + DBHandler.NL);
        string str2 = field.CanBeNull ? (DBTools.IsNullable(field.DataType) ? field.DataType : field.DataType + "?") : field.DataType;
        stringBuilder.Append("\t\tpublic ");
        stringBuilder.Append(str2 + " " + str1 + " { get; set; }" + DBHandler.NL2);
      }
      stringBuilder.Append(DBHandler.NL + "\t\t#endregion" + DBHandler.NL2);
      stringBuilder.Append(this.GetXMLCreator(tdef));
      stringBuilder.Append(DBHandler.NL + "\t}" + DBHandler.NL + "}" + DBHandler.NL);
      return stringBuilder.ToString();
    }

    private string GetXMLCreator(TableDef tdef)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("\t\tpublic String ToXML()" + DBHandler.NL + "\t\t{" + DBHandler.NL);
      stringBuilder.Append("\t\t\tStringBuilder xml = new StringBuilder();" + DBHandler.NL2);
      foreach (FieldDef field in tdef.Fields)
      {
        string str1 = DBHandler.NormalizeFieldName("_" + field.Name);
        if (str1 == tdef.Name)
          str1 += "Value";
        string str2 = field.CanBeNull ? (DBTools.IsNullable(field.DataType) ? field.DataType : field.DataType + "?") : field.DataType;
        if (DBTools.IsNullable(field.DataType) || str2.EndsWith("?"))
          stringBuilder.Append("\t\t\txml.Append(\"<" + str1 + ">\" + ((" + str1 + " == null) ? \"\" : " + str1 + ".ToString()) + \"</" + str1 + ">\");" + DBHandler.NL);
        else
          stringBuilder.Append("\t\t\txml.Append(\"<" + str1 + ">\" + " + str1 + ".ToString() + \"</" + str1 + ">\");" + DBHandler.NL);
      }
      stringBuilder.Append(DBHandler.NL + "\t\t\treturn xml.ToString();" + DBHandler.NL);
      stringBuilder.Append("\t\t}" + DBHandler.NL2);
      return stringBuilder.ToString();
    }

    private string GetToDBMapper(TableDef tdef)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = new StringBuilder();
      stringBuilder2.Append("\t\t\tif( value == null ) {" + DBHandler.NL);
      stringBuilder2.Append("\t\t\t\tthrow new DBException(\"Value Object is NULL\");" + DBHandler.NL);
      stringBuilder2.Append("\t\t\t}" + DBHandler.NL2);
      string str1 = "\r\n        protected static void MapToDB(ref DbCommand cmd, @@VALUE@@ value) \r\n        {\r\n@@DATA@@\r\n        }\r\n\r\n".Replace("@@VALUE@@", tdef.Name);
      foreach (FieldDef field in tdef.Fields)
      {
        string str2 = field.CanBeNull ? (DBTools.IsNullable(field.DataType) ? field.DataType : field.DataType + "?") : field.DataType;
        string str3 = DBHandler.NormalizeClassName((string) null, field.Name);
        if (str3 == tdef.Name)
          str3 += "Value";
        stringBuilder1.Append("\t\t\tDBTools.AddDbParameter(ref cmd, \"@" + field.Name + "\", value." + str3 + ");" + DBHandler.NL);
        if (!field.CanBeNull && (DBTools.IsNullable(field.DataType) || str2.EndsWith("?")))
        {
          stringBuilder2.Append("\t\t\tif( value." + str3 + " == null ) {" + DBHandler.NL);
          stringBuilder2.Append("\t\t\t\tthrow new DBException(\"Field '" + str3 + "' cannot be null!\");" + DBHandler.NL);
          stringBuilder2.Append("\t\t\t}" + DBHandler.NL2);
        }
        if (field.DataType == "String" && field.FieldLength <= (int) byte.MaxValue && field.FieldLength > 0)
        {
          stringBuilder2.Append("\t\t\tif( value." + str3 + " != null && value." + str3 + ".Length > " + (object) field.FieldLength + " ) {" + DBHandler.NL);
          stringBuilder2.Append("\t\t\t\tthrow new DBException(\"Field '" + str3 + "' cannot be more than '" + (object) field.FieldLength + "' characters!\");" + DBHandler.NL);
          stringBuilder2.Append("\t\t\t}" + DBHandler.NL2);
        }
      }
      string newValue = stringBuilder2.ToString() + stringBuilder1.ToString();
      return str1.Replace("@@DATA@@", newValue);
    }

    private string GetFromDBMapper(TableDef tdef)
    {
      StringBuilder stringBuilder = new StringBuilder();
      string str1 = "\r\n        protected static @@VALUE@@ MapFromDB(DbDataReader dr) \r\n        {\r\n            @@VALUE@@ item = new @@VALUE@@();\r\n\r\n@@DATA@@\r\n\r\n            return item;\r\n        }\r\n\r\n".Replace("@@VALUE@@", tdef.Name);
      foreach (FieldDef field in tdef.Fields)
      {
        string str2 = DBHandler.NormalizeClassName((string) null, field.Name);
        if (str2 == tdef.Name)
          str2 += "Value";
        string str3 = !field.CanBeNull || DBTools.IsNullable(field.DataType) ? "" : "Nullable";
        stringBuilder.Append("\t\t\titem." + str2 + " = DBTools.GetDB" + field.DataType.Substring(0, 1).ToUpper().Trim() + field.DataType.Substring(1).Trim() + str3 + "Value( dr, \"" + field.Name + "\");" + DBHandler.NL);
      }
      return str1.Replace("@@DATA@@", stringBuilder.ToString());
    }

    private string GetSelectAll(TableDef tdef)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.Append("\t\tpublic static " + tdef.Name + "[] ListAll" + tdef.Name + "(");
      stringBuilder1.Append(")" + DBHandler.NL + "\t\t{" + DBHandler.NL);
      string str = DBHandler.DB_READER_TEMPLATE.Replace("%%VO_TYPE%%", tdef.Name).Replace("%%QUERY%%", "SQL_LIST_ALL").Replace("%%SQL_PARAMS%%", "");
      StringBuilder stringBuilder2 = new StringBuilder();
      stringBuilder2.Append("items.Add(MapFromDB(dr));");
      stringBuilder1.Append(str.Replace("%%CONTENT%%", stringBuilder2.ToString()));
      stringBuilder1.Append("\t\t}" + DBHandler.NL2);
      return stringBuilder1.ToString();
    }

    private string GetSelectByIndex(TableDef tdef)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.Append("\t\tpublic static " + tdef.Name + " SelectByKey(");
      IEnumerable<FieldDef> fieldDefs = ((IEnumerable<FieldDef>) tdef.Fields).Where<FieldDef>((Func<FieldDef, bool>) (f => f.IsPrimaryKey));
      bool flag = true;
      string str1 = DBHandler.NL;
      foreach (FieldDef fieldDef in fieldDefs)
      {
        if (!flag)
          stringBuilder1.Append(",");
        else
          flag = false;
        stringBuilder1.Append(fieldDef.DataType + " " + fieldDef.Name);
        str1 = str1 + "\t\t\t\tDBTools.AddDbParameter(ref cmd, \"@" + fieldDef.Name + "\", " + fieldDef.Name + ");" + DBHandler.NL;
      }
      string newValue = str1 + DBHandler.NL;
      stringBuilder1.Append(")" + DBHandler.NL + "\t\t{" + DBHandler.NL);
      string str2 = DBHandler.DB_SINGLE_READER_TEMPLATE.Replace("%%VO_TYPE%%", tdef.Name).Replace("%%QUERY%%", "SQL_LIST_KEY").Replace("%%SQL_PARAMS%%", newValue);
      StringBuilder stringBuilder2 = new StringBuilder();
      stringBuilder2.Append("item = MapFromDB(dr);");
      stringBuilder1.Append(str2.Replace("%%CONTENT%%", stringBuilder2.ToString()));
      stringBuilder1.Append("\t\t}" + DBHandler.NL2);
      return stringBuilder1.ToString();
    }

    private string GetDeleteByIndex(TableDef tdef)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("\t\tpublic static void DeleteByKey(");
      IEnumerable<FieldDef> fieldDefs = ((IEnumerable<FieldDef>) tdef.Fields).Where<FieldDef>((Func<FieldDef, bool>) (f => f.IsPrimaryKey));
      bool flag = true;
      string str1 = DBHandler.NL;
      foreach (FieldDef fieldDef in fieldDefs)
      {
        if (!flag)
          stringBuilder.Append(",");
        else
          flag = false;
        stringBuilder.Append(fieldDef.DataType + " " + fieldDef.Name);
        str1 = str1 + "\t\t\t\tDBTools.AddDbParameter(ref cmd, \"@" + fieldDef.Name + "\", " + fieldDef.Name + ");" + DBHandler.NL;
      }
      string newValue = str1 + DBHandler.NL;
      stringBuilder.Append(")" + DBHandler.NL + "\t\t{" + DBHandler.NL);
      string str2 = DBHandler.DB_UPDATE_TEMPLATE.Replace("%%VO_TYPE%%", tdef.Name).Replace("%%QUERY%%", "SQL_DELETE").Replace("%%SQL_PARAMS%%", newValue);
      stringBuilder.Append(str2);
      stringBuilder.Append("\t\t}" + DBHandler.NL2);
      return stringBuilder.ToString();
    }

    private string GetInsertOrUpdate(TableDef tdef, string method, string SQL)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("\t\tpublic static void " + method + "(" + tdef.Name + " value)" + DBHandler.NL + "\t\t{" + DBHandler.NL);
      string str = DBHandler.DB_UPDATE_TEMPLATE.Replace("%%VO_TYPE%%", tdef.Name).Replace("%%QUERY%%", SQL).Replace("%%SQL_PARAMS%%", "MapToDB(ref cmd, value);" + DBHandler.NL);
      stringBuilder.Append(str);
      stringBuilder.Append("\t\t}" + DBHandler.NL2);
      return stringBuilder.ToString();
    }

    private string GetDAO(string dao_ns, string common_ns, string vo_ns, TableDef tdef)
    {
      StringBuilder stringBuilder = new StringBuilder();
      string str = tdef.Name + "_DAO";
      stringBuilder.Append("using System;" + DBHandler.NL + "using System.Collections.Generic;" + DBHandler.NL + "using System.Linq;" + DBHandler.NL + "using System.Web;" + DBHandler.NL);
      stringBuilder.Append("using " + common_ns + ".Database;" + DBHandler.NL + "using " + vo_ns + ";" + DBHandler.NL + "using System.Data.Common;" + DBHandler.NL2);
      stringBuilder.Append("namespace " + dao_ns.Trim() + DBHandler.NL + "{" + DBHandler.NL + "\r\n    /// <summary>\r\n    /// This class is autogenerated! Please don't change the content of this file.\r\n    /// If you need to add methods or functionallity, you should create a sub-class\r\n    /// and inherit from this DAO class.\r\n    /// </summary>\r\n\tinternal class " + str + " : DAOClass" + DBHandler.NL + "\t{" + DBHandler.NL);
      stringBuilder.Append(this.GetSQLQueries(tdef));
      stringBuilder.Append(this.GetFromDBMapper(tdef));
      stringBuilder.Append(this.GetToDBMapper(tdef));
      stringBuilder.Append(this.GetSelectAll(tdef));
      stringBuilder.Append(this.GetSelectByIndex(tdef));
      stringBuilder.Append(this.GetDeleteByIndex(tdef));
      stringBuilder.Append(this.GetInsertOrUpdate(tdef, "AddNew", "SQL_INSERT"));
      stringBuilder.Append(this.GetInsertOrUpdate(tdef, "Update", "SQL_UPDATE"));
      stringBuilder.Append(DBHandler.NL + "\t}" + DBHandler.NL + "}" + DBHandler.NL);
      return stringBuilder.ToString();
    }

    private string GetDAOImplClass(string ns, string baseClass)
    {
      return "using System;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing System.Text;\r\n\r\nnamespace %%NS%%\r\n{\r\n    /// <summary>\r\n    /// This class is not overwritten. You should keep your custom business logic\r\n    /// in this file!\r\n    /// </summary>\r\n    internal class %%BASE_CLASS%%Impl : %%BASE_CLASS%%_DAO \r\n    {\r\n    }\r\n}\r\n".Replace("%%NS%%", ns).Replace("%%BASE_CLASS%%", baseClass);
    }

    private string GetDAOBaseClass(string ns, string connString)
    {
      return "using System;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing System.Text;\r\n\r\nnamespace %%NS%%\r\n{\r\n    /// <summary>\r\n    /// This class is autogenerated! Please don't change the content of this file.\r\n    /// If you need to add methods or functionallity, you should create a sub-class\r\n    /// and inherit from this DAO class.\r\n    /// </summary>\r\n    internal class DAOClass \r\n    {\r\n        protected static String CONNECTION_STRING  = \"%%CS%%\";\r\n    }\r\n}\r\n".Replace("%%NS%%", ns).Replace("%%CS%%", connString);
    }

    private string GetXSD(TableDef tdef)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.Append("<?xml version='1.0' encoding='ISO-8859-1' ?>" + DBHandler.NL);
      stringBuilder1.Append("<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>" + DBHandler.NL2);
      StringBuilder stringBuilder2 = new StringBuilder();
      string str = "";
      stringBuilder2.Append("<xs:complexType name='" + tdef.Name + "'>" + DBHandler.NL + "\t<xs:sequence>" + DBHandler.NL);
      foreach (FieldDef field in tdef.Fields)
      {
        string xsdTypeDef = this.GetXSDTypeDef(field);
        if (str.IndexOf("'" + field.DataType.Trim() + "Type'") == -1)
          str += xsdTypeDef;
        stringBuilder2.Append("\t\t<xs:element name='" + field.Name + "' type='" + field.DataType.Trim() + "Type' />" + DBHandler.NL);
      }
      stringBuilder2.Append("\t</xs:sequence>" + DBHandler.NL);
      stringBuilder2.Append("</xs:complexType>" + DBHandler.NL);
      stringBuilder1.Append(str);
      stringBuilder1.Append(stringBuilder2.ToString());
      stringBuilder1.Append("</xs:schema>" + DBHandler.NL);
      return stringBuilder1.ToString();
    }

    private string GetXSDTypeDef(FieldDef fd)
    {
      string xsdDataType = DBTools.GetXSDDataType(fd.DataType);
      return "<xs:simpleType name='" + fd.DataType.Trim() + "Type'>" + DBHandler.NL + "\t<xs:restriction base='xs:" + xsdDataType + "'/>" + DBHandler.NL + "</xs:simpleType>" + DBHandler.NL2;
    }

    private string GetASP(TableDef tdef)
    {
      return new StringBuilder().ToString();
    }

    public void CreateClass(string dao_ns, string vo_ns, string common_ns, string table, string dstDir, string skipTablePrefix, bool genValueObject, bool genDAO, bool genXSD, bool genASP, bool genDBHelper)
    {
      TableDef tableDef = this.GetTableDef(table);
      tableDef.OriginalName = table;
      tableDef.Name = DBHandler.NormalizeClassName(skipTablePrefix, tableDef.Name);
      if (!Directory.Exists(dstDir))
        Directory.CreateDirectory(dstDir);
      if (genValueObject)
      {
        string valueObject = this.GetValueObject(vo_ns, tableDef);
        string path = Path.Combine(dstDir, vo_ns);
        if (!Directory.Exists(path))
          Directory.CreateDirectory(path);
        File.WriteAllText(path + "/" + tableDef.Name + ".cs", valueObject);
      }
      if (genDAO)
      {
        string dao = this.GetDAO(dao_ns, common_ns, vo_ns, tableDef);
        string path = Path.Combine(dstDir, dao_ns);
        if (!Directory.Exists(path))
          Directory.CreateDirectory(path);
        File.WriteAllText(path + "/" + tableDef.Name + "_DAO.cs", dao);
        string str = tableDef.Name + "Impl.cs";
        if (!File.Exists(path + "/" + str))
          File.WriteAllText(path + "/" + str, this.GetDAOImplClass(dao_ns, tableDef.Name));
        string daoBaseClass = this.GetDAOBaseClass(dao_ns, this._connString);
        File.WriteAllText(path + "/DAOClass.cs", daoBaseClass);
      }
      if (genDBHelper)
      {
        string path = Path.Combine(dstDir, common_ns, "Database");
        if (!Directory.Exists(path))
          Directory.CreateDirectory(path);
        File.WriteAllText(path + "/DBConnectionFactory.cs", "using System;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing System.Text;\r\nusing System.Data.Common;\r\nusing System.Data.SqlClient;\r\nusing MySql.Data.MySqlClient;\r\n#if SQLITE\r\nusing System.Data.SQLite;\r\n#endif\r\n\r\n\r\nnamespace %%NS%%.Database\r\n{\r\n    public class DBConnectionFactory\r\n    {\r\n        public static DbConnection GetPgSqlConnection(String connStr)\r\n        {\r\n            DbConnection conn = new Npgsql.NpgsqlConnection(connStr);\r\n            conn.Open();\r\n            return conn;\r\n        }\r\n\r\n        public static DbConnection GetSqlConnection(String connStr)\r\n        {\r\n            DbConnection conn = new SqlConnection(connStr);\r\n            conn.Open();\r\n            return conn;\r\n        }\r\n\r\n        public static DbConnection GetMySqlConnection(String connStr)\r\n        {\r\n            DbConnection conn = new MySqlConnection(connStr);\r\n            conn.Open();\r\n            return conn;\r\n        }\r\n\r\n\r\n        public static DbCommand GetPgSqlCommand(String query, DbConnection conn)\r\n        {\r\n            return new Npgsql.NpgsqlCommand(query, (Npgsql.NpgsqlConnection)conn);\r\n        }\r\n\r\n        public static DbCommand GetSqlCommand(String query, DbConnection conn)\r\n        {\r\n            return new SqlCommand(query, (SqlConnection)conn);\r\n        }\r\n\r\n        public static DbCommand GetMySqlCommand(String query, DbConnection conn)\r\n        {\r\n            return new MySqlCommand(query, (MySqlConnection)conn);\r\n        }\r\n\r\n#if SQLITE\r\n        public static DbCommand GetSqlLiteCommand(String query, DbConnection conn)\r\n        {\r\n            return new SQLiteCommand(query, (SQLiteConnection)conn);\r\n        }\r\n\r\n        public static DbConnection GetSQLiteConnection(String connStr)\r\n        {\r\n            DbConnection conn = new System.Data.SQLite.SQLiteConnection(connStr);\r\n            conn.Open();\r\n            return conn;\r\n        }\r\n#endif\r\n\r\n    }\r\n}\r\n".Replace("%%NS%%", common_ns));
        File.WriteAllText(path + "/DBException.cs", "using System;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing System.Text;\r\n\r\nnamespace %%NS%%.Database\r\n{\r\n    public class DBException : Exception \r\n    {\r\n        public DBException(String msg) : base(msg) {}\r\n        public DBException(String msg, Exception baseExeption) : base(msg, baseExeption) { }\r\n    }\r\n}\r\n".Replace("%%NS%%", common_ns));
        File.WriteAllText(path + "/DBTools.cs", "using System;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing System.Text;\r\nusing System.Data.Common;\r\n\r\nnamespace %%NS%%.Database\r\n{\r\n    public class DBTools\r\n    {\r\n        public static Object GetDBValue(DbDataReader reader, String field, Object defaultValue)\r\n        {\r\n            if (reader.IsDBNull(reader.GetOrdinal(field)))\r\n                return defaultValue;\r\n            else\r\n            {\r\n                if (defaultValue is String)\r\n                    return reader.GetString(reader.GetOrdinal(field));\r\n                else if (defaultValue is int)\r\n                    return reader.GetInt32(reader.GetOrdinal(field));\r\n                else if (defaultValue is short)\r\n                    return reader.GetInt16(reader.GetOrdinal(field));\r\n                else if (defaultValue is bool)\r\n                    return reader.GetBoolean(reader.GetOrdinal(field));\r\n                else if (defaultValue is DateTime)\r\n                {\r\n                    try\r\n                    {\r\n                        DateTime tmp = reader.GetDateTime(reader.GetOrdinal(field));\r\n                        return tmp;\r\n                    }\r\n                    catch (Exception)\r\n                    {\r\n                        return null;\r\n                    }\r\n\r\n                }\r\n                //else if (defaultValue is UInt64)\r\n                //    return reader.GetUInt64(reader.GetOrdinal(field));\r\n                else if (defaultValue is long)\r\n                    return reader.GetInt64(reader.GetOrdinal(field));\r\n                else if (defaultValue is double || defaultValue is Double)\r\n                    return reader.GetDouble(reader.GetOrdinal(field));\r\n                else if(defaultValue is Decimal || defaultValue is decimal)\r\n                    return reader.GetDecimal(reader.GetOrdinal(field));\r\n                else\r\n                    return reader.GetValue(reader.GetOrdinal(field));\r\n            }\r\n        }\r\n\r\n        public static bool GetDBBoolValue(DbDataReader reader, String field)\r\n        {\r\n            return (bool)GetDBValue(reader, field, false);\r\n        }\r\n\r\n        public static String GetDBStringValue(DbDataReader reader, String field, String defaultVal)\r\n        {\r\n            return (String)GetDBValue(reader, field, defaultVal);\r\n        }\r\n\r\n        public static String GetDBStringValue(DbDataReader reader, String field)\r\n        {\r\n            return (String)GetDBValue(reader, field, \"\");\r\n        }\r\n\r\n        public static int GetDBIntValue(DbDataReader reader, String field)\r\n        {\r\n            return (int)GetDBValue(reader, field, 0);\r\n        }\r\n\r\n        public static long GetDBLongValue(DbDataReader reader, String field)\r\n        {\r\n            return (long)GetDBValue(reader, field, 0L);\r\n        }\r\n\r\n        public static short GetDBShortValue(DbDataReader reader, String field)\r\n        {\r\n            return (short)GetDBValue(reader, field, (short)0);\r\n        }\r\n\r\n        public static UInt64 GetDBUInt64Value(DbDataReader reader, String field)\r\n        {\r\n            return (UInt64)GetDBValue(reader, field, (UInt64)0);\r\n        }\r\n\r\n        public static Int64 GetDBInt64Value(DbDataReader reader, String field)\r\n        {\r\n            return (Int64)GetDBValue(reader, field, (Int64)0);\r\n        }\r\n\r\n        public static Decimal GetDBDecimalValue(DbDataReader reader, String field)\r\n        {\r\n            return (Decimal)GetDBValue(reader, field, (Decimal)0);\r\n        }\r\n\r\n        public static DateTime GetDBDateTimeValue(DbDataReader reader, String field)\r\n        {\r\n            Object dt = GetDBValue(reader, field, DateTime.MinValue);\r\n            if (dt == null) return DateTime.MinValue;\r\n            return (DateTime)dt;\r\n        }\r\n\r\n        public static double GetDBDoubleValue(DbDataReader reader, String field)\r\n        {\r\n            return (double)GetDBValue(reader, field, 0.0d);\r\n        }\r\n\r\n        public static void AddDbParameter(ref DbCommand cmd, String name, object value, bool obsoleteParamDummy)\r\n        {\r\n            AddDbParameter(ref cmd, name, value);\r\n        }\r\n\r\n        public static void AddDbParameter(ref DbCommand cmd, String name, object value)\r\n        {\r\n            if( cmd.GetType() == typeof(MySql.Data.MySqlClient.MySqlCommand))\r\n            {\r\n                MySql.Data.MySqlClient.MySqlParameter p = new MySql.Data.MySqlClient.MySqlParameter(name.Trim(), value);\r\n                cmd.Parameters.Add(p);\r\n            }\r\n            else if (cmd.GetType() == typeof(Npgsql.NpgsqlCommand))\r\n            {\r\n                Npgsql.NpgsqlParameter p = new Npgsql.NpgsqlParameter(name, value);\r\n                cmd.Parameters.Add(p);\r\n            }\r\n            else\r\n            {\r\n                cmd.Parameters.Insert(0, value);\r\n                cmd.Parameters[0].ParameterName = name.Trim();\r\n            }\r\n        }\r\n\r\n\r\n        public static String GetCSTypeFromDBType(String dbType, double len)\r\n        {\r\n            switch( dbType.ToLower().Trim() )\r\n            {\r\n                case \"varchar\":\r\n                case \"text\":\r\n                case \"tinytext\":\r\n                case \"char\":\r\n                    return \"String\";\r\n                case \"timestamp\":\r\n                case \"date\":\r\n                case \"time\":\r\n                case \"datetime\":\r\n                    return \"DateTime\";\r\n                case \"float\":\r\n                case \"float4\":\r\n                    return \"float\";\r\n                case \"float8\":\r\n                case \"double\":\r\n                    return \"double\";\r\n                case \"int4\":\r\n                case \"int\":\r\n                    return \"int\";\r\n                case \"int8\":\r\n                    return \"long\";\r\n                case \"short\":\r\n                case \"int2\":\r\n                case \"smallint\":\r\n                    return \"short\";\r\n                case \"bool\":\r\n                case \"boolean\":\r\n                case \"tinyint\":\r\n                    return \"bool\";\r\n                case \"bigint\":\r\n                    return \"Int64\";\r\n                case \"decimal\":\r\n                    return \"Decimal\";\r\n                case \"bit\":\r\n                    if (len == 1)\r\n                        return \"bool\";\r\n                    return \"int\";\r\n                default:\r\n                    throw new Exception(\"Unknown datatype: \" + dbType);\r\n            }\r\n        }\r\n\r\n        public static bool IsNullable(String dataType)\r\n        {\r\n            switch (dataType)\r\n            {\r\n                case \"String\":\r\n                    return true;\r\n                case \"int\":\r\n                case \"bool\":\r\n                case \"Decimal\":\r\n                case \"short\":\r\n                case \"long\":\r\n                case \"double\":\r\n                case \"single\":\r\n                case \"float\":\r\n                case \"DateTime\":\r\n                    return false;\r\n            }\r\n            return false;\r\n        }\r\n\r\n\r\n\r\n    }\r\n}\r\n".Replace("%%NS%%", common_ns));
      }
      if (!genXSD)
        return;
      string path1 = Path.Combine(dstDir, "xsd");
      if (!Directory.Exists(path1))
        Directory.CreateDirectory(path1);
      File.WriteAllText(path1 + "/" + tableDef.Name.Trim() + ".xsd", this.GetXSD(tableDef));
    }
  }
}
