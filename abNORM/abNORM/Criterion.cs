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
    /// This is the basiv class for all criteria
    /// </summary>
    public class Criterion
    {
    }

    /// <summary>
    /// This criterion ANDs other criteria together
    /// </summary>
    public class CriterionAnd : Criterion
    {
        public Criterion[] crits;

        public CriterionAnd(Criterion a, Criterion b)
        {
            List<Criterion> c = new List<Criterion>();
            c.Add(a);
            c.Add(b);
            this.crits = c.ToArray();
        }
        public CriterionAnd(Criterion[] crits)
        {
            this.crits = crits;
        }
    }

    /// <summary>
    /// This one ORs other criteria together
    /// </summary>
    public class CriterionOr : Criterion
    {
        public Criterion[] crits;

        public CriterionOr(Criterion a, Criterion b)
        {
            List<Criterion> c = new List<Criterion>();
            c.Add(a);
            c.Add(b);
            this.crits = c.ToArray();
        }
        public CriterionOr(Criterion[] crits)
        {
            this.crits = crits;
        }
    }

    /// <summary>
    /// Column = Value
    /// </summary>
    public class CriterionEqual : Criterion
    {
        public CriterionEqual(string a, object b)
        {
            this.a = a;
            this.b = b;
        }

        public string a;
        public object b;
    }

    /// <summary>
    /// Column != Value
    /// </summary>
    public class CriterionNotEqual : Criterion
    {
        public CriterionNotEqual(string a, object b)
        {
            this.a = a;
            this.b = b;
        }

        public string a;
        public object b;
    }

    /// <summary>
    /// upper(Column) = upper(Value)
    /// </summary>
    public class CriterionEqualsInsensitive : Criterion
    {
        public CriterionEqualsInsensitive(string a, string b)
        {
            this.a = a;
            this.b = b;
        }

        public string a;
        public string b;
    }

    /// <summary>
    /// Column < Value
    /// </summary>
    public class CriterionLessThan : Criterion
    {
        public CriterionLessThan(string a, object b)
        {
            this.a = a;
            this.b = b;
        }
        public string a;
        public object b;
    }

    /// <summary>
    /// Column <= Value
    /// </summary>
    public class CriterionLessOrEqual : Criterion
    {
        public CriterionLessOrEqual(string a, object b)
        {
            this.a = a;
            this.b = b;
        }
        public string a;
        public object b;
    }

    /// <summary>
    /// Column > Value
    /// </summary>
    public class CriterionGreaterThan : Criterion
    {
        public CriterionGreaterThan(string a, object b)
        {
            this.a = a;
            this.b = b;
        }
        public string a;
        public object b;
    }

    /// <summary>
    /// Column >= Value
    /// </summary>
    public class CriterionGreaterOrEqual : Criterion
    {
        public CriterionGreaterOrEqual(string a, object b)
        {
            this.a = a;
            this.b = b;
        }
        public string a;
        public object b;
    }

    /// <summary>
    /// Column LIKE Value
    /// </summary>
    public class CriterionLike : Criterion
    {
        public CriterionLike(string a, object b)
        {
            this.a = a;
            this.b = b;
        }
        public string a;
        public object b;
    }

    /// <summary>
    /// This is a custom criterion. Just use it, 
    /// if you absolutely have to.
    /// </summary>
    public class CriterionText : Criterion
    {
        public CriterionText(string text)
        {
            this.text = text;
        }
        public string text;
    }

    /// <summary>
    /// Column is null
    /// </summary>
    public class CriterionIsNull : Criterion
    {
        public CriterionIsNull(string a)
        {
            this.a = a;
        }
        public string a;
    }
}
