<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MSGYESNO
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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblMsg = New System.Windows.Forms.Label()
        Me.msgbtn_no = New System.Windows.Forms.Button()
        Me.msgbtn_yes = New System.Windows.Forms.Button()
        Me.pbx_msgOK = New System.Windows.Forms.PictureBox()
        Me.Panel1.SuspendLayout()
        CType(Me.pbx_msgOK, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.pbx_msgOK)
        Me.Panel1.Controls.Add(Me.lblMsg)
        Me.Panel1.Location = New System.Drawing.Point(-8, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(321, 121)
        Me.Panel1.TabIndex = 1
        '
        'lblMsg
        '
        Me.lblMsg.BackColor = System.Drawing.Color.Transparent
        Me.lblMsg.ForeColor = System.Drawing.Color.White
        Me.lblMsg.Location = New System.Drawing.Point(111, 29)
        Me.lblMsg.Name = "lblMsg"
        Me.lblMsg.Size = New System.Drawing.Size(190, 69)
        Me.lblMsg.TabIndex = 0
        Me.lblMsg.Text = "Syste Message"
        '
        'msgbtn_no
        '
        Me.msgbtn_no.BackColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(40, Byte), Integer))
        Me.msgbtn_no.Cursor = System.Windows.Forms.Cursors.Hand
        Me.msgbtn_no.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.msgbtn_no.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.msgbtn_no.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.msgbtn_no.ForeColor = System.Drawing.Color.White
        Me.msgbtn_no.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.msgbtn_no.Location = New System.Drawing.Point(219, 127)
        Me.msgbtn_no.Name = "msgbtn_no"
        Me.msgbtn_no.Size = New System.Drawing.Size(75, 22)
        Me.msgbtn_no.TabIndex = 75
        Me.msgbtn_no.Text = "No"
        Me.msgbtn_no.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.msgbtn_no.UseVisualStyleBackColor = False
        '
        'msgbtn_yes
        '
        Me.msgbtn_yes.BackColor = System.Drawing.Color.Green
        Me.msgbtn_yes.Cursor = System.Windows.Forms.Cursors.Hand
        Me.msgbtn_yes.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.msgbtn_yes.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.msgbtn_yes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.msgbtn_yes.ForeColor = System.Drawing.Color.White
        Me.msgbtn_yes.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.msgbtn_yes.Location = New System.Drawing.Point(123, 127)
        Me.msgbtn_yes.Name = "msgbtn_yes"
        Me.msgbtn_yes.Size = New System.Drawing.Size(75, 22)
        Me.msgbtn_yes.TabIndex = 76
        Me.msgbtn_yes.Text = "Yes"
        Me.msgbtn_yes.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.msgbtn_yes.UseVisualStyleBackColor = False
        '
        'pbx_msgOK
        '
        Me.pbx_msgOK.Image = Global.BTMS.My.Resources.Resources.question
        Me.pbx_msgOK.Location = New System.Drawing.Point(17, 29)
        Me.pbx_msgOK.Name = "pbx_msgOK"
        Me.pbx_msgOK.Size = New System.Drawing.Size(62, 53)
        Me.pbx_msgOK.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbx_msgOK.TabIndex = 1
        Me.pbx_msgOK.TabStop = False
        '
        'MSGYESNO
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(304, 152)
        Me.Controls.Add(Me.msgbtn_yes)
        Me.Controls.Add(Me.msgbtn_no)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "MSGYESNO"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "MSGYESNO"
        Me.TopMost = True
        Me.Panel1.ResumeLayout(False)
        CType(Me.pbx_msgOK, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents pbx_msgOK As System.Windows.Forms.PictureBox
    Friend WithEvents lblMsg As System.Windows.Forms.Label
    Friend WithEvents msgbtn_no As System.Windows.Forms.Button
    Friend WithEvents msgbtn_yes As System.Windows.Forms.Button
End Class
