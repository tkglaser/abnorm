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
using System.Data;
using System.Data.Common;

using Ingres.Client;

namespace abNORM.ModelGenerator.BLLayer
{
    class SchemaGenerator : IGenerator
    {
        DbConnection con;

        public SchemaGenerator(DbConnection con)
        {
            this.con = con;
        }

        public TableDefinition[] generate(
            string tableOwner, 
            bool generateAll)
        {
            con.Open();
            DataTable dt = con.GetSchema("Tables");
            List<string> tableNames = new List<string>();
            foreach (DataRow myField in dt.Rows)
            {
                if (con is Ingres.Client.IngresConnection)
                {
                    tableNames.Add(myField["TABLE_NAME"].ToString());
                }
                if (con is Oracle.Client.OracleConnection)
                {
                    if (myField["TYPE"].ToString() == "User")
                    {
                        if (myField["OWNER"].ToString().ToLower() == tableOwner.ToLower())
                        {
                            tableNames.Add(myField["TABLE_NAME"].ToString());
                        }
                    }
                }
                if (con is Postgres.Client.PostgresConnection)
                {
                    if ((myField["TABLE_SCHEMA"].ToString() == "public") &&
                        (myField["TABLE_TYPE"].ToString() == "BASE TABLE"))
                    {
                        tableNames.Add(myField["TABLE_NAME"].ToString());
                    }
                }
            }
            List<TableDefinition> tables = new List<TableDefinition>();
            foreach (string tableName in tableNames)
            {
                TableDefinition table = new TableDefinition();
                table.table = new Table();
                table.table.name = tableName;
                table.table.owner = tableOwner;  // TODO: This is a Hack. Replace with real owner
                DbCommand cmd = con.CreateCommand();
                cmd.CommandText = "select * from " + tableName;
                DbDataReader reader = null;
                if (con is Oracle.Client.OracleConnection)
                {
                    reader = cmd.ExecuteReader(CommandBehavior.KeyInfo);
                }
                else
                {
                    reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly);
                }
                DataTable schemaTable = reader.GetSchemaTable();
                reader.Close();
                List<Column> columns = new List<Column>();
                foreach (DataRow myField in schemaTable.Rows)
                {
                    Column column = new Column();
                    column.isPK = Convert.ToBoolean(myField["IsKey"]);
                    column.datatype = myField["DataType"].ToString().Replace("System.", "");
                    if (Convert.ToBoolean(myField["AllowDBNull"]))
                    {
                        if (!myField["DataType"].Equals(typeof(string)))
                        {
                            column.datatype += "?";
                        }
                    }
                    column.name = myField["ColumnName"].ToString();
                    columns.Add(column);
                }
                table.columns = columns.ToArray();
                tables.Add(table);
            }
            con.Close();
            return tables.ToArray();
        }
    }
}
