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

namespace abNORM.Tests.Core
{
    class DM_Customer
    {
        public DM_Customer()
        {
        }

        public DM_Customer(
        	int customer_id, 
        	int customer_no,
        	string customer_name,
        	DateTime? date_last_modified)
        {
            this.customer_id = customer_id;
            this.customer_no = customer_no;
            this.customer_name = customer_name;
            this.date_last_modified = date_last_modified;
        }

        [PrimaryKey]
        public int customer_id;
        
        public int customer_no;

        [DBType("varchar(50)")]
        public string customer_name;

        public DateTime? date_last_modified;
    }

    class DM_Order
    {
        public DM_Order()
        {
        }

        public DM_Order(
            int order_id,
            int customer_id,
            string order_description,
            DateTime order_date,
            DateTime? date_last_modified)
        {
            this.order_id = order_id;
            this.customer_id = customer_id;
            this.order_description = order_description;
            this.order_date = order_date;
            this.date_last_modified = date_last_modified;
        }

        [PrimaryKey]
        public int order_id;

        [ForeignKey(typeof(DM_Customer), "customer_id")]
        public int customer_id;

        [DBType("varchar(50)")]
        public string order_description;

        public DateTime order_date;

        public DateTime? date_last_modified;
    }
    
    class DM_Employee
    {
        public DM_Employee()
        {
        }

    	public DM_Employee(int employee_id, string employee_name)
    	{
    		this.employee_id = employee_id;
    		this.employee_name = employee_name;
    	}
    	
    	[PrimaryKey]
    	public int employee_id;
    	
        [DBType("varchar(50)")]
    	public string employee_name;
    }
    
    class DM_Employee_Customer
    {
        public DM_Employee_Customer()
        {
        }

    	public DM_Employee_Customer(int employee_id, int customer_id)
    	{
    		this.employee_id = employee_id;
    		this.customer_id = customer_id;
    	}
    	
    	[PrimaryKey]
        [ForeignKey(typeof(DM_Employee), "employee_id")]
    	public int employee_id;
    	
    	[PrimaryKey]
        [ForeignKey(typeof(DM_Customer), "customer_id")]
    	public int customer_id;
    }
}
