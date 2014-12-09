namespace ODPTaxonomyTrainingHelper
{
    partial class Main
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
            this.btnFillRobot = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnFillRobot
            // 
            this.btnFillRobot.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFillRobot.Location = new System.Drawing.Point(71, 80);
            this.btnFillRobot.Name = "btnFillRobot";
            this.btnFillRobot.Size = new System.Drawing.Size(136, 58);
            this.btnFillRobot.TabIndex = 0;
            this.btnFillRobot.Text = "Prepare for Consensus";
            this.btnFillRobot.UseVisualStyleBackColor = true;
            this.btnFillRobot.Click += new System.EventHandler(this.btnFillRobot_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.btnFillRobot);
            this.Name = "Main";
            this.Text = "ODP Taxonomy Helper";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnFillRobot;
    }
}

