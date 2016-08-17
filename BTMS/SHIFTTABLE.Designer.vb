<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SHIFTTABLE
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
        Me.btnUpdateChanges = New System.Windows.Forms.Button()
        Me.dtgv_shiftTable = New System.Windows.Forms.DataGridView()
        Me.lblWindowTitle = New System.Windows.Forms.Label()
        Me.btnx_cancelEdit = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout()
        CType(Me.dtgv_shiftTable, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.btnUpdateChanges)
        Me.Panel1.Controls.Add(Me.dtgv_shiftTable)
        Me.Panel1.Location = New System.Drawing.Point(0, 27)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(608, 279)
        Me.Panel1.TabIndex = 0
        '
        'btnUpdateChanges
        '
        Me.btnUpdateChanges.BackColor = System.Drawing.Color.Green
        Me.btnUpdateChanges.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnUpdateChanges.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnUpdateChanges.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUpdateChanges.ForeColor = System.Drawing.Color.White
        Me.btnUpdateChanges.Location = New System.Drawing.Point(487, 251)
        Me.btnUpdateChanges.Name = "btnUpdateChanges"
        Me.btnUpdateChanges.Size = New System.Drawing.Size(109, 24)
        Me.btnUpdateChanges.TabIndex = 46
        Me.btnUpdateChanges.Text = "Update Changes"
        Me.btnUpdateChanges.UseVisualStyleBackColor = False
        '
        'dtgv_shiftTable
        '
        Me.dtgv_shiftTable.BackgroundColor = System.Drawing.Color.White
        Me.dtgv_shiftTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dtgv_shiftTable.Location = New System.Drawing.Point(6, 6)
        Me.dtgv_shiftTable.Name = "dtgv_shiftTable"
        Me.dtgv_shiftTable.RowHeadersVisible = False
        Me.dtgv_shiftTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dtgv_shiftTable.Size = New System.Drawing.Size(590, 240)
        Me.dtgv_shiftTable.TabIndex = 0
        '
        'lblWindowTitle
        '
        Me.lblWindowTitle.AutoSize = True
        Me.lblWindowTitle.BackColor = System.Drawing.Color.Transparent
        Me.lblWindowTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWindowTitle.ForeColor = System.Drawing.Color.White
        Me.lblWindowTitle.Location = New System.Drawing.Point(6, 8)
        Me.lblWindowTitle.Name = "lblWindowTitle"
        Me.lblWindowTitle.Size = New System.Drawing.Size(58, 13)
        Me.lblWindowTitle.TabIndex = 4
        Me.lblWindowTitle.Text = "Shift Table"
        '
        'btnx_cancelEdit
        '
        Me.btnx_cancelEdit.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btnx_cancelEdit.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnx_cancelEdit.FlatAppearance.BorderSize = 0
        Me.btnx_cancelEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnx_cancelEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnx_cancelEdit.ForeColor = System.Drawing.Color.White
        Me.btnx_cancelEdit.Location = New System.Drawing.Point(522, 312)
        Me.btnx_cancelEdit.Name = "btnx_cancelEdit"
        Me.btnx_cancelEdit.Size = New System.Drawing.Size(75, 28)
        Me.btnx_cancelEdit.TabIndex = 73
        Me.btnx_cancelEdit.Text = "Close"
        Me.btnx_cancelEdit.UseVisualStyleBackColor = False
        '
        'SHIFTTABLE
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(608, 344)
        Me.Controls.Add(Me.btnx_cancelEdit)
        Me.Controls.Add(Me.lblWindowTitle)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "SHIFTTABLE"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "SHIFTTABLE"
        Me.TopMost = True
        Me.Panel1.ResumeLayout(False)
        CType(Me.dtgv_shiftTable, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents dtgv_shiftTable As System.Windows.Forms.DataGridView
    Friend WithEvents btnUpdateChanges As System.Windows.Forms.Button
    Friend WithEvents lblWindowTitle As System.Windows.Forms.Label
    Friend WithEvents btnx_cancelEdit As System.Windows.Forms.Button
End Class
