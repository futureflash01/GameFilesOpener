namespace SteamGameOpener
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
            SuspendLayout();
            // 
            // titleLabel
            // 
            titleLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            titleLabel.Location = new Point(12, 21);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(540, 53);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Steam Game Folder Opener";
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // descriptionLabel
            // 
            descriptionLabel.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            descriptionLabel.Location = new Point(12, 74);
            descriptionLabel.Name = "descriptionLabel";
            descriptionLabel.Size = new Size(540, 60);
            descriptionLabel.TabIndex = 1;
            descriptionLabel.Text = "This program simply adds a 'Open Steam Game Files' button when you\r\nright click any Steam game's desktop shortcut";
            descriptionLabel.TextAlign = ContentAlignment.MiddleCenter;
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
            statusLabel.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
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
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(564, 399);
            Controls.Add(madeByLabel);
            Controls.Add(statusLabel);
            Controls.Add(actionButton);
            Controls.Add(descriptionLabel);
            Controls.Add(titleLabel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Steam Game Folder Opener";
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
    }
}
