namespace MouseToJoysticktest;

partial class JoystickEmulator
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
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        btnStart = new System.Windows.Forms.Button();
        btnStop = new System.Windows.Forms.Button();
        lblStatus = new System.Windows.Forms.Label();
        lblMousePos = new System.Windows.Forms.Label();
        lblJoystickPos = new System.Windows.Forms.Label();
        lblScreenCenter = new System.Windows.Forms.Label();
        timer1 = new System.Windows.Forms.Timer(components);
        SuspendLayout();
        // 
        // btnStart
        // 
        btnStart.Location = new System.Drawing.Point(23, 26);
        btnStart.Name = "btnStart";
        btnStart.Size = new System.Drawing.Size(315, 32);
        btnStart.TabIndex = 0;
        btnStart.Text = "Start Emulation";
        btnStart.UseVisualStyleBackColor = true;
        btnStart.Click += btnStart_Click;
        // 
        // btnStop
        // 
        btnStop.Enabled = false;
        btnStop.Location = new System.Drawing.Point(23, 64);
        btnStop.Name = "btnStop";
        btnStop.Size = new System.Drawing.Size(315, 31);
        btnStop.TabIndex = 1;
        btnStop.Text = "Stop Emulation";
        btnStop.UseVisualStyleBackColor = true;
        btnStop.Click += btnStop_Click;
        // 
        // lblStatus
        // 
        lblStatus.Location = new System.Drawing.Point(23, 110);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new System.Drawing.Size(315, 23);
        lblStatus.TabIndex = 2;
        lblStatus.Text = "Status: Stopped";
        // 
        // lblMousePos
        // 
        lblMousePos.Location = new System.Drawing.Point(23, 142);
        lblMousePos.Name = "lblMousePos";
        lblMousePos.Size = new System.Drawing.Size(315, 23);
        lblMousePos.TabIndex = 3;
        lblMousePos.Text = "Mouse (0, 0)";
        // 
        // lblJoystickPos
        // 
        lblJoystickPos.Location = new System.Drawing.Point(23, 175);
        lblJoystickPos.Name = "lblJoystickPos";
        lblJoystickPos.Size = new System.Drawing.Size(315, 23);
        lblJoystickPos.TabIndex = 4;
        lblJoystickPos.Text = "Joystick (0, 0)";
        // 
        // lblScreenCenter
        // 
        lblScreenCenter.Location = new System.Drawing.Point(23, 211);
        lblScreenCenter.Name = "lblScreenCenter";
        lblScreenCenter.Size = new System.Drawing.Size(315, 23);
        lblScreenCenter.TabIndex = 5;
        lblScreenCenter.Text = "Center (0, 0)";
        // 
        // timer1
        // 
        timer1.Interval = 20;
        timer1.Tick += timer1_Tick;
        // 
        // JoystickEmulator
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(366, 467);
        Controls.Add(lblScreenCenter);
        Controls.Add(lblJoystickPos);
        Controls.Add(lblMousePos);
        Controls.Add(lblStatus);
        Controls.Add(btnStop);
        Controls.Add(btnStart);
        Text = "JoystickEmulator";
        Load += JoystickEmulator_Load;
        Closing += JoystickEmulator_Closing;
        ResumeLayout(false);
    }

    private System.Windows.Forms.Timer timer1;

    private System.Windows.Forms.Label lblJoystickPos;
    private System.Windows.Forms.Label lblScreenCenter;

    private System.Windows.Forms.Label lblStatus;
    private System.Windows.Forms.Label lblMousePos;

    private System.Windows.Forms.Button btnStop;

    private System.Windows.Forms.Button btnStart;

    #endregion
}