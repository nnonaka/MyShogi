﻿namespace MyShogi.View.Win2D.Setting
{
    partial class EngineOptionSettingDialog
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.engineOptionSettingControl1 = new MyShogi.View.Win2D.Setting.EngineOptionSettingControl();
            this.engineOptionSettingControl2 = new MyShogi.View.Win2D.Setting.EngineOptionSettingControl();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("ＭＳ ゴシック", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1474, 1329);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.engineOptionSettingControl1);
            this.tabPage1.Location = new System.Drawing.Point(8, 42);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1458, 1279);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "共通設定";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.engineOptionSettingControl2);
            this.tabPage2.Location = new System.Drawing.Point(8, 42);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1458, 1279);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "個別設定";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // engineOptionSettingControl1
            // 
            this.engineOptionSettingControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.engineOptionSettingControl1.Location = new System.Drawing.Point(3, 3);
            this.engineOptionSettingControl1.Name = "engineOptionSettingControl1";
            this.engineOptionSettingControl1.Size = new System.Drawing.Size(1452, 1273);
            this.engineOptionSettingControl1.TabIndex = 0;
            // 
            // engineOptionSettingControl2
            // 
            this.engineOptionSettingControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.engineOptionSettingControl2.Location = new System.Drawing.Point(3, 3);
            this.engineOptionSettingControl2.Name = "engineOptionSettingControl2";
            this.engineOptionSettingControl2.Size = new System.Drawing.Size(1452, 1273);
            this.engineOptionSettingControl2.TabIndex = 0;
            // 
            // EngineOptionSettingDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1474, 1329);
            this.Controls.Add(this.tabControl1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EngineOptionSettingDialog";
            this.Text = "エンジンオプション設定";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private EngineOptionSettingControl engineOptionSettingControl2;
        private EngineOptionSettingControl engineOptionSettingControl1;
    }
}