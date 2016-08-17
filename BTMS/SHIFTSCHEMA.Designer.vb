<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SHIFTSCHEMA
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
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.lblShiftID = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnSaveSchema = New System.Windows.Forms.Button()
        Me.dtgv_ShiftSchema = New System.Windows.Forms.DataGridView()
        Me.lblWindowTitle = New System.Windows.Forms.Label()
        Me.btnx_cancelEdit = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dtgv_ShiftSchema, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.PictureBox1)
        Me.Panel1.Controls.Add(Me.lblShiftID)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.btnSaveSchema)
        Me.Panel1.Controls.Add(Me.dtgv_ShiftSchema)
        Me.Panel1.Location = New System.Drawing.Point(0, 24)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(909, 283)
        Me.Panel1.TabIndex = 0
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.BTMS.My.Resources.Resources.Right_128
        Me.PictureBox1.Location = New System.Drawing.Point(18, 249)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(49, 31)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox1.TabIndex = 48
        Me.PictureBox1.TabStop = False
        '
        'lblShiftID
        '
        Me.lblShiftID.AutoSize = True
        Me.lblShiftID.BackColor = System.Drawing.Color.Transparent
        Me.lblShiftID.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShiftID.ForeColor = System.Drawing.Color.Red
        Me.lblShiftID.Location = New System.Drawing.Point(151, 256)
        Me.lblShiftID.Name = "lblShiftID"
        Me.lblShiftID.Size = New System.Drawing.Size(16, 17)
        Me.lblShiftID.TabIndex = 47
        Me.lblShiftID.Text = "0"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Black
        Me.Label1.ForeColor = System.Drawing.Color.Lime
        Me.Label1.Location = New System.Drawing.Point(73, 256)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 15)
        Me.Label1.TabIndex = 46
        Me.Label1.Text = "SHIFTID   = "
        '
        'btnSaveSchema
        '
        Me.btnSaveSchema.BackColor = System.Drawing.Color.Green
        Me.btnSaveSchema.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnSaveSchema.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnSaveSchema.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSaveSchema.ForeColor = System.Drawing.Color.White
        Me.btnSaveSchema.Location = New System.Drawing.Point(782, 251)
        Me.btnSaveSchema.Name = "btnSaveSchema"
        Me.btnSaveSchema.Size = New System.Drawing.Size(114, 24)
        Me.btnSaveSchema.TabIndex = 45
        Me.btnSaveSchema.Text = "Update Changes"
        Me.btnSaveSchema.UseVisualStyleBackColor = False
        '
        'dtgv_ShiftSchema
        '
        Me.dtgv_ShiftSchema.BackgroundColor = System.Drawing.Color.White
        Me.dtgv_ShiftSchema.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dtgv_ShiftSchema.Location = New System.Drawing.Point(11, 20)
        Me.dtgv_ShiftSchema.Name = "dtgv_ShiftSchema"
        Me.dtgv_ShiftSchema.RowHeadersVisible = False
        Me.dtgv_ShiftSchema.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.dtgv_ShiftSchema.Size = New System.Drawing.Size(885, 225)
        Me.dtgv_ShiftSchema.TabIndex = 1
        '
        'lblWindowTitle
        '
        Me.lblWindowTitle.AutoSize = True
        Me.lblWindowTitle.BackColor = System.Drawing.Color.Transparent
        Me.lblWindowTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWindowTitle.ForeColor = System.Drawing.Color.White
        Me.lblWindowTitle.Location = New System.Drawing.Point(5, 6)
        Me.lblWindowTitle.Name = "lblWindowTitle"
        Me.lblWindowTitle.Size = New System.Drawing.Size(80, 15)
        Me.lblWindowTitle.TabIndex = 5
        Me.lblWindowTitle.Text = "Shift Schema"
        '
        'btnx_cancelEdit
        '
        Me.btnx_cancelEdit.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btnx_cancelEdit.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnx_cancelEdit.FlatAppearance.BorderSize = 0
        Me.btnx_cancelEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnx_cancelEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnx_cancelEdit.ForeColor = System.Drawing.Color.White
        Me.btnx_cancelEdit.Location = New System.Drawing.Point(822, 313)
        Me.btnx_cancelEdit.Name = "btnx_cancelEdit"
        Me.btnx_cancelEdit.Size = New System.Drawing.Size(75, 28)
        Me.btnx_cancelEdit.TabIndex = 72
        Me.btnx_cancelEdit.Text = "Close"
        Me.btnx_cancelEdit.UseVisualStyleBackColor = False
        '
        'SHIFTSCHEMA
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(909, 345)
        Me.Controls.Add(Me.btnx_cancelEdit)
        Me.Controls.Add(Me.lblWindowTitle)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "SHIFTSCHEMA"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "SHIFTSCHEMA"
        Me.TopMost = True
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dtgv_ShiftSchema, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents dtgv_ShiftSchema As System.Windows.Forms.DataGridView
    Friend WithEvents btnSaveSchema As System.Windows.Forms.Button
    Friend WithEvents lblShiftID As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblWindowTitle As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents btnx_cancelEdit As System.Windows.Forms.Button
End Class
