// Decompiled with JetBrains decompiler
// Type: CommonUtils.Database.DBTools
// Assembly: CommonUtils, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 633D58C8-273F-4F1E-A0EB-8F8C30A8EDB9
// Assembly location: C:\Users\geir\Desktop\POCO\CommonUtils.dll

using CommonUtils.Logging;
using MySql.Data.MySqlClient;
using Npgsql;
using System;
using System.Data.Common;

namespace CommonUtils.Database
{
    public class DBTools
    {
        private static ILogger log = LoggerFactory.GetLogger(typeof(DBTools));

        public static object GetDBValue(DbDataReader reader, string field, object defaultValue)
        {
            if (reader.IsDBNull(reader.GetOrdinal(field)))
                return defaultValue;
            if (defaultValue is string)
                return (object)reader.GetString(reader.GetOrdinal(field));
            if (defaultValue is int)
                return (object)reader.GetInt32(reader.GetOrdinal(field));
            if (defaultValue is short)
                return (object)reader.GetInt16(reader.GetOrdinal(field));
            if (defaultValue is bool)
                return (object)reader.GetBoolean(reader.GetOrdinal(field));
            if (defaultValue is DateTime)
            {
                try
                {
                    return (object)reader.GetDateTime(reader.GetOrdinal(field));
                }
                catch (Exception ex)
                {
                    return (object)null;
                }
            }
            else
            {
                if (defaultValue is long)
                    return (object)reader.GetInt64(reader.GetOrdinal(field));
                if (defaultValue is double || defaultValue is double)
                    return (object)reader.GetDouble(reader.GetOrdinal(field));
                if (defaultValue is Decimal || defaultValue is Decimal)
                    return (object)reader.GetDecimal(reader.GetOrdinal(field));
                return reader.GetValue(reader.GetOrdinal(field));
            }
        }

        public static bool GetDBBoolValue(DbDataReader reader, string field)
        {
            return (bool)DBTools.GetDBValue(reader, field, (object)false);
        }

        public static string GetDBStringValue(DbDataReader reader, string field, string defaultVal)
        {
            return (string)DBTools.GetDBValue(reader, field, (object)defaultVal);
        }

        public static string GetDBStringValue(DbDataReader reader, string field)
        {
            return (string)DBTools.GetDBValue(reader, field, (object)"");
        }

        public static int GetDBIntValue(DbDataReader reader, string field)
        {
            return (int)DBTools.GetDBValue(reader, field, (object)0);
        }

        public static long GetDBLongValue(DbDataReader reader, string field)
        {
            return (long)DBTools.GetDBValue(reader, field, (object)0L);
        }

        public static ulong GetDBUInt64Value(DbDataReader reader, string field)
        {
            return (ulong)DBTools.GetDBValue(reader, field, (object)0UL);
        }

        public static long GetDBInt64Value(DbDataReader reader, string field)
        {
            return (long)DBTools.GetDBValue(reader, field, (object)0L);
        }

        public static Decimal GetDBDecimalValue(DbDataReader reader, string field)
        {
            return (Decimal)DBTools.GetDBValue(reader, field, (object)new Decimal(0));
        }

        public static DateTime GetDBDateTimeValue(DbDataReader reader, string field)
        {
            object dbValue = DBTools.GetDBValue(reader, field, (object)DateTime.MinValue);
            if (dbValue == null)
                return DateTime.MinValue;
            return (DateTime)dbValue;
        }

        public static double GetDBDoubleValue(DbDataReader reader, string field)
        {
            return (double)DBTools.GetDBValue(reader, field, (object)0.0);
        }

        public static short GetDBShortValue(DbDataReader reader, string field)
        {
            return (short)DBTools.GetDBValue(reader, field, (object)(short)0);
        }

        public static void AddDbParameter(ref DbCommand cmd, string name, object value, bool obsoleteParamDummy)
        {
            DBTools.AddDbParameter(ref cmd, name, value);
        }

        public static void AddDbParameter(ref DbCommand cmd, string name, object value)
        {
            if (cmd.GetType() == typeof(MySqlCommand))
            {
                MySqlParameter mySqlParameter = new MySqlParameter(name.Trim(), value);
                cmd.Parameters.Add((object)mySqlParameter);
            }
            else if (cmd.GetType() == typeof(NpgsqlCommand))
            {
                NpgsqlParameter npgsqlParameter = new NpgsqlParameter(name, value);
                cmd.Parameters.Add((object)npgsqlParameter);
            }
            else
            {
                cmd.Parameters.Insert(0, value);
                cmd.Parameters[0].ParameterName = name.Trim();
            }
        }

        public static string GetCSTypeFromDBType(string dbType, double len)
        {
            switch (dbType.ToLower().Trim())
            {
                case "varchar":
                case "text":
                case "tinytext":
                case "char":
                    return "String";
                case "timestamp":
                case "date":
                case "time":
                case "datetime":
                    return "DateTime";
                case "float":
                case "float4":
                    return "float";
                case "float8":
                case "double":
                    return "double";
                case "int4":
                case "int":
                    return "int";
                case "int8":
                    return "long";
                case "short":
                case "int2":
                case "Int16":
                case "smallint":
                    return "short";
                case "bool":
                case "boolean":
                case "tinyint":
                    return "bool";
                case "bigint":
                    return "Int64";
                case "decimal":
                    return "Decimal";
                case "bit":
                    return len == 1.0 ? "bool" : "int";
                default:
                    throw new Exception("Unknown datatype: " + dbType);
            }
        }

        public static bool IsNullable(string dataType)
        {
            switch (dataType)
            {
                case "String":
                    return true;
                case "int":
                case "bool":
                case "Decimal":
                case "short":
                case "long":
                case "double":
                case "single":
                case "float":
                case "DateTime":
                    return false;
                default:
                    return false;
            }
        }

        public static string GetXSDDataType(string dataType)
        {
            switch (dataType)
            {
                case "UInt32":
                    return "unsignedInt";
                case "UInt64":
                    return "unsignedLong";
                case "String":
                    return "string";
                case "int":
                case "Int32":
                    return "int";
                case "bool":
                    return "boolean";
                case "Decimal":
                    return "decimal";
                case "byte":
                case "Byte":
                    return "byte";
                case "short":
                    return "short";
                case "long":
                case "Int64":
                    return "long";
                case "double":
                    return "double";
                case "single":
                case "float":
                    return "float";
                case "DateTime":
                    return "dateTime";
                default:
                    throw new Exception("Not a valid XSD type: " + dataType);
            }
        }
    }
}
