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
using System.Xml.Serialization;
using System.Xml.XPath;

namespace abNORM.Plugins.Other.XML
{
    class XMLDataProvider : IDataProvider
    {
        FileStream file;

        public XMLDataProvider(string fileName)
        {
            this.file = new FileStream(fileName, FileMode.OpenOrCreate);
        }

        protected string xmlValidString(object o)
        {
            return o.ToString();
        }

        protected string eq(object o)
        {
            return " = " + xmlValidString(o);
        }

        protected string neq(object o)
        {
            return " != " + xmlValidString(o);
        }

        protected string CriterionToXPath(Criterion crit)
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
                    andStr += CriterionToXPath(c);
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
                    orStr += CriterionToXPath(c);
                    orStr += " OR ";
                }
                orStr = orStr.Substring(0, orStr.Length - 4);
                orStr += ")";
                return orStr;
            }

            if (crit is CriterionEqual)
            {
                CriterionEqual critEq = (CriterionEqual)crit;
                return critEq.a + eq(critEq.b) + xmlValidString(critEq.b);
            }

            if (crit is CriterionNotEqual)
            {
                CriterionNotEqual eq = (CriterionNotEqual)crit;
                return eq.a + neq(eq.b) + xmlValidString(eq.b);
            }

            if (crit is CriterionEqualsInsensitive)
            {
                CriterionEqualsInsensitive eq = (CriterionEqualsInsensitive)crit;
                return "UPPER ( " + eq.a + " ) = UPPER ( " + xmlValidString(eq.b) + " ) ";
            }

            if (crit is CriterionGreaterOrEqual)
            {
                CriterionGreaterOrEqual eq = (CriterionGreaterOrEqual)crit;
                return eq.a + " >= " + xmlValidString(eq.b);
            }

            if (crit is CriterionGreaterThan)
            {
                CriterionGreaterThan eq = (CriterionGreaterThan)crit;
                return eq.a + " > " + xmlValidString(eq.b);
            }

            if (crit is CriterionLessOrEqual)
            {
                CriterionLessOrEqual eq = (CriterionLessOrEqual)crit;
                return eq.a + " <= " + xmlValidString(eq.b);
            }

            if (crit is CriterionLessThan)
            {
                CriterionLessThan eq = (CriterionLessThan)crit;
                return eq.a + " < " + xmlValidString(eq.b);
            }
            if (crit is CriterionLike)
            {
                CriterionLike eq = (CriterionLike)crit;
                return eq.a + " LIKE " + xmlValidString(eq.b);
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

        public List<object> Select(Type t, Criterion crit, List<string> orderBy)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int Update(Type t, Dictionary<string, object> fields, Criterion crit)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int Delete(Type t, Criterion crit)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Insert(object dat)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void RaiseDBEvent(string eventname, string eventtext)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void EnableAutoCommit()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public long Select(string command)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int ExecuteSql(string sql)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CreateTable(Type t)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CreateSequences(Type t)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CreatePKsForTable(Type t)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CreateForeignKey(ForeignKeyDescriptor fk)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DropTable(Type t)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DropSequences(Type t)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Commit()
        {
            file.Flush();
            file.Close();
        }

        public void Rollback()
        {
            throw new Exception("Transaction handling is not yet supported for XML.");
        }
    }
}
