<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LEAVETYPEFORM
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnSaveSchema = New System.Windows.Forms.Button()
        Me.dtgv_leaveSchema = New System.Windows.Forms.DataGridView()
        Me.lblWindowTitle = New System.Windows.Forms.Label()
        Me.btnx_cancelEdit = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout()
        CType(Me.dtgv_leaveSchema, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.btnSaveSchema)
        Me.Panel1.Controls.Add(Me.dtgv_leaveSchema)
        Me.Panel1.Location = New System.Drawing.Point(0, 27)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(719, 262)
        Me.Panel1.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.White
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(557, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(151, 180)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "This section will defined" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "all leaves of the employee." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "INFO:" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "If you wan" & _
    "t to remove" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "employee filed leave" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "in the  database " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "use the VOID LEAVE." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'btnSaveSchema
        '
        Me.btnSaveSchema.BackColor = System.Drawing.Color.Green
        Me.btnSaveSchema.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnSaveSchema.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnSaveSchema.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSaveSchema.ForeColor = System.Drawing.Color.White
        Me.btnSaveSchema.Location = New System.Drawing.Point(440, 228)
        Me.btnSaveSchema.Name = "btnSaveSchema"
        Me.btnSaveSchema.Size = New System.Drawing.Size(111, 28)
        Me.btnSaveSchema.TabIndex = 44
        Me.btnSaveSchema.Text = "Update Changes"
        Me.btnSaveSchema.UseVisualStyleBackColor = False
        '
        'dtgv_leaveSchema
        '
        Me.dtgv_leaveSchema.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dtgv_leaveSchema.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.dtgv_leaveSchema.BackgroundColor = System.Drawing.Color.White
        Me.dtgv_leaveSchema.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dtgv_leaveSchema.Location = New System.Drawing.Point(10, 3)
        Me.dtgv_leaveSchema.MultiSelect = False
        Me.dtgv_leaveSchema.Name = "dtgv_leaveSchema"
        Me.dtgv_leaveSchema.RowHeadersVisible = False
        Me.dtgv_leaveSchema.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dtgv_leaveSchema.Size = New System.Drawing.Size(541, 219)
        Me.dtgv_leaveSchema.TabIndex = 2
        '
        'lblWindowTitle
        '
        Me.lblWindowTitle.AutoSize = True
        Me.lblWindowTitle.BackColor = System.Drawing.Color.Transparent
        Me.lblWindowTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWindowTitle.ForeColor = System.Drawing.Color.White
        Me.lblWindowTitle.Location = New System.Drawing.Point(6, 7)
        Me.lblWindowTitle.Name = "lblWindowTitle"
        Me.lblWindowTitle.Size = New System.Drawing.Size(73, 15)
        Me.lblWindowTitle.TabIndex = 6
        Me.lblWindowTitle.Text = "Leave Class"
        '
        'btnx_cancelEdit
        '
        Me.btnx_cancelEdit.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btnx_cancelEdit.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnx_cancelEdit.FlatAppearance.BorderSize = 0
        Me.btnx_cancelEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnx_cancelEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnx_cancelEdit.ForeColor = System.Drawing.Color.White
        Me.btnx_cancelEdit.Location = New System.Drawing.Point(640, 293)
        Me.btnx_cancelEdit.Name = "btnx_cancelEdit"
        Me.btnx_cancelEdit.Size = New System.Drawing.Size(75, 28)
        Me.btnx_cancelEdit.TabIndex = 72
        Me.btnx_cancelEdit.Text = "Close"
        Me.btnx_cancelEdit.UseVisualStyleBackColor = False
        '
        'LEAVETYPEFORM
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(719, 325)
        Me.Controls.Add(Me.btnx_cancelEdit)
        Me.Controls.Add(Me.lblWindowTitle)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "LEAVETYPEFORM"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.TopMost = True
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.dtgv_leaveSchema, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents dtgv_leaveSchema As System.Windows.Forms.DataGridView
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnSaveSchema As System.Windows.Forms.Button
    Friend WithEvents lblWindowTitle As System.Windows.Forms.Label
    Friend WithEvents btnx_cancelEdit As System.Windows.Forms.Button
End Class
