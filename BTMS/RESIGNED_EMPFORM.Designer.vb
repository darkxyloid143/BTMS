<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RESIGNED_EMPFORM
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
        Dim ListViewItem2 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("")
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btn_rehired = New System.Windows.Forms.Button()
        Me.lsv_stf = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.btn_close = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.btn_rehired)
        Me.Panel1.Controls.Add(Me.lsv_stf)
        Me.Panel1.Location = New System.Drawing.Point(0, 24)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(513, 288)
        Me.Panel1.TabIndex = 0
        '
        'btn_rehired
        '
        Me.btn_rehired.BackColor = System.Drawing.Color.Green
        Me.btn_rehired.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btn_rehired.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btn_rehired.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btn_rehired.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_rehired.ForeColor = System.Drawing.Color.White
        Me.btn_rehired.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btn_rehired.Location = New System.Drawing.Point(432, 243)
        Me.btn_rehired.Name = "btn_rehired"
        Me.btn_rehired.Size = New System.Drawing.Size(75, 22)
        Me.btn_rehired.TabIndex = 74
        Me.btn_rehired.Text = "Rehired"
        Me.btn_rehired.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btn_rehired.UseVisualStyleBackColor = False
        '
        'lsv_stf
        '
        Me.lsv_stf.Activation = System.Windows.Forms.ItemActivation.OneClick
        Me.lsv_stf.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lsv_stf.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3})
        Me.lsv_stf.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lsv_stf.ForeColor = System.Drawing.Color.Black
        Me.lsv_stf.FullRowSelect = True
        Me.lsv_stf.GridLines = True
        Me.lsv_stf.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        ListViewItem2.StateImageIndex = 0
        Me.lsv_stf.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem2})
        Me.lsv_stf.Location = New System.Drawing.Point(2, 3)
        Me.lsv_stf.Name = "lsv_stf"
        Me.lsv_stf.Size = New System.Drawing.Size(506, 237)
        Me.lsv_stf.TabIndex = 48
        Me.lsv_stf.UseCompatibleStateImageBehavior = False
        Me.lsv_stf.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "ID"
        Me.ColumnHeader1.Width = 100
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Name"
        Me.ColumnHeader2.Width = 250
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Resigned Date"
        Me.ColumnHeader3.Width = 150
        '
        'btn_close
        '
        Me.btn_close.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btn_close.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btn_close.FlatAppearance.BorderSize = 0
        Me.btn_close.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btn_close.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_close.ForeColor = System.Drawing.Color.White
        Me.btn_close.Location = New System.Drawing.Point(431, 321)
        Me.btn_close.Name = "btn_close"
        Me.btn_close.Size = New System.Drawing.Size(75, 28)
        Me.btn_close.TabIndex = 5
        Me.btn_close.Text = "Close"
        Me.btn_close.UseVisualStyleBackColor = False
        '
        'RESIGNED_EMPFORM
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(513, 356)
        Me.Controls.Add(Me.btn_close)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "RESIGNED_EMPFORM"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "RESIGNED_EMPFORM"
        Me.TopMost = True
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btn_close As System.Windows.Forms.Button
    Friend WithEvents lsv_stf As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents btn_rehired As System.Windows.Forms.Button
End Class
