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

namespace abNORM
{
    public class EntityNotFoundException : Exception
    {
        public override string Message
        {
            get
            {
                return "The object was not found in the database";
            }
        }
    }

    public class InvalidEntityException : Exception
    {
        public override string Message
        {
            get
            {
                return "The object is not valid in this transaction";
            }
        }
    }

    public class DBConcurrencyException : Exception
    {
        public override string Message
        {
            get
            {
                return "The data was modified by another user in the meantime.";
            }
        }
    }
}
