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
using System.Reflection;

namespace abNORM.Plugins.SQLBased
{
    class SQLDataProvider : IDataProvider
    {
        protected DbConnection con;
        protected DbTransaction trx;
        protected ConnectionSettings settings;
        protected Dictionary<Type, string> createTypes;

        public SQLDataProvider(DbConnection con, ConnectionSettings settings)
        {
            createTypes = new Dictionary<Type, string>();
            createTypes.Add(typeof(Int32), "integer not null");
            createTypes.Add(typeof(DateTime), "date not null");
            createTypes.Add(typeof(Single), "float not null");

            createTypes.Add(typeof(Int32?), "integer");
            createTypes.Add(typeof(DateTime?), "date");
            createTypes.Add(typeof(Single?), "float");

            this.settings = settings;
            this.con = con;
            trx = con.BeginTransaction(IsolationLevel.ReadUncommitted);
        }

        #region dbValidString Methoden

        protected virtual string dbValidString(object obj)
        {
            if (obj == null)
                return "null";

            switch (obj.GetType().ToString())
            {
                case "System.String":
                    return dbValidString(obj.ToString());
                case "System.Int32":
                    return dbValidString(Convert.ToInt32(obj));
                case "System.Nullable`1[System.Int32]":
                    return dbValidString((Int32?)obj);
                case "System.Decimal":
                    return dbValidString(Convert.ToDecimal(obj));
                case "System.Nullable`1[System.Decimal]":
                    return dbValidString((Decimal?)obj);
                case "System.Int16":
                    return dbValidString(Convert.ToInt16(obj));
                case "System.Nullable`1[System.Int16]":
                    return dbValidString((Int16?)obj);
                case "System.DateTime":
                    return dbValidString(Convert.ToDateTime(obj));
                case "System.Nullable`1[System.DateTime]":
                    return dbValidString((DateTime?)obj);
                case "System.Double":
                    return dbValidString(Convert.ToDouble(obj));
                case "System.Nullable`1[System.Double]":
                    return dbValidString((Double?)obj);
                case "System.Boolean":
                    return dbValidString((bool)obj);
                case "System.Nullable`1[System.Boolean]":
                    return dbValidString((bool?)obj);
                default:
                    throw new Exception("Invalid Type for dbValidString");
            }
        }

        protected virtual string dbValidString(DateTime dt)
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

        protected virtual string dbValidString(float f)
        {
            return f.ToString().Replace(',', settings.decimalChar);
        }

        protected virtual string dbValidString(double f)
        {
            return f.ToString().Replace(',', settings.decimalChar);
        }

        protected virtual string dbValidString(int i)
        {
            return i.ToString();
        }

        protected virtual string dbValidString(decimal d)
        {
            return d.ToString();
        }

        protected virtual string dbValidString(long l)
        {
            return l.ToString();
        }

        protected virtual string dbValidString(bool b)
        {
            return b.ToString();
        }

        protected virtual string dbValidString(bool? b)
        {
            if (b.HasValue)
            {
                return dbValidString(b.Value);
            }
            return "null";
        }

        protected virtual string dbValidString(string s)
        {
            if (s == null)
            {
                return "null";
            }
            else
            {
                return "'" + s.Replace("'", "''") + "'";
            }
        }

        protected virtual string dbValidString(DateTime? d)
        {
            if (d.HasValue)
            {
                return dbValidString(d.Value);
            }
            else
            {
                return "null";
            }
        }

        protected virtual string dbValidString(int? i)
        {
            if (i.HasValue)
            {
                return dbValidString(i.Value);
            }
            else
            {
                return "null";
            }
        }

        protected virtual string dbValidString(decimal? i)
        {
            if (i.HasValue)
            {
                return dbValidString(i.Value);
            }
            else
            {
                return "null";
            }
        }

        protected virtual string dbValidString(double? i)
        {
            if (i.HasValue)
            {
                return dbValidString(i.Value);
            }
            else
            {
                return "null";
            }
        }

        #endregion

        protected string eq(object dat)
        {
            if (dat == null)
                return " is ";
            return " = ";
        }

        protected string neq(object dat)
        {
            if (dat == null)
                return " is not ";
            return " != ";
        }

        protected string reflectTableName(Type dat)
        {
            object[] ca = dat.GetCustomAttributes(typeof(TableName), true);
            if (ca.Length > 0)
            {
                string tablenames = "";
                foreach (TableName tn in ca)
                {
                    tablenames += tn.name;
                    if (tn.shortName != null)
                    {
                        tablenames += " " + tn.shortName;
                    }
                    tablenames += ", ";
                }
                return tablenames.Substring(0, tablenames.Length - 2);
            }
            return dat.Name;
        }

        protected string reflectCustomSelect(Type dat)
        {
            string select = "";
            object[] ca = dat.GetCustomAttributes(typeof(CustomSelect), true);

            if (ca.Length > 0)
            {
                foreach (CustomSelect cs in ca)
                {
                    select = cs.select;
                }
            }
            return select;
        }

        protected virtual string reflectSelectFields(Type dat)
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
                    values += fi.Name + ", ";
                }
            }

            values = values.Substring(0, values.Length - 2);
            return values;
        }

        protected object clone(object dat)
        {
            object theClone = dat.GetType().GetConstructor(Type.EmptyTypes).Invoke(new object[] { });
            System.Reflection.FieldInfo[] fieldInfo = dat.GetType().GetFields();

            foreach (System.Reflection.FieldInfo fi in fieldInfo)
            {
                fi.SetValue(theClone, fi.GetValue(dat));
            }
            return theClone;
        }

        protected string[] reflectPKNames(Type dat)
        {
            List<string> pkList = new List<string>();

            foreach (FieldInfo fi in dat.GetFields())
            {
                if (fi.GetCustomAttributes(typeof(PrimaryKey), true).Length > 0)
                {
                    object[] ca = fi.GetCustomAttributes(typeof(JoinKey), true);
                    if (ca.Length > 0)
                    {
                        JoinKey jk = (JoinKey)ca[0];
                        pkList.Add(jk.tableA + "." + fi.Name);
                        continue;
                    }
                    ca = fi.GetCustomAttributes(typeof(DBAlias), true);
                    if (ca.Length > 0)
                    {
                        DBAlias a = (DBAlias)ca[0];
                        pkList.Add(a.alias);
                        continue;
                    }
                    pkList.Add(fi.Name);
                }
            }
            return pkList.ToArray();
        }

        protected string CriterionToString(Criterion crit)
        {
            if (crit == null)
                throw new ArgumentNullException("crit");

            if (crit is CriterionAnd)
            {
                CriterionAnd and = (CriterionAnd)crit;
                string andStr = "";
                andStr += "(";
                foreach (Criterion c in and.crits)
                {
                    andStr += CriterionToString(c);
                    andStr += " AND ";
                }
                andStr = andStr.Substring(0, andStr.Length - 5);
                andStr += ")";
                return andStr;
            }

            if (crit is CriterionOr)
            {
                CriterionOr or = (CriterionOr)crit;
                string orStr = "";
                orStr += "(";
                foreach (Criterion c in or.crits)
                {
                    orStr += CriterionToString(c);
                    orStr += " OR ";
                }
                orStr = orStr.Substring(0, orStr.Length - 4);
                orStr += ")";
                return orStr;
            }

            if (crit is CriterionEqual)
            {
                CriterionEqual critEq = (CriterionEqual)crit;
                return critEq.a + eq(critEq.b) + dbValidString(critEq.b);
            }

            if (crit is CriterionNotEqual)
            {
                CriterionNotEqual eq = (CriterionNotEqual)crit;
                return eq.a + neq(eq.b) + dbValidString(eq.b);
            }

            if (crit is CriterionEqualsInsensitive)
            {
                CriterionEqualsInsensitive eq = (CriterionEqualsInsensitive)crit;
                return "UPPER ( " + eq.a + " ) = UPPER ( " + dbValidString(eq.b) + " ) ";
            }

            if (crit is CriterionGreaterOrEqual)
            {
                CriterionGreaterOrEqual eq = (CriterionGreaterOrEqual)crit;
                return eq.a + " >= " + dbValidString(eq.b);
            }

            if (crit is CriterionGreaterThan)
            {
                CriterionGreaterThan eq = (CriterionGreaterThan)crit;
                return eq.a + " > " + dbValidString(eq.b);
            }

            if (crit is CriterionLessOrEqual)
            {
                CriterionLessOrEqual eq = (CriterionLessOrEqual)crit;
                return eq.a + " <= " + dbValidString(eq.b);
            }

            if (crit is CriterionLessThan)
            {
                CriterionLessThan eq = (CriterionLessThan)crit;
                return eq.a + " < " + dbValidString(eq.b);
            }
            if (crit is CriterionLike)
            {
                CriterionLike eq = (CriterionLike)crit;
                return eq.a + " LIKE " + dbValidString(eq.b);
            }
            if (crit is CriterionText)
            {
                return ((CriterionText)crit).text;
            }
            if (crit is CriterionIsNull)
            {
                return ((CriterionIsNull)crit).a + " IS NULL";
            }
            throw new Exception("Unknown Criterion!");
        }

        protected string reflectJoinStatement(Type dat)
        {
            string joinString = "";

            FieldInfo[] fieldInfo = dat.GetFields();

            foreach (FieldInfo fi in fieldInfo)
            {
                object[] objLst = fi.GetCustomAttributes(typeof(JoinKey), true);
                if (objLst.Length > 0)
                {
                    JoinKey jk = (JoinKey)objLst[0];
                    joinString += jk.tableA + "." + fi.Name + " = " +
                        jk.tableB + "." + fi.Name + " AND ";
                }
            }
            if (joinString != "")
            {
                joinString = joinString.Substring(0, joinString.Length - 5);
            }
            return joinString;
        }

        public List<object> Select(Type t, Criterion crit, List<string> orderBy)
        {
            DbCommand cmd = con.CreateCommand();
            cmd.Transaction = trx;
            try
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = reflectCustomSelect(t);
                if (cmd.CommandText == "")
                {
                    cmd.CommandText =
                        "SELECT " +
                            reflectSelectFields(t) + " " +
                        "FROM " +
                            reflectTableName(t);
                }

                string joinString = reflectJoinStatement(t);

                if ((crit != null) || (joinString != ""))
                {
                    cmd.CommandText += " WHERE ";
                    if (crit != null)
                    {
                        cmd.CommandText += CriterionToString(crit) + " AND ";
                    }
                    if (joinString != "")
                    {
                        cmd.CommandText += joinString + " AND ";
                    }
                    cmd.CommandText = cmd.CommandText.Substring(0, cmd.CommandText.Length - 5);
                }

                if (orderBy != null)
                {
                    if (orderBy.Count > 0)
                    {
                        cmd.CommandText += " ORDER BY ";
                        foreach (string ord in orderBy)
                        {
                            cmd.CommandText += ord + ", ";
                        }
                        cmd.CommandText = cmd.CommandText.Substring(0, cmd.CommandText.Length - 2);
                    }
                }


                DbDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleResult);

                List<object> entList = new List<object>();

                while (reader.Read())
                {
                    object ent = createEntity(t);
                    reflectRead(ref ent, reader);
                    entList.Add(ent);
                }

                reader.Close();

                return entList;
            }
            catch (Exception)
            {
                if (cmd.Transaction != null)
                    cmd.Transaction.Rollback();
                con.Close();
                con.Dispose();
                throw;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        protected object createEntity(Type t)
        {
            object theObject
                = t.GetConstructor(Type.EmptyTypes).Invoke(new object[] { });
            return theObject;
        }

        protected virtual object getValidObject(object obj, Type type)
        {
            return obj;
        }

        protected virtual void reflectRead(ref object dat, DbDataReader reader)
        {
            System.Reflection.FieldInfo[] fieldInfo = dat.GetType().GetFields();

            int i = 0;

            bool namedCols;
            try
            {
                // Manchmal wirft der Treiber eine Exception.
                // ist wohl ein Bug
                namedCols = (reader.GetName(0) != "col1");
            }
            catch
            {
                namedCols = false;
            }

            foreach (System.Reflection.FieldInfo fi in fieldInfo)
            {
                if (namedCols)
                {
                    if (i < reader.FieldCount)
                    {
                        if (!fi.Name.ToUpper().Equals(reader.GetName(i).ToUpper()))
                        {
                            i = reader.GetOrdinal(fi.Name);
                        }
                    }
                    else
                    {
                        i = reader.GetOrdinal(fi.Name);
                    }
                }

                if (settings.typeChecking)
                {
                    string fieldType = fi.FieldType.ToString();
                    string readerType = reader.GetFieldType(i).ToString();

                    if (reader.IsDBNull(i) && fieldType.Contains("System.Nullable"))
                    {
                    }
                    else
                    {
                        fieldType = fieldType.Replace("System.Nullable`1[", "");
                        fieldType = fieldType.Replace("]", "");

                        if (!fieldType.Equals(readerType))
                        {
                            object val = reader.GetValue(i);
                            throw new Exception("Fieldtype mismatch." +
                                "\r\nEntity: " + dat.GetType().ToString() +
                                "\r\nAttribute: " + fi.Name +
                                "\r\nObjectType: " + fieldType +
                                "\r\nReaderType: " + readerType +
                                "\r\nReaderValue: " + val.ToString() +
                                "\r\nOrdinal: " + i.ToString() +
                                "\r\nGetOrdinal: " + reader.GetOrdinal(fi.Name).ToString() +
                                "\r\nGetName: " + reader.GetName(i));
                        }
                    }
                }

                if (reader.IsDBNull(i))
                {
                    fi.SetValue(dat, null);
                }
                else
                {
                    fi.SetValue(dat, getValidObject(reader.GetValue(i), fi.FieldType));
                }
                i++;
            }
        }

        protected virtual string getDbType(Type t)
        {
            if (!createTypes.ContainsKey(t))
            {
                throw new Exception(t.ToString() + " cannot be created automatically. Use DbType");
            }
            return createTypes[t];
        }

        protected virtual string reflectCreateTableFields(Type dat)
        {
            string values = "";

            foreach (FieldInfo fi in dat.GetFields())
            {
                values += fi.Name + " ";
                object[] ca = fi.GetCustomAttributes(typeof(DBType), true);
                if (ca.Length > 0)
                {
                    DBType t = (DBType)ca[0];
                    values += t.type;
                }
                else
                {
                    values += getDbType(fi.FieldType);
                }
                values += ", ";
            }

            values = values.Substring(0, values.Length - 2);
            return values;
        }

        public int Update(Type t, Dictionary<string, object> fields, Criterion crit)
        {
            DbCommand cmd = con.CreateCommand();
            cmd.Transaction = trx;
            try
            {
                string sqlFields = "";

                foreach (KeyValuePair<string, object> f in fields)
                {
                    sqlFields += f.Key + " = " + dbValidString(f.Value) + ", ";
                }

                sqlFields = sqlFields.Substring(0, sqlFields.Length - 2);

                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    "UPDATE " +
                        reflectTableName(t) + " " +
                    "SET " +
                        sqlFields;

                if (crit != null)
                {
                    cmd.CommandText += " WHERE " + CriterionToString(crit);
                }

                if (settings.sqlLog != null)
                {
                    settings.sqlLog.WriteLine("== START " +
                        DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ===");
                    settings.sqlLog.WriteLine(cmd.CommandText);
                    settings.sqlLog.WriteLine("== END =========================");
                    settings.sqlLog.Flush();
                }

                int i = cmd.ExecuteNonQuery();

                return i;
            }
            catch (Exception)
            {
                if (cmd.Transaction != null)
                    cmd.Transaction.Rollback();
                con.Close();
                con.Dispose();
                throw;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        public int Delete(Type t, Criterion crit)
        {
            DbCommand cmd = con.CreateCommand();
            cmd.Transaction = trx;
            try
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    "DELETE FROM " +
                        reflectTableName(t);

                if (crit != null)
                {
                    cmd.CommandText += " WHERE ";
                    cmd.CommandText += CriterionToString(crit);
                }

                if (settings.sqlLog != null)
                {
                    settings.sqlLog.WriteLine("== START " +
                        DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ===");
                    settings.sqlLog.WriteLine(cmd.CommandText);
                    settings.sqlLog.WriteLine("== END =========================");
                    settings.sqlLog.Flush();
                }

                int i = cmd.ExecuteNonQuery();

                return i;
            }
            catch (Exception)
            {
                if (cmd.Transaction != null)
                    cmd.Transaction.Rollback();
                con.Close();
                con.Dispose();
                throw;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        public void Insert(object dat)
        {
            DbCommand cmd = con.CreateCommand();
            cmd.Transaction = trx;

            try
            {

                string names = "";
                string values = "";

                System.Reflection.FieldInfo[] fieldInfo = dat.GetType().GetFields();

                foreach (System.Reflection.FieldInfo fi in fieldInfo)
                {
                    if (fi.GetCustomAttributes(typeof(NoInsert), true).Length > 0)
                    {
                        continue;
                    }
                    names += fi.Name + ", ";
                    values += dbValidString(fi.GetValue(dat)) + ", ";
                }
                names = names.Substring(0, names.Length - 2);
                values = values.Substring(0, values.Length - 2);

                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    "INSERT INTO " +
                        reflectTableName(dat.GetType())
                        + "(" + names + ") VALUES (" + values + ")";

                if (settings.sqlLog != null)
                {
                    settings.sqlLog.WriteLine("== START " +
                        DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ===");
                    settings.sqlLog.WriteLine(cmd.CommandText);
                    settings.sqlLog.WriteLine("== END =========================");
                    settings.sqlLog.Flush();
                }

                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                if (cmd.Transaction != null)
                    cmd.Transaction.Rollback();
                con.Close();
                con.Dispose();
                throw e;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        protected void CreateSequenceForTable(string seqStr)
        {
            DbCommand cmd = con.CreateCommand();
            cmd.Transaction = trx;

            try
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    "CREATE SEQUENCE " +
                        seqStr;

                if (settings.sqlLog != null)
                {
                    settings.sqlLog.WriteLine("== START " +
                        DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ===");
                    settings.sqlLog.WriteLine(cmd.CommandText);
                    settings.sqlLog.WriteLine("== END =========================");
                    settings.sqlLog.Flush();
                }

                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                if (cmd.Transaction != null)
                    cmd.Transaction.Rollback();
                con.Close();
                con.Dispose();
                throw;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        public void CreateTable(Type t)
        {
            DbCommand cmd = con.CreateCommand();
            cmd.Transaction = trx;

            try
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    "CREATE TABLE " +
                        reflectTableName(t) +
                    "(" +
                        reflectCreateTableFields(t) +
                    ")";

                if (settings.sqlLog != null)
                {
                    settings.sqlLog.WriteLine("== START " +
                        DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ===");
                    settings.sqlLog.WriteLine(cmd.CommandText);
                    settings.sqlLog.WriteLine("== END =========================");
                    settings.sqlLog.Flush();
                }

                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                if (cmd.Transaction != null)
                    cmd.Transaction.Rollback();
                con.Close();
                con.Dispose();
                throw;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        public void CreateSequences(Type t)
        {
            object[] ca = t.GetCustomAttributes(typeof(Sequence), true);
            foreach (Sequence seq in ca)
            {
                CreateSequenceForTable(seq.name + " " + seq.createAttribs);
            }
        }

        public void CreatePKsForTable(Type t)
        {
            DbCommand cmd = con.CreateCommand();
            cmd.Transaction = trx;

            try
            {
                string[] pks = reflectPKNames(t);
                if (pks.Length == 0)
                    return;

                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    "ALTER TABLE " +
                        reflectTableName(t) +
                    " ADD PRIMARY KEY (";

                foreach (string pk in pks)
                {
                    cmd.CommandText += pk + ", ";
                }

                cmd.CommandText = cmd.CommandText.Substring(0, cmd.CommandText.Length - 2);
                cmd.CommandText += ")";

                if (settings.sqlLog != null)
                {
                    settings.sqlLog.WriteLine("== START " +
                        DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ===");
                    settings.sqlLog.WriteLine(cmd.CommandText);
                    settings.sqlLog.WriteLine("== END =========================");
                    settings.sqlLog.Flush();
                }

                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                if (cmd.Transaction != null)
                    cmd.Transaction.Rollback();
                con.Close();
                con.Dispose();
                throw;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        public void CreateForeignKey(ForeignKeyDescriptor fk)
        {
            DbCommand cmd = con.CreateCommand();
            cmd.Transaction = trx;

            try
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    "ALTER TABLE " +
                        reflectTableName(fk.sourceType) +
                    " ADD FOREIGN KEY (" + fk.sourceFieldName + ")" +
                    " REFERENCES " + reflectTableName(fk.targetType) +
                    "(" + fk.targetFieldName + ")";

                if (settings.sqlLog != null)
                {
                    settings.sqlLog.WriteLine("== START " +
                        DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ===");
                    settings.sqlLog.WriteLine(cmd.CommandText);
                    settings.sqlLog.WriteLine("== END =========================");
                    settings.sqlLog.Flush();
                }

                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                if (cmd.Transaction != null)
                    cmd.Transaction.Rollback();
                con.Close();
                con.Dispose();
                throw;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        public void DropTable(Type t)
        {
            DbCommand cmd = con.CreateCommand();
            cmd.Transaction = trx;

            try
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    "DROP TABLE " +
                        reflectTableName(t) + " CASCADE";

                if (settings.sqlLog != null)
                {
                    settings.sqlLog.WriteLine("== START " +
                        DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ===");
                    settings.sqlLog.WriteLine(cmd.CommandText);
                    settings.sqlLog.WriteLine("== END =========================");
                    settings.sqlLog.Flush();
                }

                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                // No exceptions, silently ignore all errors
            }
            finally
            {
                cmd.Dispose();
            }
        }

        protected void DropSequenceForTable(string seqName)
        {
            DbCommand cmd = con.CreateCommand();
            cmd.Transaction = trx;

            try
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    "DROP SEQUENCE " +
                        seqName;

                if (settings.sqlLog != null)
                {
                    settings.sqlLog.WriteLine("== START " +
                        DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ===");
                    settings.sqlLog.WriteLine(cmd.CommandText);
                    settings.sqlLog.WriteLine("== END =========================");
                    settings.sqlLog.Flush();
                }

                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                //Exceptions schlucken

                //if (cmd.Transaction != null)
                //cmd.Transaction.Rollback();
                //releaseConnection(cmd.Connection);
                //throw e;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        public void DropSequences(Type t)
        {
            object[] ca = t.GetCustomAttributes(typeof(Sequence), true);
            foreach (Sequence seq in ca)
            {
                DropSequenceForTable(seq.name + " " + seq.createAttribs);
            }
        }

        public void RaiseDBEvent(string eventname, string eventtext)
        {
            ExecuteSql("RAISE DBEVENT " + eventname + " '" + eventtext + "'");
        }

        public void EnableAutoCommit()
        {
            if (trx != null)
                trx.Commit();

            trx = null;
        }

        public long Select(string command)
        {
            DbCommand cmd = con.CreateCommand();
            cmd.Transaction = trx;

            try
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = command;

                if (settings.sqlLog != null)
                {
                    settings.sqlLog.WriteLine("== START " +
                        DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ===");
                    settings.sqlLog.WriteLine(cmd.CommandText);
                    settings.sqlLog.WriteLine("== END =========================");
                    settings.sqlLog.Flush();
                }

                DbDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleResult);

                if (!reader.HasRows)
                {
                    reader.Close();
                    throw new EntityNotFoundException();
                }

                reader.Read();

                long value = Convert.ToInt64(reader.GetValue(0));

                reader.Close();

                return value;
            }
            catch (Exception)
            {
                if (cmd.Transaction != null)
                    cmd.Transaction.Rollback();
                con.Close();
                con.Dispose();
                throw;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        public int ExecuteSql(string sql)
        {
            DbCommand cmd = con.CreateCommand();
            cmd.Transaction = trx;

            try
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                if (settings.sqlLog != null)
                {
                    settings.sqlLog.WriteLine("== START " +
                        DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ===");
                    settings.sqlLog.WriteLine(cmd.CommandText);
                    settings.sqlLog.WriteLine("== END =========================");
                    settings.sqlLog.Flush();
                }

                return cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                if (cmd.Transaction != null)
                    cmd.Transaction.Rollback();
                con.Close();
                con.Dispose();
                throw;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        public void Rollback()
        {
            try
            {
                if (trx != null)
                    trx.Rollback();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

        public void Commit()
        {
            try
            {
                if (trx != null)
                    trx.Commit();
            }
            catch (Exception)
            {
                if (trx != null)
                    trx.Rollback();
                throw;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
}
