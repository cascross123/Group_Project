﻿using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.listBoxChat = new System.Windows.Forms.ListBox();
            this.textBoxMsg = new System.Windows.Forms.TextBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter your name:";
          
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.textBoxName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(80, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(410, 48);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Details";
            // 
            // textBoxName
            // 
            this.textBoxName.AcceptsReturn = true;
            this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxName.Location = new System.Drawing.Point(112, 15);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(292, 20);
            this.textBoxName.TabIndex = 1;
            this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            // 
            // buttonConnect
            // 
            this.buttonConnect.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonConnect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonConnect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonConnect.Location = new System.Drawing.Point(192, 66);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(128, 25);
            this.buttonConnect.TabIndex = 2;
            this.buttonConnect.Text = "Connect!";
            this.buttonConnect.UseCompatibleTextRendering = true;
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // listBoxChat
            // 
            this.listBoxChat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxChat.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.listBoxChat.FormattingEnabled = true;
            this.listBoxChat.ItemHeight = 16;
            this.listBoxChat.Location = new System.Drawing.Point(12, 97);
            this.listBoxChat.Name = "listBoxChat";
            this.listBoxChat.Size = new System.Drawing.Size(484, 348);
            this.listBoxChat.TabIndex = 3;
            this.listBoxChat.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBoxChat_DrawItem);
            this.listBoxChat.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.listBoxChat_MeasureItem);
           // this.listBoxChat.SelectedIndexChanged += new System.EventHandler(this.listBoxChat_SelectedIndexChanged);
            // 
            // textBoxMsg
            // 
            this.textBoxMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMsg.Location = new System.Drawing.Point(12, 451);
            this.textBoxMsg.Multiline = true;
            this.textBoxMsg.Name = "textBoxMsg";
            this.textBoxMsg.Size = new System.Drawing.Size(344, 38);
            this.textBoxMsg.TabIndex = 4;
            this.textBoxMsg.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckEnter);
            // 
            // buttonSend
            // 
            this.buttonSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSend.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonSend.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonSend.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonSend.Location = new System.Drawing.Point(362, 451);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(128, 38);
            this.buttonSend.TabIndex = 5;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 529);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.textBoxMsg);
            this.Controls.Add(this.listBoxChat);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.groupBox1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PC Part Picker";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.ListBox listBoxChat;
        private System.Windows.Forms.TextBox textBoxMsg;
        private System.Windows.Forms.Button buttonSend;
    }
}

