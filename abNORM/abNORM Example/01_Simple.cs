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
    class Simple
    {
        /// <summary>
        /// The entire objectModel used in this exercise
        /// </summary>
        class Dude
        {
            [DBType("varchar(50)")]  //A little bit of annotation. Explained later. Sorry! :)
            public string name;

            public int age;
        }

        /// <summary>
        /// This simple example shows the basic usage of abNORM
        /// </summary>
        public static void Run()
        {
            /// First, we need a connection to the database.
            /// So, you need to choose, what kind of db you have
            /// and you need to provide a standard .NET Dataprovider
            /// connection string. See the Manuals of the specific
            /// Dataproviders for that.
            IConnection con = ConnectionFactory.getConnection(
                ConnectionFactory.DBType.Ingres,
                "<connectionstring>");

            /// Next, we have to start a transaction.
            /// The Transaction-Object contains all methods for
            /// data manipulation.
            ITransaction trx = con.BeginTransaction();

            /// Suppose, we have an empty database.
            /// First we need to create the datamodel.
            /// If you already have the datamodel in the database
            /// (you normaly would) you don't need to do this.
            List<Type> dataModel = new List<Type>();
            dataModel.Add(typeof(Dude));

            /// This command issues all the create tables
            /// and create sequences and foreign keys,
            /// we specified. In this case, it would generate
            /// and execute:
            /// CREATE TABLE Dude(name varchar(50));
            trx.CreateDataBaseSchema(dataModel);

            /// Now, fill some data into the database
            Dude dude = new Dude();
            dude.name = "SomeDude";
            dude.age = 30;

            /// Just throw the object into the database
            trx.Save(dude);

            /// We've saved some data to the database!

            /// After you've done some work, you need to say commit,
            /// so the database can release all the locks.
            /// ACID, remember... ;)
            /// Alternatively, you can say trx.Rollback() which would
            /// undo all your changes since the con.BeginTransaction()
            trx.Commit();

            /// Now lets retrieve the dude again

            /// In order to start a new transaction, you need
            /// a new transaction object. Never use the old one,
            /// after you issued commit or rollback!
            trx = con.BeginTransaction();

            /// Load all Dudes from the database
            List<object> loadedDudes = trx.Load(typeof(Dude));

            /// Print out the objects to the commandline.
            /// There should only be the one object, we saved earlier.
            foreach (Dude d in loadedDudes)
            {
                Console.WriteLine(d.name);
            }

            /// That's basically it! Created a datamodel, stored 
            /// and retrieved some data.
            /// But there is much more to abNORM! If you want to know 
            /// more, check out the other examples!
            trx.Commit();

            /// Some cleaning up.
            /// Notice: There is no need to do anything with the 
            /// connection object! It's all taken care of :)
            trx = con.BeginTransaction();
            trx.DropDataBaseSchema(dataModel);
            trx.Commit();
        }
    }
}
