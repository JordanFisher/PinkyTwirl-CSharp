namespace PinkyTwirl
{
    partial class PinkyTwirlForm
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
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.LogCheckbox = new System.Windows.Forms.CheckBox();
            this.ActiveCheckbox = new System.Windows.Forms.CheckBox();
            this.GamepadCheckbox = new System.Windows.Forms.CheckBox();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxLog
            // 
            this.textBoxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLog.Location = new System.Drawing.Point(0, 46);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ReadOnly = true;
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(353, 387);
            this.textBoxLog.TabIndex = 5;
            this.textBoxLog.WordWrap = false;
            this.textBoxLog.TextChanged += new System.EventHandler(this.textBoxLog_TextChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.GamepadCheckbox);
            this.groupBox2.Controls.Add(this.LogCheckbox);
            this.groupBox2.Controls.Add(this.ActiveCheckbox);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(353, 46);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            // 
            // LogCheckbox
            // 
            this.LogCheckbox.AutoSize = true;
            this.LogCheckbox.Location = new System.Drawing.Point(145, 20);
            this.LogCheckbox.Name = "LogCheckbox";
            this.LogCheckbox.Size = new System.Drawing.Size(44, 17);
            this.LogCheckbox.TabIndex = 1;
            this.LogCheckbox.Text = "Log";
            this.LogCheckbox.UseVisualStyleBackColor = true;
            this.LogCheckbox.CheckedChanged += new System.EventHandler(this.LogCheckbox_CheckedChanged);
            // 
            // ActiveCheckbox
            // 
            this.ActiveCheckbox.AutoSize = true;
            this.ActiveCheckbox.Location = new System.Drawing.Point(13, 20);
            this.ActiveCheckbox.Name = "ActiveCheckbox";
            this.ActiveCheckbox.Size = new System.Drawing.Size(56, 17);
            this.ActiveCheckbox.TabIndex = 0;
            this.ActiveCheckbox.Text = "Active";
            this.ActiveCheckbox.UseVisualStyleBackColor = true;
            this.ActiveCheckbox.CheckedChanged += new System.EventHandler(this.Active_CheckedChanged);
            // 
            // GamepadCheckbox
            // 
            this.GamepadCheckbox.AutoSize = true;
            this.GamepadCheckbox.Location = new System.Drawing.Point(71, 20);
            this.GamepadCheckbox.Name = "GamepadCheckbox";
            this.GamepadCheckbox.Size = new System.Drawing.Size(72, 17);
            this.GamepadCheckbox.TabIndex = 2;
            this.GamepadCheckbox.Text = "Gamepad";
            this.GamepadCheckbox.UseVisualStyleBackColor = true;
            this.GamepadCheckbox.CheckedChanged += new System.EventHandler(this.GamepadCheckbox_CheckedChanged);
            // 
            // PinkyTwirlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(353, 433);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.groupBox2);
            this.Name = "PinkyTwirlForm";
            this.Text = "PinkyTwirl";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox ActiveCheckbox;
        private System.Windows.Forms.CheckBox LogCheckbox;
        private System.Windows.Forms.CheckBox GamepadCheckbox;
    }
}