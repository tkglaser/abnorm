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
 
//#define INGRES
//#define POSTGRES
//#define ORACLE
#define MYSQL

using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

namespace abNORM.Tests.Core
{
    //[SetUpFixture]
    public class Config
    {
        static string hostname = "";
        static string database = "";
        static string userid = "";
        static string password = "";

        internal static IConnection con
        {
            get
            {
                if (_con == null)
                {
                    _con = createCon();
                }
                return _con;
            }
        }

        static IConnection _con = null;

        static IConnection createCon()
        {
            ConnectionSettings settings = new ConnectionSettings();

            settings.sessionDescription = "abnORM NUnit Tests";
            settings.doConcurrencyDetection = true;
            //settings.typeChecking = true;

#if INGRES
            IConnection theCon = ConnectionFactory.getConnection(
                ConnectionFactory.DBType.Ingres,
                "Host=" + hostname + "; Database=" + database + "; " + 
                "User Id=" + userid + "; Password=" + password + "; " +
                "BlankDate = null;",
                settings);
#endif
#if POSTGRES
            IConnection theCon = ConnectionFactory.getConnection(
                ConnectionFactory.DBType.Postgres,
                "Server=" + hostname + ";Port=5432;User Id=" + 
                userid + ";Password=" + password + ";Database=" + database + ";",
                settings);
#endif
#if ORACLE
            IConnection theCon = ConnectionFactory.getConnection(
                ConnectionFactory.DBType.Oracle,
                "Data Source=" + hostname + "; User ID=" + userid + "; Password=" + password + ";",
                settings);
#endif
#if MYSQL
            IConnection theCon = ConnectionFactory.getConnection(
                ConnectionFactory.DBType.MySQL,
                "Location=" + hostname + "; Data Source=" + database + "; " +
                "User Id=" + userid + "; Password=" + password + "; ",
                settings);
#endif
            
            setup(theCon);
			return theCon;

        }

        //[SetUp]
        public static void setup(IConnection theCon)
        {
            ITransaction trx = theCon.BeginTransaction();
            List<Type> dataModel = new List<Type>();
            dataModel.Add(typeof(DM_Customer));
            dataModel.Add(typeof(DM_Order));
            dataModel.Add(typeof(DM_Employee));
            dataModel.Add(typeof(DM_Employee_Customer));
            trx.DropDataBaseSchema(dataModel); // Clean up first
            trx.Commit();

            trx = theCon.BeginTransaction();
            trx.CreateDataBaseSchema(dataModel);
            trx.Commit();

            trx = theCon.BeginTransaction();
            trx.Save(new DM_Customer(1, 4712, "Glaser Corp.", new DateTime(2007, 8, 9, 10, 11, 12)));
            trx.Save(new DM_Customer(2, 4711, "TG Inc.", new DateTime(2007, 8, 9)));
            trx.Save(new DM_Order(100, 1, "Giant Laser", new DateTime(1996, 1, 1), new DateTime(2100, 6, 30)));
            trx.Save(new DM_Order(101, 1, "Enormous Laser", new DateTime(1996, 1, 1), new DateTime(2100, 6, 30)));
            trx.Save(new DM_Order(102, 2, "Huge Laser", new DateTime(1996, 1, 1), new DateTime(2100, 6, 30)));
            trx.Save(new DM_Order(103, 2, "Horrible Laser", new DateTime(1996, 1, 1), new DateTime(2100, 6, 30)));
            trx.Save(new DM_Employee(1, "Dr. Evil"));
            trx.Save(new DM_Employee(2, "Minime"));
            trx.Save(new DM_Employee_Customer(1, 1));
            trx.Save(new DM_Employee_Customer(1, 2));
            trx.Commit();
        }

        //[TearDown]
        public void teardown()
        {
            ITransaction trx = con.BeginTransaction();
            List<Type> dataModel = new List<Type>();
            dataModel.Add(typeof(DM_Customer));
            dataModel.Add(typeof(DM_Order));
            dataModel.Add(typeof(DM_Employee));
            dataModel.Add(typeof(DM_Employee_Customer));
            trx.DropDataBaseSchema(dataModel);
            trx.Commit();
        }
    }
}
