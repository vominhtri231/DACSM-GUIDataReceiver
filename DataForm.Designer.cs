using System.Windows.Forms;

namespace GUIDataReceiver
{
    partial class DataForm
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
            this.labelName = new System.Windows.Forms.Label();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.button_makeCm = new System.Windows.Forms.Button();
            this.but_viewfile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.BackColor = System.Drawing.SystemColors.Control;
            this.labelName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelName.ForeColor = System.Drawing.Color.Crimson;
            this.labelName.Location = new System.Drawing.Point(57, 19);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(58, 24);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "name";
            // 
            // mainPanel
            // 
            this.mainPanel.AutoScroll = true;
            this.mainPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.mainPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mainPanel.Location = new System.Drawing.Point(22, 66);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(1213, 513);
            this.mainPanel.TabIndex = 1;
            // 
            // button_makeCm
            // 
            this.button_makeCm.Location = new System.Drawing.Point(990, 22);
            this.button_makeCm.Name = "button_makeCm";
            this.button_makeCm.Size = new System.Drawing.Size(75, 23);
            this.button_makeCm.TabIndex = 2;
            this.button_makeCm.Text = "Gửi lệnh";
            this.button_makeCm.UseVisualStyleBackColor = true;
            this.button_makeCm.Click += new System.EventHandler(this.button_makeCm_Click);
            // 
            // but_viewfile
            // 
            this.but_viewfile.Location = new System.Drawing.Point(1102, 20);
            this.but_viewfile.Name = "but_viewfile";
            this.but_viewfile.Size = new System.Drawing.Size(75, 23);
            this.but_viewfile.TabIndex = 3;
            this.but_viewfile.Text = "xem các file";
            this.but_viewfile.UseVisualStyleBackColor = true;
            this.but_viewfile.Click += new System.EventHandler(this.but_viewfile_Click);
            // 
            // DataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1247, 579);
            this.Controls.Add(this.but_viewfile);
            this.Controls.Add(this.button_makeCm);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.labelName);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Name = "DataForm";
            this.Text = "DataForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DataForm_FormClosed);
            this.Load += new System.EventHandler(this.DataForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Panel mainPanel;
        private Button button_makeCm;
        private Button but_viewfile;
    }
}