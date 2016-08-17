
Module privilege_schema
    '
    'form3.chkbx_privilege.items
    '
    'Add/Edit/Delete Manage Travel Order
    'Add/Edit/Delete BISBIO Devices
    'Add/Edit/Delete Holiday List
    'Add/Edit/Delete Leave Class
    'Add/Edit/Delete Departments & Employees
    'Add/Remove User Access Accounts
    'Calcutating of Reports
    'Download attendance logs
    'Filing of Employee Leaving on Business/Asking for leave
    'Filing of Employee Forgetting to Clock in/out
    'Maintenance Options(BackupDB,Company Logo, DB Path,Clear DB Tables)
    'Employee Schedules(Timetable/Shift Schedules/Assign)
    '
    '



    Public myGlobal_window As Form3


    Public Sub privileges_check(args As String)
        Dim args_arr As New List(Of Char)

        args_arr.AddRange(args.ToCharArray())
        Dim i_ctr As Integer = 0

        For Each ch As String In args_arr
            i_ctr += 1
            Dim i_chk As Integer = i_ctr - 1

            Select Case i_ctr
                Case 1
                    'Add/Edit/Delete Manage Travel Order
                    Console.WriteLine(ch & " = Add/Edit/Delete Manage Travel Order")
                    If ch = "0" Then
                        myGlobal_window.Panel22.Enabled = False
                        myGlobal_window.Panel14.Enabled = False
                        myGlobal_window.chkbx_privilege.SetItemChecked(i_chk, False)
                    Else
                        myGlobal_window.chkbx_privilege.SetItemChecked(i_chk, True)
                    End If
                Case 2
                    'Add/Edit/Delete BISBIO Devices
                    Console.WriteLine(ch & " = Add/Edit/Delete BISBIO Devices")
                    If ch = "0" Then
                        myGlobal_window.pnlx_devmgmt.Visible = False
                        myGlobal_window.chkbx_privilege.SetItemChecked(i_chk, False)
                    Else
                        myGlobal_window.chkbx_privilege.SetItemChecked(i_chk, True)
                    End If
                Case 3
                    'Add/Edit/Delete Holiday List
                    Console.WriteLine(ch & " = Add/Edit/Delete Holiday List")
                    If ch = "0" Then
                        myGlobal_window.Panel23.Enabled = False
                        myGlobal_window.Panel25.Enabled = False
                        myGlobal_window.chkbx_privilege.SetItemChecked(i_chk, False)
                    Else
                        myGlobal_window.chkbx_privilege.SetItemChecked(i_chk, True)
                    End If
                Case 4
                    'Add/Edit/Delete Leave Class
                    Console.WriteLine(ch & " = Add/Edit/Delete Leave Class")
                    If ch = "0" Then
                        myGlobal_window.btnx_olm_showleave_schema.Visible = False
                        myGlobal_window.chkbx_privilege.SetItemChecked(i_chk, False)
                    Else
                        myGlobal_window.chkbx_privilege.SetItemChecked(i_chk, True)
                    End If
                Case 5
                    'Add/Edit/Delete Department and Employees
                    Console.WriteLine(ch & " = Add/Edit/Delete Department and Employees")
                    If ch = "0" Then
                        myGlobal_window.btnSaveEmployee.Visible = False
                        myGlobal_window.btnx_cancelEdit.Visible = False
                        myGlobal_window.btn_AddDepartment.Visible = False
                        myGlobal_window.btnx_resignemp.Visible = False
                        myGlobal_window.btn_EditEmp.Visible = False
                        myGlobal_window.btn_RemoveEmp.Visible = False

                        myGlobal_window.Panel30.Enabled = False
                        myGlobal_window.Panel35.Enabled = False
                        myGlobal_window.Panel36.Enabled = False

                        Add_Edit_Delete_Department_and_Employees = False
                        myGlobal_window.chkbx_privilege.SetItemChecked(i_chk, False)
                    Else
                        myGlobal_window.chkbx_privilege.SetItemChecked(i_chk, True)
                        Add_Edit_Delete_Department_and_Employees = True
                    End If
                Case 6
                    'Add/Remove User Access Accounts
                    Console.WriteLine(ch & " = Add/Remove User Access Accounts")
                    If ch = "0" Then
                        myGlobal_window.btnc_new_user_access.Visible = False
                        myGlobal_window.btnc_remove_user_access.Visible = False
                        myGlobal_window.chkbx_privilege.SetItemChecked(i_chk, False)
                    Else
                        myGlobal_window.chkbx_privilege.SetItemChecked(i_chk, True)
                    End If
                Case 7
                    'Calcutating of Reports
                    Console.WriteLine(ch & " = Calcutating of Reports")
                    If ch = "0" Then
                        myGlobal_window.btn_SaveContinue.Visible = False
                        myGlobal_window.chkbx_privilege.SetItemChecked(i_chk, False)
                    Else
                        myGlobal_window.chkbx_privilege.SetItemChecked(i_chk, True)
                    End If
                Case 8
                    'Download attendance logs
                    Console.WriteLine(ch & " = Download attendance logs")
                    If ch = "0" Then
                        myGlobal_window.lnkx_locate_usb_logs.Visible = False
                        myGlobal_window.Panel27.Visible = False
                        myGlobal_window.chkbx_privilege.SetItemChecked(i_chk, False)
                    Else
                        myGlobal_window.chkbx_privilege.SetItemChecked(i_chk, True)
                    End If
                Case 9
                    'Filing of Employee Leaving on Business/Asking for leave
                    Console.WriteLine(ch & " = Filing of Employee Leaving on Business/Asking for leave")
                    If ch = "0" Then
                        myGlobal_window.Panel9.Enabled = False
                        myGlobal_window.Panel21.Enabled = False
                        myGlobal_window.chkbx_privilege.SetItemChecked(i_chk, False)
                    Else
                        myGlobal_window.chkbx_privilege.SetItemChecked(i_chk, True)
                    End If

                Case 10
                    'Filing of Employee Forgetting to Clock in/out
                    Console.WriteLine(ch & " = Filing of Employee Forgetting to Clock in/out")
                    If ch = "0" Then
                        myGlobal_window.Panel7.Enabled = False
                        myGlobal_window.Panel20.Enabled = False
                        myGlobal_window.chkbx_privilege.SetItemChecked(i_chk, False)
                    Else
                        myGlobal_window.chkbx_privilege.SetItemChecked(i_chk, True)
                    End If

                Case 11
                    'Maintenance Options(BackupDB,Company Logo, DB Path,Clear DB Tables)
                    Console.WriteLine(ch & " = Maintenance Options(BackupDB,Company Logo, DB Path,Clear DB Tables)")
                    If ch = "0" Then
                        myGlobal_window.GroupBox4.Visible = False
                        myGlobal_window.GroupBox2.Visible = False
                        myGlobal_window.btnSystemSettingsSave.Visible = False
                        myGlobal_window.btn_db_backup.Visible = False
                        myGlobal_window.chkbx_privilege.SetItemChecked(i_chk, False)
                    Else
                        myGlobal_window.chkbx_privilege.SetItemChecked(i_chk, True)
                    End If
                Case 12
                    'Employee Schedules(Timetable/Shift Schedules/Assign)
                    Console.WriteLine(ch & " = Employee Schedules(Timetable/Shift Schedules/Assign)")
                    If ch = "0" Then
                        myGlobal_window.btn_ShowShiftSchema.Visible = False
                        myGlobal_window.btnShowShiftTable.Visible = False
                        myGlobal_window.btnAssign1.Visible = False
                        myGlobal_window.PictureBox1.Visible = False
                        myGlobal_window.chkbx_privilege.SetItemChecked(i_chk, False)
                    Else
                        myGlobal_window.chkbx_privilege.SetItemChecked(i_chk, True)
                    End If
            End Select
        Next
    End Sub






End Module
