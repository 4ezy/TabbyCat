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
            this.panel1 = new System.Windows.Forms.Panel();
            this.geometricTranslationsGroupBox = new System.Windows.Forms.GroupBox();
            this.scaleGroupBox = new System.Windows.Forms.GroupBox();
            this.scaleControl = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.tanslationGroupBox = new System.Windows.Forms.GroupBox();
            this.zCoordinateLocationControl = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.yCoordinateLocationControl = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.xCoordinateLocationControl = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.angleGroupBox = new System.Windows.Forms.GroupBox();
            this.zAngleControl = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.yAngleControl = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.xAngleControl = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.geometricTranslationsGroupBox.SuspendLayout();
            this.scaleGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scaleControl)).BeginInit();
            this.tanslationGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zCoordinateLocationControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yCoordinateLocationControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xCoordinateLocationControl)).BeginInit();
            this.angleGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zAngleControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yAngleControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xAngleControl)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(676, 487);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.geometricTranslationsGroupBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(669, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(191, 487);
            this.panel1.TabIndex = 1;
            // 
            // geometricTranslationsGroupBox
            // 
            this.geometricTranslationsGroupBox.Controls.Add(this.scaleGroupBox);
            this.geometricTranslationsGroupBox.Controls.Add(this.angleGroupBox);
            this.geometricTranslationsGroupBox.Controls.Add(this.tanslationGroupBox);
            this.geometricTranslationsGroupBox.Location = new System.Drawing.Point(13, 12);
            this.geometricTranslationsGroupBox.Name = "geometricTranslationsGroupBox";
            this.geometricTranslationsGroupBox.Size = new System.Drawing.Size(165, 463);
            this.geometricTranslationsGroupBox.TabIndex = 0;
            this.geometricTranslationsGroupBox.TabStop = false;
            this.geometricTranslationsGroupBox.Text = "Геометрические преобразования";
            // 
            // scaleGroupBox
            // 
            this.scaleGroupBox.Controls.Add(this.scaleControl);
            this.scaleGroupBox.Controls.Add(this.label7);
            this.scaleGroupBox.Location = new System.Drawing.Point(6, 384);
            this.scaleGroupBox.Name = "scaleGroupBox";
            this.scaleGroupBox.Size = new System.Drawing.Size(150, 73);
            this.scaleGroupBox.TabIndex = 5;
            this.scaleGroupBox.TabStop = false;
            this.scaleGroupBox.Text = "Масштаб";
            // 
            // scaleControl
            // 
            this.scaleControl.DecimalPlaces = 2;
            this.scaleControl.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.scaleControl.Location = new System.Drawing.Point(6, 32);
            this.scaleControl.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.scaleControl.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.scaleControl.Name = "scaleControl";
            this.scaleControl.Size = new System.Drawing.Size(138, 20);
            this.scaleControl.TabIndex = 1;
            this.scaleControl.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Кратность";
            // 
            // tanslationGroupBox
            // 
            this.tanslationGroupBox.Controls.Add(this.zCoordinateLocationControl);
            this.tanslationGroupBox.Controls.Add(this.label5);
            this.tanslationGroupBox.Controls.Add(this.yCoordinateLocationControl);
            this.tanslationGroupBox.Controls.Add(this.label6);
            this.tanslationGroupBox.Controls.Add(this.xCoordinateLocationControl);
            this.tanslationGroupBox.Controls.Add(this.label4);
            this.tanslationGroupBox.Location = new System.Drawing.Point(6, 210);
            this.tanslationGroupBox.Name = "tanslationGroupBox";
            this.tanslationGroupBox.Size = new System.Drawing.Size(150, 168);
            this.tanslationGroupBox.TabIndex = 4;
            this.tanslationGroupBox.TabStop = false;
            this.tanslationGroupBox.Text = "Перенос";
            // 
            // zCoordinateLocationControl
            // 
            this.zCoordinateLocationControl.Location = new System.Drawing.Point(6, 131);
            this.zCoordinateLocationControl.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.zCoordinateLocationControl.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.zCoordinateLocationControl.Name = "zCoordinateLocationControl";
            this.zCoordinateLocationControl.Size = new System.Drawing.Size(138, 20);
            this.zCoordinateLocationControl.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 115);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Ось Z";
            // 
            // yCoordinateLocationControl
            // 
            this.yCoordinateLocationControl.Location = new System.Drawing.Point(6, 80);
            this.yCoordinateLocationControl.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.yCoordinateLocationControl.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.yCoordinateLocationControl.Name = "yCoordinateLocationControl";
            this.yCoordinateLocationControl.Size = new System.Drawing.Size(138, 20);
            this.yCoordinateLocationControl.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Ось Y";
            // 
            // xCoordinateLocationControl
            // 
            this.xCoordinateLocationControl.Location = new System.Drawing.Point(6, 32);
            this.xCoordinateLocationControl.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.xCoordinateLocationControl.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.xCoordinateLocationControl.Name = "xCoordinateLocationControl";
            this.xCoordinateLocationControl.Size = new System.Drawing.Size(138, 20);
            this.xCoordinateLocationControl.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Ось X";
            // 
            // angleGroupBox
            // 
            this.angleGroupBox.Controls.Add(this.zAngleControl);
            this.angleGroupBox.Controls.Add(this.label3);
            this.angleGroupBox.Controls.Add(this.yAngleControl);
            this.angleGroupBox.Controls.Add(this.label2);
            this.angleGroupBox.Controls.Add(this.label1);
            this.angleGroupBox.Controls.Add(this.xAngleControl);
            this.angleGroupBox.Location = new System.Drawing.Point(6, 33);
            this.angleGroupBox.Name = "angleGroupBox";
            this.angleGroupBox.Size = new System.Drawing.Size(150, 171);
            this.angleGroupBox.TabIndex = 3;
            this.angleGroupBox.TabStop = false;
            this.angleGroupBox.Text = "Поворот";
            // 
            // zAngleControl
            // 
            this.zAngleControl.Location = new System.Drawing.Point(6, 131);
            this.zAngleControl.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.zAngleControl.Name = "zAngleControl";
            this.zAngleControl.Size = new System.Drawing.Size(138, 20);
            this.zAngleControl.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Ось Z";
            // 
            // yAngleControl
            // 
            this.yAngleControl.Location = new System.Drawing.Point(6, 80);
            this.yAngleControl.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.yAngleControl.Name = "yAngleControl";
            this.yAngleControl.Size = new System.Drawing.Size(138, 20);
            this.yAngleControl.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Ось Y";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Ось X";
            // 
            // xAngleControl
            // 
            this.xAngleControl.Location = new System.Drawing.Point(6, 32);
            this.xAngleControl.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.xAngleControl.Name = "xAngleControl";
            this.xAngleControl.Size = new System.Drawing.Size(138, 20);
            this.xAngleControl.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(860, 487);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.geometricTranslationsGroupBox.ResumeLayout(false);
            this.scaleGroupBox.ResumeLayout(false);
            this.scaleGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scaleControl)).EndInit();
            this.tanslationGroupBox.ResumeLayout(false);
            this.tanslationGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zCoordinateLocationControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yCoordinateLocationControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xCoordinateLocationControl)).EndInit();
            this.angleGroupBox.ResumeLayout(false);
            this.angleGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zAngleControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yAngleControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xAngleControl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox geometricTranslationsGroupBox;
        private System.Windows.Forms.GroupBox scaleGroupBox;
        private System.Windows.Forms.NumericUpDown scaleControl;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox angleGroupBox;
        private System.Windows.Forms.NumericUpDown zAngleControl;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown yAngleControl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown xAngleControl;
        private System.Windows.Forms.GroupBox tanslationGroupBox;
        private System.Windows.Forms.NumericUpDown zCoordinateLocationControl;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown yCoordinateLocationControl;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown xCoordinateLocationControl;
        private System.Windows.Forms.Label label4;
    }
}

