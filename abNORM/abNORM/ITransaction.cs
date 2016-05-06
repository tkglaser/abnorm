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

namespace abNORM
{
    /// <summary>
    /// Hit flags for saving objects
    /// </summary>
    public enum SaveParams
    {
        /// <summary>
        /// The object shall be updated. 
        /// If the object is not in the entitycache, an error will be thrown.
        /// </summary>
        update,
        /// <summary>
        /// If the object is not found in the entity cache, an
        /// UPDATE statement will be generated.
        /// If it is found, an INSERT statement will be generated.
        /// </summary>
        insertorupdate,
        /// <summary>
        /// An INSERT statement will be created. If the object is already
        /// in the database, a DUPLICATE-KEY exception is likely to occur.
        /// </summary>
        insert
    }

    public interface ITransaction
    {
        /// <summary>
        /// Provides the used DbCommand object.
        /// Use of this property is strongly discouraged,
        /// but might be useful in certain cases.
        /// </summary>
        DbCommand Command { get; }

        /// <summary>
        /// Clears the entity cache. This is normally done automatically,
        /// so don't use it unless you know exactly, what you're doing.
        /// </summary>
        void ClearEntityCache();

        /// <summary>
        /// Throws a DB-Event. This feature is currently only supported 
        /// for Ingres. Refer to the ingres sqlref for more info on DB-Events.
        /// </summary>
        /// <param name="eventname">Name of the event</param>
        /// <param name="eventtext">Text of the event</param>
        void RaiseDBEvent(string eventname, string eventtext);

        /// <summary>
        /// Enables autocommit for the current transaction.
        /// This must be done BEFORE any Load/Save/Delete/Update
        /// invocations. The DBMS might throw an exception otherwise.
        /// </summary>
        void EnableAutoCommit();

        /// <summary>
        /// Selects one single numeric value. For instance:
        /// int i = trx.Select("select count(*) from customer");
        /// </summary>
        /// <param name="command">Sql statement</param>
        /// <returns>Numerical result of the query</returns>
        long Select(string command);

        /// <summary>
        /// Returns all objects of the given type
        /// </summary>
        /// <param name="t">Objecttype to load</param>
        /// <returns>List with the found objects</returns>
        List<object> Load(Type t);

        /// <summary>
        /// Loads one object of the given type using a single primary key
        /// </summary>
        /// <param name="t">Objecttype to load</param>
        /// <param name="key">Value for the primary key</param>
        /// <returns>The found object</returns>
        /// <exception cref="EntityNotFoundException">This exception will be thrown,
        /// when there is no object found with this primary key</exception>
        object Load(Type t, object key);

        /// <summary>
        /// Loads one object of the given type using a combined primary key
        /// </summary>
        /// <param name="t">Objecttype to load</param>
        /// <param name="keys">Values for the primary key</param>
        /// <returns>The found object</returns>
        /// <exception cref="EntityNotFoundException">This exception will be thrown,
        /// when there is no object found with this primary key</exception>
        object Load(Type t, object[] keys);

        /// <summary>
        /// Load objects specified by criteria
        /// </summary>
        /// <param name="t">Objecttype to load</param>
        /// <param name="crit">Criterium to specify the objects to load</param>
        /// <returns>List with the found objects</returns>
        List<object> Load(Type t, Criterion crit);

        /// <summary>
        /// Load objects specified by criteria and adding an ORDER BY string
        /// </summary>
        /// <param name="t">Objecttype to load</param>
        /// <param name="crit">Criterium to specify the objects to load</param>
        /// <param name="orderBy">Orderby String</param>
        /// <returns>List with the found objects</returns>
        List<object> Load(Type t, Criterion crit, string orderBy);

        /// <summary>
        /// Updates objects not using the entity cache
        /// </summary>
        /// <param name="t">Objecttype to load</param>
        /// <param name="fields">Dictinoary with fieldnames and their new values</param>
        /// <param name="crit">Criterium to specify the objects to update</param>
        /// <returns>Count of affected rows</returns>
        int Update(Type t, Dictionary<string, object> fields, Criterion crit);

        /// <summary>
        /// Issues an UPDATE for all objects loaded in this transaction
        /// </summary>
        void Save();

        /// <summary>
        /// Saves an object to the database. This funktion uses the 
        /// object cache to determine, whether an UPDATE or INSERT
        /// will be generated.
        /// </summary>
        /// <param name="dat">Object to save</param>
        void Save(object dat);

        /// <summary>
        /// Saves an object to the database.
        /// </summary>
        /// <param name="dat">Object to save</param>
        /// <param name="saveParams">Savehint. See declaration of SaveParams</param>
        void Save(object dat, SaveParams saveParams);

        /// <summary>
        /// Deletes a single object from the database.
        /// It will also be removed from the objectcache.
        /// </summary>
        /// <param name="dat">Object to delete</param>
        void Delete(object dat);

        /// <summary>
        /// Deletes multiple objects from the database
        /// </summary>
        /// <param name="t">Objecttype</param>
        /// <param name="crit">Criterium of the to-be-deleted objects</param>
        /// <returns>Count of affected rows</returns>
        int Delete(Type t, Criterion crit);

        /// <summary>
        /// Executes a non select sql statement.
        /// </summary>
        /// <param name="sql">The sql statement as string</param>
        /// <returns>Count of affected rows</returns>
        int ExecuteSql(string sql);

        /// <summary>
        /// Creates tables, foreign keys and sequences in the database 
        /// according to the given datamodel.
        /// </summary>
        /// <param name="model">List of objects representing the datamodel to be created</param>
        void CreateDataBaseSchema(List<Type> model);

        /// <summary>
        /// Drops all sequences, foreign keys and tables that belong to 
        /// the given datamodel.
        /// If any of these database-objects don't exist, the error is 
        /// silently ignored, there are no exceptions thrown by this method.
        /// </summary>
        /// <param name="model">List of objects representing the datamodel to be destroyed</param>
        void DropDataBaseSchema(List<Type> model);

        /// <summary>
        /// Issues a rollback operation.
        /// The ITransaction-object is not to be used again after 
        /// the invocation of this method.
        /// </summary>
        void Rollback();

        /// <summary>
        /// Issues a commit operation.
        /// The ITransaction-object is not to be used again after 
        /// the invocation of this method.
        /// </summary>
        void Commit();
    }
}
