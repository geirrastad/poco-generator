using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Npgsql;
using System.Data;
#if SQLITE
using System.Data.SQLite;
#endif

namespace CommonUtils.Database
{
    public class DBConnectionFactory
    {
        public static DbConnection GetSqlConnection(String connStr)
        {
            DbConnection conn = new SqlConnection(connStr);
            conn.Open();
            return conn;
        }

        public static DbConnection GetMySqlConnection(String connStr)
        {
            DbConnection conn = new MySqlConnection(connStr);
            conn.Open();
            return conn;
        }

        public static DbConnection GetPgSqlConnection(string connStr)
        {
            DbConnection conn = new NpgsqlConnection(connStr);
            conn.Open();
            return conn;
        }

        public static DbCommand GetSqlCommand(String query, DbConnection conn)
        {
            return new SqlCommand(query, (SqlConnection)conn);
        }

        public static DbCommand GetMySqlCommand(String query, DbConnection conn)
        {
            return new MySqlCommand(query, (MySqlConnection)conn);
        }

        public static DbCommand GetPgSqlCommand(String query, DbConnection conn)
        {
            return new NpgsqlCommand(query, (NpgsqlConnection)conn);
        }

#if SQLITE
        public static DbCommand GetSqlLiteCommand(String query, DbConnection conn)
        {
            return new SQLiteCommand(query, (SQLiteConnection)conn);
        }

        public static DbConnection GetSQLiteConnection(String connStr)
        {
            DbConnection conn = new System.Data.SQLite.SQLiteConnection(connStr);
            conn.Open();
            return conn;
        }
#endif

    }
}
