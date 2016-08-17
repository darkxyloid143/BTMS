
Public Class CPU

#Region "Public Declared Function"

    Public Function GET_ACTUALTIME(raw_logs As List(Of String), starting_value As String, ending_value As String, get_earliest As Boolean) As String
        Dim result As String = "NTR"

        Dim ACTUAL_RangeList As New List(Of String)
        'ACTUAL_RangeList = IDENTIFYHOURS(starting_value, ending_value) ''RETURN HH values
        ACTUAL_RangeList = Extractminutes(starting_value, ending_value)

        Dim listOfPunches As New List(Of String)
        'raw_logs.Sort()
        'For Each ix As Date In raw_logs
        '    If ACTUAL_RangeList.Contains(Format(ix, "HH")) Then
        '        listOfPunches.Add(ix)
        '    End If
        'Next

        For Each ix As String In raw_logs
            If ACTUAL_RangeList.Contains(Format(CDate(ix), "HH:mm")) Then
                listOfPunches.Add(ix)
            End If
        Next

        listOfPunches.Sort()
        If Not listOfPunches.Count = 0 Then
            If get_earliest = True Then
                result = listOfPunches(0)
            Else
                result = listOfPunches(listOfPunches.Count - 1)
            End If
        End If
        Return result
    End Function

    Public Function GET_LOGS_CANDIDATES(ByVal raw_logs As List(Of String), ByVal starting_value As String, ByVal ending_value As String) As List(Of String)
        Dim result As New List(Of String)

        Dim ACTUAL_RangeList As New List(Of String)
        ACTUAL_RangeList = Extractminutes(starting_value, ending_value)

        'Console.WriteLine(String.Join(" <> ", ACTUAL_RangeList.ToArray))
        'Console.WriteLine(String.Join(" <> ", raw_logs.ToArray))

        For Each ix As String In raw_logs
            If ACTUAL_RangeList.Contains(Format(CDate(ix), "HH:mm")) Then
                result.Add(ix)
            End If
        Next
        Return result
    End Function




    Public Function GET_ACTUALTIME_TWODAYSHIFT(ByVal raw_logs As List(Of String), ByVal starting_value As String, ByVal ending_value As String, ByVal get_earliest As Boolean) As String
        Dim result As String = "NTR"

        Dim ACTUAL_RangeList As New List(Of String)
        ACTUAL_RangeList = ExtractHours(starting_value, ending_value)

        Dim listOfPunches As New List(Of String)
        'raw_logs.Sort()
        For Each ix As String In raw_logs
            If ACTUAL_RangeList.Contains(ix) Then
                listOfPunches.Add(ix)
            End If
        Next



        'listOfPunches.Sort()
        If Not listOfPunches.Count = 0 Then
            If get_earliest = True Then
                result = listOfPunches(0)
            Else
                result = listOfPunches(listOfPunches.Count - 1)
            End If
        End If
        Return result
    End Function

    Public Function GET_LUNCHBREAKLOGS(ByVal raw_logs As List(Of String), ByVal starting_value As String, ByVal ending_value As String) As List(Of String)
        Dim result As New List(Of String)
        Dim ACTUAL_RangeList As New List(Of String)
        ACTUAL_RangeList = ExtractHours(starting_value, ending_value)
        raw_logs.Sort()
        For Each ix As String In raw_logs
            If ACTUAL_RangeList.Contains(ix) Then
                result.Add(ix)
            End If
        Next
        Return result
    End Function

    Public Function GET_LUNCHBREAKOFFSET(ByVal raw_log As List(Of String), ByVal starting_value As String, ByVal ending_value As String) As String
        Dim result As String = "NTR"
        Dim ACTUAL_RangeList As New List(Of String)
        Dim actual_log As String = raw_log(0)
        ACTUAL_RangeList = ExtractHours(starting_value, ending_value)
        If ACTUAL_RangeList.Contains(actual_log) Then
            Return actual_log
        End If
        Return result
    End Function
#End Region


    Public Function EXTRACTDATE(ByVal start_date As Date, ByVal end_date As Date) As List(Of Date)
        Dim result As New List(Of Date)
        Dim CurrentDate As Date = start_date.ToShortDateString
        While CurrentDate <= end_date
            result.Add(CurrentDate.ToShortDateString)
            CurrentDate = CurrentDate.AddDays(1)
        End While
        Return result
    End Function


    Public Function ExtractHours(ByVal strtHr As Date, ByVal endHr As Date) As List(Of String)
        Dim res As New List(Of String)
        Dim strt_hr As String = Format(strtHr, "HH:mm:ss")
        Dim end_hr As String = Format(endHr, "HH:mm:ss")
        While strt_hr <> end_hr
            strt_hr = Format(strtHr, "HH:mm:ss")
            res.Add(strt_hr)
            strtHr = strtHr.AddSeconds(1)
        End While
        Return res
    End Function
    Public Function Extractminutes(ByVal strtHr As Date, ByVal endHr As Date) As List(Of String)
        Dim res As New List(Of String)
        Dim strt_hr As String = Format(strtHr, "HH:mm")
        Dim end_hr As String = Format(endHr, "HH:mm")
        While strt_hr <> end_hr
            strt_hr = Format(strtHr, "HH:mm")
            res.Add(strt_hr)
            strtHr = strtHr.AddMinutes(1)
        End While
        Return res
    End Function

    Public Function IDENTIFYHOURS(ByVal strtHr As Date, ByVal endHr As Date) As List(Of String)
        Dim res As New List(Of String)
        Dim strt_hr As String = Format(strtHr, "HH")
        Dim end_hr As String = Format(endHr, "HH")
        While strt_hr <> end_hr
            strt_hr = Format(strtHr, "HH")
            res.Add(strt_hr)
            strtHr = strtHr.AddHours(1)
        End While
        Return res
    End Function
    Public Function CALCULATE_UNDERTIME_IN(user_log As Date, on_duty_time As Date) As Double
        Dim result As Double = 0
        user_log = Format(user_log, "HH:mm")    ''THIS WILL REMOVE THE SECONDS
        Dim TS As TimeSpan = user_log - on_duty_time
        Console.WriteLine("USERLOG: " & user_log & " VS " & on_duty_time)
        result = TS.TotalMinutes
        If result < 0 Then
            result = 0
        End If


        Return result
    End Function

    Public Function CALCULATE_UNDERTIME_OUT(user_log As Date, on_duty_time As Date) As Double
        Dim result As Double = 0
        user_log = Format(user_log, "HH:mm") ''THIS WILL REMOVE THE SECONDS
        Dim TS As TimeSpan = on_duty_time - user_log
        Console.WriteLine("USERLOG: " & user_log & " VS " & on_duty_time)
        result = TS.TotalMinutes
        If result < 0 Then
            result = 0
        End If
        Return result
    End Function

    Public Function CONVERT_TO_HH(total As Double) As Integer
        Dim result As Integer = 0

        If Not total = 0 Then
            Console.WriteLine("in: " & total)
            Dim ts As TimeSpan = TimeSpan.FromMinutes(total)
            Dim mydate As DateTime = New DateTime(ts.Ticks)
            If Not mydate.ToString(("HH")) = "00" Then
                result = CInt(mydate.ToString(("hh")))
                Console.WriteLine("RET: " & result)
            End If
        End If
        Return result
    End Function


    Public Function CONVERT_TO_MM(total As Double) As Integer
        Dim result As Integer = 0
        If Not total = 0 Then
            Console.WriteLine("in: " & total)
            Dim ts As TimeSpan = TimeSpan.FromMinutes(total)
            Dim mydate As DateTime = New DateTime(ts.Ticks)
            If Not mydate.ToString(("mm")) = "00" Then
                result = CInt(mydate.ToString(("mm")))
                Console.WriteLine("RET: " & result)
            End If
        End If
        Return result
    End Function

    Public Function CONVERT_TO_TIME(total As Double) As String
        Dim result As String = "00:00"
        Dim hh As String = ""
        Dim mm As String = ""
        If Not total = 0 Then
            Dim ts As TimeSpan = TimeSpan.FromMinutes(total)
            Dim mydate As DateTime = New DateTime(ts.Ticks)
            If Not mydate.ToString(("HH")) = "00" Then
                hh = CInt(mydate.ToString(("HH"))) & "h"
            Else
                hh = ""
            End If
            If Not mydate.ToString(("mm")) = "00" Then
                mm = CInt(mydate.ToString(("mm"))) & "m"
            Else
                mm = ""
            End If

        End If

        result = hh & " " & mm
        Return result
    End Function


    Public Function GET_UNDERTIME(starting_point As Date, ondutytime As Date, punch As Date, ONDUTY As Boolean, OFFDUTY As Boolean) As Integer
        Dim result As Integer = 0
        Dim s_to_o As List(Of String)
        s_to_o = Extractminutes(starting_point, ondutytime)

        Dim s_to_p As List(Of String)
        s_to_p = Extractminutes(starting_point, punch)

        If ONDUTY = True And OFFDUTY = False Then
            result = s_to_o.Count - s_to_p.Count
        ElseIf ONDUTY = False And OFFDUTY = True Then
            result = s_to_p.Count - s_to_o.Count
        End If

        If result < 0 Then
            result = CInt(result.ToString.Replace("-", "").Trim)
        Else
            result = 0
        End If

        Return result
    End Function


    Public Function GET_INTERVAL_AFTER_COUT(starting_point As Date, offdutytime As Date, punch As Date) As Integer
        Dim result As Integer = 0
        Dim s_to_o As List(Of String)
        s_to_o = Extractminutes(starting_point, offdutytime)

        Dim s_to_p As List(Of String)
        s_to_p = Extractminutes(starting_point, punch)

        'If ONDUTY = True And OFFDUTY = False Then
        '    result = s_to_o.Count - s_to_p.Count

        'ONDUTY = False
        result = s_to_p.Count - s_to_o.Count


        If result < 0 Then
            ''THIS IS  A NEGATIVE VALUE
            result = 0
        End If

        Return result
    End Function

    Public Function CONVERT_TO_ETA(input_time As String)
        Dim _time As String = input_time
        If Not _time = 0 Then
            Dim Hours As Integer = Math.Floor(_time / 60)
            Dim Minutes As Integer = _time Mod 60

            ''FORMAT THE VALUE FOR DTR TOTAL UNDERTIME OF EMPLOYEE MONTHLY
            If Not Hours = 0 Then
                If Not Minutes = 0 Then
                    _time = Hours & "h" & Minutes & "m"
                Else
                    _time = Hours & "h"
                End If
            Else
                _time = Minutes & "m"
            End If
        Else
            _time = ""
        End If
        Return _time
    End Function



End Class
