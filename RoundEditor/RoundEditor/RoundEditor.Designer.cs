namespace RoundEditor
{
    partial class RoundEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RoundEditor));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.roundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveRoundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadRoundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearRoundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.roundSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enemiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addEnemyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enemy1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enemy2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enemy3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enemy4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enemy5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.group1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.group2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.group3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.roundToolStripMenuItem,
            this.enemiesToolStripMenuItem,
            this.groupsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(984, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // roundToolStripMenuItem
            // 
            this.roundToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveRoundToolStripMenuItem,
            this.loadRoundToolStripMenuItem,
            this.clearRoundToolStripMenuItem,
            this.roundSettingsToolStripMenuItem});
            this.roundToolStripMenuItem.Name = "roundToolStripMenuItem";
            this.roundToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.roundToolStripMenuItem.Text = "Round";
            // 
            // saveRoundToolStripMenuItem
            // 
            this.saveRoundToolStripMenuItem.Name = "saveRoundToolStripMenuItem";
            this.saveRoundToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.saveRoundToolStripMenuItem.Text = "Save Round";
            this.saveRoundToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // loadRoundToolStripMenuItem
            // 
            this.loadRoundToolStripMenuItem.Name = "loadRoundToolStripMenuItem";
            this.loadRoundToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.loadRoundToolStripMenuItem.Text = "Load Round";
            this.loadRoundToolStripMenuItem.Click += new System.EventHandler(this.loadRoundToolStripMenuItem_Click);
            // 
            // clearRoundToolStripMenuItem
            // 
            this.clearRoundToolStripMenuItem.Name = "clearRoundToolStripMenuItem";
            this.clearRoundToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.clearRoundToolStripMenuItem.Text = "Clear Round";
            this.clearRoundToolStripMenuItem.Click += new System.EventHandler(this.clearRoundToolStripMenuItem_Click);
            // 
            // roundSettingsToolStripMenuItem
            // 
            this.roundSettingsToolStripMenuItem.Name = "roundSettingsToolStripMenuItem";
            this.roundSettingsToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.roundSettingsToolStripMenuItem.Text = "Round Settings";
            this.roundSettingsToolStripMenuItem.Click += new System.EventHandler(this.roundSettingsToolStripMenuItem_Click);
            // 
            // enemiesToolStripMenuItem
            // 
            this.enemiesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addEnemyToolStripMenuItem});
            this.enemiesToolStripMenuItem.Name = "enemiesToolStripMenuItem";
            this.enemiesToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.enemiesToolStripMenuItem.Text = "Enemies";
            // 
            // addEnemyToolStripMenuItem
            // 
            this.addEnemyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enemy1ToolStripMenuItem,
            this.enemy2ToolStripMenuItem,
            this.enemy3ToolStripMenuItem,
            this.enemy4ToolStripMenuItem,
            this.enemy5ToolStripMenuItem});
            this.addEnemyToolStripMenuItem.Name = "addEnemyToolStripMenuItem";
            this.addEnemyToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.addEnemyToolStripMenuItem.Text = "Add Enemy";
            // 
            // enemy1ToolStripMenuItem
            // 
            this.enemy1ToolStripMenuItem.Name = "enemy1ToolStripMenuItem";
            this.enemy1ToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.enemy1ToolStripMenuItem.Text = "Enemy 1";
            this.enemy1ToolStripMenuItem.Click += new System.EventHandler(this.enemyToolStripMenuItem_Click);
            // 
            // enemy2ToolStripMenuItem
            // 
            this.enemy2ToolStripMenuItem.Name = "enemy2ToolStripMenuItem";
            this.enemy2ToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.enemy2ToolStripMenuItem.Text = "Enemy 2";
            this.enemy2ToolStripMenuItem.Click += new System.EventHandler(this.enemyToolStripMenuItem_Click);
            // 
            // enemy3ToolStripMenuItem
            // 
            this.enemy3ToolStripMenuItem.Name = "enemy3ToolStripMenuItem";
            this.enemy3ToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.enemy3ToolStripMenuItem.Text = "Enemy 3";
            this.enemy3ToolStripMenuItem.Click += new System.EventHandler(this.enemyToolStripMenuItem_Click);
            // 
            // enemy4ToolStripMenuItem
            // 
            this.enemy4ToolStripMenuItem.Name = "enemy4ToolStripMenuItem";
            this.enemy4ToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.enemy4ToolStripMenuItem.Text = "Enemy 4";
            this.enemy4ToolStripMenuItem.Click += new System.EventHandler(this.enemyToolStripMenuItem_Click);
            // 
            // enemy5ToolStripMenuItem
            // 
            this.enemy5ToolStripMenuItem.Name = "enemy5ToolStripMenuItem";
            this.enemy5ToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.enemy5ToolStripMenuItem.Text = "Enemy 5";
            this.enemy5ToolStripMenuItem.Click += new System.EventHandler(this.enemyToolStripMenuItem_Click);
            // 
            // groupsToolStripMenuItem
            // 
            this.groupsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.group1ToolStripMenuItem,
            this.group2ToolStripMenuItem,
            this.group3ToolStripMenuItem});
            this.groupsToolStripMenuItem.Enabled = false;
            this.groupsToolStripMenuItem.Name = "groupsToolStripMenuItem";
            this.groupsToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.groupsToolStripMenuItem.Text = "Groups";
            // 
            // group1ToolStripMenuItem
            // 
            this.group1ToolStripMenuItem.Name = "group1ToolStripMenuItem";
            this.group1ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.group1ToolStripMenuItem.Text = "Group1";
            this.group1ToolStripMenuItem.Click += new System.EventHandler(this.group1ToolStripMenuItem_Click);
            // 
            // group2ToolStripMenuItem
            // 
            this.group2ToolStripMenuItem.Name = "group2ToolStripMenuItem";
            this.group2ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.group2ToolStripMenuItem.Text = "Group2";
            this.group2ToolStripMenuItem.Click += new System.EventHandler(this.group2ToolStripMenuItem_Click);
            // 
            // group3ToolStripMenuItem
            // 
            this.group3ToolStripMenuItem.Name = "group3ToolStripMenuItem";
            this.group3ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.group3ToolStripMenuItem.Text = "Group3";
            this.group3ToolStripMenuItem.Click += new System.EventHandler(this.group3ToolStripMenuItem_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(475, 275);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(50, 50);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // RoundEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 586);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "RoundEditor";
            this.Text = "Round Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem roundToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveRoundToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadRoundToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enemiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addEnemyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enemy1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enemy2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enemy3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enemy4ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enemy5ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearRoundToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem roundSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem groupsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem group1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem group2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem group3ToolStripMenuItem;
    }
}

