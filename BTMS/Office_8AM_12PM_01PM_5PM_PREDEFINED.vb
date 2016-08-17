Imports System.IO
Imports System.Data
Imports System.ComponentModel
Imports System.Drawing.Imaging
Imports System.Data.SqlServerCe
Imports System.Data.SqlClient


Module Office_8AM_12PM_01PM_5PM_PREDEFINED


    Dim Office_8AM_12PM_01PM_5PM_PREDEFINEDWorker As New BackgroundWorker
    Dim staffID As String = String.Empty
    Dim attdate As Date

   

    ''Policy 8AM-5PM PLANNED TIME
    Dim Planned_AM_IN As String = "08:00"
    Dim Planned_AM_OUT As String = "12:00"
    Dim Planned_PM_IN As String = "13:00"
    Dim Planned_PM_OUT As String = "17:00"

    ''Policy Ranges AM/PM
    Dim am_start_clk_in As String = "00:00:00"
    Dim am_ending_clk_in As String = "09:59:00"

    Dim am_start_clk_out As String = "10:00:00"
    Dim am_ending_clk_out As String = "12:59:00"

    Dim pm_start_clk_in As String = "12:00:00"
    Dim pm_ending_clk_in As String = "14:59:00"

    Dim pm_start_clk_out As String = "15:00:00"
    Dim pm_ending_clk_out As String = "23:59:00"


    Dim starting_lunch As String = "10:00:00"
    Dim ending_lunch As String = "14:59:00"
    Dim starting_am_offset As String = "10:00:00"
    Dim ending_am_offset As String = "12:29:00"
    Dim starting_pm_offset As String = "12:30:00"
    Dim ending_pm_offset As String = "14:59:00"


    ''Total Calculated Result
    Dim Total_AM_Late_IN As Integer = 0     ''Undertime
    Dim Total_AM_Early_IN As Integer = 0    ''OT
    Dim Total_AM_Early_OUT As Integer = 0   ''Undertime
    Dim Total_PM_Early_OUT As Integer = 0   ''Undertime
    Dim Total_PM_Late_OUT As Integer = 0    ''OT
    Dim Total_underTime As Integer = 0      ''Undertime total
    Dim Total_PM_Late_IN As Integer = 0

    ''Raw logslist
    ' Dim raw_logslist As New List(Of String)
    Dim lunch_break_logs As New List(Of String)

    ''ACTUAL IN/OUT FOR BIR am_CIN+am_COUT+pm_CIN+pm_COUT
    Dim am_CIN As String = String.Empty
    Dim am_COUT As String = String.Empty
    Dim pm_CIN As String = String.Empty
    Dim pm_COUT As String = String.Empty

    Public Sub Main(raw_logs_list As List(Of String))
        'Console.WriteLine("Calling Module Office_8AM_12PM_01PM_5PM_PREDEFINED")
        'Console.WriteLine("STAFFID: [" & staff_ID & "] DATE: " & Format(att_date, "yyyy-MM-dd"))
        'Console.WriteLine("RAW LOGS: " & String.Join("+", logs_list))


        'Office_8AM_12PM_01PM_5PM_PREDEFINEDWorker.WorkerReportsProgress = True
        'Office_8AM_12PM_01PM_5PM_PREDEFINEDWorker.WorkerSupportsCancellation = True
        'AddHandler Office_8AM_12PM_01PM_5PM_PREDEFINEDWorker.DoWork, AddressOf Office_8AM_12PM_01PM_5PM_PREDEFINEDWorkerDoWork
        'AddHandler Office_8AM_12PM_01PM_5PM_PREDEFINEDWorker.ProgressChanged, AddressOf Office_8AM_12PM_01PM_5PM_PREDEFINEDWorkerProgressChanged
        'AddHandler Office_8AM_12PM_01PM_5PM_PREDEFINEDWorker.RunWorkerCompleted, AddressOf Office_8AM_12PM_01PM_5PM_PREDEFINEDWorkerCompleted

        'If (staff_ID <> String.Empty) And (att_date <> Nothing) And (logs_list.Count <> 0) Then
        '    'staffID = staff_ID
        '    'attdate = att_date
        '    'raw_logslist = logs_list
        '    Office_8AM_12PM_01PM_5PM_PREDEFINEDWorker.RunWorkerAsync()
        'End If
    End Sub

#Region "Office_8AM_12PM_01PM_5PM_PREDEFINED TASK"

    Public Function GET_ACTUAL_IN_OUT(raw_logs As List(Of String)) As List(Of String)
        Dim task As New CPU
        Dim result As New List(Of String)
        ''=======================================
        '''GET ALL ACTUAL CLOCK IN / OUT 
        ''=======================================



        am_CIN = task.GET_ACTUALTIME(raw_logs, am_start_clk_in, am_ending_clk_in, True)
        pm_COUT = task.GET_ACTUALTIME(raw_logs, pm_start_clk_out, pm_ending_clk_out, True)

        lunch_break_logs = task.GET_LUNCHBREAKLOGS(raw_logs, starting_lunch, ending_lunch)

        If Not lunch_break_logs.Count = 0 Then
            If lunch_break_logs.Count = 1 Then
                If am_CIN <> "NTR" And pm_COUT = "NTR" Then      ''110
                    am_COUT = task.GET_ACTUALTIME(lunch_break_logs, starting_am_offset, ending_pm_offset, True)
                    'pm_CIN = task.GET_ACTUALTIME(lunch_break_logs, pm_start_clk_in, pm_ending_clk_in, True)
                    'pm_CIN = task.GET_LUNCHBREAKOFFSET(lunch_break_logs, starting_pm_offset, ending_pm_offset)
                    ''get diff + 4HOURS UNDERTIME

                ElseIf am_CIN = "NTR" And pm_COUT <> "NTR" Then  ''011
                    pm_CIN = task.GET_ACTUALTIME(lunch_break_logs, starting_am_offset, ending_pm_offset, True)

                ElseIf am_CIN <> "NTR" And pm_COUT <> "NTR" Then  '111
                    am_COUT = task.GET_LUNCHBREAKOFFSET(lunch_break_logs, starting_am_offset, ending_am_offset)
                    pm_CIN = task.GET_LUNCHBREAKOFFSET(lunch_break_logs, starting_pm_offset, ending_pm_offset)

                End If
            ElseIf lunch_break_logs.Count Then        '(Office 8AM-5PM) + Lunch Break
                '(Office 7AM-5PM) + Lunch Break
                '(Office 8AM-4PM)  - NO BREAK 
                '(Office 4PM-12Midnight) - 2 day Shift
                '(Office 12Midnight-8AM) - 2 day Shiftt > 1 Then
                lunch_break_logs.Sort()
                am_COUT = lunch_break_logs(0)       ''get earliest out
                pm_CIN = lunch_break_logs(lunch_break_logs.Count - 1)   ''get earliest in
            End If
        Else
            am_COUT = "NTR"
            pm_CIN = "NTR"
        End If

        Console.WriteLine("=================================================================================================")
        Console.WriteLine("RAW LOGS: " & String.Join(" + ", raw_logs))
        Console.WriteLine("AMCIN:[" & am_CIN & "] AMCOUT:[" & am_COUT & "] PMCIN:[" & pm_CIN & "] PMCOUT:[" & pm_COUT & "]")
        Console.WriteLine("=================================================================================================")



        If am_CIN = "NTR" Then
            am_CIN = ""
        End If
        If am_COUT = "NTR" Then
            am_COUT = ""
        End If
        If pm_CIN = "NTR" Then
            pm_CIN = ""
        End If
        If pm_COUT = "NTR" Then
            pm_COUT = ""
        End If


        result.Add(am_CIN)
        result.Add(am_COUT)
        result.Add(pm_CIN)
        result.Add(pm_COUT)

        Return result
    End Function
#End Region



End Module
