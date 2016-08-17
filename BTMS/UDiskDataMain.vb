
Imports System.IO
Imports System.Data
Imports System.ComponentModel
Imports System.Drawing.Imaging
Imports System.Data.SqlServerCe
Imports System.Data.SqlClient
Imports System.Threading
Imports System.Threading.Tasks


Public Class UDiskDataMain
    Dim ce_cnn As SqlCeConnection = New SqlCeConnection(My.Settings.LocalConnectionString)
    Dim ce_cmd As SqlCeCommand
    Dim ce_da As SqlCeDataAdapter
    Dim ce_ds As DataSet
    Dim ce_query As String = String.Empty
    Dim Origin As String = "USB"
    Dim isAdjusted As Boolean = 0


#Region "Declared Sub Functions"

    Public Function IDENTIFY_TRANSACTIONTYPE(ByVal val As Integer) As String
        Dim result As String = String.Empty
        Select Case val
            Case 0
                result = "IN"
            Case 1
                result = "OUT"
            Case 2
                result = "BRKOUT"
            Case 3
                result = "BRKIN"
            Case 4
                result = "OTIN"
            Case 5
                result = "OTOUT"
        End Select

        Return result
    End Function

    Public Function GET_LAST_RAW_LOG_ID() As Long
        Dim result As Long = 0

        ce_cmd = New SqlCeCommand("SELECT [RAWID] FROM TIMELOGS ORDER BY [RAWID] DESC", ce_cnn)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
        If sdr.Read = True Then
            result = sdr(0)
        End If
        sdr.Close()
        Return result
    End Function
    'Public Function VERIFY_USB_LOG(staffid As String, rw_data As String) As Boolean
    '    Dim result As Boolean = False
    '    ce_query = "SELECT * FROM TIMELOGS WHERE (STAFFID = '" & staffid & "') AND ([RAW] = '" & rw_data & "')"
    '    ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
    '    If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
    '    Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
    '    While sdr.Read = True
    '        result = True
    '    End While
    '    sdr.Close()
    '    Return result
    'End Function

    Public Function VERIFY_USB_LOG(staffid As String, rw_data As String) As Boolean
        Dim result As Boolean = False
        ce_query = "SELECT * FROM TIMELOGS WHERE (STAFFID = '" & staffid & "') AND ([RAW] = '" & rw_data & "')"
        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
        If sdr.Read = True Then
            result = True
        End If
        '  sdr.Close()
        Return result
    End Function


    Public Sub INSERT_NEW_LOG(staff_id As String, dt As Date, tme As String, trans_type As String, orgn As String, isAdjtd As Boolean, raw As String, addedby As String)
        ce_query = "INSERT INTO TIMELOGS([STAFFID], [DATE], [LOGTIME], [TRANSACTIONTYPE],[ORIGIN],[ISADJUSTED],[RAW],[DATEADDED],[ADDEDBY]) " & _
     "VALUES(@InUserID, @InAttDate, @InAttTime, @InCheckType, @Inorigin,@InisAdjt,@iraw,@dadded,@iaddedby)"

        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
        With ce_cmd
            '.Parameters.Add(New SqlCeParameter("@InLogsID", logs_id))
            .Parameters.Add(New SqlCeParameter("@InUserID", staff_id))
            .Parameters.Add(New SqlCeParameter("@InAttDate", dt.ToShortDateString))
            .Parameters.Add(New SqlCeParameter("@InAttTime", tme))
            .Parameters.Add(New SqlCeParameter("@InChecktype", trans_type))
            .Parameters.Add(New SqlCeParameter("@Inorigin", orgn))
            .Parameters.Add(New SqlCeParameter("@InisAdjt", isAdjtd))
            .Parameters.Add(New SqlCeParameter("@iraw", raw))
            .Parameters.Add(New SqlCeParameter("@dadded", DateAndTime.Now))
            .Parameters.Add(New SqlCeParameter("@iaddedby", addedby))
            .ExecuteNonQuery()
        End With
    End Sub



    Public Sub MAIN(filename As String, currentuser As String)
        '    Dim result As New List(Of String)
        '    Dim udisk As New Structs.UDisk
        '    Dim byDataBuf() As Byte = Nothing
        '    Dim iLength As Integer 'length of the bytes to get from the data

        '    Dim sPIN2 As String = ""
        '    Dim sVerified As String = ""
        '    Dim sTime_second As String = ""
        '    Dim sDeviceID As String = ""
        '    Dim sStatus As String = ""
        '    Dim sWorkcode As String = ""
        '    Dim dt As Date = Nothing


        '    Dim stream As FileStream
        '    stream = New FileStream(filename, FileMode.OpenOrCreate, FileAccess.Read)
        '    byDataBuf = File.ReadAllBytes(filename)
        '    iLength = Convert.ToInt32(stream.Length)

        '    Dim lvItem As New ListViewItem
        '    Dim iStartIndex As Integer = 0
        '    Dim iOneLogLength As Integer 'the length of one line of attendence log
        '    For i As Integer = iStartIndex To iLength - 2
        '        On Error Resume Next
        '        If byDataBuf(i) = 13 And byDataBuf(i + 1) = 10 Then

        '            iOneLogLength = (i + 1) + 1 - iStartIndex
        '            Dim bySSRAttLog(iOneLogLength - 1) As Byte
        '            Array.Copy(byDataBuf, iStartIndex, bySSRAttLog, 0, iOneLogLength)

        '            udisk.GetAttLogFromDat(bySSRAttLog, iOneLogLength, sPIN2, sTime_second, sDeviceID, sStatus, sVerified, sWorkcode)
        '            dt = sTime_second
        '            If VERIFY_USB_LOG(sPIN2.Trim, sTime_second.Trim) = False Then


        '                'Form3.UpdateText(Form3.lblx_usbprogress,
        '                '                 sPIN2 & sTime_second & sDeviceID & sStatus & sVerified & sWorkcode)
        '                INSERT_NEW_LOG(GET_LAST_RAW_LOG_ID() + 1, sPIN2.Trim, dt, Format(dt, "HH:mm:ss").Trim, IDENTIFY_TRANSACTIONTYPE(sStatus), "USB", False, sTime_second.Trim, currentuser)
        '            End If

        '            bySSRAttLog = Nothing
        '            iStartIndex += iOneLogLength
        '            iOneLogLength = 0
        '        End If
        '    Next
        '    stream.Close()
    End Sub

#End Region
End Class
