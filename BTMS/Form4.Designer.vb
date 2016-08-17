<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form4
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form4))
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.lbl_dev_status = New System.Windows.Forms.Label()
        Me.thread_dev_connect = New System.ComponentModel.BackgroundWorker()
        Me.user_pnl = New System.Windows.Forms.Panel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lbl_staffid = New System.Windows.Forms.Label()
        Me.lbl_staff_address = New System.Windows.Forms.Label()
        Me.pbx_user_img = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblName = New System.Windows.Forms.Label()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.user_pnl.SuspendLayout()
        CType(Me.pbx_user_img, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.SplitContainer1.Panel1.Controls.Add(Me.user_pnl)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.BackColor = System.Drawing.Color.Goldenrod
        Me.SplitContainer1.Panel2.Controls.Add(Me.lbl_dev_status)
        Me.SplitContainer1.Size = New System.Drawing.Size(1147, 625)
        Me.SplitContainer1.SplitterDistance = 580
        Me.SplitContainer1.TabIndex = 0
        '
        'lbl_dev_status
        '
        Me.lbl_dev_status.AutoSize = True
        Me.lbl_dev_status.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_dev_status.ForeColor = System.Drawing.Color.White
        Me.lbl_dev_status.Location = New System.Drawing.Point(6, 18)
        Me.lbl_dev_status.Name = "lbl_dev_status"
        Me.lbl_dev_status.Size = New System.Drawing.Size(224, 31)
        Me.lbl_dev_status.TabIndex = 8
        Me.lbl_dev_status.Text = "                              "
        '
        'thread_dev_connect
        '
        Me.thread_dev_connect.WorkerReportsProgress = True
        Me.thread_dev_connect.WorkerSupportsCancellation = True
        '
        'user_pnl
        '
        Me.user_pnl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.user_pnl.BackColor = System.Drawing.Color.White
        Me.user_pnl.BackgroundImage = Global.BTMS.My.Resources.Resources.mZOobZm
        Me.user_pnl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.user_pnl.Controls.Add(Me.Label4)
        Me.user_pnl.Controls.Add(Me.lbl_staffid)
        Me.user_pnl.Controls.Add(Me.lbl_staff_address)
        Me.user_pnl.Controls.Add(Me.pbx_user_img)
        Me.user_pnl.Controls.Add(Me.Label1)
        Me.user_pnl.Controls.Add(Me.lblName)
        Me.user_pnl.Location = New System.Drawing.Point(3, 3)
        Me.user_pnl.Name = "user_pnl"
        Me.user_pnl.Size = New System.Drawing.Size(1141, 630)
        Me.user_pnl.TabIndex = 7
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.DarkCyan
        Me.Label4.Location = New System.Drawing.Point(24, 301)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(96, 25)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "DETAILS"
        '
        'lbl_staffid
        '
        Me.lbl_staffid.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbl_staffid.AutoSize = True
        Me.lbl_staffid.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_staffid.ForeColor = System.Drawing.Color.DimGray
        Me.lbl_staffid.Location = New System.Drawing.Point(763, 442)
        Me.lbl_staffid.Name = "lbl_staffid"
        Me.lbl_staffid.Size = New System.Drawing.Size(19, 25)
        Me.lbl_staffid.TabIndex = 2
        Me.lbl_staffid.Text = "-"
        '
        'lbl_staff_address
        '
        Me.lbl_staff_address.AutoSize = True
        Me.lbl_staff_address.BackColor = System.Drawing.Color.Transparent
        Me.lbl_staff_address.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_staff_address.ForeColor = System.Drawing.Color.Black
        Me.lbl_staff_address.Location = New System.Drawing.Point(24, 354)
        Me.lbl_staff_address.Name = "lbl_staff_address"
        Me.lbl_staff_address.Size = New System.Drawing.Size(23, 31)
        Me.lbl_staff_address.TabIndex = 6
        Me.lbl_staff_address.Text = "-"
        '
        'pbx_user_img
        '
        Me.pbx_user_img.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbx_user_img.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pbx_user_img.Image = Global.BTMS.My.Resources.Resources.login
        Me.pbx_user_img.Location = New System.Drawing.Point(760, 24)
        Me.pbx_user_img.Name = "pbx_user_img"
        Me.pbx_user_img.Size = New System.Drawing.Size(362, 415)
        Me.pbx_user_img.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbx_user_img.TabIndex = 0
        Me.pbx_user_img.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.DarkCyan
        Me.Label1.Location = New System.Drawing.Point(24, 41)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(70, 25)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "NAME"
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.BackColor = System.Drawing.Color.Transparent
        Me.lblName.Font = New System.Drawing.Font("Microsoft Sans Serif", 30.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblName.ForeColor = System.Drawing.Color.Black
        Me.lblName.Location = New System.Drawing.Point(24, 92)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(33, 46)
        Me.lblName.TabIndex = 3
        Me.lblName.Text = "-"
        '
        'Form4
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1147, 625)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form4"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "-"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.user_pnl.ResumeLayout(False)
        Me.user_pnl.PerformLayout()
        CType(Me.pbx_user_img, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents pbx_user_img As System.Windows.Forms.PictureBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lbl_staffid As System.Windows.Forms.Label
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lbl_staff_address As System.Windows.Forms.Label
    Friend WithEvents user_pnl As System.Windows.Forms.Panel
    Friend WithEvents thread_dev_connect As System.ComponentModel.BackgroundWorker
    Friend WithEvents lbl_dev_status As System.Windows.Forms.Label
End Class
