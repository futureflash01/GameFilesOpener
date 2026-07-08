namespace GameFilesOpener
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            titleLabel = new Label();
            descriptionLabel = new Label();
            actionButton = new Button();
            statusLabel = new Label();
            madeByLabel = new Label();
            versionLabel = new Label();
            currentSupportLabel = new Label();
            SuspendLayout();
            // 
            // titleLabel
            // 
            titleLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            titleLabel.Location = new Point(12, 16);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(540, 53);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Game Files Opener";
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // descriptionLabel
            // 
            descriptionLabel.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            descriptionLabel.Location = new Point(12, 69);
            descriptionLabel.Name = "descriptionLabel";
            descriptionLabel.Size = new Size(540, 66);
            descriptionLabel.TabIndex = 1;
            descriptionLabel.Text = "This program simply adds a 'Open Game Files' button when you\r\nright click any game's desktop shortcut. This works for many\r\ngames across different launchers installed any where on your computer.";
            descriptionLabel.TextAlign = ContentAlignment.TopCenter;
            // 
            // actionButton
            // 
            actionButton.FlatStyle = FlatStyle.Flat;
            actionButton.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            actionButton.Location = new Point(40, 170);
            actionButton.Name = "actionButton";
            actionButton.Size = new Size(480, 100);
            actionButton.TabIndex = 2;
            actionButton.Text = "APP_INSTALLATION_STATUS";
            actionButton.UseVisualStyleBackColor = true;
            actionButton.Click += actionButton_Click;
            // 
            // statusLabel
            // 
            statusLabel.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            statusLabel.Location = new Point(40, 273);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(480, 102);
            statusLabel.TabIndex = 3;
            statusLabel.Text = "STATUS_LABEL";
            statusLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // madeByLabel
            // 
            madeByLabel.AutoSize = true;
            madeByLabel.Cursor = Cursors.Hand;
            madeByLabel.Font = new Font("Segoe UI", 7.25F, FontStyle.Bold | FontStyle.Underline);
            madeByLabel.ForeColor = Color.Firebrick;
            madeByLabel.Location = new Point(456, 379);
            madeByLabel.Name = "madeByLabel";
            madeByLabel.Size = new Size(101, 12);
            madeByLabel.TabIndex = 4;
            madeByLabel.Text = "Made by FutureFlash";
            madeByLabel.Click += madeByLabel_Click;
            // 
            // versionLabel
            // 
            versionLabel.AutoSize = true;
            versionLabel.ForeColor = SystemColors.ControlDark;
            versionLabel.Location = new Point(12, 375);
            versionLabel.Name = "versionLabel";
            versionLabel.Size = new Size(28, 15);
            versionLabel.TabIndex = 5;
            versionLabel.Text = "v1.1";
            // 
            // currentSupportLabel
            // 
            currentSupportLabel.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            currentSupportLabel.ForeColor = Color.CadetBlue;
            currentSupportLabel.Location = new Point(12, 136);
            currentSupportLabel.Name = "currentSupportLabel";
            currentSupportLabel.Size = new Size(540, 27);
            currentSupportLabel.TabIndex = 1;
            currentSupportLabel.Text = "Currently Supported: Steam, Epic Games, Amazon Games, Xbox, and GOG";
            currentSupportLabel.TextAlign = ContentAlignment.TopCenter;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(564, 399);
            Controls.Add(versionLabel);
            Controls.Add(madeByLabel);
            Controls.Add(statusLabel);
            Controls.Add(actionButton);
            Controls.Add(currentSupportLabel);
            Controls.Add(descriptionLabel);
            Controls.Add(titleLabel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Game Files Opener";
            Load += MainForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label titleLabel;
        private Label descriptionLabel;
        private Button actionButton;
        private Label statusLabel;
        private Label madeByLabel;
        private Label versionLabel;
        private Label currentSupportLabel;
    }
}
