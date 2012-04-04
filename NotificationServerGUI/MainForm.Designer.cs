namespace NotificationServerGUI
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.txtContent = new System.Windows.Forms.TextBox();
            this.lstGroups = new System.Windows.Forms.ListBox();
            this.lblGroups = new System.Windows.Forms.Label();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.gboxRecipients = new System.Windows.Forms.GroupBox();
            this.lstComputers = new System.Windows.Forms.ListBox();
            this.lblComputers = new System.Windows.Forms.Label();
            this.gboxContent = new System.Windows.Forms.GroupBox();
            this.gboxPreferences = new System.Windows.Forms.GroupBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lblIcon = new System.Windows.Forms.Label();
            this.cmbIcon = new System.Windows.Forms.ComboBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.gboxRecipients.SuspendLayout();
            this.gboxContent.SuspendLayout();
            this.gboxPreferences.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtContent
            // 
            this.txtContent.Location = new System.Drawing.Point(6, 13);
            this.txtContent.Multiline = true;
            this.txtContent.Name = "txtContent";
            this.txtContent.Size = new System.Drawing.Size(320, 80);
            this.txtContent.TabIndex = 2;
            // 
            // lstGroups
            // 
            this.lstGroups.FormattingEnabled = true;
            this.lstGroups.Location = new System.Drawing.Point(6, 31);
            this.lstGroups.Name = "lstGroups";
            this.lstGroups.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstGroups.Size = new System.Drawing.Size(157, 108);
            this.lstGroups.Sorted = true;
            this.lstGroups.TabIndex = 0;
            this.lstGroups.SelectedIndexChanged += new System.EventHandler(this.lstGroups_SelectedIndexChanged);
            // 
            // lblGroups
            // 
            this.lblGroups.AutoSize = true;
            this.lblGroups.Location = new System.Drawing.Point(3, 15);
            this.lblGroups.Name = "lblGroups";
            this.lblGroups.Size = new System.Drawing.Size(105, 13);
            this.lblGroups.TabIndex = 2;
            this.lblGroups.Text = "Subscription Groups:";
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(225, 346);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(121, 46);
            this.btnSubmit.TabIndex = 5;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // gboxRecipients
            // 
            this.gboxRecipients.Controls.Add(this.lstComputers);
            this.gboxRecipients.Controls.Add(this.lblComputers);
            this.gboxRecipients.Controls.Add(this.lstGroups);
            this.gboxRecipients.Controls.Add(this.lblGroups);
            this.gboxRecipients.Location = new System.Drawing.Point(12, 12);
            this.gboxRecipients.Name = "gboxRecipients";
            this.gboxRecipients.Size = new System.Drawing.Size(334, 145);
            this.gboxRecipients.TabIndex = 9;
            this.gboxRecipients.TabStop = false;
            this.gboxRecipients.Text = "Message recipients";
            // 
            // lstComputers
            // 
            this.lstComputers.FormattingEnabled = true;
            this.lstComputers.Location = new System.Drawing.Point(169, 31);
            this.lstComputers.Name = "lstComputers";
            this.lstComputers.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstComputers.Size = new System.Drawing.Size(157, 108);
            this.lstComputers.Sorted = true;
            this.lstComputers.TabIndex = 1;
            this.lstComputers.SelectedIndexChanged += new System.EventHandler(this.lstComputers_SelectedIndexChanged);
            // 
            // lblComputers
            // 
            this.lblComputers.Location = new System.Drawing.Point(166, 15);
            this.lblComputers.Name = "lblComputers";
            this.lblComputers.Size = new System.Drawing.Size(105, 13);
            this.lblComputers.TabIndex = 4;
            this.lblComputers.Text = "Single computers";
            // 
            // gboxContent
            // 
            this.gboxContent.Controls.Add(this.txtContent);
            this.gboxContent.Location = new System.Drawing.Point(12, 163);
            this.gboxContent.Name = "gboxContent";
            this.gboxContent.Size = new System.Drawing.Size(334, 98);
            this.gboxContent.TabIndex = 10;
            this.gboxContent.TabStop = false;
            this.gboxContent.Text = "Message content";
            // 
            // gboxPreferences
            // 
            this.gboxPreferences.Controls.Add(this.lblTitle);
            this.gboxPreferences.Controls.Add(this.txtTitle);
            this.gboxPreferences.Controls.Add(this.lblIcon);
            this.gboxPreferences.Controls.Add(this.cmbIcon);
            this.gboxPreferences.Location = new System.Drawing.Point(12, 267);
            this.gboxPreferences.Name = "gboxPreferences";
            this.gboxPreferences.Size = new System.Drawing.Size(334, 73);
            this.gboxPreferences.TabIndex = 11;
            this.gboxPreferences.TabStop = false;
            this.gboxPreferences.Text = "Optional notification preferences";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(6, 49);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(68, 13);
            this.lblTitle.TabIndex = 11;
            this.lblTitle.Text = "Window title:";
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(84, 46);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(242, 20);
            this.txtTitle.TabIndex = 4;
            // 
            // lblIcon
            // 
            this.lblIcon.AutoSize = true;
            this.lblIcon.Location = new System.Drawing.Point(6, 22);
            this.lblIcon.Name = "lblIcon";
            this.lblIcon.Size = new System.Drawing.Size(76, 13);
            this.lblIcon.TabIndex = 9;
            this.lblIcon.Text = "Message icon:";
            // 
            // cmbIcon
            // 
            this.cmbIcon.FormattingEnabled = true;
            this.cmbIcon.Location = new System.Drawing.Point(84, 19);
            this.cmbIcon.Name = "cmbIcon";
            this.cmbIcon.Size = new System.Drawing.Size(121, 21);
            this.cmbIcon.TabIndex = 3;
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(11, 346);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(109, 46);
            this.btnReset.TabIndex = 6;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 395);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.gboxPreferences);
            this.Controls.Add(this.gboxContent);
            this.Controls.Add(this.gboxRecipients);
            this.Controls.Add(this.btnSubmit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Notification Server";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.gboxRecipients.ResumeLayout(false);
            this.gboxRecipients.PerformLayout();
            this.gboxContent.ResumeLayout(false);
            this.gboxContent.PerformLayout();
            this.gboxPreferences.ResumeLayout(false);
            this.gboxPreferences.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtContent;
        private System.Windows.Forms.ListBox lstGroups;
        private System.Windows.Forms.Label lblGroups;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.GroupBox gboxRecipients;
        private System.Windows.Forms.ListBox lstComputers;
        private System.Windows.Forms.Label lblComputers;
        private System.Windows.Forms.GroupBox gboxContent;
        private System.Windows.Forms.GroupBox gboxPreferences;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblIcon;
        private System.Windows.Forms.ComboBox cmbIcon;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Timer timer1;

    }
}

