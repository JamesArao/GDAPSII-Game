namespace RoundEditor
{
    partial class RoundSettings
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
            this.spawningBox = new System.Windows.Forms.GroupBox();
            this.individualButton = new System.Windows.Forms.RadioButton();
            this.enemiesBox = new System.Windows.Forms.GroupBox();
            this.numberBox = new System.Windows.Forms.TextBox();
            this.numberButton = new System.Windows.Forms.RadioButton();
            this.allButton = new System.Windows.Forms.RadioButton();
            this.spawningBox.SuspendLayout();
            this.enemiesBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // spawningBox
            // 
            this.spawningBox.Controls.Add(this.individualButton);
            this.spawningBox.Location = new System.Drawing.Point(13, 13);
            this.spawningBox.Name = "spawningBox";
            this.spawningBox.Size = new System.Drawing.Size(278, 71);
            this.spawningBox.TabIndex = 0;
            this.spawningBox.TabStop = false;
            this.spawningBox.Text = "Spawning Type";
            // 
            // individualButton
            // 
            this.individualButton.AutoSize = true;
            this.individualButton.Checked = true;
            this.individualButton.Location = new System.Drawing.Point(7, 30);
            this.individualButton.Name = "individualButton";
            this.individualButton.Size = new System.Drawing.Size(70, 17);
            this.individualButton.TabIndex = 0;
            this.individualButton.TabStop = true;
            this.individualButton.Text = "Individual";
            this.individualButton.UseVisualStyleBackColor = true;
            // 
            // enemiesBox
            // 
            this.enemiesBox.Controls.Add(this.numberBox);
            this.enemiesBox.Controls.Add(this.numberButton);
            this.enemiesBox.Controls.Add(this.allButton);
            this.enemiesBox.Location = new System.Drawing.Point(13, 90);
            this.enemiesBox.Name = "enemiesBox";
            this.enemiesBox.Size = new System.Drawing.Size(278, 70);
            this.enemiesBox.TabIndex = 1;
            this.enemiesBox.TabStop = false;
            this.enemiesBox.Text = "Max Enemies on Screen (only for Individual spawning)";
            // 
            // numberBox
            // 
            this.numberBox.Enabled = false;
            this.numberBox.Location = new System.Drawing.Point(207, 35);
            this.numberBox.Name = "numberBox";
            this.numberBox.Size = new System.Drawing.Size(55, 20);
            this.numberBox.TabIndex = 2;
            // 
            // numberButton
            // 
            this.numberButton.AutoSize = true;
            this.numberButton.Location = new System.Drawing.Point(117, 35);
            this.numberButton.Name = "numberButton";
            this.numberButton.Size = new System.Drawing.Size(84, 17);
            this.numberButton.TabIndex = 1;
            this.numberButton.Text = "Set Number:";
            this.numberButton.UseVisualStyleBackColor = true;
            this.numberButton.CheckedChanged += new System.EventHandler(this.numberButton_CheckedChanged);
            // 
            // allButton
            // 
            this.allButton.AutoSize = true;
            this.allButton.Checked = true;
            this.allButton.Location = new System.Drawing.Point(7, 35);
            this.allButton.Name = "allButton";
            this.allButton.Size = new System.Drawing.Size(36, 17);
            this.allButton.TabIndex = 0;
            this.allButton.TabStop = true;
            this.allButton.Text = "All";
            this.allButton.UseVisualStyleBackColor = true;
            // 
            // RoundSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 183);
            this.Controls.Add(this.enemiesBox);
            this.Controls.Add(this.spawningBox);
            this.Name = "RoundSettings";
            this.Text = "RoundSettings";
            this.spawningBox.ResumeLayout(false);
            this.spawningBox.PerformLayout();
            this.enemiesBox.ResumeLayout(false);
            this.enemiesBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox spawningBox;
        private System.Windows.Forms.GroupBox enemiesBox;
        private System.Windows.Forms.RadioButton individualButton;
        private System.Windows.Forms.RadioButton numberButton;
        private System.Windows.Forms.RadioButton allButton;
        private System.Windows.Forms.TextBox numberBox;
    }
}