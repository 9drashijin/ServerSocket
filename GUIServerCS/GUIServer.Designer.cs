namespace GUIServerCS
{
    partial class ServerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerForm));
            this.TestMenuTree = new System.Windows.Forms.TreeView();
            this.ServerInfo = new System.Windows.Forms.RichTextBox();
            this.ServerInfoLabel = new System.Windows.Forms.Label();
            this.ClientInfo = new System.Windows.Forms.RichTextBox();
            this.ClientInfoLabel = new System.Windows.Forms.Label();
            this.receiveDisplay = new System.Windows.Forms.RichTextBox();
            this.ReceiveData = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.logoLabel = new System.Windows.Forms.Label();
            this.SendButton = new System.Windows.Forms.Button();
            this.ClearButton = new System.Windows.Forms.Button();
            this.ExpectBox = new System.Windows.Forms.RichTextBox();
            this.Desc = new System.Windows.Forms.Label();
            this.Expect = new System.Windows.Forms.Label();
            this.DescBox = new System.Windows.Forms.RichTextBox();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.percentageDisplay = new System.Windows.Forms.ToolStripStatusLabel();
            this.totalTest = new System.Windows.Forms.ToolStripStatusLabel();
            this.currentTest = new System.Windows.Forms.ToolStripStatusLabel();
            this.currentModule = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timerDisplay = new System.Windows.Forms.Label();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TestMenuTree
            // 
            this.TestMenuTree.BackColor = System.Drawing.SystemColors.Control;
            this.TestMenuTree.CheckBoxes = true;
            resources.ApplyResources(this.TestMenuTree, "TestMenuTree");
            this.TestMenuTree.Name = "TestMenuTree";
            this.TestMenuTree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.TestMenuTree_CheckedChanged);
            this.TestMenuTree.NodeMouseHover += new System.Windows.Forms.TreeNodeMouseHoverEventHandler(this.TestMenuTree_NodeMouseHover);
            this.TestMenuTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TestMenuTree_AfterSelect);
            // 
            // ServerInfo
            // 
            this.ServerInfo.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.ServerInfo, "ServerInfo");
            this.ServerInfo.Name = "ServerInfo";
            this.ServerInfo.ReadOnly = true;
            // 
            // ServerInfoLabel
            // 
            resources.ApplyResources(this.ServerInfoLabel, "ServerInfoLabel");
            this.ServerInfoLabel.Name = "ServerInfoLabel";
            // 
            // ClientInfo
            // 
            this.ClientInfo.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.ClientInfo, "ClientInfo");
            this.ClientInfo.Name = "ClientInfo";
            this.ClientInfo.ReadOnly = true;
            // 
            // ClientInfoLabel
            // 
            resources.ApplyResources(this.ClientInfoLabel, "ClientInfoLabel");
            this.ClientInfoLabel.Name = "ClientInfoLabel";
            // 
            // receiveDisplay
            // 
            resources.ApplyResources(this.receiveDisplay, "receiveDisplay");
            this.receiveDisplay.Name = "receiveDisplay";
            this.receiveDisplay.ReadOnly = true;
            // 
            // ReceiveData
            // 
            resources.ApplyResources(this.ReceiveData, "ReceiveData");
            this.ReceiveData.Name = "ReceiveData";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Menu;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // logoLabel
            // 
            resources.ApplyResources(this.logoLabel, "logoLabel");
            this.logoLabel.Name = "logoLabel";
            // 
            // SendButton
            // 
            resources.ApplyResources(this.SendButton, "SendButton");
            this.SendButton.Name = "SendButton";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // ClearButton
            // 
            resources.ApplyResources(this.ClearButton, "ClearButton");
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // ExpectBox
            // 
            resources.ApplyResources(this.ExpectBox, "ExpectBox");
            this.ExpectBox.Name = "ExpectBox";
            this.ExpectBox.ReadOnly = true;
            // 
            // Desc
            // 
            resources.ApplyResources(this.Desc, "Desc");
            this.Desc.Name = "Desc";
            // 
            // Expect
            // 
            resources.ApplyResources(this.Expect, "Expect");
            this.Expect.Name = "Expect";
            // 
            // DescBox
            // 
            resources.ApplyResources(this.DescBox, "DescBox");
            this.DescBox.Name = "DescBox";
            this.DescBox.ReadOnly = true;
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.WorkerSupportsCancellation = true;
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.percentageDisplay,
            this.totalTest,
            this.currentTest,
            this.currentModule});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.Stretch = false;
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Margin = new System.Windows.Forms.Padding(12, 3, 1, 3);
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            resources.ApplyResources(this.toolStripProgressBar1, "toolStripProgressBar1");
            this.toolStripProgressBar1.Step = 1;
            this.toolStripProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // percentageDisplay
            // 
            resources.ApplyResources(this.percentageDisplay, "percentageDisplay");
            this.percentageDisplay.Margin = new System.Windows.Forms.Padding(3, 3, 0, 2);
            this.percentageDisplay.Name = "percentageDisplay";
            // 
            // totalTest
            // 
            resources.ApplyResources(this.totalTest, "totalTest");
            this.totalTest.Margin = new System.Windows.Forms.Padding(45, 3, 0, 2);
            this.totalTest.Name = "totalTest";
            // 
            // currentTest
            // 
            resources.ApplyResources(this.currentTest, "currentTest");
            this.currentTest.Margin = new System.Windows.Forms.Padding(20, 3, 0, 2);
            this.currentTest.Name = "currentTest";
            // 
            // currentModule
            // 
            this.currentModule.Margin = new System.Windows.Forms.Padding(30, 3, 0, 2);
            this.currentModule.Name = "currentModule";
            resources.ApplyResources(this.currentModule, "currentModule");
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timerDisplay
            // 
            resources.ApplyResources(this.timerDisplay, "timerDisplay");
            this.timerDisplay.Name = "timerDisplay";
            // 
            // timer2
            // 
            this.timer2.Interval = 2500;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // ServerForm
            // 
            this.AcceptButton = this.SendButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.timerDisplay);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.Expect);
            this.Controls.Add(this.DescBox);
            this.Controls.Add(this.Desc);
            this.Controls.Add(this.ExpectBox);
            this.Controls.Add(this.ClearButton);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.logoLabel);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ReceiveData);
            this.Controls.Add(this.receiveDisplay);
            this.Controls.Add(this.ClientInfoLabel);
            this.Controls.Add(this.ClientInfo);
            this.Controls.Add(this.ServerInfoLabel);
            this.Controls.Add(this.ServerInfo);
            this.Controls.Add(this.TestMenuTree);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ServerForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ServerForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView TestMenuTree;
        private System.Windows.Forms.RichTextBox ServerInfo;
        private System.Windows.Forms.Label ServerInfoLabel;
        private System.Windows.Forms.RichTextBox ClientInfo;
        private System.Windows.Forms.Label ClientInfoLabel;
        private System.Windows.Forms.RichTextBox receiveDisplay;
        private System.Windows.Forms.Label ReceiveData;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label logoLabel;
        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.RichTextBox ExpectBox;
        private System.Windows.Forms.Label Desc;
        private System.Windows.Forms.Label Expect;
        private System.Windows.Forms.RichTextBox DescBox;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel percentageDisplay;
        private System.Windows.Forms.ToolStripStatusLabel totalTest;
        private System.Windows.Forms.ToolStripStatusLabel currentTest;
        private System.Windows.Forms.ToolStripStatusLabel currentModule;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label timerDisplay;
        private System.Windows.Forms.Timer timer2;
    }
}

