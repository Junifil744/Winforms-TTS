namespace WF_TTS
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
            comboBox1 = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            cableBox = new System.Windows.Forms.ComboBox();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            deviceBox = new System.Windows.Forms.ComboBox();
            configNuke = new System.Windows.Forms.Button();
            deviceRefresh = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new System.Drawing.Point(14, 29);
            comboBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new System.Drawing.Size(257, 23);
            comboBox1.TabIndex = 0;
            comboBox1.Text = "Microsoft Zira Desktop";
            comboBox1.SelectedIndexChanged += changeVoice;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(102, 10);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(69, 15);
            label1.TabIndex = 1;
            label1.Text = "Select Voice";
            // 
            // cableBox
            // 
            cableBox.DropDownWidth = 200;
            cableBox.FormattingEnabled = true;
            cableBox.Location = new System.Drawing.Point(14, 73);
            cableBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cableBox.Name = "cableBox";
            cableBox.Size = new System.Drawing.Size(125, 23);
            cableBox.TabIndex = 2;
            cableBox.Text = "null";
            cableBox.SelectedIndexChanged += deviceChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(14, 55);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(90, 15);
            label2.TabIndex = 3;
            label2.Text = "Select VB-Cable";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(160, 55);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(111, 15);
            label3.TabIndex = 4;
            label3.Text = "Select Audio Device";
            // 
            // deviceBox
            // 
            deviceBox.DropDownWidth = 200;
            deviceBox.FormattingEnabled = true;
            deviceBox.Location = new System.Drawing.Point(148, 73);
            deviceBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            deviceBox.Name = "deviceBox";
            deviceBox.Size = new System.Drawing.Size(125, 23);
            deviceBox.TabIndex = 5;
            deviceBox.Text = "null";
            deviceBox.SelectedIndexChanged += audioChanged;
            // 
            // configNuke
            // 
            configNuke.Location = new System.Drawing.Point(14, 102);
            configNuke.Name = "configNuke";
            configNuke.Size = new System.Drawing.Size(125, 23);
            configNuke.TabIndex = 6;
            configNuke.Text = "Nuke Config";
            configNuke.UseVisualStyleBackColor = true;
            configNuke.Click += configNuke_Click;
            // 
            // deviceRefresh
            // 
            deviceRefresh.Location = new System.Drawing.Point(148, 102);
            deviceRefresh.Name = "deviceRefresh";
            deviceRefresh.Size = new System.Drawing.Size(125, 23);
            deviceRefresh.TabIndex = 7;
            deviceRefresh.Text = "Refresh Devices";
            deviceRefresh.UseVisualStyleBackColor = true;
            deviceRefresh.Click += deviceRefresh_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(286, 136);
            Controls.Add(deviceRefresh);
            Controls.Add(configNuke);
            Controls.Add(deviceBox);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(cableBox);
            Controls.Add(label1);
            Controls.Add(comboBox1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            Name = "MainForm";
            Text = "Voice Select";
            WindowState = System.Windows.Forms.FormWindowState.Minimized;
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cableBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox deviceBox;
        private System.Windows.Forms.Button configNuke;
        private System.Windows.Forms.Button deviceRefresh;
    }
}

