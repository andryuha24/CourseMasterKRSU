namespace lab1
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
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
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.runButton = new MetroFramework.Controls.MetroButton();
            this.openButton = new MetroFramework.Controls.MetroButton();
            this.saveButton = new MetroFramework.Controls.MetroButton();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.clearButton = new MetroFramework.Controls.MetroButton();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.zedChart = new ZedGraph.ZedGraphControl();
            this.metroComboBox1 = new MetroFramework.Controls.MetroComboBox();
            this.widthFilter = new MetroFramework.Controls.MetroTextBox();
            this.labelFilterVal = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Gainsboro;
            this.pictureBox2.Location = new System.Drawing.Point(17, 76);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(240, 190);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            // 
            // runButton
            // 
            this.runButton.FontSize = MetroFramework.MetroButtonSize.Tall;
            this.runButton.Location = new System.Drawing.Point(532, 147);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(159, 65);
            this.runButton.TabIndex = 7;
            this.runButton.Text = "Run";
            this.runButton.UseSelectable = true;
            this.runButton.Click += new System.EventHandler(this.runButton_Click);
            // 
            // openButton
            // 
            this.openButton.FontSize = MetroFramework.MetroButtonSize.Tall;
            this.openButton.Location = new System.Drawing.Point(532, 76);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(159, 65);
            this.openButton.TabIndex = 8;
            this.openButton.Text = "Open";
            this.openButton.UseSelectable = true;
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.FontSize = MetroFramework.MetroButtonSize.Tall;
            this.saveButton.Location = new System.Drawing.Point(532, 218);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(159, 65);
            this.saveButton.TabIndex = 9;
            this.saveButton.Text = "Save";
            this.saveButton.UseSelectable = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(49, 56);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(150, 19);
            this.metroLabel2.TabIndex = 11;
            this.metroLabel2.Text = "Целевое изображение";
            // 
            // clearButton
            // 
            this.clearButton.FontSize = MetroFramework.MetroButtonSize.Tall;
            this.clearButton.Location = new System.Drawing.Point(532, 289);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(159, 65);
            this.clearButton.TabIndex = 13;
            this.clearButton.Text = "Clear";
            this.clearButton.UseSelectable = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.Location = new System.Drawing.Point(367, 56);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(64, 19);
            this.metroLabel3.TabIndex = 12;
            this.metroLabel3.Text = "Результат";
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Gainsboro;
            this.pictureBox3.Location = new System.Drawing.Point(275, 76);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(240, 190);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 4;
            this.pictureBox3.TabStop = false;
            // 
            // zedChart
            // 
            this.zedChart.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.zedChart.IsEnableHPan = false;
            this.zedChart.IsEnableHZoom = false;
            this.zedChart.IsEnableVPan = false;
            this.zedChart.IsEnableVZoom = false;
            this.zedChart.IsEnableWheelZoom = false;
            this.zedChart.IsShowPointValues = true;
            this.zedChart.Location = new System.Drawing.Point(17, 278);
            this.zedChart.Name = "zedChart";
            this.zedChart.ScrollGrace = 0D;
            this.zedChart.ScrollMaxX = 0D;
            this.zedChart.ScrollMaxY = 0D;
            this.zedChart.ScrollMaxY2 = 0D;
            this.zedChart.ScrollMinX = 0D;
            this.zedChart.ScrollMinY = 0D;
            this.zedChart.ScrollMinY2 = 0D;
            this.zedChart.Size = new System.Drawing.Size(498, 289);
            this.zedChart.TabIndex = 27;
            this.zedChart.UseExtendedPrintDialog = true;
            // 
            // metroComboBox1
            // 
            this.metroComboBox1.FormattingEnabled = true;
            this.metroComboBox1.ItemHeight = 23;
            this.metroComboBox1.Items.AddRange(new object[] {
            "Red",
            "Green",
            "Blue",
            "Brightness"});
            this.metroComboBox1.Location = new System.Drawing.Point(532, 404);
            this.metroComboBox1.Name = "metroComboBox1";
            this.metroComboBox1.Size = new System.Drawing.Size(159, 29);
            this.metroComboBox1.TabIndex = 28;
            this.metroComboBox1.UseSelectable = true;
            this.metroComboBox1.SelectedIndexChanged += new System.EventHandler(this.metroComboBox1_SelectedIndexChanged);
            // 
            // widthFilter
            // 
            this.widthFilter.Lines = new string[] {
        "0"};
            this.widthFilter.Location = new System.Drawing.Point(532, 465);
            this.widthFilter.MaxLength = 4;
            this.widthFilter.Name = "widthFilter";
            this.widthFilter.PasswordChar = '\0';
            this.widthFilter.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.widthFilter.SelectedText = "";
            this.widthFilter.Size = new System.Drawing.Size(159, 23);
            this.widthFilter.TabIndex = 29;
            this.widthFilter.Text = "0";
            this.widthFilter.UseSelectable = true;
            this.widthFilter.TextChanged += new System.EventHandler(this.widthFilter_TextChanged);
            this.widthFilter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.widthFilter_KeyPress);
            // 
            // labelFilterVal
            // 
            this.labelFilterVal.AutoSize = true;
            this.labelFilterVal.Location = new System.Drawing.Point(529, 449);
            this.labelFilterVal.Name = "labelFilterVal";
            this.labelFilterVal.Size = new System.Drawing.Size(164, 13);
            this.labelFilterVal.TabIndex = 30;
            this.labelFilterVal.Text = "Ширина фильтра гистограммы";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 578);
            this.Controls.Add(this.labelFilterVal);
            this.Controls.Add(this.widthFilter);
            this.Controls.Add(this.metroComboBox1);
            this.Controls.Add(this.zedChart);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.metroLabel3);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.openButton);
            this.Controls.Add(this.runButton);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Name = "Form1";
            this.Text = "Сегментация изображений с помощью порогов";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox2;
        private MetroFramework.Controls.MetroButton runButton;
        private MetroFramework.Controls.MetroButton openButton;
        private MetroFramework.Controls.MetroButton saveButton;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroButton clearButton;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private System.Windows.Forms.PictureBox pictureBox3;
        private ZedGraph.ZedGraphControl zedChart;
        private MetroFramework.Controls.MetroComboBox metroComboBox1;
        private MetroFramework.Controls.MetroTextBox widthFilter;
        private System.Windows.Forms.Label labelFilterVal;
    }
}

