﻿/// ETML - TPI
/// Author: Xavier Dougoud
/// Date:   11.05.2016
/// 
/// Modification: 
/// 
/// Summary:    

namespace WFA_TPI_dougoudxa_GatherAndDeployC_v1
{
    partial class appForm
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
            this.destinationPathLabel = new System.Windows.Forms.Label();
            this.destinationPathTextBox = new System.Windows.Forms.TextBox();
            this.analyseButton = new System.Windows.Forms.Button();
            this.synchroniseButton = new System.Windows.Forms.Button();
            this.targetHostLabel = new System.Windows.Forms.Label();
            this.hostPanelContainer = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // sourcePathLabel
            // 
            this.sourcePathLabel.AutoSize = true;
            this.sourcePathLabel.Location = new System.Drawing.Point(38, 16);
            this.sourcePathLabel.Name = "sourcePathLabel";
            this.sourcePathLabel.Size = new System.Drawing.Size(71, 13);
            this.sourcePathLabel.TabIndex = 0;
            this.sourcePathLabel.Text = "Source path: ";
            // 
            // sourcePathTextBox
            // 
            this.sourcePathTextBox.Location = new System.Drawing.Point(115, 13);
            this.sourcePathTextBox.MaxLength = 256;
            this.sourcePathTextBox.Name = "sourcePathTextBox";
            this.sourcePathTextBox.Size = new System.Drawing.Size(217, 20);
            this.sourcePathTextBox.TabIndex = 1;
            this.sourcePathTextBox.Text = "C:\\";
            // 
            // destinationPathLabel
            // 
            this.destinationPathLabel.AutoSize = true;
            this.destinationPathLabel.Location = new System.Drawing.Point(19, 51);
            this.destinationPathLabel.Name = "destinationPathLabel";
            this.destinationPathLabel.Size = new System.Drawing.Size(90, 13);
            this.destinationPathLabel.TabIndex = 2;
            this.destinationPathLabel.Text = "Destination path: ";
            // 
            // destinationPathTextBox
            // 
            this.destinationPathTextBox.Location = new System.Drawing.Point(115, 48);
            this.destinationPathTextBox.MaxLength = 256;
            this.destinationPathTextBox.Name = "destinationPathTextBox";
            this.destinationPathTextBox.Size = new System.Drawing.Size(217, 20);
            this.destinationPathTextBox.TabIndex = 3;
            this.destinationPathTextBox.Text = "C:\\";
            // 
            // analyseButton
            // 
            this.analyseButton.Location = new System.Drawing.Point(87, 414);
            this.analyseButton.Name = "analyseButton";
            this.analyseButton.Size = new System.Drawing.Size(75, 23);
            this.analyseButton.TabIndex = 5;
            this.analyseButton.Text = "Analyse";
            this.analyseButton.UseVisualStyleBackColor = true;
            // 
            // synchroniseButton
            // 
            this.synchroniseButton.Location = new System.Drawing.Point(201, 414);
            this.synchroniseButton.Name = "synchroniseButton";
            this.synchroniseButton.Size = new System.Drawing.Size(75, 23);
            this.synchroniseButton.TabIndex = 6;
            this.synchroniseButton.Text = "Synchronise";
            this.synchroniseButton.UseVisualStyleBackColor = true;
            // 
            // targetHostLabel
            // 
            this.targetHostLabel.AutoSize = true;
            this.targetHostLabel.Location = new System.Drawing.Point(42, 89);
            this.targetHostLabel.Name = "targetHostLabel";
            this.targetHostLabel.Size = new System.Drawing.Size(67, 13);
            this.targetHostLabel.TabIndex = 7;
            this.targetHostLabel.Text = "Target host: ";
            // 
            // hostPanelContainer
            // 
            this.hostPanelContainer.BackColor = System.Drawing.SystemColors.Window;
            this.hostPanelContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hostPanelContainer.Location = new System.Drawing.Point(115, 89);
            this.hostPanelContainer.Name = "hostPanelContainer";
            this.hostPanelContainer.Size = new System.Drawing.Size(217, 302);
            this.hostPanelContainer.TabIndex = 8;
            this.hostPanelContainer.AutoScroll = true;  //Needed to add scrollbar
            // 
            // appForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 458);
            this.Controls.Add(this.hostPanelContainer);
            this.Controls.Add(this.targetHostLabel);
            this.Controls.Add(this.synchroniseButton);
            this.Controls.Add(this.analyseButton);
            this.Controls.Add(this.destinationPathTextBox);
            this.Controls.Add(this.destinationPathLabel);
            this.Controls.Add(this.sourcePathTextBox);
            this.Controls.Add(this.sourcePathLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "appForm";
            this.ShowIcon = false;
            this.Text = "GatherAndDeployC#";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label sourcePathLabel;
        private System.Windows.Forms.TextBox sourcePathTextBox;
        private System.Windows.Forms.Label destinationPathLabel;
        private System.Windows.Forms.TextBox destinationPathTextBox;
        private System.Windows.Forms.Button analyseButton;
        private System.Windows.Forms.Button synchroniseButton;
        private System.Windows.Forms.Label targetHostLabel;
        private System.Windows.Forms.Panel hostPanelContainer;
    }
}
