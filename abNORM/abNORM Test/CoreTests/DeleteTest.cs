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
    [TestFixture(Description = "abNORM DELETE Methods")]
    public class DeleteTest
    {
        [SetUp]
        public void setup()
        {
            ITransaction trx = Config.con.BeginTransaction();
            
            trx.Delete(typeof(DM_Customer), new CriterionEqual("customer_id", 100));
            
            trx.Save(new DM_Customer(100, 5000, "DeleteMe", DateTime.Now));

            trx.Commit();
        }

        private void isGone()
        {
            ITransaction trx = Config.con.BeginTransaction();

            List<object> custs = trx.Load(
                typeof(DM_Customer),
                new CriterionEqual("customer_id", 100));

            Assert.AreEqual(custs.Count, 0);

            trx.Commit();
        }

        [Test(Description="Deletes by entity")]
        public void deleteByEntity()
        {
            ITransaction trx = Config.con.BeginTransaction();

            DM_Customer cust = (DM_Customer)trx.Load(
                typeof(DM_Customer),
                100);

            trx.Delete(cust);

            trx.Commit();

            isGone();
        }

        [Test(Description="Delete by Criterion")]
        public void deleteByCrit()
        {
            ITransaction trx = Config.con.BeginTransaction();

            int rows = trx.Delete(
                typeof(DM_Customer),
                new CriterionEqual("customer_id", 100));

            Assert.AreEqual(rows, 1);

            trx.Commit();

            isGone();
        }
    }
}
