Imports System
Imports System.IO
Module LOGDUMP

    Public Sub WriteLogMessage(msg_value As String)
        Using w As StreamWriter = File.AppendText("log.txt")
            Log(msg_value, w)
        End Using

        'Using r As StreamReader = File.OpenText("log.txt")
        '    DumpLog(r)
        'End Using
    End Sub
    Public Sub WRITE_INVALID(msg_value As String)
        Using w As StreamWriter = File.AppendText("invalid.txt")
            Dim wrt As TextWriter = w

            wrt.WriteLine(msg_value)

        End Using
    End Sub

    Public Sub WRITE_RAW_LOGS(msg_value As String)
        Using w As StreamWriter = File.AppendText("RAW_SWIPE_RECORD.txt")
            Dim wrt As TextWriter = w

            wrt.WriteLine(msg_value)

        End Using
    End Sub

    Public Sub Log(logMessage As String, w As TextWriter)
        w.WriteLine(logMessage)
    End Sub

    Public Sub DumpLog(r As StreamReader)
        Dim line As String
        line = r.ReadLine()
        While Not (line Is Nothing)
            Console.WriteLine(line)
            line = r.ReadLine()
        End While
    End Sub
End Module
