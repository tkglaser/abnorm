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

using NUnit.Framework;

namespace abNORM.Tests.Core
{
    [TestFixture(Description = "abNORM DB Concurrency Detection")]
    public class DBConcurrencyTest
    {
        [SetUp]
        public void setup()
        {
            ITransaction trx = Config.con.BeginTransaction();

            DM_Customer cust = (DM_Customer)trx.Load(
                typeof(DM_Customer),
                1);
                
            cust.customer_name = "Glaser Corp.";

            trx.Save();

            trx.Delete(typeof(DM_Order), new CriterionEqual("order_id", 200));

            trx.Commit();
        }

        [Test(Description = "Tests DB Concurrency Detection in case of conflicting UPDATE")]
        public void checkUpdateConflict()
        {
            ITransaction trxA = Config.con.BeginTransaction();
            ITransaction trxB = Config.con.BeginTransaction();
            
            DM_Customer cA = (DM_Customer)trxA.Load(
                typeof(DM_Customer),
                1);
                
            DM_Customer cB = (DM_Customer)trxB.Load(
                typeof(DM_Customer),
                1);
                
            cA.customer_name = "Changed by A";

            trxA.Save(cA);

            trxA.Commit();
            
            cB.customer_name = "Changed by B";

            Exception ex = null;

            try
            {
                trxB.Save();
            }
            catch (Exception e)
            {
                ex = e;
            }

            Assert.IsNotNull(ex);
            Assert.AreEqual(typeof(DBConcurrencyException).ToString(), ex.GetType().ToString());

            setup();
        }

        [Test(Description = "Tests DB Concurrency Detection in case of mergeable UPDATE")]
        public void checkUpdateMerge()
        {
            ITransaction trxA = Config.con.BeginTransaction();
            ITransaction trxB = Config.con.BeginTransaction();

            DM_Customer cA = (DM_Customer)trxA.Load(
                typeof(DM_Customer),
                1);
                
            DM_Customer cB = (DM_Customer)trxB.Load(
                typeof(DM_Customer),
                1);
			
			cA.customer_name = "Changed by A";

            trxA.Save(cA);

            trxA.Commit();

			cB.customer_no = 4321;

            trxB.Save(cB);

            trxB.Commit();

            trxA = Config.con.BeginTransaction();

            cA = (DM_Customer)trxA.Load(
                typeof(DM_Customer),
                1);

			cA.customer_name = "Glaser Corp.";
			cA.customer_no = 4711;

            trxA.Save();
            trxA.Commit();
        }

        [Test(Description = "Tests DB Concurrency Detection in case of DELETE")]
        public void checkDelete()
        {
            ITransaction trx = Config.con.BeginTransaction();
            
            trx.Save(new DM_Order(200, 1, "Another Laser", DateTime.Now, DateTime.Now));

            trx.Commit();

            ITransaction trxA = Config.con.BeginTransaction();
            ITransaction trxB = Config.con.BeginTransaction();

            DM_Order oA = (DM_Order)trxA.Load(typeof(DM_Order), 200);
            DM_Order oB = (DM_Order)trxB.Load(typeof(DM_Order), 200);

            trxA.Delete(oA);

            trxA.Commit();

            Exception ex = null;

            try
            {
                trxB.Delete(oB);
            }
            catch (Exception e)
            {
                ex = e;
            }

            Assert.IsNotNull(ex);
            Assert.AreEqual(typeof(DBConcurrencyException).ToString(), ex.GetType().ToString());
        }
    }
}
