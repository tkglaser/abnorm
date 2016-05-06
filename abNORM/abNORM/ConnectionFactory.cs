#region Copyright (C) 2007 Thomas Glaser. All Rights Reserved.
/*
 * This file is part of abNORM.
 *
 * abNORM is free software; you can redistribute it and/or modify it under 
 * the terms of the GNU General Public License (Version 2) as published by 
 * the Free Software Foundation.
 *
 * abNORM is distributed in the hope that it will be useful, but WITHOUT 
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or 
 * FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License 
 * for more details.
 *
 * You should have received a copy of the GNU General Public License 
 * along with abNORM; if not, write to the Free Software Foundation, 
 * Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA
 */
#endregion
 
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace abNORM
{
    public class ConnectionFactory
    {
        /// <summary>
        /// DBType
        /// </summary>
        public enum DBType
        {
            /// <summary>
            /// Ingres DBMS
            /// </summary>
            Ingres,

            /// <summary>
            /// Postgres DBMS
            /// </summary>
            Postgres,

            /// <summary>
            /// Oracle DBMS
            /// </summary>
            Oracle,

            /// <summary>
            /// MySQL DBMS
            /// </summary>
            MySQL,

            /// <summary>
            /// Microsoft SQL Server
            /// </summary>
            MSSQL,

            /// <summary>
            /// XML-based Persistence
            /// </summary>
            XML
        }

        /// <summary>
        /// Provides a new connection
        /// </summary>
        /// <param name="type">DBType</param>
        /// <param name="connectstring">Connection String</param>
        /// <returns>ProduktivDB-Objectlayer</returns>
        public static IConnection getConnection(
            DBType type, 
            string connectstring)
        {
            return getConnection(type, connectstring, new ConnectionSettings());
        }

        /// <summary>
        /// Returns a DB-specific connectionstring.
        /// </summary>
        /// <param name="type">DBType</param>
        /// <param name="host">The machine, the database is hosted on</param>
        /// <param name="database">Name of the database</param>
        /// <param name="user">Username for connection</param>
        /// <param name="password">Password</param>
        /// <returns>The DB-specific connectionstring</returns>
        public static string createConnectionString(
            DBType type,
            string host,
            string database,
            string user,
            string password)
        {
            switch (type)
            {
                case DBType.Ingres:
                    Ingres.Client.IngresConnectionStringBuilder ingresCSB 
                        = new Ingres.Client.IngresConnectionStringBuilder();
                    ingresCSB.Server = host;
                    ingresCSB.Database = database;
                    ingresCSB.UserID = user;
                    ingresCSB.Password = password;
                    return ingresCSB.ConnectionString;

                case DBType.Oracle:
                    /* Unfortunately, the oracledriver of mono-1.2.5.1
                     * does not contain a OracleConnectionStringBuilder class, so
                     * this part failes to compile under mono.
                     * As a workaround for now, the connection string gets pieced
                     * together manually. Once, the appropriate class is present in 
                     * mono, this part can be commented in again.
                     * 
                    System.Data.OracleClient.OracleConnectionStringBuilder oracleCSB 
                        = new System.Data.OracleClient.OracleConnectionStringBuilder();
                    oracleCSB.DataSource = host;
                    oracleCSB.UserID = user;
                    oracleCSB.Password = password;
                    return oracleCSB.ConnectionString;
                     */
                    return "Data Source=" + host
                        + ";User ID=" + user
                        + ";Password=" + password;

                case DBType.XML:
                    return database;

                case DBType.MySQL:
                    MySQLDriverCS.MySQLConnectionStringBuilder mysqlCSB 
                        = new MySQLDriverCS.MySQLConnectionStringBuilder();
                    mysqlCSB.Host = host;
                    mysqlCSB.DataSource = database;
                    mysqlCSB.UserId = user;
                    mysqlCSB.Password = password;
                    return mysqlCSB.ConnectionString;

                case DBType.MSSQL:
                    System.Data.SqlClient.SqlConnectionStringBuilder mssqlCSB 
                        = new System.Data.SqlClient.SqlConnectionStringBuilder();
                    mssqlCSB.DataSource = host;
                    mssqlCSB.UserID = user;
                    mssqlCSB.Password = password;
                    return mssqlCSB.ConnectionString;

                case DBType.Postgres:
                    return "Server = " + host
                        + "; User Id = " + user
                        + "; Password = " + password
                        + "; Database = " + database
                        + "; Encoding = UNICODE;";
            }
            throw new Exception("Unknown database type encountered!");
        }

        /// <summary>
        /// Provides a new connection
        /// </summary>
        /// <param name="type"></param>
        /// <param name="connectstring"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static IConnection getConnection(
            DBType type,
            string connectstring,
            ConnectionSettings settings)
        {
            switch (type)
            {
                case DBType.Ingres:
                    return new Plugins.SQLBased.Ingres.IngresConnection(
                        connectstring,
                        settings);

                case DBType.Postgres:
                    return new Plugins.SQLBased.Postgres.PostgresConnection(
                        connectstring,
                        settings);

                case DBType.Oracle:
                    return new Plugins.SQLBased.Oracle.OracleConnection(
                        connectstring,
                        settings);

                case DBType.MySQL:
                    return new Plugins.SQLBased.MySQL.MySQLConnection(
                        connectstring,
                        settings);

                case DBType.MSSQL:
                    return new Plugins.SQLBased.MSSQL.MSSQLConnection(
                        connectstring,
                        settings);

                case DBType.XML:
                    return new Plugins.Other.XML.XMLConnection(
                        connectstring,
                        settings);
            }

            throw new Exception("Something is terribly wrong...");
        }
    }
}
