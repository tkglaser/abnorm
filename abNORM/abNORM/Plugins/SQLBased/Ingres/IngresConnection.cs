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
using IngClient=Ingres.Client;

namespace abNORM.Plugins.SQLBased.Ingres
{
    internal class IngresConnection : IConnection
    {
        protected string conString;
        private ConnectionSettings settings;

        internal IngresConnection(
            string connectionstring,
            ConnectionSettings settings)
        {
            conString = connectionstring;
            this.settings = settings;
        }

        public ConnectionFactory.DBType getDbType()
        {
            return ConnectionFactory.DBType.Ingres;
        }

        protected void configSession(DbConnection con)
        {
            /*
             * The Ingres-User needs the following Rights:
             *  - Security-Admin (for Session-Prioritity)
             *  - Set Trace Flags (for dm503)
             * */
            DbCommand cmd = null;
            DbTransaction trx = null;
            try
            {
                cmd = con.CreateCommand();
                trx = con.BeginTransaction(IsolationLevel.ReadUncommitted);
                cmd.Transaction = trx;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SET session WITH description = '" + settings.sessionDescription + "'";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "SET session WITH priority = minimum";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "SET lockmode session WHERE level=page, readlock=nolock, maxlocks=1000, timeout=300";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "SET trace point dm503";
                cmd.ExecuteNonQuery();
                trx.Commit();
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (trx != null)
                    trx.Dispose();
            }
        }

        protected System.Data.Common.DbConnection getConnection()
        {
            DbConnection con = new IngClient.IngresConnection(conString);

            con.Open();
            configSession(con);

            return con;
        }

        public ITransaction BeginTransaction()
        {
            return new TransactionTemplate(
                new IngresDataProvider(getConnection(), settings), settings);
        }
    }
}
