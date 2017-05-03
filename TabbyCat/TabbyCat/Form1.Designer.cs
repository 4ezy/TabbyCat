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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.scaleGroupBox = new System.Windows.Forms.GroupBox();
            this.scaleControl = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.tanslationGroupBox = new System.Windows.Forms.GroupBox();
            this.zOffsetControlControl = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.yOffsetControl = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.xOffsetControl = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.angleGroupBox = new System.Windows.Forms.GroupBox();
            this.zAngleControl = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.yAngleControl = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.xAngleControl = new System.Windows.Forms.NumericUpDown();
            this.modelGeometricType = new System.Windows.Forms.GroupBox();
            this.surfaceRadioButton = new System.Windows.Forms.RadioButton();
            this.wireFrameRadioButton = new System.Windows.Forms.RadioButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.scaleGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scaleControl)).BeginInit();
            this.tanslationGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zOffsetControlControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yOffsetControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xOffsetControl)).BeginInit();
            this.angleGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zAngleControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yAngleControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xAngleControl)).BeginInit();
            this.modelGeometricType.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(765, 532);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 230F));
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(995, 532);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(768, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(224, 526);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tableLayoutPanel2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(216, 500);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Геометрические преобразования";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.scaleGroupBox, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.tanslationGroupBox, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.angleGroupBox, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.modelGeometricType, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(210, 477);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // scaleGroupBox
            // 
            this.scaleGroupBox.Controls.Add(this.scaleControl);
            this.scaleGroupBox.Controls.Add(this.label7);
            this.scaleGroupBox.Location = new System.Drawing.Point(3, 418);
            this.scaleGroupBox.Name = "scaleGroupBox";
            this.scaleGroupBox.Size = new System.Drawing.Size(204, 56);
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
            this.scaleControl.Size = new System.Drawing.Size(192, 20);
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
            this.tanslationGroupBox.Controls.Add(this.zOffsetControlControl);
            this.tanslationGroupBox.Controls.Add(this.label5);
            this.tanslationGroupBox.Controls.Add(this.yOffsetControl);
            this.tanslationGroupBox.Controls.Add(this.label6);
            this.tanslationGroupBox.Controls.Add(this.xOffsetControl);
            this.tanslationGroupBox.Controls.Add(this.label4);
            this.tanslationGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.tanslationGroupBox.Location = new System.Drawing.Point(3, 248);
            this.tanslationGroupBox.Name = "tanslationGroupBox";
            this.tanslationGroupBox.Size = new System.Drawing.Size(204, 160);
            this.tanslationGroupBox.TabIndex = 4;
            this.tanslationGroupBox.TabStop = false;
            this.tanslationGroupBox.Text = "Перенос";
            // 
            // zOffsetControlControl
            // 
            this.zOffsetControlControl.Location = new System.Drawing.Point(6, 131);
            this.zOffsetControlControl.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.zOffsetControlControl.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.zOffsetControlControl.Name = "zOffsetControlControl";
            this.zOffsetControlControl.Size = new System.Drawing.Size(192, 20);
            this.zOffsetControlControl.TabIndex = 10;
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
            // yOffsetControl
            // 
            this.yOffsetControl.Location = new System.Drawing.Point(6, 80);
            this.yOffsetControl.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.yOffsetControl.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.yOffsetControl.Name = "yOffsetControl";
            this.yOffsetControl.Size = new System.Drawing.Size(192, 20);
            this.yOffsetControl.TabIndex = 8;
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
            // xOffsetControl
            // 
            this.xOffsetControl.Location = new System.Drawing.Point(6, 32);
            this.xOffsetControl.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.xOffsetControl.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.xOffsetControl.Name = "xOffsetControl";
            this.xOffsetControl.Size = new System.Drawing.Size(192, 20);
            this.xOffsetControl.TabIndex = 6;
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
            this.angleGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.angleGroupBox.Location = new System.Drawing.Point(3, 78);
            this.angleGroupBox.Name = "angleGroupBox";
            this.angleGroupBox.Size = new System.Drawing.Size(204, 164);
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
            this.zAngleControl.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.zAngleControl.Name = "zAngleControl";
            this.zAngleControl.Size = new System.Drawing.Size(192, 20);
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
            this.yAngleControl.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.yAngleControl.Name = "yAngleControl";
            this.yAngleControl.Size = new System.Drawing.Size(192, 20);
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
            this.xAngleControl.Location = new System.Drawing.Point(9, 32);
            this.xAngleControl.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.xAngleControl.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.xAngleControl.Name = "xAngleControl";
            this.xAngleControl.Size = new System.Drawing.Size(189, 20);
            this.xAngleControl.TabIndex = 0;
            // 
            // modelGeometricType
            // 
            this.modelGeometricType.Controls.Add(this.surfaceRadioButton);
            this.modelGeometricType.Controls.Add(this.wireFrameRadioButton);
            this.modelGeometricType.Dock = System.Windows.Forms.DockStyle.Top;
            this.modelGeometricType.Location = new System.Drawing.Point(3, 3);
            this.modelGeometricType.Name = "modelGeometricType";
            this.modelGeometricType.Size = new System.Drawing.Size(204, 69);
            this.modelGeometricType.TabIndex = 5;
            this.modelGeometricType.TabStop = false;
            this.modelGeometricType.Text = "Тип геометрической модели";
            // 
            // surfaceRadioButton
            // 
            this.surfaceRadioButton.AutoSize = true;
            this.surfaceRadioButton.Location = new System.Drawing.Point(9, 42);
            this.surfaceRadioButton.Name = "surfaceRadioButton";
            this.surfaceRadioButton.Size = new System.Drawing.Size(103, 17);
            this.surfaceRadioButton.TabIndex = 1;
            this.surfaceRadioButton.Text = "Поверхностная";
            this.surfaceRadioButton.UseVisualStyleBackColor = true;
            // 
            // wireFrameRadioButton
            // 
            this.wireFrameRadioButton.AutoSize = true;
            this.wireFrameRadioButton.Checked = true;
            this.wireFrameRadioButton.Location = new System.Drawing.Point(9, 19);
            this.wireFrameRadioButton.Name = "wireFrameRadioButton";
            this.wireFrameRadioButton.Size = new System.Drawing.Size(80, 17);
            this.wireFrameRadioButton.TabIndex = 0;
            this.wireFrameRadioButton.TabStop = true;
            this.wireFrameRadioButton.Text = "Каркасная";
            this.wireFrameRadioButton.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(216, 500);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(995, 532);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.scaleGroupBox.ResumeLayout(false);
            this.scaleGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scaleControl)).EndInit();
            this.tanslationGroupBox.ResumeLayout(false);
            this.tanslationGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zOffsetControlControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yOffsetControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xOffsetControl)).EndInit();
            this.angleGroupBox.ResumeLayout(false);
            this.angleGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zAngleControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yAngleControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xAngleControl)).EndInit();
            this.modelGeometricType.ResumeLayout(false);
            this.modelGeometricType.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
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
        private System.Windows.Forms.NumericUpDown zOffsetControlControl;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown yOffsetControl;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown xOffsetControl;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox modelGeometricType;
        private System.Windows.Forms.RadioButton surfaceRadioButton;
        private System.Windows.Forms.RadioButton wireFrameRadioButton;
    }
}

