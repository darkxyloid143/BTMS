Public Class BIR_8AM_PM
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

    Public Function Get_Actual_INOUT(ByVal raw_logs As List(Of String)) As List(Of String)       ''Return values AMIN + AMOUT + PMIN + PMOUT
        Dim result As New List(Of String)
        ''ACTUAL IN/OUT FOR BIR am_CIN+am_COUT+pm_CIN+pm_COUT
        Dim am_CIN As String = String.Empty
        Dim am_COUT As String = String.Empty
        Dim pm_CIN As String = String.Empty
        Dim pm_COUT As String = String.Empty
        Dim user_raw_logs As New List(Of String)
        Dim lunch_break_logs As New List(Of String)
        Dim task As New CPU


        am_CIN = task.GET_ACTUALTIME(user_raw_logs, am_start_clk_in, am_ending_clk_in, True)
        pm_COUT = task.GET_ACTUALTIME(user_raw_logs, pm_start_clk_out, pm_ending_clk_out, True)


        lunch_break_logs = task.GET_LUNCHBREAKLOGS(raw_logs, starting_lunch, ending_lunch)

        If Not lunch_break_logs.Count = 0 Then
            If lunch_break_logs.Count = 1 Then
                If am_CIN <> "NULL" And pm_COUT = "NULL" Then      ''100
                    am_COUT = task.GET_ACTUALTIME(lunch_break_logs, starting_am_offset, ending_pm_offset, True)
                    GoTo finish
                ElseIf am_CIN = "NULL" And pm_COUT <> "NULL" Then  ''011
                    pm_CIN = task.GET_ACTUALTIME(lunch_break_logs, starting_am_offset, ending_pm_offset, True)
                    GoTo finish

                ElseIf am_CIN <> "NULL" And pm_COUT <> "NULL" Then  '111
                    am_COUT = task.GET_LUNCHBREAKOFFSET(lunch_break_logs, starting_am_offset, ending_am_offset)
                    pm_CIN = task.GET_LUNCHBREAKOFFSET(lunch_break_logs, starting_pm_offset, ending_pm_offset)
                    'ElseIf am_CIN <> "NULL" And am_COUT = "NULL" And pm_CIN = "NULL" And pm_COUT <> "NULL" Then ''1001

                End If
            ElseIf lunch_break_logs.Count > 1 And am_CIN <> "NULL" And pm_COUT = "NULL" Then  '1110
                lunch_break_logs.Sort()
                am_COUT = lunch_break_logs(0)       ''get earliest out
                pm_CIN = lunch_break_logs(lunch_break_logs.Count - 1)   ''get earliest in
                GoTo finish
            ElseIf lunch_break_logs.Count > 1 And am_CIN = "NULL" And pm_COUT <> "NULL" Then    '0111
                lunch_break_logs.Sort()
                am_COUT = lunch_break_logs(0)       ''get earliest out
                pm_CIN = lunch_break_logs(lunch_break_logs.Count - 1)   ''get earliest in
                GoTo finish
            ElseIf lunch_break_logs.Count > 1 And am_CIN = "NULL" And pm_COUT = "NULL" Then
                lunch_break_logs.Sort()
                am_COUT = lunch_break_logs(0)       ''get earliest out
                pm_CIN = lunch_break_logs(lunch_break_logs.Count - 1)   ''get earliest in

            End If
        Else
            am_COUT = "NULL"
            pm_CIN = "NULL"
        End If



finish:
        result.Add(am_CIN)
        result.Add(am_COUT)
        result.Add(pm_CIN)
        result.Add(pm_COUT)


        'Console.WriteLine("=================================================================================================")
        'Console.WriteLine("RAW LOGS: " & String.Join(" + ", user_raw_logs))
        'Console.WriteLine("AMCIN:[" & am_CIN & "] AMCOUT:[" & am_COUT & "] PMCIN:[" & pm_CIN & "] PMCOUT:[" & pm_COUT & "]")
        'Console.WriteLine("=================================================================================================")

        Return result
    End Function
End Class
