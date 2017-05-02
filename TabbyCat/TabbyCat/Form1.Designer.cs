namespace TabbyCat
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.oxAngleTrackbar = new System.Windows.Forms.TrackBar();
            this.oyAngleTrackbar = new System.Windows.Forms.TrackBar();
            this.ozAngleTrackbar = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.oxAngleTrackbar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.oyAngleTrackbar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ozAngleTrackbar)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(860, 487);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // oxAngleTrackbar
            // 
            this.oxAngleTrackbar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.oxAngleTrackbar.Location = new System.Drawing.Point(0, 442);
            this.oxAngleTrackbar.Maximum = 360;
            this.oxAngleTrackbar.Name = "oxAngleTrackbar";
            this.oxAngleTrackbar.Size = new System.Drawing.Size(860, 45);
            this.oxAngleTrackbar.TabIndex = 2;
            this.oxAngleTrackbar.TickFrequency = 0;
            this.oxAngleTrackbar.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.oxAngleTrackbar.Value = 180;
            this.oxAngleTrackbar.KeyDown += new System.Windows.Forms.KeyEventHandler(this.trackBar1_KeyDown);
            // 
            // oyAngleTrackbar
            // 
            this.oyAngleTrackbar.Dock = System.Windows.Forms.DockStyle.Right;
            this.oyAngleTrackbar.Location = new System.Drawing.Point(815, 0);
            this.oyAngleTrackbar.Maximum = 360;
            this.oyAngleTrackbar.Name = "oyAngleTrackbar";
            this.oyAngleTrackbar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.oyAngleTrackbar.Size = new System.Drawing.Size(45, 442);
            this.oyAngleTrackbar.TabIndex = 2;
            this.oyAngleTrackbar.TickFrequency = 0;
            this.oyAngleTrackbar.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.oyAngleTrackbar.Value = 180;
            // 
            // ozAngleTrackbar
            // 
            this.ozAngleTrackbar.Dock = System.Windows.Forms.DockStyle.Left;
            this.ozAngleTrackbar.Location = new System.Drawing.Point(0, 0);
            this.ozAngleTrackbar.Maximum = 360;
            this.ozAngleTrackbar.Name = "ozAngleTrackbar";
            this.ozAngleTrackbar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.ozAngleTrackbar.Size = new System.Drawing.Size(45, 442);
            this.ozAngleTrackbar.TabIndex = 3;
            this.ozAngleTrackbar.TickFrequency = 0;
            this.ozAngleTrackbar.Value = 180;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(860, 487);
            this.Controls.Add(this.ozAngleTrackbar);
            this.Controls.Add(this.oyAngleTrackbar);
            this.Controls.Add(this.oxAngleTrackbar);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.oxAngleTrackbar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.oyAngleTrackbar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ozAngleTrackbar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TrackBar oxAngleTrackbar;
        private System.Windows.Forms.TrackBar oyAngleTrackbar;
        private System.Windows.Forms.TrackBar ozAngleTrackbar;
    }
}

