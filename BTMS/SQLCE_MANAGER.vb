Imports System.IO
Imports System.Data
Imports System.Data.SqlServerCe
Imports System.Data.SqlClient


Public Class SQLCE_MANAGER
    Dim ce_cnn As SqlCeConnection = New SqlCeConnection(My.Settings.LocalConnectionString)
    Dim ce_cmd As SqlCeCommand
    Dim ce_da As SqlCeDataAdapter
    Dim ce_dr As SqlCeDataReader
    Dim ce_ds As DataSet
    Dim ce_builder As SqlCeCommandBuilder
    Dim ce_tbl As DataTable
    Dim ce_query As String = String.Empty
    Dim btms_cpu_worker As New CPU
    Public Function GET_USER_DETAILS(staff_id As String, table_name As String, col_name As String, dt As Date) As List(Of String)
        Dim result As New List(Of String)
        ce_query = "SELECT [" & col_name & "] FROM " & table_name & " WHERE WORKDATE = '" & dt & "'"
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
        While sdr.Read = True
            result.Add(sdr(0))
        End While
        sdr.Close()
        Return result
    End Function




    Public Function GET_USER_ID(staffname As String, dep_id As Integer) As String
        ''[USAGE] Obtain the user id/staffid of the user
        ''[Parameter] staffname, department id 
        ''[Return Value] Return Staff ID if it is successful, or return "NULL"

        Dim result As String = "NULL"

        ce_query = "SELECT [EMPID] FROM EmployeeProfiles WHERE FULLNAME = '" & staffname & "' AND DEPID = '" & dep_id & "'"
        Try
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = sdr(0)
            End If
            sdr.Dispose()
        Catch ex As Exception
            MessageBox.Show("[GET_USER_ID] " & vbCrLf & ex.Message)
        End Try
        Return result
    End Function

    Public Function GET_USER_RAWLOGS(staff_id As String, s_date As Date, e_date As Date) As List(Of String)
        ''[USAGE] Obtain user raw swipe / raw clock in-out
        ''[Parameter] staff id , start date , end date
        ''[Return Value] Return Staff raw logs as list if it is successful, or return empty list.

        Dim result As New List(Of String)

        ce_query = "SELECT RAWID,LOGTIME FROM TIMELOGS WHERE STAFFID = '" & staff_id & "' AND (DATE BETWEEN '" & s_date.ToShortDateString & "' AND '" & e_date.ToShortDateString & "') ORDER BY RAWID"
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()

        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
        'sdr = ce_cmd.ExecuteReader()
        While sdr.Read
            result.Add(sdr("LOGTIME"))
        End While
        sdr.Dispose()
        ce_cmd.Dispose()
        GC.Collect()
        Return result
    End Function

    Public Function GET_USER_RAWLOGS_ALL_CIN(staff_id As String, s_date As Date, e_date As Date) As List(Of String)
        ''[USAGE] Obtain user all CLOCK-IN only raw swipe. 
        ''[Parameter] staff id , start date , end date
        ''[Return Value] Return Staff raw logs as list if it is successful, or return empty list.


        Dim result As New List(Of String)

        ce_query = "SELECT RAWID,LOGTIME FROM TIMELOGS WHERE STAFFID = '" & staff_id & "' AND TRANSACTIONTYPE = 'IN' AND (DATE BETWEEN '" & s_date.ToShortDateString & "' AND '" & e_date.ToShortDateString & "') ORDER BY RAWID"
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()

        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
        'sdr = ce_cmd.ExecuteReader()
        While sdr.Read
            result.Add(sdr("LOGTIME"))
        End While
        sdr.Dispose()
        ce_cmd.Dispose()
        GC.Collect()
        Return result
    End Function

    Public Function GET_CIN(staffid As String, s_date As Date, e_date As Date, begining_in As String, ending_in As String, get_earliest As Boolean) As String
  
        Console.WriteLine("Searching of IN PUNCHES WHERE BEGINING_IN = " & begining_in & " ENDING_IN: " & ending_in & " GET_EARLIEST: " & get_earliest)
        Dim result As String = ""
        Dim rawlogs As New List(Of String)
        ce_query = "SELECT RAWID,LOGTIME FROM TIMELOGS WHERE STAFFID = '" & staffid & "' AND TRANSACTIONTYPE = 'IN' AND (DATE BETWEEN '" & s_date.ToShortDateString & "' AND '" & e_date.ToShortDateString & "') ORDER BY RAWID"
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()

        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
        'sdr = ce_cmd.ExecuteReader()
        While sdr.Read
            rawlogs.Add(Format(CDate(sdr("LOGTIME")), "HH:mm"))
        End While
        sdr.Dispose()
        ce_cmd.Dispose()
        Console.WriteLine("     RAWLOGS FOUND: " & String.Join("+", rawlogs.ToArray))
        Dim listOfRange As New List(Of String)
        listOfRange = btms_cpu_worker.Extractminutes(begining_in, ending_in)
        rawlogs.Distinct()
        'Console.WriteLine("Extracted Range: " & String.Join(",", listOfRange.ToArray))
        Dim raw_candidates As New List(Of String)
        listOfRange.ForEach(Sub(t)
                                If rawlogs.Contains(t) Then
                                    raw_candidates.Add(t)
                                End If
                            End Sub)
        Console.WriteLine("     SELECTED PUNCH: " & String.Join("+", raw_candidates.ToArray))
        If get_earliest = True Then
            If Not raw_candidates.Count = 0 Then
                result = raw_candidates(0)
            End If
        Else
            If Not raw_candidates.Count = 0 Then
                result = raw_candidates(raw_candidates.Count - 1)
            End If
        End If
        'GC.Collect()
        Return result
    End Function
    Public Function GET_CIN_OR_COUT(staffid As String, s_date As Date, e_date As Date, begining_in As String, ending_in As String, get_earliest As Boolean) As String
        Console.WriteLine("Searching of CIN_OR_COUT PUNCHES WHERE START = " & begining_in & " END: " & ending_in & " GET_EARLIEST: " & get_earliest)
        Dim result As String = ""
        Dim rawlogs As New List(Of String)
        ce_query = "SELECT RAWID,LOGTIME FROM TIMELOGS WHERE STAFFID = '" & staffid & "' AND (DATE BETWEEN '" & s_date.ToShortDateString & "' AND '" & e_date.ToShortDateString & "') ORDER BY RAWID"
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()

        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
        'sdr = ce_cmd.ExecuteReader()
        While sdr.Read
            rawlogs.Add(Format(CDate(sdr("LOGTIME")), "HH:mm"))
        End While
        sdr.Dispose()
        ce_cmd.Dispose()
        Console.WriteLine("     RAWLOGS FOUND: " & String.Join("+", rawlogs.ToArray))
        Dim listOfRange As New List(Of String)
        listOfRange = btms_cpu_worker.Extractminutes(begining_in, ending_in)
        rawlogs.Distinct()
        'Console.WriteLine("Extracted Range: " & String.Join(",", listOfRange.ToArray))
        Dim raw_candidates As New List(Of String)
        listOfRange.ForEach(Sub(t)
                                If rawlogs.Contains(t) Then
                                    raw_candidates.Add(t)
                                End If
                            End Sub)
        Console.WriteLine("     SELECTED PUNCH: " & String.Join("+", raw_candidates.ToArray))
        If get_earliest = True Then
            If Not raw_candidates.Count = 0 Then
                result = raw_candidates(0)
            End If
        Else
            If Not raw_candidates.Count = 0 Then
                result = raw_candidates(raw_candidates.Count - 1)
            End If
        End If
        'GC.Collect()
        Return result
    End Function

    Public Function GET_COUT(staffid As String, s_date As Date, e_date As Date, begining_out As String, ending_out As String, get_earliest As Boolean) As String
        Console.WriteLine("Searching of OUT PUNCHES WHERE BEGINING_OUT = " & begining_out & " ENDING_OUT: " & ending_out & " GET_EARLIEST: " & get_earliest)

        Dim result As String = ""
        Dim rawlogs As New List(Of String)
        'Console.WriteLine("Dated: " & s_date & " To " & e_date)
        ce_query = "SELECT RAWID,LOGTIME FROM TIMELOGS WHERE STAFFID = '" & staffid & "' AND TRANSACTIONTYPE = 'OUT' AND (DATE BETWEEN '" & s_date.ToShortDateString & "' AND '" & e_date.ToShortDateString & "') ORDER BY RAWID"
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()

        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
        'sdr = ce_cmd.ExecuteReader()
        While sdr.Read
            rawlogs.Add(Format(CDate(sdr("LOGTIME")), "HH:mm"))
        End While

        sdr.Dispose()
        ce_cmd.Dispose()
        Console.WriteLine("     RAWLOGS FOUND: " & String.Join("+", rawlogs.ToArray))
        Dim listOfRange As New List(Of String)
        Console.WriteLine("     VALID RANGE: " & begining_out & " --> " & ending_out)
        listOfRange = btms_cpu_worker.Extractminutes(begining_out, ending_out)

        rawlogs.Distinct()
        Console.WriteLine("Extracted Range: " & String.Join(",", listOfRange.ToArray))
        Dim raw_candidates As New List(Of String)
        listOfRange.ForEach(Sub(t)
                                If rawlogs.Contains(t) Then
                                    raw_candidates.Add(t)
                                End If
                            End Sub)
        Console.WriteLine("     SELECTED PUNCH: " & String.Join("+", raw_candidates.ToArray))
        If get_earliest = True Then
            If Not raw_candidates.Count = 0 Then
                result = raw_candidates(0)
                Console.WriteLine("get_earliest = " & result)
            End If
        Else
            If Not raw_candidates.Count = 0 Then
                result = raw_candidates(raw_candidates.Count - 1)
                Console.WriteLine("raw_candidate = " & result)
            End If
        End If
        'GC.Collect()
        Return result
    End Function

    Public Function GET_USER_RAWLOGS_SPECIFIC(staff_id As String, s_date As Date, e_date As Date, CIN As Boolean, COUT As Boolean) As List(Of String)
        Dim result As New List(Of String)
        Dim TRANSTYPE As String = ""
        If CIN = True Then
            TRANSTYPE = "IN"
        ElseIf COUT = True Then
            TRANSTYPE = "OUT"
        End If

        ce_query = "SELECT RAWID,LOGTIME FROM TIMELOGS WHERE STAFFID = '" & staff_id & "' AND TRANSACTIONTYPE = '" & TRANSTYPE & "' (DATE BETWEEN '" & s_date.ToShortDateString & "' AND '" & e_date.ToShortDateString & "') ORDER BY RAWID"
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()

        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
        'sdr = ce_cmd.ExecuteReader()
        While sdr.Read
            result.Add(sdr("LOGTIME"))
        End While
        sdr.Dispose()
        ce_cmd.Dispose()
        GC.Collect()
        Return result
    End Function

    Public Function GET_USER_ID_LIST(tablename As String) As List(Of String)
        Dim result As New List(Of String)
        'SELECT DISTINCT [Department] FROM [EmployeeProfiles]
        ce_query = "SELECT DISTINCT [STAFFID] FROM " & tablename & " ORDER BY [STAFFID] DESC"
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
        While sdr.Read
            result.Add(sdr(0))
        End While
        sdr.Close()
        Return result
    End Function

    Function CALCULATION_DONE(day As Date, staffid As String) As Boolean
        Dim result As Boolean = False

        'SELECT        STAFFID, DATE, AMCIN, AMCOUT, PMCIN, PMCOUT
        'FROM            DTR
        'WHERE        (STAFFID = '898989') AND (DATE = '2/2/2016')

        ce_query = "SELECT STAFFID, DATE, AMCIN, AMCOUT, PMCIN, PMCOUT FROM DTR WHERE STAFFID = '" & staffid & "' and DATE = '" & day & "'"
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
        If sdr.Read Then
            Return True
        End If

        sdr.Close()


        Return result
    End Function


   



#Region "ASSING STAFF SCHEDULE"

    Public Sub START_ASSIGNING(staff_id As String, dept_id As Integer, startdate As Date, enddate As Date,
                               listOfWorkingDay As List(Of String), listOfDayOff As List(Of String), shift_id As Integer, shift_name As String)
        ''====================================
        ''Loop all pattern in grpPattern
        ''====================================
        'MessageBox.Show(shiftname)

        'Dim CurrentDate As Date = startdate.ToShortDateString
        'Dim _Cx As Date
        'Dim _dName As String = String.Empty
        'While CurrentDate < enddate.AddDays(1)
        '    _Cx = CurrentDate
        '    _dName = Format(_Cx, "ddd")
        '    If listOfWorkingDay.Contains(_dName) Then
        '        'staffId = GET_USER_ID(staffname, dept_id)
        '        If VerifyScheduleIfExist(staff_id, CurrentDate) = False Then
        '            _dName = Format(_Cx, "dd") & " " & Format(_Cx, "ddd")
        '            InsertStaffSched(staff_id, "", CurrentDate, _dName, "", shift_name, shift_id)
        '        Else
        '            'MessageBox.Show("User schedule conflict please review the assigned schedule! " & CurrentDate.ToShortDateString)
        '            ''Prompt conflict Update or not to update
        '            'MessageBox.Show("User schedule conflict please review the assigned schedule! " & CurrentDate.ToShortDateString)

        '            'If MessageBox.Show("Schedule conflict do you want to update?" & vbCrLf & GET_STAFF_NAME(staff_id) & vbCrLf & "Date: " & CurrentDate, "", MessageBoxButtons.A, MessageBoxIcon.Question) = DialogResult.Yes Then
        '            UpdateStaffSched(staff_id, "", CurrentDate, _dName, "", shift_name, shift_id)
        '            'Else
        '            ''skip conflict schedule proceed next
        '        End If

        '    End If
        '    'End If

        '    If listOfDayOff.Contains(_dName) Then
        '        'staffId = GET_USER_ID(staffname, deptname)
        '        If VerifyScheduleIfExist(staff_id, CurrentDate) = False Then
        '            _dName = Format(_Cx, "dd") & " " & Format(_Cx, "ddd")
        '            InsertStaffSched(staff_id, "", CurrentDate, _dName, "Off", "Off", 0)
        '        Else
        '            ''Prompt conflict Update or not to update
        '            'MessageBox.Show("User schedule conflict please review the assigned schedule! " & CurrentDate.ToShortDateString)

        '            ' If MessageBox.Show("Schedule conflict do you want to update?" & vbCrLf & GET_STAFF_NAME(staff_id) & vbCrLf & "Date: " & CurrentDate, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
        '            UpdateStaffSched(staff_id, "", CurrentDate, _dName, "Off", "Off", 0)
        '            'Else
        '            '    ''skip conflict schedule proceed next
        '            'End If

        '        End If
        '        End If
        '    CurrentDate = CurrentDate.AddDays(1)
        'End While
    End Sub
    Public Function VERIFY_SCHEDULE_EXIST(staffID As String, dt As Date) As Boolean
        Dim result As Boolean = False
        ce_query = "SELECT * FROM ATTENDANCETABLE WHERE STAFFID = '" & staffID & "' AND WORKDATE  = '" & dt.ToShortDateString & "'"
        Try
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = True
            End If
            'GC.Collect()
            'ce_cmd.Dispose()
        Catch ex As Exception
            MessageBox.Show("[VERIFY_SCHEDULE_EXIST] " & vbCrLf & ex.Message)
        End Try
        Return result
    End Function
    Public Sub INSERT_STAFF_SCHEDULE(staffid As String, staffname As String, dt As Date, dname As String, rmrks As String, shiftname As String, shift_id As Integer, deptname As String)
        ' MessageBox.Show(shiftname)
        Try
            ce_query = "INSERT INTO ATTENDANCETABLE([STAFFID],[STAFFNAME],[WORKDATE],[WEEK],[DTRREMARKS],[MONTHLYREMARKS],[SHIFTNAME],[SHIFTID],[DEPTNAME]) VALUES(@stfid,@stfnme,@wrkDte,@wk,@dtrrmrks,@mnthlyrmrks,@shftnme,@shftid,@deptname)"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            With ce_cmd
                .Parameters.Add(New SqlCeParameter("@stfid", staffid))
                .Parameters.Add(New SqlCeParameter("@stfnme", staffname))
                .Parameters.Add(New SqlCeParameter("@wrkDte", dt.ToShortDateString))
                .Parameters.Add(New SqlCeParameter("@wk", dname))
                .Parameters.Add(New SqlCeParameter("@dtrrmrks", rmrks)) '@mnthlyrmrks
                .Parameters.Add(New SqlCeParameter("@mnthlyrmrks", rmrks)) '@mnthlyrmrks
                .Parameters.Add(New SqlCeParameter("@shftnme", shiftname)) '@mnthlyrmrks
                .Parameters.Add(New SqlCeParameter("@shftid", shift_id)) '@mnthlyrmrks
                .Parameters.Add(New SqlCeParameter("@deptname", deptname)) '@mnthlyrmrks
                .ExecuteNonQuery()
            End With
            'ce_cmd.Dispose()
        Catch ex As Exception
            MessageBox.Show("[INSERT_STAFF_SCHEDULE()]" & vbCrLf & ex.Message)
        End Try
    End Sub
    Public Sub DELETE_STAFF_SCHEDULE(staffid As String, dt As Date)
        ' MessageBox.Show(shiftname)
        Try
            ce_query = "DELETE FROM ATTENDANCETABLE WHERE STAFFID = '" & staffid & "' AND WORKDATE = '" & dt.ToShortDateString & "'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            ce_cmd.ExecuteNonQuery()

            'ce_cmd.Dispose()
        Catch ex As Exception
            MessageBox.Show("[DELETE_STAFF_SCHEDULE()]" & vbCrLf & ex.Message)
        End Try
    End Sub
    Public Sub UPDATE_STAFF_SCHEDULE(staffid As String, staffname As String, dt As Date, dname As String, rmrks As String, shiftname As String, shift_id As Integer)
        ' MessageBox.Show(shiftname)
        Try

            ce_query = "UPDATE ATTENDANCETABLE SET SHIFTNAME = @shftnme, DTRREMARKS = @dtrrmrks, SHIFTID = @shftid WHERE STAFFID = @stfid AND WORKDATE = @wrkdate"
            ' cqueryLoc = "INSERT INTO ATTENDANCETABLE([STAFFID],[STAFFNAME],[WORKDATE],[WEEK],[DTRREMARKS],[MONTHLYREMARKS],[SHIFTNAME]) VALUES(@stfid,@stfnme,@wrkDte,@wk,@dtrrmrks,@mnthlyrmrks,@shftnme)"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            With ce_cmd
                .Parameters.Add(New SqlCeParameter("@stfid", staffid))
                .Parameters.Add(New SqlCeParameter("@wrkdate", dt.ToShortDateString))
                .Parameters.Add(New SqlCeParameter("@dtrrmrks", rmrks)) '@mnthlyrmrks
                .Parameters.Add(New SqlCeParameter("@mnthlyrmrks", rmrks)) '@mnthlyrmrks
                .Parameters.Add(New SqlCeParameter("@shftnme", shiftname)) '@mnthlyrmrks
                .Parameters.Add(New SqlCeParameter("@shftid", shift_id)) '@mnthlyrmrks
                .ExecuteNonQuery()
            End With
            'ce_cmd.Dispose()
        Catch ex As Exception
            MessageBox.Show("[UpdateStaffSched()]" & vbCrLf & ex.Message)
        End Try
    End Sub


    Public Function CHECK_STAFF_SCHEDULE(staff_id As String, _day As Date) As Boolean
        Dim result As Boolean = False
        ce_query = "SELECT * FROM ATTENDANCETABLE WHERE STAFFID = '" & staff_id & "' AND WORKDATE  = '" & _day.ToShortDateString & "'"
        Try
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = True
            End If
            'ce_cmd.Dispose()
        Catch ex As Exception
            MessageBox.Show("[CHECK_STAFF_SCHEDULE] " & vbCrLf & ex.Message)
        End Try
        Return result
    End Function

    Public Function GET_STAFF_SCHEDULENAME(staff_id As String, _day As Date) As String
        Dim result As String = Nothing
        ce_query = "SELECT SHIFTNAME FROM ATTENDANCETABLE WHERE STAFFID = '" & staff_id & "' AND WORKDATE  = '" & _day.ToShortDateString & "'"
        Try
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = sdr(0).ToString
            End If
            'ce_cmd.Dispose()
        Catch ex As Exception
            MessageBox.Show("[GET_STAFF_SCHEDULENAME] " & vbCrLf & ex.Message)
        End Try
        Return result
    End Function
    Public Function GET_STAFF_SHIFT_ID(staff_id As String, _day As Date) As Integer
        Dim result As String = Nothing
        ce_query = "SELECT SHIFTID FROM ATTENDANCETABLE WHERE STAFFID = '" & staff_id & "' AND WORKDATE  = '" & _day.ToShortDateString & "'"
        Try
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = sdr(0).ToString
            End If
            'ce_cmd.Dispose()
        Catch ex As Exception
            MessageBox.Show("[GET_STAFF_SCHEDULE_ID] " & vbCrLf & ex.Message)
        End Try
        Return result
    End Function

    Public Function GET_STAFF_SHIFT_NAME(staff_id As String, _day As Date) As String
        Dim result As String = ""
        ce_query = "SELECT SHIFTNAME FROM ATTENDANCETABLE WHERE STAFFID = '" & staff_id & "' AND WORKDATE  = '" & _day.ToShortDateString & "'"
        Try
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = sdr(0).ToString
            End If
            'ce_cmd.Dispose()
        Catch ex As Exception
            MessageBox.Show("[GET_STAFF_SHIFT_NAME] " & vbCrLf & ex.Message)
        End Try
        Return result
    End Function



    Public Function CHECK_FLEXIBLE_SHIFT(shiftid As Integer) As Boolean
        Dim result As Boolean = False
        ce_query = "SELECT * FROM SHIFTTABLE WHERE SHIFTID = " & shiftid & " AND FLEXIBLE = '1'"
        Try
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = True
            End If
        Catch ex As Exception
            MessageBox.Show("[CHECK_FLEXIBLE_SHIFT] " & vbCrLf & ex.Message)
        End Try



        Return result
    End Function




#End Region






#Region "DTR TABLE POLLING AND UPDATING"

    Public Sub INSERT_INTO_DTR_TABLE(staff_id As String, staff_name As String, department_name As String, _day As Date,
                                     amCIN As String, amCOUT As String,
                                     pmCIN As String, pmCOUT As String,
                                     week As String, dtr_remarks As String, mothly_remarks As String,
                                     totalUndertime As Integer, undertimehh As String, undertimemm As String,
                                     total_undertime_daily As String, total_ot_daily As Integer, total_ot_daily_remarks As String,
                                     official_arrival As String, official_departure As String,
                                     daily_ut As String, daily_late As String, daily_remarks As String,
                                     daily_late_total As Integer, daily_ut_total As Integer,
                                     monthly_late_total As String, monthly_ut_total As String, designation As String)

        ''PROCESS FIRST THE DOUBLE TYPE FOR UNDERTIME MINUTES INSTRUCT CODE DO NOT ROUND OFF REMOVE THE FLOATING POINT
        Dim ActualTotalUnderTimeinMinutes As String() = total_undertime_daily.Split(".")
        If Not ActualTotalUnderTimeinMinutes(0) = "" Then
            total_undertime_daily = ActualTotalUnderTimeinMinutes(0)
        End If


        ce_query = "INSERT INTO DTR(STAFFID, STAFFNAME, DEPARTMENT, DATE, AMCIN, AMCOUT, PMCIN, PMCOUT,WEEK,UNDERHHREMARKS,UNDERMMREMARKS,UNDERTIMEDAILY,MONTHLYREMARKS,OTTOTALDAILY,OTTOTALDAILYREMARKS,OFFICIALARRIVAL,OFFICIALDEPARTURE,COL_UT,COL_LATE,COL_DAILY_REMARKS,COL_LATE_TOTAL,COL_UT_TOTAL,COL_LATE_TOTAL_MONTHLY,COL_UT_TOTAL_MONTHLY, DESIGNATION) VALUES(@stffid, @stffnme, @dptnme, @dy, @acin, @acout, @pcin, @pcout,@wk,@undrhhrmrks,@undrmmrmrks,@undtmedaily,@monthlyrmrks,@totalotdaily,@totalotdailyremarks,@official_arrival,@official_departure,@daily_ut,@daily_late,@daily_remarks,@col_late_total,@col_ut_total,@col_late_total_monthly,@col_ut_total_monthly, @indesignation)"

        Try
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            With ce_cmd
                .Parameters.Add(New SqlCeParameter("@stffid", staff_id))
                .Parameters.Add(New SqlCeParameter("@stffnme", staff_name))
                .Parameters.Add(New SqlCeParameter("@dptnme", department_name))
                .Parameters.Add(New SqlCeParameter("@dy", _day.ToShortDateString))
                .Parameters.Add(New SqlCeParameter("@acin", amCIN))
                .Parameters.Add(New SqlCeParameter("@acout", amCOUT))
                .Parameters.Add(New SqlCeParameter("@pcin", pmCIN))
                .Parameters.Add(New SqlCeParameter("@pcout", pmCOUT))
                .Parameters.Add(New SqlCeParameter("@wk", week))
                .Parameters.Add(New SqlCeParameter("@undrhhrmrks", undertimehh))
                .Parameters.Add(New SqlCeParameter("@undrmmrmrks", undertimemm))
                .Parameters.Add(New SqlCeParameter("@undtmedaily", CInt(total_undertime_daily)))
                .Parameters.Add(New SqlCeParameter("@monthlyrmrks", mothly_remarks))
                .Parameters.Add(New SqlCeParameter("@totalotdaily", total_ot_daily))
                .Parameters.Add(New SqlCeParameter("@totalotdailyremarks", total_ot_daily_remarks))
                .Parameters.Add(New SqlCeParameter("@official_arrival", official_arrival))
                .Parameters.Add(New SqlCeParameter("@official_departure", official_departure))
                .Parameters.Add(New SqlCeParameter("@daily_ut", daily_ut))
                .Parameters.Add(New SqlCeParameter("@daily_late", daily_late))
                .Parameters.Add(New SqlCeParameter("@daily_remarks", daily_remarks))
                .Parameters.Add(New SqlCeParameter("@col_late_total", daily_late_total))
                .Parameters.Add(New SqlCeParameter("@col_ut_total", daily_ut_total))
                .Parameters.Add(New SqlCeParameter("@col_late_total_monthly", monthly_late_total))
                .Parameters.Add(New SqlCeParameter("@col_ut_total_monthly", monthly_ut_total))
                .Parameters.Add(New SqlCeParameter("@indesignation", designation))


                .ExecuteNonQuery()

            End With
            'ce_cmd.Dispose()
        Catch ex As Exception
            MessageBox.Show("[INSERT_INTO_DTR_TABLE()]" & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Sub UPDATE_INTO_DTR_TABLE_TOTAL_UNDERTIME_MONTHLY(staff_id As String, _day As Date,
                                       totalUndertime_monthly As String, total_as_minutes As Integer,
                                       late_total_monthly As String, ut_total_monthly As String)

        ce_query = "UPDATE DTR SET UNDERTIMETOTALMONTHLY = @totalundrtme, UNDERTIMETOTALMONTHLYASMINUTES = @ttlmin , COL_LATE_TOTAL_MONTHLY = @late_total, COL_UT_TOTAL_MONTHLY = @ut_total WHERE STAFFID = @stfid AND DATE = @dt"
        Try

            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            With ce_cmd
                .Parameters.Add(New SqlCeParameter("@stfid", staff_id))
                .Parameters.Add(New SqlCeParameter("@dt", _day.ToShortDateString))
                .Parameters.Add(New SqlCeParameter("@ttlmin", total_as_minutes))
                .Parameters.Add(New SqlCeParameter("@totalundrtme", totalUndertime_monthly)) '@mnthlyrmrks
                .Parameters.Add(New SqlCeParameter("@late_total", late_total_monthly)) '@mnthlyrmrks
                .Parameters.Add(New SqlCeParameter("@ut_total", ut_total_monthly)) '@mnthlyrmrks

                .ExecuteNonQuery()
            End With
            'ce_cmd.Dispose()
        Catch ex As Exception
            MessageBox.Show("[UPDATE_INTO_DTR_TABLE_MONTHLYTOTAL_UNDERTTIME()]" & vbCrLf & ex.Message)
        End Try
    End Sub
    Public Function SUM_ALL_UNDERTIME_DAILY_IN_DTR_TABLE(staff_id As String, s_date As Date, e_date As Date) As Integer
        Dim result As Integer = 0
        ce_query = "SELECT SUM(UNDERTIMEDAILY) AS TOTAL FROM DTR WHERE STAFFID = '" & staff_id & "' AND (DATE BETWEEN '" & s_date.ToShortDateString & "' AND '" & e_date.ToShortDateString & "')"
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
        'If sdr.Read Then
        '    result = sdr("TOTAL")
        'End If
        If sdr.Read Then
            If Not sdr.IsDBNull(sdr.GetOrdinal("TOTAL")) Then
                result = sdr(0)
            End If
        End If
        sdr.Close()
        Return result
    End Function

    Public Function SUM_LATE_DAILY(staff_id As String, s_date As Date, e_date As Date) As Integer
        Dim result As Integer = 0
        ce_query = "SELECT SUM(COL_LATE_TOTAL) AS TOTAL FROM DTR WHERE STAFFID = '" & staff_id & "' AND (DATE BETWEEN '" & s_date.ToShortDateString & "' AND '" & e_date.ToShortDateString & "')"
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
        'If sdr.Read Then
        '    result = sdr("TOTAL")
        'End If
        If sdr.Read Then
            If Not sdr.IsDBNull(sdr.GetOrdinal("TOTAL")) Then
                result = sdr(0)
            End If
        End If
        sdr.Close()
        Return result
    End Function

    Public Function SUM_UT_DAILY(staff_id As String, s_date As Date, e_date As Date) As Integer
        Dim result As Integer = 0
        ce_query = "SELECT SUM(COL_UT_TOTAL) AS TOTAL FROM DTR WHERE STAFFID = '" & staff_id & "' AND (DATE BETWEEN '" & s_date.ToShortDateString & "' AND '" & e_date.ToShortDateString & "')"
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
        'If sdr.Read Then
        '    result = sdr("TOTAL")
        'End If
        If sdr.Read Then
            If Not sdr.IsDBNull(sdr.GetOrdinal("TOTAL")) Then
                result = sdr(0)
            End If
        End If
        sdr.Close()
        Return result
    End Function



    Public Sub CLEAN_DTR_TABLE()
        Try
            ce_query = "DELETE FROM DTR"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            ce_cmd.ExecuteNonQuery()
            'ce_cmd.Dispose()
        Catch ex As Exception
            MessageBox.Show("[CLEAN_DTR_TABLE()]" & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Function GET_LIST_OF_STAFFNAME_IN_DTRTABLE() As List(Of String)
        Dim result As New List(Of String)
        ce_query = "SELECT DISTINCT STAFFNAME FROM DTR"
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
        While sdr.Read
            result.Add(sdr(0))
        End While
        sdr.Close()
        Return result
    End Function

    Public Function GET_MONTHLY_REMARKS_IN_DTRTABLE(staffname As String) As List(Of String)
        Dim result As New List(Of String)
        Try
            ce_query = "SELECT MONTHLYREMARKS FROM DTR WHERE STAFFNAME = '" & staffname & "' ORDER BY DATE ASC"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            While sdr.Read
                result.Add(sdr(0).ToString)
            End While
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_MONTHLY_REMARKS_IN_DTRTABLE()]", ex.Message)
        End Try

        Return result
    End Function

    Public Function GET_EXCEPTION_COUNT(staff_name As String, exception_name As String, s_date As Date, e_date As Date) As Integer
        Dim result As Integer = 0
        ce_query = "SELECT COUNT(*) FROM DTR WHERE([STAFFNAME] = '" & staff_name & "') AND ([MONTHLYREMARKS] = '" & exception_name & "') AND (DATE BETWEEN '" & s_date.ToShortDateString & "' AND '" & e_date.ToShortDateString & "')"
        ' "SELECT COUNT(*) AS TOTAL FROM DTR WHERE (STAFFID = '5566') AND (MONTHLYREMARKS = 'A')"

        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
        If sdr.Read Then
            result = sdr(0)
        Else
            result = 0
        End If
        sdr.Close()
        Return result
    End Function

    Public Function GET_TOTALMINUTES_UNDERTIMEMONTHLY(staff_name As String, s_date As Date, e_date As Date) As Integer
        Dim result As Integer = 0
        ce_query = "SELECT UNDERTIMETOTALMONTHLYASMINUTES AS EXPRES1 FROM DTR WHERE([STAFFNAME] = '" & staff_name & "') AND (DATE BETWEEN '" & s_date.ToShortDateString & "' AND '" & e_date.ToShortDateString & "')"
        ' "SELECT COUNT(*) AS TOTAL FROM DTR WHERE (STAFFID = '5566') AND (MONTHLYREMARKS = 'A')"

        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()

        ' If sdr.Read Then
        '            If Not sdr.IsDBNull(sdr.GetOrdinal("MM")) Then

        While sdr.Read
            If Not sdr.IsDBNull(sdr.GetOrdinal("EXPRES1")) Then
                result = sdr(0)
            End If
        End While
        sdr.Close()
        Return result
    End Function
    Public Function LOAD_EMPLOYEE_PROFILES() As DataTable
        Dim result As DataTable
        'connStr = New SqlCeConnection(My.Settings.LocalConnectionString)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        'ce_query = "SELECT * FROM EMPLOYEEPROFILES"
        'sCommand = New SqlCeCommand(sql, connStr)
        ce_query = "SELECT EmployeeProfiles.EmpID, EmployeeProfiles.FullName, DEPARTMENTTABLE.DEPARTMENTNAME FROM EmployeeProfiles INNER JOIN DEPARTMENTTABLE ON EmployeeProfiles.DEPID = DEPARTMENTTABLE.DEPARTMENTNAME"
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        ce_da = New SqlCeDataAdapter(ce_cmd)
        ce_builder = New SqlCeCommandBuilder(ce_da)
        ce_ds = New DataSet()
        ce_da.Fill(ce_ds, "EMPLOYEEPROFILES")
        ce_tbl = ce_ds.Tables("EMPLOYEEPROFILES")
        ce_cnn.Close()
        result = ce_ds.Tables("EMPLOYEEPROFILES")
        Return result
    End Function


    Public Function LOAD_DTR_TABLE() As DataTable
        Dim result As DataTable

        'connStr = New SqlCeConnection(My.Settings.LocalConnectionString)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        ce_query = "SELECT * FROM DTR"
        'sCommand = New SqlCeCommand(sql, connStr)
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        ce_da = New SqlCeDataAdapter(ce_cmd)
        ce_builder = New SqlCeCommandBuilder(ce_da)
        ce_ds = New DataSet()
        ce_da.Fill(ce_ds, "DTR")
        ce_tbl = ce_ds.Tables("DTR")
        ce_cnn.Close()
        result = ce_ds.Tables("DTR")
        Return result
    End Function

#End Region




#Region "SR OVERTIME POLLING AND UPDATING"

    Public Sub INSERT_STAFFNAME_CTR_INTO_SROTTABLE(staff_name As String, item_id As String)
        ce_query = "INSERT INTO SROT(ITEMID, NAME) VALUES(@i_id, @staffnme)"

        Try
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            With ce_cmd
                .Parameters.Add(New SqlCeParameter("@i_id", item_id))
                .Parameters.Add(New SqlCeParameter("@staffnme", staff_name))
                .ExecuteNonQuery()
            End With
        Catch ex As Exception
            MessageBox.Show("[INSERT_STAFFNAME_CTR_INTO_SROTTABLE()]" & vbCrLf & ex.Message)
        End Try

    End Sub
    Public Sub CLEAN_SROTABLE()
        Try
            ce_query = "DELETE FROM SROT"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            ce_cmd.ExecuteNonQuery()
            'ce_cmd.Dispose()
        Catch ex As Exception
            MessageBox.Show("[CLEAN_SROTABLE()]" & vbCrLf & ex.Message)
        End Try
    End Sub
    Public Function GET_OT_DAILY_REMARKS(staffname As String) As List(Of String)
        Dim result As New List(Of String)
        Try
            ce_query = "SELECT OTTOTALDAILYREMARKS FROM DTR WHERE STAFFNAME = '" & staffname & "' ORDER BY DATE ASC"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            While sdr.Read
                result.Add(sdr(0).ToString)
            End While
        Catch ex As Exception
            MessageBox.Show("[GET_OT_MONTHLY_REMARKS()] " & ex.Message)
        End Try


        Return result
    End Function


    Public Sub UPDATE_SROTBLE(staff_name As String, list_of_remarks As List(Of String), HH As String, MM As String)
        ''COMPLETE THE LIST OF REMARKS IF THE USER GENERATED DAYS LESS THAN 31 DAYS
        If list_of_remarks.Count < 31 Then
            Dim totalcount As Integer = list_of_remarks.Count
            For i = totalcount To 31
                list_of_remarks.Add("")
            Next
        End If


        Try

            ce_query = "UPDATE SROT SET [01] = @day1, [02] = @day2, [03] = @day3, [04] = @day4, [05] = @day5, [06] = @day6, [07] = @day7, [08] = @day8, [09] = @day9, [10] = @day10, [11] = @day11, [12] = @day12, [13] = @day13, [14] = @day14, [15] = @day15, [16] = @day16, [17] = @day17,[18] = @day18, [19] = @day19, [20] = @day20, [21] = @day21, [22] = @day22, [23] = @day23, [24] = @day24, [25] = @day25, [26] = @day26,  [27] = @day27, [28] = @day28, [29] = @day29, [30] = @day30, [31] = @day31, [HH] = @ihh, [MM] = @imm WHERE NAME = @staffnme"
            ' cqueryLoc = "INSERT INTO ATTENDANCETABLE([STAFFID],[STAFFNAME],[WORKDATE],[WEEK],[DTRREMARKS],[MONTHLYREMARKS],[SHIFTNAME]) VALUES(@stfid,@stfnme,@wrkDte,@wk,@dtrrmrks,@mnthlyrmrks,@shftnme)"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            With ce_cmd
                .Parameters.Add(New SqlCeParameter("@staffnme", staff_name))
                .Parameters.Add(New SqlCeParameter("@day1", list_of_remarks(0).ToString))
                .Parameters.Add(New SqlCeParameter("@day2", list_of_remarks(1).ToString))
                .Parameters.Add(New SqlCeParameter("@day3", list_of_remarks(2).ToString))
                .Parameters.Add(New SqlCeParameter("@day4", list_of_remarks(3).ToString))
                .Parameters.Add(New SqlCeParameter("@day5", list_of_remarks(4).ToString))
                .Parameters.Add(New SqlCeParameter("@day6", list_of_remarks(5).ToString))
                .Parameters.Add(New SqlCeParameter("@day7", list_of_remarks(6).ToString))
                .Parameters.Add(New SqlCeParameter("@day8", list_of_remarks(7).ToString))
                .Parameters.Add(New SqlCeParameter("@day9", list_of_remarks(8).ToString))
                .Parameters.Add(New SqlCeParameter("@day10", list_of_remarks(9).ToString))
                .Parameters.Add(New SqlCeParameter("@day11", list_of_remarks(10).ToString))
                .Parameters.Add(New SqlCeParameter("@day12", list_of_remarks(11).ToString))
                .Parameters.Add(New SqlCeParameter("@day13", list_of_remarks(12).ToString))
                .Parameters.Add(New SqlCeParameter("@day14", list_of_remarks(13).ToString))
                .Parameters.Add(New SqlCeParameter("@day15", list_of_remarks(14).ToString))
                .Parameters.Add(New SqlCeParameter("@day16", list_of_remarks(15).ToString))
                .Parameters.Add(New SqlCeParameter("@day17", list_of_remarks(16).ToString))
                .Parameters.Add(New SqlCeParameter("@day18", list_of_remarks(17).ToString))
                .Parameters.Add(New SqlCeParameter("@day19", list_of_remarks(18).ToString))
                .Parameters.Add(New SqlCeParameter("@day20", list_of_remarks(19).ToString))
                .Parameters.Add(New SqlCeParameter("@day21", list_of_remarks(20).ToString))
                .Parameters.Add(New SqlCeParameter("@day22", list_of_remarks(21).ToString))
                .Parameters.Add(New SqlCeParameter("@day23", list_of_remarks(22).ToString))
                .Parameters.Add(New SqlCeParameter("@day24", list_of_remarks(23).ToString))
                .Parameters.Add(New SqlCeParameter("@day25", list_of_remarks(24).ToString))
                .Parameters.Add(New SqlCeParameter("@day26", list_of_remarks(25).ToString))
                .Parameters.Add(New SqlCeParameter("@day27", list_of_remarks(26).ToString))
                .Parameters.Add(New SqlCeParameter("@day28", list_of_remarks(27).ToString))
                .Parameters.Add(New SqlCeParameter("@day29", list_of_remarks(28).ToString))
                .Parameters.Add(New SqlCeParameter("@day30", list_of_remarks(29).ToString))
                .Parameters.Add(New SqlCeParameter("@day31", list_of_remarks(30).ToString))

                .Parameters.Add(New SqlCeParameter("@ihh", HH))
                .Parameters.Add(New SqlCeParameter("@imm", MM))


                .ExecuteNonQuery()
            End With

        Catch ex As Exception
            MessageBox.Show("[UPDATE_SROTBLE()]" & vbCrLf & ex.Message)
        End Try
    End Sub


    Public Function GET_TOTALMINUTES_OVERTIME(staff_name As String, s_date As Date, e_date As Date) As Integer

        Dim result As Integer = 0
        Try
            ce_query = "SELECT SUM(OTTOTALDAILY) AS EXPRES1 FROM DTR WHERE([STAFFNAME] = '" & staff_name & "') AND (DATE BETWEEN '" & s_date.ToShortDateString & "' AND '" & e_date.ToShortDateString & "')"
            ' "SELECT COUNT(*) AS TOTAL FROM DTR WHERE (STAFFID = '5566') AND (MONTHLYREMARKS = 'A')"

            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()

            While sdr.Read
                If Not sdr.IsDBNull(sdr.GetOrdinal("EXPRES1")) Then
                    result = sdr(0)
                End If
            End While
        Catch ex As Exception
            MessageBox.Show("[GET_TOTALMINUTES_OVERTIME()] " & ex.Message)
        End Try
        Console.WriteLine("TOTAL OT SUM: " & result)
  
        Return result
    End Function



#End Region














#Region "SRA TABLE POLLING AND UPDATING"

    Public Sub CLEAN_SRATABLE()
        Try
            ce_query = "DELETE FROM SRATABLE"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            ce_cmd.ExecuteNonQuery()
            'ce_cmd.Dispose()
        Catch ex As Exception
            MessageBox.Show("[CLEAN_SRATABLE()]" & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Sub INSERT_STAFFNAME_CTR_INTO_SRATABLE(staff_name As String, item_id As String)
        ce_query = "INSERT INTO SRATABLE(ITEMID, NAME) VALUES(@i_id, @staffnme)"

        Try
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            With ce_cmd
                .Parameters.Add(New SqlCeParameter("@i_id", item_id))
                .Parameters.Add(New SqlCeParameter("@staffnme", staff_name))
                .ExecuteNonQuery()
            End With
        Catch ex As Exception
            MessageBox.Show("[INSERT_STAFFNAME_CTR_INTO_SRATABLE()]" & vbCrLf & ex.Message)
        End Try

    End Sub

    Public Sub UPDATE_SRATABLE(staff_name As String, list_of_remarks As List(Of String),
                               MC6 As Integer, FL As Integer, MLA As Integer, PATL As Integer,
                               VL As Integer, SL As Integer, A As Integer, HH As String, MM As String)




        ''COMPLETE THE LIST OF REMARKS IF THE USER GENERATED DAYS LESS THAN 31 DAYS
        If list_of_remarks.Count < 31 Then
            Dim totalcount As Integer = list_of_remarks.Count
            For i = totalcount To 31
                list_of_remarks.Add("")
            Next
        End If


        Try

            ce_query = "UPDATE SRATABLE SET [01] = @day1, [02] = @day2, [03] = @day3, [04] = @day4, [05] = @day5, [06] = @day6, [07] = @day7, [08] = @day8, [09] = @day9, [10] = @day10, [11] = @day11, [12] = @day12, [13] = @day13, [14] = @day14, [15] = @day15, [16] = @day16, [17] = @day17,[18] = @day18, [19] = @day19, [20] = @day20, [21] = @day21, [22] = @day22, [23] = @day23, [24] = @day24, [25] = @day25, [26] = @day26,  [27] = @day27, [28] = @day28, [29] = @day29, [30] = @day30, [31] = @day31, [MC6] = @imc6, [FL] = @ifl, [MLA] = @imla, [PatL] = @ipatl, [VL] = @ivl, [SL] = @isl, [A] = @ia, [HH] = @ihh, [MM] = @imm WHERE NAME = @staffnme"
            ' cqueryLoc = "INSERT INTO ATTENDANCETABLE([STAFFID],[STAFFNAME],[WORKDATE],[WEEK],[DTRREMARKS],[MONTHLYREMARKS],[SHIFTNAME]) VALUES(@stfid,@stfnme,@wrkDte,@wk,@dtrrmrks,@mnthlyrmrks,@shftnme)"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            With ce_cmd
                .Parameters.Add(New SqlCeParameter("@staffnme", staff_name))
                .Parameters.Add(New SqlCeParameter("@day1", list_of_remarks(0).ToString))
                .Parameters.Add(New SqlCeParameter("@day2", list_of_remarks(1).ToString))
                .Parameters.Add(New SqlCeParameter("@day3", list_of_remarks(2).ToString))
                .Parameters.Add(New SqlCeParameter("@day4", list_of_remarks(3).ToString))
                .Parameters.Add(New SqlCeParameter("@day5", list_of_remarks(4).ToString))
                .Parameters.Add(New SqlCeParameter("@day6", list_of_remarks(5).ToString))
                .Parameters.Add(New SqlCeParameter("@day7", list_of_remarks(6).ToString))
                .Parameters.Add(New SqlCeParameter("@day8", list_of_remarks(7).ToString))
                .Parameters.Add(New SqlCeParameter("@day9", list_of_remarks(8).ToString))
                .Parameters.Add(New SqlCeParameter("@day10", list_of_remarks(9).ToString))
                .Parameters.Add(New SqlCeParameter("@day11", list_of_remarks(10).ToString))
                .Parameters.Add(New SqlCeParameter("@day12", list_of_remarks(11).ToString))
                .Parameters.Add(New SqlCeParameter("@day13", list_of_remarks(12).ToString))
                .Parameters.Add(New SqlCeParameter("@day14", list_of_remarks(13).ToString))
                .Parameters.Add(New SqlCeParameter("@day15", list_of_remarks(14).ToString))
                .Parameters.Add(New SqlCeParameter("@day16", list_of_remarks(15).ToString))
                .Parameters.Add(New SqlCeParameter("@day17", list_of_remarks(16).ToString))
                .Parameters.Add(New SqlCeParameter("@day18", list_of_remarks(17).ToString))
                .Parameters.Add(New SqlCeParameter("@day19", list_of_remarks(18).ToString))
                .Parameters.Add(New SqlCeParameter("@day20", list_of_remarks(19).ToString))
                .Parameters.Add(New SqlCeParameter("@day21", list_of_remarks(20).ToString))
                .Parameters.Add(New SqlCeParameter("@day22", list_of_remarks(21).ToString))
                .Parameters.Add(New SqlCeParameter("@day23", list_of_remarks(22).ToString))
                .Parameters.Add(New SqlCeParameter("@day24", list_of_remarks(23).ToString))
                .Parameters.Add(New SqlCeParameter("@day25", list_of_remarks(24).ToString))
                .Parameters.Add(New SqlCeParameter("@day26", list_of_remarks(25).ToString))
                .Parameters.Add(New SqlCeParameter("@day27", list_of_remarks(26).ToString))
                .Parameters.Add(New SqlCeParameter("@day28", list_of_remarks(27).ToString))
                .Parameters.Add(New SqlCeParameter("@day29", list_of_remarks(28).ToString))
                .Parameters.Add(New SqlCeParameter("@day30", list_of_remarks(29).ToString))
                .Parameters.Add(New SqlCeParameter("@day31", list_of_remarks(30).ToString))

                .Parameters.Add(New SqlCeParameter("@imc6", MC6))
                .Parameters.Add(New SqlCeParameter("@ifl", FL))
                .Parameters.Add(New SqlCeParameter("@imla", MLA))
                .Parameters.Add(New SqlCeParameter("@ipatl", PATL))
                .Parameters.Add(New SqlCeParameter("@ivl", VL))
                .Parameters.Add(New SqlCeParameter("@isl", SL))
                .Parameters.Add(New SqlCeParameter("@ia", A))
                .Parameters.Add(New SqlCeParameter("@ihh", HH))
                .Parameters.Add(New SqlCeParameter("@imm", MM))


                .ExecuteNonQuery()
            End With

        Catch ex As Exception
            MessageBox.Show("[UPDATE_SRATABLE()]" & vbCrLf & ex.Message)
        End Try
    End Sub








#End Region

    'Public Function GET_SUM_TOTAL_UNDERTIME_MONTHLYHH(staff_id As String, s_date As Date, e_date As Date) As Integer
    '    Dim result As Integer
    '    Dim hh As Integer = 0

    '    'ce_query = "SELECT SUM(CONVERT(INT, UNDERHHREMARKS)) AS [HH], SUM(CONVERT(INT, UNDERMMREMARKS)) AS MM FROM DTR WHERE (STAFFID = '" & staff_id & "') AND (UNDERHHREMARKS = '1' OR UNDERHHREMARKS = '2' OR UNDERHHREMARKS = '3' OR UNDERHHREMARKS = '4' OR UNDERHHREMARKS = '5' OR UNDERHHREMARKS = '6' OR UNDERHHREMARKS = '7' OR UNDERHHREMARKS = '8' OR UNDERHHREMARKS = '9' OR UNDERHHREMARKS = '10' OR UNDERHHREMARKS = '11' OR UNDERHHREMARKS = '12' OR UNDERHHREMARKS = '13' OR UNDERHHREMARKS = '14' OR UNDERHHREMARKS = '15') AND (DATE BETWEEN '" & s_date.ToShortDateString & "' AND '" & e_date.ToShortDateString & "')"

    '    'ce_query = "SELECT SUM(CONVERT(INT, UNDERHHREMARKS)) AS HH, SUM(CONVERT(INT, UNDERMMREMARKS)) AS MM FROM(DTR) WHERE (STAFFID = '" & staff_id & "') AND (UNDERHHREMARKS BETWEEN '0' AND '9') OR (STAFFID = '" & staff_id & "') AND (UNDERMMREMARKS BETWEEN '0' AND '9') AND (DATE BETWEEN '" & s_date.ToShortDateString & "' AND '" & e_date.ToShortDateString & "')"

    '    '  ce_query = "SELECT SUM(CONVERT(INT, UNDERHHREMARKS)) AS HH, SUM(CONVERT(INT, UNDERMMREMARKS)) AS MM FROM (DTR) WHERE (STAFFID = '" & staff_id & "') AND (UNDERHHREMARKS BETWEEN '0' AND '9') AND (DATE BETWEEN '" & s_date.ToShortDateString & "' AND '" & e_date.ToShortDateString & "') OR (STAFFID = '" & staff_id & "') AND (UNDERMMREMARKS BETWEEN '0' AND '9') AND (DATE BETWEEN '" & s_date.ToShortDateString & "' AND '" & e_date.ToShortDateString & "')"
    '    ce_query = "SELECT SUM(CONVERT(INT, UNDERHHREMARKS)) AS [HH] FROM(DTR) WHERE (STAFFID = '" & staff_id & "') AND (UNDERHHREMARKS BETWEEN '0' AND '9') AND (DATE BETWEEN '" & s_date.ToShortDateString & "' AND '" & e_date.ToShortDateString & "')"
    '    Try
    '        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
    '        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
    '        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
    '        If sdr.Read Then
    '            If Not sdr.IsDBNull(sdr.GetOrdinal("HH")) Then
    '                hh = sdr("HH")
    '            Else
    '                result = 0
    '            End If
    '        Else
    '            result = 0
    '        End If

    '        result = hh
    '        sdr.Close()
    '    Catch ex As Exception
    '        MessageBox.Show("[GET_SUM_TOTAL_UNDERTIME_MONTHLYHH()]" & vbCrLf & ex.Message)

    '    End Try

    '    Return result
    'End Function


    'Public Function GET_SUM_TOTAL_UNDERTIME_MONTHLYMM(staff_id As String, s_date As Date, e_date As Date) As Integer
    '    Dim result As Integer
    '    Dim mm As Integer = 0
    '    'ce_query = "SELECT SUM(CONVERT(INT, UNDERHHREMARKS)) AS [HH], SUM(CONVERT(INT, UNDERMMREMARKS)) AS MM FROM DTR WHERE (STAFFID = '" & staff_id & "') AND (UNDERHHREMARKS = '1' OR UNDERHHREMARKS = '2' OR UNDERHHREMARKS = '3' OR UNDERHHREMARKS = '4' OR UNDERHHREMARKS = '5' OR UNDERHHREMARKS = '6' OR UNDERHHREMARKS = '7' OR UNDERHHREMARKS = '8' OR UNDERHHREMARKS = '9' OR UNDERHHREMARKS = '10' OR UNDERHHREMARKS = '11' OR UNDERHHREMARKS = '12' OR UNDERHHREMARKS = '13' OR UNDERHHREMARKS = '14' OR UNDERHHREMARKS = '15') AND (DATE BETWEEN '" & s_date.ToShortDateString & "' AND '" & e_date.ToShortDateString & "')"

    '    'ce_query = "SELECT SUM(CONVERT(INT, UNDERHHREMARKS)) AS HH, SUM(CONVERT(INT, UNDERMMREMARKS)) AS MM FROM(DTR) WHERE (STAFFID = '" & staff_id & "') AND (UNDERHHREMARKS BETWEEN '0' AND '9') OR (STAFFID = '" & staff_id & "') AND (UNDERMMREMARKS BETWEEN '0' AND '9') AND (DATE BETWEEN '" & s_date.ToShortDateString & "' AND '" & e_date.ToShortDateString & "')"

    '    '  ce_query = "SELECT SUM(CONVERT(INT, UNDERHHREMARKS)) AS HH, SUM(CONVERT(INT, UNDERMMREMARKS)) AS MM FROM (DTR) WHERE (STAFFID = '" & staff_id & "') AND (UNDERHHREMARKS BETWEEN '0' AND '9') AND (DATE BETWEEN '" & s_date.ToShortDateString & "' AND '" & e_date.ToShortDateString & "') OR (STAFFID = '" & staff_id & "') AND (UNDERMMREMARKS BETWEEN '0' AND '9') AND (DATE BETWEEN '" & s_date.ToShortDateString & "' AND '" & e_date.ToShortDateString & "')"
    '    ce_query = "SELECT SUM(CONVERT(INT, UNDERMMREMARKS)) AS [MM] FROM(DTR) WHERE (STAFFID = '" & staff_id & "') AND (UNDERMMREMARKS BETWEEN '0' AND '9') AND (DATE BETWEEN '" & s_date.ToShortDateString & "' AND '" & e_date.ToShortDateString & "')"
    '    Try
    '        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
    '        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
    '        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
    '        If sdr.Read Then
    '            If Not sdr.IsDBNull(sdr.GetOrdinal("MM")) Then
    '                mm = sdr("MM")
    '            Else
    '                result = 0
    '            End If
    '        Else
    '            result = 0
    '        End If

    '        result = mm
    '        sdr.Close()
    '    Catch ex As Exception
    '        MessageBox.Show("[GET_SUM_TOTAL_UNDERTIME_MONTHLYMM()]" & vbCrLf & ex.Message)

    '    End Try

    '    Return result
    'End Function








#Region "USER ACCESS"

    Public Function CHECK_USER_ACCESS_TABLE_ISEMPTY() As Boolean
        Dim result As Boolean = True
        Try

            ce_query = "SELECT * FROM USERACCESS"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = False
            End If
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("CHECK_USER_ACCESS_ISEMPTY() " & ex.Message)
        End Try

        Return result
    End Function


    Public Function ALLOW_ACCESS(username As String, password As String) As Boolean
        Dim result As Boolean = False
        Try

            ce_query = "SELECT * FROM USERACCESS WHERE USERNAME = '" & username & "' AND PASSWORD = '" & password & "'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            While sdr.Read
                result = True
            End While
            sdr.Dispose()
        Catch ex As Exception
            MessageBox.Show("ALLOW_ACCESS() " & ex.Message)
        End Try

        Return result
    End Function

    Public Function GET_ALL_USERACCESS_NAME() As List(Of String)
        Dim result As New List(Of String)
        Try

            ce_query = "SELECT STAFFNAME FROM USERACCESS"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            While sdr.Read
                result.Add(sdr(0).ToString)
            End While


            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("GET_ALL_USERACCESS_NAME() " & ex.Message)
        End Try

        Return result

    End Function


    Public Sub INSERT_NEW_USERACCESS(staffname As String, username As String, password As String, priv As String)
        ce_query = "INSERT INTO USERACCESS(STAFFNAME, USERNAME, PASSWORD, PREVILEGE) VALUES(@istaffname, @iusername, @ipassword,@ipriv)"

        Try
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            With ce_cmd
                .Parameters.Add(New SqlCeParameter("@istaffname", staffname))
                .Parameters.Add(New SqlCeParameter("@iusername", username))
                .Parameters.Add(New SqlCeParameter("@ipassword", password))
                .Parameters.Add(New SqlCeParameter("@ipriv", priv))
                .ExecuteNonQuery()
            End With
        Catch ex As Exception
            MessageBox.Show("[USER_ACCESS_ADD()]" & vbCrLf & ex.Message)
        End Try
    End Sub
    Public Function GET_USER_ACCESS_FULLNAME(username As String, password As String) As String
        Dim result As String = ""

        ce_query = "SELECT STAFFNAME FROM USERACCESS WHERE (USERNAME = '" & username & "') AND (PASSWORD = '" & password & "')"

        Try
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            While sdr.Read
                result = sdr(0).ToString
            End While
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("GET_USER_ACCESS_FULLNAME() " & ex.Message)
        End Try
        Return result
    End Function

    Public Sub REMOVE_SELECTED_USERNAME(staffname As String)
        ce_query = "DELETE FROM USERACCESS WHERE STAFFNAME = '" & staffname & "'"

        Try
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            ce_cmd.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show("[REMOVE_SELECTED_USERNAME()]" & vbCrLf & ex.Message)
        End Try
    End Sub
    Public Function GET_USER_PRIVILEGE(username As String, password As String) As String
        Dim result As String = ""

        ce_query = "SELECT PREVILEGE FROM USERACCESS WHERE (USERNAME = '" & username & "') AND (PASSWORD = '" & password & "')"

        Try
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            While sdr.Read
                result = sdr(0).ToString
            End While
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("GET_USER_PRIVILEGE() " & ex.Message)
        End Try
        Return result
    End Function

#End Region







#Region "PUBLIC HOLIDAY POLLING"

    Public Function CHECK_HOLIDAY(_day As Date) As Boolean
        Dim result As Boolean = False
        ce_query = "SELECT * FROM PUBLICHOLIDAYTABLE WHERE DATE = '" & _day.ToShortDateString & "'"
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
        If sdr.Read Then
            result = True
        End If
        sdr.Close()
        Return result
    End Function
    Public Function GET_HOLIDAY_NAME(_day As Date) As String
        Dim result As String = Nothing
        ce_query = "SELECT HOLIDAYNAME FROM PUBLICHOLIDAYTABLE WHERE DATE = '" & _day.ToShortDateString & "'"
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
        If sdr.Read Then
            result = sdr(0).ToString
        End If
        sdr.Close()
        Return result
    End Function


#End Region




#Region "FILED LEAVE POLLING"

    Public Function CHECK_FILED_LEAVE(staff_id As String, _day As Date) As Boolean
        Dim result As Boolean = False
        ce_query = "SELECT * FROM FILEDLEAVETABLE WHERE STAFFID = '" & staff_id & "' AND WORKDATE = '" & _day.ToShortDateString & "'"
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
        If sdr.Read Then
            result = True
        End If
        sdr.Close()
        Return result
    End Function

    Public Function GET_FILED_LEAVEID(staff_id As String, _day As Date) As Integer
        Dim result As Integer = Nothing
        ce_query = "SELECT LEAVEID FROM FILEDLEAVETABLE WHERE STAFFID = '" & staff_id & "' AND WORKDATE = '" & _day.ToShortDateString & "'"
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
        If sdr.Read Then
            result = sdr(0)
        End If
        sdr.Close()
        Return result
    End Function

    Public Function GET_LEAVE_SYMBOL(leave_id As Integer) As String
        Dim result As String = ""
        ce_query = "SELECT SYMBOL FROM LEAVECLASSTABLE WHERE ID = '" & leave_id & "'"
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
        If sdr.Read Then
            result = sdr(0)
        End If
        sdr.Close()
        Return result
    End Function

    Public Function GET_LEAVE_NAME(leave_id As Integer) As String
        Dim result As String = ""
        ce_query = "SELECT NAME FROM LEAVECLASSTABLE WHERE ID = '" & leave_id & "'"
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
        If sdr.Read Then
            result = sdr(0).ToString
        End If
        sdr.Close()
        Return result
    End Function
    Public Function CHECK_IF_PAID_LEAVE(leaveid As Integer) As Boolean
        Dim result As Boolean = False
        ce_query = "SELECT * FROM LEAVECLASSTABLE WHERE ID = '" & leaveid & "' AND PAIDLEAVE = 'TRUE'"
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
        If sdr.Read Then
            result = True
        End If
        sdr.Close()
        Return result
    End Function


#End Region





#Region "Assign User Schedule"

    Public Function LOAD_EXISTING_SHEDULE(staffid As String, s_date As Date, e_date As Date) As DataTable
        Dim result As DataTable

        'connStr = New SqlCeConnection(My.Settings.LocalConnectionString)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        ce_query = "SELECT STAFFNAME as [NAME],SHIFTNAME as [SHIFT NAME],WORKDATE as [WORKDATE] FROM ATTENDANCETABLE WHERE [STAFFID] = '" & staffid & "' AND [WORKDATE] BETWEEN '" & s_date.ToShortDateString & "' AND  '" & e_date.ToShortDateString & "'"
        'sCommand = New SqlCeCommand(sql, connStr)
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        ce_da = New SqlCeDataAdapter(ce_cmd)
        ce_builder = New SqlCeCommandBuilder(ce_da)
        ce_ds = New DataSet()
        ce_da.Fill(ce_ds, "ATTENDANCETABLE")
        ce_tbl = ce_ds.Tables("ATTENDANCETABLE")
        ce_cnn.Close()
        result = ce_ds.Tables("ATTENDANCETABLE")
        Return result
    End Function

    Public Sub REMOVE_SELECTED_SCHEDULE(staff_id As String, sched_date As Date)
        Try
            ce_query = "DELETE FROM ATTENDANCETABLE WHERE STAFFID = @staffid AND WORKDATE = @wrkdate"
            ' cqueryLoc = "INSERT INTO ATTENDANCETABLE([STAFFID],[STAFFNAME],[WORKDATE],[WEEK],[DTRREMARKS],[MONTHLYREMARKS],[SHIFTNAME]) VALUES(@stfid,@stfnme,@wrkDte,@wk,@dtrrmrks,@mnthlyrmrks,@shftnme)"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            With ce_cmd
                .Parameters.Add(New SqlCeParameter("@wrkdate", sched_date.ToShortDateString))
                .Parameters.Add(New SqlCeParameter("@staffid", staff_id))
                .ExecuteNonQuery()
            End With
            'ce_cmd.Dispose()
        Catch ex As Exception
            MessageBox.Show("[REMOVE_SELECTED_SCHEDULE()]" & vbCrLf & ex.Message)
        End Try

    End Sub

#End Region




#Region "ATTENDANCE OPTION"

    Public Function LOAD_ATTENDANCE_TABLE(All As Boolean, All_modified As Boolean, staffname As String, dep_id As Integer, s_date As Date, e_date As Date) As DataTable
        Dim result As DataTable = Nothing

        If All = True And All_modified = False Then
            ce_query = "SELECT EmployeeProfiles.FullName as [NAME], TIMELOGS.LOGTIME as [LOGS], TIMELOGS.DATE, TIMELOGS.TRANSACTIONTYPE as [TRANSACTION TYPE], TIMELOGS.ISADJUSTED as [iSMODIFIED], TIMELOGS.ORIGIN as [SOURCE]" & _
                    "FROM TIMELOGS INNER JOIN " & _
                    "EmployeeProfiles ON TIMELOGS.STAFFID = EmployeeProfiles.EmpID " & _
                    "WHERE(TIMELOGS.DATE BETWEEN '" & s_date.ToShortDateString & "' AND '" & e_date.ToShortDateString & "')"

        ElseIf All = False And All_modified = True Then
            ce_query = "SELECT EmployeeProfiles.FullName as [NAME], TIMELOGS.LOGTIME as [LOGS], TIMELOGS.DATE, TIMELOGS.TRANSACTIONTYPE as [TRANSACTION TYPE], TIMELOGS.ISADJUSTED as [iSMODIFIED], TIMELOGS.ORIGIN as [SOURCE]" & _
             "FROM TIMELOGS INNER JOIN " & _
             "EmployeeProfiles ON TIMELOGS.STAFFID = EmployeeProfiles.EmpID INNER JOIN DEPARTMENTTABLE ON EmployeeProfiles.DEPID = DEPARTMENTTABLE.ID " & _
             "WHERE(TIMELOGS.DATE BETWEEN '" & s_date.ToShortDateString & "' AND '" & e_date.ToShortDateString & "') AND (EmployeeProfiles.FullName = '" & staffname & "') AND (DEPARTMENTTABLE.ID = '" & dep_id & "') AND (ISADJUSTED = 'TRUE')"

        Else
            ce_query = "SELECT EmployeeProfiles.FullName as [NAME], TIMELOGS.LOGTIME as [LOGS], TIMELOGS.DATE, TIMELOGS.TRANSACTIONTYPE as [TRANSACTION TYPE], TIMELOGS.ISADJUSTED as [iSMODIFIED], TIMELOGS.ORIGIN as [SOURCE]" & _
             "FROM TIMELOGS INNER JOIN " & _
             "EmployeeProfiles ON TIMELOGS.STAFFID = EmployeeProfiles.EmpID INNER JOIN DEPARTMENTTABLE ON EmployeeProfiles.DEPID = DEPARTMENTTABLE.ID " & _
             "WHERE(TIMELOGS.DATE BETWEEN '" & s_date.ToShortDateString & "' AND '" & e_date.ToShortDateString & "') AND (EmployeeProfiles.FullName = '" & staffname & "') AND (DEPARTMENTTABLE.ID = '" & dep_id & "')"
        End If



        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        ce_da = New SqlCeDataAdapter(ce_cmd)
        ce_builder = New SqlCeCommandBuilder(ce_da)
        ce_ds = New DataSet()
        ce_da.Fill(ce_ds, "LOGS")
        ce_tbl = ce_ds.Tables("LOGS")
        ce_cnn.Close()
        result = ce_ds.Tables("LOGS")
        Return result
    End Function

#End Region




#Region "DEPARTMENT MANAGEMENT"

    Public Function GET_ALL_DEPARTMENT_NAME() As List(Of String)

        Dim result As New List(Of String)
        ce_query = "SELECT DEPARTMENTNAME FROM DEPARTMENTTABLE"

        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
        While sdr.Read = True
            If Not IsDBNull(sdr(0)) Then
                result.Add(sdr(0))
            End If

        End While
        sdr.Close()
        Return result
    End Function

    Public Function COUNT_EMPLOYEE_IN_DEPARTMENT(dept_name As String) As Integer

        Dim result As Integer = 0
        ' ce_query = "SELECT DEPARTMENTNAME FROM DEPARTMENTTABLE"
        ce_query = "SELECT COUNT(EmployeeProfiles.EmpID) AS EMPCOUNT FROM DEPARTMENTTABLE INNER JOIN EmployeeProfiles ON DEPARTMENTTABLE.ID = EmployeeProfiles.DEPID WHERE(DEPARTMENTTABLE.DEPARTMENTNAME = '" & dept_name & "')"
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
        While sdr.Read = True
            result = sdr(0)
        End While
        sdr.Close()
        Return result
    End Function
    Public Function GET_DEPARTMENT_ID(department_name As String) As Integer
        Dim result As String = Nothing
        ce_query = "SELECT [ID] FROM DEPARTMENTTABLE WHERE DEPARTMENTNAME = '" & department_name & "'"

        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
        If sdr.Read = True Then
            result = sdr(0)
        End If
        sdr.Close()
        Return result
    End Function

    Public Sub INSERT_NEW_DEPARTMENT(departmentname As String)

        Try
            ce_query = "INSERT INTO DEPARTMENTTABLE([DEPARTMENTNAME]) VALUES(@dpnme)"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            With ce_cmd
                .Parameters.Add(New SqlCeParameter("@dpnme", departmentname))
                .ExecuteNonQuery()
            End With
        Catch ex As Exception
            MessageBox.Show("[INSERT_NEW_DEPARTMENT()]" & ex.Message)
        End Try
    End Sub

    Private Function get_lastid(tablename As String, colname As String) As String
        Dim result As String = Nothing
        ce_query = "SELECT [ID] FROM " & tablename & " ORDER BY [ID] DESC"
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
        If sdr.Read Then
            result = sdr(0)
        End If
        sdr.Close()
        Return result
    End Function

    Public Function CHECK_DEPARTMENT_IF_EXIST(dp_name As String) As Boolean
        Dim result As Boolean = False
        ce_query = "SELECT * FROM DEPARTMENTTABLE WHERE DEPARTMENTNAME = '" & dp_name & "'"
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
        If sdr.Read Then
            result = True
        End If
        sdr.Close()
        Return result
    End Function

    Public Sub REMOVE_SELECTED_DEPARTMENT(dp_name As String)
        Try
            ce_query = "DELETE FROM DEPARTMENTTABLE WHERE DEPARTMENTNAME = '" & dp_name & "'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            ce_cmd.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show("[REMOVE_SELECTED_DEPARTMENT()]" & vbCrLf & ex.Message)
        End Try

    End Sub
    Public Sub UPDATE_SELECTED_DEPARTMENT(dp_name As String, new_dp_name As String)
        Try
            ' ce_query = "UPDATE WHERE DEPARTMENTNAME = '" & dp_name & "'"
            ce_query = "UPDATE DEPARTMENTTABLE SET [DEPARTMENTNAME] = @new_dp_name WHERE [DEPARTMENTNAME] = @cur_dp_name"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            With ce_cmd
                .Parameters.Add(New SqlCeParameter("@new_dp_name", new_dp_name))
                .Parameters.Add(New SqlCeParameter("@cur_dp_name", dp_name))
                .ExecuteNonQuery()
            End With
        Catch ex As Exception
            MessageBox.Show("[UPDATE_SELECTED_DEPARTMENT()]" & vbCrLf & ex.Message)
        End Try

    End Sub

    Public Function GET_DEPARTMENT_NAME(department_id As Integer) As String
        Dim result As String = ""


        ce_query = "SELECT DEPARTMENTNAME FROM DEPARTMENTTABLE WHERE ID = '" & department_id & "'"
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
        If sdr.Read Then
            result = sdr(0).ToString
        End If
        sdr.Close()

        Return result
    End Function


    Public Function GET_STAFF_ASSIGN_DEPARTMENT_NAME(staff_id As String) As String
        Dim result As String = ""
        Try
            ce_query = "SELECT DEPARTMENTTABLE.DEPARTMENTNAME FROM EmployeeProfiles INNER JOIN DEPARTMENTTABLE ON EmployeeProfiles.DEPID = DEPARTMENTTABLE.ID WHERE(EmployeeProfiles.EmpID = '" & staff_id & "')"
            ' ce_query = "SELECT DEPARTMENTNAME FROM DEPARTMENTTABLE WHERE ID = '" & department_id & "'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = sdr(0).ToString
            End If
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_STAFF_ASSIGN_DEPARTMENT_NAME()]" & vbCrLf & ex.Message)
        End Try
        Return result
    End Function
    Public Function GET_STAFF_DESIGNATION(staff_id As String) As String
        Dim result As String = ""
        Try
            ce_query = "SELECT DESIGNATION FROM EmployeeProfiles WHERE(EmpID = '" & staff_id & "')"
            ' ce_query = "SELECT DEPARTMENTNAME FROM DEPARTMENTTABLE WHERE ID = '" & department_id & "'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = sdr(0).ToString
            End If
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_STAFF_ASSIGN_DEPARTMENT_NAME()]" & vbCrLf & ex.Message)
        End Try
        Return result
    End Function
    Public Function GET_STAFF_OTHER_DETAILS(staff_id As String) As String
        Dim result As String = ""
        Try
            ce_query = "SELECT OTHERDETAILS FROM EmployeeProfiles WHERE(EmpID = '" & staff_id & "')"
            ' ce_query = "SELECT DEPARTMENTNAME FROM DEPARTMENTTABLE WHERE ID = '" & department_id & "'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = sdr(0).ToString
            End If
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_STAFF_OTHER_DETAILS()]" & vbCrLf & ex.Message)
        End Try
        Return result
    End Function
    Public Function GET_STAFF_IMAGEPATH(staff_id As String) As String
        Dim result As String = ""
        Try
            ce_query = "SELECT IMAGEPATH FROM EmployeeProfiles WHERE(EmpID = '" & staff_id & "')"
            ' ce_query = "SELECT DEPARTMENTNAME FROM DEPARTMENTTABLE WHERE ID = '" & department_id & "'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = sdr(0).ToString
            End If
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_STAFF_IMAGEPATH()]" & vbCrLf & ex.Message)
        End Try
        Return result
    End Function




    'Public Sub INSERT_NEW_DEPARTMENT()

    'End Sub



#End Region


#Region "EMPLOYEE PROFILES"


    Public Function GET_ALL_EMPLOYEES(ALL As Boolean, by_department As Boolean, department_name As String, by_staff_id As Boolean, staff_id As String, by_staff_name As Boolean, staff_name As String) As DataTable
        Dim result As DataTable

        If ALL = True Then
            ce_query = "SELECT EmpID AS [STAFF ID], FullName AS NAME FROM EmployeeProfiles"
        ElseIf by_department = True Then
            ce_query = "SELECT EmployeeProfiles.EmpID as [STAFF ID], EmployeeProfiles.FullName as [NAME], DEPARTMENTTABLE.DEPARTMENTNAME as [DEPARTMENT] FROM DEPARTMENTTABLE INNER JOIN EmployeeProfiles ON DEPARTMENTTABLE.ID = EmployeeProfiles.DEPID " & _
                 "WHERE (DEPARTMENTTABLE.DEPARTMENTNAME = '" & department_name & "')"
        ElseIf by_staff_id = True Then
            'WHERE EmpID LIKE '%" & txtEmpIDNo.Text.Trim & "%'"
            ce_query = "SELECT EmployeeProfiles.EmpID as [STAFF ID], EmployeeProfiles.FullName as [NAME], DEPARTMENTTABLE.DEPARTMENTNAME as [DEPARTMENT] FROM DEPARTMENTTABLE INNER JOIN EmployeeProfiles ON DEPARTMENTTABLE.ID = EmployeeProfiles.DEPID " & _
            "WHERE (EmployeeProfiles.EmpID = '%" & staff_id & "%')"

        ElseIf by_staff_name = True Then
            ce_query = "SELECT EmployeeProfiles.EmpID as [STAFF ID], EmployeeProfiles.FullName as [NAME], DEPARTMENTTABLE.DEPARTMENTNAME as [DEPARTMENT] FROM DEPARTMENTTABLE INNER JOIN EmployeeProfiles ON DEPARTMENTTABLE.ID = EmployeeProfiles.DEPID " & _
                        "WHERE (EmployeeProfiles.FullName = '%" & staff_name & "%')"
        End If

        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        ce_da = New SqlCeDataAdapter(ce_cmd)
        ce_builder = New SqlCeCommandBuilder(ce_da)
        ce_ds = New DataSet()
        ce_da.Fill(ce_ds, "EMPLOYEEPROFILES")
        ce_tbl = ce_ds.Tables("EMPLOYEEPROFILES")
        ce_cnn.Close()
        result = ce_ds.Tables("EMPLOYEEPROFILES")
        Return result
    End Function


    Public Sub GET_ALL_STAFFNAME_PLUS_ID_AS_DICTIONARY(ByRef listOfStaff_and_Name As Dictionary(Of String, String), query_str As String)
        ce_query = query_str
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
        While sdr.Read
            listOfStaff_and_Name.Add(sdr(0), sdr(1))
        End While
        sdr.Close()
    End Sub





    Public Function TEXT_CHANGE_SEARCH_ID(staffid As String) As DataTable
        Dim result As DataTable = Nothing
        Try
            ce_query = "SELECT [EmpID] as ID, [FullName] as Name FROM [EmployeeProfiles] WHERE EmpID LIKE '%" & staffid & "%'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            ce_da = New SqlCeDataAdapter(ce_cmd)
            ce_builder = New SqlCeCommandBuilder(ce_da)
            ce_ds = New DataSet()
            ce_da.Fill(ce_ds, "MYTABLE")
            ce_tbl = ce_ds.Tables("MYTABLE")
            ce_cnn.Close()
            result = ce_ds.Tables("MYTABLE")
        Catch ex As Exception
            MessageBox.Show("[TEXT_CHANGE_SEARCH_ID()]" & vbCrLf & ex.Message)
        End Try
        Return result
    End Function



    Public Function LOAD_ALL_INACTIVE_STAFF() As DataTable
        Dim result As DataTable
        ce_query = "SELECT STAFFID AS  [STAFF ID], STAFFNAME AS [STAFFNAME], DEPARTMENT, INACTIVEDATE AS [INACTIVE DATE]FROM INACTIVESTAFF"
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        ce_da = New SqlCeDataAdapter(ce_cmd)
        ce_builder = New SqlCeCommandBuilder(ce_da)
        ce_ds = New DataSet()
        ce_da.Fill(ce_ds, "INACTIVESTAFF")
        ce_tbl = ce_ds.Tables("INACTIVESTAFF")
        ce_cnn.Close()
        result = ce_ds.Tables("INACTIVESTAFF")


        Return result
    End Function

    Public Function GET_EMPLOYEE_DETAILS(Staff_id As String) As List(Of String)
        Dim result As New List(Of String)


        '  ce_query = "SELECT EmployeeProfiles.EmpID, EmployeeProfiles.FullName, DEPARTMENTTABLE.DEPARTMENTNAME FROM DEPARTMENTTABLE " & _
        '     "INNER JOIN EmployeeProfiles ON DEPARTMENTTABLE.ID = EmployeeProfiles.DEPID WHERE (EmployeeProfiles.EmpID = '" & Staff_id & "')"

        ce_query = "SELECT * FROM EMPLOYEEPROFILES WHERE EMPID = '" & Staff_id & "'"

        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
        If sdr.Read Then
            result.Add(sdr("EMPID"))
            result.Add(sdr("FULLNAME"))
            'result.Add(sdr("DEPARTMENTNAME"))
        End If
        sdr.Close()
        Return result
    End Function
    Public Sub UPDATE_EMPLOYEE_DETAILS(staff_id As String, staff_name As String, dep_id As Integer, designation As String, other_details As String, image_path As String)
        Try
            ce_query = "UPDATE EMPLOYEEPROFILES SET FULLNAME = @nme, DEPID = @dptid , DESIGNATION = @desig, OTHERDETAILS = @otherdetails, IMAGEPATH = @imgpath WHERE EMPID = @stfid"
            ' cqueryLoc = "INSERT INTO ATTENDANCETABLE([STAFFID],[STAFFNAME],[WORKDATE],[WEEK],[DTRREMARKS],[MONTHLYREMARKS],[SHIFTNAME]) VALUES(@stfid,@stfnme,@wrkDte,@wk,@dtrrmrks,@mnthlyrmrks,@shftnme)"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            With ce_cmd
                .Parameters.Add(New SqlCeParameter("@stfid", staff_id))
                .Parameters.Add(New SqlCeParameter("@nme", staff_name))
                .Parameters.Add(New SqlCeParameter("@dptid", dep_id))
                .Parameters.Add(New SqlCeParameter("@desig", designation))
                .Parameters.Add(New SqlCeParameter("@otherdetails", other_details))
                .Parameters.Add(New SqlCeParameter("@imgpath", image_path))

                .ExecuteNonQuery()
            End With
            'ce_cmd.Dispose()
        Catch ex As Exception
            MessageBox.Show("[UPDATE_EMPLOYEE_DETAILS()]" & vbCrLf & ex.Message)
        End Try
    End Sub
    Public Function GET_ALL_EMPLOYEES_ASLIST(ALL As Boolean, department_id As Integer) As List(Of String)
        Dim result As New List(Of String)

        If ALL = True Then
            ce_query = "SELECT FULLNAME as [NAME] FROM EMPLOYEEPROFILES"
        Else
            ce_query = "SELECT FULLNAME as [NAME] FROM EMPLOYEEPROFILES WHERE DEPID = '" & department_id & "'"
        End If
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
        While sdr.Read
            result.Add(sdr(0))
        End While
        sdr.Close()
        Return result
    End Function

    Public Sub REMOVE_EMPLOYEE(staff_id As String)
        Try
            ce_query = "DELETE FROM EMPLOYEEPROFILES WHERE EMPID = '" & staff_id & "'"
            ' cqueryLoc = "INSERT INTO ATTENDANCETABLE([STAFFID],[STAFFNAME],[WORKDATE],[WEEK],[DTRREMARKS],[MONTHLYREMARKS],[SHIFTNAME]) VALUES(@stfid,@stfnme,@wrkDte,@wk,@dtrrmrks,@mnthlyrmrks,@shftnme)"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            ce_cmd.ExecuteNonQuery()
            'ce_cmd.Dispose()
        Catch ex As Exception
            MessageBox.Show("[REMOVE_EMPLOYEE()] " & vbCrLf & ex.Message)
        End Try
    End Sub
    Public Sub TAG_AS_RESIGN_EMPLOYEE(staff_id As String, staff_name As String, department_name As String, datefiled As Date, filedby As String)
        ce_query = "INSERT INTO INACTIVESTAFF(STAFFID,STAFFNAME,DEPARTMENT,INACTIVEDATE,FILEDBY) VALUES(@instaffid, @instaffname, @indepartment,@inactivedate,@infiledby)"

        Try
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            With ce_cmd
                .Parameters.Add(New SqlCeParameter("@instaffid", staff_id))
                .Parameters.Add(New SqlCeParameter("@instaffname", staff_name))
                .Parameters.Add(New SqlCeParameter("@indepartment", department_name))
                .Parameters.Add(New SqlCeParameter("@inactivedate", datefiled))
                .Parameters.Add(New SqlCeParameter("@infiledby", filedby))
                .ExecuteNonQuery()
            End With
        Catch ex As Exception
            MessageBox.Show("[TAG_AS_RESIGN_EMPLOYEE()]" & vbCrLf & ex.Message)
        End Try

    End Sub
    Public Sub TAG_AS_REHIRED_EMPLOYEE(staff_id As String, staff_name As String, department_id As Integer)
        ce_query = "INSERT INTO EMPLOYEEPROFILES(EMPID,FULLNAME,DEPID) VALUES(@instaffid, @infullname, @indepid)"

        Try
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            With ce_cmd
                .Parameters.Add(New SqlCeParameter("@instaffid", staff_id))
                .Parameters.Add(New SqlCeParameter("@infullname", staff_name))
                .Parameters.Add(New SqlCeParameter("@indepid", department_id))
                .ExecuteNonQuery()
            End With
        Catch ex As Exception
            MessageBox.Show("[TAG_AS_REHIRED_EMPLOYEE()] " & vbCrLf & ex.Message)
        End Try


        'Try
        '    ce_query = "INSERT INTO FROM EMPLOYEEPROFILES WHERE EMPID = '" & staff_id & "'"
        '    ' cqueryLoc = "INSERT INTO ATTENDANCETABLE([STAFFID],[STAFFNAME],[WORKDATE],[WEEK],[DTRREMARKS],[MONTHLYREMARKS],[SHIFTNAME]) VALUES(@stfid,@stfnme,@wrkDte,@wk,@dtrrmrks,@mnthlyrmrks,@shftnme)"
        '    ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        '    If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        '    ce_cmd.ExecuteNonQuery()
        ' 'ce_cmd.Dispose()
        'Catch ex As Exception
        '    MessageBox.Show("[REMOVE_EMPLOYEE()] " & vbCrLf & ex.Message)
        'End Try
    End Sub
    Public Function CHECK_EMPLOYEEID_IF_EXIST(staffid As String) As Boolean
        Dim result As Boolean = False
        ce_query = "SELECT * FROM EMPLOYEEPROFILES WHERE EMPID = '" & staffid & "'"
        Try
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = True
            End If
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[CHECK_EMPLOYEEID_IF_EXIST] " & vbCrLf & ex.Message)
        End Try
        Return result
    End Function

    Public Sub INSERT_NEW_STAFF(staff_id As String, staff_name As String, dept_id As Integer, designation As String, other_details As String, img_path As String)
        ce_query = "INSERT INTO EmployeeProfiles(EmpID, FullName, DEPID, DESIGNATION,OTHERDETAILS,IMAGEPATH) VALUES(@InID, @InFullName, @InDeptID, @InDesignation, @InOtherDetails, @InImagepath)"

        Try
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            With ce_cmd
                .Parameters.Add(New SqlCeParameter("@InID", staff_id))
                .Parameters.Add(New SqlCeParameter("@InDeptID", dept_id))
                .Parameters.Add(New SqlCeParameter("@InFullName", staff_name))
                .Parameters.Add(New SqlCeParameter("@InDesignation", designation))
                .Parameters.Add(New SqlCeParameter("@InOtherDetails", other_details))
                .Parameters.Add(New SqlCeParameter("@InImagepath", img_path))

                .ExecuteNonQuery()
            End With
        Catch ex As Exception
            MessageBox.Show("[INSERT_NEW_STAFF()]" & vbCrLf & ex.Message)
        End Try
    End Sub
    Public Sub REMOVE_REHIRED_STAFF_FROM_INACTIVETABLE(staff_id As String)
        ce_query = "DELETE FROM INACTIVESTAFF WHERE STAFFID = '" & staff_id & "'"
        Try
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            ce_cmd.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show("[REMOVE_REHIRED_STAFF_FROM_INACTIVETABLE(" & staff_id & ")]" & vbCrLf & ex.Message)
        End Try
    End Sub
    Public Function GET_RESIGNED_DATE(staff_id As String) As String

        Dim result As String = ""
        ce_query = "SELECT INACTIVEDATE FROM INACTIVESTAFF WHERE STAFFID = '" & staff_id & "'"
        Try
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = sdr(0)
            End If
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_RESIGNED_DATE(" & staff_id & ")]" & vbCrLf & ex.Message)
        End Try
        Return result
    End Function


    Public Function GET_ALL_STAFF_DETAILS(dpt_name As String) As List(Of String)
        Dim lst_of_stf_dtls As New List(Of String)

        Try
            'ce_query = "SELECT EmployeeProfiles.EmpID, EmployeeProfiles.FullName, DEPARTMENTTABLE.DEPARTMENTNAME FROM EmployeeProfiles INNER JOIN DEPARTMENTTABLE ON EmployeeProfiles.DEPID = DEPARTMENTTABLE.ID WHERE (DEPARTMENTTABLE.DEPARTMENTNAME = '" & dpt_name & "')"
            'ce_query = "SELECT FULLNAME, EMPID, DESIGNATION FROM EMPLOYEEPROFILES WHERE (DEPARTMENTNAME = '" & dpt_name & "')"
            ce_query = "SELECT EmployeeProfiles.EmpID, EmployeeProfiles.FullName, EmployeeProfiles.DESIGNATION FROM EmployeeProfiles INNER JOIN DEPARTMENTTABLE ON EmployeeProfiles.DEPID = DEPARTMENTTABLE.ID WHERE (DEPARTMENTTABLE.DEPARTMENTNAME = '" & dpt_name & "')"

            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            While sdr.Read
                lst_of_stf_dtls.Add(sdr(1) & "+" & sdr(0) & "+" & sdr(2))
                ''staffname + staffid + DESIGNATION
                ' Console.WriteLine(sdr(1))
            End While
            lst_of_stf_dtls.Sort()
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_ALL_STAFF_DETAILS()]" & ex.Message)
        End Try
        Return lst_of_stf_dtls
    End Function




    Public Function GET_ALL_STAFF_ID(ALL As Boolean, department_id As Integer) As List(Of String)

        Dim result As New List(Of String)
        If ALL = True Then
            ce_query = "SELECT EMPID FROM EMPLOYEEPROFILES"
        Else

            ce_query = "SELECT EMPID FROM EMPLOYEEPROFILES WHERE DEPID = '" & department_id & "'"

        End If

        Try
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            While sdr.Read
                result.Add(sdr(0))
            End While
            sdr.Close()
            'ce_cmd.Dispose()
        Catch ex As Exception
            MessageBox.Show("[GET_ALL_STAFF_ID] " & vbCrLf & ex.Message)
        End Try
        Return result
    End Function


    Public Function GET_STAFF_NAME(staff_id As String) As String
        Dim result As String = ""
        ce_query = "SELECT FULLNAME FROM EMPLOYEEPROFILES WHERE EMPID = '" & staff_id & "'"
        Try
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = sdr(0).ToString
            End If
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_STAFF_NAME] " & vbCrLf & ex.Message)
        End Try


        Return result
    End Function




    Public Function GET_STAFF_ID(staff_name As String) As String
        Dim result As String = ""
        ce_query = "SELECT EMPID FROM EMPLOYEEPROFILES WHERE FULLNAME = '" & staff_name & "'"
        Try
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = sdr(0).ToString
            End If
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_STAFF_ID] " & vbCrLf & ex.Message)
        End Try


        Return result
    End Function

    Public Function GET_ALL_KNOWN_DESIGNATION() As List(Of String)
        Dim result As New List(Of String)
        Try
            ce_query = "SELECT DISTINCT DESIGNATION FROM EMPLOYEEPROFILES ORDER BY DESIGNATION ASC"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            While sdr.Read
                result.Add(sdr(0).ToString)
            End While
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_ALL_KNOWN_DESIGNATION()] " & ex.Message)
        End Try
        Return result
    End Function


    Public Function GET_ALL_KNOWN_DEPARTMENT() As List(Of String)
        Dim result As New List(Of String)
        Try
            ce_query = "SELECT DISTINCT DEPARTMENTNAME FROM DEPARTMENTTABLE ORDER BY DEPARTMENTNAME ASC"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            While sdr.Read
                result.Add(sdr(0).ToString)
            End While
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_ALL_KNOWN_DEPARTMENT()] " & ex.Message)
        End Try
        Return result
    End Function











#End Region


#Region "LEAVE MANAGEMENT"

    Public Sub FILE_LEAVE(staff_id As String, leave_id As Integer, workday As Date, file_by As String)
        Try
            ce_query = "INSERT INTO FILEDLEAVETABLE([STAFFID],[LEAVEID],[WORKDATE],[FILEDDATE],APPROVEDBY) VALUES(@stfid,@lvid,@dt,@fldt,@aprvby)"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            With ce_cmd
                .Parameters.Add(New SqlCeParameter("@stfid", staff_id))
                .Parameters.Add(New SqlCeParameter("@lvid", leave_id))
                .Parameters.Add(New SqlCeParameter("@dt", workday.ToShortDateString))
                .Parameters.Add(New SqlCeParameter("@fldt", DateAndTime.Now.ToShortDateString))
                .Parameters.Add(New SqlCeParameter("@aprvby", file_by))
                .ExecuteNonQuery()
            End With
            'ce_cmd.Dispose()
        Catch ex As Exception
            MessageBox.Show("[FILE_LEAVE()]" & vbCrLf & ex.Message)
        End Try
    End Sub
    Public Sub REMOVE_FILED_LEAVE(staffid As String, workday As Date)

        Try
            ce_query = "DELETE FROM FILEDLEAVETABLE WHERE (STAFFID = '" & staffid & "') AND (WORKDATE = '" & workday.ToShortDateString & "')"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            ce_cmd.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show("[REMOVE_FILED_LEAVE()] " & vbCrLf & ex.Message)
        End Try

    End Sub
    Public Function COUNT_TOTAL_FILED_LEAVE(staff_id As String, leave_id As Integer) As Integer
        Dim result As Integer = 0
        Try

            ce_query = "SELECT COUNT(*) as TOTALFILED FROM FILEDLEAVETABLE WHERE (STAFFID = '" & staff_id & "') AND (LEAVEID = '" & leave_id & "')"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = sdr("TOTALFILED")
            End If
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[COUNT_TOTAL_FILED_LEAVE()] " & vbCrLf & ex.Message)
        End Try
        Return result
    End Function

    Public Function GET_LEAVEID_MAX_COUNT(leave_id As Integer) As Integer
        Dim result As Integer = 0
        Try

            ce_query = "SELECT [ALLOWEDTOTAL] FROM LEAVECLASSTABLE WHERE ID = '" & leave_id & "'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = sdr(0)
            End If
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_LEAVEID_MAX_COUNT()] " & vbCrLf & ex.Message)
        End Try
        Return result


    End Function

    Public Function GET_LEAVECLASS_ID(leave_name As String) As Integer
        Dim result As Integer = Nothing
        Try

            ce_query = "SELECT [ID] FROM LEAVECLASSTABLE WHERE NAME = '" & leave_name & "'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = sdr(0)
            End If
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_LEAVECLASS_ID()] " & vbCrLf & ex.Message)
        End Try

        Return result
    End Function

    Public Function CHECK_IFPAID_LEAVE(leave_id As Integer) As Integer
        Dim result As Boolean = False
        Try

            ce_query = "SELECT * FROM LEAVECLASSTABLE WHERE ID = '" & leave_id & "' AND PAIDLEAVE = 'TRUE'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = True
            End If
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[CHECK_IFPAID_LEAVE()] " & vbCrLf & ex.Message)
        End Try

        Return result
    End Function

    Public Function CHECK_IF_LEAVE_HASALREADY_FILED(staffid As String, filed_date As Date) As Integer
        Dim result As Boolean = False
        Try

            ce_query = "SELECT * FROM FILEDLEAVETABLE WHERE STAFFID = '" & staffid & "' AND WORKDATE = '" & filed_date.ToShortDateString & "'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = True
            End If
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[CHECK_IF_LEAVE_HASALREADY_FILED()] " & vbCrLf & ex.Message)
        End Try

        Return result
    End Function
    Public Sub UPDATE_FILED_LEAVE(staffid As String, file_date As Date, workdate As Date, leave_id As Integer, approvedby As String)
        ce_query = "UPDATE FILEDLEAVETABLE SET LEAVEID = @inleaveid, FILEDDATE = @infileddate, APPROVEDBY = @inapprovedby WHERE STAFFID = @instaffid AND WORKDATE = @inworkdate"
        ' cqueryLoc = "INSERT INTO ATTENDANCETABLE([STAFFID],[STAFFNAME],[WORKDATE],[WEEK],[DTRREMARKS],[MONTHLYREMARKS],[SHIFTNAME]) VALUES(@stfid,@stfnme,@wrkDte,@wk,@dtrrmrks,@mnthlyrmrks,@shftnme)"
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        Try
            With ce_cmd
                .Parameters.Add(New SqlCeParameter("@instaffid", staffid))
                .Parameters.Add(New SqlCeParameter("@inleaveid", leave_id))
                .Parameters.Add(New SqlCeParameter("@infileddate", file_date.ToShortDateString))
                .Parameters.Add(New SqlCeParameter("@inworkdate", workdate))
                .Parameters.Add(New SqlCeParameter("@inapprovedby", approvedby))
                .ExecuteNonQuery()
            End With
            'ce_cmd.Dispose()
        Catch ex As Exception
            MessageBox.Show("[UPDATE_FILED_LEAVE()]" & vbCrLf & ex.Message)
        End Try
    End Sub






    Public Function GET_ALL_LEAVE_NAME() As List(Of String)
        Dim result As New List(Of String)
        'SELECT DISTINCT [Department] FROM [EmployeeProfiles]
        ce_query = "SELECT [NAME] FROM LEAVECLASSTABLE"
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
        While sdr.Read
            result.Add(sdr(0).ToString)
        End While
        sdr.Close()
        Return result
    End Function

#End Region





#Region "SHIFT MANAGEMENT"


    Public Function GET_SHIFTID(shift_name As String) As Integer
        Dim result As Integer = 0
        Try
            ce_query = "SELECT SHIFTID FROM SHIFTTABLE WHERE SHIFTNAME = '" & shift_name & "'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = sdr(0)
            End If
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_SHIFTID()] " & vbCrLf & ex.Message)
        End Try
        Return result
    End Function
    Public Function GET_INTERVAL_OF_LEAVING_COUNT_AS_OT(shiftid As Integer) As Integer
        Dim result As Integer = 0
        Try
            ce_query = "SELECT INTERVALOFLEAVINGCOUNTASOT FROM SHIFTTABLE WHERE SHIFTID = '" & shiftid & "'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            'If sdr.Read Then
            '    result = sdr(0)
            'End If

            If sdr.Read Then
                If Not sdr.IsDBNull(sdr.GetOrdinal("INTERVALOFLEAVINGCOUNTASOT")) Then
                    result = sdr(0)
                End If
            End If

            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_INTERVAL_OF_LEAVING_COUNT_AS_OT()] " & vbCrLf & ex.Message)
        End Try



        Return result
    End Function




    Public Function GET_ALL_SHIFTNAME() As List(Of String)
        Dim result As New List(Of String)
        Try
            ce_query = "SELECT SHIFTNAME FROM SHIFTTABLE"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            While sdr.Read
                result.Add(sdr(0))
            End While
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_ALL_SHIFTNAME()] " & vbCrLf & ex.Message)
        End Try
        Return result
    End Function

    Public Function GET_SHIFT_TIMETABLEIDS(shift_id As Integer) As List(Of Integer)
        Dim result As New List(Of Integer)
        Try
            'If 2dayshift = True Then
            ce_query = "SELECT TIMETABLEID FROM SHIFTSCHEMATABLE WHERE SHIFTID = '" & shift_id & "'"
            'End If
            'If OT = True Then
            '    ' ce_query = "SELECT TIMETABLEID FROM SHIFTSCHEMATABLE WHERE SHIFTID = '" & shift_id & "' AND REGULAR = '" & Regular & "'"
            'End If


            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            While sdr.Read
                result.Add(sdr(0))
            End While
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_SHIFT_TIMETABLEIDS()] " & vbCrLf & ex.Message)
        End Try
        Return result

    End Function
    Public Function CHECK_AUTOPILOT(timetable_id As Integer) As Boolean
        Dim result As Boolean = False
        Try
            ce_query = "SELECT AUTOPILOT FROM SHIFTTABLE WHERE SHIFTID = '" & timetable_id & "' AND AUTOPILOT = 'TRUE'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = True
            End If
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[CHECK_AUTOPILOT()] " & vbCrLf & ex.Message)
        End Try
        Return result


    End Function
    Public Function DETERMINE_TWO_DAY_SHIFT(timetable_id As Integer) As Boolean
        Dim result As Boolean = False
        Try
            ce_query = "SELECT * FROM SHIFTSCHEMATABLE WHERE TIMETABLEID = '" & timetable_id & "' AND TWODAYSHIFT = 'TRUE'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = True
            End If
            sdr.Dispose()
            ce_cmd.Dispose()
            GC.Collect()
        Catch ex As Exception
            MessageBox.Show("[DETERMINE_TWO_DAY_SHIFT()] " & vbCrLf & ex.Message)
        End Try
        Return result


    End Function

#End Region







#Region "SHIFT SCHEMA POLLING"

    Public Function GET_BEGINING_IN(timetable_id As Integer) As String
        Dim result As String = ""

        Try
            ce_query = "SELECT BEGININGIN FROM SHIFTSCHEMATABLE WHERE TIMETABLEID = '" & timetable_id & "'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = sdr(0).ToString
            End If
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_BEGINING_IN()] " & vbCrLf & ex.Message)
        End Try
        Return result
    End Function

    Public Function GET_ENDING_IN(timetable_id As Integer) As String
        Dim result As String = ""

        Try
            ce_query = "SELECT ENDINGIN FROM SHIFTSCHEMATABLE WHERE TIMETABLEID = '" & timetable_id & "'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = sdr(0).ToString
            End If
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_ENDING_IN()] " & vbCrLf & ex.Message)
        End Try
        Return result
    End Function

    Public Function GET_BEGINING_OUT(timetable_id As Integer) As String
        Dim result As String = ""

        Try
            ce_query = "SELECT BEGININGOUT FROM SHIFTSCHEMATABLE WHERE TIMETABLEID = '" & timetable_id & "'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = sdr(0).ToString
            End If
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_BEGINING_OUT()] " & vbCrLf & ex.Message)
        End Try
        Return result
    End Function

    Public Function GET_ENDING_OUT(timetable_id As Integer) As String
        Dim result As String = ""

        Try
            ce_query = "SELECT ENDINGOUT FROM SHIFTSCHEMATABLE WHERE TIMETABLEID = '" & timetable_id & "'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = sdr(0).ToString
            End If
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_ENDING_OUT()] " & vbCrLf & ex.Message)
        End Try
        Return result
    End Function

    Public Function GET_ON_DUTY_TIME(timetable_id As Integer) As String
        Dim result As String = ""
        Try
            ce_query = "SELECT ONDUTYTIME FROM SHIFTSCHEMATABLE WHERE TIMETABLEID = '" & timetable_id & "'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = sdr(0).ToString
            End If
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_ON_DUTY_TIME()] " & vbCrLf & ex.Message)
        End Try

        Return result
    End Function

    Public Function GET_OFF_DUTY_TIME(timetable_id As Integer) As String
        Dim result As String = ""
        Try
            ce_query = "SELECT OFFDUTYTIME FROM SHIFTSCHEMATABLE WHERE TIMETABLEID = '" & timetable_id & "'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = sdr(0).ToString
            End If
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_OFF_DUTY_TIME()] " & vbCrLf & ex.Message)
        End Try

        Return result
    End Function

    Public Function GET_TOTAL_WORKING_HOURS(timetable_id As Integer) As Integer
        Dim result As Integer = 0
        Try
            ce_query = "SELECT WORKINGHOURS FROM SHIFTSCHEMATABLE WHERE TIMETABLEID = '" & timetable_id & "'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = sdr(0)
            End If
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_TOTAL_WORKING_HOURS()] " & vbCrLf & ex.Message)
        End Try

        Return result
    End Function

    Public Function CHECK_MUST_IN(timetable_id As Integer) As Boolean
        Dim result As Boolean = False
        Try
            ce_query = "SELECT * FROM SHIFTSCHEMATABLE WHERE TIMETABLEID = '" & timetable_id & "' AND  (MUSTIN = 'TRUE')"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = True
            End If
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[CHECK_MUST_IN()] " & vbCrLf & ex.Message)
        End Try

        Return result
    End Function

    Public Function CHECK_MUST_OUT(timetable_id As Integer) As Boolean
        Dim result As Boolean = False
        Try
            ce_query = "SELECT * FROM SHIFTSCHEMATABLE WHERE TIMETABLEID = '" & timetable_id & "' AND  (MUSTOUT = 'TRUE')"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = True
            End If
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[CHECK_MUST_OUT()] " & vbCrLf & ex.Message)
        End Try

        Return result
    End Function

#End Region



#Region "MODIFIED LOGS"

    Public Function CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id As String, s_date As Date, e_date As Date, log_time As String) As String

        Dim result As String = log_time



        If IsDate(log_time) Then
            log_time = Format(CDate(log_time), "HH:mm:ss")
            Console.WriteLine("CheckingIfModified: " & log_time)
            Try
                ce_query = "SELECT LOGTIME FROM TIMELOGS WHERE (STAFFID = '" & staff_id & "') AND (LOGTIME = '" & log_time & "') AND (DATE BETWEEN '" & s_date.ToShortDateString & "'  AND '" & e_date.ToShortDateString & "')  AND (ISADJUSTED = 'True')"
                ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
                If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
                Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
                If sdr.Read Then
                    result = "*" & Format(CDate(log_time), "hh:mmtt")
                    isModifiedLogs = True
                Else
                    result = Format(CDate(log_time), "hh:mmtt")
                End If
                sdr.Close()
            Catch ex As Exception
                MessageBox.Show("[CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER()] " & vbCrLf & ex.Message)
            End Try
        End If
        Return result
    End Function

    Public Function CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER_AS_BOOLEAN(staff_id As String, s_date As Date, e_date As Date, log_time As String) As String

        Dim result As Boolean = False
        log_time = Format(CDate(log_time), "HH:mm:ss")
        Console.WriteLine("CheckingIfModifiedAsBoolean: " & log_time)
        Try
            ce_query = "SELECT LOGTIME FROM TIMELOGS WHERE (STAFFID = '" & staff_id & "') AND (LOGTIME = '" & log_time & "') AND (DATE BETWEEN '" & s_date.ToShortDateString & "'  AND '" & e_date.ToShortDateString & "')  AND (ISADJUSTED = 'True')"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = True
            End If
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER_AS_BOOLEAN()] " & vbCrLf & ex.Message)
        End Try
        Return result
    End Function



#End Region


#Region "BISBIO DEVICES MANAGEMENT"

    Public Function GET_ALL_BISBIO_DEVICES() As List(Of String)
        Dim result As New List(Of String)
        Try
            ce_query = "SELECT DEVICENAME FROM DEVICES"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            While sdr.Read
                result.Add(sdr(0).ToString)
            End While
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_ALL_BISBIO_DEVICES()] " & vbCrLf & ex.Message)
        End Try
        Return result
    End Function
    Public Function GET_DETAILS_DEVICE_IP(dev_name As String) As String
        Dim result As String = ""
        Try
            ce_query = "SELECT IPADDRESS FROM DEVICES WHERE DEVICENAME = '" & dev_name & "'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            While sdr.Read
                result = sdr(0).ToString
            End While
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_DETAILS_DEVICE_IP()] " & vbCrLf & ex.Message)
        End Try
        Return result
    End Function


    Public Function GET_DETAILS_DEVICE_PORT(dev_name As String) As String
        Dim result As String = ""
        Try
            ce_query = "SELECT PORT FROM DEVICES WHERE DEVICENAME = '" & dev_name & "'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            While sdr.Read
                result = sdr(0).ToString
            End While
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_DETAILS_DEVICE_PORT()] " & vbCrLf & ex.Message)
        End Try
        Return result
    End Function


  


#End Region


#Region "MANAGE TRAVEL ORDER"
    'Public Sub INSERT_NEW_MTO()
    '    Try
    '        ce_query = "INSERT INTO TRAVELORDER"

    '    Catch ex As Exception
    '        MessageBox.Show("INSERT_NEW_MTO() " & ex.Message)
    '    End Try
    'End Sub

 
    Public Function GET_ALL_KNOWN_MTO_PURPOSES() As List(Of String)
        Dim result As New List(Of String)
        Try
            ce_query = "SELECT DISTINCT PURPOSE FROM FILEDTRAVELORDER ORDER BY PURPOSE ASC"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            While sdr.Read
                result.Add(sdr(0).ToString)
            End While
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_KNOWN_MTO_PURPOSES()] " & ex.Message)
        End Try
        Return result
    End Function


    Public Function GET_ALL_KNOWN_MTO_LOCATION() As List(Of String)
        Dim result As New List(Of String)
        Try
            ce_query = "SELECT DISTINCT LOCATION FROM FILEDTRAVELORDER ORDER BY LOCATION ASC"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            While sdr.Read
                result.Add(sdr(0).ToString)
            End While
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_ALL_KNOWN_MTO_LOCATION()] " & ex.Message)
        End Try
        Return result
    End Function

    Public Function CHECK_FILED_MTO(staffid As String, workdate As Date) As Boolean
        Dim result As Boolean = False
        Try
            Console.Write("Checking filed MTO for date: " & workdate)
            ce_query = "SELECT * FROM FILEDTRAVELORDER WHERE STAFFID = '" & staffid & "'  AND WORKDATE = '" & workdate.ToShortDateString & "'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                Console.Write(">>YES")
                result = True
            Else
                Console.Write(">>NO")
            End If
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[CHECK_FILED_MTO()] " & ex.Message)
        End Try
        Return result
    End Function
    Public Function GET_FILED_MTO_ID(staffid As String, workdate As Date) As Integer
        Dim result As Integer = 0
        ce_query = "SELECT TRAVELORDERCLASSTABLE.TROID FROM FILEDTRAVELORDER INNER JOIN TRAVELORDERCLASSTABLE ON FILEDTRAVELORDER.TRAVELORDERID = TRAVELORDERCLASSTABLE.TROID WHERE (FILEDTRAVELORDER.STAFFID = '" & staffid & "') AND (FILEDTRAVELORDER.WORKDATE = '" & workdate.ToShortDateString & "')"

        Try
            ' ce_query = "SELECT * FROM FILEDTRAVELORDER WHERE STAFFID = '" & staffid & "'  AND DATE = '" & workdate.ToShortDateString & "'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = sdr(0)
            End If
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_FILED_MTO_ID()] " & ex.Message)
        End Try

        Return result
    End Function
    Public Function GET_MTO_PURPOSE(staffid As String, workdate As Date) As String
        Dim result As String = ""

        Try
            ce_query = "SELECT PURPOSE FROM FILEDTRAVELORDER WHERE STAFFID = '" & staffid & "' AND WORKDATE = '" & workdate.ToShortDateString & "'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = sdr(0)
            End If
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_MTO_PURPOSE()] " & ex.Message)
        End Try


        Return result
    End Function

    Public Function GET_MTO_SYMBOL(trave_order_id As Integer) As String
        Dim result As String = ""

        Try
            ce_query = "SELECT SYMBOL FROM TRAVELORDERCLASSTABLE WHERE TROID = '" & trave_order_id & "'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = sdr(0)
            End If
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_MTO_SYMBOL()] " & ex.Message)
        End Try


        Return result
    End Function

    Public Function CHECK_IF_PAID_MTO(travel_order_id As Integer) As Boolean
        Dim result As Boolean

        ce_query = "SELECT * FROM TRAVELORDERCLASSTABLE WHERE TROID = '" & travel_order_id & "' AND PAID = 'True'"

        Try
            ' ce_query = "SELECT * FROM FILEDTRAVELORDER WHERE STAFFID = '" & staffid & "'  AND DATE = '" & workdate.ToShortDateString & "'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = True
            End If
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[CHECK_IF_PAID_MTO()] " & ex.Message)
        End Try




        Return result
    End Function
    Public Function GET_ALL_TRAVEL_ORDER_NAMES() As List(Of String)
        Dim result As New List(Of String)
        Try
            ce_query = "SELECT DEFINITION FROM TRAVELORDERCLASSTABLE"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            While sdr.Read
                result.Add(sdr(0).ToString)
            End While
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_ALL_TRAVEL_ORDER_NAMES()] " & ex.Message)
        End Try
        Return result
    End Function


    Public Sub FILE_MTO(staffid As String, travelorder_id As Integer, workdate As Date, filed_date As Date, filedby As String, location As String, purpose As String)

        '    ce_query = "INSERT INTO ATTENDANCETABLE([STAFFID],[STAFFNAME],[WORKDATE],[WEEK],[DTRREMARKS],[MONTHLYREMARKS],[SHIFTNAME],[SHIFTID]) VALUES(@stfid,@stfnme,@wrkDte,@wk,@dtrrmrks,@mnthlyrmrks,@shftnme,@shftid)"


        Try
            ce_query = "INSERT INTO FILEDTRAVELORDER([TRAVELORDERID],[STAFFID],[WORKDATE],[FILEDDATE],[FILEDBY],[LOCATION],[PURPOSE]) VALUES(@intravelorderid,@instaffid,@inworkdate,@infileddate,@infiledby,@inlocation,@inpurpose)"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            With ce_cmd
                .Parameters.Add(New SqlCeParameter("@instaffid", staffid))
                .Parameters.Add(New SqlCeParameter("@intravelorderid", travelorder_id))
                .Parameters.Add(New SqlCeParameter("@inworkdate", workdate.ToShortDateString))
                .Parameters.Add(New SqlCeParameter("@infileddate", filed_date.ToShortDateString))
                .Parameters.Add(New SqlCeParameter("@infiledby", filedby))
                .Parameters.Add(New SqlCeParameter("@inlocation", location))
                .Parameters.Add(New SqlCeParameter("@inpurpose", purpose))
                .ExecuteNonQuery()
            End With


        Catch ex As Exception
            MessageBox.Show("[FILE_MTO()] " & ex.Message)
        End Try

    End Sub

    Public Function VERIFY_MTO_IF_FILED_ALREADY(staffid As String, workdate As Date) As Boolean
        Dim result As Boolean = False


        Try
            ce_query = "SELECT * FROM FILEDTRAVELORDER WHERE STAFFID = '" & staffid & "' AND WORKDATE = '" & workdate.ToShortDateString & "'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read = True Then
                result = True
            End If
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[VERIFY_MTO_IF_FILED_ALREADY()] " & vbCrLf & ex.Message)
        End Try


        Return result
    End Function
    Public Sub REMOVE_FILED_MTO(staffid As String, workdate As Date)
        Try
            ce_query = "DELETE FROM FILEDTRAVELORDER WHERE STAFFID=@instaffid AND WORKDATE=@inworkdate"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            With ce_cmd
                .Parameters.Add(New SqlCeParameter("@instaffid", staffid))
                .Parameters.Add(New SqlCeParameter("@inworkdate", workdate.ToShortDateString))
                .ExecuteNonQuery()
            End With
        Catch ex As Exception
            MessageBox.Show("[REMOVE_FILED_MTO(staffid,date)] " & vbCrLf & ex.Message)
        End Try

    End Sub
    Public Function GET_TRAVEL_ORDER_ID(travel_order_name As String) As Integer
        Dim result As Integer = 0


        Try
            ce_query = "SELECT TROID FROM TRAVELORDERCLASSTABLE WHERE DEFINITION = '" & travel_order_name & "'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read = True Then
                result = sdr(0)
            End If
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_TRAVEL_ORDER_ID()] " & vbCrLf & ex.Message)
        End Try
        Return result
    End Function
    Public Sub UPDATE_FILED_MTO(staffid As String, travel_id As Integer, workdate As Date, fileddate As Date, filedby As String, location As String, purpose As String)

        Try
            ce_query = "UPDATE FILEDTRAVELORDER SET TRAVELORDERID = @intravelorderid,FILEDDATE = @infileddate,FILEDBY = @infiledby,LOCATION = @inlocation, PURPOSE = @inpurpose WHERE STAFFID = @instaffid AND WORKDATE = @inworkdate"

            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            With ce_cmd
                .Parameters.Add(New SqlCeParameter("@instaffid", staffid))
                .Parameters.Add(New SqlCeParameter("@intravelorderid", travel_id))
                .Parameters.Add(New SqlCeParameter("@inworkdate", workdate.ToShortDateString))
                .Parameters.Add(New SqlCeParameter("@infileddate", fileddate.ToShortDateString))
                .Parameters.Add(New SqlCeParameter("@infiledby", filedby))
                .Parameters.Add(New SqlCeParameter("@inlocation", location))
                .Parameters.Add(New SqlCeParameter("@inpurpose", purpose))
                .ExecuteNonQuery()
            End With
            'ce_cmd.Dispose()


        Catch ex As Exception
            MessageBox.Show("[UPDATE_FILED_MTO()] " & ex.Message)
        End Try







        'Try

        '    ce_query = "UPDATE ATTENDANCETABLE SET SHIFTNAME = @shftnme, DTRREMARKS = @dtrrmrks WHERE STAFFID = @stfid AND WORKDATE = @wrkdate"
        '    ' cqueryLoc = "INSERT INTO ATTENDANCETABLE([STAFFID],[STAFFNAME],[WORKDATE],[WEEK],[DTRREMARKS],[MONTHLYREMARKS],[SHIFTNAME]) VALUES(@stfid,@stfnme,@wrkDte,@wk,@dtrrmrks,@mnthlyrmrks,@shftnme)"
        '    ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        '    If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        '    With ce_cmd
        '        .Parameters.Add(New SqlCeParameter("@stfid", staffid))
        '        .Parameters.Add(New SqlCeParameter("@wrkdate", dt.ToShortDateString))
        '        .Parameters.Add(New SqlCeParameter("@dtrrmrks", rmrks)) '@mnthlyrmrks
        '        .Parameters.Add(New SqlCeParameter("@mnthlyrmrks", rmrks)) '@mnthlyrmrks
        '        .Parameters.Add(New SqlCeParameter("@shftnme", shiftname)) '@mnthlyrmrks
        '        .Parameters.Add(New SqlCeParameter("@shftid", shift_id)) '@mnthlyrmrks
        '        .ExecuteNonQuery()
        '    End With
        ' 'ce_cmd.Dispose()
        'Catch ex As Exception
        '    MessageBox.Show("[UpdateStaffSched()]" & vbCrLf & ex.Message)


    End Sub
#End Region



#Region "Master file mass upload"
    Public Sub CLEAR_MASTERLIST_TABLE()
        ce_query = "DELETE FROM STAFFMASTERLIST"

        Try
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            ce_cmd.ExecuteNonQuery()

        Catch ex As Exception
            MessageBox.Show("[CLEAR_MASTERLIST_TABLE()]" & vbCrLf & ex.Message)
        End Try
    End Sub
    Public Sub INSERT_TO_STAFF_MASTERFILE(staffid As String, staffname As String, departmentname As String, designation As String)
        ce_query = "INSERT INTO STAFFMASTERLIST(ID,NAME,DEPARTMENT, DESIGNATION) VALUES(@instaffid, @instaffname,@indepartmentname, @inDesignation)"

        Try
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            With ce_cmd
                .Parameters.Add(New SqlCeParameter("@instaffid", staffid))
                .Parameters.Add(New SqlCeParameter("@instaffname", staffname))
                .Parameters.Add(New SqlCeParameter("@indepartmentname", departmentname))
                .Parameters.Add(New SqlCeParameter("@inDesignation", designation))
                .ExecuteNonQuery()
            End With
        Catch ex As Exception
            MessageBox.Show("[INSERT_TO_STAFF_MASTERFILE()]" & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Function GET_ALL_DEPARTMENT_NAME_FROM_MASTERFILE_TABLE() As List(Of String)
        Dim result As New List(Of String)
        Try
            ce_query = "SELECT DISTINCT DEPARTMENT FROM STAFFMASTERLIST"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            While sdr.Read
                result.Add(sdr(0).ToString)
            End While
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_ALL_DEPARTMENT_NAME_FROM_MASTERFILE_TABLE()] " & ex.Message)
        End Try
        Return result
    End Function

    Public Function GET_ALL_STAFFID_FROM_MASTERFILE_TABLE() As List(Of String)
        Dim result As New List(Of String)
        Try
            ce_query = "SELECT DISTINCT ID FROM STAFFMASTERLIST"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            While sdr.Read
                result.Add(sdr(0).ToString)
            End While
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_ALL_STAFFID_FROM_MASTERFILE_TABLE()] " & ex.Message)
        End Try
        Return result
    End Function

    Public Function GET_STAFFNAME_FROM_MASTERFILE_TABLE(staffid As String) As String
        Dim result As String = ""
        Try
            ce_query = "SELECT NAME FROM STAFFMASTERLIST WHERE ID = '" & staffid & "'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            While sdr.Read
                result = sdr(0).ToString
            End While
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_STAFFNAME_FROM_MASTERFILE_TABLE()] " & ex.Message)
        End Try
        Return result
    End Function

    Public Function GET_STAFF_DESIGNATION_FROM_MASTERFILE_TABLE(staffid As String) As String
        Dim result As String = ""
        Try
            ce_query = "SELECT DESIGNATION FROM STAFFMASTERLIST WHERE ID = '" & staffid & "'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            While sdr.Read
                result = sdr(0).ToString
            End While
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_STAFFNAME_FROM_MASTERFILE_TABLE()] " & ex.Message)
        End Try
        Return result
    End Function

    Public Function GET_ASSIGN_DEPARTMENT_FROM_MASTERFILE_TABLE(staffid As String) As String
        Dim result As String = ""
        Try
            ce_query = "SELECT DEPARTMENT FROM STAFFMASTERLIST WHERE ID = '" & staffid & "'"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            While sdr.Read
                result = sdr(0).ToString
            End While
            sdr.Close()
        Catch ex As Exception
            MessageBox.Show("[GET_STAFFNAME_FROM_MASTERFILE_TABLE()] " & ex.Message)
        End Try
        Return result
    End Function
#End Region


#Region "BTMS SCHEMA"

    Public Sub BTMS_CLEAR_ATTENDANCETABLE()
        Try
            ce_query = "DELETE FROM ATTENDANCETABLE"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            ce_cmd.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show("[BTMS_CLEAR_ATTENDANCETABLE()] " & ex.Message)
        End Try
    End Sub

    Public Sub BTMS_RESET_TIMELOGS_SEED()
        ce_query = "ALTER TABLE TIMELOGS ALTER COLUMN RAWID IDENTITY (1,1)"
        Try

            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            ce_cmd.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show("[BTMS_RESET_TIMELOGS_SEED()] " & ex.Message)
        End Try


    End Sub



    Public Sub BTMS_CLEAR_TIMELOGS(ALL As Boolean, s_date As Date, e_date As Date)
        Try
            If ALL = True Then
                ce_query = "DELETE FROM TIMELOGS"
            ElseIf ALL = False Then
                ce_query = "DELETE FROM TIMELOGS WHERE DATE BETWEEN '" & s_date.ToShortDateString & "' AND '" & e_date.ToShortDateString & "'"
            End If
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            ce_cmd.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show("[BTMS_CLEAR_TIMELOGS()] " & ex.Message)
        End Try
    End Sub
    Public Sub BTMS_CLEAR_EMPLOYEEPROFILES()
        Try
            ce_query = "DELETE FROM EMPLOYEEPROFILES"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            ce_cmd.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show("[BTMS_CLEAR_EMPLOYEEPROFILES()] " & ex.Message)
        End Try
    End Sub
    Public Sub BTMS_CLEAR_FILED_TRAVEL_ORDER()
        Try
            ce_query = "DELETE FROM FILEDTRAVELORDER"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            ce_cmd.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show("[BTMS_CLEAR_FILED_TRAVEL_ORDER()] " & ex.Message)
        End Try
    End Sub

    Public Sub BTMS_CLEAR_FILED_LEAVE()
        Try
            ce_query = "DELETE FROM FILEDLEAVETABLE"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            ce_cmd.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show("[BTMS_CLEAR_FILED_LEAVE()] " & ex.Message)
        End Try
    End Sub
#End Region



#Region "PRODUCT ACTIVATION"


    Public Sub BTMS_ACTIVATE_PRODUCT(product_key As String, activation_date As Date)

        Try
            ce_query = "INSERT INTO CHAVEDOPRODUTO(PRODUCTKEY,DATE) VALUES(@pkey,@acdate)"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            With ce_cmd
                .Parameters.Add(New SqlCeParameter("@pkey", product_key))
                .Parameters.Add(New SqlCeParameter("@acdate", activation_date))
                .ExecuteNonQuery()
            End With
            'ce_cmd.Dispose()
        Catch ex As Exception
            MessageBox.Show("[ACTIVATE_PRODUCT()]" & vbCrLf & ex.Message)
        End Try
    End Sub
    Public Sub BTMS_CLEAN_LICENSETABLE()

        Try
            ce_query = "DELETE FROM CHAVEDOPRODUTO"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            ce_cmd.ExecuteNonQuery()

            'ce_cmd.Dispose()
        Catch ex As Exception
            MessageBox.Show("[BTMS_CLEAN_LICENSETABLE()]" & vbCrLf & ex.Message)
        End Try
    End Sub
    Public Function BTMS_GET_PRODUCTKEY() As String
        Dim result As String = ""
        Try
            ce_query = "SELECT PRODUCTKEY FROM CHAVEDOPRODUTO"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = sdr(0).ToString
            End If
            ce_cmd.Dispose()
        Catch ex As Exception
            MessageBox.Show("[BTMS_GET_PRODUCTKEY()]" & vbCrLf & ex.Message)
        End Try
        Return result
    End Function


#End Region
#Region "AUDIT IT"
    Public Sub AUDIT_IT(current_user As String, task As String, date_time As Date)
        ce_query = "INSERT INTO SYSTEMTRACKS"


    End Sub
#End Region



#Region "DB TABLE MANAGEMENT"

    Public Function BMTS_CHECK_IF_TABLECOLUMN_IS_EXIST(tablename As String, columnname As String) As Boolean
        Dim result As Boolean = False
        Try
            ce_query = "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE (table_name = @pTable) AND (column_name = @pCol)"
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            With ce_cmd
                .Parameters.Add(New SqlCeParameter("@pTable", tablename))
                .Parameters.Add(New SqlCeParameter("@pCol", columnname))
            End With
            Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
            If sdr.Read Then
                result = True
            End If
        Catch ex As Exception
            MessageBox.Show("[BMTS_CHECK_IF_TABLECOLUMN_IS_EXIST()]" & vbCrLf & ex.Message)
        End Try
        Return result
    End Function

    Public Sub BTMS_CREATE_TABLE_COLUMN(tablename As String, columnname As String, variable_type As Integer)
        Try
            Dim var_type As String = ""
            Select Case variable_type
                Case 1
                    var_type = "nvarchar(100)"
                Case 2
                    var_type = "bit"
                Case 3
                    var_type = "bigint"
                Case 4
                    var_type = "nvarchar(300)"
            End Select


            ce_query = "ALTER TABLE " & tablename & " ADD COLUMN " & columnname & " " & var_type
            ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            ce_cmd.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show("[BTMS_CREATE_TABLE_COLUMN()]" & vbCrLf & ex.Message)
        End Try
    End Sub
#End Region





End Class
