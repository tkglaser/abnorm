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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace abNORM.ModelGenerator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnLos_Click(object sender, EventArgs e)
        {
            status.Text = "Analyzing Datamodel...";
            this.Refresh();
            BLLayer.TableDefinition[] def =
                BLLayer.GeneratorFactory.createGenerator(
                    getSelectedDBType(),
                    tbServer.Text,
                    tbDatabase.Text,
                    tbUser.Text,
                    tbPwd.Text).generate(
                    tbTableOwner.Text,
                    cbGenerateAll.Checked);
            status.Text = "Generating Files...";
            this.Refresh();
            BLLayer.FileWriter.write(
                def,
                tbNameSpace.Text,
                tbSaveToDir.Text,
                tbMappingFile.Text,
                cbAskClassname.Checked);
            status.Text = "Done.";
        }

        private ConnectionFactory.DBType getSelectedDBType()
        {
            switch (cbDBType.SelectedItem.ToString())
            {
                case "Ingres":
                    return ConnectionFactory.DBType.Ingres;
                case "Postgres":
                    return ConnectionFactory.DBType.Postgres;
                case "Oracle":
                    return ConnectionFactory.DBType.Oracle;
                case "MySQL":
                    return ConnectionFactory.DBType.MySQL;
                case "MSSQL":
                    return ConnectionFactory.DBType.MSSQL;
            }
            throw new Exception("Unknown DBType");
        }

        private void btnSearchSaveDir_Click(object sender, EventArgs e)
        {
            DialogResult res = folderBrowserDialog.ShowDialog();
            if (res == DialogResult.OK)
            {
                tbSaveToDir.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMappingFile_Click(object sender, EventArgs e)
        {
            DialogResult res = openFileDialog.ShowDialog();
            if (res == DialogResult.OK)
            {
                tbMappingFile.Text = openFileDialog.FileName;
            }
        }

        private void tbUser_TextChanged(object sender, EventArgs e)
        {
            tbTableOwner.Text = tbUser.Text;
        }

    }
}