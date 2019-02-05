namespace soundPlayerWPF
{
    partial class Form1
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
            this.explorerBrowser1 = new Microsoft.WindowsAPICodePack.Controls.WindowsForms.ExplorerBrowser();
            this.SuspendLayout();
            // 
            // explorerBrowser1
            // 
            this.explorerBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.explorerBrowser1.Location = new System.Drawing.Point(1, 1);
            this.explorerBrowser1.Margin = new System.Windows.Forms.Padding(0);
            this.explorerBrowser1.Name = "explorerBrowser1";
            this.explorerBrowser1.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.explorerBrowser1.PropertyBagName = "Microsoft.WindowsAPICodePack.Controls.WindowsForms.ExplorerBrowser";
            this.explorerBrowser1.Size = new System.Drawing.Size(482, 319);
            this.explorerBrowser1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(500, 500);
            this.Controls.Add(this.explorerBrowser1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.WindowsAPICodePack.Controls.WindowsForms.ExplorerBrowser explorerBrowser1;
    }
}