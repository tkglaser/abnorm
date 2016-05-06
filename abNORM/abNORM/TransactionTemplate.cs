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
using System.Reflection;
using System.Text;

namespace abNORM
{
    class TransactionTemplate : ITransaction
    {
        protected IDataProvider impl;
        protected ConnectionSettings settings;
        protected Dictionary<object, object> entityCache =
            new Dictionary<object, object>();

        public TransactionTemplate(IDataProvider impl, ConnectionSettings settings)
        {
            this.impl = impl;
            this.settings = settings;
        }

        public System.Data.Common.DbCommand Command
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public void ClearEntityCache()
        {
            entityCache.Clear();
        }

        public void RaiseDBEvent(string eventname, string eventtext)
        {
            impl.RaiseDBEvent(eventname, eventtext);
        }

        public void EnableAutoCommit()
        {
            impl.EnableAutoCommit();
        }

        public long Select(string command)
        {
            return impl.Select(command);
        }

        public List<object> Load(Type t)
        {
            return impl.Select(t, null, null);
        }

        protected string[] reflectPKNames(Type dat)
        {
            List<string> pkList = new List<string>();

            foreach (FieldInfo fi in dat.GetFields())
            {
                if (fi.GetCustomAttributes(typeof(PrimaryKey), true).Length > 0)
                {
                    object[] ca = fi.GetCustomAttributes(typeof(JoinKey), true);
                    if (ca.Length > 0)
                    {
                        JoinKey jk = (JoinKey)ca[0];
                        pkList.Add(jk.tableA + "." + fi.Name);
                        continue;
                    }
                    ca = fi.GetCustomAttributes(typeof(DBAlias), true);
                    if (ca.Length > 0)
                    {
                        DBAlias a = (DBAlias)ca[0];
                        pkList.Add(a.alias);
                        continue;
                    }
                    pkList.Add(fi.Name);
                }
            }
            return pkList.ToArray();
        }

        protected void reflectCopy(object orig, ref object dest)
        {
            System.Reflection.FieldInfo[] fieldInfo = orig.GetType().GetFields();

            foreach (System.Reflection.FieldInfo fi in fieldInfo)
            {
                fi.SetValue(dest, fi.GetValue(orig));
            }
        }

        protected object clone(object dat)
        {
            object theClone = createEntity(dat.GetType());
            reflectCopy(dat, ref theClone);
            return theClone;
        }

        protected object createEntity(Type t)
        {
            object theObject
                = t.GetConstructor(Type.EmptyTypes).Invoke(new object[] { });
            return theObject;
        }

        public object Load(Type t, object key)
        {
            string[] pkNames = reflectPKNames(t);
            if (pkNames.Length != 1)
                throw new Exception("There is more than one primary key in this type!");

            List<object> result = impl.Select(t, new CriterionEqual(pkNames[0], key), null);

            if (result.Count != 1)
            {
                Rollback();
                throw new EntityNotFoundException();
            }

            entityCache.Add(result[0], clone(result[0]));

            return result[0];
        }

        public object Load(Type t, object[] keys)
        {
            string[] pkNames = reflectPKNames(t);
            if (pkNames.Length != keys.Length)
                throw new Exception("The number of primary keys is not equal!");

            List<Criterion> critList = new List<Criterion>();

            for (int i = 0; i < keys.Length; i++)
            {
                critList.Add(new CriterionEqual(pkNames[i], keys[i]));
            }

            List<object> result = impl.Select(t, new CriterionAnd(critList.ToArray()), null);

            if (result.Count != 1)
            {
                Rollback();
                throw new EntityNotFoundException();
            }

            entityCache.Add(result[0], clone(result[0]));

            return result[0];
        }

        public List<object> Load(Type t, Criterion crit)
        {
            List<object> result = impl.Select(t, crit, null);
            foreach (object loadedObj in result)
            {
                entityCache.Add(loadedObj, clone(loadedObj));
            }
            return result;
        }

        public List<object> Load(Type t, Criterion crit, string orderBy)
        {
            string[] orderBySplit = orderBy.Split(',');
            List<string> orderByList = new List<string>();
            foreach (string ob in orderBySplit)
            {
                if (!string.IsNullOrEmpty(ob.Trim()))
                {
                    orderByList.Add(ob.Trim());
                }
            }
            List<object> result = impl.Select(t, crit, orderByList);
            foreach(object res in result)
            {
                entityCache.Add(res, clone(res));
            }
            return result;
        }

        protected void updateEntity(object dat, object origdat)
        {
            Dictionary<string, object> fields = new Dictionary<string, object>();
            List<Criterion> critList = new List<Criterion>();

            foreach (FieldInfo fi in dat.GetType().GetFields())
            {
                object o1 = fi.GetValue(dat);
                object o2 = fi.GetValue(origdat);

                bool equal = false;

                if ((o1 == null) && (o2 == null))
                    equal = true;

                if ((o1 != null) && (o2 != null))
                    if (o1.ToString() == o2.ToString())
                        equal = true;

                if (!equal)
                {
                    fields.Add(fi.Name, o1);
                    if (settings.doConcurrencyDetection)
                    {
                        critList.Add(new CriterionEqual(fi.Name, o2));
                    }
                }
            }

            if (fields.Count == 0)
                return;

            foreach (string pk in reflectPKNames(dat.GetType()))
            {
                critList.Add(new CriterionEqual(pk, dat.GetType().GetField(pk).GetValue(dat)));
            }

            int i = impl.Update(dat.GetType(), fields, new CriterionAnd(critList.ToArray()));

            if (settings.doConcurrencyDetection && (i == 0))
            {
                Rollback();
                throw new DBConcurrencyException();
            }
        }

        public int Update(Type t, Dictionary<string, object> fields, Criterion crit)
        {
            return impl.Update(t, fields, crit);
        }

        public void Save()
        {
            Dictionary<object, object>.KeyCollection keys = entityCache.Keys;
            List<object> ents = new List<object>();

            foreach (object key in keys)
            {
                ents.Add(key);
            }

            foreach (object ent in ents)
            {
                Save(ent, SaveParams.insertorupdate);
            }
        }

        public void Save(object dat)
        {
            Save(dat, SaveParams.insertorupdate);
        }

        public void Save(object dat, SaveParams saveParams)
        {
            if (!entityCache.ContainsKey(dat))
            {
                if (saveParams != SaveParams.update)
                {
                    impl.Insert(dat);
                }
                else
                {
                    throw new InvalidEntityException();
                }
            }
            else
            {
                if (saveParams != SaveParams.insert)
                {
                    updateEntity(dat, entityCache[dat]);
                    entityCache.Remove(dat);
                }
                else
                {
                    throw new InvalidEntityException();
                }
            }
            entityCache.Add(dat, clone(dat));
        }

        public void Delete(object dat)
        {
            string[] pkNames = reflectPKNames(dat.GetType());
            List<Criterion> critList = new List<Criterion>();
            foreach (string pk in pkNames)
            {
                critList.Add(new CriterionEqual(
                    pk, 
                    dat.GetType().GetField(pk).GetValue(dat)));
            }

            int i = impl.Delete(dat.GetType(), new CriterionAnd(critList.ToArray()));

            if (entityCache.ContainsKey(dat))
                entityCache.Remove(dat);

            if (i == 0)
            {
                Rollback();
                throw new DBConcurrencyException();
            }
        }

        public int Delete(Type t, Criterion crit)
        {
            return impl.Delete(t, crit);
        }

        public int ExecuteSql(string sql)
        {
            return impl.ExecuteSql(sql);
        }

        public void CreateDataBaseSchema(List<Type> model)
        {
            foreach (Type entity in model)
            {
                impl.CreateSequences(entity);
                impl.CreateTable(entity);
                impl.CreatePKsForTable(entity);
            }
            foreach (Type entity in model)
            {
                foreach (FieldInfo fi in entity.GetFields())
                {
                    object[] fkList = fi.GetCustomAttributes(typeof(ForeignKey), true);
                    if (fkList.Length > 0)
                    {
                        ForeignKey fk = (ForeignKey)fkList[0];
                        ForeignKeyDescriptor fkdesc = new ForeignKeyDescriptor();
                        fkdesc.targetType = fk.reference;
                        fkdesc.targetFieldName = fk.attributeName;
                        fkdesc.sourceType = entity;
                        fkdesc.sourceFieldName = fi.Name;
                        impl.CreateForeignKey(fkdesc);
                    }
                }
            }
        }

        public void DropDataBaseSchema(List<Type> model)
        {
            foreach (Type entity in model)
            {
                impl.DropSequences(entity);
                impl.DropTable(entity);
            }
        }

        public void Rollback()
        {
            entityCache.Clear();
            impl.Rollback();
        }

        public void Commit()
        {
            entityCache.Clear();
            impl.Commit();
        }
    }
}
