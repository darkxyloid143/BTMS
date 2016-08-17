
Imports System.Drawing.Imaging
Imports System.Data.SqlServerCe
Imports System.Data.SqlClient
Imports System.IO
Imports System.ComponentModel
Imports System.Data


Module AssigningStaffSchedule
    ''Local sdf file
    Public connStrLoc As SqlCeConnection
    Public crdrLoc As SqlCeDataReader
    Public ccmdLoc As SqlCeCommand
    Public cDaLoc As SqlCeDataAdapter
    Public cDataSetLoc As DataSet
    Public cqueryLoc As String

    Public AssignScheduleWorker As New BackgroundWorker


    Public Sub TriggerAssignScheduleWorker(ByVal arg1 As String, ByVal arg2 As String, ByVal arg3 As String, ByVal arg4 As String)
        Dim staffname As String = arg1
        Dim deptName As String = arg2
        Dim dtRangeStart As String = arg3
        Dim dtRangeEnd As String = arg4


        MessageBox.Show(dtRangeStart & "+" & dtRangeEnd)


        AssignScheduleWorker.WorkerReportsProgress = True
        AssignScheduleWorker.WorkerSupportsCancellation = True
        AddHandler AssignScheduleWorker.DoWork, AddressOf AssignScheduleWorkerDoWork
        AddHandler AssignScheduleWorker.ProgressChanged, AddressOf AssignScheduleWorkerProgressChanged
        AddHandler AssignScheduleWorker.RunWorkerCompleted, AddressOf AssignScheduleWorkerCompleted
        ReadyConnection()
        AssignScheduleWorker.RunWorkerAsync(staffname & "+" & deptName & "+" & dtRangeStart & "+" & dtRangeEnd)
    End Sub

#Region "AssignScheduleWoker"
    Private Sub AssignScheduleWorkerDoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs)

        Dim argz As String() = e.Argument.ToString.Split("+")
        Dim staffname As String = argz(0)
        Dim deptName As String = argz(1)
        Dim startDate As Date = argz(2)
        Dim endDate As Date = argz(3)


        ''====================================
        ''Loop all pattern in grpPattern
        ''====================================
        Dim listOfPattern As New List(Of String)

        For Each ix As Control In Form3.grpPattern.Controls
            listOfPattern.Add(ix.Text)
        Next
 

        Dim CurrentDate As Date = Form3.dtpRangeS1.Value.AddDays(1)
        While CurrentDate < Form3.dtpRangeE1.Value
            CurrentDate = CurrentDate.AddDays(1)
        End While



        'Dim CurrentDate As Date = startDate.AddDays(-1)
        'Dim _Cx As Date
        'While CurrentDate < Form3.ED
        '    _Cx = CurrentDate
        '    If listOfPattern.Contains(Format(_Cx, "ddd")) Then
        '        If VerifyScheduleIfExist(GetStaffID(staffname, deptName), CurrentDate) = False Then
        '            Dim staffid As String = GetStaffID(staffname, deptName)
        '            InsertStaffSched(staffname, CurrentDate)
        '        Else
        '            MessageBox.Show("User schedule conflict please review the assigned schedule! " & CurrentDate.ToShortDateString)
        '        End If
        '    End If
        '    CurrentDate = CurrentDate.AddDays(1)
        '    'MessageBox.Show(CurrentDate & "**" & EndDate)
        'End While
    End Sub

    Private Sub AssignScheduleWorkerProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs)

    End Sub
    Private Sub AssignScheduleWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs)
        MessageBox.Show("Complete!")
    End Sub
#End Region






    Public Sub LoadDpt()
        connStrLoc = New SqlCeConnection(My.Settings.LocalConnectionString)
        cqueryLoc = "SELECT * FROM DEPARTMENTDETAILS"
        Try
            Using ccmdLoc = New SqlCeCommand(cqueryLoc, connStrLoc)
                If connStrLoc.State = ConnectionState.Closed Then connStrLoc.Open()
                crdrLoc = ccmdLoc.ExecuteReader()
                While crdrLoc.Read
                    Form3.cmbDepartmentList1.Items.Add(crdrLoc(0))
                End While
                If Not Form3.cmbDepartmentList1.Items.Count = 0 Then
                    Form3.cmbDepartmentList1.SelectedIndex = 0
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("[LoadDepartments()] " & vbCrLf & ex.Message)
        End Try
    End Sub
    Public Sub LoadDepartmentStaffs(ByVal dpname As String)
        cqueryLoc = "SELECT * FROM EmployeeProfiles where Department = '" & dpname.Trim & "'"
        Try
            Using ccmdLoc = New SqlCeCommand(cqueryLoc, connStrLoc)
                If connStrLoc.State = ConnectionState.Closed Then connStrLoc.Open()
                crdrLoc = ccmdLoc.ExecuteReader()
                While crdrLoc.Read
                    Form3.lstboxStafflist.Items.Add(crdrLoc("FULLNAME"))
                End While
            End Using
        Catch ex As Exception
            MessageBox.Show("[LoadDepartmentStaffs] " & vbCrLf & ex.Message)
        End Try
    End Sub

   
    Private Function VerifyScheduleIfExist(ByVal staffID As String, ByVal dt As Date) As Boolean
        Dim res As Boolean = False
        cqueryLoc = "SELECT * FROM ATTENDANCETABLE WHERE STAFFID = '" & staffID & "' AND WORKDATE  = '" & dt.ToShortDateString & "'"
        Try
            Using ccmdLoc = New SqlCeCommand(cqueryLoc, connStrLoc)
                If connStrLoc.State = ConnectionState.Closed Then connStrLoc.Open()
                crdrLoc = ccmdLoc.ExecuteReader()
                If crdrLoc.Read Then
                    res = True
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("[VerifyScheduleIfExist] " & vbCrLf & ex.Message)
        End Try
        Return res
    End Function
    Private Sub InsertStaffSched(ByVal staffid As String, ByVal staffname As String, ByVal dt As Date, ByVal dname As String, ByVal rmrks As String)
        Try
            cqueryLoc = "INSERT INTO ATTENDANCETABLE([STAFFID],[STAFFNAME],[WORKDATE],[WEEK],[REMARKS]) VALUES(@stfid,@stfnme,@wrkDte,@wk,@rmrks)"
            ccmdLoc = New SqlCeCommand(cqueryLoc, connStrLoc)
            If connStrLoc.State = ConnectionState.Closed Then connStrLoc.Open()
            With ccmdLoc
                .Parameters.Add(New SqlCeParameter("@stfid", staffid))
                .Parameters.Add(New SqlCeParameter("@stfnme", staffname))
                .Parameters.Add(New SqlCeParameter("@wrkDte", dt.ToShortDateString))
                .Parameters.Add(New SqlCeParameter("@wk", dname))
                .Parameters.Add(New SqlCeParameter("@rmrks", rmrks))

                .ExecuteNonQuery()
            End With
            ccmdLoc.Dispose()
        Catch ex As Exception
            MessageBox.Show("[InsertStaffSched()]" & vbCrLf & ex.Message)
        End Try
    End Sub
    Private Function GetStaffID(ByVal staffname As String, ByVal deptName As String)
        Dim res As String = String.Empty

        cqueryLoc = "SELECT [EMPID] FROM EmployeeProfiles WHERE FULLNAME = '" & staffname & "' AND DEPARTMENT = '" & deptName & "'"
        Try
            Using ccmdLoc = New SqlCeCommand(cqueryLoc, connStrLoc)
                If connStrLoc.State = ConnectionState.Closed Then connStrLoc.Open()
                crdrLoc = ccmdLoc.ExecuteReader()
                If crdrLoc.Read Then
                    res = crdrLoc("EMPID")
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("[GetStaffID] " & vbCrLf & ex.Message)
        End Try
        Return res
    End Function

    Private Sub ReadyConnection()
        connStrLoc = New SqlCeConnection(My.Settings.LocalConnectionString)
    End Sub
    Public Sub Main(ByVal staffname As String, ByVal deptname As String, ByVal arg3 As String, ByVal arg4 As String)
        ReadyConnection()
        ''====================================
        ''Loop all pattern in grpPattern
        ''====================================
        Dim listOfWorkDay As New List(Of String)
        Dim listOfDayOff As New List(Of String)
        For Each ix As CheckBox In Form3.grpPattern.Controls
            If ix.CheckState = CheckState.Checked Then
                listOfWorkDay.Add(ix.Text.Trim)
            Else
                listOfDayOff.Add(ix.Text.Trim)
            End If
        Next

        Dim CurrentDate As Date = Form3.dtpRangeS1.Value
        Dim _Cx As Date
        Dim _dName As String = String.Empty
        Dim staffId As String = String.Empty
        While CurrentDate < Form3.dtpRangeE1.Value.AddDays(1)
            _Cx = CurrentDate
            _dName = Format(_Cx, "ddd")
            If listOfWorkDay.Contains(_dName) Then
                staffId = GetStaffID(staffname, deptname)
                If VerifyScheduleIfExist(staffId, CurrentDate) = False Then
                    InsertStaffSched(staffId, staffname, CurrentDate, _dName, "")
                Else
                    MessageBox.Show("User schedule conflict please review the assigned schedule! " & CurrentDate.ToShortDateString)
                End If
            Else
                staffId = GetStaffID(staffname, deptname)
                If VerifyScheduleIfExist(staffId, CurrentDate) = False Then
                    InsertStaffSched(staffId, staffname, CurrentDate, _dName, "Off")
                Else
                    MessageBox.Show("User schedule conflict please review the assigned schedule! " & CurrentDate.ToShortDateString)
                End If
            End If
            CurrentDate = CurrentDate.AddDays(1)
        End While
        MessageBox.Show("Done!")
    End Sub


End Module
