Public Class BISBIONetworkDownload

    ''DEVICE SETUP
    Private axCZKEM1 As New zkemkeeper.CZKEM
    Dim bIsConnected = False 'the boolean value identifies whether the device is connected
    Dim iMachineNumber As Integer 'the serial number of the device.After connecting the device ,this value will be changed.

    Public ExceptionMessage As String = ""

    'Public Sub MAIN()
    'End Sub


    Public Function DEVICE_CONNECT(ip As String, port As String) As Boolean
        Dim result As Boolean = False
        Dim idwErrorCode As Integer
        bIsConnected = axCZKEM1.Connect_Net(ip.Trim(), Convert.ToInt32(port.Trim()))
        If bIsConnected = True Then
            'MessageBox.Show("Connection Success!")
            iMachineNumber = 1 'In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
            axCZKEM1.RegEvent(iMachineNumber, 65535) 'Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
            result = True
        Else
            axCZKEM1.GetLastError(idwErrorCode)
            ExceptionMessage = "Unable to connect the device,ErrorCode=" & idwErrorCode
        End If
        Return result
    End Function

    Public Sub DOWNLOAD_ALL_ATTLOGS()

        Dim sdwEnrollNumber As String = ""
        Dim idwVerifyMode As Integer
        Dim idwInOutMode As Integer
        Dim idwYear As Integer
        Dim idwMonth As Integer
        Dim idwDay As Integer
        Dim idwHour As Integer
        Dim idwMinute As Integer
        Dim idwSecond As Integer
        Dim idwWorkcode As Integer

        Dim idwErrorCode As Integer
        Dim iGLCount = 0



        Dim sPIN2 As String = ""
        Dim sVerified As String = ""
        Dim sTime_second As String = ""
        Dim sDeviceID As String = ""
        Dim sStatus As String = ""
        Dim sWorkcode As String = ""
        Dim dt As Date = Nothing



        axCZKEM1.EnableDevice(iMachineNumber, False) 'disable the device
        If axCZKEM1.ReadGeneralLogData(iMachineNumber) Then 'read all the attendance records to the memory
            'get records from the memory
            While axCZKEM1.SSR_GetGeneralLogData(iMachineNumber, sdwEnrollNumber, idwVerifyMode, idwInOutMode, idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond, idwWorkcode)
                iGLCount += 1
                'lvItem = lvLogs.Items.Add(iGLCount.ToString())
                'ListBox1.Items.Add(sdwEnrollNumber & "|" & idwVerifyMode.ToString() & "|" & "|" & idwInOutMode.ToString() & "|" & idwYear.ToString() & "-" + idwMonth.ToString() & "-" & idwDay.ToString() & " " & idwHour.ToString() & ":" & idwMinute.ToString() & ":" & idwSecond.ToString() & "|" & _
                '                   "|" & idwWorkcode.ToString())

                'ListBox1.Items.Add(idwVerifyMode.ToString())
                'ListBox1.Items.Add(idwInOutMode.ToString())
                'ListBox1.Items.Add(idwYear.ToString() & "-" + idwMonth.ToString() & "-" & idwDay.ToString() & " " & idwHour.ToString() & ":" & idwMinute.ToString() & ":" & idwSecond.ToString())
                'ListBox1.Items.Add(idwWorkcode.ToString())
                sPIN2 = sdwEnrollNumber.ToString.Trim
                sVerified = idwVerifyMode.ToString.Trim
                dt = (idwYear.ToString() & "-" + idwMonth.ToString() & "-" & idwDay.ToString() & " " & idwHour.ToString() & ":" & idwMinute.ToString() & ":" & idwSecond.ToString()).Trim
                sWorkcode = idwWorkcode.ToString.Trim



            End While
        Else

            axCZKEM1.GetLastError(idwErrorCode)
            If idwErrorCode <> 0 Then
                MessageBox.Show("Reading data from terminal failed,ErrorCode: " & idwErrorCode)
            Else
                MsgBox("No data from terminal returns!", MsgBoxStyle.Exclamation, "Error")
            End If
        End If

        axCZKEM1.EnableDevice(iMachineNumber, True) 'enable the device
    End Sub


End Class
