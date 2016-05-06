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
    [TestFixture(Description = "abNORM Save Methods")]
    public class SaveTest
    {
        public void isSavedUpdate()
        {
            ITransaction trx = Config.con.BeginTransaction();

            DM_Order order = (DM_Order)trx.Load(
                typeof(DM_Order),
                100);

            Assert.AreEqual(order.order_description, "ChangedBySaveTests");

            order.order_description = "Giant Laser";

            trx.Save(order);

            trx.Commit();
        }

        public void isSavedInsert()
        {
            ITransaction trx = Config.con.BeginTransaction();

            DM_Order order = (DM_Order)trx.Load(
                typeof(DM_Order),
                200);

            Assert.AreEqual(order.customer_id, 2);
            Assert.AreEqual(order.order_description, "Monstrous Laser");
            Assert.AreEqual(order.order_id, 200);

            trx.Delete(order);

            trx.Commit();
        }

        [Test(Description = "Tests the save() Method for UPDATE")]
        public void saveAllUpd()
        {
            ITransaction trx = Config.con.BeginTransaction();

            DM_Order order = (DM_Order)trx.Load(
                typeof(DM_Order),
                100);
                
            Assert.AreEqual(order.order_description, "Giant Laser");

            order.order_description = "ChangedBySaveTests";

            trx.Save();

            trx.Commit();

            isSavedUpdate();
        }

        [Test(Description = "Tests the save(object) Method for UPDATE")]
        public void saveEntityUpd()
        {
            ITransaction trx = Config.con.BeginTransaction();

            DM_Order order = (DM_Order)trx.Load(
                typeof(DM_Order),
                100);

            Assert.AreEqual(order.order_description, "Giant Laser");

            order.order_description = "ChangedBySaveTests";

            trx.Save(order);

            trx.Commit();

            isSavedUpdate();
        }

        [Test(Description = "Tests the save() Method for INSERT")]
        public void saveEntityInsert()
        {
            ITransaction trx = Config.con.BeginTransaction();

            trx.Save(new DM_Order(200, 2, "Monstrous Laser", DateTime.Now, DateTime.Now)); 

            trx.Commit();

            isSavedInsert();
        }

        [Test(Description = "Tests the save() Method for INSERT with a forced UPDATE")]
        [ExpectedException(typeof(InvalidEntityException))]
        public void saveEntityInsertForcedUpdate()
        {
            ITransaction trx = Config.con.BeginTransaction();

            trx.Save(
            	new DM_Order(200, 2, "Monstrous Laser", DateTime.Now, DateTime.Now), 
            	SaveParams.update);

            trx.Commit();
        }

        [Test(Description = "Tests the save(object) Methode for UPDATE with a forced INSERT")]
        [ExpectedException(typeof(InvalidEntityException))]
        public void saveEntityUpdForcedInsert()
        {
            ITransaction trx = Config.con.BeginTransaction();

            DM_Order order = (DM_Order)trx.Load(
                typeof(DM_Order),
                100);

            Assert.AreEqual(order.order_description, "Giant Laser");

            order.order_description = "ChangedBySaveTests";

            trx.Save(order, SaveParams.insert);

            trx.Commit();

            isSavedUpdate();
        }
    }
}
