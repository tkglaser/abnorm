	MySQLDriverCS: An C# driver for MySQL.
	Copyright (c) 2002-2007 Manuel Lucas Viñas Livschitz.

	This file is part of MySQLDriverCS.

    MySQLDriverCS is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    MySQLDriverCS is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU Lesser General Public
    License along with this library; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
ABOUT
-----
This project was developed because the only ADO.NET compliant driver is 
commercial. This driver is free (LGPL).
There were another solutions as OLEDB driver and ODBC.NET but the first 
is very limited and was abandoned by the owner in the year 2001, the 
second solution ODBC.NET doesn't works fine because ODBC.NET is a very 
advanced driver not very compatible with the GPL ODBC driver of MySQL.

OBJECTIVES
----------
-	Support all SQL basic functions.
-	Additional features.


INSTALLATION/DEVELOPMENT
------------------------
You must add a reference to MySQLDriverCS.DLL in your project. 
MySQLDriverCS.DLL calls libMYSQL.dll but this file produces errors when 
MySQLDriverCS is used in ASP.NET projects so put it in windows\system 
folder to avoid errors and remove it from ASP.NET bin folder of your 
project. Because of this, the package includes a install option to make
it automatically (for deployments).


USAGE
-----
Use it as OleDb driver or Sql driver but taking care of unsupported members.
This driver works now with text-only queries.

RELEASE VERSION 4.0.1 NOTES
----------------------------
Date: 2007-07-12

Features:
	- Claudia Murialdo (cmurialdo@artech.com.uy)
		- Fix: System Stack OverFlow when reusing prepared statements.
    - Dot Net 1.1 removed.
    
RELEASE VERSION 4.0.0 NOTES
----------------------------
Date: 2007-03-12

Features:
	-Claudia Murialdo (cmurialdo@artech.com.uy)
		Added -	Support for "Character Set" keyword in the connection string in order
			to support multibyte characteres in the stmts.
			parm.Value = System.DBNull.Value or parm.Value=null.
    -Working on Dot Net 2.0, 1.1 is now closed.


RELEASE VERSION 3.0.18 NOTES
----------------------------
Date: 2005-04-19

Features:
	-Claudia Murialdo (cmurialdo@artech.com.uy)
		Fixed -	MySQLParameter: now it is possible to assign 
			parm.Value = System.DBNull.Value or parm.Value=null.
		Added -	MySQLDataReader: implemented method GetBytes( 
			int i, long fieldOffset, byte[] buffer, int bufferoffset, 
			int length ) in order to support binary data..
		Fixed - MySQLCommand: _replaceparameters method now supports 
			System.DBNull.Value and byte[] (for binary data) as 
			parameter value in the stmts. 


RELEASE VERSION 3.0.17 NOTES
----------------------------
Date: 2005-01-01

Features:
	-Chris Palowitch (chrispalo@bellsouth.net)
		Added support for boolean parameters.
		Fixed performance issues in MySQLUtil.Escape()

RELEASE VERSION 3.0.16 NOTES
----------------------------
Date: 2004-10-01

Features:
	-Claudia Murialdo
		Fixed - Date and DateTimes values throw Invalid Cast Exception 
		on MySql Version >= 4.1.2-alpha .
		Changed- MySQLParameter now extends ICloneable besides 
		IDataParameter and IDbDataParameter.

RELEASE VERSION 3.0.15 NOTES
----------------------------
Date: 2004-07-29

Features:
	-Claudia Murialdo
		Added  - number property to MySQLExceptions. 
		Changed- MySQLParameter now extends IDbDataParameter besides 
			IDDataParameter. 
		Changed -  Commands to take into account time portion in 
			DateTime values. 
		Fixed - Connection object so it won't close after ExecuteScalar 
			method. 
		Fixed - Commands with parameters: now it is possible to repeat 
			the execute() method with different parameters values. 
		Fixed - Culture independent numeric values. For example, 
			GetDecimal() now returns always the correct decimal, 
			independently of the current regional and language options. 
		Fixed - Execution of stmts with similar parameter names like 
			SELECT * FROM Table1 WHERE A=@ParmName AND B=@ParmNameOther.

RELEASE VERSION 3.0.14 NOTES
----------------------------
Date: 2004-07-24

Features:
	-Alex Seewald changes:
		The first one to activate Connection timeout; the 
		mysql library used to freeze the application at times - 
		although it was rare - when you're connecting to a remote 
		database over the Internet; I also added mysql client/server 
		compression, too. 

		I've also managed to improve the overall performance of 
		MySQLDriver by using the most recent version of the MySQL 
		client library (makes things a bit more compatible and more 
		reliable) - and compiling it to be optimized for all possible Intel 
		CPUs.

	-M.L. Viñas Livschitz:
		Timeout defaulted to 100 secs in favor of connection pooling.
		A greater value is recomended.

RELEASE VERSION 3.0.13 NOTES 
----------------------------
Date: 2004-26-02
Features:
-MySQLDataReader: Now IsDBNull is supported.
-Now supports MySQL 4.x versions: (libMySQLd.dll upgraded).
-MySQL documentation updated


RELEASE VERSION 3.0.12 NOTES 
----------------------------
Date: 2004-01-18
Features:
MySQLCommand:
Items update by Omar del Valle Rodríguez (01/18/2004)
- Method ExecuteScalar is now support.
- ExecuteReader now is declare protected and support CloseConnection parameter
- Created method public ExecuteReader() with CloseConnection in false
- Updated method ExecuteReader(CommandBehavior behavior) in order support
CommandBehavior.CloseConnection

MySQLDataReader:
Items update by Omar del Valle Rodríguez (01/18/2004)
- Constrctor MySQLDataReader now support bool parameter to close connection
after close DataReader.
- Update method Close in order support CommandBehavior.CloseConnection

RELEASE VERSION 3.0.11 NOTES 
----------------------------
Date: 2003-12-05
Features:
	Blob, Text and TimeStamp support (by "Christophe Ravier" <c.ravier@laposte.net>).
	Explicit conection errors (by "Christophe Ravier" <c.ravier@laposte.net>). 


RELEASE VERSION 3.0.10 NOTES 
---------------------------
Date: 2003-08-31
Features:
	Parameter usage added (by Omar del Valle Rodríguez and William Reinoso). 
BugFixes:
	MySQLCommand changes in order to support parameters.

RELEASE VERSION 3.0.8 NOTES 
---------------------------
Date: 2003-05-24
Features:
	MySQLDataAdapter added (by Omar del Valle Rodríguez). 
BugFixes:
	DataReader changes in order to support MySQLDataAdapter.
	
RELEASE VERSION 3.0.7 NOTES 
---------------------------
Date: 2003-02-16
Features:
	Strong Name Added.
	GAC.bat -> GAC registering for MySQLDriverCS 
BugFixes:
	none

RELEASE VERSION 3.0.6 NOTES 
---------------------------
Date: 2003-01-28
BugFixes:
	MySQLCommand.ExecuteReader() and ExecuteNonQuery() bugfixed while checking connection state.
	Connection property in MySQLCommand


RELEASE VERSION 3.0.5 NOTES 
---------------------------
Date: 2002-11-27
Features:
	Transactions bugfixed [Vincent (vdaron) 2002-11-23 and Me 2002-11-27]
	GetDouble and GetFloat decimation bugfixed for Languages with comma decimation like German
	
RELEASE VERSION 3.0.4 NOTES 
---------------------------
Date: 2002-11-14
Features:
	VB.NET sample added

RELEASE VERSION 3.0.3 NOTES 
---------------------------
Date: 2002-11-04
BugFixes:
	 MySQLSelectCommand.Exec: Bugfixed by Yann Sénécheau 2002-10-28
	 MySQLCommand.CommandText: setting hadn't any effect
Features:
	 Port added as property of connection and as part of connection string

RELEASE VERSION 3.0.2 NOTES 
---------------------------
Date: 2002-10-28
- Now supports: Transactions. 
		all related functions.
		MySQLTransaction class
- New Functions:
		MySQLConnection
			ChangeDatabase
			CreateCommand
		MySQLCommand
			Cancel
- New MySQLSelectCommand function (a select that can do anything)


RELEASE VERSION 3.0.1 NOTES 
---------------------------
Date: 2002-10-05
-	Small bugfix in Easy-Query-Tools where clauses.

RELEASE VERSION 3.0.0 NOTES 
---------------------------
Date: 2002-10-02
-	MySQL Easy-Query-Tools to make query creation and database connection easier.
-	More installer options to avoid default mysqllib.dll installation.
-	Internal enhacements.
-	More examples and more clearer.
-	Examples in help.

RELEASE VERSION 2.1.2 NOTES 
---------------------------
Date: 2002-10-01
-	Installer fixed (samples doesn't appear correct with SDK-only option).

RELEASE VERSION 2.1.1 NOTES
---------------------------
-	MySQL documentation added.

RELEASE VERSION 2.1 NOTES
-------------------------
-	Example added.

RELEASE VERSION 2.0 NOTES
-------------------------
-	Due problems with MySQL++ MySQLDriver is now executed over 
	mylsqlLib.dll only.
-	Fixed errors with null fields.
-	All runs ok!!!!
-	Managed C++ code was removed.

RELEASE VERSION 1.01 NOTES
--------------------------
-	Minor fixes in documentation

RELEASE VERSION 1.00 NOTES
--------------------------
-	Basic funcionality.
-	NULL value is returned as string "NULL" instead of a null pointer.
-	Retrieving values as integers or strings may be ok but with other types
	would be unpredictable results.


