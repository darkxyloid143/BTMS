<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DeviceDetails
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
        Me.dtgv_bisbiodevices = New System.Windows.Forms.DataGridView()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btn_DeviceUpdate = New System.Windows.Forms.Button()
        Me.lblWindowTitle = New System.Windows.Forms.Label()
        Me.btnx_cancelEdit = New System.Windows.Forms.Button()
        CType(Me.dtgv_bisbiodevices, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'dtgv_bisbiodevices
        '
        Me.dtgv_bisbiodevices.BackgroundColor = System.Drawing.Color.White
        Me.dtgv_bisbiodevices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dtgv_bisbiodevices.Location = New System.Drawing.Point(5, 3)
        Me.dtgv_bisbiodevices.Name = "dtgv_bisbiodevices"
        Me.dtgv_bisbiodevices.RowHeadersVisible = False
        Me.dtgv_bisbiodevices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dtgv_bisbiodevices.Size = New System.Drawing.Size(672, 191)
        Me.dtgv_bisbiodevices.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.btn_DeviceUpdate)
        Me.Panel1.Controls.Add(Me.dtgv_bisbiodevices)
        Me.Panel1.Location = New System.Drawing.Point(0, 29)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(681, 229)
        Me.Panel1.TabIndex = 1
        '
        'btn_DeviceUpdate
        '
        Me.btn_DeviceUpdate.BackColor = System.Drawing.Color.Green
        Me.btn_DeviceUpdate.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btn_DeviceUpdate.Enabled = False
        Me.btn_DeviceUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btn_DeviceUpdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_DeviceUpdate.ForeColor = System.Drawing.Color.White
        Me.btn_DeviceUpdate.Location = New System.Drawing.Point(564, 198)
        Me.btn_DeviceUpdate.Name = "btn_DeviceUpdate"
        Me.btn_DeviceUpdate.Size = New System.Drawing.Size(113, 25)
        Me.btn_DeviceUpdate.TabIndex = 46
        Me.btn_DeviceUpdate.Text = "Update Changes"
        Me.btn_DeviceUpdate.UseVisualStyleBackColor = False
        '
        'lblWindowTitle
        '
        Me.lblWindowTitle.AutoSize = True
        Me.lblWindowTitle.BackColor = System.Drawing.Color.Transparent
        Me.lblWindowTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWindowTitle.ForeColor = System.Drawing.Color.White
        Me.lblWindowTitle.Location = New System.Drawing.Point(6, 9)
        Me.lblWindowTitle.Name = "lblWindowTitle"
        Me.lblWindowTitle.Size = New System.Drawing.Size(84, 13)
        Me.lblWindowTitle.TabIndex = 6
        Me.lblWindowTitle.Text = "BISBIO Devices"
        '
        'btnx_cancelEdit
        '
        Me.btnx_cancelEdit.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btnx_cancelEdit.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnx_cancelEdit.FlatAppearance.BorderSize = 0
        Me.btnx_cancelEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnx_cancelEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnx_cancelEdit.ForeColor = System.Drawing.Color.White
        Me.btnx_cancelEdit.Location = New System.Drawing.Point(603, 262)
        Me.btnx_cancelEdit.Name = "btnx_cancelEdit"
        Me.btnx_cancelEdit.Size = New System.Drawing.Size(75, 28)
        Me.btnx_cancelEdit.TabIndex = 72
        Me.btnx_cancelEdit.Text = "Close"
        Me.btnx_cancelEdit.UseVisualStyleBackColor = False
        '
        'DeviceDetails
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.ClientSize = New System.Drawing.Size(681, 296)
        Me.Controls.Add(Me.btnx_cancelEdit)
        Me.Controls.Add(Me.lblWindowTitle)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "DeviceDetails"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "DeviceDetails"
        Me.TopMost = True
        CType(Me.dtgv_bisbiodevices, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dtgv_bisbiodevices As System.Windows.Forms.DataGridView
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btn_DeviceUpdate As System.Windows.Forms.Button
    Friend WithEvents lblWindowTitle As System.Windows.Forms.Label
    Friend WithEvents btnx_cancelEdit As System.Windows.Forms.Button
End Class
