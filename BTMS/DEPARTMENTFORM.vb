Imports System.Data.SqlServerCe
Imports System.Data
Imports System.IO
Imports BTMS.MSGOK
Public Class DEPARTMENTFORM
    Private SQL As New SQLControl
    Dim department_worker As New SQLCE_MANAGER
    Dim current_selected_department As String = ""
    Private Sub linkExit_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        'Form3.LOAD_DEPARTMENT_TO_COMBOBOX_EMPLOYEEPROFILES()
        'Me.Close()
    End Sub

    Private Sub DEPARTMENTFORM_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        LOAD_DEPARTMENT()
        txt_department_name.Text = ""
    End Sub

    Private Sub LOAD_DEPARTMENT()
        Dim listOfDepartment As New List(Of String)
        listOfDepartment = department_worker.GET_ALL_DEPARTMENT_NAME()
        ListView1.Items.Clear()
        Dim lvItem As New ListViewItem
        For Each dp_name As String In listOfDepartment
            lvItem = ListView1.Items.Add(dp_name)
            lvItem.SubItems.Add(department_worker.COUNT_EMPLOYEE_IN_DEPARTMENT(dp_name))
        Next
    End Sub
    Private Sub btn_UpdateChangeToDepartmentSchema_Click(sender As System.Object, e As System.EventArgs)
        'Try
        '    SQL.SQLDA.Update(SQL.SQLDS)
        '    MessageBox.Show("Update Success!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        'Catch ex As Exception
        '    MessageBox.Show(ex.Message)
        '    Console.WriteLine(ex.Message)
        'End Try
    End Sub
    Private Sub LoadGrid()
        'dtgv_DepartmentSchema.DataSource = SQL.SQLDS.Tables(0)
        'dtgv_DepartmentSchema.Rows(0).Selected = True
        'SQL.SQLDA.UpdateCommand = New SqlServerCe.SqlCeCommandBuilder(SQL.SQLDA).GetUpdateCommand

    End Sub
    'Public Sub ARRANGE_DATAGRID_DEPARTMENT()

    '    For I1 = 0 To dtgv_DepartmentSchema.Columns.Count - 1

    '        If dtgv_DepartmentSchema.Columns(I1).Name = "DEPARTMENT NAME" Then
    '            Dim column As DataGridViewColumn = dtgv_DepartmentSchema.Columns(I1)
    '            column.Width = 1000
    '        ElseIf dtgv_DepartmentSchema.Columns(I1).Name = "ID" Then
    '            Dim column As DataGridViewColumn = dtgv_DepartmentSchema.Columns(I1)
    '            dtgv_DepartmentSchema.Columns(I1).Visible = False
    '            column.Width = 100
    '        End If
    '    Next
    'End Sub

    Private Sub dtgv_DepartmentSchema_CellValueChanged1(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs)
        'btn_UpdateChangeToDepartmentSchema.Enabled = True
    End Sub
    Private Sub dtgv_DepartmentSchema_RowsRemoved1(sender As Object, e As System.Windows.Forms.DataGridViewRowsRemovedEventArgs)
        '  btn_UpdateChangeToDepartmentSchema.Enabled = True
    End Sub
    Private Sub btnx_cancelEdit_Click(sender As System.Object, e As System.EventArgs) Handles btnx_cancelEdit.Click
        Form3.LOAD_DEPARTMENT_TO_COMBOBOX_EMPLOYEEPROFILES()
        btn_addDepartment.Enabled = True
        btn_EditDepartment.Enabled = True
        btn_RemoveDepartment.Enabled = True
        Me.Close()
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ListView1.SelectedIndexChanged
        On Error Resume Next
        Dim sel_item As String = ""

        With ListView1.SelectedItems(0)
            'txt_department_name.Text = .Text
            sel_item = .Text
            If Not String.IsNullOrEmpty(sel_item) Then
                btn_EditDepartment.Enabled = True
                btn_RemoveDepartment.Enabled = True
                btn_addDepartment.Enabled = True
                btn_addDepartment.Text = "New"
                btn_EditDepartment.Text = "Edit"
                btn_RemoveDepartment.Text = "Remove"
            Else
                btn_EditDepartment.Enabled = False
                btn_RemoveDepartment.Enabled = False
                btn_addDepartment.Enabled = True
            End If
        End With
    End Sub

    Private Sub Label4_Click(sender As System.Object, e As System.EventArgs) Handles Label4.Click

    End Sub

    Private Sub btn_addDepartment_Click(sender As System.Object, e As System.EventArgs) Handles btn_addDepartment.Click

        Select Case btn_addDepartment.Text
            Case "New"
                btn_EditDepartment.Enabled = False
                btn_RemoveDepartment.Text = "Cancel"
                btn_RemoveDepartment.BackColor = Color.FromArgb(40, 40, 40)
                btn_addDepartment.Text = "Save"
                ' SetCueText(txt_department_name, "Type department name here")
                txt_department_name.Enabled = True
                btn_RemoveDepartment.Enabled = True
                txt_department_name.Text = ""
                txt_department_name.Text = "Type new department name here"
                txt_department_name.Select()
                txt_department_name.Focus()
            Case "Save"
                Dim department_name As String = ""
                department_name = txt_department_name.Text.Trim

                If Not String.IsNullOrEmpty(department_name) Then

                    If Not department_worker.CHECK_DEPARTMENT_IF_EXIST(department_name) = True Then
                        department_worker.INSERT_NEW_DEPARTMENT(department_name)
                        LOAD_DEPARTMENT()
                        btn_addDepartment.Text = "New"
                        btn_EditDepartment.Enabled = True
                        btn_RemoveDepartment.Text = "Remove"
                        btn_RemoveDepartment.BackColor = Color.Red
                        txt_department_name.Text = ""
                        txt_department_name.Enabled = False
                    Else
                        MessageBox.Show("Department name is already exist!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                Else
                    SetCueText(txt_department_name, "Type department name here")
                    MessageBox.Show("Type department name", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
        End Select

      
    End Sub

    Private Sub btn_EditDepartment_Click(sender As System.Object, e As System.EventArgs) Handles btn_EditDepartment.Click

        Select Case btn_EditDepartment.Text
            Case "Edit"

                On Error Resume Next
                Dim dept_item As String = ""
                With ListView1.SelectedItems(0)
                    txt_department_name.Text = .Text
                End With
                Dim dept_name As String = ""
                dept_name = txt_department_name.Text.Trim
                If Not String.IsNullOrEmpty(dept_name) Then
                    current_selected_department = dept_name
                    ListView1.Enabled = False
                    ListView1.ForeColor = Color.Gray
                    txt_department_name.Focus()
                    btn_EditDepartment.Text = "Save"
                    btn_RemoveDepartment.Text = "Cancel"
                    btn_RemoveDepartment.BackColor = Color.FromArgb(40, 40, 40)
                    txt_department_name.Enabled = True
                    btn_addDepartment.Enabled = False
                    btn_RemoveDepartment.Enabled = True
                    btn_EditDepartment.Enabled = True
                Else

                End If

              
            Case "Save"
                If Not String.IsNullOrEmpty(txt_department_name.Text) Then
                    department_worker.UPDATE_SELECTED_DEPARTMENT(current_selected_department, txt_department_name.Text)
                    ' MessageBox.Show("Save Complete!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)

                End If

                btn_addDepartment.Enabled = True
                btn_EditDepartment.Text = "Edit"
                btn_RemoveDepartment.Text = "Remove"
                btn_RemoveDepartment.BackColor = Color.Red
                ListView1.Enabled = True
                ListView1.ForeColor = Color.Black
                txt_department_name.Text = ""
                txt_department_name.Enabled = False
                LOAD_DEPARTMENT()
        End Select

    End Sub

    Private Sub btn_RemoveDepartment_Click(sender As System.Object, e As System.EventArgs) Handles btn_RemoveDepartment.Click
        Select Case btn_RemoveDepartment.Text
            Case "Remove"
                Dim dp_name As String = ""
                On Error Resume Next
                With ListView1.SelectedItems(0)
                    dp_name = .Text
                    If Not String.IsNullOrEmpty(dp_name) Then
                        Dim emp_count As Integer = 0
                        emp_count = department_worker.COUNT_EMPLOYEE_IN_DEPARTMENT(dp_name)

                        If emp_count > 0 Then
                            'MessageBox.Show("Department name cannot be deleted, there's an employee(s) currently assigned.", "", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                            MSGOK.MSGOK_GLOBAL = "Department name cannot be deleted, there's an employee(s) currently assigned."
                            MSGOK.pbx_msgOK.Image = My.Resources._error
                            MSGOK.ShowDialog()
                            Exit Sub
                        End If

                        GLOBAL_MSGCOM.YesNo_Message = dp_name & "  will be remove in the system. Please confirm."
                        MSGYESNO.ShowDialog()

                        If GLOBAL_MSGCOM.YesNo_Value = "Yes" Then
                            department_worker.REMOVE_SELECTED_DEPARTMENT(dp_name)
                            LOAD_DEPARTMENT()
                        End If
                        End If
                End With
            Case "Cancel"
                ListView1.Enabled = True
                txt_department_name.Text = ""
                btn_RemoveDepartment.Text = "Remove"
                btn_RemoveDepartment.BackColor = Color.Red
                btn_EditDepartment.Text = "Edit"
                txt_department_name.Enabled = False
                btn_addDepartment.Enabled = True
                ListView1.ForeColor = Color.Black
                btn_addDepartment.Text = "New"
                SetCueText(txt_department_name, "")
                current_selected_department = ""
        End Select
    End Sub

    Private Sub txt_department_name_TextChanged(sender As System.Object, e As System.EventArgs) Handles txt_department_name.TextChanged
        'btn_EditDepartment.Enabled = False
        'btn_RemoveDepartment.Enabled = False
        'btn_addDepartment.Enabled = True
    End Sub
End Class