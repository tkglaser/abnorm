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

using NUnit.Framework;

namespace abNORM.Tests.Core
{
    [TestFixture(Description = "Test the DateTime Conversion")]
    public class DateTimeTest
    {
        [Test(Description="Tests the DateTime conversion on Load")]
        public void loadTest()
        {
            ITransaction trx = Config.con.BeginTransaction();
            
            DM_Customer cust = (DM_Customer)trx.Load(
            	typeof(DM_Customer),
            	1);

            trx.Commit();
            
            Assert.AreEqual(cust.date_last_modified, new DateTime(2007, 8, 9, 10, 11, 12));
        }

        [Test(Description="Tests the DateTime conversion in the where string")]
        public void whereTest()
        {
            ITransaction trx = Config.con.BeginTransaction();
            
            List<object> custs = trx.Load(
            	typeof(DM_Customer),
            	new CriterionEqual("date_last_modified", new DateTime(2007, 8, 9, 10, 11, 12)));

            trx.Commit();
            
            Assert.AreEqual(custs.Count, 1);
        }
    }
}
