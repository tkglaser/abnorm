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
    class IngresGenerator
    {
        [TableName("iitables")]
        class IITable
        {
            public string table_name;
            public string table_owner;
            public string table_type;
        }

        [TableName("iicolumns")]
        class IIColumn
        {
            public string table_name;
            public string table_owner;
            public string column_name;
            public string column_datatype;
            public Int32 column_length;
            public string column_nulls;
            public Int32 key_sequence;
            public Int32 column_sequence;
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
                typeof(IITable),
                crit);

            List<TableDefinition> defs = new List<TableDefinition>();

            foreach (IITable t in tables)
            {
                TableDefinition def = new TableDefinition();
                def.table = map(t);

                def.columns = trx.Load(
                    typeof(IIColumn),
                    new CriterionEqual("table_name", t.table_name),
                    "column_sequence")
                    .ConvertAll<Column>(
                        new Converter<object, Column>(
                            delegate(object o) { return map((IIColumn)o); }))
                    .ToArray();

                defs.Add(def);
            }

            trx.Commit();

            return defs.ToArray();
        }

        private Table map(IITable tab)
        {
            Table t = new Table();
            t.name = tab.table_name.Trim();
            t.owner = tab.table_owner.Trim();
            t.type = tab.table_type.Trim();
            return t;
        }

        private Column map(IIColumn col)
        {
            Column c = new Column();

            c.name = col.column_name.Trim();

            c.isPK = false;
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
                case "INGRESDATE":
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
                            throw new Exception("Encountered an integer with size=" 
                                + col.column_length + ". That is not supported.");
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
                    throw new Exception("Unknown datatype: " + col.column_datatype.Trim());
            }

            return c;
        }
    }
}
