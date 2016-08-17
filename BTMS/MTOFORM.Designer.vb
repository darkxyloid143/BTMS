<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MTOFORM
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
        Me.btnSaveSchema = New System.Windows.Forms.Button()
        Me.dtgv_mtoschema = New System.Windows.Forms.DataGridView()
        Me.lblWindowTitle = New System.Windows.Forms.Label()
        Me.btnx_cancelEdit = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout()
        CType(Me.dtgv_mtoschema, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.btnSaveSchema)
        Me.Panel1.Controls.Add(Me.dtgv_mtoschema)
        Me.Panel1.Location = New System.Drawing.Point(0, 28)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(474, 245)
        Me.Panel1.TabIndex = 0
        '
        'btnSaveSchema
        '
        Me.btnSaveSchema.BackColor = System.Drawing.Color.Green
        Me.btnSaveSchema.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnSaveSchema.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnSaveSchema.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSaveSchema.ForeColor = System.Drawing.Color.White
        Me.btnSaveSchema.Location = New System.Drawing.Point(347, 214)
        Me.btnSaveSchema.Name = "btnSaveSchema"
        Me.btnSaveSchema.Size = New System.Drawing.Size(114, 24)
        Me.btnSaveSchema.TabIndex = 46
        Me.btnSaveSchema.Text = "Update Changes"
        Me.btnSaveSchema.UseVisualStyleBackColor = False
        '
        'dtgv_mtoschema
        '
        Me.dtgv_mtoschema.BackgroundColor = System.Drawing.Color.White
        Me.dtgv_mtoschema.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dtgv_mtoschema.Location = New System.Drawing.Point(3, 6)
        Me.dtgv_mtoschema.Name = "dtgv_mtoschema"
        Me.dtgv_mtoschema.RowHeadersVisible = False
        Me.dtgv_mtoschema.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dtgv_mtoschema.Size = New System.Drawing.Size(458, 202)
        Me.dtgv_mtoschema.TabIndex = 2
        '
        'lblWindowTitle
        '
        Me.lblWindowTitle.AutoSize = True
        Me.lblWindowTitle.BackColor = System.Drawing.Color.Transparent
        Me.lblWindowTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWindowTitle.ForeColor = System.Drawing.Color.White
        Me.lblWindowTitle.Location = New System.Drawing.Point(3, 9)
        Me.lblWindowTitle.Name = "lblWindowTitle"
        Me.lblWindowTitle.Size = New System.Drawing.Size(107, 15)
        Me.lblWindowTitle.TabIndex = 6
        Me.lblWindowTitle.Text = "Travel Order Class"
        '
        'btnx_cancelEdit
        '
        Me.btnx_cancelEdit.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btnx_cancelEdit.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnx_cancelEdit.FlatAppearance.BorderSize = 0
        Me.btnx_cancelEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnx_cancelEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnx_cancelEdit.ForeColor = System.Drawing.Color.White
        Me.btnx_cancelEdit.Location = New System.Drawing.Point(387, 279)
        Me.btnx_cancelEdit.Name = "btnx_cancelEdit"
        Me.btnx_cancelEdit.Size = New System.Drawing.Size(75, 28)
        Me.btnx_cancelEdit.TabIndex = 73
        Me.btnx_cancelEdit.Text = "Close"
        Me.btnx_cancelEdit.UseVisualStyleBackColor = False
        '
        'MTOFORM
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(474, 311)
        Me.Controls.Add(Me.btnx_cancelEdit)
        Me.Controls.Add(Me.lblWindowTitle)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "MTOFORM"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "MTOFORM"
        Me.TopMost = True
        Me.Panel1.ResumeLayout(False)
        CType(Me.dtgv_mtoschema, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lblWindowTitle As System.Windows.Forms.Label
    Friend WithEvents dtgv_mtoschema As System.Windows.Forms.DataGridView
    Friend WithEvents btnSaveSchema As System.Windows.Forms.Button
    Friend WithEvents btnx_cancelEdit As System.Windows.Forms.Button
End Class
