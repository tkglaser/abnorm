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
using System.Data.OracleClient;
using System.Text;

namespace Oracle.Client
{
    internal class OracleDataReader : DbDataReader
    {
        System.Data.OracleClient.OracleDataReader rdr;

        public OracleDataReader(System.Data.OracleClient.OracleDataReader reader)
        {
            this.rdr = reader;
        }

        public override void Close()
        {
            rdr.Close();
        }

        public override int Depth
        {
            get { return rdr.Depth; }
        }

        public override int FieldCount
        {
            get { return rdr.FieldCount; }
        }

        public override bool GetBoolean(int ordinal)
        {
            return rdr.GetBoolean(ordinal);
        }

        public override byte GetByte(int ordinal)
        {
            return rdr.GetByte(ordinal);
        }

        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            return rdr.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);
        }

        public override char GetChar(int ordinal)
        {
            return rdr.GetChar(ordinal);
        }

        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            return rdr.GetChars(ordinal, dataOffset, buffer, bufferOffset, length);
        }

        public override string GetDataTypeName(int ordinal)
        {
            return rdr.GetDataTypeName(ordinal);
        }

        public override DateTime GetDateTime(int ordinal)
        {
            return rdr.GetDateTime(ordinal).ToLocalTime();
        }

        public override decimal GetDecimal(int ordinal)
        {
            return rdr.GetDecimal(ordinal);
        }

        public override double GetDouble(int ordinal)
        {
            return rdr.GetDouble(ordinal);
        }

        public override System.Collections.IEnumerator GetEnumerator()
        {
            //return rdr.GetEnumerator();
            throw new NotSupportedException("GetEnumerator() not supported by r3.0.2");
        }

        public override Type GetFieldType(int ordinal)
        {
            return rdr.GetFieldType(ordinal);
        }

        public override float GetFloat(int ordinal)
        {
            return rdr.GetFloat(ordinal);
        }

        public override Guid GetGuid(int ordinal)
        {
            return rdr.GetGuid(ordinal);
        }

        public override short GetInt16(int ordinal)
        {
            return rdr.GetInt16(ordinal);
        }

        public override int GetInt32(int ordinal)
        {
            return rdr.GetInt32(ordinal);
        }

        public override long GetInt64(int ordinal)
        {
            return rdr.GetInt64(ordinal);
        }

        public override string GetName(int ordinal)
        {
            return rdr.GetName(ordinal);
        }

        public override int GetOrdinal(string name)
        {
            return rdr.GetOrdinal(name);
        }

        public override System.Data.DataTable GetSchemaTable()
        {
            return rdr.GetSchemaTable();
        }

        public override string GetString(int ordinal)
        {
            return rdr.GetString(ordinal);
        }

        public override object GetValue(int ordinal)
        {
            return rdr.GetValue(ordinal);
        }

        public override int GetValues(object[] values)
        {
            return rdr.GetValues(values);
        }

        public override bool HasRows
        {
            get { return rdr.HasRows; }
        }

        public override bool IsClosed
        {
            get { return rdr.IsClosed; }
        }

        public override bool IsDBNull(int ordinal)
        {
            return rdr.IsDBNull(ordinal);
        }

        public override bool NextResult()
        {
            return rdr.NextResult();
        }

        public override bool Read()
        {
            return rdr.Read();
        }

        public override int RecordsAffected
        {
            get { return rdr.RecordsAffected; }
        }

        public override object this[string name]
        {
            get { return rdr[name]; }
        }

        public override object this[int ordinal]
        {
            get { return rdr[ordinal]; }
        }
    }

    internal class OracleTransaction : DbTransaction
    {
        System.Data.OracleClient.OracleTransaction trx;
        OracleConnection con;

        public OracleTransaction(System.Data.OracleClient.OracleTransaction trx, OracleConnection con)
        {
            this.trx = trx;
            this.con = con;
        }

        public System.Data.OracleClient.OracleTransaction OraTransaction
        {
            get
            {
                return trx;
            }
        }

        public override void Commit()
        {
            trx.Commit();
        }

        protected override DbConnection DbConnection
        {
            get { return con; }
        }

        public override System.Data.IsolationLevel IsolationLevel
        {
            get { return trx.IsolationLevel; }
        }

        public override void Rollback()
        {
            trx.Rollback();
        }
    }

    internal class OracleDBParameter : DbParameter
    {
        public System.Data.OracleClient.OracleParameter par;

        public OracleDBParameter(System.Data.OracleClient.OracleParameter par)
        {
            this.par = par;
        }

        public override DbType DbType
        {
            get
            {
                return par.DbType;
            }
            set
            {
                par.DbType = value;
            }
        }

        public override ParameterDirection Direction
        {
            get
            {
                return par.Direction;
            }
            set
            {
                par.Direction = value;
            }
        }

        public override bool IsNullable
        {
            get
            {
                return par.IsNullable;
            }
            set
            {
                par.IsNullable = value;
            }
        }

        public override string ParameterName
        {
            get
            {
                return par.ParameterName;
            }
            set
            {
                par.ParameterName = value;
            }
        }

        public override void ResetDbType()
        {
            throw new NotImplementedException();
        }

        public override int Size
        {
            get
            {
                return par.Size;
            }
            set
            {
                par.Size = value;
            }
        }

        public override string SourceColumn
        {
            get
            {
                return par.SourceColumn;
            }
            set
            {
                par.SourceColumn = value;
            }
        }

        public override bool SourceColumnNullMapping
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override DataRowVersion SourceVersion
        {
            get
            {
                return par.SourceVersion;
            }
            set
            {
                par.SourceVersion = value;
            }
        }

        public override object Value
        {
            get
            {
                return par.Value;
            }
            set
            {
                par.Value = value;
            }
        }
    }

    internal class OracleDBParameterCollection : DbParameterCollection
    {
        private System.Data.OracleClient.OracleParameterCollection parcol;

        public OracleDBParameterCollection(System.Data.OracleClient.OracleParameterCollection parcol)
        {
            this.parcol = parcol;
        }

        public override int Add(object value)
        {
            OracleDBParameter ingPar = (OracleDBParameter)value;
            parcol.Add(ingPar.par);
            return 0;
        }

        public override void AddRange(Array values)
        {
            foreach (object o in values)
                parcol.Add(o);
        }

        public override void Clear()
        {
            parcol.Clear();
        }

        public override bool Contains(string value)
        {
            return parcol.Contains(value);
        }

        public override bool Contains(object value)
        {
            return parcol.Contains(value);
        }

        public override void CopyTo(Array array, int index)
        {
            parcol.CopyTo(array, index);
        }

        public override int Count
        {
            get { return parcol.Count; }
        }

        public override System.Collections.IEnumerator GetEnumerator()
        {
            return parcol.GetEnumerator();
        }

        protected override DbParameter GetParameter(string parameterName)
        {
            return new OracleDBParameter(parcol[parameterName]);
        }

        protected override DbParameter GetParameter(int index)
        {
            return new OracleDBParameter(parcol[index]);
        }

        public override int IndexOf(string parameterName)
        {
            return parcol.IndexOf(parameterName);
        }

        public override int IndexOf(object value)
        {
            return parcol.IndexOf(value);
        }

        public override void Insert(int index, object value)
        {
            parcol.Insert(index, value);
        }

        public override bool IsFixedSize
        {
            get { return parcol.IsFixedSize; }
        }

        public override bool IsReadOnly
        {
            get { return parcol.IsReadOnly; }
        }

        public override bool IsSynchronized
        {
            get { return parcol.IsSynchronized; }
        }

        public override void Remove(object value)
        {
            parcol.Remove(value);
        }

        public override void RemoveAt(string parameterName)
        {
            parcol.RemoveAt(parameterName);
        }

        public override void RemoveAt(int index)
        {
            parcol.RemoveAt(index);
        }

        protected override void SetParameter(string parameterName, DbParameter value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void SetParameter(int index, DbParameter value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override object SyncRoot
        {
            get { return parcol.SyncRoot; }
        }
    }

    internal class OracleCommand : DbCommand
    {
        System.Data.OracleClient.OracleCommand cmd;
        OracleTransaction trx;
        OracleConnection con;

        public OracleCommand(System.Data.OracleClient.OracleCommand command, OracleConnection con)
        {
            cmd = command;
            this.con = con;
        }

        public override void Cancel()
        {
            cmd.Cancel();
        }

        public override string CommandText
        {
            get
            {
                return cmd.CommandText;
            }
            set
            {
                cmd.CommandText = value;
            }
        }

        public override int CommandTimeout
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override System.Data.CommandType CommandType
        {
            get
            {
                return cmd.CommandType;
            }
            set
            {
                cmd.CommandType = value;
            }
        }

        protected override DbParameter CreateDbParameter()
        {
            return new OracleDBParameter(cmd.CreateParameter());
        }

        protected override DbConnection DbConnection
        {
            get
            {
                return con;
            }
            set
            {
                con = (OracleConnection)value;
            }
        }

        protected override DbParameterCollection DbParameterCollection
        {
            get { return new OracleDBParameterCollection(cmd.Parameters); }
        }

        protected override DbTransaction DbTransaction
        {
            get
            {
                return trx;
            }
            set
            {
                trx = (OracleTransaction)value;
                cmd.Transaction = (trx == null) ? null : trx.OraTransaction;
            }
        }

        public override bool DesignTimeVisible
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        protected override DbDataReader ExecuteDbDataReader(System.Data.CommandBehavior behavior)
        {
            return new OracleDataReader(cmd.ExecuteReader(behavior));
        }

        public override int ExecuteNonQuery()
        {
            return cmd.ExecuteNonQuery();
        }

        public override object ExecuteScalar()
        {
            return cmd.ExecuteScalar();
        }

        public override void Prepare()
        {
            cmd.Prepare();
        }

        public override System.Data.UpdateRowSource UpdatedRowSource
        {
            get
            {
                return cmd.UpdatedRowSource;
            }
            set
            {
                cmd.UpdatedRowSource = value;
            }
        }
    }

    internal class OracleConnection : DbConnection
    {
        System.Data.OracleClient.OracleConnection con;

        public OracleConnection(string connectionstring)
        {
            con = new System.Data.OracleClient.OracleConnection(connectionstring);
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return new OracleTransaction(con.BeginTransaction(), this);
        }

        public override void ChangeDatabase(string databaseName)
        {
        	throw new NotImplementedException();
        }

        public override void Close()
        {
            con.Close();
        }

        public override string ConnectionString
        {
            get
            {
                return con.ConnectionString;
            }
            set
            {
                con.ConnectionString = value;
            }
        }

        protected override DbCommand CreateDbCommand()
        {
            return new OracleCommand(con.CreateCommand(), this);
        }

        public override string DataSource
        {
            get { return con.DataSource; }
        }

        public override string Database
        {
            get { throw new NotImplementedException(); }
        }

        public override void Open()
        {
            con.Open();
        }

        public override string ServerVersion
        {
            get { return con.ServerVersion.ToString(); }
        }

        public override ConnectionState State
        {
            get { return con.State; }
        }
    }
}
