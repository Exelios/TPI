/// ETML - TPI
/// Author: Xavier Dougoud
/// Date:   11.05.2016
/// 
/// Modification: 
/// 
/// Summary:    

namespace WFA_TPI_dougoudxa_GatherAndDeployC_v1
{
    partial class gAndDForm
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
            this.sourcePathLabel = new System.Windows.Forms.Label();
            this.sourcePathTextBox = new System.Windows.Forms.TextBox();
            this.targetPathLabel = new System.Windows.Forms.Label();
            this.targetPathTextBox = new System.Windows.Forms.TextBox();
            this.analyzeButton = new System.Windows.Forms.Button();
            this.synchronizeButton = new System.Windows.Forms.Button();
            this.targetHostLabel = new System.Windows.Forms.Label();
            this.hostPanelContainer = new System.Windows.Forms.Panel();
            this.roomComboBox = new System.Windows.Forms.ComboBox();
            this.roomComboBoxLabel = new System.Windows.Forms.Label();
            this.manualHostTextBox = new System.Windows.Forms.TextBox();
            this.manualHostTextBoxLabel = new System.Windows.Forms.Label();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.logLabel = new System.Windows.Forms.Label();
            this.logSaveButton = new System.Windows.Forms.Button();
            this.sourceInfoLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // sourcePathLabel
            // 
            this.sourcePathLabel.AutoSize = true;
            this.sourcePathLabel.Location = new System.Drawing.Point(21, 21);
            this.sourcePathLabel.Name = "sourcePathLabel";
            this.sourcePathLabel.Size = new System.Drawing.Size(71, 13);
            this.sourcePathLabel.TabIndex = 0;
            this.sourcePathLabel.Text = "Source path: ";
            // 
            // sourcePathTextBox
            // 
            this.sourcePathTextBox.Location = new System.Drawing.Point(93, 17);
            this.sourcePathTextBox.MaxLength = 256;
            this.sourcePathTextBox.Name = "sourcePathTextBox";
            this.sourcePathTextBox.Size = new System.Drawing.Size(505, 20);
            this.sourcePathTextBox.TabIndex = 1;
            this.sourcePathTextBox.Text = "C:\\Users\\Win10Rsync\\Desktop\\PBS-SpaceTime";
            // 
            // targetPathLabel
            // 
            this.targetPathLabel.AutoSize = true;
            this.targetPathLabel.Location = new System.Drawing.Point(2, 56);
            this.targetPathLabel.Name = "targetPathLabel";
            this.targetPathLabel.Size = new System.Drawing.Size(90, 13);
            this.targetPathLabel.TabIndex = 2;
            this.targetPathLabel.Text = "Destination path: ";
            // 
            // targetPathTextBox
            // 
            this.targetPathTextBox.Location = new System.Drawing.Point(93, 52);
            this.targetPathTextBox.MaxLength = 256;
            this.targetPathTextBox.Name = "targetPathTextBox";
            this.targetPathTextBox.Size = new System.Drawing.Size(505, 20);
            this.targetPathTextBox.TabIndex = 3;
            this.targetPathTextBox.Text = "C:\\Users\\Public\\PBS-SpaceTime";
            // 
            // analyzeButton
            // 
            this.analyzeButton.Location = new System.Drawing.Point(83, 463);
            this.analyzeButton.Name = "analyzeButton";
            this.analyzeButton.Size = new System.Drawing.Size(75, 23);
            this.analyzeButton.TabIndex = 5;
            this.analyzeButton.Text = "Analyze";
            this.analyzeButton.UseVisualStyleBackColor = true;
            this.analyzeButton.Click += new System.EventHandler(this.analyzeButtonClick);
            // 
            // synchronizeButton
            // 
            this.synchronizeButton.Location = new System.Drawing.Point(197, 463);
            this.synchronizeButton.Name = "synchronizeButton";
            this.synchronizeButton.Size = new System.Drawing.Size(75, 23);
            this.synchronizeButton.TabIndex = 6;
            this.synchronizeButton.Text = "Synchronize";
            this.synchronizeButton.UseVisualStyleBackColor = true;
            this.synchronizeButton.Click += new System.EventHandler(this.synchronizeButtonClick);
            // 
            // targetHostLabel
            // 
            this.targetHostLabel.AutoSize = true;
            this.targetHostLabel.Location = new System.Drawing.Point(207, 87);
            this.targetHostLabel.Name = "targetHostLabel";
            this.targetHostLabel.Size = new System.Drawing.Size(66, 13);
            this.targetHostLabel.TabIndex = 7;
            this.targetHostLabel.Text = "Target hosts";
            // 
            // hostPanelContainer
            // 
            this.hostPanelContainer.AutoScroll = true;
            this.hostPanelContainer.BackColor = System.Drawing.SystemColors.Window;
            this.hostPanelContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hostPanelContainer.Location = new System.Drawing.Point(132, 103);
            this.hostPanelContainer.Name = "hostPanelContainer";
            this.hostPanelContainer.Size = new System.Drawing.Size(217, 314);
            this.hostPanelContainer.TabIndex = 8;
            // 
            // roomComboBox
            // 
            this.roomComboBox.FormattingEnabled = true;
            this.roomComboBox.Location = new System.Drawing.Point(5, 103);
            this.roomComboBox.Name = "roomComboBox";
            this.roomComboBox.Size = new System.Drawing.Size(120, 21);
            this.roomComboBox.TabIndex = 9;
            // 
            // roomComboBoxLabel
            // 
            this.roomComboBoxLabel.AutoSize = true;
            this.roomComboBoxLabel.Location = new System.Drawing.Point(48, 87);
            this.roomComboBoxLabel.Name = "roomComboBoxLabel";
            this.roomComboBoxLabel.Size = new System.Drawing.Size(35, 13);
            this.roomComboBoxLabel.TabIndex = 10;
            this.roomComboBoxLabel.Text = "Room";
            // 
            // manualHostTextBox
            // 
            this.manualHostTextBox.Location = new System.Drawing.Point(5, 164);
            this.manualHostTextBox.Multiline = true;
            this.manualHostTextBox.Name = "manualHostTextBox";
            this.manualHostTextBox.Size = new System.Drawing.Size(120, 253);
            this.manualHostTextBox.TabIndex = 11;
            // 
            // manualHostTextBoxLabel
            // 
            this.manualHostTextBoxLabel.AutoSize = true;
            this.manualHostTextBoxLabel.Location = new System.Drawing.Point(30, 148);
            this.manualHostTextBoxLabel.Name = "manualHostTextBoxLabel";
            this.manualHostTextBoxLabel.Size = new System.Drawing.Size(70, 13);
            this.manualHostTextBoxLabel.TabIndex = 12;
            this.manualHostTextBoxLabel.Text = "Manual hosts";
            // 
            // logTextBox
            // 
            this.logTextBox.Location = new System.Drawing.Point(356, 103);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.Size = new System.Drawing.Size(242, 314);
            this.logTextBox.TabIndex = 13;
            // 
            // logLabel
            // 
            this.logLabel.AutoSize = true;
            this.logLabel.Location = new System.Drawing.Point(457, 87);
            this.logLabel.Name = "logLabel";
            this.logLabel.Size = new System.Drawing.Size(29, 13);
            this.logLabel.TabIndex = 14;
            this.logLabel.Text = "LOG";
            // 
            // logSaveButton
            // 
            this.logSaveButton.Location = new System.Drawing.Point(440, 463);
            this.logSaveButton.Name = "logSaveButton";
            this.logSaveButton.Size = new System.Drawing.Size(75, 23);
            this.logSaveButton.TabIndex = 15;
            this.logSaveButton.Text = "Save";
            this.logSaveButton.UseVisualStyleBackColor = true;
            this.logSaveButton.Click += new System.EventHandler(this.logSaveButtonClick);
            // 
            // sourceInfoLabel
            // 
            this.sourceInfoLabel.AutoSize = true;
            this.sourceInfoLabel.Location = new System.Drawing.Point(13, 424);
            this.sourceInfoLabel.Name = "sourceInfoLabel";
            this.sourceInfoLabel.Size = new System.Drawing.Size(101, 13);
            this.sourceInfoLabel.TabIndex = 16;
            this.sourceInfoLabel.Text = "Source information: ";
            // 
            // gAndDForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(603, 498);
            this.Controls.Add(this.sourceInfoLabel);
            this.Controls.Add(this.logSaveButton);
            this.Controls.Add(this.logLabel);
            this.Controls.Add(this.logTextBox);
            this.Controls.Add(this.manualHostTextBoxLabel);
            this.Controls.Add(this.manualHostTextBox);
            this.Controls.Add(this.roomComboBoxLabel);
            this.Controls.Add(this.roomComboBox);
            this.Controls.Add(this.hostPanelContainer);
            this.Controls.Add(this.targetHostLabel);
            this.Controls.Add(this.synchronizeButton);
            this.Controls.Add(this.analyzeButton);
            this.Controls.Add(this.targetPathTextBox);
            this.Controls.Add(this.targetPathLabel);
            this.Controls.Add(this.sourcePathLabel);
            this.Controls.Add(this.sourcePathTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "gAndDForm";
            this.ShowIcon = false;
            this.Text = "GatherAndDeployC#";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label sourcePathLabel;
        private System.Windows.Forms.TextBox sourcePathTextBox;
        private System.Windows.Forms.Label targetPathLabel;
        private System.Windows.Forms.TextBox targetPathTextBox;
        private System.Windows.Forms.Button analyzeButton;
        private System.Windows.Forms.Button synchronizeButton;
        private System.Windows.Forms.Label targetHostLabel;
        private System.Windows.Forms.Panel hostPanelContainer;
        private System.Windows.Forms.ComboBox roomComboBox;
        private System.Windows.Forms.Label roomComboBoxLabel;
        private System.Windows.Forms.TextBox manualHostTextBox;
        private System.Windows.Forms.Label manualHostTextBoxLabel;
        private System.Windows.Forms.TextBox logTextBox;
        private System.Windows.Forms.Label logLabel;
        private System.Windows.Forms.Button logSaveButton;
        private System.Windows.Forms.Label sourceInfoLabel;
    }
}

