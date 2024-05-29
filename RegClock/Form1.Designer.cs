namespace RegClock
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            groupBox1 = new GroupBox();
            label1 = new Label();
            groupBox2 = new GroupBox();
            label2 = new Label();
            groupBox3 = new GroupBox();
            label7 = new Label();
            label3 = new Label();
            groupBox4 = new GroupBox();
            label4 = new Label();
            groupBox5 = new GroupBox();
            button4 = new Button();
            button3 = new Button();
            button2 = new Button();
            button1 = new Button();
            label5 = new Label();
            timer1 = new System.Windows.Forms.Timer(components);
            timer2 = new System.Windows.Forms.Timer(components);
            timer3 = new System.Windows.Forms.Timer(components);
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox5.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.FromArgb(192, 255, 192);
            groupBox1.Controls.Add(label1);
            groupBox1.Font = new Font("01フロップデザイン", 11.9999981F, FontStyle.Regular, GraphicsUnit.Point, 128);
            groupBox1.Location = new Point(13, 10);
            groupBox1.Margin = new Padding(3, 2, 3, 2);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(3, 2, 3, 2);
            groupBox1.Size = new Size(1112, 160);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "現実時間";
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("01フロップデザイン", 23.9999962F, FontStyle.Regular, GraphicsUnit.Point, 128);
            label1.Location = new Point(54, 55);
            label1.Name = "label1";
            label1.Size = new Size(984, 71);
            label1.TabIndex = 1;
            label1.Text = "2024/12/31 (日曜日) 23:59:59";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // groupBox2
            // 
            groupBox2.BackColor = Color.Moccasin;
            groupBox2.Controls.Add(label2);
            groupBox2.Font = new Font("01フロップデザイン", 11.9999981F, FontStyle.Regular, GraphicsUnit.Point, 128);
            groupBox2.Location = new Point(13, 174);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(354, 160);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "レグナス時間";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Font = new Font("01フロップデザイン", 23.9999962F, FontStyle.Regular, GraphicsUnit.Point, 128);
            label2.Location = new Point(34, 50);
            label2.Name = "label2";
            label2.Size = new Size(311, 71);
            label2.TabIndex = 2;
            label2.Text = "夜 23:59";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // groupBox3
            // 
            groupBox3.BackColor = Color.FromArgb(192, 255, 192);
            groupBox3.Controls.Add(label7);
            groupBox3.Controls.Add(label3);
            groupBox3.Font = new Font("01フロップデザイン", 11.9999981F, FontStyle.Regular, GraphicsUnit.Point, 128);
            groupBox3.Location = new Point(373, 176);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(752, 159);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "ラウェハラ岬 潮時刻 : 現在 満潮";
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label7.AutoSize = true;
            label7.Font = new Font("01フロップデザイン", 13.8749981F, FontStyle.Regular, GraphicsUnit.Point, 128);
            label7.Location = new Point(50, 72);
            label7.Name = "label7";
            label7.Size = new Size(370, 41);
            label7.TabIndex = 6;
            label7.Text = "次の潮位変動まで 約";
            label7.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Font = new Font("01フロップデザイン", 23.9999962F, FontStyle.Regular, GraphicsUnit.Point, 128);
            label3.Location = new Point(436, 48);
            label3.Name = "label3";
            label3.Size = new Size(206, 71);
            label3.TabIndex = 3;
            label3.Text = "12:30";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // groupBox4
            // 
            groupBox4.BackColor = Color.FromArgb(192, 255, 192);
            groupBox4.Controls.Add(label4);
            groupBox4.Font = new Font("01フロップデザイン", 11.9999981F, FontStyle.Regular, GraphicsUnit.Point, 128);
            groupBox4.Location = new Point(13, 340);
            groupBox4.Margin = new Padding(3, 2, 3, 2);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new Padding(3, 2, 3, 2);
            groupBox4.Size = new Size(1112, 160);
            groupBox4.TabIndex = 3;
            groupBox4.TabStop = false;
            groupBox4.Text = "レイド・防衛戦 次回 開始時刻";
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Font = new Font("01フロップデザイン", 23.9999962F, FontStyle.Regular, GraphicsUnit.Point, 128);
            label4.Location = new Point(54, 54);
            label4.Name = "label4";
            label4.Size = new Size(984, 71);
            label4.TabIndex = 1;
            label4.Text = "2024/12/31 (日曜日) 23:59:59";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // groupBox5
            // 
            groupBox5.BackColor = Color.FromArgb(192, 255, 192);
            groupBox5.Controls.Add(button4);
            groupBox5.Controls.Add(button3);
            groupBox5.Controls.Add(button2);
            groupBox5.Controls.Add(button1);
            groupBox5.Controls.Add(label5);
            groupBox5.Font = new Font("01フロップデザイン", 11.9999981F, FontStyle.Regular, GraphicsUnit.Point, 128);
            groupBox5.Location = new Point(12, 504);
            groupBox5.Margin = new Padding(3, 2, 3, 2);
            groupBox5.Name = "groupBox5";
            groupBox5.Padding = new Padding(3, 2, 3, 2);
            groupBox5.Size = new Size(1113, 160);
            groupBox5.TabIndex = 4;
            groupBox5.TabStop = false;
            groupBox5.Text = "消耗品 タイマー";
            // 
            // button4
            // 
            button4.Font = new Font("01フロップデザイン", 13.8749981F);
            button4.Location = new Point(939, 34);
            button4.Name = "button4";
            button4.Size = new Size(160, 114);
            button4.TabIndex = 8;
            button4.Text = "初期化";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button3
            // 
            button3.Font = new Font("01フロップデザイン", 13.8749981F);
            button3.Location = new Point(773, 34);
            button3.Name = "button3";
            button3.Size = new Size(160, 114);
            button3.TabIndex = 7;
            button3.Text = "180m";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button2
            // 
            button2.Font = new Font("01フロップデザイン", 13.8749981F);
            button2.Location = new Point(607, 34);
            button2.Name = "button2";
            button2.Size = new Size(160, 114);
            button2.TabIndex = 6;
            button2.Text = "60m";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Font = new Font("01フロップデザイン", 13.8749981F);
            button1.Location = new Point(441, 34);
            button1.Name = "button1";
            button1.Size = new Size(160, 114);
            button1.TabIndex = 5;
            button1.Text = "30m";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label5.AutoSize = true;
            label5.Font = new Font("01フロップデザイン", 23.9999962F, FontStyle.Regular, GraphicsUnit.Point, 128);
            label5.Location = new Point(87, 47);
            label5.Name = "label5";
            label5.Size = new Size(268, 71);
            label5.TabIndex = 1;
            label5.Text = "**:**:**";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // timer1
            // 
            timer1.Interval = 1000;
            timer1.Tick += Timer1_Tick;
            // 
            // timer2
            // 
            timer2.Interval = 1000;
            timer2.Tick += timer2_Tick;
            // 
            // timer3
            // 
            timer3.Interval = 1000;
            timer3.Tick += timer3_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(14F, 26F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1135, 675);
            Controls.Add(groupBox5);
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Font = new Font("01フロップデザイン", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 128);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            ImeMode = ImeMode.Disable;
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form1";
            Opacity = 0.75D;
            ShowIcon = false;
            Text = "RegClock";
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Label label1;
        private GroupBox groupBox2;
        private Label label2;
        private GroupBox groupBox3;
        private Label label3;
        private GroupBox groupBox4;
        private Label label4;
        private GroupBox groupBox5;
        private Button button2;
        private Button button1;
        private Label label5;
        private Button button4;
        private Button button3;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private Label label7;
        private System.Windows.Forms.Timer timer3;
    }
}
