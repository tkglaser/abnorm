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

namespace abNORM.Example
{
	/// <summary>
	/// This example explaines the Annotations defined by abNORM
	/// </summary>
	class Annotations
	{
		/// This exercise needs a little more elaborate Datamodel.
		/// So here it comes.
		
		[TableName("dm_shows")] // This specifies, that the database table is called dm_shows
		[Sequence("dm_shows_show_id", "")] // This creates a sequence along with the table
		class Show
		{
			[PrimaryKey] // This means, that the Primary Key for this Table is show_id
			[NoInsert] // Never do an insert on this column. Why? See next annotation...
				// The annotation DBType overrides the type, this column gets in the database.
				// This can be used for autoincrementing columns, like here. The database
				// default only works, when an insert does not contain the column, so
				// use it together with NoInsert.
			[DBType("int NOT NULL default dm_shows_show_id.nextval")]
			public int show_id;
			
				// Also use DBType to be more specific. Note, that this is only
				// required, when you want to create a datamodel with abNORM.
				// If the Datamodel is already present in the database,
				// abNORM does not care about stringlengths and such.
			[DBType("varchar(100) not null")] 		
			public string show_name;
		}
		
		[TableName("dm_episodes")] // See the Show class for an explanation of these
		[Sequence("dm_episodes_episode_id", "")]
		class Episode
		{
			[PrimaryKey]
			[NoInsert]
			[DBType("int NOT NULL default dm_episodes_episode_id.nextval")]
			public int episode_id;
			
			// This is new. You can specify a foreign key relationship like this.
			// When you create a Datamodel, a foreign key restriction will be placed
			// into the database (where supported). Break it, and the database 
			// will yell at you.
 			[ForeignKey(typeof(Show), "show_id")]
			public int show_id;
			
			[DBType("varchar(100) not null")]			
			public string episode_name;
			
			// You can define an alias for the column in the database. 
			// Especially useful, when your attributes are reserved words in a 
			// specific database.
			[DBAlias("first_aired")]
			public DateTime date_first_aired;
		}
		
		// The following class is one, that does not belong to the datamodel.
		// Defined in this way, it can only be used for SELECTS, I mean trx.Load()s.
		[TableName("dm_shows", "s")]    // What, two tablenames? That is a definition
		[TableName("dm_episodes", "e")] // for a join. Watch the JoinKey annotation...
		class ShowEpisodes
		{
				// This is the key, the tables are joined on. Note, that we used
				// the shorthand for the tables as defined above.
				// The created SELECT for this class would be:
				// select s.show_id, show_name, episode_name 
				//   from dm_shows s, dm_episodes e where s.show_id = e.show_id
			[JoinKey("s", "e")]
			public int show_id;
			public string show_name;
			public string episode_name;
		}

		// This unspeakable thing is also possible, but I highly
		// discourage it, because it makes all attempts at database abstraction
		// an exercise in futility!
		[CustomSelect("select episode_id, episode_name from dm_episode")]
		class CustomEpisodes
		{
			public int episode_id;
			public string episode_name;
		}

        public static void Run()
        {
            /// First, generate the datamodel into the database.
            /// Details on these commands -> see the Simple.cs
            IConnection con = ConnectionFactory.getConnection(
                ConnectionFactory.DBType.Ingres,
                "<connectionstring>");
            ITransaction trx = con.BeginTransaction();
            List<Type> dataModel = new List<Type>();
            dataModel.Add(typeof(Show));
            dataModel.Add(typeof(Episode));
            trx.CreateDataBaseSchema(dataModel);
            trx.Commit();

            /// Now, lets create some shows...
            Show show = new Show();
            show.show_name = "Doctor Who";

            trx.Save(show);

            show = new Show();
            show.show_name = "Torchwood";

            /// Remember, the show_id is filled by the database
            trx.Save(show);
            trx.Commit();

            /// Now, we load the show named Doctor Who
            /// to see, which id it got

            trx = con.BeginTransaction();
            show = (Show)trx.Load(
                typeof(Show),
                new CriterionEqual("show_name", "Doctor Who"))[0];

            trx.Commit();

            Episode episode = new Episode();
            episode.episode_name = "Rose";
            episode.date_first_aired = new DateTime(2005, 3, 26);

            /// Now, that's pretty important. The foreign key, remember?
            episode.show_id = show.show_id;

            /// The episode_id is filled by the database
            trx.Save(episode);
            trx.Commit();

            /// Cleaning up the database.
            trx = con.BeginTransaction();
            trx.DropDataBaseSchema(dataModel);
            trx.Commit();
        }
	}
}