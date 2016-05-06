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

namespace abNORM.ModelGenerator.BLLayer
{
    class PostgresGenerator 
    {
        [CustomSelect("select oid, tablename from pg_tables t, pg_class c")]
        class PGTable
        {
            public int oid;
            public string tablename;
        }

        [CustomSelect("select attname, typname, attlen, atttypmod, " + 
            "attnotnull from pg_attribute a, pg_type t")]
        class PGColumn
        {
            public string attname;
            public string typname;
            public int attlen;
            public int atttypmod;
            public string attnotnull;
        }

        [TableName("pg_index")]
        class PGIndex
        {
            public string indkey;
        }


        public TableDefinition[] generate(
            string host,
            string database,
            string tableOwner,
            string user,
            string pwd,
            bool generateAll)
        {
            OL.setConnection(host, database, user, pwd);

            ITransaction trx = OL.Instance.BeginTransaction();

            Criterion crit = new CriterionEqual("table_owner", tableOwner);
            if (!generateAll)
            {
                crit = new CriterionAnd(
                    crit,
                    new CriterionEqual("table_type", "T"));
            }

            List<object> tables = trx.Load(
                typeof(PGTable),
                new CriterionAnd(
                  new CriterionEqual("t.schemaname", "public"),
                  new CriterionText("t.tablename=c.relname")));

            List<TableDefinition> defs = new List<TableDefinition>();

            foreach (PGTable t in tables)
            {
                TableDefinition def = new TableDefinition();
                def.table = map(t);
                /*
                List<object> idx = trx.Load(
                    typeof(PGIndex),
                    new CriterionAnd(
                        new CriterionEqual("indrelid", t.oid),
                        new CriterionEqual("indisprimary", "t")));
                */
                def.columns = trx.Load(
                    typeof(PGColumn),
                    new CriterionAnd(
                        new CriterionGreaterThan("attnum", 0),
                        new CriterionEqual("t.oid", t.oid)),
                    "attnum")
                    .ConvertAll<Column>(
                        new Converter<object, Column>(
                            delegate(object o) { return map((PGColumn)o); }))
                    .ToArray();

                defs.Add(def);
            }

            trx.Commit();

            return defs.ToArray();
        }

        private Table map(PGTable tab)
        {
            Table t = new Table();
            t.name = tab.tablename.Trim();
            t.owner = "public";
            t.type = "";
            return t;
        }

        private Column map(PGColumn col)
        {
            Column c = new Column();

            c.name = col.attname.Trim();

            c.isPK = false;
            /*
            if (col.key_sequence != 0)
            {
                c.isPK = true;
            }

            switch (col.column_datatype.Trim())
            {
                case "C":
                case "CHAR":
                case "VARCHAR":
                case "TEXT":
                    c.datatype = "string";
                    break;

                case "DATE":
                    c.datatype = "DateTime";
                    if (col.column_nulls == "Y")
                    {
                        c.datatype += "?";
                    }
                    break;

                case "INTEGER":
                    switch (col.column_length)
                    {
                        case 1:
                        case 2:
                            c.datatype = "Int16";
                            break;
                        case 4:
                            c.datatype = "Int32";
                            break;
                        case 8:
                            c.datatype = "Int64";
                            break;
                        default:
                            throw new Exception("Hallo");
                    }
                    if (col.column_nulls == "Y")
                    {
                        c.datatype += "?";
                    }
                    break;

                case "FLOAT":
                    c.datatype = "Double";
                    if (col.column_nulls == "Y")
                    {
                        c.datatype += "?";
                    }
                    break;

                case "DECIMAL":
                    c.datatype = "Decimal";
                    if (col.column_nulls == "Y")
                    {
                        c.datatype += "?";
                    }
                    break;

                default:
                    throw new Exception("Weg");
            }
            */
            return c;
        }
    }
}
