﻿namespace lab1
{
    partial class MainForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea7 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea8 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea9 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series9 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart3 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.metroComboBox1 = new MetroFramework.Controls.MetroComboBox();
            this.runButton = new MetroFramework.Controls.MetroButton();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.numN = new System.Windows.Forms.NumericUpDown();
            this.numDT = new System.Windows.Forms.NumericUpDown();
            this.metroLabel5 = new MetroFramework.Controls.MetroLabel();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDT)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            chartArea7.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea7);
            this.chart1.Location = new System.Drawing.Point(10, 63);
            this.chart1.Name = "chart1";
            series7.ChartArea = "ChartArea1";
            series7.Legend = "Legend1";
            series7.Name = "Series1";
            this.chart1.Series.Add(series7);
            this.chart1.Size = new System.Drawing.Size(326, 300);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // chart2
            // 
            chartArea8.Name = "ChartArea1";
            this.chart2.ChartAreas.Add(chartArea8);
            this.chart2.Location = new System.Drawing.Point(330, 63);
            this.chart2.Name = "chart2";
            series8.ChartArea = "ChartArea1";
            series8.Name = "Series1";
            this.chart2.Series.Add(series8);
            this.chart2.Size = new System.Drawing.Size(326, 300);
            this.chart2.TabIndex = 1;
            this.chart2.Text = "chart2";
            // 
            // chart3
            // 
            chartArea9.Name = "ChartArea1";
            this.chart3.ChartAreas.Add(chartArea9);
            this.chart3.Location = new System.Drawing.Point(663, 63);
            this.chart3.Name = "chart3";
            series9.ChartArea = "ChartArea1";
            series9.Name = "Series1";
            this.chart3.Series.Add(series9);
            this.chart3.Size = new System.Drawing.Size(326, 300);
            this.chart3.TabIndex = 2;
            this.chart3.Text = "chart3";
            // 
            // metroComboBox1
            // 
            this.metroComboBox1.FormattingEnabled = true;
            this.metroComboBox1.ItemHeight = 23;
            this.metroComboBox1.Items.AddRange(new object[] {
            "x = sin(2t)",
            "x = sin(2t)+sin(4t)",
            "x = 1/e^t",
            "x = sin(2t), x<N/2, x = sin(2t)+sin(4t), x>= N/2"});
            this.metroComboBox1.Location = new System.Drawing.Point(238, 392);
            this.metroComboBox1.Name = "metroComboBox1";
            this.metroComboBox1.Size = new System.Drawing.Size(299, 29);
            this.metroComboBox1.TabIndex = 30;
            this.metroComboBox1.UseSelectable = true;
            // 
            // runButton
            // 
            this.runButton.FontSize = MetroFramework.MetroButtonSize.Tall;
            this.runButton.Location = new System.Drawing.Point(53, 375);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(164, 75);
            this.runButton.TabIndex = 29;
            this.runButton.Text = "Run";
            this.runButton.UseSelectable = true;
            this.runButton.Click += new System.EventHandler(this.runButton_Click);
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel1.Location = new System.Drawing.Point(238, 364);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(138, 25);
            this.metroLabel1.TabIndex = 31;
            this.metroLabel1.Text = "Временной ряд:";
            // 
            // metroLabel2
            // 
            this.metroLabel2.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel2.Location = new System.Drawing.Point(569, 364);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(165, 50);
            this.metroLabel2.TabIndex = 33;
            this.metroLabel2.Text = "Длина временного ряда 2^j:";
            this.metroLabel2.WrapToLine = true;
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel3.Location = new System.Drawing.Point(574, 422);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(33, 25);
            this.metroLabel3.TabIndex = 34;
            this.metroLabel3.Text = "j =";
            // 
            // metroLabel4
            // 
            this.metroLabel4.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel4.Location = new System.Drawing.Point(797, 364);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(192, 57);
            this.metroLabel4.TabIndex = 36;
            this.metroLabel4.Text = "deltaT - дискретизация временного отрезка:";
            this.metroLabel4.WrapToLine = true;
            // 
            // numN
            // 
            this.numN.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.numN.Location = new System.Drawing.Point(618, 424);
            this.numN.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.numN.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numN.Name = "numN";
            this.numN.Size = new System.Drawing.Size(72, 26);
            this.numN.TabIndex = 37;
            this.numN.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numDT
            // 
            this.numDT.DecimalPlaces = 2;
            this.numDT.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.numDT.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.numDT.Location = new System.Drawing.Point(875, 424);
            this.numDT.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numDT.Name = "numDT";
            this.numDT.Size = new System.Drawing.Size(72, 26);
            this.numDT.TabIndex = 38;
            // 
            // metroLabel5
            // 
            this.metroLabel5.AutoSize = true;
            this.metroLabel5.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel5.Location = new System.Drawing.Point(822, 422);
            this.metroLabel5.Name = "metroLabel5";
            this.metroLabel5.Size = new System.Drawing.Size(53, 25);
            this.metroLabel5.TabIndex = 39;
            this.metroLabel5.Text = "dT = ";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1034, 473);
            this.Controls.Add(this.metroLabel5);
            this.Controls.Add(this.numDT);
            this.Controls.Add(this.numN);
            this.Controls.Add(this.metroLabel4);
            this.Controls.Add(this.metroLabel3);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.metroComboBox1);
            this.Controls.Add(this.runButton);
            this.Controls.Add(this.chart3);
            this.Controls.Add(this.chart2);
            this.Controls.Add(this.chart1);
            this.MaximumSize = new System.Drawing.Size(1034, 473);
            this.MinimumSize = new System.Drawing.Size(1034, 473);
            this.Name = "MainForm";
            this.Text = "Быстрое преобразование фурье";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDT)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart3;
        private MetroFramework.Controls.MetroComboBox metroComboBox1;
        private MetroFramework.Controls.MetroButton runButton;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private System.Windows.Forms.NumericUpDown numN;
        private System.Windows.Forms.NumericUpDown numDT;
        private MetroFramework.Controls.MetroLabel metroLabel5;
    }
}