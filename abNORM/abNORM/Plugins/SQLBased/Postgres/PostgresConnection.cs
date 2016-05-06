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
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;
using PostgresClient = Postgres.Client;

namespace abNORM.Plugins.SQLBased.Postgres
{
    internal class PostgresConnection : IConnection
    {
        private string conString;
        private ConnectionSettings settings;

        internal PostgresConnection(
            string connectionstring,
            ConnectionSettings settings)
        {
            conString = connectionstring;
            this.settings = settings;
        }

        public ConnectionFactory.DBType getDbType()
        {
            return ConnectionFactory.DBType.Postgres;
        }

        protected System.Data.Common.DbConnection getConnection()
        {
            DbConnection con = new PostgresClient.PostgresConnection(conString);

            con.Open();

            return con;
        }

        public ITransaction BeginTransaction()
        {
            return new TransactionTemplate(
                new PostgresDataProvider(getConnection(), settings), settings);
        }
    }
}
