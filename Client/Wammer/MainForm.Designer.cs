﻿namespace Waveface
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
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.panelPost = new System.Windows.Forms.Panel();
            this.postsArea = new Waveface.PostArea();
            this.splitterLeft = new System.Windows.Forms.Splitter();
            this.panelLeftInfo = new System.Windows.Forms.Panel();
            this.leftArea = new Waveface.LeftArea();
            this.panelMain = new System.Windows.Forms.Panel();
            this.detailView = new Waveface.DetailView();
            this.itemCountLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.connectedStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.connectedImageLabel = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.mnuTray = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.restoreMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.screenShotMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.regionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.screenMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.preferencesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.timerDelayPost = new System.Windows.Forms.Timer(this.components);
            this.splitterRight = new System.Windows.Forms.Splitter();
            this.timerGetNewestPost = new System.Windows.Forms.Timer(this.components);
            this.timerFetchOlderPost = new System.Windows.Forms.Timer(this.components);
            this.panelTop = new System.Windows.Forms.Panel();
            this.linkLabelLogin = new System.Windows.Forms.LinkLabel();
            this.labelName = new System.Windows.Forms.Label();
            this.pictureBoxAvatar = new System.Windows.Forms.PictureBox();
            this.pictureBoxPost = new System.Windows.Forms.PictureBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.panelLeft.SuspendLayout();
            this.panelPost.SuspendLayout();
            this.panelLeftInfo.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.mnuTray.SuspendLayout();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAvatar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPost)).BeginInit();
            this.SuspendLayout();
            // 
            // BottomToolStripPanel
            // 
            this.BottomToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.BottomToolStripPanel.Name = "BottomToolStripPanel";
            this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.BottomToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // TopToolStripPanel
            // 
            this.TopToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.TopToolStripPanel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.TopToolStripPanel.Name = "TopToolStripPanel";
            this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.TopToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // RightToolStripPanel
            // 
            this.RightToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.RightToolStripPanel.Name = "RightToolStripPanel";
            this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.RightToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // LeftToolStripPanel
            // 
            this.LeftToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftToolStripPanel.Name = "LeftToolStripPanel";
            this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.LeftToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // ContentPanel
            // 
            this.ContentPanel.Size = new System.Drawing.Size(1078, 549);
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.panelPost);
            this.panelLeft.Controls.Add(this.splitterLeft);
            this.panelLeft.Controls.Add(this.panelLeftInfo);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 56);
            this.panelLeft.Margin = new System.Windows.Forms.Padding(0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(480, 545);
            this.panelLeft.TabIndex = 9;
            // 
            // panelPost
            // 
            this.panelPost.Controls.Add(this.postsArea);
            this.panelPost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPost.Location = new System.Drawing.Point(162, 0);
            this.panelPost.Margin = new System.Windows.Forms.Padding(0);
            this.panelPost.Name = "panelPost";
            this.panelPost.Size = new System.Drawing.Size(318, 545);
            this.panelPost.TabIndex = 7;
            // 
            // postsArea
            // 
            this.postsArea.BackColor = System.Drawing.SystemColors.Window;
            this.postsArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.postsArea.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.postsArea.Location = new System.Drawing.Point(0, 0);
            this.postsArea.Margin = new System.Windows.Forms.Padding(0);
            this.postsArea.Name = "postsArea";
            this.postsArea.Size = new System.Drawing.Size(318, 545);
            this.postsArea.TabIndex = 4;
            // 
            // splitterLeft
            // 
            this.splitterLeft.Location = new System.Drawing.Point(160, 0);
            this.splitterLeft.Margin = new System.Windows.Forms.Padding(0);
            this.splitterLeft.Name = "splitterLeft";
            this.splitterLeft.Size = new System.Drawing.Size(2, 545);
            this.splitterLeft.TabIndex = 5;
            this.splitterLeft.TabStop = false;
            // 
            // panelLeftInfo
            // 
            this.panelLeftInfo.Controls.Add(this.leftArea);
            this.panelLeftInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeftInfo.Location = new System.Drawing.Point(0, 0);
            this.panelLeftInfo.Margin = new System.Windows.Forms.Padding(0);
            this.panelLeftInfo.Name = "panelLeftInfo";
            this.panelLeftInfo.Size = new System.Drawing.Size(160, 545);
            this.panelLeftInfo.TabIndex = 6;
            // 
            // leftArea
            // 
            this.leftArea.BackColor = System.Drawing.Color.Transparent;
            this.leftArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.leftArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftArea.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.leftArea.Location = new System.Drawing.Point(0, 0);
            this.leftArea.Name = "leftArea";
            this.leftArea.Size = new System.Drawing.Size(160, 545);
            this.leftArea.TabIndex = 3;
            this.leftArea.TabStop = false;
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.detailView);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(482, 56);
            this.panelMain.Margin = new System.Windows.Forms.Padding(0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(596, 545);
            this.panelMain.TabIndex = 11;
            // 
            // detailView
            // 
            this.detailView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.detailView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.detailView.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.detailView.Location = new System.Drawing.Point(0, 0);
            this.detailView.Margin = new System.Windows.Forms.Padding(0);
            this.detailView.MinimumSize = new System.Drawing.Size(200, 2);
            this.detailView.Name = "detailView";
            this.detailView.Post = null;
            this.detailView.Size = new System.Drawing.Size(596, 545);
            this.detailView.TabIndex = 8;
            this.detailView.User = null;
            // 
            // itemCountLabel
            // 
            this.itemCountLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.itemCountLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.itemCountLabel.Name = "itemCountLabel";
            this.itemCountLabel.Padding = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.itemCountLabel.Size = new System.Drawing.Size(64, 20);
            this.itemCountLabel.Text = "{0} Items";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(739, 20);
            this.toolStripStatusLabel2.Spring = true;
            // 
            // connectedStatusLabel
            // 
            this.connectedStatusLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.connectedStatusLabel.Name = "connectedStatusLabel";
            this.connectedStatusLabel.Size = new System.Drawing.Size(154, 20);
            this.connectedStatusLabel.Text = "All folders are up to date.";
            // 
            // connectedImageLabel
            // 
            this.connectedImageLabel.Image = ((System.Drawing.Image)(resources.GetObject("connectedImageLabel.Image")));
            this.connectedImageLabel.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.connectedImageLabel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.connectedImageLabel.Margin = new System.Windows.Forms.Padding(0);
            this.connectedImageLabel.Name = "connectedImageLabel";
            this.connectedImageLabel.Size = new System.Drawing.Size(102, 25);
            this.connectedImageLabel.Text = "Connected";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.toolStripStatusLabel4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None;
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(4, 20);
            this.toolStripStatusLabel4.Text = "toolStripStatusLabel4";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemCountLabel,
            this.toolStripStatusLabel2,
            this.connectedStatusLabel,
            this.connectedImageLabel,
            this.toolStripStatusLabel4});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1078, 25);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "19 Items";
            // 
            // mnuTray
            // 
            this.mnuTray.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.restoreMenuItem,
            this.toolStripMenuItem1,
            this.screenShotMenu,
            this.toolStripMenuItem3,
            this.preferencesMenuItem,
            this.toolStripMenuItem2,
            this.mnuExit});
            this.mnuTray.Name = "mnuTree";
            this.mnuTray.Size = new System.Drawing.Size(151, 110);
            // 
            // restoreMenuItem
            // 
            this.restoreMenuItem.Name = "restoreMenuItem";
            this.restoreMenuItem.Size = new System.Drawing.Size(150, 22);
            this.restoreMenuItem.Text = "Restore";
            this.restoreMenuItem.Click += new System.EventHandler(this.restoreMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(147, 6);
            // 
            // screenShotMenu
            // 
            this.screenShotMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.regionMenuItem,
            this.windowsMenuItem,
            this.screenMenuItem});
            this.screenShotMenu.Name = "screenShotMenu";
            this.screenShotMenu.Size = new System.Drawing.Size(150, 22);
            this.screenShotMenu.Text = "Screen Shot";
            // 
            // regionMenuItem
            // 
            this.regionMenuItem.Name = "regionMenuItem";
            this.regionMenuItem.Size = new System.Drawing.Size(128, 22);
            this.regionMenuItem.Text = "Region";
            this.regionMenuItem.Click += new System.EventHandler(this.regionMenuItem_Click);
            // 
            // windowsMenuItem
            // 
            this.windowsMenuItem.Name = "windowsMenuItem";
            this.windowsMenuItem.Size = new System.Drawing.Size(128, 22);
            this.windowsMenuItem.Text = "Windows";
            this.windowsMenuItem.Click += new System.EventHandler(this.windowsMenuItem_Click);
            // 
            // screenMenuItem
            // 
            this.screenMenuItem.Name = "screenMenuItem";
            this.screenMenuItem.Size = new System.Drawing.Size(128, 22);
            this.screenMenuItem.Text = "Desktop";
            this.screenMenuItem.Click += new System.EventHandler(this.screenMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(147, 6);
            // 
            // preferencesMenuItem
            // 
            this.preferencesMenuItem.Name = "preferencesMenuItem";
            this.preferencesMenuItem.Size = new System.Drawing.Size(150, 22);
            this.preferencesMenuItem.Text = "Preferences...";
            this.preferencesMenuItem.Click += new System.EventHandler(this.preferencesMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(147, 6);
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(150, 22);
            this.mnuExit.Text = "Exit";
            this.mnuExit.Click += new System.EventHandler(this.OnMenuExitClick);
            // 
            // timerDelayPost
            // 
            this.timerDelayPost.Enabled = true;
            this.timerDelayPost.Interval = 500;
            this.timerDelayPost.Tick += new System.EventHandler(this.timerDelayPost_Tick);
            // 
            // splitterRight
            // 
            this.splitterRight.Location = new System.Drawing.Point(480, 56);
            this.splitterRight.Margin = new System.Windows.Forms.Padding(0);
            this.splitterRight.Name = "splitterRight";
            this.splitterRight.Size = new System.Drawing.Size(2, 545);
            this.splitterRight.TabIndex = 10;
            this.splitterRight.TabStop = false;
            // 
            // timerGetNewestPost
            // 
            this.timerGetNewestPost.Enabled = true;
            this.timerGetNewestPost.Interval = 10000;
            this.timerGetNewestPost.Tick += new System.EventHandler(this.timerGetNewestPost_Tick);
            // 
            // timerFetchOlderPost
            // 
            this.timerFetchOlderPost.Interval = 1000;
            this.timerFetchOlderPost.Tick += new System.EventHandler(this.timerFetchOlderPost_Tick);
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.Orange;
            this.panelTop.Controls.Add(this.linkLabelLogin);
            this.panelTop.Controls.Add(this.labelName);
            this.panelTop.Controls.Add(this.pictureBoxAvatar);
            this.panelTop.Controls.Add(this.pictureBoxPost);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Margin = new System.Windows.Forms.Padding(0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1078, 56);
            this.panelTop.TabIndex = 12;
            // 
            // linkLabelLogin
            // 
            this.linkLabelLogin.AutoSize = true;
            this.linkLabelLogin.Location = new System.Drawing.Point(56, 35);
            this.linkLabelLogin.Name = "linkLabelLogin";
            this.linkLabelLogin.Size = new System.Drawing.Size(36, 14);
            this.linkLabelLogin.TabIndex = 3;
            this.linkLabelLogin.TabStop = true;
            this.linkLabelLogin.Text = "Login";
            this.linkLabelLogin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLabelLogin.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelLogin_LinkClicked);
            // 
            // labelName
            // 
            this.labelName.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelName.ForeColor = System.Drawing.Color.White;
            this.labelName.Location = new System.Drawing.Point(56, 12);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(83, 23);
            this.labelName.TabIndex = 2;
            this.labelName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBoxAvatar
            // 
            this.pictureBoxAvatar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxAvatar.Location = new System.Drawing.Point(10, 9);
            this.pictureBoxAvatar.Name = "pictureBoxAvatar";
            this.pictureBoxAvatar.Size = new System.Drawing.Size(40, 40);
            this.pictureBoxAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxAvatar.TabIndex = 1;
            this.pictureBoxAvatar.TabStop = false;
            // 
            // pictureBoxPost
            // 
            this.pictureBoxPost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxPost.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxPost.Image")));
            this.pictureBoxPost.Location = new System.Drawing.Point(985, 17);
            this.pictureBoxPost.Name = "pictureBoxPost";
            this.pictureBoxPost.Size = new System.Drawing.Size(87, 27);
            this.pictureBoxPost.TabIndex = 0;
            this.pictureBoxPost.TabStop = false;
            this.pictureBoxPost.Click += new System.EventHandler(this.pictureBoxPost_Click);
            // 
            // statusStrip2
            // 
            this.statusStrip.Location = new System.Drawing.Point(0, 601);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1078, 22);
            this.statusStrip.TabIndex = 13;
            this.statusStrip.Text = "statusStrip2";
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1078, 623);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.splitterRight);
            this.Controls.Add(this.panelLeft);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.statusStrip);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Waveface";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.Form_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form_DragEnter);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.Form_DragOver);
            this.DragLeave += new System.EventHandler(this.Form_DragLeave);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.panelLeft.ResumeLayout(false);
            this.panelPost.ResumeLayout(false);
            this.panelLeftInfo.ResumeLayout(false);
            this.panelMain.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.mnuTray.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAvatar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPost)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
        
        private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
        private System.Windows.Forms.ToolStripPanel TopToolStripPanel;
        private System.Windows.Forms.ToolStripPanel RightToolStripPanel;
        private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
        private System.Windows.Forms.ToolStripContentPanel ContentPanel;
        private DetailView detailView;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Splitter splitterLeft;
        private System.Windows.Forms.Splitter splitterRight;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelPost;
        private System.Windows.Forms.Panel panelLeftInfo;
        private PostArea postsArea;
        private System.Windows.Forms.ToolStripStatusLabel itemCountLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel connectedStatusLabel;
        private System.Windows.Forms.ToolStripSplitButton connectedImageLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ContextMenuStrip mnuTray;
        private System.Windows.Forms.ToolStripMenuItem screenShotMenu;
        private System.Windows.Forms.ToolStripMenuItem regionMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem screenMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.ToolStripMenuItem restoreMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.Timer timerDelayPost;
        private System.Windows.Forms.Timer timerGetNewestPost;
        private System.Windows.Forms.Timer timerFetchOlderPost;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.PictureBox pictureBoxPost;
        private System.Windows.Forms.PictureBox pictureBoxAvatar;
        private System.Windows.Forms.Label labelName;
        private LeftArea leftArea;
        private System.Windows.Forms.LinkLabel linkLabelLogin;
        private System.Windows.Forms.ToolStripMenuItem preferencesMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.StatusStrip statusStrip;
	}
}
