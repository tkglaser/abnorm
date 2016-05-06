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
using System.Text;

using Oracle.Client;

// This Wrapper can be removed when the Mono Oracle Client becomes .NET 2.0 compatible

namespace abNORM.Plugins.SQLBased.Oracle
{
    class OracleDataProvider : SQLDataProvider
    {
        internal OracleDataProvider(DbConnection con, ConnectionSettings settings)
            : base(con, settings)
        {
        }

        /*
        protected DbTransaction BeginTransaction(DbConnection con)
        {
            return con.BeginTransaction(IsolationLevel.ReadCommitted);
        }
         */

        /*
        protected override string dbValidCol(string col)
        {
            if (col.ToUpper() == "DATE")
            {
                return "\"" + col + "\"";
            }
            else
            {
                return base.dbValidCol(col);
            }
        }
         */

        protected override string dbValidString(DateTime dt)
        {
            if (dt.ToString("HHmmss").Equals("000000"))
            {
                return "to_date('" + dt.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
            }
            return "to_date('" + dt.ToString("yyyy-MM-dd HH:mm:ss") + "', 'YYYY-MM-DD HH24:MI:SS')";
        }

        protected override object getValidObject(object obj, Type type)
        {
            if (obj.GetType().ToString().Contains("System.Decimal"))
            {
                try
                {
                    if (type.ToString().Contains("System.Int16"))
                        return Convert.ToInt16(obj);
                    if (type.ToString().Contains("System.Int32"))
                        return Convert.ToInt32(obj);
                    if (type.ToString().Contains("System.Int64"))
                        return Convert.ToInt64(obj);
                }
                catch
                {
                    return obj;
                }
            }
            return obj;
        }
    }
}
