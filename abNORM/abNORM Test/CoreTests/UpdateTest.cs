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
    [TestFixture(Description = "abNORM Update Method")]
    public class UpdateTest
    {
        [Test(Description = "Tests the update() Method")]
        public void update()
        {
            ITransaction trx = Config.con.BeginTransaction();

            Dictionary<string, object> fields = new Dictionary<string,object>();
            
            fields.Add("customer_name", "alteredByUpdate");

            trx.Update(
                typeof(DM_Customer),
                fields,
                new CriterionEqual("customer_id", 1));

            trx.Commit();

            trx = Config.con.BeginTransaction();

            DM_Customer cust = (DM_Customer)trx.Load(typeof(DM_Customer), 1);

            Assert.AreEqual(cust.customer_name, "alteredByUpdate");
            
            cust.customer_name = "Glaser Corp.";

            trx.Save();

            trx.Commit();
        }
    }
}
