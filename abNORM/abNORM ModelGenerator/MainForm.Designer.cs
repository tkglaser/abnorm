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
 
namespace abNORM.ModelGenerator
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnLos = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbPwd = new System.Windows.Forms.TextBox();
            this.tbUser = new System.Windows.Forms.TextBox();
            this.tbDatabase = new System.Windows.Forms.TextBox();
            this.tbServer = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbAskClassname = new System.Windows.Forms.CheckBox();
            this.cbGenerateAll = new System.Windows.Forms.CheckBox();
            this.tbTableOwner = new System.Windows.Forms.TextBox();
            this.tbNameSpace = new System.Windows.Forms.TextBox();
            this.btnMappingFile = new System.Windows.Forms.Button();
            this.btnSearchSaveDir = new System.Windows.Forms.Button();
            this.tbMappingFile = new System.Windows.Forms.TextBox();
            this.tbSaveToDir = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.btnClose = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.status = new System.Windows.Forms.ToolStripStatusLabel();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.cbDBType = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLos
            // 
            this.btnLos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLos.Location = new System.Drawing.Point(12, 308);
            this.btnLos.Name = "btnLos";
            this.btnLos.Size = new System.Drawing.Size(75, 23);
            this.btnLos.TabIndex = 14;
            this.btnLos.Text = "Go";
            this.btnLos.UseVisualStyleBackColor = true;
            this.btnLos.Click += new System.EventHandler(this.btnLos_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbDBType);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.tbPwd);
            this.groupBox1.Controls.Add(this.tbUser);
            this.groupBox1.Controls.Add(this.tbDatabase);
            this.groupBox1.Controls.Add(this.tbServer);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(371, 107);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Database Connection";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(203, 77);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Pwd";
            // 
            // tbPwd
            // 
            this.tbPwd.Location = new System.Drawing.Point(247, 74);
            this.tbPwd.Name = "tbPwd";
            this.tbPwd.PasswordChar = '*';
            this.tbPwd.Size = new System.Drawing.Size(100, 20);
            this.tbPwd.TabIndex = 5;
            // 
            // tbUser
            // 
            this.tbUser.Location = new System.Drawing.Point(247, 48);
            this.tbUser.Name = "tbUser";
            this.tbUser.Size = new System.Drawing.Size(100, 20);
            this.tbUser.TabIndex = 4;
            this.tbUser.TextChanged += new System.EventHandler(this.tbUser_TextChanged);
            // 
            // tbDatabase
            // 
            this.tbDatabase.Location = new System.Drawing.Point(63, 74);
            this.tbDatabase.Name = "tbDatabase";
            this.tbDatabase.Size = new System.Drawing.Size(100, 20);
            this.tbDatabase.TabIndex = 3;
            // 
            // tbServer
            // 
            this.tbServer.Location = new System.Drawing.Point(63, 48);
            this.tbServer.Name = "tbServer";
            this.tbServer.Size = new System.Drawing.Size(100, 20);
            this.tbServer.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(203, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "User";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "DB";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Server";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbAskClassname);
            this.groupBox2.Controls.Add(this.cbGenerateAll);
            this.groupBox2.Controls.Add(this.tbTableOwner);
            this.groupBox2.Controls.Add(this.tbNameSpace);
            this.groupBox2.Controls.Add(this.btnMappingFile);
            this.groupBox2.Controls.Add(this.btnSearchSaveDir);
            this.groupBox2.Controls.Add(this.tbMappingFile);
            this.groupBox2.Controls.Add(this.tbSaveToDir);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(12, 125);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(371, 174);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Generationsettings";
            // 
            // cbAskClassname
            // 
            this.cbAskClassname.AutoSize = true;
            this.cbAskClassname.Checked = true;
            this.cbAskClassname.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAskClassname.Location = new System.Drawing.Point(9, 146);
            this.cbAskClassname.Name = "cbAskClassname";
            this.cbAskClassname.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cbAskClassname.Size = new System.Drawing.Size(224, 17);
            this.cbAskClassname.TabIndex = 13;
            this.cbAskClassname.Text = "Ask for Classname if not found in Mapping";
            this.cbAskClassname.UseVisualStyleBackColor = true;
            // 
            // cbGenerateAll
            // 
            this.cbGenerateAll.AutoSize = true;
            this.cbGenerateAll.Location = new System.Drawing.Point(9, 123);
            this.cbGenerateAll.Name = "cbGenerateAll";
            this.cbGenerateAll.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cbGenerateAll.Size = new System.Drawing.Size(201, 17);
            this.cbGenerateAll.TabIndex = 12;
            this.cbGenerateAll.Text = "Generate all objects (including views)";
            this.cbGenerateAll.UseVisualStyleBackColor = true;
            // 
            // tbTableOwner
            // 
            this.tbTableOwner.Location = new System.Drawing.Point(78, 19);
            this.tbTableOwner.Name = "tbTableOwner";
            this.tbTableOwner.Size = new System.Drawing.Size(254, 20);
            this.tbTableOwner.TabIndex = 6;
            // 
            // tbNameSpace
            // 
            this.tbNameSpace.Location = new System.Drawing.Point(78, 97);
            this.tbNameSpace.Name = "tbNameSpace";
            this.tbNameSpace.Size = new System.Drawing.Size(254, 20);
            this.tbNameSpace.TabIndex = 11;
            // 
            // btnMappingFile
            // 
            this.btnMappingFile.Location = new System.Drawing.Point(338, 68);
            this.btnMappingFile.Name = "btnMappingFile";
            this.btnMappingFile.Size = new System.Drawing.Size(27, 23);
            this.btnMappingFile.TabIndex = 10;
            this.btnMappingFile.Text = "...";
            this.btnMappingFile.UseVisualStyleBackColor = true;
            this.btnMappingFile.Click += new System.EventHandler(this.btnMappingFile_Click);
            // 
            // btnSearchSaveDir
            // 
            this.btnSearchSaveDir.Location = new System.Drawing.Point(338, 42);
            this.btnSearchSaveDir.Name = "btnSearchSaveDir";
            this.btnSearchSaveDir.Size = new System.Drawing.Size(27, 23);
            this.btnSearchSaveDir.TabIndex = 8;
            this.btnSearchSaveDir.Text = "...";
            this.btnSearchSaveDir.UseVisualStyleBackColor = true;
            this.btnSearchSaveDir.Click += new System.EventHandler(this.btnSearchSaveDir_Click);
            // 
            // tbMappingFile
            // 
            this.tbMappingFile.Location = new System.Drawing.Point(78, 71);
            this.tbMappingFile.Name = "tbMappingFile";
            this.tbMappingFile.ReadOnly = true;
            this.tbMappingFile.Size = new System.Drawing.Size(254, 20);
            this.tbMappingFile.TabIndex = 9;
            // 
            // tbSaveToDir
            // 
            this.tbSaveToDir.Location = new System.Drawing.Point(78, 45);
            this.tbSaveToDir.Name = "tbSaveToDir";
            this.tbSaveToDir.ReadOnly = true;
            this.tbSaveToDir.Size = new System.Drawing.Size(254, 20);
            this.tbSaveToDir.TabIndex = 7;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Tableowner";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Namespace";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 74);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Mapping";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Save To";
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.Description = "Please choose the Location for the generated files";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(307, 308);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 15;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.status});
            this.statusStrip1.Location = new System.Drawing.Point(0, 337);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(394, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip";
            // 
            // status
            // 
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(0, 17);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "XML-Dateien|*.xml|Alle Dateien|*.*";
            this.openFileDialog.Title = "Bitte XML Mappingdatei angeben";
            // 
            // cbDBType
            // 
            this.cbDBType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDBType.FormattingEnabled = true;
            this.cbDBType.Items.AddRange(new object[] {
            "Ingres",
            "Postgres",
            "Oracle",
            "MySQL",
            "MSSQL"});
            this.cbDBType.Location = new System.Drawing.Point(63, 19);
            this.cbDBType.Name = "cbDBType";
            this.cbDBType.Size = new System.Drawing.Size(284, 21);
            this.cbDBType.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(46, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "DBType";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 359);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnLos);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "abNORM Datamodel Generator";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLos;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSearchSaveDir;
        private System.Windows.Forms.TextBox tbSaveToDir;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.TextBox tbUser;
        private System.Windows.Forms.TextBox tbDatabase;
        private System.Windows.Forms.TextBox tbServer;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbPwd;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel status;
        private System.Windows.Forms.TextBox tbNameSpace;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnMappingFile;
        private System.Windows.Forms.TextBox tbMappingFile;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TextBox tbTableOwner;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox cbGenerateAll;
        private System.Windows.Forms.CheckBox cbAskClassname;
        private System.Windows.Forms.ComboBox cbDBType;
        private System.Windows.Forms.Label label9;
    }
}

