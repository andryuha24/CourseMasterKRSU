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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.runButton = new MetroFramework.Controls.MetroButton();
            this.saveButton = new MetroFramework.Controls.MetroButton();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.clearButton = new MetroFramework.Controls.MetroButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pauseButton = new MetroFramework.Controls.MetroButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.Location = new System.Drawing.Point(17, 82);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(431, 300);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // runButton
            // 
            this.runButton.FontSize = MetroFramework.MetroButtonSize.Tall;
            this.runButton.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.runButton.Location = new System.Drawing.Point(17, 391);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(97, 68);
            this.runButton.TabIndex = 7;
            this.runButton.Text = "Отобразить";
            this.runButton.UseSelectable = true;
            this.runButton.Click += new System.EventHandler(this.runButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.FontSize = MetroFramework.MetroButtonSize.Tall;
            this.saveButton.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.saveButton.Location = new System.Drawing.Point(227, 393);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(104, 67);
            this.saveButton.TabIndex = 9;
            this.saveButton.Text = "Сохранить";
            this.saveButton.UseSelectable = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(187, 57);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(108, 19);
            this.metroLabel1.TabIndex = 10;
            this.metroLabel1.Text = "Желтый квадрат";
            // 
            // clearButton
            // 
            this.clearButton.FontSize = MetroFramework.MetroButtonSize.Tall;
            this.clearButton.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.clearButton.Location = new System.Drawing.Point(338, 393);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(110, 67);
            this.clearButton.TabIndex = 13;
            this.clearButton.Text = "Очистить";
            this.clearButton.UseSelectable = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pauseButton
            // 
            this.pauseButton.FontSize = MetroFramework.MetroButtonSize.Tall;
            this.pauseButton.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.pauseButton.Location = new System.Drawing.Point(120, 392);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(101, 68);
            this.pauseButton.TabIndex = 14;
            this.pauseButton.Text = "Пауза";
            this.pauseButton.UseSelectable = true;
            this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 467);
            this.Controls.Add(this.pauseButton);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.runButton);
            this.Controls.Add(this.pictureBox1);
            this.MaximumSize = new System.Drawing.Size(470, 467);
            this.MinimumSize = new System.Drawing.Size(470, 467);
            this.Name = "Form1";
            this.Text = "Клеточные автоматы";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private MetroFramework.Controls.MetroButton runButton;
        private MetroFramework.Controls.MetroButton saveButton;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroButton clearButton;
        private System.Windows.Forms.Timer timer1;
        private MetroFramework.Controls.MetroButton pauseButton;
    }
}

