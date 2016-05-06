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
 
#define USE_GMT_HACK

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Reflection;

namespace abNORM.Plugins.SQLBased.Ingres
{
    class IngresDataProvider : SQLDataProvider
    {
        internal IngresDataProvider(DbConnection con, ConnectionSettings settings)
            : base(con, settings)
        {
        }

        protected override string dbValidString(DateTime dt)
        {
            if (dt.Equals(DateTime.MinValue))
            {
                return "date('')";
            }

            if (dt.ToString("HHmmss").Equals("000000"))
            {
                return "DATE('" + dt.ToString("yyyy-MM-dd") + "')";
            }
            return "date_gmt('"
                + dt.ToUniversalTime().ToString("yyyy_MM_dd HH:mm:ss")
                + " GMT')";
        }

        protected override string reflectSelectFields(Type dat)
        {
            string values = "";

            System.Reflection.FieldInfo[] fieldInfo = dat.GetFields();

            foreach (System.Reflection.FieldInfo fi in fieldInfo)
            {
                object[] ca = fi.GetCustomAttributes(typeof(JoinKey), true);
                if (ca.Length > 0)
                {
                    JoinKey jk = (JoinKey)ca[0];
                    values += jk.tableA + ".";
                }

                ca = fi.GetCustomAttributes(typeof(DBAlias), true);
                if (ca.Length > 0)
                {
                    DBAlias a = (DBAlias)ca[0];
                    values += a.alias + ", ";
                }
                else
                {
#if USE_GMT_HACK
                    if (fi.FieldType.ToString().Contains("System.DateTime"))
                    {
                        values += "date_gmt(" + fi.Name + ") as " + fi.Name + ", ";
                    }
                    else
#endif
                    {
                        values += fi.Name + ", ";
                    }
                }
            }

            values = values.Substring(0, values.Length - 2);
            return values;
        }


#if USE_GMT_HACK
        protected DateTime parseGMT(string gmtTime)
        {
            if (string.IsNullOrEmpty(gmtTime.Trim()))
            {
                return DateTime.MinValue;
            }
            string year = gmtTime.Substring(0, 4);
            string month = gmtTime.Substring(5, 2);
            string day = gmtTime.Substring(8, 2);

            string time = gmtTime.Substring(11, 8);

            DateTime dt;

            if (time.Equals("00:00:00"))
            {
                dt = new DateTime(
                    int.Parse(year),
                    int.Parse(month),
                    int.Parse(day), 0, 0, 0, DateTimeKind.Local);
            }
            else
            {
                dt = new DateTime(
                    int.Parse(year),
                    int.Parse(month),
                    int.Parse(day), 0, 0, 0, DateTimeKind.Utc);

                dt = dt.Add(TimeSpan.Parse(time));

                dt = dt.ToLocalTime();
            }

            return dt;
        }
#endif
        protected override object getValidObject(object obj, Type type)
        {
#if USE_GMT_HACK
            if (type.ToString().Contains("System.DateTime"))
            {
                DateTime res = parseGMT(obj.ToString());
                if (res.Equals(DateTime.MinValue))
                {
                    return null;
                }
                return res;
            }
            else
#endif
            if (type.ToString().Contains("System.String"))
            {
                return obj.ToString().Replace("\0", "");
            }
            else
            {
                return obj;
            }
        }
    }
}
