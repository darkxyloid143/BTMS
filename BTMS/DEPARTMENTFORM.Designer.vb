<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DEPARTMENTFORM
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
        Me.txt_department_name = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btn_RemoveDepartment = New System.Windows.Forms.Button()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.btn_EditDepartment = New System.Windows.Forms.Button()
        Me.btn_addDepartment = New System.Windows.Forms.Button()
        Me.lblWindowTitle = New System.Windows.Forms.Label()
        Me.btnx_cancelEdit = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.txt_department_name)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.btn_RemoveDepartment)
        Me.Panel1.Controls.Add(Me.ListView1)
        Me.Panel1.Controls.Add(Me.btn_EditDepartment)
        Me.Panel1.Controls.Add(Me.btn_addDepartment)
        Me.Panel1.Location = New System.Drawing.Point(0, 25)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(434, 263)
        Me.Panel1.TabIndex = 0
        '
        'txt_department_name
        '
        Me.txt_department_name.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txt_department_name.Enabled = False
        Me.txt_department_name.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_department_name.Location = New System.Drawing.Point(61, 11)
        Me.txt_department_name.Name = "txt_department_name"
        Me.txt_department_name.Size = New System.Drawing.Size(368, 21)
        Me.txt_department_name.TabIndex = 75
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Black
        Me.Label4.Location = New System.Drawing.Point(6, 14)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(55, 15)
        Me.Label4.TabIndex = 76
        Me.Label4.Text = "Selected"
        '
        'btn_RemoveDepartment
        '
        Me.btn_RemoveDepartment.BackColor = System.Drawing.Color.Red
        Me.btn_RemoveDepartment.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btn_RemoveDepartment.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btn_RemoveDepartment.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btn_RemoveDepartment.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_RemoveDepartment.ForeColor = System.Drawing.Color.White
        Me.btn_RemoveDepartment.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btn_RemoveDepartment.Location = New System.Drawing.Point(350, 218)
        Me.btn_RemoveDepartment.Name = "btn_RemoveDepartment"
        Me.btn_RemoveDepartment.Size = New System.Drawing.Size(75, 22)
        Me.btn_RemoveDepartment.TabIndex = 74
        Me.btn_RemoveDepartment.Text = "Remove"
        Me.btn_RemoveDepartment.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btn_RemoveDepartment.UseVisualStyleBackColor = False
        '
        'ListView1
        '
        Me.ListView1.Activation = System.Windows.Forms.ItemActivation.OneClick
        Me.ListView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.ListView1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListView1.ForeColor = System.Drawing.Color.Black
        Me.ListView1.FullRowSelect = True
        Me.ListView1.GridLines = True
        Me.ListView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        ListViewItem2.StateImageIndex = 0
        Me.ListView1.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem2})
        Me.ListView1.Location = New System.Drawing.Point(3, 39)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(424, 173)
        Me.ListView1.TabIndex = 47
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Department Name"
        Me.ColumnHeader1.Width = 280
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "No. Employees"
        Me.ColumnHeader2.Width = 100
        '
        'btn_EditDepartment
        '
        Me.btn_EditDepartment.BackColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(40, Byte), Integer))
        Me.btn_EditDepartment.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btn_EditDepartment.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btn_EditDepartment.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btn_EditDepartment.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_EditDepartment.ForeColor = System.Drawing.Color.White
        Me.btn_EditDepartment.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btn_EditDepartment.Location = New System.Drawing.Point(269, 218)
        Me.btn_EditDepartment.Name = "btn_EditDepartment"
        Me.btn_EditDepartment.Size = New System.Drawing.Size(75, 22)
        Me.btn_EditDepartment.TabIndex = 73
        Me.btn_EditDepartment.Text = "Edit"
        Me.btn_EditDepartment.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btn_EditDepartment.UseVisualStyleBackColor = False
        '
        'btn_addDepartment
        '
        Me.btn_addDepartment.BackColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(40, Byte), Integer))
        Me.btn_addDepartment.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btn_addDepartment.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btn_addDepartment.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btn_addDepartment.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_addDepartment.ForeColor = System.Drawing.Color.White
        Me.btn_addDepartment.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btn_addDepartment.Location = New System.Drawing.Point(183, 218)
        Me.btn_addDepartment.Name = "btn_addDepartment"
        Me.btn_addDepartment.Size = New System.Drawing.Size(75, 22)
        Me.btn_addDepartment.TabIndex = 72
        Me.btn_addDepartment.Text = "New"
        Me.btn_addDepartment.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btn_addDepartment.UseVisualStyleBackColor = False
        '
        'lblWindowTitle
        '
        Me.lblWindowTitle.AutoSize = True
        Me.lblWindowTitle.BackColor = System.Drawing.Color.Transparent
        Me.lblWindowTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWindowTitle.ForeColor = System.Drawing.Color.White
        Me.lblWindowTitle.Location = New System.Drawing.Point(4, 5)
        Me.lblWindowTitle.Name = "lblWindowTitle"
        Me.lblWindowTitle.Size = New System.Drawing.Size(105, 15)
        Me.lblWindowTitle.TabIndex = 5
        Me.lblWindowTitle.Text = "Department Class"
        '
        'btnx_cancelEdit
        '
        Me.btnx_cancelEdit.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btnx_cancelEdit.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnx_cancelEdit.FlatAppearance.BorderSize = 0
        Me.btnx_cancelEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnx_cancelEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnx_cancelEdit.ForeColor = System.Drawing.Color.White
        Me.btnx_cancelEdit.Location = New System.Drawing.Point(354, 294)
        Me.btnx_cancelEdit.Name = "btnx_cancelEdit"
        Me.btnx_cancelEdit.Size = New System.Drawing.Size(75, 28)
        Me.btnx_cancelEdit.TabIndex = 71
        Me.btnx_cancelEdit.Text = "Close"
        Me.btnx_cancelEdit.UseVisualStyleBackColor = False
        '
        'DEPARTMENTFORM
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(434, 326)
        Me.Controls.Add(Me.btnx_cancelEdit)
        Me.Controls.Add(Me.lblWindowTitle)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "DEPARTMENTFORM"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "DEPARTMENTFORM"
        Me.TopMost = True
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lblWindowTitle As System.Windows.Forms.Label
    Friend WithEvents btnx_cancelEdit As System.Windows.Forms.Button
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents btn_RemoveDepartment As System.Windows.Forms.Button
    Friend WithEvents btn_EditDepartment As System.Windows.Forms.Button
    Friend WithEvents btn_addDepartment As System.Windows.Forms.Button
    Friend WithEvents txt_department_name As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
End Class
