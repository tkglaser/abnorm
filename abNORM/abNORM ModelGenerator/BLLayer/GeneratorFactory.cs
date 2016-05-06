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
using System.Text;
using System.Data.Common;

namespace abNORM.ModelGenerator.BLLayer
{
    class GeneratorFactory
    {
        public static IGenerator createGenerator(
            ConnectionFactory.DBType type,
            string host, 
            string database, 
            string user, 
            string pwd)
        {
            DbConnection con = null;
            switch (type)
            {
                case ConnectionFactory.DBType.Ingres:
                    con = new Ingres.Client.IngresConnection(
                        ConnectionFactory.createConnectionString(
                            ConnectionFactory.DBType.Ingres,
                            host,
                            database,
                            user,
                            pwd));
                    //return new IngresGenerator();
                    return new SchemaGenerator(con);
                case ConnectionFactory.DBType.Postgres:
                    con = new Postgres.Client.PostgresConnection(
                        ConnectionFactory.createConnectionString(
                            ConnectionFactory.DBType.Postgres,
                            host,
                            database,
                            user,
                            pwd));
                    //return new IngresGenerator();
                    return new SchemaGenerator(con);

                case ConnectionFactory.DBType.Oracle:
                    con = new Oracle.Client.OracleConnection(
                        ConnectionFactory.createConnectionString(
                            ConnectionFactory.DBType.Oracle,
                            host,
                            database,
                            user,
                            pwd));
                    return new SchemaGenerator(con);
            }
            throw new Exception("Unknown Type");
        }
    }
}
