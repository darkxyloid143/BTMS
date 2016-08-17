Imports System.Globalization

'####################################################################################
'       Project Name : BISBIO TIME MANAGEMENT SYSTEM
'        Class  Name : CLASSCALCULATE
' Class Discription  : Calculation of raw logs happens here!. All Calculated data will 
'                    : be inserted into GovEngine.sdf table "DTR".
'           Arguments: STAFF ID + DATE
'        Author Name : Brylle M. Lloren
'              Email : 
'          Copyright :  6-10-2015
'####################################################################################

Public Class CLASSCALCULATE

    Dim sql_worker As New SQLCE_MANAGER
    Dim cpu_worker As New CPU

    Dim staff_raw_logs As New List(Of String)
    Dim monthly_remarks As String = ""
    Dim dtr_remarks As String = ""
    Dim UNDERHH As String = ""
    Dim UNDERMM As String = ""
    'Dim OFFICIAL_ARRIVAL As String = ""
    'Dim OFFICIAL_DEPARTURE As String = ""
    Dim TotalUnderTime_daily As Double = 0
    Dim PUNCH1 As String = "NTR"
    Dim PUNCH2 As String = "NTR"
    Dim PUNCH3 As String = "NTR"
    Dim PUNCH4 As String = "NTR"
    Dim PUNCH5 As String = "NTR"
    Dim PUNCH6 As String = "NTR"



    Dim MTO_PURPOSE As String = ""
    Dim TOTAL_WORK_HOURS As Integer = 0
    Dim TOTAL_OVERTIME As Integer = 0
    Dim TOTAL_OT_DAILY_REMARKS As String = ""
    'Dim TOTAL_OTHH As String = ""
    'Dim TOTAL_OTMM As String = ""

    'Dim staffname As String = sql_worker.GET_STAFF_NAME(staff_id)
    Dim week As String = ""
    Dim staff_id As String = ""
    Dim staffname As String = ""
    Dim departmentname As String = ""
    Dim designation As String = ""
    Dim _day As Date
    Dim shift_id As Integer = 0
    Dim shiftname As String = ""
    Dim TimeTableList As New List(Of Integer)
    Dim autopilot As Boolean = False

    '''UNDERTIME AND LATE VARIABLES
    '''
    Dim Daily_UT As String = ""
    Dim Daily_LATE As String = ""
    Dim Daily_REMARKS As String = ""
    Dim Daily_UT_TOTAL As Integer = 0
    Dim Daily_LATE_TOTAL As Integer = 0
    Dim Daily_UT_TOTAL_MONTHLY As String = ""
    Dim Daily_LATE_TOTAL_MONTHLY As String = ""

    



    Public Sub MAIN(_staff_id As String, _staffname As String, _departmentname As String, __day As Date,
                    _shift_id As Integer, _shiftname As String, _TimeTableList As List(Of Integer),
                    _autopilot As Boolean, _designation As String)
        'Dim sql_worker As New SQLCE_MANAGER
        'Dim cpu_worker As New CPU
        'shift_id = sql_worker.GET_STAFF_SHIFT_ID(staff_id, _day)
        'shiftname = sql_worker.GET_STAFF_SHIFT_NAME(staff_id, _day)
        'TimeTableList = sql_worker.GET_SHIFT_TIMETABLEIDS(shift_id)
        'autopilot = sql_worker.CHECK_AUTOPILOT(shift_id)

        



        Daily_UT = ""
        Daily_LATE = ""
        Daily_REMARKS = ""
        Daily_UT_TOTAL = Nothing
        Daily_LATE_TOTAL = Nothing
        Daily_LATE_TOTAL_MONTHLY = ""
        Daily_UT_TOTAL_MONTHLY = ""

        'Dim shift_id As Integer = 0
        'Dim shiftname As String = ""
        'Dim TimeTableList As New List(Of Integer)
        'Dim autopilot As Boolean = False
        'TimeTableList = sql_worker.GET_SHIFT_TIMETABLEIDS(shift_id)
        'autopilot = sql_worker.CHECK_AUTOPILOT(shift_id)

        'shift_id = sql_worker.GET_STAFF_SHIFT_ID(staff_id, _day)
        'shiftname = sql_worker.GET_STAFF_SHIFT_NAME(staff_id, _day)
        'TimeTableList = sql_worker.GET_SHIFT_TIMETABLEIDS(shift_id)
        'autopilot = sql_worker.CHECK_AUTOPILOT(shift_id)


        ' Dim staff_raw_logs As New List(Of String)
        monthly_remarks = ""
        dtr_remarks = ""
        UNDERHH = ""
        UNDERMM = ""
        TotalUnderTime_daily = 0
        PUNCH1 = "NTR"
        PUNCH2 = "NTR"
        PUNCH3 = "NTR"
        PUNCH4 = "NTR"
        PUNCH5 = "NTR"
        PUNCH6 = "NTR"

        MTO_PURPOSE = ""
        TOTAL_WORK_HOURS = 0
        TOTAL_OVERTIME = 0
        TOTAL_OT_DAILY_REMARKS = ""


        staff_id = _staff_id
        staffname = _staffname
        departmentname = _departmentname
        designation = _designation
        _day = __day
        shift_id = _shift_id
        shiftname = _shiftname
        TimeTableList = _TimeTableList
        autopilot = _autopilot

        isModifiedLogs = False




        '  week = Format(_day, "dd") & " " & Format(_day, "ddd")
        Select Case GlobalVariables.DTR_TYPE.ToString
            Case "DTR"
                week = _day.ToString("d ddd", CultureInfo.InvariantCulture)
            Case "DTR_B"
                week = _day.ToString("d ddd", CultureInfo.InvariantCulture)
            Case "DTR_C"
                week = CInt(_day.ToString("dd", CultureInfo.InvariantCulture))
            Case "DTR_D"
                week = _day.ToString("d ddd", CultureInfo.InvariantCulture)
            Case "DTR_E"
                week = _day.ToString("d ddd", CultureInfo.InvariantCulture)
            Case "DTR_F"
                week = _day.ToString("d ddd", CultureInfo.InvariantCulture)
            Case "SROT"
                week = _day.ToString("d ddd", CultureInfo.InvariantCulture)
            Case "SRA"
                week = _day.ToString("d ddd", CultureInfo.InvariantCulture)
        End Select


        ''''GET OFFICIAL ARRIVAL AND OFFICIAL DEPARTURE
        If Not shiftname = "Off" Then
            'Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$" & TimeTableList.Count)
            Select Case TimeTableList.Count
                Case 1
                    GlobalVariables.OFFICIAL_ARRIVAL = Format(CDate(sql_worker.GET_ON_DUTY_TIME(TimeTableList(0))), "hh:mm tt")
                    GlobalVariables.OFFICIAL_DEPARTURE = Format(CDate(sql_worker.GET_OFF_DUTY_TIME(TimeTableList(0))), "hh:mm tt")
                Case 2
                    GlobalVariables.OFFICIAL_ARRIVAL = Format(CDate(sql_worker.GET_ON_DUTY_TIME(TimeTableList(0))), "hh:mm tt")
                    GlobalVariables.OFFICIAL_DEPARTURE = Format(CDate(sql_worker.GET_OFF_DUTY_TIME(TimeTableList(1))), "hh:mm tt")
            End Select
        End If







        'If sql_worker.CHECK_HOLIDAY(_day) = True Then
        '    'monthly_remarks = "Hol"
        '    'UNDERHH = sql_worker.GET_HOLIDAY_NAME(_day)
        '    TAG_AS_HOLIDAY()
        '    Exit Sub
        'End If



        If Not shiftname = "Off" Then
            '''=========================================================
            '''START CALCULATION WITH SCHEDULE
            '''=========================================================
            If TimeTableList.Count <> 0 And autopilot = False Then  'IF 0 VALUE THIS IS THE DAY OFF TAG'
                Console.WriteLine("SHIFTID: " & shift_id & " TIMETABLE COUNT: " & TimeTableList.Count)
                Select Case TimeTableList.Count
                    ''++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    Case 1  'EXPECT 2 LOGS  A DAY
                        ''++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                        If sql_worker.DETERMINE_TWO_DAY_SHIFT(TimeTableList(0)) = False Then
                            '''GOTO REGULAR 2 LOGS A DAY
                            Console.WriteLine("GOTO REGULAR 2 LOGS A DAY")

                            DAILY_2_LOGS()





                            ''++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                        Else ''EXPECT 2 LOGS ''TWO DAY SHIFT
                            ''++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                        End If

                        ''++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    Case 2                      'EXPECT 4 LOGS   (1 DAY SHIFT)
                        ''++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                        staff_raw_logs = sql_worker.GET_USER_RAWLOGS(staff_id, _day, _day)
                        staff_raw_logs.Sort()
                        monthly_remarks = ""
                        Console.WriteLine("EXPECT 4 LOGS   (1 DAY SHIFT)")
                        If sql_worker.DETERMINE_TWO_DAY_SHIFT(TimeTableList(0)) = False And sql_worker.DETERMINE_TWO_DAY_SHIFT(TimeTableList(1)) = False Then
                            If GlobalVariables.DTR_TYPE = "DTR_F" Then
                                ''TO DILG SAN FERNANDO SHIFT
                                '  MessageBox.Show(_day)
                                DAILY_4_LOGS_DILG_SAN_FERNANDO_LAUNION()

                                'Else
                                '    ''TO BIR SAN PABLO SHIFT 
                                '    ' MessageBox.Show("DAILY_4_LOGS")
                            Else
                                DAILY_4_LOGS()
                            End If
                            ''+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                        Else   ''4 LOGS TWO DAY SHIFT
                            ''+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                            ''TODO HERE TWO DAY SHIFT

                        End If


                        ''+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    Case 3 'EXPECT 6 LOGS A DAY
                        ''++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                End Select

            ElseIf TimeTableList.Count = 3 And autopilot = True Then ''THIS IS AN AUTOSCHED
 

                If My.Settings.dilg_rules_enabled = True Then
                    ''+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    ''THIS SECTION WILL DO WORK FOR AUTOSCHEDULING BASE ON TIMETABLE (CREATED FOR DILG GUARDS)
                    ''++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    ''
                    AUTO_SCHED_3()
                Else
                    ''+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    ''THIS SECTION WILL DO WORK FOR AUTOSCHEDULING BASE ON TIMETABLE (CREATED FOR BIR SAN PABLO FOR GUARDS)
                    ''++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    ' Console.WriteLine("3 SHIFT AUTOPILOT STAFFID: " & staff_id & " WORDATE: " & _day)
                    AUTO_SCHED_1()
                End If





            ElseIf TimeTableList.Count = 2 And autopilot = True Then
                '''+++++++++++++++++++++++++++++++++++
                '''POSIBLE DILG GUARD SCHEDULE
                '''NO CALCULATION OF LATE
                '''+++++++++++++++++++++++++++++++++++
                If sql_worker.DETERMINE_TWO_DAY_SHIFT(TimeTableList(0)) = False And sql_worker.DETERMINE_TWO_DAY_SHIFT(TimeTableList(1)) = True Then

                    '''PATROL 117 RULES
                    '''7AM-7PM
                    '''7PM-7AM
                    If GlobalVariables.DTR_TYPE = "DTR_E" Then
                        Console.WriteLine("Calling Function PATROL_117(" & _day.ToLongDateString & ")")
                        ''NOW CHECK FIRST IF THIS DAY IS ALREADY BEEN CALCULATED.
                        If Not sql_worker.CALCULATION_DONE(_day, staff_id) Then
                            PATROL_117_1A()
                        End If

                    Else
                        Console.WriteLine("START DAY " & _day.ToLongDateString)
                        AUTO_SCHED_2()
                        Console.WriteLine("END DAY " & _day.ToLongDateString)
                        Console.WriteLine("--------------------------------------------------------------------------------")

                    End If
                End If
            Else
                ''++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                ''THIS SECTION IS TAG AS NO SCHEDULE
                ''+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                Console.WriteLine("NO SCHEDULE DATED: " & _day & " STAFFID: " & staff_id)
                UNDERHH = "No-Schedule-Assign"
                Daily_REMARKS = "No-Schedule-Assign"
                monthly_remarks = ""
                PUNCH1 = ""
                PUNCH2 = ""
                PUNCH3 = ""
                PUNCH4 = ""

                sql_worker.INSERT_INTO_DTR_TABLE(staff_id, staffname, departmentname,
                                                  _day, PUNCH1, PUNCH2, PUNCH3, PUNCH4,
                                                  week, dtr_remarks, monthly_remarks, TotalUnderTime_daily, UNDERHH, UNDERMM,
                                                  TotalUnderTime_daily, TOTAL_OVERTIME, TOTAL_OT_DAILY_REMARKS,
                                                  GlobalVariables.OFFICIAL_ARRIVAL, GlobalVariables.OFFICIAL_DEPARTURE, Daily_UT, Daily_LATE, Daily_REMARKS,
                                                  Daily_LATE_TOTAL, Daily_UT_TOTAL, "", "", designation)


            End If
            ''=====================================================
            ''END OF CALCULATION OF SCHEDULE
            ''================================================

        Else
            ''++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ''THIS SECTION TAG AS DAY OFF
            ''+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            TAG_AS_DAYOFF()
        End If

JMPOUT:
        Daily_UT = ""
        Daily_LATE = ""
        Daily_REMARKS = ""
        Daily_UT_TOTAL = Nothing
        Daily_LATE_TOTAL = Nothing
        Daily_LATE_TOTAL_MONTHLY = ""
        Daily_UT_TOTAL_MONTHLY = ""



        PUNCH1 = Nothing
        PUNCH2 = Nothing
        PUNCH3 = Nothing
        PUNCH4 = Nothing
        PUNCH5 = Nothing
        PUNCH6 = Nothing


        staff_raw_logs.Clear()
        ''END OF TASK HERE
    End Sub

    Public Sub TAG_AS_DAYOFF()
        Dim breaktimerange As New List(Of String)
        Dim actualListOfmins As New List(Of String)
        Dim actuallistOfOTmins As New List(Of String)

        Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++")
        Console.WriteLine("DAYOFF DATED: " & _day & " STAFFID: " & staff_id)
        staff_raw_logs = sql_worker.GET_USER_RAWLOGS(staff_id, _day, _day)
        staff_raw_logs.Sort()

        Daily_LATE = ""
        Daily_UT = ""
        Daily_LATE_TOTAL = 0
        Daily_UT_TOTAL = 0

        If Not staff_raw_logs.Count = 0 Then
            staff_raw_logs.Sort()
            PUNCH1 = staff_raw_logs(0)
            PUNCH4 = staff_raw_logs(staff_raw_logs.Count - 1)

            ''CALCULATE OVERTIME

            actualListOfmins = cpu_worker.Extractminutes(PUNCH1, PUNCH4)
            breaktimerange = cpu_worker.Extractminutes("12:00", "12:59")
            Console.WriteLine("LIST OF BREAKTIME: " & String.Join("+", breaktimerange.ToArray))
            actualListOfmins.ForEach(Sub(min)
                                         If Not breaktimerange.Contains(min) Then
                                             actuallistOfOTmins.Add(min)
                                         End If
                                     End Sub)
            Console.WriteLine("ACTUAL RANGE: " & String.Join("+", actuallistOfOTmins.ToArray))
            Console.WriteLine("STAFFID: " & staff_id & " TOTAL OVERTIME(DAY-OFF): " & actuallistOfOTmins.Count)
            Console.WriteLine("ACTUALOTCOUNT: " & actualListOfmins.Count & " MINUS 1 HOUR LUNCH BREAK: " & breaktimerange.Count)
            TOTAL_OVERTIME = actuallistOfOTmins.Count
            ''FORMAT OVERTIME
            Dim Hours As Integer = Math.Floor(TOTAL_OVERTIME / 60)
            Dim Minutes As Integer = TOTAL_OVERTIME Mod 60

            If Hours <> 0 And Minutes = 0 Then
                TOTAL_OT_DAILY_REMARKS = Hours & "h"
            ElseIf Hours = 0 And Minutes <> 0 Then
                TOTAL_OT_DAILY_REMARKS = Minutes & "m"
            Else
                TOTAL_OT_DAILY_REMARKS = Hours & "h" & " " & Minutes & "m"
            End If
        End If


        ''''If sql_worker.CHECK_FILED_LEAVE(staff_id, _day) = True Then
        ''''    UNDERHH = sql_worker.GET_LEAVE_NAME(sql_worker.GET_FILED_LEAVEID(staff_id, _day))
        ''''    monthly_remarks = sql_worker.GET_LEAVE_SYMBOL(sql_worker.GET_FILED_LEAVEID(staff_id, _day))
        ''''End If



        ''''If Not PUNCH1 <> "NTR" And PUNCH4 <> "NTR" Then


        ''''End If



        ''SECTION DTR AND MONTHLY
        Select Case GlobalVariables.DTR_TYPE.ToString
            Case "DTR"
                UNDERHH = My.Settings.dtr_remarks_for_dayoff
            Case "DTR_B"
                UNDERHH = My.Settings.dtr_remarks_for_dayoff
            Case "DTR_C"
                UNDERHH = _day.ToString("dddd", CultureInfo.InvariantCulture).ToUpper
        End Select

        'Daily_REMARKS = _day.ToString("dddd", CultureInfo.InvariantCulture).ToUpper
        monthly_remarks = My.Settings.sra_remarks_for_dayoff



        ''STEP 3 DETERMINE HOLIDAY
        If sql_worker.CHECK_HOLIDAY(_day) = True Then
            monthly_remarks = "Hol"
            UNDERHH = sql_worker.GET_HOLIDAY_NAME(_day)
        End If


        ''FORMATING 
        If Not PUNCH1 = "NTR" Then
            PUNCH1 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, PUNCH1)
        Else
            PUNCH1 = ""
        End If
        If Not PUNCH2 = "NTR" Then
            PUNCH2 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, PUNCH2)
        Else
            PUNCH2 = ""
        End If
        If Not PUNCH3 = "NTR" Then
            PUNCH3 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, PUNCH3)
        Else
            PUNCH3 = ""
        End If
        If Not PUNCH4 = "NTR" Then
            PUNCH4 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, PUNCH4)
        Else
            PUNCH4 = ""
        End If

        sql_worker.INSERT_INTO_DTR_TABLE(staff_id, staffname, departmentname,
                                         _day, PUNCH1, PUNCH2, PUNCH3, PUNCH4,
                                         week, dtr_remarks, monthly_remarks, TotalUnderTime_daily, UNDERHH, UNDERMM, TotalUnderTime_daily,
                                         TOTAL_OVERTIME, TOTAL_OT_DAILY_REMARKS,
                                         GlobalVariables.OFFICIAL_ARRIVAL, GlobalVariables.OFFICIAL_DEPARTURE, Daily_UT, Daily_LATE, Daily_REMARKS,
                                         Daily_LATE_TOTAL, Daily_UT_TOTAL, "", "", designation)
        Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++")
    End Sub



    Public Sub TAG_AS_HOLIDAY()
        Dim breaktimerange As New List(Of String)
        Dim actualListOfmins As New List(Of String)
        Dim actuallistOfOTmins As New List(Of String)

        Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++")
        Console.WriteLine("HOLIDAY DATED: " & _day & " STAFFID: " & staff_id)
        staff_raw_logs = sql_worker.GET_USER_RAWLOGS(staff_id, _day, _day)
        staff_raw_logs.Sort()

        Daily_LATE = ""
        Daily_UT = ""
        Daily_LATE_TOTAL = 0
        Daily_UT_TOTAL = 0

        If Not staff_raw_logs.Count = 0 Then
            staff_raw_logs.Sort()
            PUNCH1 = staff_raw_logs(0)
            PUNCH4 = staff_raw_logs(staff_raw_logs.Count - 1)

            ''CALCULATE OVERTIME

            actualListOfmins = cpu_worker.Extractminutes(PUNCH1, PUNCH4)
            breaktimerange = cpu_worker.Extractminutes("12:00", "12:59")
            Console.WriteLine("LIST OF BREAKTIME: " & String.Join("+", breaktimerange.ToArray))
            actualListOfmins.ForEach(Sub(min)
                                         If Not breaktimerange.Contains(min) Then
                                             actuallistOfOTmins.Add(min)
                                         End If
                                     End Sub)
            Console.WriteLine("ACTUAL RANGE: " & String.Join("+", actuallistOfOTmins.ToArray))
            Console.WriteLine("STAFFID: " & staff_id & " TOTAL OVERTIME(DAY-OFF): " & actuallistOfOTmins.Count)
            Console.WriteLine("ACTUALOTCOUNT: " & actualListOfmins.Count & " MINUS 1 HOUR LUNCH BREAK: " & breaktimerange.Count)
            TOTAL_OVERTIME = actuallistOfOTmins.Count
            ''FORMAT OVERTIME
            Dim Hours As Integer = Math.Floor(TOTAL_OVERTIME / 60)
            Dim Minutes As Integer = TOTAL_OVERTIME Mod 60

            If Hours <> 0 And Minutes = 0 Then
                TOTAL_OT_DAILY_REMARKS = Hours & "h"
            ElseIf Hours = 0 And Minutes <> 0 Then
                TOTAL_OT_DAILY_REMARKS = Minutes & "m"
            Else
                TOTAL_OT_DAILY_REMARKS = Hours & "h" & " " & Minutes & "m"
            End If
        End If


        'If sql_worker.CHECK_FILED_LEAVE(staff_id, _day) = True Then
        '    UNDERHH = sql_worker.GET_LEAVE_NAME(sql_worker.GET_FILED_LEAVEID(staff_id, _day))
        '    monthly_remarks = sql_worker.GET_LEAVE_SYMBOL(sql_worker.GET_FILED_LEAVEID(staff_id, _day))
        'End If



        'If Not PUNCH1 <> "NTR" And PUNCH4 <> "NTR" Then
        'End If



        ''SECTION DTR AND MONTHLY
        UNDERHH = "Holiday-" & sql_worker.GET_HOLIDAY_NAME(_day)
        monthly_remarks = "Hol"
        Daily_REMARKS = sql_worker.GET_HOLIDAY_NAME(_day)


        ''STEP 3 DETERMINE HOLIDAY
        'If sql_worker.CHECK_HOLIDAY(_day) = True Then
        '    monthly_remarks = "Hol"
        '    UNDERHH = sql_worker.GET_HOLIDAY_NAME(_day)
        'End If


        ''FORMATING 
        If Not PUNCH1 = "" Then
            PUNCH1 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, PUNCH1)
        Else
            PUNCH1 = ""
        End If
        'If Not PUNCH2 = "NTR" Then
        '    PUNCH2 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, PUNCH2)
        'Else
        '    PUNCH2 = ""
        'End If
        'If Not PUNCH3 = "NTR" Then
        '    PUNCH3 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, PUNCH3)
        'Else
        '    PUNCH3 = ""
        'End If
        If Not PUNCH4 = "" Then
            PUNCH4 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, PUNCH4)
        Else
            PUNCH4 = ""
        End If


        TotalUnderTime_daily = 0

        sql_worker.INSERT_INTO_DTR_TABLE(staff_id, staffname, departmentname,
                                         _day, PUNCH1, PUNCH2, PUNCH3, PUNCH4,
                                         week, dtr_remarks, monthly_remarks, TotalUnderTime_daily, UNDERHH, UNDERMM, TotalUnderTime_daily,
                                         TOTAL_OVERTIME, TOTAL_OT_DAILY_REMARKS,
                                         GlobalVariables.OFFICIAL_ARRIVAL, GlobalVariables.OFFICIAL_DEPARTURE, Daily_UT, Daily_LATE, Daily_REMARKS,
                                         Daily_LATE_TOTAL, Daily_UT_TOTAL, "", "", designation)
        Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++")
    End Sub



    Private Sub DAILY_2_LOGS()

        staff_raw_logs = sql_worker.GET_USER_RAWLOGS(staff_id, _day, _day)
        'staff_raw_logs.Sort()
        monthly_remarks = ""


        ''' PUNCH1,PUNCH4
        Console.WriteLine("ID: " & staff_id & " Date: " & _day & " LOGS: " & String.Join(" + ", staff_raw_logs))
        ''STEP 1 GET ALL PUNCHES

        PUNCH1 = cpu_worker.GET_ACTUALTIME(staff_raw_logs, sql_worker.GET_BEGINING_IN(TimeTableList(0)), sql_worker.GET_ENDING_IN(TimeTableList(0)), True)
        PUNCH4 = cpu_worker.GET_ACTUALTIME(staff_raw_logs, sql_worker.GET_BEGINING_OUT(TimeTableList(0)), sql_worker.GET_ENDING_OUT(TimeTableList(0)), False)

        PUNCH2 = ""
        PUNCH3 = ""



        ''IF NO OUT CHECK THE NEXT DAY OUT POSIBLE
        ''STAFF MUST USE PHYSICAL IN/OUT BUTTON TO DETERMINE THE ACTUAL OUT OF THE STAFF.
        If PUNCH4 = "NTR" And PUNCH1 <> "NTR" And My.Settings.dilg_rules_enabled = 1 Then
            Dim Pout As String = ""
            Pout = sql_worker.GET_COUT(staff_id, _day.AddDays(1), _day.AddDays(1), "00:00", "06:30", True)
            If Not String.IsNullOrEmpty(Pout) Then
                PUNCH4 = Pout
            End If
        End If






        ''DETERMINE IF USER IS HALFDAY AFTERNOON
        ''*****************************************
        'MORNIG01
        ''*****************************************
        If PUNCH1 = "NTR" And PUNCH4 <> "NTR" Then

            If sql_worker.CHECK_MUST_IN(TimeTableList(0)) = True Then
                ''DETERMINE IF THE USER FILED A TRAVEL ORDER
                If sql_worker.CHECK_FILED_MTO(staff_id, _day) = True Then
                    If sql_worker.CHECK_IF_PAID_MTO(sql_worker.GET_FILED_MTO_ID(staff_id, _day)) Then
                        PUNCH1 = sql_worker.GET_MTO_SYMBOL(sql_worker.GET_FILED_MTO_ID(staff_id, _day))
                        monthly_remarks = PUNCH1
                        MTO_PURPOSE = sql_worker.GET_MTO_PURPOSE(staff_id, _day)
                        Daily_REMARKS = MTO_PURPOSE
                        'MessageBox.Show(MTO_PURPOSE)
                        ''DO NOT CALCULATE UNDERTIME IF PAID
                    Else
                        ''TODO GET SYMBOL AND CALCULATE UNDERTIME
                        PUNCH1 = sql_worker.GET_MTO_SYMBOL(sql_worker.GET_FILED_MTO_ID(staff_id, _day))
                        monthly_remarks = PUNCH1
                        TotalUnderTime_daily = TotalUnderTime_daily + sql_worker.GET_TOTAL_WORKING_HOURS(TimeTableList(0)) * 60
                    End If
                Else
                    ''CALCULATE UNDERTIME IF NOT FILED TRAVEL ORDER
                    TotalUnderTime_daily = TotalUnderTime_daily + sql_worker.GET_TOTAL_WORKING_HOURS(TimeTableList(0)) * 60
                    PUNCH1 = ""
                    Daily_REMARKS = "-incomplete"

                    ''''++++++++++++++++++++++++++++++++++++++++++++++++
                    ''''DILG RULES START 01b
                    ''''++++++++++++++++++++++++++++++++++++++++++++++++
                    'If My.Settings.dilg_rules_enabled = 1 Then

                    '   Dim Px4 As String = (Format(CDate(PUNCH4.Replace("*", "")), "HH:mm")).Replace(":", "")
                    '    Dim REGLR_SCHED_IN As Integer = sql_worker.GET_ON_DUTY_TIME(TimeTableList(0)).Replace(":", "")
                    '    Dim REGLR_SCHED_OUT As Integer = sql_worker.GET_OFF_DUTY_TIME(TimeTableList(0)).Replace(":", "")
                    '    Dim xOFF_DUTY As Date = sql_worker.GET_OFF_DUTY_TIME(TimeTableList(0))



                    '    Console.WriteLine("####ACTUALOUT: " & CInt(Px4))
                    '    Console.WriteLine("####FLEXITIMEIN: " & My.Settings.dilg_flex_onduty_rules)
                    '    Console.WriteLine("####FLEXITIMEOUT: " & My.Settings.dilg_flex_offduty_rules)
                    '    Console.WriteLine("####REGULARSCHEDIN: " & REGLR_SCHED_IN)
                    '    Console.WriteLine("####REGULARSCHEDOUT:" & REGLR_SCHED_OUT)


                    '    ''CHECK FOR UNDERTIME DAILY
                    '    If Px4 > REGLR_SCHED_OUT Or Px4 = REGLR_SCHED_OUT Then
                    '    Else
                    '        ''THIS IS THE UNDERTIME PART
                    '        Daily_UT = cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_OUT(TimeTableList(0)), xOFF_DUTY, PUNCH4, False, True)
                    '        Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$Px4 is LESS than")
                    '        Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$UT VALUE: " & Daily_UT)
                    '        Daily_UT_TOTAL = Daily_UT

                    '        If Not Daily_UT = 0 Then
                    '            Dim Hours As Integer = Math.Floor(Daily_UT / 60)
                    '            Dim Minutes As Integer = Daily_UT Mod 60

                    '            ''FORMAT THE VALUE FOR DTR TOTAL UNDERTIME OF EMPLOYEE MONTHLY
                    '            If Not Hours = 0 Then
                    '                If Not Minutes = 0 Then
                    '                    Daily_UT = Hours & "h" & Minutes & "m"
                    '                Else
                    '                    Daily_UT = Hours & "h"
                    '                End If
                    '            Else
                    '                Daily_UT = Minutes & "m"
                    '            End If
                    '        Else
                    '            Daily_UT = ""
                    '        End If
                    '    End If
                    'End If

                    ''''++++++++++++++++++++++++++++++++++++++++++++++++
                    ''''DILG RULES START 01
                    ''''++++++++++++++++++++++++++++++++++++++++++++++++

                End If

            Else
                ''GET ONDUTY  TIME FOR TIME TABLE NO UNDERTIME CALCULATION 
                PUNCH1 = Format(CDate(sql_worker.GET_ON_DUTY_TIME(TimeTableList(0))), "hh:mm tt")
            End If
            ''CHECK IF LOG IS MODIFIED
            PUNCH4 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, PUNCH4)


            ''*******************************
            ''AFTERNOON 10
            ''********************************
        ElseIf PUNCH1 <> "NTR" And PUNCH4 = "NTR" Then

            If sql_worker.CHECK_MUST_OUT(TimeTableList(0)) = True Then
                ''DETERMINE IF THE USER FILED A TRAVEL ORDER
                If sql_worker.CHECK_FILED_MTO(staff_id, _day) = True Then
                    If sql_worker.CHECK_IF_PAID_MTO(sql_worker.GET_FILED_MTO_ID(staff_id, _day)) Then
                        PUNCH4 = sql_worker.GET_MTO_SYMBOL(sql_worker.GET_FILED_MTO_ID(staff_id, _day))
                        monthly_remarks = PUNCH4
                        MTO_PURPOSE = sql_worker.GET_MTO_PURPOSE(staff_id, _day)
                        Daily_REMARKS = MTO_PURPOSE
                        ''DO NOT CALCULATE UNDERTIME IF PAID
                    Else
                        ''TODO GET SYMBOL AND CALCULATE UNDERTIME
                        PUNCH4 = sql_worker.GET_MTO_SYMBOL(sql_worker.GET_FILED_MTO_ID(staff_id, _day))
                        monthly_remarks = PUNCH4
                        TotalUnderTime_daily = TotalUnderTime_daily + sql_worker.GET_TOTAL_WORKING_HOURS(TimeTableList(0)) * 60
                    End If
                Else
                    ''CALCULATE UNDERTIME IF NO FILED TRAVEL ORDER
                    TotalUnderTime_daily = TotalUnderTime_daily + sql_worker.GET_TOTAL_WORKING_HOURS(TimeTableList(0)) * 60
                    PUNCH4 = ""
                    Daily_REMARKS = "-incomplete"


                    '''+++++++++++++++++++++++++++++++++++++++++++
                    '''DILG RULES START 10b
                    '''+++++++++++++++++++++++++++++++++++++++++++
                    If My.Settings.dilg_rules_enabled = 1 Then
                        Dim Px1 As String = (Format(CDate(PUNCH1.Replace("*", "")), "HH:mm")).Replace(":", "")
                        Dim REGLR_SCHED_IN As Integer = sql_worker.GET_ON_DUTY_TIME(TimeTableList(0)).Replace(":", "")
                        Dim REGLR_SCHED_OUT As Integer = sql_worker.GET_OFF_DUTY_TIME(TimeTableList(0)).Replace(":", "")
                        Dim xOFF_DUTY As Date = sql_worker.GET_OFF_DUTY_TIME(TimeTableList(0))




                        Console.WriteLine("####ACTUALIN: " & CInt(Px1))
                        Console.WriteLine("####FLEXITIMEIN: " & My.Settings.dilg_flex_onduty_rules)
                        Console.WriteLine("####FLEXITIMEOUT: " & My.Settings.dilg_flex_offduty_rules)
                        Console.WriteLine("####REGULARSCHEDIN: " & REGLR_SCHED_IN)
                        Console.WriteLine("####REGULARSCHEDOUT:" & REGLR_SCHED_OUT)


                        '''CHECK FOR LATE DAILY
                        If Px1 < REGLR_SCHED_IN Or Px1 = REGLR_SCHED_IN Then
                            Console.WriteLine("####STAFFINTIME = " & My.Settings.dilg_flex_onduty_rules)
                            Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$Px1 is Less than")

                            REGLR_SCHED_OUT = CInt(My.Settings.dilg_flex_offduty_rules.Replace(":", ""))
                            xOFF_DUTY = CDate(My.Settings.dilg_flex_offduty_rules)
                            ''
                        Else
                            ''THIS IS THE LATE PART
                            Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$BEGINING_IN: " & sql_worker.GET_BEGINING_IN(TimeTableList(0)))
                            Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$ON_DUTY: " & sql_worker.GET_ON_DUTY_TIME(TimeTableList(0)))
                            Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$PUNC1: " & PUNCH1)

                            Daily_LATE = cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_IN(TimeTableList(0)), sql_worker.GET_ON_DUTY_TIME(TimeTableList(0)), PUNCH1, True, False)
                            Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$Px1 is greater than")
                            Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$LATE VALUE: " & Daily_LATE)
                            Daily_LATE_TOTAL = Daily_LATE
                            If Not Daily_LATE = 0 Then
                                Dim Hours As Integer = Math.Floor(Daily_LATE / 60)
                                Dim Minutes As Integer = Daily_LATE Mod 60
                                ''FORMAT THE VALUE FOR DTR TOTAL UNDERTIME OF EMPLOYEE MONTHLY
                                If Not Hours = 0 Then
                                    If Not Minutes = 0 Then
                                        Daily_LATE = Hours & "h" & Minutes & "m"
                                    Else
                                        Daily_LATE = Hours & "h"
                                    End If

                                Else
                                    Daily_LATE = Minutes & "m"
                                End If
                            Else
                                Daily_LATE = ""
                            End If

                        End If
                    End If
                    '''+++++++++++++++++++++++++++++++++++++++++++++
                    '''END OF DILG RULES
                    '''+++++++++++++++++++++++++++++++++++++++++++++


                End If

            Else
                ''GET OFFDUTY  TIME FOR TIME TABLE NO UNDERTIME CALCULATION 
                PUNCH4 = Format(CDate(sql_worker.GET_OFF_DUTY_TIME(TimeTableList(0))), "hh:mm tt")
            End If
            ''CHECK IF LOG IS MODIFIED
            PUNCH1 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, PUNCH1)

            Console.WriteLine("PUNCH1 IS NOW: " & PUNCH1)

            ''****************************************
            ''AFTERNOON 00 
            ''****************************************
            ''IDENTIFIED NO CIN/COUT IN AFTERNOON (HALFDAY FOR AFTERNOON)
        ElseIf PUNCH1 = "NTR" And PUNCH4 = "NTR" Then
            If sql_worker.CHECK_MUST_OUT(TimeTableList(0)) = True Then

                ''DETERMINE IF THE USER FILED A TRAVEL ORDER
                If sql_worker.CHECK_FILED_MTO(staff_id, _day) = True Then
                    If sql_worker.CHECK_IF_PAID_MTO(sql_worker.GET_FILED_MTO_ID(staff_id, _day)) Then
                        PUNCH1 = sql_worker.GET_MTO_SYMBOL(sql_worker.GET_FILED_MTO_ID(staff_id, _day))
                        PUNCH4 = PUNCH1
                        MTO_PURPOSE = sql_worker.GET_MTO_PURPOSE(staff_id, _day)
                        Daily_REMARKS = MTO_PURPOSE
                        ''DO NOT CALCULATE UNDERTIME IF PAID
                    Else
                        ''TODO GET SYMBOL AND CALCULATE UNDERTIME
                        PUNCH1 = sql_worker.GET_MTO_SYMBOL(sql_worker.GET_FILED_MTO_ID(staff_id, _day))
                        PUNCH4 = PUNCH1
                        monthly_remarks = PUNCH1
                        TotalUnderTime_daily = TotalUnderTime_daily + sql_worker.GET_TOTAL_WORKING_HOURS(TimeTableList(0)) * 60
                    End If
                Else
                    ''CALCULATE UNDERTIME IF NO FILED TRAVEL ORDER
                    TotalUnderTime_daily = TotalUnderTime_daily + sql_worker.GET_TOTAL_WORKING_HOURS(TimeTableList(0)) * 60
                    PUNCH1 = ""
                    PUNCH4 = ""
                    Daily_REMARKS = ""

                End If

            Else
                ''GET OFFDUTY  TIME FOR TIME TABLE NO UNDERTIME CALCULATION 
                ''CONFIRM THIS IS ABSENT
                PUNCH1 = ""
                PUNCH4 = ""
                TotalUnderTime_daily = sql_worker.GET_TOTAL_WORKING_HOURS(TimeTableList(0)) * 60
            End If

        ElseIf PUNCH1 <> "NTR" And PUNCH4 <> "NTR" Then
            ''**************************************************
            ''AFTERNOON 11
            ''****************************************************

            If My.Settings.dilg_rules_enabled = 1 Then
                ''DETERMINE WHERE TO APPLY THE FLEXI
                ''THIS RULES APPLY FOR DILG ONLY 7:30 to 16:30

DILG_QUEZON_AVE:

                Dim Px1 As String = (Format(CDate(PUNCH1.Replace("*", "")), "HH:mm")).Replace(":", "")
                Dim Px4 As String = (Format(CDate(PUNCH4.Replace("*", "")), "HH:mm")).Replace(":", "")
                Dim REGLR_SCHED_IN As Integer = sql_worker.GET_ON_DUTY_TIME(TimeTableList(0)).Replace(":", "")
                Dim REGLR_SCHED_OUT As Integer = sql_worker.GET_OFF_DUTY_TIME(TimeTableList(0)).Replace(":", "")

                Dim xOFF_DUTY As Date = sql_worker.GET_OFF_DUTY_TIME(TimeTableList(0))









                Console.WriteLine("####ACTUALIN: " & CInt(Px1))
                Console.WriteLine("####ACTUALOUT: " & CInt(Px4))
                Console.WriteLine("####FLEXITIMEIN: " & My.Settings.dilg_flex_onduty_rules)
                Console.WriteLine("####FLEXITIMEOUT: " & My.Settings.dilg_flex_offduty_rules)
                Console.WriteLine("####REGULARSCHEDIN: " & REGLR_SCHED_IN)
                Console.WriteLine("####REGULARSCHEDOUT:" & REGLR_SCHED_OUT)






                '''CHECK FOR LATE DAILY
                If Px1 < REGLR_SCHED_IN Or Px1 = REGLR_SCHED_IN Then
                    Console.WriteLine("####STAFFINTIME = " & My.Settings.dilg_flex_onduty_rules)
                    Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$Px1 is Less than")

                    '''1. CHECK IF BELOW 7:30AM
                    '(Format(CDate(PUNCH4.Replace("*", "")), "HH:mm")).Replace(":", "")
                    If (Px1 <= (Format(CDate(My.Settings.dilg_flex_onduty_rules), "HH:mm")).Replace(":", "")) Then
                        xOFF_DUTY = CDate(My.Settings.dilg_flex_offduty_rules)

                        Console.WriteLine("USER TIME-IN: " & PUNCH1)
                        Console.WriteLine("NEW OFF DUTY TIME: " & xOFF_DUTY)
                    Else
                        '''CONFIRMED BETWEEN 7:30 - REGULAR_ON_DUTY_SCHED
                        xOFF_DUTY = CDate(PUNCH1.Replace("*", "")).AddHours(9)
                        Console.WriteLine("USER TIME-IN: " & PUNCH1)
                        Console.WriteLine("NEW OFF DUTY TIME: " & xOFF_DUTY)

                    End If

                    REGLR_SCHED_OUT = CInt(Format(xOFF_DUTY, "HH:mm").Replace(":", ""))
                    Console.WriteLine("NEW REGLR_SCHED_OUT: " & xOFF_DUTY)
                    ''
                    ''
                Else
                    ''THIS IS THE LATE PART
                    Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$BEGINING_IN: " & sql_worker.GET_BEGINING_IN(TimeTableList(0)))
                    Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$ON_DUTY: " & sql_worker.GET_ON_DUTY_TIME(TimeTableList(0)))
                    Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$PUNC1: " & PUNCH1)

                    Daily_LATE = cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_IN(TimeTableList(0)), sql_worker.GET_ON_DUTY_TIME(TimeTableList(0)), PUNCH1, True, False)
                    Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$Px1 is greater than")
                    Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$LATE VALUE: " & Daily_LATE)
                    Daily_LATE_TOTAL = Daily_LATE

                    If Not Daily_LATE = 0 Then
                        Dim Hours As Integer = Math.Floor(Daily_LATE / 60)
                        Dim Minutes As Integer = Daily_LATE Mod 60
                        ''FORMAT THE VALUE FOR DTR TOTAL UNDERTIME OF EMPLOYEE MONTHLY
                        If Not Hours = 0 Then
                            If Not Minutes = 0 Then
                                Daily_LATE = Hours & "h" & Minutes & "m"
                            Else
                                Daily_LATE = Hours & "h"
                            End If

                        Else
                            Daily_LATE = Minutes & "m"
                        End If
                    Else
                        Daily_LATE = ""
                    End If

                End If


                ''CHECK FOR UNDERTIME DAILY
                If Px4 > REGLR_SCHED_OUT Or Px4 = REGLR_SCHED_OUT Then
                Else
                    ''THIS IS THE UNDERTIME PART
                    Daily_UT = cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_OUT(TimeTableList(0)), xOFF_DUTY, PUNCH4, False, True)
                    Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$Px4 is LESS than")
                    Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$UT VALUE: " & Daily_UT)
                    Daily_UT_TOTAL = Daily_UT

                    If Not Daily_UT = 0 Then
                        Dim Hours As Integer = Math.Floor(Daily_UT / 60)
                        Dim Minutes As Integer = Daily_UT Mod 60

                        ''FORMAT THE VALUE FOR DTR TOTAL UNDERTIME OF EMPLOYEE MONTHLY
                        If Not Hours = 0 Then
                            If Not Minutes = 0 Then
                                Daily_UT = Hours & "h" & Minutes & "m"
                            Else
                                Daily_UT = Hours & "h"
                            End If

                        Else
                            Daily_UT = Minutes & "m"
                        End If
                    Else
                        Daily_UT = ""
                    End If

                End If

            Else
                ''PROCEED TO THE REGULAR CALCULATION IF FLEXI IS NOT DETECTED.
                TotalUnderTime_daily = TotalUnderTime_daily + cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_IN(TimeTableList(0)), sql_worker.GET_ON_DUTY_TIME(TimeTableList(0)), PUNCH1, True, False)
                TotalUnderTime_daily = TotalUnderTime_daily + cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_OUT(TimeTableList(0)), sql_worker.GET_OFF_DUTY_TIME(TimeTableList(0)), PUNCH4, False, True)
            End If



            ''CALCULATE OVERTIME CHECK FIRST IF THE LOGS IS MODIFIED IF NOT DO NOT ALLOW OT
            ''COUNT OVERTIME
            ''STEP1 GET INTERVALCOUNT AFTER CHECK OUT(expect minutes value)
            If sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER_AS_BOOLEAN(staff_id, _day, _day, PUNCH4) = False Then
                Dim total_minutes_after_out As Integer = 0
                Dim interval_of_leaving_count_as_OT As Integer = 0
                ''GET INTERVAL OT AFTER COUT
                interval_of_leaving_count_as_OT = sql_worker.GET_INTERVAL_OF_LEAVING_COUNT_AS_OT(shift_id)
                total_minutes_after_out = cpu_worker.GET_INTERVAL_AFTER_COUT(sql_worker.GET_BEGINING_OUT(TimeTableList(0)), sql_worker.GET_OFF_DUTY_TIME(TimeTableList(0)), PUNCH4)
                ''CALCULATE
                If total_minutes_after_out >= interval_of_leaving_count_as_OT Then
                    TOTAL_OVERTIME = total_minutes_after_out

                    Dim Hours As Integer = Math.Floor(TOTAL_OVERTIME / 60)
                    Dim Minutes As Integer = TOTAL_OVERTIME Mod 60

                    If Hours <> 0 And Minutes = 0 Then
                        TOTAL_OT_DAILY_REMARKS = Hours & "hr"
                    ElseIf Hours = 0 And Minutes <> 0 Then
                        TOTAL_OT_DAILY_REMARKS = Minutes & "min"
                    Else
                        TOTAL_OT_DAILY_REMARKS = Hours & "hr" & " " & Minutes & "min"
                    End If
                    ''FORMAT THE VALUE FOR DTR TOTAL OT OF EMPLOYEE DAILY

                End If
            Else

                ''TODO IF MODIFEID LOGS COUNT AS OT
            End If

            ''CHECK IF LOG IS MODIFIED
            PUNCH1 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, PUNCH1)
            PUNCH4 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, PUNCH4)
        End If


        ''FORMATING ALL THE VARIABLES
        If monthly_remarks = "" Then
            monthly_remarks = cpu_worker.CONVERT_TO_TIME(TotalUnderTime_daily)
            UNDERHH = cpu_worker.CONVERT_TO_HH(TotalUnderTime_daily)
            UNDERMM = cpu_worker.CONVERT_TO_MM(TotalUnderTime_daily)
            If UNDERHH = 0 Then
                UNDERHH = ""
            End If
            If UNDERMM = 0 Then
                UNDERMM = ""
            End If
        Else
            UNDERHH = MTO_PURPOSE
        End If



        Console.WriteLine("MONTHLY REMARKS: " & monthly_remarks)
        Console.WriteLine("UNDERHH: " & UNDERHH)
        Console.WriteLine("UNDERMM: " & UNDERMM)



        ''DETERMINE TOTAL WORK HOURS OF THE SCHEDULE
        TOTAL_WORK_HOURS = (sql_worker.GET_TOTAL_WORKING_HOURS(TimeTableList(0)) * 60)
        Console.WriteLine("TOTAL UNDERTIME: " & TotalUnderTime_daily & " DATED: " & _day)



        ''SECTION HOLIDAY
        If sql_worker.CHECK_HOLIDAY(_day) = True Then
            TAG_AS_HOLIDAY()
            TotalUnderTime_daily = 0
            Console.WriteLine("Today is Holiday: " & UNDERHH & " DATED: " & _day)
            Exit Sub
            ''SECTION FILE LEAVE
        ElseIf sql_worker.CHECK_FILED_LEAVE(staff_id, _day) = True Then
            UNDERHH = sql_worker.GET_LEAVE_NAME(sql_worker.GET_FILED_LEAVEID(staff_id, _day))
            UNDERMM = ""
            Daily_REMARKS = UNDERHH
            monthly_remarks = sql_worker.GET_LEAVE_SYMBOL(sql_worker.GET_FILED_LEAVEID(staff_id, _day))
            Console.WriteLine("Staff has filed leave: " & monthly_remarks & " DATED: " & _day)

            If sql_worker.CHECK_IF_PAID_LEAVE(sql_worker.GET_FILED_LEAVEID(staff_id, _day)) = True Then
                TotalUnderTime_daily = 0
            End If

            ''DETERMINE IF STAFF WHOLEDAY ABSENT
        ElseIf TotalUnderTime_daily >= TOTAL_WORK_HOURS Then
            Console.WriteLine("USER EXCEED TOTALUNDERTIME: " & TotalUnderTime_daily & " DATED: " & _day)
            monthly_remarks = "A"
            UNDERHH = My.Settings.dtr_remarks_for_absent
            'PUNCH1 = ""
            'PUNCH2 = ""
        End If



        Select Case GlobalVariables.DTR_TYPE.ToString
            Case "DTR"
            Case "DTR_B"
            Case "DTR_C"
                If IsNumeric(UNDERHH) Then
                    ''DTR TYPE C is for DPWH Bacolod Kabankalan
                    ''Rules is no daily calculation of undertime by default UNDERHH has the daily calculated values.
                    ''so detect if UNDERHH variable has a value with "IsNumeric()" function.
                    ''if there is a value clear the UNDERHH variable to meet the rules for the DTR report.
                    UNDERHH = ""
                End If
        End Select


        '''POPULATE DTR TABLE
        sql_worker.INSERT_INTO_DTR_TABLE(staff_id, staffname, departmentname,
                                         _day, PUNCH1, PUNCH2, PUNCH3, PUNCH4,
                                         week, dtr_remarks, monthly_remarks, TotalUnderTime_daily, UNDERHH, UNDERMM,
                                         TotalUnderTime_daily, TOTAL_OVERTIME, TOTAL_OT_DAILY_REMARKS,
                                         GlobalVariables.OFFICIAL_ARRIVAL, GlobalVariables.OFFICIAL_DEPARTURE, Daily_UT, Daily_LATE, Daily_REMARKS,
                                         Daily_LATE_TOTAL, Daily_UT_TOTAL, "", "", designation)

    End Sub




    Public Sub DAILY_4_LOGS()
        ''' PUNCH1,PUNCH2,PUNCH3,PUNCH4
        Console.WriteLine("ID: " & staff_id & " Date: " & _day & " LOGS: " & String.Join(" + ", staff_raw_logs))
        ''STEP 1 GET ALL PUNCHES

        PUNCH1 = cpu_worker.GET_ACTUALTIME(staff_raw_logs, sql_worker.GET_BEGINING_IN(TimeTableList(0)), sql_worker.GET_ENDING_IN(TimeTableList(0)), True)
        'staff_raw_logs.Remove(PUNCH1)
        PUNCH2 = cpu_worker.GET_ACTUALTIME(staff_raw_logs, sql_worker.GET_BEGINING_OUT(TimeTableList(0)), sql_worker.GET_ENDING_OUT(TimeTableList(0)), True)
        'staff_raw_logs.Remove(PUNCH2)
        PUNCH3 = cpu_worker.GET_ACTUALTIME(staff_raw_logs, sql_worker.GET_BEGINING_IN(TimeTableList(1)), sql_worker.GET_ENDING_IN(TimeTableList(1)), False)
        'staff_raw_logs.Remove(PUNCH3)
        PUNCH4 = cpu_worker.GET_ACTUALTIME(staff_raw_logs, sql_worker.GET_BEGINING_OUT(TimeTableList(1)), sql_worker.GET_ENDING_OUT(TimeTableList(1)), False)
        'staff_raw_logs.Remove(pun4)
        ''STEP 2 DETERMIN POLICY FOR UNDERTIME

        ''DETERMINE IF USER IS HALFDAY MORNING
        ''*****************************************
        'MORNING 01
        ''*****************************************
        If PUNCH1 = "NTR" And PUNCH2 <> "NTR" Then
            If sql_worker.CHECK_MUST_IN(TimeTableList(0)) = True Then
                ''DETERMINE IF THE USER FILED A TRAVEL ORDER
                If sql_worker.CHECK_FILED_MTO(staff_id, _day) = True Then
                    If sql_worker.CHECK_IF_PAID_MTO(sql_worker.GET_FILED_MTO_ID(staff_id, _day)) Then
                        PUNCH1 = sql_worker.GET_MTO_SYMBOL(sql_worker.GET_FILED_MTO_ID(staff_id, _day))
                        monthly_remarks = PUNCH1
                        MTO_PURPOSE = sql_worker.GET_MTO_PURPOSE(staff_id, _day)
                        Daily_REMARKS = MTO_PURPOSE
                        ''DO NOT CALCULATE UNDERTIME IF PAID
                    Else
                        ''TODO GET SYMBOL AND CALCULATE UNDERTIME
                        PUNCH1 = sql_worker.GET_MTO_SYMBOL(sql_worker.GET_FILED_MTO_ID(staff_id, _day))
                        monthly_remarks = PUNCH1
                        Daily_REMARKS = sql_worker.GET_MTO_PURPOSE(staff_id, _day)
                        TotalUnderTime_daily = TotalUnderTime_daily + sql_worker.GET_TOTAL_WORKING_HOURS(TimeTableList(0)) * 60
                    End If
                Else
                    ''CALCULATE UNDERTIME IF NOT FILED TRAVEL ORDER
                    TotalUnderTime_daily = TotalUnderTime_daily + sql_worker.GET_TOTAL_WORKING_HOURS(TimeTableList(0)) * 60
                    PUNCH1 = ""
                    Daily_REMARKS = "-incomplete"
                End If

            Else
                ''GET ONDUTY  TIME FOR TIME TABLE NO UNDERTIME CALCULATION 
                PUNCH1 = sql_worker.GET_ON_DUTY_TIME(TimeTableList(0))
            End If
            'staff_raw_logs.Remove(PUNCH2)
            PUNCH2 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, PUNCH2)


            ''*******************************
            ''MORNING 10
            ''********************************
        ElseIf PUNCH1 <> "NTR" And PUNCH2 = "NTR" Then
            If sql_worker.CHECK_MUST_OUT(TimeTableList(0)) = True Then

                ''DETERMINE IF THE USER FILED A TRAVEL ORDER
                If sql_worker.CHECK_FILED_MTO(staff_id, _day) = True Then
                    If sql_worker.CHECK_IF_PAID_MTO(sql_worker.GET_FILED_MTO_ID(staff_id, _day)) Then
                        PUNCH2 = sql_worker.GET_MTO_SYMBOL(sql_worker.GET_FILED_MTO_ID(staff_id, _day))
                        monthly_remarks = PUNCH2
                        MTO_PURPOSE = sql_worker.GET_MTO_PURPOSE(staff_id, _day)
                        Daily_REMARKS = MTO_PURPOSE
                        ''DO NOT CALCULATE UNDERTIME IF PAID
                    Else
                        ''TODO GET SYMBOL AND CALCULATE UNDERTIME
                        PUNCH2 = sql_worker.GET_MTO_SYMBOL(sql_worker.GET_FILED_MTO_ID(staff_id, _day))
                        monthly_remarks = PUNCH2
                        Daily_REMARKS = sql_worker.GET_MTO_PURPOSE(staff_id, _day)
                        TotalUnderTime_daily = TotalUnderTime_daily + sql_worker.GET_TOTAL_WORKING_HOURS(TimeTableList(0)) * 60
                    End If
                Else
                    ''CALCULATE UNDERTIME IF NO FILED TRAVEL ORDER
                    TotalUnderTime_daily = TotalUnderTime_daily + sql_worker.GET_TOTAL_WORKING_HOURS(TimeTableList(0)) * 60
                    PUNCH2 = ""
                    Daily_REMARKS = "-incomplete"
                End If

            Else
                ''GET OFFDUTY  TIME FOR TIME TABLE NO UNDERTIME CALCULATION 
                PUNCH2 = sql_worker.GET_OFF_DUTY_TIME(TimeTableList(0))
            End If
            ''CHECK PUNCH1 IF MODIFIED
            'staff_raw_logs.Remove(PUNCH1)
            PUNCH1 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, PUNCH1)



            ''****************************************
            ''MORNING 00 
            ''****************************************
            ''IDENTIFIED NO CIN/COUT IN MORNING (HALFDAY FOR MORNING)
        ElseIf PUNCH1 = "NTR" And PUNCH2 = "NTR" Then
            If sql_worker.CHECK_MUST_OUT(TimeTableList(0)) = True Then

                ''DETERMINE IF THE USER FILED A TRAVEL ORDER
                If sql_worker.CHECK_FILED_MTO(staff_id, _day) = True Then
                    If sql_worker.CHECK_IF_PAID_MTO(sql_worker.GET_FILED_MTO_ID(staff_id, _day)) Then
                        PUNCH1 = sql_worker.GET_MTO_SYMBOL(sql_worker.GET_FILED_MTO_ID(staff_id, _day))
                        PUNCH2 = PUNCH1
                        monthly_remarks = PUNCH1
                        MTO_PURPOSE = sql_worker.GET_MTO_PURPOSE(staff_id, _day)
                        Daily_REMARKS = MTO_PURPOSE
                        ''DO NOT CALCULATE UNDERTIME IF PAID
                    Else
                        ''TODO GET SYMBOL AND CALCULATE UNDERTIME
                        PUNCH1 = sql_worker.GET_MTO_SYMBOL(sql_worker.GET_FILED_MTO_ID(staff_id, _day))
                        PUNCH2 = PUNCH1
                        monthly_remarks = PUNCH1
                        Daily_REMARKS = sql_worker.GET_MTO_PURPOSE(staff_id, _day)
                        TotalUnderTime_daily = TotalUnderTime_daily + sql_worker.GET_TOTAL_WORKING_HOURS(TimeTableList(0)) * 60
                    End If
                Else
                    ''CALCULATE UNDERTIME IF NO FILED TRAVEL ORDER
                    TotalUnderTime_daily = TotalUnderTime_daily + sql_worker.GET_TOTAL_WORKING_HOURS(TimeTableList(0)) * 60
                    PUNCH1 = ""
                    PUNCH2 = ""
                End If

            Else
                ''GET OFFDUTY  TIME FOR TIME TABLE NO UNDERTIME CALCULATION 
                PUNCH1 = sql_worker.GET_ON_DUTY_TIME(TimeTableList(0))
                PUNCH2 = sql_worker.GET_OFF_DUTY_TIME(TimeTableList(0))
            End If
        ElseIf PUNCH1 <> "NTR" And PUNCH2 <> "NTR" Then
            ''**************************************************
            ''MORNING 11
            ''****************************************************
            TotalUnderTime_daily = TotalUnderTime_daily + cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_IN(TimeTableList(0)), sql_worker.GET_ON_DUTY_TIME(TimeTableList(0)), PUNCH1, True, False)
            TotalUnderTime_daily = TotalUnderTime_daily + cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_OUT(TimeTableList(0)), sql_worker.GET_OFF_DUTY_TIME(TimeTableList(0)), PUNCH2, False, True)
            PUNCH1 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, PUNCH1)
            PUNCH2 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, PUNCH2)

        End If



        ''DETERMINE IF USER IS HALFDAY AFTERNOON
        ''*****************************************
        'AFTERNOON 01
        ''*****************************************
        If PUNCH3 = "NTR" And PUNCH4 <> "NTR" Then
            If sql_worker.CHECK_MUST_IN(TimeTableList(1)) = True Then
                ''DETERMINE IF THE USER FILED A TRAVEL ORDER
                If sql_worker.CHECK_FILED_MTO(staff_id, _day) = True Then
                    If sql_worker.CHECK_IF_PAID_MTO(sql_worker.GET_FILED_MTO_ID(staff_id, _day)) Then
                        PUNCH3 = sql_worker.GET_MTO_SYMBOL(sql_worker.GET_FILED_MTO_ID(staff_id, _day))
                        monthly_remarks = PUNCH3
                        MTO_PURPOSE = sql_worker.GET_MTO_PURPOSE(staff_id, _day)
                        Daily_REMARKS = MTO_PURPOSE
                        'MessageBox.Show(MTO_PURPOSE)
                        ''DO NOT CALCULATE UNDERTIME IF PAID
                    Else
                        ''TODO GET SYMBOL AND CALCULATE UNDERTIME
                        PUNCH3 = sql_worker.GET_MTO_SYMBOL(sql_worker.GET_FILED_MTO_ID(staff_id, _day))
                        monthly_remarks = PUNCH3
                        Daily_REMARKS = sql_worker.GET_MTO_PURPOSE(staff_id, _day)
                        TotalUnderTime_daily = TotalUnderTime_daily + sql_worker.GET_TOTAL_WORKING_HOURS(TimeTableList(1)) * 60
                    End If
                Else
                    ''CALCULATE UNDERTIME IF NOT FILED TRAVEL ORDER
                    TotalUnderTime_daily = TotalUnderTime_daily + sql_worker.GET_TOTAL_WORKING_HOURS(TimeTableList(1)) * 60
                    PUNCH3 = ""
                    Daily_REMARKS = "-incomplete"
                End If

            Else
                ''GET ONDUTY  TIME FOR TIME TABLE NO UNDERTIME CALCULATION 
                PUNCH3 = sql_worker.GET_ON_DUTY_TIME(TimeTableList(1))
            End If
            ''CHECK IF LOG IS MODIFIED
            'staff_raw_logs.Remove(PUNCH4)
            PUNCH4 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, PUNCH4)

            ''*******************************
            ''AFTERNOON 10
            ''********************************
        ElseIf PUNCH3 <> "NTR" And PUNCH4 = "NTR" Then
            If sql_worker.CHECK_MUST_OUT(TimeTableList(1)) = True Then

                ''DETERMINE IF THE USER FILED A TRAVEL ORDER
                If sql_worker.CHECK_FILED_MTO(staff_id, _day) = True Then
                    If sql_worker.CHECK_IF_PAID_MTO(sql_worker.GET_FILED_MTO_ID(staff_id, _day)) Then
                        PUNCH4 = sql_worker.GET_MTO_SYMBOL(sql_worker.GET_FILED_MTO_ID(staff_id, _day))
                        monthly_remarks = PUNCH4
                        MTO_PURPOSE = sql_worker.GET_MTO_PURPOSE(staff_id, _day)
                        Daily_REMARKS = MTO_PURPOSE
                        ''DO NOT CALCULATE UNDERTIME IF PAID
                    Else
                        ''TODO GET SYMBOL AND CALCULATE UNDERTIME
                        PUNCH4 = sql_worker.GET_MTO_SYMBOL(sql_worker.GET_FILED_MTO_ID(staff_id, _day))
                        monthly_remarks = PUNCH4
                        Daily_REMARKS = sql_worker.GET_MTO_PURPOSE(staff_id, _day)
                        TotalUnderTime_daily = TotalUnderTime_daily + sql_worker.GET_TOTAL_WORKING_HOURS(TimeTableList(1)) * 60
                    End If
                Else
                    ''CALCULATE UNDERTIME IF NO FILED TRAVEL ORDER
                    TotalUnderTime_daily = TotalUnderTime_daily + sql_worker.GET_TOTAL_WORKING_HOURS(TimeTableList(1)) * 60
                    PUNCH4 = ""
                    Daily_REMARKS = "-incomplete"
                End If

            Else
                ''GET OFFDUTY  TIME FOR TIME TABLE NO UNDERTIME CALCULATION 
                PUNCH4 = sql_worker.GET_OFF_DUTY_TIME(TimeTableList(1))
            End If
            ''CHECK IF LOG IS MODIFIED
            'staff_raw_logs.Remove(PUNCH3)
            PUNCH3 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, PUNCH3)

            ''****************************************
            ''AFTERNOON 00 
            ''****************************************
            ''IDENTIFIED NO CIN/COUT IN AFTERNOON (HALFDAY FOR AFTERNOON)
        ElseIf PUNCH3 = "NTR" And PUNCH4 = "NTR" Then
            If sql_worker.CHECK_MUST_OUT(TimeTableList(1)) = True Then

                ''DETERMINE IF THE USER FILED A TRAVEL ORDER
                If sql_worker.CHECK_FILED_MTO(staff_id, _day) = True Then
                    If sql_worker.CHECK_IF_PAID_MTO(sql_worker.GET_FILED_MTO_ID(staff_id, _day)) Then
                        PUNCH3 = sql_worker.GET_MTO_SYMBOL(sql_worker.GET_FILED_MTO_ID(staff_id, _day))
                        PUNCH4 = PUNCH3
                        MTO_PURPOSE = sql_worker.GET_MTO_PURPOSE(staff_id, _day)
                        Daily_REMARKS = MTO_PURPOSE
                        ''DO NOT CALCULATE UNDERTIME IF PAID
                    Else
                        ''TODO GET SYMBOL AND CALCULATE UNDERTIME
                        PUNCH3 = sql_worker.GET_MTO_SYMBOL(sql_worker.GET_FILED_MTO_ID(staff_id, _day))
                        PUNCH4 = PUNCH3
                        monthly_remarks = PUNCH3
                        Daily_REMARKS = sql_worker.GET_MTO_PURPOSE(staff_id, _day)
                        TotalUnderTime_daily = TotalUnderTime_daily + sql_worker.GET_TOTAL_WORKING_HOURS(TimeTableList(1)) * 60
                    End If
                Else
                    ''CALCULATE UNDERTIME IF NO FILED TRAVEL ORDER
                    TotalUnderTime_daily = TotalUnderTime_daily + sql_worker.GET_TOTAL_WORKING_HOURS(TimeTableList(1)) * 60
                    PUNCH3 = ""
                    PUNCH4 = ""
                End If

            Else
                ''GET OFFDUTY  TIME FOR TIME TABLE NO UNDERTIME CALCULATION 
                PUNCH3 = sql_worker.GET_ON_DUTY_TIME(TimeTableList(1))
                PUNCH4 = sql_worker.GET_OFF_DUTY_TIME(TimeTableList(1))
            End If

        ElseIf PUNCH3 <> "NTR" And PUNCH4 <> "NTR" Then
            ''**************************************************
            ''AFTERNOON 11
            ''****************************************************

            TotalUnderTime_daily = TotalUnderTime_daily + cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_IN(TimeTableList(1)), sql_worker.GET_ON_DUTY_TIME(TimeTableList(1)), PUNCH3, True, False)
            TotalUnderTime_daily = TotalUnderTime_daily + cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_OUT(TimeTableList(1)), sql_worker.GET_OFF_DUTY_TIME(TimeTableList(1)), PUNCH4, False, True)


            ''CALCULATE OVERTIME CHECK FIRST IF THE LOGS IS MODIFIED IF NOT DO NOT ALLOW OT
            ''COUNT OVERTIME
            ''STEP1 GET INTERVALCOUNT AFTER CHECK OUT(expect minutes value)
            If sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER_AS_BOOLEAN(staff_id, _day, _day, PUNCH4) = False Then
                Dim total_minutes_after_out As Integer = 0
                Dim interval_of_leaving_count_as_OT As Integer = 0
                ''GET INTERVAL OT AFTER COUT
                interval_of_leaving_count_as_OT = sql_worker.GET_INTERVAL_OF_LEAVING_COUNT_AS_OT(shift_id)
                total_minutes_after_out = cpu_worker.GET_INTERVAL_AFTER_COUT(sql_worker.GET_BEGINING_OUT(TimeTableList(1)), sql_worker.GET_OFF_DUTY_TIME(TimeTableList(1)), PUNCH4)
                ''CALCULATE
                If total_minutes_after_out >= interval_of_leaving_count_as_OT Then
                    TOTAL_OVERTIME = total_minutes_after_out

                    Dim Hours As Integer = Math.Floor(TOTAL_OVERTIME / 60)
                    Dim Minutes As Integer = TOTAL_OVERTIME Mod 60

                    If Hours <> 0 And Minutes = 0 Then
                        TOTAL_OT_DAILY_REMARKS = Hours & "h"
                    ElseIf Hours = 0 And Minutes <> 0 Then
                        TOTAL_OT_DAILY_REMARKS = Minutes & "m"
                    Else
                        TOTAL_OT_DAILY_REMARKS = Hours & "h" & " " & Minutes & "m"
                    End If

                    ''FORMAT THE VALUE FOR DTR TOTAL OT OF EMPLOYEE DAILY

                End If
            Else

                ''TODO IF MODIFEID LOGS COUNT AS OT
            End If

            ''CHECK IF LOG IS MODIFIED
            PUNCH3 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, PUNCH3)
            PUNCH4 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, PUNCH4)
        End If










        ''FORMATING ALL THE VARIABLES
        If monthly_remarks = "" Then
            monthly_remarks = cpu_worker.CONVERT_TO_TIME(TotalUnderTime_daily)
            UNDERHH = cpu_worker.CONVERT_TO_HH(TotalUnderTime_daily)
            UNDERMM = cpu_worker.CONVERT_TO_MM(TotalUnderTime_daily)
            If UNDERHH = 0 Then
                UNDERHH = ""
            End If
            If UNDERMM = 0 Then
                UNDERMM = ""
            End If
        Else
            UNDERHH = MTO_PURPOSE
        End If



        Console.WriteLine("MONTHLY REMARKS: " & monthly_remarks)
        Console.WriteLine("UNDERHH: " & UNDERHH)
        Console.WriteLine("UNDERMM: " & UNDERMM)



        ''DETERMINE TOTAL WORK HOURS OF THE SCHEDULE
        TOTAL_WORK_HOURS = (sql_worker.GET_TOTAL_WORKING_HOURS(TimeTableList(0)) * 60) + (sql_worker.GET_TOTAL_WORKING_HOURS(TimeTableList(1)) * 60)
        Console.WriteLine("TOTAL UNDERTIME: " & TotalUnderTime_daily & " DATED: " & _day)



        ''SECTION HOLIDAY
        If sql_worker.CHECK_HOLIDAY(_day) = True Then
            TAG_AS_HOLIDAY()
            TotalUnderTime_daily = 0
            Console.WriteLine("Today is Holiday: " & UNDERHH & " DATED: " & _day)
            Exit Sub
            ''SECTION FILE LEAVE
        ElseIf sql_worker.CHECK_FILED_LEAVE(staff_id, _day) = True Then
            UNDERHH = sql_worker.GET_LEAVE_NAME(sql_worker.GET_FILED_LEAVEID(staff_id, _day))
            UNDERMM = ""
            Daily_REMARKS = UNDERHH
            monthly_remarks = sql_worker.GET_LEAVE_SYMBOL(sql_worker.GET_FILED_LEAVEID(staff_id, _day))
            Console.WriteLine("Staff has filed leave: " & monthly_remarks & " DATED: " & _day)

            If sql_worker.CHECK_IF_PAID_LEAVE(sql_worker.GET_FILED_LEAVEID(staff_id, _day)) = True Then
                TotalUnderTime_daily = 0
            End If

            ''DETERMINE IF STAFF WHOLEDAY ABSENT
        ElseIf TotalUnderTime_daily >= TOTAL_WORK_HOURS Then
            Console.WriteLine("USER EXCEED TOTALUNDERTIME: " & TotalUnderTime_daily & " DATED: " & _day)
            monthly_remarks = My.Settings.sra_remarks_for_absent
            UNDERHH = My.Settings.dtr_remarks_for_absent

            'KABANKALAN RULES
            If IsDate(PUNCH1) Or IsDate(PUNCH2) Or IsDate(PUNCH3) Or IsDate(PUNCH4) Or isModifiedLogs = True Then
                UNDERHH = ""
            End If

        End If



        ''''GET OFFICIAL ARRIVAL AND OFFICIAL DEPARTURE

        'OFFICIAL_ARRIVAL = Format(CDate(sql_worker.GET_ON_DUTY_TIME(TimeTableList(0))), "hh:mm tt")
        'OFFICIAL_DEPARTURE = Format(CDate(sql_worker.GET_OFF_DUTY_TIME(TimeTableList(1))), "hh:mm tt")

        ' TOTAL_OVERTIME = CALCULATE_OT_DAILY(PUNCH4, sql_worker.GET_OFF_DUTY_TIME(TimeTableList(1)))

        Select Case GlobalVariables.DTR_TYPE.ToString
            Case "DTR"
            Case "DTR_B"
            Case "DTR_C"
                If IsNumeric(UNDERHH) Then
                    UNDERHH = ""
                End If
                'If UNDERHH = "Absent" Then
                '    UNDERHH = ""
                'End If
        End Select


     





        sql_worker.INSERT_INTO_DTR_TABLE(staff_id, staffname, departmentname,
                                         _day, PUNCH1, PUNCH2, PUNCH3, PUNCH4,
                                         week, dtr_remarks, monthly_remarks, TotalUnderTime_daily, UNDERHH, UNDERMM,
                                         TotalUnderTime_daily, TOTAL_OVERTIME, TOTAL_OT_DAILY_REMARKS,
                                        GlobalVariables.OFFICIAL_ARRIVAL, GlobalVariables.OFFICIAL_DEPARTURE, Daily_UT, Daily_LATE, Daily_REMARKS,
                                        Daily_LATE_TOTAL, Daily_UT_TOTAL, "", "", designation)

        '''
        '''END OF EXPECT_4LOGS
        '''
    End Sub



    Public Sub DAILY_4_LOGS_DILG_SAN_FERNANDO_LAUNION()
        ''' PUNCH1,PUNCH2,PUNCH3,PUNCH4
        Console.WriteLine("ID: " & staff_id & " Date: " & _day & " LOGS: " & String.Join(" + ", staff_raw_logs))
        ''STEP 1 GET ALL PUNCHES
        Dim lunchBreakPunches As IEnumerable(Of String)
        Dim lstDecision As New List(Of String)  '0= false, 1= true

        PUNCH1 = cpu_worker.GET_ACTUALTIME(staff_raw_logs, sql_worker.GET_BEGINING_IN(TimeTableList(0)), sql_worker.GET_ENDING_IN(TimeTableList(0)), True)

        If Not PUNCH1 = "NTR" Then
            lstDecision.Add("1")
        Else
            PUNCH1 = ""
            lstDecision.Add("0")
        End If

        ''GET IN ADVANCE THE VALUE OF PUNCH4 TO DETERMINE
        ''THE BINARY DECESION FOR 1011 AND 1101(BINARY DECISION IS JUST A COMBINATION OF PUNCH1,PUNC2,PUNC3,PUNC4)
        PUNCH4 = cpu_worker.GET_ACTUALTIME(staff_raw_logs, sql_worker.GET_BEGINING_OUT(TimeTableList(1)), sql_worker.GET_ENDING_OUT(TimeTableList(1)), False)
        lunchBreakPunches = cpu_worker.GET_LUNCHBREAKLOGS(staff_raw_logs, sql_worker.GET_BEGINING_OUT(TimeTableList(0)), sql_worker.GET_ENDING_IN(TimeTableList(1)))
      
        If Not lunchBreakPunches.Count = 0 Then
            If lunchBreakPunches.Count = 1 Then
                '''one puch
                ''HERE PUNCH1 IS ALREADY CLEARED ABOVE
                ''NOW WE MUST FILTER PUNCH1 VIA BLANK VALUE

                If PUNCH1 <> "" And PUNCH4 <> "NTR" Then
                    ''1001
                    lstDecision.Add("1")
                    lstDecision.Add("0")
                    PUNCH2 = lunchBreakPunches.First
                    PUNCH3 = ""
                ElseIf PUNCH1 <> "" And PUNCH4 = "NTR" Then
                    ''1000
                    lstDecision.Add("1")
                    lstDecision.Add("0")
                    PUNCH2 = lunchBreakPunches.First
                    PUNCH3 = ""
                ElseIf PUNCH1 = "" And PUNCH4 <> "NTR" Then
                    ''0001
                    lstDecision.Add("0")
                    lstDecision.Add("1")
                    PUNCH2 = ""
                    PUNCH3 = lunchBreakPunches.First
                ElseIf PUNCH1 = "" And PUNCH4 = "NTR" Then
                    lstDecision.Add("0")
                    lstDecision.Add("1")
                    PUNCH2 = ""
                    PUNCH3 = lunchBreakPunches.First

                End If

            Else
                '''
                lstDecision.Add("1")
                lstDecision.Add("1")
                PUNCH2 = lunchBreakPunches.First
                PUNCH3 = lunchBreakPunches.Last
            End If
        Else
            lstDecision.Add("0")
            lstDecision.Add("0")
            PUNCH2 = ""
            PUNCH3 = ""
        End If


        If Not PUNCH4 = "NTR" Then
            lstDecision.Add("1")
        Else
            lstDecision.Add("0")
            PUNCH4 = ""
        End If
        Console.WriteLine(String.Join("<>", lunchBreakPunches.ToArray))
        Console.WriteLine(String.Join("<>", lstDecision.ToArray))
        Console.WriteLine(PUNCH1 & " + " & PUNCH2 & " + " & PUNCH3 & " + " & PUNCH4)
        Dim dilg_rslt As New List(Of String)
        Dim b As Date = "00:00"
        Select Case String.Join("", lstDecision.ToArray)
            Case "1111"
                dilg_rslt = F_0B00000001(PUNCH1, PUNCH2, PUNCH3, PUNCH4, "1111")
                Daily_LATE = dilg_rslt(0)
                Daily_LATE_TOTAL = dilg_rslt(1)
                Daily_UT = dilg_rslt(2)
                Daily_UT_TOTAL = dilg_rslt(3)

                PUNCH1 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, Format(CDate(PUNCH1), "hh:mmtt"))
                PUNCH2 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, Format(CDate(PUNCH2), "hh:mmtt"))
                PUNCH3 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, Format(CDate(PUNCH3), "hh:mmtt"))
                PUNCH4 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, Format(CDate(PUNCH4), "hh:mmtt"))


            Case "1001"
                dilg_rslt = F_0B00000001(PUNCH1, b, b, PUNCH4, "1001")
                Daily_LATE = dilg_rslt(0)
                Daily_LATE_TOTAL = dilg_rslt(1)
                Daily_UT = dilg_rslt(2)
                Daily_UT_TOTAL = dilg_rslt(3)
                PUNCH1 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, Format(CDate(PUNCH1), "hh:mmtt"))
                PUNCH4 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, Format(CDate(PUNCH4), "hh:mmtt"))
                'Daily_REMARKS = "-incomplete"
                Daily_REMARKS = My.Settings.dilg_0b1001
            Case "1000"
                dilg_rslt = F_0B00000001(PUNCH1, b, b, b, "1000")
                Daily_LATE = dilg_rslt(0)
                Daily_LATE_TOTAL = dilg_rslt(1)
                Daily_UT = dilg_rslt(2)
                Daily_UT_TOTAL = dilg_rslt(3)
                PUNCH1 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, Format(CDate(PUNCH1), "hh:mmtt"))
                'Daily_REMARKS = "-incomplete"
                Daily_REMARKS = My.Settings.dilg_0b1000

            Case "0001"
                dilg_rslt = F_0B00000001(b, b, b, PUNCH4, "0001")
                Daily_LATE = dilg_rslt(0)
                Daily_LATE_TOTAL = dilg_rslt(1)
                Daily_UT = dilg_rslt(2)
                Daily_UT_TOTAL = dilg_rslt(3)
                PUNCH4 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, Format(CDate(PUNCH4), "hh:mmtt"))
                'Daily_REMARKS = "-incomplete"
                Daily_REMARKS = My.Settings.dilg_0b0001
            Case "0011"
                dilg_rslt = F_0B00000001(b, b, PUNCH3, PUNCH4, "0011")
                Daily_LATE = dilg_rslt(0)
                Daily_LATE_TOTAL = dilg_rslt(1)
                Daily_UT = dilg_rslt(2)
                Daily_UT_TOTAL = dilg_rslt(3)
                PUNCH3 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, Format(CDate(PUNCH3), "hh:mmtt"))
                PUNCH4 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, Format(CDate(PUNCH4), "hh:mmtt"))
                'Daily_REMARKS = "-halfday_morning"
                Daily_REMARKS = My.Settings.dilg_0b0011

            Case "1100"

                dilg_rslt = F_0B00000001(PUNCH1, PUNCH2, b, b, "1100")
                Daily_LATE = dilg_rslt(0)
                Daily_LATE_TOTAL = dilg_rslt(1)
                Daily_UT = dilg_rslt(2)
                Daily_UT_TOTAL = dilg_rslt(3)
                PUNCH1 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, Format(CDate(PUNCH1), "hh:mmtt"))
                PUNCH2 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, Format(CDate(PUNCH2), "hh:mmtt"))
                'Daily_REMARKS = "-halfday_afternoon"
                Daily_REMARKS = My.Settings.dilg_0b1100
            Case "1101"
                dilg_rslt = F_0B00000001(PUNCH1, PUNCH2, b, PUNCH4, "1101")
                Daily_LATE = dilg_rslt(0)
                Daily_LATE_TOTAL = dilg_rslt(1)
                Daily_UT = dilg_rslt(2)
                Daily_UT_TOTAL = dilg_rslt(3)
                PUNCH1 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, Format(CDate(PUNCH1), "hh:mmtt"))
                PUNCH2 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, Format(CDate(PUNCH2), "hh:mmtt"))
                PUNCH4 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, Format(CDate(PUNCH4), "hh:mmtt"))
                'Daily_REMARKS = "-incomplete"
                Daily_REMARKS = My.Settings.dilg_0b1101
            Case "0111"
                dilg_rslt = F_0B00000001(b, PUNCH2, PUNCH3, PUNCH4, "0111")
                Daily_LATE = dilg_rslt(0)
                Daily_LATE_TOTAL = dilg_rslt(1)
                Daily_UT = dilg_rslt(2)
                Daily_UT_TOTAL = dilg_rslt(3)

                PUNCH2 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, Format(CDate(PUNCH2), "hh:mmtt"))
                PUNCH3 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, Format(CDate(PUNCH3), "hh:mmtt"))
                PUNCH4 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, Format(CDate(PUNCH4), "hh:mmtt"))
                'Daily_REMARKS = "-incomplete"
                Daily_REMARKS = My.Settings.dilg_0b0111
            Case "1110"
                dilg_rslt = F_0B00000001(PUNCH1, PUNCH2, PUNCH3, b, "1110")
                Daily_LATE = dilg_rslt(0)
                Daily_LATE_TOTAL = dilg_rslt(1)
                Daily_UT = dilg_rslt(2)
                Daily_UT_TOTAL = dilg_rslt(3)

                PUNCH1 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, Format(CDate(PUNCH1), "hh:mmtt"))
                PUNCH2 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, Format(CDate(PUNCH2), "hh:mmtt"))
                PUNCH3 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, Format(CDate(PUNCH3), "hh:mmtt"))
                'Daily_REMARKS = "-incomplete"
                Daily_REMARKS = My.Settings.dilg_0b1110
            Case "1011"
                dilg_rslt = F_0B00000001(PUNCH1, b, PUNCH3, PUNCH4, "1011")
                Daily_LATE = dilg_rslt(0)
                Daily_LATE_TOTAL = dilg_rslt(1)
                Daily_UT = dilg_rslt(2)
                Daily_UT_TOTAL = dilg_rslt(3)
                PUNCH1 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, Format(CDate(PUNCH1), "hh:mmtt"))
                PUNCH3 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, Format(CDate(PUNCH3), "hh:mmtt"))
                PUNCH4 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, Format(CDate(PUNCH4), "hh:mmtt"))
                'Daily_REMARKS = "-incomplete"
                Daily_REMARKS = My.Settings.dilg_0b1011
            Case "0110"
                dilg_rslt = F_0B00000001(b, PUNCH2, PUNCH3, b, "0110")
                Daily_LATE = dilg_rslt(0)
                Daily_LATE_TOTAL = dilg_rslt(1)
                Daily_UT = dilg_rslt(2)
                Daily_UT_TOTAL = dilg_rslt(3)
                PUNCH2 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, Format(CDate(PUNCH2), "hh:mmtt"))
                PUNCH3 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, Format(CDate(PUNCH3), "hh:mmtt"))
                'Daily_REMARKS = "-incomplete"
                Daily_REMARKS = My.Settings.dilg_0b0110
            Case "0010"
                dilg_rslt = F_0B00000001(b, b, PUNCH3, b, "0010")
                Daily_LATE = dilg_rslt(0)
                Daily_LATE_TOTAL = dilg_rslt(1)
                Daily_UT = dilg_rslt(2)
                Daily_UT_TOTAL = dilg_rslt(3)
                PUNCH3 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, Format(CDate(PUNCH3), "hh:mmtt"))
                'Daily_REMARKS = "-incomplete"
                Daily_REMARKS = My.Settings.dilg_0b0010
            Case "0100"
                dilg_rslt = F_0B00000001(b, PUNCH2, b, b, "0100")
                Daily_LATE = dilg_rslt(0)
                Daily_LATE_TOTAL = dilg_rslt(1)
                Daily_UT = dilg_rslt(2)
                Daily_UT_TOTAL = dilg_rslt(3)
                PUNCH2 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, Format(CDate(PUNCH2), "hh:mmtt"))
                'Daily_REMARKS = "-incomplete"
                Daily_REMARKS = My.Settings.dilg_0b0100

        End Select

        '''POPULATE DTR TABLE
        sql_worker.INSERT_INTO_DTR_TABLE(staff_id, staffname, departmentname,
                                         _day, PUNCH1, PUNCH2, PUNCH3, PUNCH4,
                                         week, dtr_remarks, monthly_remarks, TotalUnderTime_daily, UNDERHH, UNDERMM,
                                         TotalUnderTime_daily, TOTAL_OVERTIME, TOTAL_OT_DAILY_REMARKS,
                                         GlobalVariables.OFFICIAL_ARRIVAL, GlobalVariables.OFFICIAL_DEPARTURE, Daily_UT, Daily_LATE, Daily_REMARKS,
                                         Daily_LATE_TOTAL, Daily_UT_TOTAL, "", "", designation)
    End Sub

    Public Function F_0B00000001(rwlog1 As String, rwlog2 As String, rwlog3 As String, rwlog4 As String, b_status As String) As List(Of String)
        ''DETERMINE WHERE TO APPLY THE FLEXI
        ''THIS RULES APPLY FOR DILG ONLY 7:30 to 16:30

        ''RETURN VALUE AS LIST(DAILY_LATE, DAILY_LATE_TOTAL, DAILY_UT, DAILY_UT_TOTAL)

        Dim rtrn_lst As New List(Of String)

        Dim D_LATE As Object = 0
        Dim D_LATE_TTL As Object = 0
        Dim D_UT As Object = 0
        Dim D_UT_TTL As Object = 0
        
        '''DETERMINE UT & LATE FOR PX2 AND PX3
        '''PX just only varibles for punch
        '''1111     - flexi algorthm
        '''1001     
        '''1000
        '''0001
        '''0011     
        '''1100
        '''1101
        '''1011
        '''0110
        '''0010
        '''0100










        Select Case b_status

            Case "1111"

                D_UT = cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_OUT(TimeTableList(0)), sql_worker.GET_OFF_DUTY_TIME(TimeTableList(0)), rwlog2, False, True)
                D_UT += cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_OUT(TimeTableList(1)), GET_DILG_NEW_COUT(rwlog1), rwlog4, False, True)

                D_LATE = cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_IN(TimeTableList(0)), sql_worker.GET_ON_DUTY_TIME(TimeTableList(0)), rwlog1, True, False)
                D_LATE += cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_IN(TimeTableList(1)), sql_worker.GET_ON_DUTY_TIME(TimeTableList(1)), rwlog3, True, False)

                D_UT_TTL = D_UT
                D_LATE_TTL += D_LATE
            Case "0011"
                D_LATE = cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_IN(TimeTableList(1)), sql_worker.GET_ON_DUTY_TIME(TimeTableList(1)), rwlog3, True, False)
                D_LATE_TTL = D_LATE
                D_UT = cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_OUT(TimeTableList(1)), sql_worker.GET_OFF_DUTY_TIME(TimeTableList(1)), rwlog4, False, True) + (sql_worker.GET_TOTAL_WORKING_HOURS(TimeTableList(1)) * 60)
                D_UT_TTL = D_UT
            Case "1100"

                D_LATE = cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_IN(TimeTableList(0)), sql_worker.GET_ON_DUTY_TIME(TimeTableList(0)), rwlog1, True, False)
                D_LATE_TTL = D_LATE
                D_UT = cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_OUT(TimeTableList(0)), sql_worker.GET_OFF_DUTY_TIME(TimeTableList(0)), rwlog2, False, True) + (sql_worker.GET_TOTAL_WORKING_HOURS(TimeTableList(1)) * 60)
                D_UT_TTL = D_UT
            Case "1101"
                D_LATE = cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_IN(TimeTableList(0)), sql_worker.GET_ON_DUTY_TIME(TimeTableList(0)), rwlog1, True, False)
                D_LATE_TTL = D_LATE
                D_UT = cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_OUT(TimeTableList(0)), sql_worker.GET_OFF_DUTY_TIME(TimeTableList(0)), rwlog2, False, True)
                D_UT_TTL = D_UT
            Case "1011"
                D_LATE = cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_IN(TimeTableList(0)), sql_worker.GET_ON_DUTY_TIME(TimeTableList(0)), rwlog1, True, False)

                D_LATE += cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_IN(TimeTableList(1)), sql_worker.GET_ON_DUTY_TIME(TimeTableList(1)), rwlog3, True, False)
                D_UT_TTL = D_LATE

            Case "1001"
                D_LATE = cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_IN(TimeTableList(0)), sql_worker.GET_ON_DUTY_TIME(TimeTableList(0)), rwlog1, True, False)
                D_LATE_TTL = D_LATE
                D_UT = cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_OUT(TimeTableList(1)), GET_DILG_NEW_COUT(rwlog1), rwlog4, False, True)
                D_UT_TTL = D_UT
            Case "0111"
                D_LATE = cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_IN(TimeTableList(1)), sql_worker.GET_ON_DUTY_TIME(TimeTableList(1)), rwlog3, True, False)
                D_LATE_TTL = D_LATE
                D_UT = cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_OUT(TimeTableList(1)), sql_worker.GET_OFF_DUTY_TIME(TimeTableList(1)), rwlog4, False, True) + (sql_worker.GET_TOTAL_WORKING_HOURS(TimeTableList(1)) * 60)
                D_UT_TTL = D_UT
            Case "1110"
                D_LATE = cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_IN(TimeTableList(0)), sql_worker.GET_ON_DUTY_TIME(TimeTableList(0)), rwlog1, True, False)
                D_LATE_TTL = D_LATE
                D_UT = cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_OUT(TimeTableList(0)), sql_worker.GET_OFF_DUTY_TIME(TimeTableList(0)), rwlog2, False, True) + (sql_worker.GET_TOTAL_WORKING_HOURS(TimeTableList(0)) * 60)
                D_UT_TTL = D_UT

        End Select


        


        If Not D_LATE_TTL = 0 Then
            Dim Hours As Integer = Math.Floor(D_LATE_TTL / 60)
            Dim Minutes As Integer = D_LATE Mod 60
            ''FORMAT THE VALUE FOR DTR TOTAL UNDERTIME OF EMPLOYEE MONTHLY
            If Not Hours = 0 Then
                If Not Minutes = 0 Then
                    D_LATE = Hours & "h" & Minutes & "m"
                Else
                    D_LATE = Hours & "h"
                End If
            Else
                D_LATE = Minutes & "m"
            End If
        Else
            D_LATE = ""
        End If

        If Not D_UT_TTL = 0 Then
            Dim Hours As Integer = Math.Floor(D_UT_TTL / 60)
            Dim Minutes As Integer = D_UT Mod 60

            ''FORMAT THE VALUE FOR DTR TOTAL UNDERTIME OF EMPLOYEE MONTHLY
            If Not Hours = 0 Then
                If Not Minutes = 0 Then
                    D_UT = Hours & "h" & Minutes & "m"
                Else
                    D_UT = Hours & "h"
                End If

            Else
                D_UT = Minutes & "m"
            End If
        Else
            D_UT = ""
        End If


  
        rtrn_lst.Add(D_LATE)
        rtrn_lst.Add(D_LATE_TTL)
        rtrn_lst.Add(D_UT)
        rtrn_lst.Add(D_UT_TTL)
        Return rtrn_lst

    End Function

    Function GET_DILG_NEW_COUT(c_in As Object)
        ''THIS FUNCTION RETURNS EXACT OUT OF AN EMPLOYEE IF HE/SHE HAS CLOCK IN
        ''IN THE MORNING. THIS ALGORITHM IS BASED ON CUSTOMER REQUIREMENTS WHERE
        ''THEY HAVE FLEXIBLE SCHEDULE EVERY TUE-WED-THU-FRI THAT STARTS AT 7:30 AM TO 8:30 AM.
        ''IF THE EMPLOYEE CLOCK IN WITHIN THE FLEXI SCHED OF 7:30AM TO 8:30AM
        ''FOR EXAMPLE CLOCK-IN IS 7:45AM SO THE RETURN NEW OUT TIME OF THE EMPLOYEE WOULD BE 4:45
        ''IF THE EMPLOYEE CLOCK-IN IS 7:20AM THE RETURN NEW OUT TIME IS 4:30 FIXED BECAUSE IT DOES NOT FALL WITHIN THE FLEXI RULES.(7:30 TO 8:30)
        ''IF THE EMPLOYEE CLOCK-IN IS 8:31AM THE NEW OUT TIME WOULD BE 5:30 FIXED BECAUSE IT DOES NOT FALL TO THE FLEXI RULES.
        ''NOTE THIS ALGORITHM IS USED IN DILG SAN FERNANDO REGION 1 ONLY. SINCE THIS ALGORITHM IS A REVISION OF 
        ''THE FIRST ALGROTHM I CREATED FOR DILG HEAD OFFICE REFER TO JUMP FUNCTION "DILG_QUEZON_AVE:" apx. line 841.
        Dim result As Object

 
      
        If Not Format(_day, "dddd") = "Monday" Then
            Dim lst_flex As New List(Of String)
            lst_flex = cpu_worker.Extractminutes("7:30", "8:30")
            If lst_flex.Contains(Format(CDate(c_in), "HH:mm")) Then
                result = CDate(c_in).AddHours(9)
            Else
                If CInt(Format(CDate(c_in), "HH:mm").ToString.Replace(":", "")) < 730 Then
                    result = "16:30"
                ElseIf CInt(Format(CDate(c_in), "HH:mm").ToString.Replace(":", "")) > 830 Then
                    result = "17:30"
                End If
            End If
        Else
            ''THIS IS MONDAY 8:00 TO 5:00 NO FLEXI RULES
            result = sql_worker.GET_OFF_DUTY_TIME(TimeTableList(1))
        End If


     
        Console.WriteLine(CInt(Format(CDate(c_in), "HH:mm").ToString.Replace(":", "")))
        Console.WriteLine("DILG ACTUAL IN:" & Format(CDate(c_in), "hh:mm tt"))
        Console.WriteLine("DILG NEW OUT:" & Format(CDate(result), "hh:mm tt"))

        Return result
    End Function


















    Public Sub PATROL_117_1A()


        ''PATTERN COMBINATION
        ''MODIFIED: 11-28-2015
        ''am_td-pm_td-am_tw
        ''0-0-0 =   absent/ignore
        ''0-0-1 =   

        Dim pm_ty, am_td, pm_td, am_tw As String
        Dim a1 As String = ""
        Dim a2 As String = ""
        Dim p1 As String = ""
        Dim p2 As String = ""
        pm_ty = sql_worker.GET_CIN_OR_COUT(staff_id, _day.AddDays(-1), _day.AddDays(-1),
                                           sql_worker.GET_BEGINING_OUT(TimeTableList(0)), sql_worker.GET_ENDING_OUT(TimeTableList(0)), False)
        am_td = sql_worker.GET_CIN_OR_COUT(staff_id, _day, _day,
                                           sql_worker.GET_BEGINING_IN(TimeTableList(0)), sql_worker.GET_ENDING_IN(TimeTableList(0)), False)
        pm_td = sql_worker.GET_CIN_OR_COUT(staff_id, _day, _day,
                                            sql_worker.GET_BEGINING_OUT(TimeTableList(0)), sql_worker.GET_ENDING_OUT(TimeTableList(0)), False)
        am_tw = sql_worker.GET_CIN_OR_COUT(staff_id, _day.AddDays(1), _day.AddDays(1),
                                           sql_worker.GET_BEGINING_OUT(TimeTableList(1)), sql_worker.GET_ENDING_OUT(TimeTableList(1)), False)


        If Not (String.IsNullOrEmpty(pm_td)) And Not (String.IsNullOrEmpty(am_tw)) And (String.IsNullOrEmpty(am_td)) Then
            ''0-1-1 = 2nd shift confirm & COMPLETE
            p1 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, Format(CDate(pm_td), "hh:mmtt"))
            a2 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day.AddDays(1), _day.AddDays(1), Format(CDate(am_tw), "hh:mmtt"))

            Daily_LATE_TOTAL = cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_IN(TimeTableList(1)), sql_worker.GET_ON_DUTY_TIME(TimeTableList(1)), pm_td, True, False)
            Daily_LATE = cpu_worker.CONVERT_TO_ETA(Daily_LATE_TOTAL)
            Daily_UT_TOTAL = cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_OUT(TimeTableList(1)), sql_worker.GET_OFF_DUTY_TIME(TimeTableList(1)), am_tw, False, True)
            Daily_UT = cpu_worker.CONVERT_TO_ETA(Daily_UT_TOTAL)

            Console.WriteLine(pm_td & "<>" & am_tw)

            ''THIS INSERT CODE IS FOR NEXT DAY OUT SINCE WE ALREADY DETERMINE THE SHIFT OF THE STAFF
            ''WE ARE GOING TO INSERT THIS DETAILS TODAY AND TOMMOROW. PM IN AND AM OUT FOR NEXT DAY.
            ''WE DO NOT INCLUDE ANYMORE THE LATE FOR NEXT DAY INSERT ONLY UNDERTIME.
        
            sql_worker.INSERT_INTO_DTR_TABLE(staff_id, staffname, departmentname,
                                      _day.AddDays(1), "", a2, "", "",
                                      _day.AddDays(1).ToString("d ddd", CultureInfo.InvariantCulture), dtr_remarks, monthly_remarks, 0, "", "",
                                      0, 0, "",
                                     GlobalVariables.OFFICIAL_ARRIVAL, GlobalVariables.OFFICIAL_DEPARTURE, Daily_UT, "", "",
                                     0, Daily_UT_TOTAL, "", "", designation)
            ''RESET THE UNDERTIME TO AVOID DUPLICATE ENTRY
            Daily_UT_TOTAL = 0
            Daily_UT = ""
            p2 = ""
            a2 = ""

            'sql_worker.INSERT_INTO_DTR_TABLE(staff_id, staffname, departmentname,
            '                          _day, a1, a2, p1, p2,
            '                          week, dtr_remarks, monthly_remarks, TotalUnderTime_daily, UNDERHH, UNDERMM,
            '                          TotalUnderTime_daily, TOTAL_OVERTIME, TOTAL_OT_DAILY_REMARKS,
            '                         GlobalVariables.OFFICIAL_ARRIVAL, GlobalVariables.OFFICIAL_DEPARTURE, Daily_UT, Daily_LATE, Daily_REMARKS,
            '                         Daily_LATE_TOTAL, Daily_UT_TOTAL, "", "", designation)

        ElseIf Not (String.IsNullOrEmpty(am_td)) And Not (String.IsNullOrEmpty(pm_td)) And (String.IsNullOrEmpty(am_tw)) Then
            ''1-1-0 = 1st shift confirm & COMPLETE
            a1 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, Format(CDate(am_td), "hh:mmtt"))
            p2 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, Format(CDate(pm_td), "hh:mmtt"))

            Daily_LATE_TOTAL = cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_IN(TimeTableList(0)), sql_worker.GET_ON_DUTY_TIME(TimeTableList(0)), am_td, True, False)
            Daily_LATE = cpu_worker.CONVERT_TO_ETA(Daily_LATE_TOTAL)

            Daily_UT_TOTAL = cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_OUT(TimeTableList(0)), sql_worker.GET_OFF_DUTY_TIME(TimeTableList(0)), pm_td, False, True)
            Daily_UT = cpu_worker.CONVERT_TO_ETA(Daily_UT_TOTAL)


            Console.WriteLine(am_td & "<>" & pm_td)


        Else
            ''INCOMPLETE PATTERN COMBINATION
            If Not String.IsNullOrEmpty(pm_td) And String.IsNullOrEmpty(am_td) And String.IsNullOrEmpty(am_tw) Then
                ''0-1-0
                a2 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, Format(CDate(pm_td), "hh:mmtt"))
                Daily_REMARKS = "-incomplete"
            ElseIf Not String.IsNullOrEmpty(am_td) And String.IsNullOrEmpty(pm_td) And String.IsNullOrEmpty(am_tw) Then
                ''1-0-0
                ''CHECK IF THE USER HAS LOGS YESTERDAY
                ''IF TRUE DO NOT DISPLAY THE a1
                If String.IsNullOrEmpty(pm_ty) Then
                    a1 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, Format(CDate(am_td), "hh:mmtt"))
                    Daily_REMARKS = "-incomplete"
                End If
            ElseIf Not String.IsNullOrEmpty(am_td) And String.IsNullOrEmpty(pm_td) And Not String.IsNullOrEmpty(am_tw) Then
                If String.IsNullOrEmpty(pm_ty) Then
                    ''1-0-1
                    a1 = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, Format(CDate(am_td), "hh:mmtt"))
                    Daily_REMARKS = "-incomplete"
                End If
            ElseIf Not String.IsNullOrEmpty(am_td) And Not String.IsNullOrEmpty(pm_td) And Not String.IsNullOrEmpty(am_tw) Then
                ''1-1-1
                a1 = Format(CDate(am_td), "hh:mmtt")
                a2 = Format(CDate(pm_td), "hh:mmtt")
                p1 = Format(CDate(pm_td), "hh:mmtt")
                p2 = Format(CDate(am_tw), "hh:mmtt")
            End If

        End If
        Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>" & am_td & "<>" & pm_td & "<>" & am_tw)


        sql_worker.INSERT_INTO_DTR_TABLE(staff_id, staffname, departmentname,
                                  _day, a1, a2, p1, p2,
                                  week, dtr_remarks, monthly_remarks, TotalUnderTime_daily, UNDERHH, UNDERMM,
                                  TotalUnderTime_daily, TOTAL_OVERTIME, TOTAL_OT_DAILY_REMARKS,
                                 GlobalVariables.OFFICIAL_ARRIVAL, GlobalVariables.OFFICIAL_DEPARTURE, Daily_UT, Daily_LATE, Daily_REMARKS,
                                 Daily_LATE_TOTAL, Daily_UT_TOTAL, "", "", designation)



    End Sub


    ''++++++++++++++++++++++++++++
    ''THIS IS FOR DILG-SG-JO(SEC GUARD JOB ORDER) 
    ''1SHFIT 6AM-6PM AND 2NDSHIFT 6PM-6AM
    ''++++++++++++++++++++++++++
    Public Sub AUTO_SCHED_2()
        Dim Px1TD As String = ""
        Dim Px2TD As String = ""
        Dim Px1TW As String = ""
        Dim Px2TW As String = ""

        '''GET PUNCHES
        Console.WriteLine("1st SHIFT Start:-----------")
        Px1TD = sql_worker.GET_CIN(staff_id, _day, _day, sql_worker.GET_BEGINING_IN(TimeTableList(0)), sql_worker.GET_ENDING_IN(TimeTableList(0)), True)
        Px2TD = sql_worker.GET_COUT(staff_id, _day, _day, sql_worker.GET_BEGINING_OUT(TimeTableList(0)), sql_worker.GET_ENDING_OUT(TimeTableList(0)), True)
        Console.WriteLine("1st SHIFT End:------------")
        Console.WriteLine(" ")
        Console.WriteLine("2nd SHIFT Start:------------")
        Px1TW = sql_worker.GET_CIN(staff_id, _day, _day, sql_worker.GET_BEGINING_IN(TimeTableList(1)), sql_worker.GET_ENDING_IN(TimeTableList(1)), True)
        Px2TW = sql_worker.GET_COUT(staff_id, _day.AddDays(1), _day.AddDays(1), sql_worker.GET_BEGINING_OUT(TimeTableList(1)), sql_worker.GET_ENDING_OUT(TimeTableList(1)), False)
        Console.WriteLine("2nd SHIFT End:--------------")



        If Px1TD <> "" Then
            Px1TD = Format(CDate(Px1TD), "hh:mmtt")
        End If

        If Px2TD <> "" Then
            Px2TD = Format(CDate(Px2TD), "hh:mmtt")
        End If
        If Px1TW <> "" Then
            Px1TW = Format(CDate(Px1TW), "hh:mmtt")
        End If
        If Px2TW <> "" Then
            Px2TW = Format(CDate(Px2TW), "hh:mmtt")
        End If



        sql_worker.INSERT_INTO_DTR_TABLE(staff_id, staffname, departmentname,
                                         _day, Px1TD, Px2TD, Px1TW, Px2TW,
                                         week, dtr_remarks, monthly_remarks, TotalUnderTime_daily, UNDERHH, UNDERMM,
                                         TotalUnderTime_daily, TOTAL_OVERTIME, TOTAL_OT_DAILY_REMARKS,
                                        GlobalVariables.OFFICIAL_ARRIVAL, GlobalVariables.OFFICIAL_DEPARTURE, Daily_UT, Daily_LATE, Daily_REMARKS,
                                        Daily_LATE_TOTAL, Daily_UT_TOTAL, "", "", designation)




    End Sub

    Public Sub AUTO_SCHED_3()
        Dim Px1TDa As String = ""
        Dim Px2TDa As String = ""
        Dim Px1TDb As String = ""
        Dim Px2TDb As String = ""

        Dim Px1TW As String = ""
        Dim Px2TW As String = ""

        '''GET PUNCHES
        Console.WriteLine("1st SHIFT Start:-----------")
        Px1TDa = sql_worker.GET_CIN(staff_id, _day, _day, sql_worker.GET_BEGINING_IN(TimeTableList(0)), sql_worker.GET_ENDING_IN(TimeTableList(0)), True)
        Px2TDa = sql_worker.GET_COUT(staff_id, _day, _day, sql_worker.GET_BEGINING_OUT(TimeTableList(0)), sql_worker.GET_ENDING_OUT(TimeTableList(0)), True)
        Console.WriteLine("1st SHIFT End:------------")
        Console.WriteLine(" ")

        Console.WriteLine("2nd SHIFT Start:-----------")
        Px1TDb = sql_worker.GET_CIN(staff_id, _day, _day, sql_worker.GET_BEGINING_IN(TimeTableList(1)), sql_worker.GET_ENDING_IN(TimeTableList(1)), True)
        Px2TDb = sql_worker.GET_COUT(staff_id, _day, _day, sql_worker.GET_BEGINING_OUT(TimeTableList(1)), sql_worker.GET_ENDING_OUT(TimeTableList(1)), True)
        Console.WriteLine("2nd SHIFT End:------------")
        Console.WriteLine(" ")


        Console.WriteLine("3rd SHIFT Start:------------")
        Px1TW = sql_worker.GET_CIN(staff_id, _day, _day, sql_worker.GET_BEGINING_IN(TimeTableList(2)), sql_worker.GET_ENDING_IN(TimeTableList(2)), True)
        Px2TW = sql_worker.GET_COUT(staff_id, _day.AddDays(1), _day.AddDays(1), sql_worker.GET_BEGINING_OUT(TimeTableList(2)), sql_worker.GET_ENDING_OUT(TimeTableList(2)), False)
        Console.WriteLine("3rd SHIFT End:--------------")


        ''FIRST SHIFT
        ''
        If Px1TDa <> "" And Px2TDa <> "" Then
            Px1TDa = Format(CDate(Px1TDa), "hh:mmtt")
            Px2TDa = Format(CDate(Px2TDa), "hh:mmtt")
            If Px1TDb <> "" And Px2TDb <> "" Then
                Px1TDb = Format(CDate(Px1TDb), "hh:mmtt")
                Px2TDb = Format(CDate(Px2TDb), "hh:mmtt")
            Else
                If Not String.IsNullOrEmpty(Px1TDb) Then
                    Px1TDb = Format(CDate(Px1TDb), "hh:mmtt")
                ElseIf Not String.IsNullOrEmpty(Px2TDb) Then
                    Px2TDb = Format(CDate(Px2TDb), "hh:mmtt")
                End If
            End If


            sql_worker.INSERT_INTO_DTR_TABLE(staff_id, staffname, departmentname,
                                             _day, Px1TDa, Px2TDa, Px1TDb, Px2TDb,
                                             week, dtr_remarks, monthly_remarks, TotalUnderTime_daily, UNDERHH, UNDERMM,
                                             TotalUnderTime_daily, TOTAL_OVERTIME, TOTAL_OT_DAILY_REMARKS,
                                            GlobalVariables.OFFICIAL_ARRIVAL, GlobalVariables.OFFICIAL_DEPARTURE, Daily_UT, Daily_LATE, Daily_REMARKS,
                                            Daily_LATE_TOTAL, Daily_UT_TOTAL, "", "", designation)

        ElseIf Px1TDb <> "" And Px2TDb <> "" Then
            Px1TDb = Format(CDate(Px1TDb), "hh:mmtt")
            Px2TDb = Format(CDate(Px2TDb), "hh:mmtt")
            If Px1TW <> "" And Px2TW <> "" Then
                Px1TW = Format(CDate(Px1TW), "hh:mmtt")
                Px2TW = Format(CDate(Px2TW), "hh:mmtt")
            Else
                If Not String.IsNullOrEmpty(Px1TW) Then
                    Px1TW = Format(CDate(Px1TW), "hh:mmtt")
                ElseIf Not String.IsNullOrEmpty(Px2TW) Then
                    Px2TW = Format(CDate(Px2TW), "hh:mmtt")
                End If
            End If
            sql_worker.INSERT_INTO_DTR_TABLE(staff_id, staffname, departmentname,
                                             _day, Px1TDb, Px2TDb, Px1TW, Px2TW,
                                             week, dtr_remarks, monthly_remarks, TotalUnderTime_daily, UNDERHH, UNDERMM,
                                             TotalUnderTime_daily, TOTAL_OVERTIME, TOTAL_OT_DAILY_REMARKS,
                                            GlobalVariables.OFFICIAL_ARRIVAL, GlobalVariables.OFFICIAL_DEPARTURE, Daily_UT, Daily_LATE, Daily_REMARKS,
                                            Daily_LATE_TOTAL, Daily_UT_TOTAL, "", "", designation)

        ElseIf Px1TW <> "" And Px2TW <> "" Then
            Px1TW = Format(CDate(Px1TW), "hh:mmtt")
            Px2TW = Format(CDate(Px2TW), "hh:mmtt")
            sql_worker.INSERT_INTO_DTR_TABLE(staff_id, staffname, departmentname,
                                             _day, "", "", Px1TW, Px2TW,
                                             week, dtr_remarks, monthly_remarks, TotalUnderTime_daily, UNDERHH, UNDERMM,
                                             TotalUnderTime_daily, TOTAL_OVERTIME, TOTAL_OT_DAILY_REMARKS,
                                            GlobalVariables.OFFICIAL_ARRIVAL, GlobalVariables.OFFICIAL_DEPARTURE, Daily_UT, Daily_LATE, Daily_REMARKS,
                                            Daily_LATE_TOTAL, Daily_UT_TOTAL, "", "", designation)


        Else
            If Not String.IsNullOrEmpty(Px1TDa) Then
                Px1TDa = Format(CDate(Px1TDa), "hh:mmtt")
            End If
            If Not String.IsNullOrEmpty(Px2TDa) Then
                Px2TDa = Format(CDate(Px2TDa), "hh:mmtt")
            End If
            If Not String.IsNullOrEmpty(Px1TDb) Then
                Px1TDb = Format(CDate(Px1TDb), "hh:mmtt")
            End If
            If Not String.IsNullOrEmpty(Px2TDb) Then
                Px2TDb = Format(CDate(Px2TDb), "hh:mmtt")
            End If
            If Not String.IsNullOrEmpty(Px1TW) Then
                Px1TW = Format(CDate(Px1TW), "hh:mmtt")
            End If
            If Not String.IsNullOrEmpty(Px2TW) Then
                Px2TW = Format(CDate(Px2TW), "hh:mmtt")
            End If
            sql_worker.INSERT_INTO_DTR_TABLE(staff_id, staffname, departmentname,
                                             _day, Px1TDa, Px2TDa, Px1TDb, Px2TDb,
                                             week, dtr_remarks, monthly_remarks, TotalUnderTime_daily, UNDERHH, UNDERMM,
                                             TotalUnderTime_daily, TOTAL_OVERTIME, TOTAL_OT_DAILY_REMARKS,
                                            GlobalVariables.OFFICIAL_ARRIVAL, GlobalVariables.OFFICIAL_DEPARTURE, Daily_UT, Daily_LATE, Daily_REMARKS,
                                            Daily_LATE_TOTAL, Daily_UT_TOTAL, "", "", designation)

        End If
    End Sub




    '''++++++++++++++++++++++++++++++
    '''THIS IS FOR BIR-SEC GUARD
    '''++++++++++++++++++++++++++++
    Public Sub AUTO_SCHED_1()
        ''YESTERDAY PUNCHES
        Dim P1YD As String = ""
        Dim P2YD As String = ""
        Dim P3YD As String = ""
        Dim P4YD As String = ""
        Dim P5YD As String = ""
        Dim P6YD As String = ""

        ''TODAY PUNCHES
        Dim P1TD As String = ""
        Dim P2TD As String = ""
        Dim P3TD As String = ""
        Dim P4TD As String = ""
        Dim P5TD As String = ""
        Dim P6TD As String = ""

        ''TOMMOROW PUNCHES
        Dim P1TW As String = ""
        Dim P2TW As String = ""
        Dim P3TW As String = ""
        Dim P4TW As String = ""
        Dim P5TW As String = ""
        Dim P6TW As String = ""


        ''RAW LOGS HOLDER



        ''A=GET YESTERDAY LOGS OFFSET FROM BEGINING_OUT TO 00:00(COUT) 
        ''B=AND GET TODAY OFFSET 00:00 TO ENDING_IN TOADY(CIN)
        ''A+B
        ''

        P5YD = sql_worker.GET_CIN(staff_id, _day.AddDays(-1), _day.AddDays(-1), sql_worker.GET_BEGINING_IN(TimeTableList(2)), sql_worker.GET_ENDING_IN(TimeTableList(2)), True)
        ''TO GET P6YD VALUE GET OFFSET FOR
        Dim A As String = ""
        Dim B As String = ""
        Dim C As New List(Of String)
        A = sql_worker.GET_COUT(staff_id, _day.AddDays(-1), _day.AddDays(-1), sql_worker.GET_BEGINING_OUT(TimeTableList(2)), "00:00", True)
        If Not A = "" Then
            P6YD = A
        Else
            B = sql_worker.GET_COUT(staff_id, _day, _day, "00:00", sql_worker.GET_ENDING_IN(TimeTableList(0)), True)
            P6YD = B
        End If

        ''GET P1TD VALUE(CIN FOR FIRST SHIFT)
        A = sql_worker.GET_CIN(staff_id, _day.AddDays(-1), _day.AddDays(-1), sql_worker.GET_BEGINING_OUT(TimeTableList(2)), "00:00", True)
        Console.WriteLine("     A=" & A)
        Console.WriteLine("     BEGINING_OUT: " & sql_worker.GET_BEGINING_OUT(TimeTableList(2)))
        If Not A = "" Then
            P1TD = A
        Else
            B = sql_worker.GET_CIN(staff_id, _day, _day, "00:00", sql_worker.GET_ENDING_IN(TimeTableList(0)), True)
            Console.WriteLine("     B=" & B)
            P1TD = B
        End If
        ''GET P2TD VALUE(OUT FOR FIRST SHIFT)
        P2TD = sql_worker.GET_COUT(staff_id, _day, _day, sql_worker.GET_BEGINING_OUT(TimeTableList(0)), sql_worker.GET_ENDING_OUT(TimeTableList(0)), True)
        ''GET P3TD VALUE (IN FOR SECOND SHIFT)
        P3TD = sql_worker.GET_CIN(staff_id, _day, _day, sql_worker.GET_BEGINING_IN(TimeTableList(1)), sql_worker.GET_ENDING_IN(TimeTableList(1)), True)
        P4TD = sql_worker.GET_COUT(staff_id, _day, _day, sql_worker.GET_BEGINING_OUT(TimeTableList(1)), sql_worker.GET_ENDING_OUT(TimeTableList(1)), True)

        ''GET VALUES FOR LAST SHIFT 
        Console.WriteLine("     GET P5TD")
        P5TD = sql_worker.GET_CIN(staff_id, _day, _day, sql_worker.GET_BEGINING_IN(TimeTableList(2)), sql_worker.GET_ENDING_IN(TimeTableList(2)), True)
        Console.WriteLine("     GET P6TD")
        A = sql_worker.GET_COUT(staff_id, _day, _day, sql_worker.GET_BEGINING_OUT(TimeTableList(2)), "00:00", True)
        Console.WriteLine("     A=" & A)
        Console.WriteLine("     BEGINING_OUT: " & sql_worker.GET_BEGINING_OUT(TimeTableList(2)))
        If Not A = "" Then
            P6TD = A
        Else
            B = sql_worker.GET_COUT(staff_id, _day.AddDays(1), _day.AddDays(1), "00:00", sql_worker.GET_ENDING_IN(TimeTableList(0)), True)
            Console.WriteLine("     B=" & B)
            P6TD = B
        End If

        ''GET VALUES FOR P1TW AND P2TW
        Console.WriteLine("     GET P1TW")
        A = sql_worker.GET_CIN(staff_id, _day, _day, sql_worker.GET_BEGINING_OUT(TimeTableList(2)), "00:00", True)
        Console.WriteLine("     A=" & A)
        Console.WriteLine("     BEGINING_IN: " & sql_worker.GET_BEGINING_IN(TimeTableList(0)))
        If Not A = "" Then
            P1TW = A
        Else
            B = sql_worker.GET_CIN(staff_id, _day.AddDays(1), _day.AddDays(1), "00:00", sql_worker.GET_ENDING_IN(TimeTableList(0)), True)
            Console.WriteLine("     B=" & B)
            P1TW = B
        End If
        Console.WriteLine("     GET P2TW")
        P2TW = sql_worker.GET_COUT(staff_id, _day.AddDays(1), _day.AddDays(1), sql_worker.GET_BEGINING_OUT(TimeTableList(0)), sql_worker.GET_ENDING_OUT(TimeTableList(0)), True)






        Console.WriteLine("     P5YD: " & P5YD)
        Console.WriteLine("     P6YD: " & P6YD)
        Console.WriteLine("     P1TD: " & P1TD)
        Console.WriteLine("     P2TD: " & P2TD)
        Console.WriteLine("     P3TD: " & P3TD)
        Console.WriteLine("     P4TD: " & P4TD)
        Console.WriteLine("     P5TD: " & P5TD)
        Console.WriteLine("     P6TD: " & P6TD)
        Console.WriteLine("     P1TW: " & P1TW)
        Console.WriteLine("     P2TW: " & P2TW)

        ''CALCULATION
        ''FIRST SHIFT ALGO
        If P1TD <> "" And P2TD <> "" Then
            If P5YD <> "" And P6YD <> "" Then
                P1TD = sql_worker.GET_ON_DUTY_TIME(TimeTableList(0))
                ''JUMP TO ADDRR P5TD_P6TD
                GoTo P5TD_P6TD
            Else
                If P3TD <> "" And P4TD <> "" Then
                    P2TD = sql_worker.GET_OFF_DUTY_TIME(TimeTableList(0))
                    P3TD = sql_worker.GET_ON_DUTY_TIME(TimeTableList(0))

                    TotalUnderTime_daily = cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_IN(TimeTableList(0)), sql_worker.GET_ON_DUTY_TIME(TimeTableList(0)), P1TD, True, False)
                    TotalUnderTime_daily = TotalUnderTime_daily + cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_OUT(TimeTableList(1)), sql_worker.GET_OFF_DUTY_TIME(TimeTableList(1)), P4TD, False, True)
                    P1TD = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day.AddDays(-1), _day, P1TD)
                    P2TD = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, P2TD)
                    P3TD = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, P3TD)
                    P4TD = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, P4TD)
                    Console.WriteLine("     OUTPUT = P1TD:" & P1TD & " P2TD:" & P2TD & " P3TD:" & P3TD & " P4TD:" & P4TD)
                    Console.WriteLine("     TOTAL UNDERTIME: " & TotalUnderTime_daily)

                    UNDERHH = cpu_worker.CONVERT_TO_HH(TotalUnderTime_daily)
                    UNDERMM = cpu_worker.CONVERT_TO_MM(TotalUnderTime_daily)
                    monthly_remarks = cpu_worker.CONVERT_TO_TIME(TotalUnderTime_daily)
                    If UNDERHH = 0 Then
                        UNDERHH = ""
                    End If
                    If UNDERMM = 0 Then
                        UNDERMM = ""
                    End If
                    sql_worker.INSERT_INTO_DTR_TABLE(staff_id, staffname, departmentname,
                             _day, P1TD, P2TD, P3TD, P4TD,
                             week, dtr_remarks, monthly_remarks, TotalUnderTime_daily, UNDERHH, UNDERMM,
                             TotalUnderTime_daily, TOTAL_OVERTIME, TOTAL_OT_DAILY_REMARKS,
                             GlobalVariables.OFFICIAL_ARRIVAL, GlobalVariables.OFFICIAL_DEPARTURE, Daily_UT, Daily_LATE, Daily_REMARKS,
                             Daily_LATE_TOTAL, Daily_UT_TOTAL, "", "", designation)
                Else
P5TD_P6TD:
                    If P5TD <> "" And P6TD <> "" Then
                        ''LOOKUP TOMMOROW LOGS
                        If P1TW <> "" And P2TW <> "" Then
                            P6TD = sql_worker.GET_OFF_DUTY_TIME(TimeTableList(2))
                            TotalUnderTime_daily = cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_IN(TimeTableList(0)), sql_worker.GET_ON_DUTY_TIME(TimeTableList(0)), P1TD, True, False)
                            TotalUnderTime_daily = TotalUnderTime_daily + cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_OUT(TimeTableList(0)), sql_worker.GET_OFF_DUTY_TIME(TimeTableList(0)), P2TD, False, True)

                            TotalUnderTime_daily = TotalUnderTime_daily + cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_IN(TimeTableList(2)), sql_worker.GET_ON_DUTY_TIME(TimeTableList(2)), P5TD, True, False)
                            TotalUnderTime_daily = TotalUnderTime_daily + cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_OUT(TimeTableList(2)), sql_worker.GET_OFF_DUTY_TIME(TimeTableList(2)), P6TD, False, True)

                            P1TD = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day.AddDays(-1), _day, P1TD)
                            P2TD = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, P2TD)
                            P5TD = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, P5TD)
                            P6TD = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day.AddDays(1), P6TD)

                            Console.WriteLine("     OUTPUT = P1TD:" & P1TD & " P2TD:" & P2TD & " P5TD:" & P5TD & " P6TD:" & P6TD)
                            Console.WriteLine("     TOTAL UNDERTIME: " & TotalUnderTime_daily)
                            UNDERHH = cpu_worker.CONVERT_TO_HH(TotalUnderTime_daily)
                            UNDERMM = cpu_worker.CONVERT_TO_MM(TotalUnderTime_daily)
                            monthly_remarks = cpu_worker.CONVERT_TO_TIME(TotalUnderTime_daily)
                            If UNDERHH = 0 Then
                                UNDERHH = ""
                            End If
                            If UNDERMM = 0 Then
                                UNDERMM = ""
                            End If
                            sql_worker.INSERT_INTO_DTR_TABLE(staff_id, staffname, departmentname,
                                                             _day, P1TD, P2TD, P5TD, P6TD,
                                                            week, dtr_remarks, monthly_remarks, TotalUnderTime_daily, UNDERHH, UNDERMM,
                                                            TotalUnderTime_daily, TOTAL_OVERTIME, TOTAL_OT_DAILY_REMARKS,
                                                            GlobalVariables.OFFICIAL_ARRIVAL, GlobalVariables.OFFICIAL_DEPARTURE, Daily_UT, Daily_LATE, Daily_REMARKS,
                                                            Daily_LATE_TOTAL, Daily_UT_TOTAL, "", "", designation)
                        Else
                            TotalUnderTime_daily = cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_IN(TimeTableList(0)), sql_worker.GET_ON_DUTY_TIME(TimeTableList(0)), P1TD, True, False)
                            TotalUnderTime_daily = TotalUnderTime_daily + cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_OUT(TimeTableList(0)), sql_worker.GET_OFF_DUTY_TIME(TimeTableList(0)), P2TD, False, True)

                            TotalUnderTime_daily = TotalUnderTime_daily + cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_IN(TimeTableList(2)), sql_worker.GET_ON_DUTY_TIME(TimeTableList(2)), P5TD, True, False)
                            TotalUnderTime_daily = TotalUnderTime_daily + cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_OUT(TimeTableList(2)), sql_worker.GET_OFF_DUTY_TIME(TimeTableList(2)), P6TD, False, True)

                            P1TD = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day.AddDays(-1), _day, P1TD)
                            P2TD = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, P2TD)
                            P5TD = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, P5TD)
                            P6TD = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day.AddDays(1), P6TD)

                            Console.WriteLine("     OUTPUT = P1TD:" & P1TD & " P2TD:" & P2TD & " P5TD:" & P5TD & " P6TD:" & P6TD)
                            Console.WriteLine("     TOTAL UNDERTIME: " & TotalUnderTime_daily)
                            UNDERHH = cpu_worker.CONVERT_TO_HH(TotalUnderTime_daily)
                            UNDERMM = cpu_worker.CONVERT_TO_MM(TotalUnderTime_daily)
                            monthly_remarks = cpu_worker.CONVERT_TO_TIME(TotalUnderTime_daily)
                            If UNDERHH = 0 Then
                                UNDERHH = ""
                            End If
                            If UNDERMM = 0 Then
                                UNDERMM = ""
                            End If
                            sql_worker.INSERT_INTO_DTR_TABLE(staff_id, staffname, departmentname,
                                                             _day, P1TD, P2TD, P5TD, P6TD,
                                                            week, dtr_remarks, monthly_remarks, TotalUnderTime_daily, UNDERHH, UNDERMM,
                                                            TotalUnderTime_daily, TOTAL_OVERTIME, TOTAL_OT_DAILY_REMARKS,
                                                            GlobalVariables.OFFICIAL_ARRIVAL, GlobalVariables.OFFICIAL_DEPARTURE, Daily_UT, Daily_LATE, Daily_REMARKS,
                                                            Daily_LATE_TOTAL, Daily_UT_TOTAL, "", "", designation)
                        End If
                    Else
                        TotalUnderTime_daily = cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_IN(TimeTableList(0)), sql_worker.GET_ON_DUTY_TIME(TimeTableList(0)), P1TD, True, False)
                        TotalUnderTime_daily = TotalUnderTime_daily + cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_OUT(TimeTableList(0)), sql_worker.GET_OFF_DUTY_TIME(TimeTableList(0)), P2TD, False, True)
                        P1TD = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day.AddDays(-1), _day, P1TD)
                        P2TD = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, P2TD)

                        Console.WriteLine("     OUTPUT = P1TD:" & P1TD & " P2TD:" & P2TD)
                        Console.WriteLine("     TOTAL UNDERTIME: " & TotalUnderTime_daily)
                        UNDERHH = cpu_worker.CONVERT_TO_HH(TotalUnderTime_daily)
                        UNDERMM = cpu_worker.CONVERT_TO_MM(TotalUnderTime_daily)
                        monthly_remarks = cpu_worker.CONVERT_TO_TIME(TotalUnderTime_daily)
                        If UNDERHH = 0 Then
                            UNDERHH = ""
                        End If
                        If UNDERMM = 0 Then
                            UNDERMM = ""
                        End If
                        sql_worker.INSERT_INTO_DTR_TABLE(staff_id, staffname, departmentname,
                                 _day, P1TD, P2TD, "", "",
                                week, dtr_remarks, monthly_remarks, TotalUnderTime_daily, UNDERHH, UNDERMM,
                                TotalUnderTime_daily, TOTAL_OVERTIME, TOTAL_OT_DAILY_REMARKS,
                                GlobalVariables.OFFICIAL_ARRIVAL, GlobalVariables.OFFICIAL_DEPARTURE, Daily_UT, Daily_LATE, Daily_REMARKS,
                                Daily_LATE_TOTAL, Daily_UT_TOTAL, "", "", designation)
                    End If
                End If
            End If
            ''SECOND SHIFT ALGO
        ElseIf P3TD <> "" And P4TD <> "" Then
            If P5TD <> "" And P6TD <> "" Then
                P4TD = sql_worker.GET_OFF_DUTY_TIME(TimeTableList(1))
                P5TD = sql_worker.GET_ON_DUTY_TIME(TimeTableList(2))
                TotalUnderTime_daily = cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_IN(TimeTableList(1)), sql_worker.GET_ON_DUTY_TIME(TimeTableList(1)), P3TD, True, False)
                TotalUnderTime_daily = TotalUnderTime_daily + cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_OUT(TimeTableList(2)), sql_worker.GET_OFF_DUTY_TIME(TimeTableList(2)), P6TD, False, True)
                P4TD = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, P4TD)
                P5TD = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, P5TD)

                P3TD = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, P3TD)
                P6TD = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day.AddDays(1), P6TD)
                Console.WriteLine("     OUTPUT = P3TD:" & P3TD & " P4TD:" & P4TD & " P5TD:" & P5TD & " P6TD:" & P6TD)
                Console.WriteLine("     TOTAL UNDERTIME: " & TotalUnderTime_daily)
                UNDERHH = cpu_worker.CONVERT_TO_HH(TotalUnderTime_daily)
                UNDERMM = cpu_worker.CONVERT_TO_MM(TotalUnderTime_daily)
                monthly_remarks = cpu_worker.CONVERT_TO_TIME(TotalUnderTime_daily)
                If UNDERHH = 0 Then
                    UNDERHH = ""
                End If
                If UNDERMM = 0 Then
                    UNDERMM = ""
                End If
                sql_worker.INSERT_INTO_DTR_TABLE(staff_id, staffname, departmentname,
                                 _day, P3TD, P4TD, P5TD, P6TD,
                                week, dtr_remarks, monthly_remarks, TotalUnderTime_daily, UNDERHH, UNDERMM,
                                TotalUnderTime_daily, TOTAL_OVERTIME, TOTAL_OT_DAILY_REMARKS,
                                GlobalVariables.OFFICIAL_ARRIVAL, GlobalVariables.OFFICIAL_DEPARTURE, Daily_UT, Daily_LATE, Daily_REMARKS,
                                Daily_LATE_TOTAL, Daily_UT_TOTAL, "", "", designation)
            Else
                TotalUnderTime_daily = cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_IN(TimeTableList(1)), sql_worker.GET_ON_DUTY_TIME(TimeTableList(1)), P3TD, True, False)
                TotalUnderTime_daily = TotalUnderTime_daily + cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_OUT(TimeTableList(1)), sql_worker.GET_OFF_DUTY_TIME(TimeTableList(1)), P4TD, False, True)
                P3TD = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, P3TD)
                P4TD = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, P4TD)

                Console.WriteLine("     OUTPUT = P3TD:" & P3TD & " P4TD:" & P4TD)
                Console.WriteLine("     TOTAL UNDERTIME: " & TotalUnderTime_daily)
                UNDERHH = cpu_worker.CONVERT_TO_HH(TotalUnderTime_daily)
                UNDERMM = cpu_worker.CONVERT_TO_MM(TotalUnderTime_daily)
                monthly_remarks = cpu_worker.CONVERT_TO_TIME(TotalUnderTime_daily)
                If UNDERHH = 0 Then
                    UNDERHH = ""
                End If
                If UNDERMM = 0 Then
                    UNDERMM = ""
                End If
                sql_worker.INSERT_INTO_DTR_TABLE(staff_id, staffname, departmentname,
                                 _day, P3TD, P4TD, "", "",
                                week, dtr_remarks, monthly_remarks, TotalUnderTime_daily, UNDERHH, UNDERMM,
                                TotalUnderTime_daily, TOTAL_OVERTIME, TOTAL_OT_DAILY_REMARKS,
                                GlobalVariables.OFFICIAL_ARRIVAL, GlobalVariables.OFFICIAL_DEPARTURE, Daily_UT, Daily_LATE, Daily_REMARKS,
                                Daily_LATE_TOTAL, Daily_UT_TOTAL, "", "", designation)
            End If
            ''THIRD SHIFT ALGO
        ElseIf P5TD <> "" And P6TD <> "" Then
            If P1TW <> "" And P2TW <> "" Then
                P6TD = sql_worker.GET_OFF_DUTY_TIME(TimeTableList(2))
                TotalUnderTime_daily = cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_IN(TimeTableList(2)), sql_worker.GET_ON_DUTY_TIME(TimeTableList(2)), P5TD, True, False)
                TotalUnderTime_daily = TotalUnderTime_daily + cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_OUT(TimeTableList(2)), sql_worker.GET_OFF_DUTY_TIME(TimeTableList(2)), P6TD, False, True)
                P5TD = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, P5TD)
                P6TD = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day.AddDays(1), P6TD)



                Console.WriteLine("     OUTPUT = P5TD:" & P5TD & " P6TD:" & P6TD)
                Console.WriteLine("     TOTAL UNDERTIME: " & TotalUnderTime_daily)
                UNDERHH = cpu_worker.CONVERT_TO_HH(TotalUnderTime_daily)
                UNDERMM = cpu_worker.CONVERT_TO_MM(TotalUnderTime_daily)
                monthly_remarks = cpu_worker.CONVERT_TO_TIME(TotalUnderTime_daily)
                If UNDERHH = 0 Then
                    UNDERHH = ""
                End If
                If UNDERMM = 0 Then
                    UNDERMM = ""
                End If
                sql_worker.INSERT_INTO_DTR_TABLE(staff_id, staffname, departmentname,
                                 _day, "", "", P5TD, P6TD,
                                week, dtr_remarks, monthly_remarks, TotalUnderTime_daily, UNDERHH, UNDERMM,
                                TotalUnderTime_daily, TOTAL_OVERTIME, TOTAL_OT_DAILY_REMARKS,
                               GlobalVariables.OFFICIAL_ARRIVAL, GlobalVariables.OFFICIAL_DEPARTURE, Daily_UT, Daily_LATE, Daily_REMARKS,
                               Daily_LATE_TOTAL, Daily_UT_TOTAL, "", "", designation)
            Else
                TotalUnderTime_daily = cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_IN(TimeTableList(2)), sql_worker.GET_ON_DUTY_TIME(TimeTableList(2)), P5TD, True, False)
                TotalUnderTime_daily = TotalUnderTime_daily + cpu_worker.GET_UNDERTIME(sql_worker.GET_BEGINING_OUT(TimeTableList(2)), sql_worker.GET_OFF_DUTY_TIME(TimeTableList(2)), P6TD, False, True)

                P5TD = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day, P5TD)
                P6TD = sql_worker.CHECK_RAWLOGSTIME_IF_MODIFIED_BY_USER(staff_id, _day, _day.AddDays(1), P6TD)

                Console.WriteLine("P5TD:" & P5TD & " P6TD:" & P6TD)
                Console.WriteLine("     TOTAL UNDERTIME: " & TotalUnderTime_daily)
                UNDERHH = cpu_worker.CONVERT_TO_HH(TotalUnderTime_daily)
                UNDERMM = cpu_worker.CONVERT_TO_MM(TotalUnderTime_daily)
                monthly_remarks = cpu_worker.CONVERT_TO_TIME(TotalUnderTime_daily)
                If UNDERHH = 0 Then
                    UNDERHH = ""
                End If
                If UNDERMM = 0 Then
                    UNDERMM = ""
                End If
                sql_worker.INSERT_INTO_DTR_TABLE(staff_id, staffname, departmentname,
                                 _day, "", "", P5TD, P6TD,
                                week, dtr_remarks, monthly_remarks, TotalUnderTime_daily, UNDERHH, UNDERMM,
                                TotalUnderTime_daily, TOTAL_OVERTIME, TOTAL_OT_DAILY_REMARKS,
                                GlobalVariables.OFFICIAL_ARRIVAL, GlobalVariables.OFFICIAL_DEPARTURE, Daily_UT, Daily_LATE, Daily_REMARKS,
                                Daily_LATE_TOTAL, Daily_UT_TOTAL, "", "", designation)
            End If
        Else
            ''TAG AS ABSENT
            TotalUnderTime_daily = sql_worker.GET_TOTAL_WORKING_HOURS(TimeTableList(0)) * 60
            UNDERHH = My.Settings.dtr_remarks_for_absent
            monthly_remarks = My.Settings.sra_remarks_for_absent
            Console.WriteLine("Absent")
            sql_worker.INSERT_INTO_DTR_TABLE(staff_id, staffname, departmentname,
                                 _day, P1TD, P2TD, P5TD, P6TD,
                                week, dtr_remarks, monthly_remarks, TotalUnderTime_daily, UNDERHH, UNDERMM,
                                TotalUnderTime_daily, TOTAL_OVERTIME, TOTAL_OT_DAILY_REMARKS,
                               GlobalVariables.OFFICIAL_ARRIVAL, GlobalVariables.OFFICIAL_DEPARTURE, Daily_UT, Daily_LATE, Daily_REMARKS,
                               Daily_LATE_TOTAL, Daily_UT_TOTAL, "", "", designation)
        End If


        Console.WriteLine("DONE CALCULATION WORKDATE: " & _day)
        Console.WriteLine("")
    End Sub

    'Private Function Px1() As Object
    '    Throw New NotImplementedException
    'End Function



End Class







