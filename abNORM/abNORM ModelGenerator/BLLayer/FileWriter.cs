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
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace abNORM.ModelGenerator.BLLayer
{
    class FileWriter
    {
        readonly static string template =
            "using System;\r\n" +
            "using System.Collections.Generic;\r\n" +
            "using System.Text;\r\n\r\n" +
            "using abNORM;\r\n\r\n" +
            "namespace $NameSpace$\r\n" +
            "{\r\n" +
            "    [TableName(\"$TableName$\")]\r\n" +
            "    public class $ClassName$\r\n" +
            "    {\r\n" +
            "$Columns$" +
            "    }\r\n" +
            "}";


        public static void write(
            TableDefinition[] model, 
            string nameSpace, 
            string targetDir, 
            string xmlMapping,
            bool askClassname)
        {
            XPathDocument doc = null;
            XPathNavigator nav = null; 
            if (xmlMapping != "")
            {
                doc = new XPathDocument(xmlMapping);
                nav = doc.CreateNavigator();
            }
            ClassNameDlg dlg = new ClassNameDlg();
            foreach (TableDefinition def in model)
            {
                string classname = "";
                if (nav != null)
                {
                    XPathNavigator loc = nav.SelectSingleNode(
                        "/Mapping/Entity[@tablename='" + 
                            def.table.name + 
                            "']/@classname");
                    if (loc != null)
                    {
                        classname = loc.ToString();
                    }
                }
                if (classname == "")
                {
                    classname = def.table.name;
                    if (askClassname)
                    {
                        dlg.tbClassname.Text = classname;
                        dlg.tbTableName.Text = classname;
                        dlg.Focus();
                        dlg.tbClassname.Focus();
                        if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            classname = dlg.tbClassname.Text;
                        }
                    }
                }
                string filecontent = template;
                filecontent = filecontent.Replace("$NameSpace$", nameSpace);
                filecontent = filecontent.Replace(
                    "$TableName$",
                    def.table.owner + "." + def.table.name);
                filecontent = filecontent.Replace("$ClassName$", classname);
                string columns = "";
                foreach (Column c in def.columns)
                {
                    if (c.isPK)
                    {
                        columns += "        [PrimaryKey]\r\n";
                    }
                    columns += "        public " + c.datatype + " " + c.name + ";\r\n";
                }
                filecontent = filecontent.Replace("$Columns$", columns);
                StreamWriter file
                    = new System.IO.StreamWriter(targetDir + "\\" + classname + ".cs", false);
                file.WriteLine(filecontent);
                file.Close();
                file.Dispose();
            }
        }
    }
}
