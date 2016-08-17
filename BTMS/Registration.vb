Imports System.Net.NetworkInformation

Public Class Registration
    Dim indictionary As New Dictionary(Of String, String)
    Dim proddictionary As New Dictionary(Of String, String)
    Public Function GET_PRODUCTKEY(regkey As String) As String

        Dim PRODUCT_KEY As String = ""

        Dim xkeys As New List(Of Char)
        xkeys.AddRange(regkey.ToCharArray)
        Dim PRODUCTNAME As String = IDENTIFY_PRODUCT(xkeys(xkeys.Count - 1))
        Dim rawkeys As New List(Of String)
        Dim ctr As Integer = 1
        xkeys.ForEach(Sub(k)
                          If ctr = 1 Then
                              k = CChar(Char.ConvertFromUtf32(Char.ConvertToUtf32(k, 0) + 1))
                              Console.WriteLine(k)
                              rawkeys.Add("0" & k)
                              ctr += 1
                          ElseIf ctr = 2 Then
                              k = CChar(Char.ConvertFromUtf32(Char.ConvertToUtf32(k, 0) + 2))
                              Console.WriteLine(k)
                              rawkeys.Add("0" & k)
                              ctr += 1
                          ElseIf ctr = 3 Then
                              k = CChar(Char.ConvertFromUtf32(Char.ConvertToUtf32(k, 0) + 3))
                              Console.WriteLine(k)
                              rawkeys.Add("0" & k)
                              ctr = 1
                          End If
                      End Sub)
        init_pdictionary()

        Dim PRODLIST As New List(Of String)
        rawkeys.ForEach(Sub(key)
                            If indictionary.ContainsKey(key) Then
                                ' Write value of the key.
                                Dim kv As String = indictionary.Item(key)
                                Console.WriteLine("Key: " & kv)
                                PRODLIST.Add(kv)
                            Else
                                'Dim kv As String = dictionary.Item(key)
                                Console.WriteLine("SKIP: " & key)
                                'PRODLIST.Add("0")
                            End If
                        End Sub)


        PRODUCT_KEY = String.Join("", PRODLIST.ToArray)
        Console.WriteLine("Registration Complete!")
        Console.WriteLine("PRODUCTTYPE: " & PRODUCTNAME & " REGISTRATIONKEY: " & regkey & " PRODUCTKEY: " & String.Join("", PRODLIST.ToArray))



        Return PRODUCT_KEY
    End Function
    ''' <summary>
    ''' Returns the the physcal address
    ''' </summary>
    Public Function GET_PHISYCAL_ADDRESS() As String
        Dim nics() As NetworkInterface = NetworkInterface.GetAllNetworkInterfaces()
        Return nics(0).GetPhysicalAddress.ToString
    End Function

    ''' <summary>
    ''' Return the registration key
    ''' </summary>
    ''' <param name="value">Software type</param>
    ''' <remarks></remarks>
    Public Function GET_REGKEY(BTMS As Boolean, FAMS As Boolean, POS As Boolean,
                               CANTEEN_BILLING As Boolean, CURRENCY_CONVERTER As Boolean) As String
        on Error Resume Next
        Dim result As String = ""

        ''GET PRODUCTNAME CODE
        Dim PRODUCTCODE As String = ""
        init_product_dictionary()
        If BTMS = True Then
            PRODUCTCODE = proddictionary.Item("BTMS")
        ElseIf FAMS = True Then
            PRODUCTCODE = proddictionary.Item("FAMS")
        ElseIf POS = True Then
            PRODUCTCODE = proddictionary.Item("POS")
        ElseIf CANTEEN_BILLING = True Then
            PRODUCTCODE = proddictionary.Item("CANTEEN BILLING")
        ElseIf CURRENCY_CONVERTER = True Then
            PRODUCTCODE = proddictionary.Item("CURRENCY CONVERTER")
        Else
            PRODUCTCODE = proddictionary.Item("UNKNOWN")
        End If

        Dim hddsn_details As New List(Of String)
        Dim SN As String = "CAMSIBB"

        ''wmic memorychip get serialnumber
        ''wmic diskdrive get serialnumber
        ''wmic baseboard get serialnumber
        ''wmic cdrom where drive='d:' get SerialNumber
        hddsn_details = EXECUTE_WMIC("wmic diskdrive where index=0 get serialnumber")
        ''GET bios SN LOCATED A POSITION 1
        SN = hddsn_details(1).ToString.Trim
        If Not SN.Length < 6 Then
            ''  GET LAST 6 DIGIT IN SN
            Dim regkey As New List(Of Char)
            regkey.AddRange(SN.ToCharArray)
            SN = regkey(regkey.Count - 6) & _
                regkey(regkey.Count - 5) & _
                regkey(regkey.Count - 4) & _
                regkey(regkey.Count - 3) & _
                regkey(regkey.Count - 2) & _
                regkey(regkey.Count - 1)
        Else
            hddsn_details = EXECUTE_WMIC("wmic baseboard get serialnumber")
            SN = hddsn_details(1).ToString.Trim
            Dim regkey As New List(Of Char)
            regkey.AddRange(SN.ToCharArray)
            SN = regkey(regkey.Count - 6) & _
                regkey(regkey.Count - 5) & _
                regkey(regkey.Count - 4) & _
                regkey(regkey.Count - 3) & _
                regkey(regkey.Count - 2) & _
                regkey(regkey.Count - 1)
        End If
        


        Console.WriteLine("SN: " & SN)
        result = SN & PRODUCTCODE
        Console.WriteLine("REGKEY: " & result)

        Return result
    End Function


    Public Function GET_DRIVE_SERIAL_NUMBER() As String
        Dim DriveSerial As Long
        Dim fso As Object, Drv As Object
        'Create a FileSystemObject object
        fso = CreateObject("Scripting.FileSystemObject")
        Drv = fso.GetDrive(fso.GetDriveName(AppDomain.CurrentDomain.BaseDirectory))
        With Drv
            If .IsReady Then
                DriveSerial = .SerialNumber
            Else    '"Drive Not Ready!"
                DriveSerial = 88888
            End If
        End With
        'Clean up
        Drv = Nothing
        fso = Nothing
        GET_DRIVE_SERIAL_NUMBER = Hex(DriveSerial)
    End Function

    Function EXECUTE_WMIC(command As String) As List(Of String)
        ' ----- Load the output of cmd.exe into a ListBox.
        Dim CmdCommand As Process
        Dim oneLine As String
        Dim result As New List(Of String)
        ' ----- Remove any existing items.
        'OutputData.Items.Clear()
        'ActProcess.Enabled = False
        ' ----- Build and run the command.
        CmdCommand = New Process()
        CmdCommand.StartInfo.FileName = "cmd.exe"
        'If (IncludeAll.Checked = True) Then CmdCommand.StartInfo.Arguments = "/C " & j
        CmdCommand.StartInfo.Arguments = "/C" & command
        CmdCommand.StartInfo.UseShellExecute = False
        CmdCommand.StartInfo.RedirectStandardOutput = True
        CmdCommand.StartInfo.CreateNoWindow = True
        CmdCommand.Start()

        ' ----- Process each input line.
        Do While Not CmdCommand.StandardOutput.EndOfStream

            oneLine = CmdCommand.StandardOutput.ReadLine()
            Console.WriteLine("Line: " & oneLine)
            If Not String.IsNullOrEmpty(oneLine.Trim) Then
                result.Add(oneLine.ToString)
            End If
        Loop
        CmdCommand.WaitForExit()
        CmdCommand.Dispose()
        'ActProcess.Enabled = True
        Return result
    End Function
    Function IDENTIFY_PRODUCT(key As String) As String
        Dim result As String = ""
        init_product_dictionary()
        For Each pair In proddictionary
            If pair.Value = key Then
                result = pair.Key
            End If
        Next
        Return result
    End Function



    ''DICTIONARY FOR REGISTRATION KEY
    Sub init_rdictionary()
        indictionary.Clear()
        indictionary.Add("0", "H")
        indictionary.Add("1", "I")
        indictionary.Add("2", "J")
        indictionary.Add("3", "K")
        indictionary.Add("4", "L")
        indictionary.Add("5", "M")
        indictionary.Add("6", "N")
        indictionary.Add("7", "O")
        indictionary.Add("8", "P")
        indictionary.Add("9", "Q")
        indictionary.Add("A", "R")
        indictionary.Add("B", "S")
        indictionary.Add("C", "T")
        indictionary.Add("D", "U")
        indictionary.Add("E", "V")
        indictionary.Add("F", "W")
        indictionary.Add("G", "X")
        indictionary.Add("H", "Y")
        indictionary.Add("I", "Z")
        indictionary.Add("J", "0")
        indictionary.Add("K", "1")
        indictionary.Add("L", "2")
        indictionary.Add("M", "3")
        indictionary.Add("N", "4")
        indictionary.Add("O", "5")
        indictionary.Add("P", "6")
        indictionary.Add("Q", "7")
        indictionary.Add("R", "8")
        indictionary.Add("S", "9")
        indictionary.Add("T", "A")
        indictionary.Add("U", "B")
        indictionary.Add("V", "C")
        indictionary.Add("W", "D")
        indictionary.Add("X", "E")
        indictionary.Add("Y", "F")
        indictionary.Add("Z", "G")
    End Sub
    ''DICTIONARY FOR PRODUCT KEY
    Sub init_pdictionary()
        indictionary.Clear()
        indictionary.Add("00", "H")
        indictionary.Add("01", "I")
        indictionary.Add("02", "J")
        indictionary.Add("03", "K")
        indictionary.Add("04", "L")
        indictionary.Add("05", "M")
        indictionary.Add("06", "N")
        indictionary.Add("07", "O")
        indictionary.Add("08", "P")
        indictionary.Add("09", "Q")
        indictionary.Add("0A", "R")
        indictionary.Add("0B", "S")
        indictionary.Add("0C", "T")
        indictionary.Add("0D", "U")
        indictionary.Add("0E", "V")
        indictionary.Add("0F", "W")
        indictionary.Add("0G", "X")
        indictionary.Add("0H", "Y")
        indictionary.Add("0I", "Z")
        indictionary.Add("0J", "0")
        indictionary.Add("0K", "1")
        indictionary.Add("0L", "2")
        indictionary.Add("0M", "3")
        indictionary.Add("0N", "4")
        indictionary.Add("0O", "5")
        indictionary.Add("0P", "6")
        indictionary.Add("0Q", "7")
        indictionary.Add("0R", "8")
        indictionary.Add("0S", "9")
        indictionary.Add("0T", "A")
        indictionary.Add("0U", "B")
        indictionary.Add("0V", "C")
        indictionary.Add("0W", "D")
        indictionary.Add("0X", "E")
        indictionary.Add("0Y", "F")
        indictionary.Add("0Z", "G")
    End Sub

    Sub init_product_dictionary()
        proddictionary.Clear()
        proddictionary.Add("BTMS", "B")
        proddictionary.Add("FAMS", "I")
        proddictionary.Add("CANTEEN BILLING", "S")
        proddictionary.Add("CURRENCY CONVERTER", "M")
        proddictionary.Add("POS", "A")
        proddictionary.Add("UNKNOWN", "C")
    End Sub

End Class
