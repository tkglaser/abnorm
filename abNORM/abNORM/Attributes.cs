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

namespace abNORM
{
    /// <summary>
    /// Marks a primary key
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public sealed class PrimaryKey : System.Attribute
    {
    }

    /// <summary>
    /// Don't do any inserts into this column (useful for autoincrementing columns)
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public sealed class NoInsert : System.Attribute
    {
    }

    /// <summary>
    /// Marks a foreign key
    /// </summary>
    public sealed class ForeignKey : System.Attribute
    {
        private Type theReference;
        private string theAttributeName;

        /// <summary>
        /// Constructor for Foreignkey
        /// </summary>
        /// <param name="reference">The type of the referenced object</param>
        /// <param name="attributeName">The referenced column in the object</param>
        public ForeignKey(Type reference, string attributeName)
        {
            this.theReference = reference;
            this.theAttributeName = attributeName;
        }

        /// <summary>
        /// The type of the referenced object
        /// </summary>
        public Type reference
        {
            get
            {
                return theReference;
            }
        }

        /// <summary>
        /// The referenced column in the object
        /// </summary>
        public string attributeName
        {
            get
            {
                return theAttributeName;
            }
        }
    }

    /// <summary>
    /// Marks a Join Key
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public sealed class JoinKey : System.Attribute
    {
        private string theTableA;
        private string theTableB;

        /// <summary>
        /// Constructor for the join key
        /// </summary>
        /// <param name="tableA">First table</param>
        /// <param name="tableB">Second table</param>
        public JoinKey(string tableA, string tableB)
        {
            this.theTableA = tableA;
            this.theTableB = tableB;
        }

        /// <summary>
        /// First table
        /// </summary>
        public string tableA
        {
            get
            {
                return theTableA;
            }
        }

        /// <summary>
        /// Second table
        /// </summary>
        public string tableB
        {
            get
            {
                return theTableB;
            }
        }
    }

    /// <summary>
    /// Marks a different name of the attribute in the database. 
    /// This name is used when creating sql rather than the own
    /// attribute name.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class DBAlias : System.Attribute
    {
        protected string theAlias;

        public DBAlias(string theAlias)
        {
            this.theAlias = theAlias;
        }

        /// <summary>
        /// The alias name for use in the database
        /// </summary>
        public string alias
        {
            get
            {
                return theAlias;
            }
        }
    }

    /// <summary>
    /// Executes a custum select statement rather than generating one.
    /// This is a little dangerous, since the user needs to make sure,
    /// that the number and types of attributes are correct.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomSelect : System.Attribute
    {
        protected string theSelect;

        public CustomSelect(string theSelect)
        {
            this.theSelect = theSelect;
        }

        /// <summary>
        /// The custom select statement
        /// </summary>
        public string select
        {
            get
            {
                return theSelect;
            }
        }
    }

    /// <summary>
    /// Specifies the tablename for this object. Is a class has no 
    /// tablename attribute, a table with the classname is assumed.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class TableName : System.Attribute
    {
        protected string tableName;
        protected string theShortName;

        public TableName(string tableName)
        {
            this.tableName = tableName;
            this.theShortName = null;
        }

        public TableName(string tableName, string shortName)
        {
            this.tableName = tableName;
            this.theShortName = shortName;
        }

        /// <summary>
        /// The name of the table to be used.
        /// </summary>
        public string name
        {
            get
            {
                return tableName;
            }
        }

        /// <summary>
        /// The optional short name for the generation of joins.
        /// </summary>
        public string shortName
        {
            get
            {
                return theShortName;
            }
        }
    }

    /// <summary>
    /// Specifies a sequence.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class Sequence : System.Attribute
    {
        protected string seqName;
        protected string seqCreateAttribs;

        public Sequence(string seqName, string createAttribs)
        {
            this.seqName = seqName;
            this.seqCreateAttribs = createAttribs;
        }

        /// <summary>
        /// The name of the sequence
        /// </summary>
        public string name
        {
            get
            {
                return seqName;
            }
        }

        /// <summary>
        /// The attributes to be given to the create sequence statement
        /// </summary>
        public string createAttribs
        {
            get
            {
                return seqCreateAttribs;
            }
        }
    }

    /// <summary>
    /// Specifies the DBType of a class. This is only needed, when a 
    /// datamodel is to be created.
    /// </summary>
    public class DBType : System.Attribute
    {
        protected string dbType;

        public DBType(string type)
        {
            this.dbType = type;
        }

        /// <summary>
        /// The sql type of the attribute
        /// </summary>
        public string type
        {
            get
            {
                return this.dbType;
            }
        }
    }
}
