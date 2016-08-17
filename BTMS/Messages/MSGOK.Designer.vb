<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MSGOK
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
        Me.pbx_msgOK = New System.Windows.Forms.PictureBox()
        Me.lblMsg = New System.Windows.Forms.Label()
        Me.msgbtn_exit = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout()
        CType(Me.pbx_msgOK, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.pbx_msgOK)
        Me.Panel1.Controls.Add(Me.lblMsg)
        Me.Panel1.Location = New System.Drawing.Point(-6, -2)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(321, 121)
        Me.Panel1.TabIndex = 0
        '
        'pbx_msgOK
        '
        Me.pbx_msgOK.Image = Global.BTMS.My.Resources.Resources.wrong
        Me.pbx_msgOK.Location = New System.Drawing.Point(12, 13)
        Me.pbx_msgOK.Name = "pbx_msgOK"
        Me.pbx_msgOK.Size = New System.Drawing.Size(88, 85)
        Me.pbx_msgOK.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbx_msgOK.TabIndex = 1
        Me.pbx_msgOK.TabStop = False
        '
        'lblMsg
        '
        Me.lblMsg.BackColor = System.Drawing.Color.Transparent
        Me.lblMsg.ForeColor = System.Drawing.Color.White
        Me.lblMsg.Location = New System.Drawing.Point(111, 29)
        Me.lblMsg.Name = "lblMsg"
        Me.lblMsg.Size = New System.Drawing.Size(201, 69)
        Me.lblMsg.TabIndex = 0
        Me.lblMsg.Text = "Updated successfuly"
        '
        'msgbtn_exit
        '
        Me.msgbtn_exit.BackColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(40, Byte), Integer))
        Me.msgbtn_exit.Cursor = System.Windows.Forms.Cursors.Hand
        Me.msgbtn_exit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.msgbtn_exit.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.msgbtn_exit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.msgbtn_exit.ForeColor = System.Drawing.Color.White
        Me.msgbtn_exit.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.msgbtn_exit.Location = New System.Drawing.Point(232, 124)
        Me.msgbtn_exit.Name = "msgbtn_exit"
        Me.msgbtn_exit.Size = New System.Drawing.Size(75, 22)
        Me.msgbtn_exit.TabIndex = 74
        Me.msgbtn_exit.Text = "OK"
        Me.msgbtn_exit.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.msgbtn_exit.UseVisualStyleBackColor = False
        '
        'MSGOK
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(315, 152)
        Me.Controls.Add(Me.msgbtn_exit)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "MSGOK"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "MSGOK"
        Me.TopMost = True
        Me.Panel1.ResumeLayout(False)
        CType(Me.pbx_msgOK, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents msgbtn_exit As System.Windows.Forms.Button
    Friend WithEvents lblMsg As System.Windows.Forms.Label
    Friend WithEvents pbx_msgOK As System.Windows.Forms.PictureBox
End Class
