namespace ProfileManager
{
    partial class FormProfiles
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbAddProfile = new System.Windows.Forms.ToolStripButton();
            this.tsbRunProfile = new System.Windows.Forms.ToolStripButton();
            this.tsbAbout = new System.Windows.Forms.ToolStripButton();
            this.tsbOpenRimWorld = new System.Windows.Forms.ToolStripButton();
            this.tsbOpenProfile = new System.Windows.Forms.ToolStripButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAddProfile,
            this.tsbRunProfile,
            this.tsbAbout,
            this.tsbOpenRimWorld,
            this.tsbOpenProfile});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(824, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbAddProfile
            // 
            this.tsbAddProfile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAddProfile.Image = global::ProfileManager.Properties.Resources.AddMark_10580;
            this.tsbAddProfile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddProfile.Name = "tsbAddProfile";
            this.tsbAddProfile.Size = new System.Drawing.Size(23, 22);
            this.tsbAddProfile.Text = "Add Profile";
            this.tsbAddProfile.Visible = false;
            // 
            // tsbRunProfile
            // 
            this.tsbRunProfile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRunProfile.Image = global::ProfileManager.Properties.Resources.startwithoutdebugging_6556;
            this.tsbRunProfile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRunProfile.Name = "tsbRunProfile";
            this.tsbRunProfile.Size = new System.Drawing.Size(23, 22);
            this.tsbRunProfile.Text = "Run Profile";
            this.tsbRunProfile.Click += new System.EventHandler(this.tsbRun_Click);
            // 
            // tsbAbout
            // 
            this.tsbAbout.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAbout.Image = global::ProfileManager.Properties.Resources.DynamicHelp_5659;
            this.tsbAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAbout.Name = "tsbAbout";
            this.tsbAbout.Size = new System.Drawing.Size(23, 22);
            this.tsbAbout.Text = "About";
            // 
            // tsbOpenRimWorld
            // 
            this.tsbOpenRimWorld.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbOpenRimWorld.Image = global::ProfileManager.Properties.Resources.Folder_6221;
            this.tsbOpenRimWorld.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOpenRimWorld.Name = "tsbOpenRimWorld";
            this.tsbOpenRimWorld.Size = new System.Drawing.Size(23, 22);
            this.tsbOpenRimWorld.Text = "Open RimWorld Directory";
            this.tsbOpenRimWorld.Click += new System.EventHandler(this.tsbOpenRimWorld_Click);
            // 
            // tsbOpenProfile
            // 
            this.tsbOpenProfile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbOpenProfile.Image = global::ProfileManager.Properties.Resources.Folder_special_open__5844_16x;
            this.tsbOpenProfile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOpenProfile.Name = "tsbOpenProfile";
            this.tsbOpenProfile.Size = new System.Drawing.Size(23, 22);
            this.tsbOpenProfile.Text = "Open Profile Directory";
            this.tsbOpenProfile.Click += new System.EventHandler(this.tsbOpenProfile_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 25);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(824, 355);
            this.dataGridView1.TabIndex = 2;
            // 
            // FormProfiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 380);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FormProfiles";
            this.Text = "RimWorld Profile Manager";
            this.Load += new System.EventHandler(this.FormProfiles_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbAddProfile;
        private System.Windows.Forms.ToolStripButton tsbRunProfile;
        private System.Windows.Forms.ToolStripButton tsbAbout;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStripButton tsbOpenRimWorld;
        private System.Windows.Forms.ToolStripButton tsbOpenProfile;
    }
}

