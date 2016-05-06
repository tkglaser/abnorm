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

namespace abNORM.Example
{
    /// <summary>
    /// This example shows the use of Criterions
    /// </summary>
    class Criterions
    {
        class Show
        {
            [PrimaryKey]
            public int show_id;
            [DBType("varchar(50)")]
            public string show_name;
            public DateTime first_aired;
        }

        public static void Run()
        {
            /// First, generate the datamodel into the database.
            /// Details on these commands -> see the Simple.cs
            IConnection con = ConnectionFactory.getConnection(
                ConnectionFactory.DBType.Ingres,
                "<connectionstring>");
            ITransaction trx = con.BeginTransaction();
            List<Type> dataModel = new List<Type>();
            dataModel.Add(typeof(Show));
            trx.CreateDataBaseSchema(dataModel);
            trx.Commit();

            trx = con.BeginTransaction();
            Show show = new Show();
            show.show_id = 1;
            show.show_name = "Heroes";
            show.first_aired = new DateTime(2006, 9, 25);
            trx.Save(show);

            show = new Show();
            show.show_id = 2;
            show.show_name = "Lost";
            show.first_aired = new DateTime(2004, 9, 22);
            trx.Save(show);
            trx.Commit();

            trx = con.BeginTransaction();
            /// First, lets load a show named Lost
            /// There should be one result.
            List<object> result = trx.Load(
                typeof(Show),
                new CriterionEqual("show_name", "Lost"));

            /// Now, lets load the show with the primary key 2
            /// Note the special syntax.
            Show lostShow = (Show)trx.Load(typeof(Show), 2);

            trx.Commit();
        }
    }
}
