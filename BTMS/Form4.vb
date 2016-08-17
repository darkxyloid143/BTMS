Imports System.Net
Imports System.Net.NetworkInformation
Imports System.IO
Public Class Form4
    ''DECLARE BISBIO DEVICES
    Public BISBIO_1 As New zkemkeeper.CZKEM
    Public port As String = "4370"

#Region "Setup the FORM4"

    Private Sub Form4_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ''Remove the handler when closing the form.
        RemoveHandler BISBIO_1.OnAttTransactionEx, AddressOf BISBIO_1_OnAttTransactionEx
        Form3.Visible = True
    End Sub
    Private Sub Form4_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        lbl_staffid.Text = ""
        lblName.Text = ""
        lbl_staff_address.Text = ""
        pbx_user_img.Image = My.Resources.login
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Form3.Visible = False
        thread_dev_connect.RunWorkerAsync()



    End Sub
    Private Sub SplitContainer1_Panel1_DoubleClick(sender As System.Object, e As System.EventArgs) Handles SplitContainer1.Panel1.DoubleClick
        If Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None Then
            Me.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedSingle
            Me.WindowState = FormWindowState.Normal
        Else
            Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
            Me.WindowState = FormWindowState.Maximized
        End If
    End Sub


    Private Sub Form4_Resize(sender As Object, e As System.EventArgs) Handles Me.Resize
        If Me.WindowState = FormWindowState.Normal Then
            Me.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedSingle
        ElseIf Me.WindowState = FormWindowState.Maximized Then
            Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        End If
    End Sub
#End Region


#Region "Communication BISBIO_1"
    ''This region is Transactions for BISBIO 1
    ''
    ''
    Private bIsConnected_BISBIO_1 = False 'the boolean value identifies whether the device is connected
    Private iMachineNumber_BISBIO_1 As Integer 'the serial number of the device.After connecting the device ,this value will be changed.

  

    Private Sub BISBIO_1_OnAttTransactionEx(ByVal sEnrollNumber As String, ByVal iIsInValid As Integer, ByVal iAttState As Integer, ByVal iVerifyMethod As Integer, _
                   ByVal iYear As Integer, ByVal iMonth As Integer, ByVal iDay As Integer, ByVal iHour As Integer, ByVal iMinute As Integer, ByVal iSecond As Integer, ByVal iWorkCode As Integer)
        '    lbRTShow.Items.Add("RTEvent OnAttTrasactionEx Has been Triggered,Verified OK")
        Dim sqlworker As New SQLCE_MANAGER


        lbl_staffid.Text = ""
        lblName.Text = ""
        lbl_staff_address.Text = ""
        pbx_user_img.Image = My.Resources.login


        Console.WriteLine("...UserID:" & sEnrollNumber)
        Console.WriteLine("...attState:" & iAttState.ToString())
        Console.WriteLine("...Time:" & iYear.ToString() & "-" & iMonth.ToString() & "-" & iDay.ToString() & " " & iHour.ToString() & ":" & iMinute.ToString() & ":" & iSecond.ToString())
        lbl_staffid.Text = "#: " & sEnrollNumber
        lblName.Text = sqlworker.GET_STAFF_NAME(sEnrollNumber)
        lbl_staff_address.Text = sqlworker.GET_STAFF_OTHER_DETAILS(sEnrollNumber)

        Dim imglocjpg As String = Application.StartupPath & "\Photo\" & sEnrollNumber & ".jpg"
        Dim imglocpng As String = Application.StartupPath & "\Photo\" & sEnrollNumber & ".png"

        If File.Exists(imglocjpg) Then
            Dim ImageA As Image
            Using Str As Stream = File.OpenRead(imglocjpg)
                ImageA = Image.FromStream(Str)
            End Using
            pbx_user_img.Image = ImageA
            pbx_user_img.SizeMode = PictureBoxSizeMode.StretchImage

        ElseIf File.Exists(imglocpng) Then
            Dim ImageA As Image
            Using Str As Stream = File.OpenRead(imglocpng)
                ImageA = Image.FromStream(Str)
            End Using
            pbx_user_img.Image = ImageA
            pbx_user_img.SizeMode = PictureBoxSizeMode.StretchImage
        End If
    End Sub

#End Region
    Private Sub user_pnl_DoubleClick(sender As System.Object, e As System.EventArgs) Handles user_pnl.DoubleClick
        If Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None Then
            Me.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedSingle
            Me.WindowState = FormWindowState.Normal
        Else
            Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
            Me.WindowState = FormWindowState.Maximized
        End If
    End Sub

    Private Sub thread_dev_connect_DoWork(sender As System.Object, e As System.ComponentModel.DoWorkEventArgs) Handles thread_dev_connect.DoWork
        ''During formload lets collect all the devices that save to our DB.
        ''and check each of it if valid and lets connect to it to perform
        ''the Real Time Events
        Dim sql_reader As New SQLCE_MANAGER
        Dim list_of_name_devices As New List(Of String)
        Dim list_of_devip As New List(Of String)
        list_of_name_devices = sql_reader.GET_ALL_BISBIO_DEVICES()
        For Each dev As String In list_of_name_devices
            'Console.WriteLine(sql_reader.GET_DETAILS_DEVICE_IP(dev))
            Dim devip As String = sql_reader.GET_DETAILS_DEVICE_IP(dev)

            Dim chckip As String() = devip.Split(New Char() {"."c})
            If chckip.Count = 4 Then
                Dim ctr As Integer = 0
                For Each int_Val As String In chckip
                    If IsNumeric(int_Val) Then
                        ''check if the value is within the range of 0-255 ipv4 octets
                        If Not int_Val < 0 Or int_Val > 255 Then
                            '''the address is valid

                        End If
                    Else
                        '''
                        ''invalid address
                        Console.WriteLine(">Invalid Address:" & devip)
                        GoTo EXIT_DEV
                    End If
                Next
                ''verdict here if detect not in range of ipv4 octets
            Else
                Console.WriteLine(">Invalid Address:" & devip)
                GoTo EXIT_DEV

            End If

            Dim ping As New System.Net.NetworkInformation.Ping
            Dim reply As System.Net.NetworkInformation.PingReply


            reply = ping.Send(devip)




            If reply.Status = IPStatus.Success Then
                ' --- handle reachable here
                Console.WriteLine("ping time " & reply.Options.Ttl)
                If reply.Options.Ttl = "64" Then
                    ''ttl= 64 mostly if the device is directly connected to the ethernet interface 
                    ''the reply ttl value = 64, means it is a linux device or zkteco device/bisbio.
                    list_of_devip.Add(devip)
                End If
            Else
                ' --- handle non reachable here
                UpdateText(lbl_dev_status, "Unable to connect to the device,DevIp=" & devip)
            End If
EXIT_DEV:
        Next



        ''no lets check if listof ip has ip addresses that are valid
        Select Case list_of_devip.Count
            Case 1
                ''FUNCTION FOR ONE DEVICE ONLY
                ''by default port will be 4370
                Console.WriteLine("Triggered 1 device detected")
                'BISBIO_1_Connect(list_of_devip(0), "4370")
                Dim idwErrorCode As Integer
                ' Cursor = Cursors.WaitCursor

                bIsConnected_BISBIO_1 = BISBIO_1.Connect_Net(list_of_devip(0).Trim(), Convert.ToInt32(port.Trim()))
                If bIsConnected_BISBIO_1 = True Then
                    'btnConnect.Text = "Disconnect"
                    'btnConnect.Refresh()
                    'lblState.Text = "Current State:Connected"
                    iMachineNumber_BISBIO_1 = 1 'In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.

                    If BISBIO_1.RegEvent(iMachineNumber_BISBIO_1, 65535) = True Then 'Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
                        AddHandler BISBIO_1.OnAttTransactionEx, AddressOf BISBIO_1_OnAttTransactionEx
                        ' UpdateText(lbl_dev_status, "Connected..")
                    Else
                        BISBIO_1.GetLastError(idwErrorCode)
                        '    MsgBox("Unable to connect the device,ErrorCode=" & idwErrorCode, MsgBoxStyle.Exclamation, "Error")

                        UpdateText(lbl_dev_status, "Unable to connect the device, ErrorCode=" & idwErrorCode)

                    End If
                End If
            Case 2
                ''FUNCTION FOR TWO DEVICES COUNT
                Console.WriteLine("Triggered 2 device detected")
            Case 3
                ''FUNCTION FOR THREE DEVICES COUNT
                Console.WriteLine("Triggered 3 device detected")
        End Select
    End Sub
    Public Sub UpdateText(ByVal labelx As Label, ByVal text As String)
        Invoke(New MethodInvoker(Sub()
                                     labelx.Text = text
                                     labelx.Refresh()
                                 End Sub))
    End Sub
End Class