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
    [TableName("DM_Order", "ord")]
    [TableName("DM_Customer", "cu")]
    class OrderCustomer
    {
        [PrimaryKey]
        public int order_id;
        [JoinKey("ord", "cu")]
        public int customer_id;
        public string order_description;
        public int customer_no;
        public string customer_name;
    }

    [TestFixture(Description = "Tests misc abNORM Features")]
    public class MiscTest
    {
        [Test(Description="Tests the Select method")]
        public void select()
        {
            ITransaction trx = Config.con.BeginTransaction();

            long count = trx.Select("select count(*) from DM_Customer");

            trx.Commit();

            Assert.AreEqual(count, 2);
        }

        [Test(Description = "Tests the JoinKey attribute")]
        public void joinKey()
        {
            ITransaction trx = Config.con.BeginTransaction();

            OrderCustomer ordCust = (OrderCustomer)trx.Load(
                typeof(OrderCustomer),
                100);

            trx.Commit();

            Assert.AreEqual(ordCust.order_id, 100);
            Assert.AreEqual(ordCust.customer_id, 1);
            Assert.AreEqual(ordCust.order_description, "Giant Laser");
            Assert.AreEqual(ordCust.customer_no, 4711);
            Assert.AreEqual(ordCust.customer_name, "Glaser Corp.");
        }
    }
}
