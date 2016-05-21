﻿/// ETML - TPI
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
            this.analyseButton = new System.Windows.Forms.Button();
            this.synchroniseButton = new System.Windows.Forms.Button();
            this.targetHostLabel = new System.Windows.Forms.Label();
            this.hostPanelContainer = new System.Windows.Forms.Panel();
            this.roomListBox = new System.Windows.Forms.ListBox();
            this.roomListBoxLabel = new System.Windows.Forms.Label();
            this.manualHostTextBox = new System.Windows.Forms.TextBox();
            this.manualHostTextBoxLabel = new System.Windows.Forms.Label();
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
            this.sourcePathTextBox.Size = new System.Drawing.Size(256, 20);
            this.sourcePathTextBox.TabIndex = 1;
            //For testing puposes only
            this.sourcePathTextBox.Text = @"C:\Users\Win10Rsync\Desktop\";

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
            this.targetPathTextBox.Size = new System.Drawing.Size(256, 20);
            this.targetPathTextBox.TabIndex = 3;
            //For testing purposes only
            this.targetPathTextBox.Text = @"C:\Users\Public\";
            // 
            // analyseButton
            // 
            this.analyseButton.Location = new System.Drawing.Point(84, 423);
            this.analyseButton.Name = "analyseButton";
            this.analyseButton.Size = new System.Drawing.Size(75, 23);
            this.analyseButton.TabIndex = 5;
            this.analyseButton.Text = "Analyze";
            this.analyseButton.UseVisualStyleBackColor = true;
            this.analyseButton.Click += new System.EventHandler(this.analyseButtonClick);
            // 
            // synchroniseButton
            // 
            this.synchroniseButton.Location = new System.Drawing.Point(198, 423);
            this.synchroniseButton.Name = "synchroniseButton";
            this.synchroniseButton.Size = new System.Drawing.Size(75, 23);
            this.synchroniseButton.TabIndex = 6;
            this.synchroniseButton.Text = "Synchronize";
            this.synchroniseButton.UseVisualStyleBackColor = true;
            this.synchroniseButton.Click += new System.EventHandler(this.synchronizeButtonClick);
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
            // roomListBox
            // 
            this.roomListBox.FormattingEnabled = true;
            this.roomListBox.Location = new System.Drawing.Point(5, 103);
            this.roomListBox.Name = "roomListBox";
            this.roomListBox.Size = new System.Drawing.Size(120, 30);
            this.roomListBox.TabIndex = 9;
            // 
            // roomListBoxLabel
            // 
            this.roomListBoxLabel.AutoSize = true;
            this.roomListBoxLabel.Location = new System.Drawing.Point(48, 87);
            this.roomListBoxLabel.Name = "roomListBoxLabel";
            this.roomListBoxLabel.Size = new System.Drawing.Size(35, 13);
            this.roomListBoxLabel.TabIndex = 10;
            this.roomListBoxLabel.Text = "Room";
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
            // gAndDForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(355, 458);
            this.Controls.Add(this.manualHostTextBoxLabel);
            this.Controls.Add(this.manualHostTextBox);
            this.Controls.Add(this.roomListBoxLabel);
            this.Controls.Add(this.roomListBox);
            this.Controls.Add(this.hostPanelContainer);
            this.Controls.Add(this.targetHostLabel);
            this.Controls.Add(this.synchroniseButton);
            this.Controls.Add(this.analyseButton);
            this.Controls.Add(this.targetPathTextBox);
            this.Controls.Add(this.targetPathLabel);
            this.Controls.Add(this.sourcePathLabel);
            this.Controls.Add(this.sourcePathTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "gAndDForm";
            this.ShowIcon = false;
            this.Text = "GatherAndDeployC#";
            //this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.gAndDFormFormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label sourcePathLabel;
        private System.Windows.Forms.TextBox sourcePathTextBox;
        private System.Windows.Forms.Label targetPathLabel;
        private System.Windows.Forms.TextBox targetPathTextBox;
        private System.Windows.Forms.Button analyseButton;
        private System.Windows.Forms.Button synchroniseButton;
        private System.Windows.Forms.Label targetHostLabel;
        private System.Windows.Forms.Panel hostPanelContainer;
        private System.Windows.Forms.ListBox roomListBox;
        private System.Windows.Forms.Label roomListBoxLabel;
        private System.Windows.Forms.TextBox manualHostTextBox;
        private System.Windows.Forms.Label manualHostTextBoxLabel;
    }
}

