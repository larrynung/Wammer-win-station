﻿namespace Waveface.Component
{
    partial class MsgBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MsgBox));
            this.chkBx = new System.Windows.Forms.CheckBox();
            this.btn1 = new Waveface.Component.XPButton();
            this.btn2 = new Waveface.Component.XPButton();
            this.messageLbl = new System.Windows.Forms.Label();
            this.btn3 = new Waveface.Component.XPButton();
            this.SuspendLayout();
            // 
            // chkBx
            // 
            this.chkBx.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBx.AutoSize = true;
            this.chkBx.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBx.Location = new System.Drawing.Point(14, 123);
            this.chkBx.Name = "chkBx";
            this.chkBx.Size = new System.Drawing.Size(152, 20);
            this.chkBx.TabIndex = 22;
            this.chkBx.Text = "Don\'t show this again";
            this.chkBx.UseVisualStyleBackColor = true;
            this.chkBx.Visible = false;
            // 
            // btn1
            // 
            this.btn1.AdjustImageLocation = new System.Drawing.Point(0, 0);
            this.btn1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn1.AutoSize = true;
            this.btn1.BtnShape = Waveface.Component.emunType.BtnShape.Rectangle;
            this.btn1.BtnStyle = Waveface.Component.emunType.XPStyle.Silver;
            this.btn1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn1.Location = new System.Drawing.Point(279, 118);
            this.btn1.Name = "btn1";
            this.btn1.Size = new System.Drawing.Size(87, 28);
            this.btn1.TabIndex = 5;
            this.btn1.Text = "Button1";
            this.btn1.UseVisualStyleBackColor = true;
            this.btn1.Visible = false;
            this.btn1.Click += new System.EventHandler(this.btn_Click);
            // 
            // btn2
            // 
            this.btn2.AdjustImageLocation = new System.Drawing.Point(0, 0);
            this.btn2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn2.AutoSize = true;
            this.btn2.BtnShape = Waveface.Component.emunType.BtnShape.Rectangle;
            this.btn2.BtnStyle = Waveface.Component.emunType.XPStyle.Silver;
            this.btn2.Location = new System.Drawing.Point(373, 118);
            this.btn2.Name = "btn2";
            this.btn2.Size = new System.Drawing.Size(87, 28);
            this.btn2.TabIndex = 6;
            this.btn2.Text = "Button2";
            this.btn2.UseVisualStyleBackColor = true;
            this.btn2.Visible = false;
            this.btn2.Click += new System.EventHandler(this.btn_Click);
            // 
            // messageLbl
            // 
            this.messageLbl.AutoSize = true;
            this.messageLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.messageLbl.Location = new System.Drawing.Point(68, 10);
            this.messageLbl.Name = "messageLbl";
            this.messageLbl.Size = new System.Drawing.Size(73, 16);
            this.messageLbl.TabIndex = 19;
            this.messageLbl.Text = "[Message]";
            // 
            // btn3
            // 
            this.btn3.AdjustImageLocation = new System.Drawing.Point(0, 0);
            this.btn3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn3.AutoSize = true;
            this.btn3.BtnShape = Waveface.Component.emunType.BtnShape.Rectangle;
            this.btn3.BtnStyle = Waveface.Component.emunType.XPStyle.Silver;
            this.btn3.Location = new System.Drawing.Point(468, 118);
            this.btn3.Name = "btn3";
            this.btn3.Size = new System.Drawing.Size(87, 28);
            this.btn3.TabIndex = 7;
            this.btn3.Text = "Button3";
            this.btn3.UseVisualStyleBackColor = true;
            this.btn3.Visible = false;
            this.btn3.Click += new System.EventHandler(this.btn_Click);
            // 
            // MsgBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn1;
            this.ClientSize = new System.Drawing.Size(566, 161);
            this.ControlBox = false;
            this.Controls.Add(this.btn3);
            this.Controls.Add(this.chkBx);
            this.Controls.Add(this.btn1);
            this.Controls.Add(this.btn2);
            this.Controls.Add(this.messageLbl);
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MsgBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "[Title]";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.DialogBox_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkBx;
        private XPButton btn1;
        private XPButton btn2;
        private System.Windows.Forms.Label messageLbl;
        private XPButton btn3;
    }
}