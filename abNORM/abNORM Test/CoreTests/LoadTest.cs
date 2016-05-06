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
    [TestFixture(Description="abNORM Load Methods")]
    public class LoadTest
    {
        [Test(Description="Tests the Load(Type) method")]
        public void loadAll()
        {
            ITransaction trx = Config.con.BeginTransaction();
            
            List<DM_Order> orders = trx.Load(typeof(DM_Order))
                .ConvertAll<DM_Order>(
                    new Converter<object, DM_Order>(
                        delegate(object o) { return (DM_Order)o; }));
                        
            trx.Commit();
            
			Assert.AreEqual(orders.Count, 4);
        }

        [Test(Description="Tests the Load(Type, object) method (a.k.a Load-by-PK)")]
        public void loadBySinglePK()
        {
            ITransaction trx = Config.con.BeginTransaction();

            DM_Order order = (DM_Order)trx.Load(
                typeof(DM_Order),
                100);
                
            trx.Commit();
            
			Assert.AreEqual(order.order_id, 100);
			Assert.AreEqual(order.customer_id, 1);
			Assert.AreEqual(order.order_description, "Giant Laser");
			Assert.AreEqual(order.order_date, new DateTime(1996, 1, 1));
			Assert.AreEqual(order.date_last_modified, new DateTime(2100, 6, 30));
        }

        [Test(Description = "Tests the Load-by-PK method with a nonexistent row")]
        [ExpectedException(typeof(EntityNotFoundException))]
        public void loadBySinglePKNotFound()
        {
            ITransaction trx = Config.con.BeginTransaction();

            trx.Load(typeof(DM_Order), 9999);

            trx.Commit();
        }

        [Test(Description="Tests the Load-by-PK method for a multipart primary key")]
        public void loadByMultiPK()
        {
            ITransaction trx = Config.con.BeginTransaction();
            
            DM_Employee_Customer ec = (DM_Employee_Customer)trx.Load(
            	typeof(DM_Employee_Customer),
            	new object[] { 1, 2 });

            trx.Commit();
            
            Assert.AreEqual(ec.employee_id, 1);
            Assert.AreEqual(ec.customer_id, 2);
        }

        [Test(Description="Tests the Load(Type, Criterion) method")]
        public void loadByCriterion()
        {
            ITransaction trx = Config.con.BeginTransaction();
            
            List<object> orders = trx.Load(
            	typeof(DM_Order),
            	new CriterionEqual("customer_id", 1));

            trx.Commit();
            
            Assert.AreEqual(orders.Count, 2);
        }

        [Test(Description="Tests the Load(Type, Criterion, string orderBy) method")]
        public void loadOrderBy()
        {
            ITransaction trx = Config.con.BeginTransaction();
            
            List<DM_Customer> customers = trx.Load(
            	typeof(DM_Customer),
            	null,
            	"customer_no")
                .ConvertAll<DM_Customer>(
                    new Converter<object, DM_Customer>(
                        delegate(object o) { return (DM_Customer)o; }));

            trx.Commit();
            
            Assert.AreEqual(customers.Count, 2);
            Assert.AreEqual(customers[0].customer_no, 4711);
            Assert.AreEqual(customers[1].customer_no, 4712);
        }
    }
}
