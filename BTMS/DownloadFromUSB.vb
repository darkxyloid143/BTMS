
'Imports System.IO
'Imports System.Data
'Imports System.ComponentModel
'Imports System.Drawing.Imaging
'Imports System.Data.SqlServerCe
'Imports System.Data.SqlClient


'Module DownloadFromUSB
'    Dim ce_cnn As SqlCeConnection = New SqlCeConnection(My.Settings.LocalConnectionString)
'    Dim ce_cmd As SqlCeCommand
'    Dim ce_da As SqlCeDataAdapter
'    Dim ce_ds As DataSet
'    Dim ce_query As String = String.Empty
'    Dim Origin As String = "USB"
'    Dim isAdjusted As Boolean = 0
'    'Dim RawLogsReaderWorker As New BackgroundWorker
'    'Public Sub main(ByVal filepath As String)
'    '    connStrLoc = New SqlCeConnection(My.Settings.LocalConnectionString)
'    '    RawLogsReaderWorker.WorkerReportsProgress = True
'    '    RawLogsReaderWorker.WorkerSupportsCancellation = True
'    '    AddHandler RawLogsReaderWorker.DoWork, AddressOf RawLogsReaderWorkerDoWork
'    '    AddHandler RawLogsReaderWorker.ProgressChanged, AddressOf RawLogsReaderWorkerProgressChanged
'    '    AddHandler RawLogsReaderWorker.RunWorkerCompleted, AddressOf RawLogsReaderWorkerCompleted
'    '    If File.Exists(filepath) Then
'    '        RawLogsReaderWorker.RunWorkerAsync(filepath)
'    '    End If
'    'End Sub



'    '#Region "RawLogsReaderWorker Task"

'    '    'Private Sub RawLogsReaderWorkerDoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs)
'    '    '    Using SPreader As Microsoft.VisualBasic.FileIO.TextFieldParser = New Microsoft.VisualBasic.FileIO.TextFieldParser(e.Argument.ToString)
'    '    '        SPreader.TextFieldType = FileIO.FieldType.Delimited
'    '    '        SPreader.SetDelimiters("	")
'    '    '        Dim staffID As String

'    '    '        Dim TransType As String
'    '    '        Dim VerifMode As String
'    '    '        Dim LogsID As Long = 0
'    '    '        Dim ctr As Integer = 0
'    '    '        Dim lnArr As String()

'    '    '        While Not SPreader.EndOfData
'    '    '            ctr += 1
'    '    '            RawLogsReaderWorker.ReportProgress(ctr)
'    '    '            lnArr = SPreader.ReadFields()
'    '    '            If Not lnArr.Length = 6 Then
'    '    '                Exit While
'    '    '            End If
'    '    '            ''Get current logs count via logsID value
'    '    '            LogsID = GetLastRawLogsID() + 1
'    '    '            ''Assign values
'    '    '            staffID = lnArr.GetValue(0)
'    '    '            TransType = lnArr.GetValue(3)
'    '    '            VerifMode = lnArr.GetValue(4)
'    '    '            ''Format date
'    '    '            Dim arrLogTime As String()
'    '    '            Dim stringLogTime As String = lnArr.GetValue(1).ToString.Trim
'    '    '            arrLogTime = stringLogTime.Split(" ")

'    '    '            Dim dt As Date = Format(CDate(arrLogTime(0).Trim), "MM/dd/yyyy")
'    '    '            Dim tme As String = Format(arrLogTime(1)).Trim
'    '    '            ''Verify what status
'    '    '            TransType = Identify_TransactionType(TransType)
'    '    '            ''Check if logs is exist in the CHECKINOUT table
'    '    '            'If VerifyRawLogsIfExist(staffID, dt, tme, TransType) = False Then
'    '    '            '    InsertNewRawLogs(LogsID, staffID, dt, tme, TransType, Origin, False)
'    '    '            'End If
'    '    '        End While
'    '    '    End Using
'    '    'End Sub
'    '    'Private Sub RawLogsReaderWorkerProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs)

'    '    'End Sub
'    '    'Private Sub RawLogsReaderWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs)
'    '    '    Form3.LoadingPbx.Visible = False
'    '    '    Form3.Label33.Visible = False
'    '    '    Form3.btnBrowseLogs.Visible = True
'    '    'End Sub
'    '#End Region


'#Region "Declared Sub Functions"
'    Private Function IDENTIFY_TRANSACTIONTYPE(ByVal val As Integer) As String
'        Dim result As String = String.Empty
'        Select Case val
'            Case 0
'                result = "IN"
'            Case 1
'                result = "OUT"
'            Case 2
'                result = "BRKOUT"
'            Case 3
'                result = "BRKIN"
'            Case 4
'                result = "OTIN"
'            Case 5
'                result = "OTOUT"
'        End Select

'        Return result
'    End Function

'    Public Function GET_LAST_RAW_LOG_ID() As Long
'        Dim result As Long = 0

'        ce_cmd = New SqlCeCommand("SELECT [RAWID] FROM LOGS ORDER BY [RAWID] DESC", ce_cnn)
'        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
'        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
'        If sdr.Read = True Then
'            result = sdr(0)
'        End If
'        sdr.Close()
'        Return result
'    End Function
'    Private Function VERIFY_USB_LOG(ByVal staffid As String, ByVal raw_date As Date) As Boolean
'        Dim result As Boolean = False
'        ce_query = "SELECT * FROM LOGS WHERE (STAFFID = '" & staffid & "') AND ([RAW] = '" & raw_date.ToShortDateString & "')"
'        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
'        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
'        Dim sdr As SqlCeDataReader = ce_cmd.ExecuteReader()
'        While sdr.Read = True
'            result = True
'        End While
'        sdr.Close()
'        Return result
'    End Function
'    Public Sub INSERT_NEW_LOG(logs_id As String, staff_id As String, dt As Date, tme As String, trans_type As String, orgn As String, isAdjtd As Boolean, raw As String)
'        ce_query = "INSERT INTO LOGS([RAWID], [STAFFID], [DATE], [LOGTIME], [TRANSACTIONTYPE],[ORIGIN],[ISADJUSTED],[RAW]) " & _
'     "VALUES(@InLogsID, @InUserID, @InAttDate, @InAttTime, @InCheckType, @Inorigin,@InisAdjt,@iraw)"

'        ce_cmd = New SqlCeCommand(ce_query, ce_cnn)
'        If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
'        With ce_cmd
'            .Parameters.Add(New SqlCeParameter("@InLogsID", logs_id))
'            .Parameters.Add(New SqlCeParameter("@InUserID", staff_id))
'            .Parameters.Add(New SqlCeParameter("@InAttDate", dt.ToShortDateString))
'            .Parameters.Add(New SqlCeParameter("@InAttTime", tme))
'            .Parameters.Add(New SqlCeParameter("@InChecktype", trans_type))
'            .Parameters.Add(New SqlCeParameter("@Inorigin", orgn))
'            .Parameters.Add(New SqlCeParameter("@InisAdjt", isAdjtd))
'            .Parameters.Add(New SqlCeParameter("@iraw", raw))
'            .ExecuteNonQuery()
'        End With
'    End Sub



'    Public Sub MAIN(filename As String)
'        Dim result As New List(Of String)
'        Dim udisk As New Structs.UDisk
'        Dim byDataBuf() As Byte = Nothing
'        Dim iLength As Integer 'length of the bytes to get from the data

'        Dim sPIN2 As String = ""
'        Dim sVerified As String = ""
'        Dim sTime_second As String = ""
'        Dim sDeviceID As String = ""
'        Dim sStatus As String = ""
'        Dim sWorkcode As String = ""
'        Dim dt As Date = Nothing


'        Dim stream As FileStream
'        stream = New FileStream(filename, FileMode.OpenOrCreate, FileAccess.Read)
'        byDataBuf = File.ReadAllBytes(filename)
'        iLength = Convert.ToInt32(stream.Length)

'        Dim lvItem As New ListViewItem
'        Dim iStartIndex As Integer = 0
'        Dim iOneLogLength As Integer 'the length of one line of attendence log
'        For i As Integer = iStartIndex To iLength - 2

'            If byDataBuf(i) = 13 And byDataBuf(i + 1) = 10 Then

'                iOneLogLength = (i + 1) + 1 - iStartIndex
'                Dim bySSRAttLog(iOneLogLength - 1) As Byte
'                Array.Copy(byDataBuf, iStartIndex, bySSRAttLog, 0, iOneLogLength)

'                udisk.GetAttLogFromDat(bySSRAttLog, iOneLogLength, sPIN2, sTime_second, sDeviceID, sStatus, sVerified, sWorkcode)
'                dt = sTime_second
'                If Not VERIFY_USB_LOG(sPIN2, sTime_second) Then
'                    INSERT_NEW_LOG(GET_LAST_RAW_LOG_ID() + 1, sPIN2, dt, Format(dt, "HH:mm:ss"), IDENTIFY_TRANSACTIONTYPE(sStatus), "USB", False, sTime_second)
'                End If

'                bySSRAttLog = Nothing
'                iStartIndex += iOneLogLength
'                iOneLogLength = 0
'            End If

'        Next
'        stream.Close()
'    End Sub
'#End Region

'End Module
