namespace ReStore_Kitchen_Pricing_Application
{
    partial class initialsForm
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
            this.promptLabel = new System.Windows.Forms.Label();
            this.initialsTextBox = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // promptLabel
            // 
            this.promptLabel.AutoSize = true;
            this.promptLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.promptLabel.Location = new System.Drawing.Point(38, 9);
            this.promptLabel.Name = "promptLabel";
            this.promptLabel.Size = new System.Drawing.Size(228, 25);
            this.promptLabel.TabIndex = 0;
            this.promptLabel.Text = "Please enter your initials:";
            // 
            // initialsTextBox
            // 
            this.initialsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.initialsTextBox.Location = new System.Drawing.Point(104, 58);
            this.initialsTextBox.Name = "initialsTextBox";
            this.initialsTextBox.Size = new System.Drawing.Size(100, 30);
            this.initialsTextBox.TabIndex = 1;
            // 
            // okButton
            // 
            this.okButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.okButton.Location = new System.Drawing.Point(115, 94);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 30);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // initialsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(308, 139);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.initialsTextBox);
            this.Controls.Add(this.promptLabel);
            this.Name = "initialsForm";
            this.Text = "Initial Entry";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label promptLabel;
        private System.Windows.Forms.TextBox initialsTextBox;
        private System.Windows.Forms.Button okButton;
    }
}