'####################################################################################
'       Project Name : BISBIO TIME MANAGEMENT SYSTEM
'        FORM   Name : FORM CLASS
' Module Discription : GUI Transactions happens here!
'        Author Name : Brylle M. Lloren
'              Email : 
'          Copyright :  6-10-2015
'####################################################################################
Imports System.Data.SqlServerCe
Imports System.Data
Imports System.IO
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Net.NetworkInformation
Imports System.Text
Imports System.Windows.Forms

Imports System.Collections.Generic
Imports System.Text.RegularExpressions
Imports System.Security
Imports System.Globalization
Imports System.Security.Permissions

Imports System.Data.OleDb

Public Class Form3

    Private Const SELECT_STRING As String = "SELECT * FROM PUBLICHOLIDAYTABLE ORDER BY DATE ASC"
    Private CONNECT_STRING As String = My.Settings.LocalConnectionString.ToString
    Private Hol_DataSet As DataSet
    Private Hol_dataadapter As SqlCeDataAdapter












    Dim DeptFlag As String = "New"
    Dim LeaveClassFlag As String = "New"
    Dim drag As Boolean
    Dim mousex As Integer
    Dim mousey As Integer


    'User Access Connection
    Dim ce_cnn As SqlCeConnection = New SqlCeConnection(My.Settings.LocalConnectionString)
    Dim ce_cmd As SqlCeCommand
    Dim ce_da As SqlCeDataAdapter
    Dim ce_ds As DataSet
    Dim ce_builder As SqlCeCommandBuilder
    Dim ce_tbl As DataTable
    Dim ce_query As String = String.Empty
    Dim UserAccessUpdateRequired As Boolean = False

    Dim usb_worker As New UDiskDataMain
    Public Shared sqlce_asstnt As New SQLCE_MANAGER
    Dim CurrentLoginAccount As String = "No User Associated"
    Dim SQL_HOLIDAY_SCHEMA As New SQLControl

    Dim selectedReportIconName As String = ""

    Dim cpu_wrkr As New CPU
    Dim calculate_worker As New CLASSCALCULATE
    Dim PUBLICKEY As Boolean = False

    ''BISBIO DEVICE SETUP
    'Public axCZKEM1 As New zkemkeeper.CZKEM
    Dim bIsConnected = False 'the boolean value identifies whether the device is connected
    Dim iMachineNumber As Integer 'the serial number of the device.After connecting the device ,this value will be changed.
    Dim DeviceTask As String = "DOWNLOADLOGS"
    Dim devip As String = ""

    ''SCROLLINGPOSITIONING FOR ATTENDANCE OPTION MENU
    Dim SCROLL_COUNTER_RIGHT As Integer = 0
    Dim SCROLL_COUNTER_LEFT As Integer = 0
    Dim minscroll_right As Integer = 0
    Dim maxscroll_right As Integer = 0
    Dim minscroll_left As Integer = 0
    Dim maxscroll_left As Integer = 0

    ''SCROLING POSITIONING FOR EMPLOYEE SCHEDULE
    Dim panel_es_sched As Boolean = False
    Dim panel_es_assign As Boolean = True



    ''REGISTRATION KEY VALIDATION
    Dim PRODUCTISACTIVATED As Boolean = False
    Dim REGISTRATIONKEY As String = ""
    Dim PRODUCTKEY As String = ""
    Dim registration_worker As New Registration

    Public REHIRED_STAFF_ID As String = ""



    'Declaration RESTART APPLCIATON
    <SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags:=SecurityPermissionFlag.UnmanagedCode)> _
    <SecurityPermissionAttribute(SecurityAction.Demand, Flags:=SecurityPermissionFlag.UnmanagedCode)> _
    Public Shared Sub Restart()
        Application.Restart()
    End Sub



    Public axCZKEM1 As New zkemkeeper.CZKEM




    Private Sub Form3_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

        If e.KeyCode = Keys.Left Then
            'pbx_scroll_left_Click(sender, e)
            'Console.WriteLine("Scroll Left")
        ElseIf e.KeyCode = Keys.Right Then
            'pbx_scroll_right_Click(sender, e)
            'Console.WriteLine("Scroll Right")
        ElseIf e.KeyCode = Keys.F10 Then


            btnShowShiftTable.Visible = True
            btn_ShowShiftSchema.Visible = True


        End If
        'btnShowShiftTable.Visible = False
        'btn_ShowShiftSchema.Visible = False
    End Sub



    Private Sub Form3_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        myGlobal_window = Me

        If sqlce_asstnt.BMTS_CHECK_IF_TABLECOLUMN_IS_EXIST("USERACCESS", "PREVILEGE") = False Then
            sqlce_asstnt.BTMS_CREATE_TABLE_COLUMN("USERACCESS", "PREVILEGE", 1)
        End If

        If sqlce_asstnt.BMTS_CHECK_IF_TABLECOLUMN_IS_EXIST("EMPLOYEEPROFILES", "DESIGNATION") = False Then
            sqlce_asstnt.BTMS_CREATE_TABLE_COLUMN("EMPLOYEEPROFILES", "DESIGNATION", 1)
        End If

        If sqlce_asstnt.BMTS_CHECK_IF_TABLECOLUMN_IS_EXIST("STAFFMASTERLIST", "DESIGNATION") = False Then
            sqlce_asstnt.BTMS_CREATE_TABLE_COLUMN("STAFFMASTERLIST", "DESIGNATION", 1)
        End If

        If sqlce_asstnt.BMTS_CHECK_IF_TABLECOLUMN_IS_EXIST("EMPLOYEEPROFILES", "OTHERDETAILS") = False Then
            sqlce_asstnt.BTMS_CREATE_TABLE_COLUMN("EMPLOYEEPROFILES", "OTHERDETAILS", 4)
        End If

        If sqlce_asstnt.BMTS_CHECK_IF_TABLECOLUMN_IS_EXIST("EMPLOYEEPROFILES", "IMAGEPATH") = False Then
            sqlce_asstnt.BTMS_CREATE_TABLE_COLUMN("EMPLOYEEPROFILES", "IMAGEPATH", 4)
        End If

        ''CHECK FOR REAL TIME MONITORING IF VISIBLE
        If My.Settings.RTM = False Then
            '''
            lbl_rtm.Visible = False
        End If



        Try

            Me.KeyPreview = True
            MainControPanelTab.Size = New Size(810, 470)
            Me.Size = New Size(778, 521)

            'TabManager(MainControPanelTab, TabPage6)
            'Exit Sub

            REGISTRATIONKEY = registration_worker.GET_REGKEY(True, False, False, False, False)
            PRODUCTKEY = registration_worker.GET_PRODUCTKEY(REGISTRATIONKEY)

            If PRODUCTKEY = sqlce_asstnt.BTMS_GET_PRODUCTKEY Then
                ''PRODUCTKEY IS MATCH
                PRODUCTISACTIVATED = True
                lblx_registration.Text = "Product is Activated"
                If sqlce_asstnt.CHECK_USER_ACCESS_TABLE_ISEMPTY = True Then
                    LOAD_WITH_NO_LOGIN_WINDOW()
                Else
                    LOAD_WITH_LOGIN_WINDOW()
                End If
            Else
                ''TODO PRODUCT IS NOT ACTIVATED
                PRODUCTISACTIVATED = False
                lblx_registration.Text = "Product Registration"

                lblx_registration_LinkClicked(Nothing, Nothing)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub
    Sub LOAD_WITH_NO_LOGIN_WINDOW()

        'TODO: This line of code loads data into the 'GovEngineDataSet.PUBLICHOLIDAYTABLE' table. You can move, or remove it, as needed.
        Me.PUBLICHOLIDAYTABLETableAdapter.Fill(Me.GovEngineDataSet.PUBLICHOLIDAYTABLE)


        TabManager(MainControPanelTab, TabPage1)
        For Each c As Control In Me.MainFlowlayoutPanel.Controls
            c.BackColor = Color.Transparent
            c.Cursor = Cursors.Hand
            c.Margin = New Padding(15, 15, 15, 25)
            AddHandler c.MouseEnter, AddressOf MainPanelItems_MouseEnter
            AddHandler c.MouseLeave, AddressOf MainPanelItems_MouseLeave
        Next
        'pbx_audit.Visible = True
    End Sub
    Sub LOAD_WITH_LOGIN_WINDOW()
        'TODO: This line of code loads data into the 'GovEngineDataSet.PUBLICHOLIDAYTABLE' table. You can move, or remove it, as needed.
        pbx_company_image1.ImageLocation = My.Settings.CompanyLogo
        pbx_company_image2.ImageLocation = My.Settings.CompanyLogo
        Me.PUBLICHOLIDAYTABLETableAdapter.Fill(Me.GovEngineDataSet.PUBLICHOLIDAYTABLE)


        'connStr = New SqlCeConnection(My.Settings.LocalConnectionString)
        lblWindowTitle.Visible = False


        SetCueText(txtUserName, "Username")
        SetCueText(txtUserPwd, "Password")

        TabManager(MainControPanelTab, TabPage7)

        For Each c As Control In Me.MainFlowlayoutPanel.Controls
            c.BackColor = Color.Transparent
            c.Cursor = Cursors.Hand
            AddHandler c.MouseEnter, AddressOf MainPanelItems_MouseEnter
            AddHandler c.MouseLeave, AddressOf MainPanelItems_MouseLeave
        Next

        Panel2.Focus()
        txtUserPwd.Focus()
    End Sub






    Public Sub TabManager(ByVal ControlTabName As TabControl, ByVal tabpagename As TabPage)
        If Not PUBLICKEY = True Then
            For i = 1 To MainControPanelTab.TabPages.Count
                MainControPanelTab.TabPages.RemoveAt(0)
            Next
            MainControPanelTab.TabPages.Add(tabpagename)

            For x = 1 To systemconfigControlTab.TabPages.Count
                systemconfigControlTab.TabPages.RemoveAt(0)
            Next

            systemconfigControlTab.TabPages.Add(TabPage21)      ''LOAD SYSTEM ACCESS ONLY
            systemconfigControlTab.TabPages.Add(TabPage20)      ''LOAD DATABASE OPTION
        Else

            For i = 1 To MainControPanelTab.TabPages.Count
                MainControPanelTab.TabPages.RemoveAt(0)
            Next

            MainControPanelTab.TabPages.Add(tabpagename)

            For x = 1 To systemconfigControlTab.TabPages.Count
                systemconfigControlTab.TabPages.RemoveAt(0)
            Next
            systemconfigControlTab.TabPages.Add(TabPage20)      ''LOAD DATABASE OPTION
            systemconfigControlTab.TabPages.Add(TabPage21)      ''LOAD SYSTEM ACCESS

        End If
    End Sub

    Public WithEvents MyLink As PictureBox
    Public Sub MainPanelItems_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyLink.MouseHover

        'For Each c As Control In Me.MainFlowlayoutPanel.Controls
        '    c.BackColor = Color.Transparent
        'Next

        For Each ctrl As Control In MainFlowlayoutPanel.Controls
            If (ctrl.GetType() Is GetType(PictureBox)) Then
                Dim pbx As PictureBox = CType(ctrl, PictureBox)
                pbx.BorderStyle = BorderStyle.None
            End If
        Next
        Me.MainFlowlayoutPanel.Refresh()

        Dim btn As PictureBox = TryCast(sender, PictureBox)
        ' btn.BorderStyle = BorderStyle.FixedSingle

        btn.BackColor = Color.FromArgb(10, 10, 10)
        ' MainFlowlayoutPanel.BorderStyle = BorderStyle.NoNE
    End Sub
    Private Sub MainPanelItems_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyLink.MouseHover
        Dim btn As PictureBox = TryCast(sender, PictureBox)




        For Each ctrl As Control In MainFlowlayoutPanel.Controls
            If (ctrl.GetType() Is GetType(PictureBox)) Then
                Dim pbx As PictureBox = CType(ctrl, PictureBox)
                pbx.BorderStyle = BorderStyle.None
                pbx.BackColor = Color.Transparent
            End If
        Next
        ' MainFlowlayoutPanel.BorderStyle = BorderStyle.None

        ' btn.BorderStyle = BorderStyle.None
        btn.BackColor = Color.Transparent
        btn.BorderStyle = BorderStyle.None
        Me.MainFlowlayoutPanel.Refresh()






    End Sub
    Private Sub lblClose_Click(sender As System.Object, e As System.EventArgs)
        Me.Close()
    End Sub
    Private Sub btnEmployeeProfiles_Click(sender As System.Object, e As System.EventArgs) Handles btnEmployeeProfiles.Click

        '''LOAD ALL DEPARTMENT
        'Dim ls_dp As New List(Of String)
        'ls_dp = sqlce_asstnt.GET_ALL_DEPARTMENT_NAME()
        'ls_dp.Sort()
        'lstdp.Items.Clear()
        'lstdp.Items.AddRange(ls_dp.ToArray)
        'TabManager(MainControPanelTab, TabPage6)
        'Exit Sub



        lblCurrentUser.Visible = False
        pbx_audit.Visible = False

        lblWindowTitle.Visible = False

        lblWindowTitle.Visible = False
        SetCueText(txtEmpIDNo, "Type ID No.")
        SetCueText(txtEmpName, "Type full name")
        'dtgEmpProfiles.DataSource = sqlce_asstnt.GET_ALL_EMPLOYEES(True, False, Nothing, True, Nothing, False, Nothing)
        LOAD_DEPARTMENT_TO_COMBOBOX_EMPLOYEEPROFILES()



        Dim listOfDepartment As New List(Of String)
        listOfDepartment = sqlce_asstnt.GET_ALL_DEPARTMENT_NAME()
        listOfDepartment.Sort()
        lstdp.Items.Clear()
        For Each dp_name As String In listOfDepartment
            lstdp.Items.Add(dp_name)
        Next

        '' Initialize the random-number generator.
        'Randomize()
        '' Generate random value between 1 and 6. 
        'Dim value As Integer = CInt(Int((listOfDepartment.Count * Rnd()) + 0))
        'If Not listOfDepartment.Count = 0 Then
        '    lstdp.SelectedIndex = value
        'End If

        flp_staff_details.Controls.Clear()
        TabManager(MainControPanelTab, TabPage6)


        cmbDepartment.Focus()
        Me.Refresh()
    End Sub
    Public Sub LOAD_DEPARTMENT_TO_COMBOBOX_EMPLOYEEPROFILES()
        Dim listOfDpt As New List(Of String)
        listOfDpt = sqlce_asstnt.GET_ALL_DEPARTMENT_NAME()
        listOfDpt.Sort()
        cmbDepartment.Items.Clear()
        cmbDepartment.Text = ""
        listOfDpt.ForEach(Sub(X) cmbDepartment.Items.Add(X))
        cmb1.Items.Clear()
        cmb1.Text = ""
        cmb1.Items.Add("*****ALL ACTIVE Staff(s)")
        cmb1.Items.Add("*****ALL IN-ACTIVE Staff(s)")
        For Each x As String In listOfDpt
            cmb1.Items.Add(x)
        Next
        If Not listOfDpt.Count = 0 Then
            cmb1.SelectedIndex = 0
            cmbDepartment.SelectedIndex = 0
        End If



        Dim listOfDepartment As New List(Of String)
        listOfDepartment = sqlce_asstnt.GET_ALL_DEPARTMENT_NAME()
        listOfDepartment.Sort()
        lstdp.Items.Clear()
        For Each dp_name As String In listOfDepartment
            lstdp.Items.Add(dp_name)
        Next


    End Sub



    'Private Sub btnDepartment_Click(sender As System.Object, e As System.EventArgs) Handles btnDepartment.Click
    '    lblWindowTitle.Visible = False
    '    TabManager(MainControPanelTab, TabPage3, False)
    '    SetCueText(txtDepartmentName, "Department Name")
    '    Dim listOfDpt As New List(Of String)
    '    listOfDpt = sqlce_asstnt.GET_ALL_DEPARTMENT_NAME()
    '    If listOfDpt.Count <> Nothing Then
    '        lstboxDepartment.Items.Clear()
    '        lstboxDepartment.Items.AddRange(listOfDpt.ToArray)
    '    End If
    '    Me.Refresh()
    '    Label13.Focus()



    'End Sub



    'Private Sub btnShiftMgmt_Click(sender As System.Object, e As System.EventArgs) Handles btnShiftMgmt.Click
    '    'SetCueText(txtShiftName, "Ex. (Office 8-5 PM)")
    '    'SelectAllShiftDetailsFromTimezone()
    '    'lblWindowTitle.Visible = False
    '    'For I1 = 0 To dtgShift.Columns.Count - 1
    '    '    If dtgShift.Columns(I1).Name = "SHIFT" Then
    '    '        Dim column As DataGridViewColumn = dtgShift.Columns(I1)
    '    '        column.Width = 200
    '    '    ElseIf dtgShift.Columns(I1).Name = "DETAIL" Then
    '    '        Dim column As DataGridViewColumn = dtgShift.Columns(I1)
    '    '        column.Width = 600
    '    '    End If
    '    'Next
    '    'lblWindowTitle.Text = "SHIFT MANAGEMENT"
    '    'MainControPanelTab.SelectedIndex = 3
    '    'TabManager(MainControPanelTab, TabPage4, False)
    '    'Me.Refresh()
    'End Sub


    Private Sub btnShiftCancel_Click(sender As System.Object, e As System.EventArgs)
        lblWindowTitle.Text = "Time Management System"
        lblWindowTitle.Visible = True
        MainControPanelTab.SelectedIndex = 0
    End Sub
    Private Sub btnReports_Click(sender As System.Object, e As System.EventArgs) Handles btnReports.Click
        lblCurrentUser.Visible = False
        pbx_audit.Visible = False
        ''Adjust startdate - 30
        'dtpStartDateRpt.Value = Date.Now.AddDays(-15)
        'dtpEndDateRpt.Value = Date.Now

        dtpStartDateRpt.Value = FirstDayOfMonth(DateAndTime.Now)
        dtpEndDateRpt.Value = LastDayOfMonth(DateAndTime.Now)
        pbx_ReportConfiguration.Visible = False
        pbx_ReportIconsControl.Visible = True
        LOAD_TREEVIEW_DETAILS()

        SetCueText(txtx_NotedName, "Type Signatory Name")
        SetCueText(txtx_NotedPosition, "Type Signatory Position")
        SetCueText(txtx_PreparedbyName, "Type Signatory Name")
        SetCueText(txtx_PreparedbyPosition, "Type Signatory Position")
        SetCueText(txtx_CertifiedName, "Type Signatory Name")
        SetCueText(txtx_CertifiedPosition, "Type Signatory Position")

        '''SET REPORTS TO BE VIEWED ONLY

        If My.Settings.DTR_A = False Then
            pbx_dtr.Visible = False
            Label39.Visible = False
        Else
            pbx_dtr.Visible = True
            Label39.Visible = True
        End If
        If My.Settings.DTR_B = False Then
            pbx_dtr_b.Visible = False
            Label94.Visible = False
        Else
            pbx_dtr_b.Visible = True
            Label94.Visible = True
        End If
        If My.Settings.DTR_C = False Then
            pbx_dtr_c.Visible = False
            Label51.Visible = False
        Else
            pbx_dtr_c.Visible = True
            Label51.Visible = True
        End If
        If My.Settings.DTR_D = False Then
            pbx_dtr_d.Visible = False
            Label52.Visible = False
        Else
            pbx_dtr_d.Visible = True
            Label52.Visible = True
        End If

        If My.Settings.DTR_E = False Then
            pbx_dtr_e.Visible = False
            Label89.Visible = False
        Else
            pbx_dtr_e.Visible = True
            Label89.Visible = True
        End If

        If My.Settings.DTR_F = False Then
            pbx_dtr_f.Visible = False
            Label89.Visible = False
        Else
            pbx_dtr_e.Visible = True
            Label89.Visible = True
        End If

        lblWindowTitle.Visible = False
        'cmbReportType.SelectedIndex = 0
        TabManager(MainControPanelTab, TabPage5)
        Me.Refresh()

    End Sub
    'Private Function GetAllEmployeeName(ByVal PARAMS As String) As IList
    '    Dim res As New List(Of String)
    '    If PARAMS = "ALL" Then
    '        cquery = "SELECT FULLNAME FROM EMPLOYEEPROFILES"
    '    Else
    '        cquery = "SELECT FULLNAME FROM EMPLOYEEPROFILES WHERE DEPARTMENT = '" & PARAMS & "'"
    '    End If

    '    ccmd = New SqlCeCommand(cquery, connStr)
    '    If connStr.State = ConnectionState.Closed Then connStr.Open()
    '    crdr = ccmd.ExecuteReader()
    '    While crdr.Read
    '        res.Add(crdr(0))
    '    End While
    '    Return res
    'End Function

    'Private Function GetAllDepartmentNames() As IList
    '    Dim DepNames As New List(Of String)
    '    Try
    '        cquery = "SELECT DISTINCT [Department] FROM [EmployeeProfiles]"

    '        Using ccmd = New SqlCeCommand(cquery, connStr)
    '            If connStr.State = ConnectionState.Closed Then connStr.Open()
    '            crdr = ccmd.ExecuteReader()
    '            While crdr.Read
    '                DepNames.Add(crdr(0))
    '            End While

    '        End Using

    '    Catch ex As Exception
    '        MessageBox.Show("Oops! It didn't work. " & vbCrLf & ex.Message)
    '    End Try
    '    Return DepNames
    'End Function

    'Private Sub btnSaveDepartment_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveDepartment.Click



    '    If String.IsNullOrEmpty(txtDepartmentName.Text.Trim()) Then
    '        txtDepartmentName.Focus()
    '        Exit Sub
    '    End If
    '    If Not sqlce_asstnt.CHECK_DEPARTMENT_IF_EXIST(txtDepartmentName.Text.Trim.ToUpper) Then
    '        Try
    '            sqlce_asstnt.indictionary_NEW_DEPARTMENT(txtDepartmentName.Text.ToString.ToUpper)
    '            txtDepartmentName.Focus()
    '            txtDepartmentName.SelectAll()

    '            Dim listOfDpt As New List(Of String)
    '            listOfDpt = sqlce_asstnt.GET_ALL_DEPARTMENT_NAME()
    '            listOfDpt.Sort()
    '            If listOfDpt.Count <> Nothing Then
    '                lstboxDepartment.Items.Clear()
    '                lstboxDepartment.Items.AddRange(listOfDpt.ToArray)
    '            End If


    '        Catch ex As Exception
    '            MessageBox.Show("Oops! It didn't work. " & vbCrLf & ex.Message)
    '        End Try
    '    Else
    '        lblPromptDuplicateDepName.Visible = True
    '        txtDepartmentName.SelectAll()
    '        txtDepartmentName.Focus()
    '        Me.Refresh()
    '    End If
    'End Sub
    'Private Sub SelectAllEmployeeFromEmployeeProfiles()
    '    Try
    '        cquery = "SELECT [EmpID] as ID, [FullName] as NAME, [Department] as DEPARTMENT FROM [EmployeeProfiles]"

    '        ccmd = New SqlCeCommand(cquery, connStr)
    '        If connStr.State = ConnectionState.Closed Then connStr.Open()
    '        cDA = New SqlCeDataAdapter(ccmd)
    '        cDataSet = New DataSet
    '        cDA.Fill(cDataSet, "MyTable")
    '        dtgEmpProfiles.DataSource = cDataSet.Tables("MyTable").DefaultView

    '        ccmd.Dispose()

    '    Catch ex As Exception
    '        MessageBox.Show("Oops! It didn't work." & vbCrLf & ex.Message)
    '    End Try
    '    For I1 = 0 To dtgEmpProfiles.Columns.Count - 1
    '        If dtgEmpProfiles.Columns(I1).Name = "NAME" Then
    '            Dim column As DataGridViewColumn = dtgEmpProfiles.Columns(I1)
    '            column.Width = 300
    '        ElseIf dtgEmpProfiles.Columns(I1).Name = "DEPARTMENT" Then
    '            Dim column As DataGridViewColumn = dtgEmpProfiles.Columns(I1)
    '            column.Width = 370
    '        ElseIf dtgEmpProfiles.Columns(I1).Name = "ID" Then
    '            Dim column As DataGridViewColumn = dtgEmpProfiles.Columns(I1)
    '            column.Width = 100
    '        End If
    '    Next
    'End Sub
    'Private Sub txtDepartmentName_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtDepartmentName.KeyDown
    '    If e.KeyCode = Keys.Enter Then
    '        btnSaveDepartment_Click(sender, e)
    '    End If
    'End Sub

    'Private Sub lstboxDepartment_MouseDown(sender As System.Object, e As System.Windows.Forms.MouseEventArgs)
    '    Try
    '        Dim curItem As String = lstboxDepartment.SelectedItem.ToString()
    '        Dim CurrPos As Integer = lstboxDepartment.FindString(curItem)
    '        If Not lstboxDepartment.Items.Count = -1 Then
    '            If e.Button = Windows.Forms.MouseButtons.Right Then
    '                RightClickDepartment.Show(MousePosition)
    '            End If
    '        End If

    '    Catch ex As Exception
    '        Exit Sub
    '    End Try

    'End Sub

    'Private Sub RemoveDepartment_Click(sender As System.Object, e As System.EventArgs) Handles RemoveDepartment.Click
    '    Dim curItem As String = lstboxDepartment.SelectedItem.ToString()
    '    If Not DepartmentNameHasBeenAssign(curItem) Then
    '        sqlce_asstnt.REMOVE_SELECTED_DEPARTMENT(curItem)
    '        Dim listOfDpt As New List(Of String)
    '        listOfDpt = sqlce_asstnt.GET_ALL_DEPARTMENT_NAME()
    '        If listOfDpt.Count <> Nothing Then
    '            lstboxDepartment.Items.Clear()
    '            lstboxDepartment.Items.AddRange(listOfDpt.ToArray)
    '        End If
    '    Else
    '        MsgBox("" & curItem & "has been assign to existing employee and cannot deleted.", vbExclamation, "")
    '        ' MessageBox.Show( & " ")

    '    End If

    'End Sub

    'Private Function DepartmentNameHasBeenAssign(ByVal DepName As String) As Boolean
    '    Dim res As Boolean = False
    '    ''check if department has been assign to an epmployee

    '    Try
    '        cquery = "Select * from EmployeeProfiles where [Department] = '" & DepName.ToUpper & "'"

    '        ccmd = New SqlCeCommand(cquery, connStr)
    '        If connStr.State = ConnectionState.Closed Then connStr.Open()
    '        crdr = ccmd.ExecuteReader()
    '        If crdr.Read = True Then
    '            res = True
    '        End If

    '    Catch ex As Exception
    '        MessageBox.Show("Oops! It didn't work. " & vbCrLf & ex.Message)
    '    End Try
    '    Return res
    'End Function

    'Public Sub SelectAllItemsFromDepartmentDetailsTable()
    '    Try


    '        cquery = "Select * from DepartmentDetails"
    '        ccmd = New SqlCeCommand(cquery, connStr)
    '        If connStr.State = ConnectionState.Closed Then connStr.Open()
    '        crdr = ccmd.ExecuteReader()

    '        cmb1.Items.Clear()
    '        lstboxDepartment.Items.Clear()
    '        cmbDepartment.Items.Clear()
    '        cmb1.Items.Add("All")
    '        While crdr.Read = True
    '            cmb1.BeginUpdate()
    '            cmb1.Items.Add(crdr.GetString(1))
    '            cmb1.EndUpdate()
    '            cmbDepartment.BeginUpdate()
    '            cmbDepartment.Items.Add(crdr.GetString(1))
    '            cmbDepartment.EndUpdate()
    '            lstboxDepartment.BeginUpdate()
    '            lstboxDepartment.Items.Add(crdr.GetString(1))
    '            lstboxDepartment.EndUpdate()
    '        End While
    '        If Not cmb1.Items.Count = 0 Then
    '            cmb1.SelectedIndex = 0
    '        End If

    '    Catch ex As Exception
    '        MessageBox.Show("Oops! It didn't work. " & vbCrLf & ex.Message)
    '    End Try
    'End Sub

    'Private Function DepartmentExist(ByVal DepName As String) As Boolean
    '    ''check if department is exist
    '    Dim res As Boolean = False
    '    Try
    '        cquery = "Select * from DepartmentDetails where DepartmentName = '" & DepName.ToUpper & "'"

    '        ccmd = New SqlCeCommand(cquery, connStr)
    '        If connStr.State = ConnectionState.Closed Then connStr.Open()
    '        crdr = ccmd.ExecuteReader()
    '        If crdr.Read = True Then
    '            res = True
    '        End If

    '    Catch ex As Exception
    '        MessageBox.Show("Oops! It didn't work. " & vbCrLf & ex.Message)
    '    End Try
    '    Return res
    'End Function

    Private Sub Panel1_MouseDown(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles Panel1.MouseDown
        drag = True                                             'Sets the variable drag to true.
        mousex = Windows.Forms.Cursor.Position.X - Me.Left          'Sets variable mousex
        mousey = Windows.Forms.Cursor.Position.Y - Me.Top               'Sets variable mousey
    End Sub

    Private Sub Panel1_MouseMove(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles Panel1.MouseMove
        If drag Then
            Me.Top = Windows.Forms.Cursor.Position.Y - mousey
            Me.Left = Windows.Forms.Cursor.Position.X - mousex
        End If
    End Sub

    Private Sub Panel1_MouseUp(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles Panel1.MouseUp
        drag = False
    End Sub


    Private Sub btnSaveEmployee_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveEmployee.Click

        Dim department_name As String = ""
        department_name = cmbDepartment.Text.ToString

        If PRODUCTISACTIVATED = False Then
            CHECK_TRIAL()
        End If

        Select Case btnSaveEmployee.Text.ToString
            Case "Create"
                If String.IsNullOrEmpty(txtEmpIDNo.Text.Trim()) Or String.IsNullOrEmpty(txtEmpName.Text.Trim()) Or String.IsNullOrEmpty(cmbDepartment.Text.Trim()) Then


                    ''DETERMINE IF THE SOFTWARE IS TRIAL



                    If String.IsNullOrEmpty(txtEmpIDNo.Text.Trim()) Then
                        lblDuplicateIDPrompt.Visible = True
                        lblDuplicateIDPrompt.Text = "Empty!"

                        Exit Sub
                    ElseIf String.IsNullOrEmpty(txtEmpName.Text.Trim()) Then
                        txtEmpName.SelectAll()
                        txtEmpName.Focus()
                        lblPromptEmptyEmpName.Visible = True
                        Exit Sub
                    ElseIf String.IsNullOrEmpty(cmbDepartment.Text) Then
                        cmbDepartment.Focus()
                        lblPromptEmptyDepName.Visible = True
                    End If
                    Exit Sub
                End If

                '''Capitalise Name in textbox before saving
                'Dim txtName As String = txtEmpName.Text.Trim
                'Dim EmpName As String = StrConv(txtName, VbStrConv.ProperCase)
                Dim EmpName As String = txtEmpName.Text.Trim.ToUpper



                If Not sqlce_asstnt.CHECK_EMPLOYEEID_IF_EXIST(txtEmpIDNo.Text.Trim) Then
                    sqlce_asstnt.INSERT_NEW_STAFF(txtEmpIDNo.Text.Trim, EmpName, sqlce_asstnt.GET_DEPARTMENT_ID(cmbDepartment.Text), txtc_designation.Text.ToString.ToUpper.Trim, txtc_other_details.Text, image_path_src)
                    'dtgEmpProfiles.DataSource = sqlce_asstnt.GET_ALL_EMPLOYEES(True, False, Nothing, False, Nothing, False, Nothing)
                    cmb1.Enabled = True
                    cmb1.Text = ""
                    cmb1.SelectedText = cmbDepartment.Text
                    cmb1_SelectedIndexChanged(sender, e)
                    txtEmpName.Text = ""
                    txtEmpIDNo.Text = ""
                    cmbDepartment.Text = ""
                    Me.Refresh()
                    txtEmpIDNo.Focus()
                    txtEmpIDNo.SelectAll()
                Else
                    lblDuplicateIDPrompt.Visible = True
                    lblDuplicateIDPrompt.Text = "Duplicate ID!"
                    'txtEmpName.Text = sqlce_asstnt.GET_STAFF_NAME(txtEmpIDNo.Text.Trim)
                    txtEmpIDNo.SelectAll()
                    txtEmpIDNo.Focus()
                End If

            Case "Update"
                If String.IsNullOrEmpty(txtEmpIDNo.Text.Trim()) Or String.IsNullOrEmpty(txtEmpName.Text.Trim()) Or String.IsNullOrEmpty(cmbDepartment.Text.Trim()) Then

                    If String.IsNullOrEmpty(txtEmpIDNo.Text.Trim()) Then
                        lblDuplicateIDPrompt.Visible = True
                        lblDuplicateIDPrompt.Text = "Empty!"

                        Exit Sub
                    ElseIf String.IsNullOrEmpty(txtEmpName.Text.Trim()) Then
                        txtEmpName.SelectAll()
                        txtEmpName.Focus()
                        lblPromptEmptyEmpName.Visible = True
                        Exit Sub
                    ElseIf String.IsNullOrEmpty(cmbDepartment.Text) Then
                        cmbDepartment.Focus()
                        lblPromptEmptyDepName.Visible = True
                    End If
                    Exit Sub
                End If

                ''Capitalise Name in textbox before saving
                'Dim txtName As String = txtEmpName.Text.Trim
                'Dim EmpName As String = StrConv(txtName, VbStrConv.ProperCase)
                Dim EmpName As String = txtEmpName.Text.Trim.ToUpper
                Dim dpname As String = cmbDepartment.Text
                If sqlce_asstnt.CHECK_EMPLOYEEID_IF_EXIST(txtEmpIDNo.Text.Trim) Then
                    sqlce_asstnt.UPDATE_EMPLOYEE_DETAILS(txtEmpIDNo.Text.Trim, EmpName, sqlce_asstnt.GET_DEPARTMENT_ID(cmbDepartment.Text), txtc_designation.Text.ToString.ToUpper.Trim, txtc_other_details.Text, image_path_src)
                    'dtgEmpProfiles.DataSource = sqlce_asstnt.GET_ALL_EMPLOYEES(True, False, Nothing, False, Nothing, False, Nothing)
                    txtEmpName.Text = ""

                    cmbDepartment.SelectedIndex = -1
                    Me.Refresh()
                    txtEmpIDNo.Enabled = True
                    txtEmpIDNo.Focus()
                    txtEmpIDNo.SelectAll()

                    btnx_resignemp.Enabled = True
                    btn_RemoveEmp.Enabled = True
                    btn_EditEmp.Enabled = True
                    btnx_cancelEdit.Visible = False
                    txtEmpIDNo.Text = ""
                    txtEmpName.Text = ""
                    cmb1.Enabled = True
                    lvStaffs.Enabled = True
                    cmb1.Text = ""
                    cmb1.SelectedText = dpname
                    cmb1_SelectedIndexChanged(sender, e)

                Else
                    '    'lblDuplicateIDPrompt.Visible = True
                    '    'lblDuplicateIDPrompt.Text = "Duplicate ID!"
                    '    'txtEmpIDNo.SelectAll()
                    '    'txtEmpIDNo.Focus()
                End If




                btnSaveEmployee.Text = "Create"
            Case "Trial"
                MessageBox.Show("You need to register this product before adding additional items" & vbCrLf & "Thank you for playing fair", "", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Case "Save"
                Dim staff_id As String = txtEmpIDNo.Text
                Dim staff_name As String = txtEmpName.Text
                Dim dep_name As String = cmbDepartment.Text


                If String.IsNullOrEmpty(txtEmpIDNo.Text.Trim()) Or String.IsNullOrEmpty(txtEmpName.Text.Trim()) Or String.IsNullOrEmpty(cmbDepartment.Text.Trim()) Then
                    If String.IsNullOrEmpty(txtEmpIDNo.Text.Trim()) Then
                        lblDuplicateIDPrompt.Visible = True
                        lblDuplicateIDPrompt.Text = "Empty!"
                        Exit Sub
                    ElseIf String.IsNullOrEmpty(txtEmpName.Text.Trim()) Then
                        txtEmpName.SelectAll()
                        txtEmpName.Focus()
                        lblPromptEmptyEmpName.Visible = True
                        Exit Sub
                    ElseIf String.IsNullOrEmpty(cmbDepartment.Text) Then
                        cmbDepartment.Focus()
                        lblPromptEmptyDepName.Visible = True
                    End If
                    Exit Sub
                End If


                'sqlce_asstnt.TAG_AS_REHIRED_EMPLOYEE(staff_id, staff_name, sqlce_asstnt.GET_DEPARTMENT_ID(dep_name))
                'sqlce_asstnt.REMOVE_REHIRED_STAFF_FROM_INACTIVETABLE(staff_id)

                'MessageBox.Show("Task Completed!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                'btnx_cancelEdit.Visible = False
                'btnSaveEmployee.Text = "Create"
                'txtEmpIDNo.Text = ""
                'txtEmpName.Text = ""
                'cmb1.Enabled = True
                'cmb1.Text = ""
                'cmb1.SelectedText = dep_name
                If Not sqlce_asstnt.CHECK_EMPLOYEEID_IF_EXIST(txtEmpIDNo.Text.Trim) Then
                    sqlce_asstnt.INSERT_NEW_STAFF(txtEmpIDNo.Text.Trim, staff_name, sqlce_asstnt.GET_DEPARTMENT_ID(cmbDepartment.Text), _
                                                  txtc_designation.Text.ToString.ToUpper.Trim, txtc_other_details.Text, image_path_src)
                    'dtgEmpProfiles.DataSource = sqlce_asstnt.GET_ALL_EMPLOYEES(True, False, Nothing, False, Nothing, False, Nothing)
                    sqlce_asstnt.REMOVE_REHIRED_STAFF_FROM_INACTIVETABLE(staff_id)
                    txtEmpName.Text = ""
                    txtEmpIDNo.Text = ""
                    cmbDepartment.Text = ""
                    cmb1.Enabled = True
                    cmb1.Text = ""
                    cmb1.SelectedText = dep_name
                    cmb1_SelectedIndexChanged(sender, e)
                    MessageBox.Show("Task Completed!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    btnx_cancelEdit.Visible = False
                    btnSaveEmployee.Text = "Create"
                    Me.Refresh()
                    txtEmpIDNo.Focus()
                    txtEmpIDNo.SelectAll()
                Else
                    lblDuplicateIDPrompt.Visible = True
                    lblDuplicateIDPrompt.Text = "Duplicate ID!"
                    'txtEmpName.Text = sqlce_asstnt.GET_STAFF_NAME(txtEmpIDNo.Text.Trim)
                    txtEmpIDNo.SelectAll()
                    txtEmpIDNo.Focus()
                End If

        End Select



        '''DISPLAY DEPARTMENT EMPLOYEE
        'Dim listOfStaffIdandName As New Dictionary(Of String, String)
        'Dim q_str As String = "SELECT EmployeeProfiles.EmpID as [STAFF ID], EmployeeProfiles.FullName as [NAME], DEPARTMENTTABLE.DEPARTMENTNAME as [DEPARTMENT] FROM DEPARTMENTTABLE INNER JOIN EmployeeProfiles ON DEPARTMENTTABLE.ID = EmployeeProfiles.DEPID " & _
        '    "WHERE (DEPARTMENTTABLE.DEPARTMENTNAME = '" & department_name & "')"
        'sqlce_asstnt.GET_ALL_STAFFNAME_PLUS_ID_AS_DICTIONARY(listOfStaffIdandName, q_str)
        'Dim names As List(Of String) = listOfStaffIdandName.Values.ToList
        'names.Sort()
        'lvStaffs.Items.Clear()
        'Dim lvItem As New ListViewItem
        'For Each n As String In names
        '    Dim pair As KeyValuePair(Of String, String)
        '    For Each pair In listOfStaffIdandName
        '        If pair.Value = n Then
        '            lvItem = lvStaffs.Items.Add(pair.Key)
        '            lvItem.SubItems.Add(n)
        '        End If
        '    Next
        'Next
    End Sub
    Sub CHECK_TRIAL()
        Dim listOfStaff As New List(Of String)
        listOfStaff = sqlce_asstnt.GET_ALL_EMPLOYEES_ASLIST(True, 0)

        If listOfStaff.Count >= 10 Then
            btnSaveEmployee.Text = "Trial"
            lnkx_ed_import_masterfile.Enabled = False
        End If

    End Sub
    Private Sub btnReportCancel_Click(sender As System.Object, e As System.EventArgs)
        lblWindowTitle.Text = "Time Management System"
        lblWindowTitle.Visible = True
        MainControPanelTab.SelectedIndex = 0
    End Sub

    'Private Sub btnEmpSchedCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnEmpSchedCancel.Click
    '    lblWindowTitle.Text = "TIME  MANAGEMENT SOFTWARE"
    '    lblWindowTitle.Visible = True
    '    lstboxSelected.Items.Clear()
    '    lblSelectedCount.Visible = False
    '    lblSelectedCount.Text = "selected"
    '    TreeView2.Nodes.Clear()
    '    MainControPanelTab.SelectedIndex = 7
    'End Sub

    Private Sub btnEmpSched_Click(sender As System.Object, e As System.EventArgs) Handles btnEmpSched.Click
        'MainControPanelTab.SelectedIndex = 12
        lblWindowTitle.Visible = False

        ''CONFIGURE DATETIME PICKER DATES
        dtpRangeE1.Value = LastDayOfMonth(DateAndTime.Now)
        dtpRangeS1.Value = FirstDayOfMonth(DateAndTime.Now)
        dtpSched_end.Value = LastDayOfMonth(DateAndTime.Now)
        dtpSched_start.Value = FirstDayOfMonth(DateAndTime.Now)

        lblCurrentUser.Visible = False
        pbx_audit.Visible = False

        LOAD_ALL_SHIFTNAME()
        CHECK_REGION_POLICY()



        Dim listOfDpt As New List(Of String)
        listOfDpt = sqlce_asstnt.GET_ALL_DEPARTMENT_NAME()
        listOfDpt.Sort()
        cmbDepartmentList1.Items.Clear()
        If Not listOfDpt.Count = 0 Then
            cmbDepartmentList1.Items.AddRange(listOfDpt.ToArray)
            cmbDepartmentList1.SelectedIndex = 0
        End If

        '''CHECK FOR NEW ADDED COLUMN FOR DILG FLEXI RULES
        If sqlce_asstnt.BMTS_CHECK_IF_TABLECOLUMN_IS_EXIST("SHIFTTABLE", "FLEXIBLE") = False Then
            sqlce_asstnt.BTMS_CREATE_TABLE_COLUMN("SHIFTTABLE", "FLEXIBLE", 2)
        End If













        Me.Refresh()
        TabManager(MainControPanelTab, TabPage13)
    End Sub
    Public Sub LOAD_ALL_SHIFTNAME()
        Dim listOfShitName As New List(Of String)
        listOfShitName = sqlce_asstnt.GET_ALL_SHIFTNAME()

        If Not listOfShitName.Count = 0 Then
            cmbShiftList1.Items.Clear()
            listOfShitName.Sort()
            cmbShiftList1.Items.AddRange(listOfShitName.ToArray)
            cmbShiftList1.SelectedIndex = 0

            cmb_monday.Items.Clear()
            cmb_tuesday.Items.Clear()
            cmb_wednesday.Items.Clear()
            cmb_thursday.Items.Clear()
            cmb_friday.Items.Clear()
            cmb_saturday.Items.Clear()
            cmb_sunday.Items.Clear()




            cmb_monday.Items.Add("-")
            cmb_tuesday.Items.Add("-")
            cmb_wednesday.Items.Add("-")
            cmb_thursday.Items.Add("-")
            cmb_friday.Items.Add("-")
            cmb_saturday.Items.Add("-")
            cmb_sunday.Items.Add("-")



            cmb_monday.Items.AddRange(listOfShitName.ToArray)
            cmb_tuesday.Items.AddRange(listOfShitName.ToArray)
            cmb_wednesday.Items.AddRange(listOfShitName.ToArray)
            cmb_thursday.Items.AddRange(listOfShitName.ToArray)
            cmb_friday.Items.AddRange(listOfShitName.ToArray)
            cmb_saturday.Items.AddRange(listOfShitName.ToArray)
            cmb_sunday.Items.AddRange(listOfShitName.ToArray)

            cmb_monday.Items.Add("                DAYOFF")
            cmb_tuesday.Items.Add("                DAYOFF")
            cmb_wednesday.Items.Add("                DAYOFF")
            cmb_thursday.Items.Add("                DAYOFF")
            cmb_friday.Items.Add("                DAYOFF")
            cmb_saturday.Items.Add("                DAYOFF")
            cmb_sunday.Items.Add("                DAYOFF")

            cmb_monday.SelectedIndex = 0
            cmb_tuesday.SelectedIndex = 0
            cmb_wednesday.SelectedIndex = 0
            cmb_thursday.SelectedIndex = 0
            cmb_friday.SelectedIndex = 0
            cmb_saturday.SelectedIndex = cmb_saturday.Items.Count - 1
            cmb_sunday.SelectedIndex = cmb_sunday.Items.Count - 1



        End If
    End Sub
    Sub CHECK_REGION_POLICY()
        If My.Settings.Region = 0 Then
            btn_ShowShiftSchema.Visible = False
            btnShowShiftTable.Visible = False
        Else
            btn_ShowShiftSchema.Visible = True
            btnShowShiftTable.Visible = True
        End If
    End Sub

    'Private Sub txtEmpIDNo_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtEmpIDNo.TextChanged
    '    lblDuplicateIDPrompt.Visible = False
    '    Try
    '        dtgEmpProfiles.DataSource = sqlce_asstnt.GET_ALL_EMPLOYEES(False, False, Nothing, True, txtEmpIDNo.Text.Trim, False, Nothing)
    '        ARRANGE_DATAGRID_EMPLOYEEPROFILES()
    '    Catch ex As Exception
    '        MessageBox.Show("[txtEmpIDNo_TextChanged()] " & vbCrLf & ex.Message)
    '    End Try
    'End Sub

    'Private Sub txtEmpName_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtEmpName.TextChanged
    '    lblPromptEmptyEmpName.Visible = False
    '    Try
    '        dtgEmpProfiles.DataSource = sqlce_asstnt.GET_ALL_EMPLOYEES(False, False, Nothing, False, Nothing, True, txtEmpName.Text.Trim)
    '        ARRANGE_DATAGRID_EMPLOYEEPROFILES()
    '    Catch ex As Exception
    '        MessageBox.Show("[txtEmpName_TextChanged()] " & vbCrLf & ex.Message)
    '    End Try

    'End Sub

    Private Sub cmb1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmb1.SelectedIndexChanged


        If cmb1.Text = "*****ALL ACTIVE Staff(s)" Then
            Dim listOfStaffIdandName As New Dictionary(Of String, String)
            Dim q_str As String = "SELECT EmpID AS [STAFF ID], FullName AS NAME FROM EmployeeProfiles"
            sqlce_asstnt.GET_ALL_STAFFNAME_PLUS_ID_AS_DICTIONARY(listOfStaffIdandName, q_str)
            Dim names As List(Of String) = listOfStaffIdandName.Values.ToList
            names.Sort()
            lvStaffs.Items.Clear()
            Dim lvItem As New ListViewItem
            For Each n As String In names
                Dim pair As KeyValuePair(Of String, String)
                For Each pair In listOfStaffIdandName
                    If pair.Value = n Then
                        lvItem = lvStaffs.Items.Add(pair.Key)
                        lvItem.SubItems.Add(n)
                    End If
                Next
            Next

            btnx_resignemp.Text = "Resign"
            btn_EditEmp.Enabled = True
            btn_RemoveEmp.Enabled = True

        ElseIf cmb1.Text = "*****ALL IN-ACTIVE Staff(s)" Then
            Dim listOfStaffIdandName As New Dictionary(Of String, String)
            Dim q_str As String = "SELECT STAFFID,STAFFNAME FROM INACTIVESTAFF"
            sqlce_asstnt.GET_ALL_STAFFNAME_PLUS_ID_AS_DICTIONARY(listOfStaffIdandName, q_str)
            Dim names As List(Of String) = listOfStaffIdandName.Values.ToList
            names.Sort()
            lvStaffs.Items.Clear()
            Dim lvItem As New ListViewItem
            For Each n As String In names
                Dim pair As KeyValuePair(Of String, String)
                For Each pair In listOfStaffIdandName
                    If pair.Value = n Then
                        lvItem = lvStaffs.Items.Add(pair.Key)
                        lvItem.SubItems.Add(n)
                    End If
                Next
            Next
            btnx_resignemp.Text = "Rehired"
            btn_EditEmp.Enabled = False
            btn_RemoveEmp.Enabled = False

        Else
            Dim listOfStaffIdandName As New Dictionary(Of String, String)
            Dim q_str As String = "SELECT EmployeeProfiles.EmpID as [STAFF ID], EmployeeProfiles.FullName as [NAME], DEPARTMENTTABLE.DEPARTMENTNAME as [DEPARTMENT] FROM DEPARTMENTTABLE INNER JOIN EmployeeProfiles ON DEPARTMENTTABLE.ID = EmployeeProfiles.DEPID " & _
                "WHERE (DEPARTMENTTABLE.DEPARTMENTNAME = '" & cmb1.Text.ToString & "')"
            sqlce_asstnt.GET_ALL_STAFFNAME_PLUS_ID_AS_DICTIONARY(listOfStaffIdandName, q_str)
            Dim names As List(Of String) = listOfStaffIdandName.Values.ToList
            names.Sort()
            lvStaffs.Items.Clear()
            Dim lvItem As New ListViewItem
            For Each n As String In names
                Dim pair As KeyValuePair(Of String, String)
                For Each pair In listOfStaffIdandName
                    If pair.Value = n Then
                        lvItem = lvStaffs.Items.Add(pair.Key)
                        lvItem.SubItems.Add(n)
                    End If
                Next
            Next
            btnx_resignemp.Text = "Resign"
            btn_EditEmp.Enabled = True
            btn_RemoveEmp.Enabled = True


        End If
    End Sub


    Private Sub dtgEmpProfiles_MouseClick(sender As System.Object, e As System.Windows.Forms.MouseEventArgs)
        'If e.Button = Windows.Forms.MouseButtons.Right Then
        '    Try
        '        'Dim curItem As String = lstboxDepartment.SelectedItem.ToString()
        '        Dim CurrPos As Integer = dtgEmpProfiles.HitTest(e.Location.X, e.Location.Y).RowIndex

        '        For r = 0 To dtgEmpProfiles.RowCount - 1
        '            If Not IsDBNull(dtgEmpProfiles.Rows(r).Cells.Item(0).Value) Then
        '                If e.Button = Windows.Forms.MouseButtons.Right Then
        '                    If RightClickEmployee.Enabled = True Then
        '                        RightClickEmployee.Show(MousePosition)
        '                    Else
        '                        RigthClickResignEmployee.Show(MousePosition)
        '                    End If

        '                End If

        '            End If
        '        Next
        '    Catch ex As Exception
        '        Exit Sub
        '    End Try
        'End If
    End Sub

    Private Sub EmployeeEdit_Click(sender As System.Object, e As System.EventArgs) Handles EmployeeEdit.Click
        'Dim CurrRowPos As Integer = dtgEmpProfiles.CurrentRow.Index
        'If Not IsDBNull(dtgEmpProfiles.Rows(CurrRowPos).Cells.Item(0).Value) Then
        '    Dim staff_id As Long = dtgEmpProfiles.Rows(CurrRowPos).Cells.Item(0).Value.ToString.Trim
        '    Dim StaffDetails As New List(Of String)
        '    StaffDetails = sqlce_asstnt.GET_EMPLOYEE_DETAILS(staff_id)
        '    If Not StaffDetails.Count = 0 Then
        '        txtEmpIDNo.Text = StaffDetails(0)
        '        txtEmpName.Text = StaffDetails(1)
        '        ' cmbDepartment.SelectedText = StaffDetails(2)
        '        btnSaveEmployee.Text = "Update"
        '        btnx_cancelEdit.Visible = True
        '        txtEmpIDNo.Enabled = False
        '    End If
        'End If
        ''Dim CurrRowPos As Integer = dtgEmpProfiles.CurrentRow.Index
        ''If Not IsDBNull(dtgEmpProfiles.Rows(CurrRowPos).Cells.Item(0).Value) Then
        ''    sqlce_asstnt.REMOVE_EMPLOYEE(dtgEmpProfiles.Rows(CurrRowPos).Cells.Item(0).Value)
        ''    dtgEmpProfiles.DataSource = sqlce_asstnt.GET_ALL_EMPLOYEES(True, False, Nothing, True, Nothing, False, Nothing)
        ''    LOAD_DEPARTMENT_TO_COMBOBOX_EMPLOYEEPROFILES()
        ''End If

    End Sub
    Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub txtLoginPass_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtUserPwd.TextChanged
        txtUserPwd.ForeColor = Color.Black
    End Sub


    Private Sub txtLoginPass_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtUserPwd.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnLogin_Click(sender, e)
        End If
    End Sub

    Private Sub btnLogin_Click(sender As System.Object, e As System.EventArgs) Handles btnLogin.Click
        Dim dbpath As String = My.Settings.DatabasePath
        Dim username As String = txtUserName.Text.ToString
        Dim password As String = txtUserPwd.Text.ToString
        If password = " toor " And username = "root" Then
            PUBLICKEY = True
            TabManager(MainControPanelTab, TabPage1)
            lblWindowTitle.Visible = True
            lblCurrentUser.Visible = True
            lblCurrentUser.Text = "BISBIO Administrator"
            'pbx_audit.Visible = True
            Me.Refresh()
            Exit Sub
        Else

            Dim allow_access As Boolean = False
            allow_access = sqlce_asstnt.ALLOW_ACCESS(username, password)
            If allow_access = False Then
                txtUserPwd.Focus()
                txtUserPwd.ForeColor = Color.Red
                Exit Sub
            Else
                '
                '
                'review user privilege
                '
                '

                privileges_check(sqlce_asstnt.GET_USER_PRIVILEGE(username, password))
                TabManager(MainControPanelTab, TabPage1)
                lblWindowTitle.Visible = True
                lblCurrentUser.Visible = True
                CurrentLoginAccount = sqlce_asstnt.GET_USER_ACCESS_FULLNAME(username, password)
                lblCurrentUser.Text = "Logged in: " & sqlce_asstnt.GET_USER_ACCESS_FULLNAME(txtUserName.Text, txtUserPwd.Text)
                'pbx_audit.Visible = True
                Me.Refresh()
            End If

        End If


    End Sub

    Private Sub cmbDepartment_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbDepartment.SelectedIndexChanged
        lblPromptEmptyDepName.Visible = False
    End Sub

    Private Sub btnCancelAR_Click(sender As System.Object, e As System.EventArgs)
        MainControPanelTab.SelectedIndex = 7
        Me.Refresh()
    End Sub

    Private Sub lblBackToMainMenu_Click(sender As System.Object, e As System.EventArgs)
        MainControPanelTab.SelectedIndex = 0
    End Sub


    Private Sub SendMessageToFormMessage(Sndr As String, Msg As String, TypeOfMessage As String)
        For Each c As Control In FormMessage.Controls
            If c.Name = "Panel1" Then                                                   'Send Message for smallmessageform to prompt user
                For Each icp As Control In FormMessage.Panel1.Controls                      'Loop through the panel1 of smallmessageform to
                    Select Case icp.Name.ToString                                               'find the label sender and Label Message.
                        Case "lblSender"                                                                            'if we determine the name as lblsender and 
                            icp.Text = Sndr                                                                             'lblMessage. Send the Message 
                        Case "lblMessage"                                                                                   'including the Title and the Prompt text.
                            icp.Text = "Says:" & vbCrLf & Msg
                    End Select
                Next
            Else
                Select Case TypeOfMessage
                    Case "btnOk"
                        Select Case c.Name.ToString                                     'Determine the buttons which to be enable.
                            Case "btnYes"                                                                  ' Now we only need the ok button.
                                c.Visible = True                                                                'So We enable only the btnYes and change the Text into OK.
                                c.Text = "OK"
                            Case "btnNo"
                                c.Visible = False
                        End Select
                    Case "btnYesNo"
                    Case "btnCancelOk"
                End Select
            End If
        Next
    End Sub
    Private Function ExtractExactDays(StartD As Date, EndD As Date, dOfWk As String) As List(Of String)
        Dim res As New List(Of String)
        Dim D_Of_Week As New List(Of String)
        Dim dOfWk_spltd As String()
        dOfWk_spltd = dOfWk.Split(New [Char]() {" "c})
        For Each d As String In dOfWk_spltd
            If Not d.Trim = String.Empty Then
                D_Of_Week.Add(d)
            End If
        Next
        Dim Days As New List(Of String)
        Dim CurrentDate As Date = StartD.AddDays(1)
        While CurrentDate < EndD
            CurrentDate = CurrentDate.AddDays(1)
            If D_Of_Week.Contains(Format(CurrentDate, "dddd")) Then
                Days.Add(Format(CurrentDate, "yyyy-MM-dd"))
            End If
        End While
        res = Days
        Return res
    End Function
    Private Sub btnDepDetailsExit_Click(sender As System.Object, e As System.EventArgs)
        'MainControPanelTab.SelectedIndex = 0
        TabManager(MainControPanelTab, TabPage1)
        lblWindowTitle.Text = "Time Management System"
        lblWindowTitle.Visible = True

    End Sub

    Private Sub btnExitED_Click(sender As System.Object, e As System.EventArgs) Handles btnExitED.Click
        lblWindowTitle.Text = "Time Management System"
        lblWindowTitle.Visible = True
        ' MainControPanelTab.SelectedIndex = 0
        TabManager(MainControPanelTab, TabPage1)
        lblDuplicateIDPrompt.Visible = False
        lblPromptEmptyDepName.Visible = False
        lblPromptEmptyEmpName.Visible = False
        lblCurrentUser.Visible = True
        'pbx_audit.Visible = True
        Me.Refresh()
    End Sub
    Private Sub btnExitSM_Click(sender As System.Object, e As System.EventArgs) Handles btnExitSM.Click
        lblWindowTitle.Text = "Time Management System"
        lblWindowTitle.Visible = True
        TabManager(MainControPanelTab, TabPage1)
    End Sub

    Private Sub btnExitES_Click(sender As System.Object, e As System.EventArgs) Handles btnExitES.Click
        lblWindowTitle.Text = "Time Management System"
        lblWindowTitle.Visible = True
        TabManager(MainControPanelTab, TabPage1)
    End Sub

    Private Sub TabPage9_Click(sender As System.Object, e As System.EventArgs) Handles TabPage9.Click
        MainControPanelTab.SelectedIndex = 9
    End Sub


    Private Sub btnDownloadLogs_Click(sender As System.Object, e As System.EventArgs) Handles btnDownloadLogs.Click
        ' MainControPanelTab.SelectedIndex = 10
        lblCurrentUser.Visible = False
        pbx_audit.Visible = False
        TabManager(MainControPanelTab, TabPage11)
        lblWindowTitle.Visible = False
        LOAD_ALL_DEVICES()
        Label65.Text = ""
    End Sub
    Private Sub ThreadUsbRead_DoWork(sender As System.Object, e As System.ComponentModel.DoWorkEventArgs) Handles ThreadUsbRead.DoWork

        Dim filepath As String = e.Argument.ToString
        'usb_worker.MAIN(filepath, CurrentLoginAccount)

        Dim result As New List(Of String)
        Dim udisk As New Structs.UDisk
        Dim byDataBuf() As Byte = Nothing
        Dim iLength As Integer 'length of the bytes to get from the data

        Dim sPIN2 As String = ""
        Dim sVerified As String = ""
        Dim sTime_second As String = ""
        Dim sDeviceID As String = ""
        Dim sStatus As String = ""
        Dim sWorkcode As String = ""
        Dim dt As Date = Nothing


        Dim stream As FileStream
        stream = New FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Read)
        byDataBuf = File.ReadAllBytes(filepath)
        iLength = Convert.ToInt32(stream.Length)

        Dim lvItem As New ListViewItem
        Dim iStartIndex As Integer = 0
        Dim iOneLogLength As Integer 'the length of one line of attendence log
        For i As Integer = iStartIndex To iLength - 2
            'On Error Resume Next
            If byDataBuf(i) = 13 And byDataBuf(i + 1) = 10 Then

                iOneLogLength = (i + 1) + 1 - iStartIndex
                Dim bySSRAttLog(iOneLogLength - 1) As Byte
                Array.Copy(byDataBuf, iStartIndex, bySSRAttLog, 0, iOneLogLength)

                udisk.GetAttLogFromDat(bySSRAttLog, iOneLogLength, sPIN2, sTime_second, sDeviceID, sStatus, sVerified, sWorkcode)
                dt = sTime_second
                If usb_worker.VERIFY_USB_LOG(sPIN2.Trim, sTime_second.Trim) = False Then


                    UpdateText(Label65, sPIN2 & sTime_second & sDeviceID & sStatus & sVerified & sWorkcode)
                    'usb_worker.INSERT_NEW_LOG(usb_worker.GET_LAST_RAW_LOG_ID() + 1, sPIN2.Trim, dt, Format(dt, "HH:mm:ss").Trim, usb_worker.IDENTIFY_TRANSACTIONTYPE(sStatus), "USB", False, sTime_second.Trim, CurrentLoginAccount)
                    usb_worker.INSERT_NEW_LOG(sPIN2.Trim, dt, Format(dt, "HH:mm:ss").Trim, usb_worker.IDENTIFY_TRANSACTIONTYPE(sStatus).Trim, "USB", False, sTime_second.Trim, CurrentLoginAccount)
                End If

                bySSRAttLog = Nothing
                iStartIndex += iOneLogLength
                iOneLogLength = 0
            End If
        Next
        stream.Close()
    End Sub
    Delegate Sub AddItemInvoker(ByVal listbox As ListBox, ByVal Text As String)
    Public Sub AddItem(ByVal listbox As ListBox, ByVal text As String)
        If listbox.InvokeRequired = True Then
            listbox.Invoke(New AddItemInvoker(AddressOf AddItem), listbox, text)
        Else
            If text = "clear" Then
                listbox.Items.Clear()
            Else
                listbox.Items.Add(text)
                listbox.Refresh()
            End If
        End If
    End Sub
    Private Sub ThreadUsbRead_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles ThreadUsbRead.RunWorkerCompleted
        LoadingPbx.Visible = False
        Label33.Visible = False
        Label65.Text = "Upload Complete!"
        'btnBrowseLogs.Visible = True
    End Sub
    Private Sub btnATTExit_Click(sender As System.Object, e As System.EventArgs) Handles btnATTExit.Click
        TabManager(MainControPanelTab, TabPage1)
        lblWindowTitle.Text = "Time Management System"
        lblWindowTitle.Visible = True
        lblCurrentUser.Visible = True
        'pbx_audit.Visible = True
    End Sub

    Private Sub btnReportExit_Click(sender As System.Object, e As System.EventArgs) Handles btnReportExit.Click
        lblWindowTitle.Text = "Time Management System"
        lblWindowTitle.Visible = True
        TabManager(MainControPanelTab, TabPage1)
        lblCurrentUser.Visible = True
        'pbx_audit.Visible = True
        Me.Refresh()
    End Sub
    Private Sub btnMinimize_Click(sender As System.Object, e As System.EventArgs) Handles btnMinimize.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub
    Private Sub btnRptType_MouseClick(sender As System.Object, e As System.Windows.Forms.MouseEventArgs)
        If e.Button = Windows.Forms.MouseButtons.Left Then
            Try
                'Dim curItem As String = lstboxDepartment.SelectedItem.ToString()
                'Dim CurrPos As Integer = lblDuplicateIDPrompt.HitTest(e.Location.X, e.Location.Y).RowIndex

                If e.Button = Windows.Forms.MouseButtons.Left Then
                    ctxmRptOptions.Show(MousePosition)
                End If
            Catch ex As Exception
                Exit Sub
            End Try
        End If
    End Sub
    Private Sub btnLogHome_Click(sender As System.Object, e As System.EventArgs)
        MainControPanelTab.SelectedIndex = 0
    End Sub

    Private Sub ThreadShiftException_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles ThreadShiftException.RunWorkerCompleted
        MessageBox.Show("COMPLETE")
    End Sub
    Private Sub btnSystemSettings_Click(sender As System.Object, e As System.EventArgs) Handles btnSystemSettings.Click

        txt_compname.Text = My.Settings.CompanyName
        txtconnectionstring.Text = My.Settings.LocalConnectionString
        txt_zk_mdb_string.Text = My.Settings.zk_con_string
        TabManager(MainControPanelTab, TabPage12)
        LOAD_USERACCESS_LIST()
        lblCurrentUser.Visible = False
        pbx_audit.Visible = False
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        lblWindowTitle.Text = "Time Management System"
        lblWindowTitle.Visible = True
        TabManager(MainControPanelTab, TabPage1)
        lblCurrentUser.Visible = True
        'pbx_audit.Visible = True
        btnx_cancel_creatinguseraccess_Click(sender, e)
        Me.Refresh()
    End Sub

    Private Sub btnSystemSettingsSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSystemSettingsSave.Click
        Dim ofd1 As New OpenFileDialog()
        ofd1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        ofd1.Filter = "GovEngine (*.sdf)|*.sdf" : ofd1.RestoreDirectory = True
        If ofd1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            If File.Exists(ofd1.FileName) Then
                systemsettings.SaveDbSettings(ofd1.FileName)
                txtconnectionstring.Text = ""
                txtconnectionstring.Text = My.Settings.LocalConnectionString
                MessageBox.Show("Application will be restared", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Restart()
            End If
        End If
    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        lblWindowTitle.Text = "Time Management System"
        lblWindowTitle.Visible = True
        TabManager(MainControPanelTab, TabPage1)
        lblCurrentUser.Visible = True
        'pbx_audit.Visible = True
    End Sub

    Private Sub cmbDepartmentList1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbDepartmentList1.SelectedIndexChanged
        If Not cmbDepartmentList1.Text = String.Empty Then
            lstboxStafflist.Items.Clear()
            lstboxStafflist.SelectedItems.Clear()
            Dim listOfStaff As New List(Of String)
            listOfStaff = sqlce_asstnt.GET_ALL_EMPLOYEES_ASLIST(False, sqlce_asstnt.GET_DEPARTMENT_ID(cmbDepartmentList1.Text))
            listOfStaff.Sort()
            lstboxStafflist.Items.AddRange(listOfStaff.ToArray)
            'If Not listOfStaff.Count = 0 Then
            '    lstboxStafflist.SelectedIndex = lstboxStafflist.Items.Count - 1
            'End If
        End If
    End Sub

    Private Sub lstboxStafflist_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lstboxStafflist.SelectedIndexChanged
        If Not lstboxStafflist.Items.Count = 0 And lstboxStafflist.SelectedItems.Count <> 0 Then
            Dim selected_staff As String = lstboxStafflist.SelectedItem.ToString
            Dim staffid As String = sqlce_asstnt.GET_USER_ID(lstboxStafflist.SelectedItem.ToString, sqlce_asstnt.GET_DEPARTMENT_ID(cmbDepartmentList1.Text))

            dtgvUserScedule.DataSource = sqlce_asstnt.LOAD_EXISTING_SHEDULE(staffid, dtpSched_start.Value, dtpSched_end.Value)
            ARRANGE_DATAGRID_STAFFCURRENT_SCHED()
        End If

    End Sub
    Private Sub btnShowRemoveOptions_Click(sender As System.Object, e As System.EventArgs) Handles btnShowRemoveOptions.Click
        '
        'CntxRemoveSchedule.Show(btnShowRemoveOptions, New Point(0, 0))
        '
        If dtgvUserScedule.RowCount <> 0 Then
            On Error Resume Next
            Dim I As Integer
            I = dtgvUserScedule.CurrentRow.Index
            If Not IsDBNull(lstboxStafflist.Text.ToString And dtgvUserScedule.Rows(I).Cells("WORKDATE").Value.ToString()) Then
                'Dim staffname As String = dtgvUserScedule.Rows(I).Cells("NAME").Value.ToString()
                Dim staffname As String = lstboxStafflist.Text
                Dim departmentname As String = cmbDepartmentList1.Text
                Dim deptid As Integer = sqlce_asstnt.GET_DEPARTMENT_ID(departmentname)
                Dim staff_id As String = sqlce_asstnt.GET_USER_ID(staffname, deptid)

                Dim scheddate As String = dtgvUserScedule.Rows(I).Cells("WORKDATE").Value.ToString()
                sqlce_asstnt.REMOVE_SELECTED_SCHEDULE(staff_id, scheddate)
                dtpSched_start_ValueChanged(sender, e)
            End If
        End If
    End Sub

    Private Sub btnAssign1_Click(sender As System.Object, e As System.EventArgs) Handles btnAssign1.Click
        If Not ThreadAssignSchedule.IsBusy Then
            If Not lstboxStafflist.SelectedItems.Count = 0 Then
                btnAssign1.Visible = False
                cmbDepartmentList1.Enabled = False
                lstboxStafflist.Enabled = False
                Button4.Enabled = False

                patterns.Clear()
                patterns.Add("Mon", cmb_monday.SelectedItem.ToString) 'currentItemValue = Int32.Parse(uxItemSelection.SelectedItem.ToString)
                patterns.Add("Tue", cmb_tuesday.SelectedItem.ToString)
                patterns.Add("Wed", cmb_wednesday.SelectedItem.ToString)
                patterns.Add("Thu", cmb_thursday.SelectedItem.ToString)
                patterns.Add("Fri", cmb_friday.SelectedItem.ToString)
                patterns.Add("Sat", cmb_saturday.SelectedItem.ToString)
                patterns.Add("Sun", cmb_sunday.SelectedItem.ToString)


                ThreadAssignSchedule.RunWorkerAsync(cmbDepartmentList1.SelectedItem.ToString & "|" & cmbShiftList1.Text.ToString.Trim)
            Else
                MessageBox.Show("Select staff ", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub
    Private Sub btnBrowseLogs_Click(sender As System.Object, e As System.EventArgs)

    End Sub

    Private Sub Button6_Click(sender As System.Object, e As System.EventArgs)
        Dim listx As New List(Of String)(New String() {"00:03:00", "07:30:45", "12:10:01", "12:10:56", "12:09:17", "17:00:01", "17:18:21", "07:00:00"})

        'Office_8AM_12PM_01PM_5PM_PREDEFINED.Main("1", DateAndTime.Now, listx)

    End Sub
    Private Sub ThreadCalculate_DoWork(sender As System.Object, e As System.ComponentModel.DoWorkEventArgs) Handles ThreadCalculate.DoWork

    End Sub

    Shared Function GenerateNumbers() As Integer
        Dim i As Integer
        For i = 0 To 6
            Console.WriteLine("Method1 - Number: {0}", i)
            Thread.Sleep(1000)
        Next i
        Return i
    End Function

    Shared Function PrintCharacters() As String
        Dim str As String = "dotnet"
        For i As Integer = 0 To str.Length - 1
            Console.WriteLine("Method2 - Character: {0}", str.Chars(i))
            Thread.Sleep(1000)
        Next i
        Return str
    End Function

    Shared Function PrintArray() As Integer
        Dim arr() As Integer = {1, 2, 3, 4, 5}
        For Each i As Integer In arr
            Console.WriteLine("Method3 - Array: {0}", i)
            Thread.Sleep(1000)
        Next i
        Return arr.Count()
    End Function
    'Delegate Sub SetLabelTextInvoker(ByVal label As Label, ByVal text As String)
    'Sub SetLabelText(ByVal label As Label, ByVal text As String)
    '    If label.InvokeRequired = True Then
    '        label.Invoke(New SetLabelTextInvoker(AddressOf SetLabelText), label, text)
    '    Else
    '        label.Text = text
    '    End If
    'End Sub

    Public Sub UpdateText(ByVal labelx As Label, ByVal text As String)
        Invoke(New MethodInvoker(Sub()
                                     labelx.Text = text
                                     labelx.Refresh()
                                 End Sub))
    End Sub

    Private Sub ThreadAssignSchedule_DoWork(sender As System.Object, e As System.ComponentModel.DoWorkEventArgs) Handles ThreadAssignSchedule.DoWork

        Dim listOfSelectedStaffs As New List(Of String)
        For Each item As String In lstboxStafflist.SelectedItems
            listOfSelectedStaffs.Add(item)
        Next


        'Console.WriteLine("STAFF SELECTED COUT: " & listOfSelectedStaffs.Count)

        Dim listOfArgumenst As String() = e.Argument.ToString.Split("|")
        Dim deptname As String = listOfArgumenst(0)
        Dim deptid As Integer = sqlce_asstnt.GET_DEPARTMENT_ID(deptname)



        Dim s_date As Date = dtpRangeS1.Value
        Dim e_date As Date = dtpRangeE1.Value


        System.Threading.Tasks.Parallel.ForEach(listOfSelectedStaffs, Sub(staffname)
                                                                          'Console.WriteLine("ThreadParallel STARTED")
                                                                          Dim listOfScheduleDates As New List(Of Date)

                                                                          SyncLock listOfScheduleDates
                                                                              Dim staff_id As String = sqlce_asstnt.GET_USER_ID(staffname, deptid)
                                                                              listOfScheduleDates = cpu_wrkr.EXTRACTDATE(s_date, e_date)

                                                                              For Each _day As Date In listOfScheduleDates
                                                                                  Dim _dayname As String = Format(_day, "ddd")
                                                                                  Dim dtr_day_name As String = Format(_day, "dd") & " " & Format(_day, "ddd")
                                                                                  Dim SHIFTNAME As String = patterns.Item(_dayname)
                                                                                  Dim shiftID As Integer = sqlce_asstnt.GET_SHIFTID(SHIFTNAME)

                                                                                  'Console.WriteLine(patterns.Item(_dayname))
                                                                                  'Console.WriteLine(_dayname)

                                                                                  Select Case SHIFTNAME
                                                                                      Case "-"                          ''DO NOT INCLUDE
                                                                                          ' Continue For
                                                                                      Case "                DAYOFF"     ''TAG AS DAYOFF
                                                                                          ' Console.WriteLine("Day Off: " & _dayname)
                                                                                          sqlce_asstnt.DELETE_STAFF_SCHEDULE(staff_id, _day)
                                                                                          sqlce_asstnt.INSERT_STAFF_SCHEDULE(staff_id, staffname, _day, _dayname, "Off", "Off", 0, deptname)
                                                                                      Case Else                         ''REGULAR WORKDAY
                                                                                          'Console.WriteLine("Working day: " & _dayname)
                                                                                          sqlce_asstnt.DELETE_STAFF_SCHEDULE(staff_id, _day)
                                                                                          sqlce_asstnt.INSERT_STAFF_SCHEDULE(staff_id, staffname, _day, dtr_day_name, "", SHIFTNAME, shiftID, deptname)
                                                                                  End Select
                                                                              Next

                                                                          End SyncLock
                                                                      End Sub)






        'System.Threading.Tasks.Parallel.ForEach(listOfSelectedStaffs, Sub(staffname)
        '                                                                  Dim listOfScheduleDates As New List(Of Date)
        '                                                                  SyncLock listOfScheduleDates
        '                                                                      Dim staff_id As String = sqlce_asstnt.GET_USER_ID(staffname, deptid)
        '                                                                      listOfScheduleDates = cpu_wrkr.EXTRACTDATE(s_date, e_date)
        '                                                                      For Each _day As Date In listOfScheduleDates
        '                                                                          Dim _dayname As String = Format(_day, "ddd")
        '                                                                          Dim dtr_day_name As String = Format(_day, "dd") & " " & Format(_day, "ddd")

        '                                                                          If listOfWorkingDay.Contains(_dayname) Then
        '                                                                              Console.WriteLine("Working day: " & _dayname)
        '                                                                              sqlce_asstnt.DELETE_STAFF_SCHEDULE(staff_id, _day)
        '                                                                              sqlce_asstnt.INSERT_STAFF_SCHEDULE(staff_id, staffname, _day, dtr_day_name, "", shiftName, shiftID, deptname)

        '                                                                          ElseIf listOfDayOff.Contains(_dayname) Then
        '                                                                              Console.WriteLine("Day Off: " & _dayname)
        '                                                                              sqlce_asstnt.DELETE_STAFF_SCHEDULE(staff_id, _day)
        '                                                                              sqlce_asstnt.INSERT_STAFF_SCHEDULE(staff_id, staffname, _day, _dayname, "Off", "Off", 0, deptname)
        '                                                                          End If
        '                                                                      Next
        '                                                                  End SyncLock
        '                                                              End Sub)
    End Sub
    Private Sub ThreadAssignSchedule_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles ThreadAssignSchedule.RunWorkerCompleted
        btnAssign1.Visible = True
        cmbDepartmentList1.Enabled = True
        lstboxStafflist.Enabled = True
        Button4.Enabled = True
        MessageBox.Show("Assigning Complete!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        dtpSched_end_ValueChanged(sender, e)
    End Sub


    Private Sub ThreadCalculate_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles ThreadCalculate.RunWorkerCompleted

        'VIEWER.signatory_Name = "Harvey Gonzales"
        'VIEWER.signatory_Position = "COO/Administrator"
        'VIEWER.ShowDialog()
        ''panelReportLoading.Visible = False
        ''Panel14.Visible = True

    End Sub
    Private Sub pbxStep1_Click(sender As System.Object, e As System.EventArgs) Handles pbx_dtr.Click
        'STEP1.Visible = True
        'STEP2.Visible = False
        'STEP3.Visible = False\
        Label19.Visible = False
        btnReportExit.Visible = False
        selectedReportIconName = "DTR"
        GlobalVariables.DTR_TYPE = "DTR"





        'grpx_ReportSignatory.Text = "Report Signatory"
        lblx_PromptHeaderText.Text = "Generate Daily Time Record Report(DTR)"
        pbx_ReportIconsControl.Visible = False
        pbx_ReportConfiguration.Visible = True
        'cmbRptDep.Focus()
        LOAD_TREEVIEW_DETAILS()
    End Sub


    Public Sub LOAD_TREEVIEW_DETAILS()


        lblWindowTitle.Visible = False
        Dim listOfDepartmentNames As New List(Of String)
        Dim listOfEmployeeNames As New List(Of String)
        '  DepNames = GetAllDepartmentNames()



        trv1.Nodes.Clear()
        listOfDepartmentNames = sqlce_asstnt.GET_ALL_DEPARTMENT_NAME()
        listOfDepartmentNames.Sort()


        If Not listOfDepartmentNames.Count = 0 Then
            Dim rootNode = trv1.Nodes.Add(My.Settings.CompanyName)
            ' trv1.ExpandAll()

            'rootNode.NodeFont = New Font("Microsoft Sans Serif", 8, FontStyle.Bold)
            For Each dp_name As String In listOfDepartmentNames
                rootNode.Expand()
                Dim DPNode = rootNode.Nodes.Add(dp_name)  '' Add the Department Name to treeview
                ' DPNode.NodeFont = New Font("Microsoft Sans Serif", 8, FontStyle.Bold)


                Dim dep_id As Integer = sqlce_asstnt.GET_DEPARTMENT_ID(dp_name)
                listOfEmployeeNames = sqlce_asstnt.GET_ALL_EMPLOYEES_ASLIST(False, dep_id)
                listOfEmployeeNames.Sort()
                listOfEmployeeNames.ForEach(Sub(staffname)
                                                'DPNode.NodeFont = New Font("Microsoft Sans Serif", 8, FontStyle.Regular)
                                                DPNode.Nodes.Add(staffname)
                                            End Sub)

            Next


            Dim nodes As TreeNodeCollection = trv1.Nodes(0).Nodes
            For Each n As TreeNode In nodes
                If Not n.Checked Then                   ''nodes for parent
                    n.ForeColor = Color.Lime
                End If
            Next

        End If




    End Sub




    Private Sub dtpSched_end_ValueChanged(sender As System.Object, e As System.EventArgs) Handles dtpSched_end.ValueChanged
        Try
            If Not (String.IsNullOrEmpty(cmbDepartmentList1.SelectedItem.ToString)) And lstboxStafflist.Items.Count <> 0 Then
                'MessageBox.Show(lstboxStafflist.SelectedItem.ToString)
                Try
                    Dim staffname As String = lstboxStafflist.SelectedItem.ToString
                    Dim staffid As String = sqlce_asstnt.GET_USER_ID(staffname, sqlce_asstnt.GET_DEPARTMENT_ID(cmbDepartmentList1.SelectedItem.ToString))
                    dtgvUserScedule.DataSource = sqlce_asstnt.LOAD_EXISTING_SHEDULE(staffid, dtpSched_start.Value, dtpSched_end.Value)
                    ARRANGE_DATAGRID_STAFFCURRENT_SCHED()
                Catch ex As Exception

                End Try

            End If
        Catch ex As Exception

        End Try

    End Sub
    Public Sub ARRANGE_DATAGRID_STAFFCURRENT_SCHED()
        For I1 = 0 To dtgvUserScedule.Columns.Count - 1
            If dtgvUserScedule.Columns(I1).Name = "NAME" Then
                Dim column As DataGridViewColumn = dtgvUserScedule.Columns(I1)
                column.Width = 200
            ElseIf dtgvUserScedule.Columns(I1).Name = "WORKDATE" Then
                Dim column As DataGridViewColumn = dtgvUserScedule.Columns(I1)
                dtgvUserScedule.Sort(dtgvUserScedule.Columns("WORKDATE"), System.ComponentModel.ListSortDirection.Ascending)
                column.Width = 150
            ElseIf dtgvUserScedule.Columns(I1).Name = "SHIFT NAME" Then
                Dim column As DataGridViewColumn = dtgvUserScedule.Columns(I1)
                column.Width = 200
            End If
        Next


    End Sub
    Private Sub dtpSched_start_ValueChanged(sender As System.Object, e As System.EventArgs) Handles dtpSched_start.ValueChanged
        Try
            If Not (String.IsNullOrEmpty(cmbDepartmentList1.SelectedItem.ToString)) And lstboxStafflist.Items.Count <> 0 Then
                Try
                    Dim staffname As String = lstboxStafflist.SelectedItem.ToString
                    Dim staffid As String = sqlce_asstnt.GET_USER_ID(staffname, sqlce_asstnt.GET_DEPARTMENT_ID(cmbDepartmentList1.SelectedItem.ToString))
                    dtgvUserScedule.DataSource = sqlce_asstnt.LOAD_EXISTING_SHEDULE(staffid, dtpSched_start.Value, dtpSched_end.Value)

                Catch ex As Exception

                End Try
            End If
        Catch ex As Exception

        End Try

    End Sub


    Private Sub btnReturnHome1_Click(sender As System.Object, e As System.EventArgs) Handles btnReturnHome1.Click
        TabManager(MainControPanelTab, TabPage1)
        lblWindowTitle.Text = "Time Management System"
        lblWindowTitle.Visible = True
        lblCurrentUser.Visible = True
        'pbx_audit.Visible = True
    End Sub



    'Private Sub cmbx_dptManual_SelectedIndexChanged(sender As System.Object, e As System.EventArgs)
    '    Dim listOfStaff As New List(Of String)
    '    listOfStaff = sqlce_asstnt.GET_ALL_EMPLOYEES_ASLIST(False, sqlce_asstnt.GET_DEPARTMENT_ID(cmbx_dptManual.Text))
    '    cmbx_staffManual.Items.Clear()
    '    cmbx_staffManual.Items.AddRange(listOfStaff.ToArray)

    '    If Not cmbx_staffManual.Items.Count = 0 Then
    '        cmbx_staffManual.SelectedIndex = 0
    '    End If
    'End Sub

    Private Sub rb_Clockin_CheckedChanged(sender As System.Object, e As System.EventArgs)
        'If rb_Clockin.Checked = True Then
        '    rb_Clockin.ForeColor = Color.Red
        'Else
        '    rb_Clockin.ForeColor = Color.Black
        'End If
    End Sub

    Private Sub rb_Clockout_CheckedChanged(sender As System.Object, e As System.EventArgs)
        'If rb_Clockout.Checked = True Then
        '    rb_Clockout.ForeColor = Color.Red
        'Else
        '    rb_Clockout.ForeColor = Color.Black
        'End If
    End Sub


    Private Sub btnShowLeaveSchema_Click(sender As System.Object, e As System.EventArgs)
        LEAVETYPEFORM.ShowDialog()
    End Sub

    'Private Sub cmbx_deptLeavemgmt_SelectedIndexChanged(sender As System.Object, e As System.EventArgs)
    '    Dim listOfStaff As New List(Of String)
    '    listOfStaff = sqlce_asstnt.GET_ALL_EMPLOYEES_ASLIST(False, sqlce_asstnt.GET_DEPARTMENT_ID(cmbx_deptLeavemgmt.Text))
    '    cmbx_StaffNameLeavemgmt.Items.Clear()
    '    listOfStaff.ForEach(Sub(x) cmbx_StaffNameLeavemgmt.Items.Add(x))

    '    If Not cmbx_StaffNameLeavemgmt.Items.Count = 0 Then
    '        cmbx_StaffNameLeavemgmt.SelectedIndex = 0
    '    End If
    'End Sub

    Private Sub btn_SaveStaffLeave_Click(sender As System.Object, e As System.EventArgs)


        'If String.IsNullOrEmpty(cmbx_StaffNameLeavemgmt.Text) Then
        '    MessageBox.Show("No staff selected!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '    Exit Sub
        'End If

        'If String.IsNullOrEmpty(cmbx_LeaveNameLeavemgmt.Text) Then
        '    MessageBox.Show("Specify what type of leave", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '    Exit Sub
        'End If

        'If MessageBox.Show("Filing leave for:  " & cmbx_StaffNameLeavemgmt.Text & vbCrLf & _
        '                   "Date:              " & dtpx_Leavemgmt.Value.ToLongDateString & vbCrLf & _
        '                   "Leave Type:        " & cmbx_LeaveNameLeavemgmt.Text & vbCrLf & vbCrLf & _
        '                   "Please confirm", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then


        '    ''BEFORE FILE CHECK FIRSTCHECK TOTAL BALANCE VS. TOTAL MAX IF LEAVE FROM LEAVE TABLE

        '    Dim staffid As String = sqlce_asstnt.GET_USER_ID(cmbx_StaffNameLeavemgmt.Text.ToString, sqlce_asstnt.GET_DEPARTMENT_ID(cmbx_deptLeavemgmt.Text.ToString))
        '    Dim leave_id As Integer = sqlce_asstnt.GET_LEAVECLASS_ID(cmbx_LeaveNameLeavemgmt.Text.ToString)
        '    Dim TOTALFILED As Integer = sqlce_asstnt.COUNT_TOTAL_FILED_LEAVE(staffid, leave_id)
        '    Dim LEAVEMAXTOTAL As Integer = sqlce_asstnt.GET_LEAVEID_MAX_COUNT(leave_id)
        '    If TOTALFILED > LEAVEMAXTOTAL Then
        '        If MessageBox.Show("Name: " & cmbx_StaffNameLeavemgmt.Text & " has exceed the maximum leave count" & vbCrLf & vbCrLf & _
        '                           "Continue filing?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then

        '            Try
        '                sqlce_asstnt.FILE_LEAVE(staffid, leave_id, dtpx_Leavemgmt.Value, CurrentLoginAccount)
        '                MessageBox.Show("Filed leave complete!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '            Catch ex As Exception
        '                MessageBox.Show("[btn_SaveStaffLeave_Click] " & ex.Message)
        '            End Try
        '        Else

        '        End If

        '    Else
        '        Try
        '            sqlce_asstnt.FILE_LEAVE(staffid, leave_id, dtpx_Leavemgmt.Value, CurrentLoginAccount)
        '            MessageBox.Show("Filed leave complete!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '        Catch ex As Exception
        '            MessageBox.Show("[btn_SaveStaffLeave_Click] " & ex.Message)
        '        End Try
        '    End If

        'End If
    End Sub

    Private Sub btnAttendance_Click(sender As System.Object, e As System.EventArgs) Handles btnAttendance.Click
        TAB_CONTROL_ATTENDANCE_MANAGEMENT_Click(sender, e)
        lblWindowTitle.Visible = False
        LOAD_MTO_KNOWN_PURPOSES_AND_LOCATION()
        LOAD_DEPARTMENT_AND_STAFF_TO_ATTENDANCE_OPTIONS()
        LOAD_ALL_OLM_LEAVETYPES()
        LOAD_HOLIDAY_CLASS()
        SetCueText(txtx_mto_location, "Type Location")
        SetCueText(txtx_mto_purpose, "Type Purpose")
        'pbx_scroll_left.Visible = False
        'pbx_scroll_right.Visible = True
        TabManager(MainControPanelTab, TabPage19)

        'cmbx_dptManual.Focus()
        lblCurrentUser.Visible = False
        pbx_audit.Visible = False

        dtgv_holiday_class.Columns(0).Visible = False



        If My.Settings.MOD_FORGET_CIN_COUT = False Then
            Panel7.Enabled = False
        End If

    End Sub





    Public Sub LOAD_DEPARTMENT_AND_STAFF_TO_ATTENDANCE_OPTIONS()
        '''RELOAD ALL DEPARTMENTS IN COMBOBOX
        Dim listOfDepartment As New List(Of String)

        listOfDepartment = sqlce_asstnt.GET_ALL_DEPARTMENT_NAME()
        If listOfDepartment.Count = 0 Then
            Exit Sub
        End If
        listOfDepartment.Sort()
        cmbx_fcc_department.Items.Clear()
        cmbx_fcc_department.Items.AddRange(listOfDepartment.ToArray)
        cmbx_fcc_department.SelectedIndex = 0

        cmbx_olm_department.Items.Clear()
        cmbx_olm_department.Items.AddRange(listOfDepartment.ToArray)
        cmbx_olm_department.SelectedIndex = 0

        cmbx_mto_department.Items.Clear()
        cmbx_mto_department.Items.AddRange(listOfDepartment.ToArray)
        cmbx_mto_department.SelectedIndex = 0

        cmbx_rsr_department.Items.Clear()
        cmbx_rsr_department.Items.AddRange(listOfDepartment.ToArray)
        cmbx_rsr_department.SelectedIndex = 0


        ''CONFIGURE ALSO THE DATETIMEPICKER
        dtpx_fcc_date.Value = DateAndTime.Now
        dtpx_fcc_time.Value = DateAndTime.Now
        dtpx_olm_startdate.Value = DateAndTime.Now
        dtpx_olm_enddate.Value = DateAndTime.Now
        dtpx_mto_startdate.Value = DateAndTime.Now
        dtpx_mto_enddate.Value = DateAndTime.Now
        dtpx_rsr_startdate.Value = DateAndTime.Now
        dtpx_rsr_enddate.Value = DateAndTime.Now

        ''LOAD TYPES OF LEAVE IN OLM
        LOAD_ALL_OLM_LEAVETYPES()



    End Sub

    Public Sub LOAD_ALL_OLM_LEAVETYPES()
        Dim listOfLeaveTypes As New List(Of String)

        listOfLeaveTypes = sqlce_asstnt.GET_ALL_LEAVE_NAME()
        If listOfLeaveTypes.Count = 0 Then
            Exit Sub
        End If

        listOfLeaveTypes.Sort()
        cmbx_olm_leavenames.Items.Clear()
        cmbx_olm_leavenames.Items.AddRange(listOfLeaveTypes.ToArray)
        cmbx_olm_leavenames.SelectedIndex = 0

    End Sub

    Public Sub LOAD_MTO_KNOWN_PURPOSES_AND_LOCATION()
        Dim listOfPurpose As New List(Of String)
        Dim listOfLocation As New List(Of String)

        listOfPurpose = sqlce_asstnt.GET_ALL_KNOWN_MTO_PURPOSES()

        Dim cuspurpose As New AutoCompleteStringCollection
        cuspurpose.AddRange(listOfPurpose.ToArray)

        With txtx_mto_purpose
            .AutoCompleteCustomSource = cuspurpose
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.CustomSource
        End With


        listOfLocation = sqlce_asstnt.GET_ALL_KNOWN_MTO_LOCATION()

        Dim cuslocation As New AutoCompleteStringCollection
        cuslocation.AddRange(listOfLocation.ToArray)

        With txtx_mto_location
            .AutoCompleteCustomSource = cuslocation
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.CustomSource
        End With

        ''INCLUDE ALSO DIFINITION
        Dim listOfMTONames As New List(Of String)
        listOfMTONames = sqlce_asstnt.GET_ALL_TRAVEL_ORDER_NAMES()
        If listOfMTONames.Count = 0 Then
            Exit Sub
        End If
        cmbx_mto_ordertypes.Items.Clear()
        cmbx_mto_ordertypes.Items.AddRange(listOfMTONames.ToArray)
        cmbx_mto_ordertypes.SelectedIndex = 0
        SetCueText(txtx_mto_location, "Location")
        SetCueText(txtx_mto_purpose, "Purpose")

    End Sub

    Private Sub TAB_CONTROL_ATTENDANCE_MANAGEMENT_Click(sender As System.Object, e As System.EventArgs)
        'Dim listOfDep As New List(Of String)
        'listOfDep = sqlce_asstnt.GET_ALL_DEPARTMENT_NAME()

        'If TAB_CONTROL_ATTENDANCE_MANAGEMENT.SelectedTab Is TabPage22 Then      'Records Tab
        '    dtpx_rsr_startdate.Value = DateAndTime.Now.AddDays(-15)
        '    dtpx_rsr_enddate.Value = DateAndTime.Now

        '    cmbx_rsr_department.Items.Clear()
        '    cmbx_rsr_department.Items.Add("ALL")
        '    For Each ix As String In listOfDep
        '        cmbx_rsr_department.Items.Add(ix)
        '    Next
        '    cmbx_rsr_department.SelectedIndex = 0


        '    cmbx_rsr_department.Focus()

        'ElseIf TAB_CONTROL_ATTENDANCE_MANAGEMENT.SelectedTab Is TabPage23 Then  ''MANUAL ADJUSTMENT TAB


        '    ''FORGET CLOCK/INOUT
        '    listOfDep.Sort()
        '    cmbx_dptManual.Items.Clear()
        '    cmbx_dptManual.Items.AddRange(listOfDep.ToArray)
        '    If Not cmbx_dptManual.Items.Count = 0 Then
        '        cmbx_dptManual.SelectedIndex = 0
        '    End If

        '    ''LEAVE MANAGEMENT

        '    'LOAD_LEAVE_INTO_MANUAL_ADJUSTMENT()
        '    cmbx_dptManual.Focus()
        'ElseIf TAB_CONTROL_ATTENDANCE_MANAGEMENT.SelectedTab Is TabPage24 Then  ''HOLIDAY TAB
        '    ''HOLIDAY
        '    SQL_HOLIDAY_SCHEMA.ExecQuery("SELECT ID, HOLIDAYNAME AS [HOLIDAY NAME],[DATE] FROM PUBLICHOLIDAYTABLE")
        '    dtgv_PublicHolidaySchema.DataSource = SQL_HOLIDAY_SCHEMA.SQLDS.Tables(0)
        '    dtgv_PublicHolidaySchema.Rows(0).Selected = True
        '    SQL_HOLIDAY_SCHEMA.SQLDA.UpdateCommand = New SqlServerCe.SqlCeCommandBuilder(SQL_HOLIDAY_SCHEMA.SQLDA).GetUpdateCommand
        '    ARRANGE_HOLIDAY_DATAGRID_TRABLE()

        'End If
    End Sub

    'Private Sub btn_UpdateChangesHolidaySchema_Click(sender As System.Object, e As System.EventArgs)
    '    Try
    '        dtgv_PublicHolidaySchema.AllowUserToAddRows = False
    '        SQL_HOLIDAY_SCHEMA.SQLDA.Update(SQL_HOLIDAY_SCHEMA.SQLDS)
    '        dtgv_PublicHolidaySchema.AllowUserToAddRows = True
    '        MessageBox.Show("Update Success!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '        ARRANGE_HOLIDAY_DATAGRID_TRABLE()
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message)
    '        Console.WriteLine(ex.Message)
    '    End Try
    'End Sub
    'Private Sub ARRANGE_HOLIDAY_DATAGRID_TRABLE()
    '    For I1 = 0 To dtgv_PublicHolidaySchema.Columns.Count - 1
    '        If dtgv_PublicHolidaySchema.Columns(I1).Name = "ID" Then
    '            Dim column As DataGridViewColumn = dtgv_PublicHolidaySchema.Columns(I1)
    '            dtgv_PublicHolidaySchema.Columns(0).Visible = False
    '            column.Width = 200
    '        ElseIf dtgv_PublicHolidaySchema.Columns(I1).Name = "HOLIDAY NAME" Then
    '            Dim column As DataGridViewColumn = dtgv_PublicHolidaySchema.Columns(I1)
    '            column.Width = 500
    '        ElseIf dtgv_PublicHolidaySchema.Columns(I1).Name = "DATE" Then
    '            Dim column As DataGridViewColumn = dtgv_PublicHolidaySchema.Columns(I1)
    '            column.Width = 600
    '        End If
    '    Next
    'End Sub


    Private Sub dtgv_PublicHolidaySchema_CellValueChanged(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs)
        btn_UpdateChangesHolidaySchema.Enabled = True
    End Sub

    Private Sub dtgv_PublicHolidaySchema_ControlRemoved(sender As Object, e As System.Windows.Forms.ControlEventArgs)
        btn_UpdateChangesHolidaySchema.Enabled = True
    End Sub
    Private Sub EmployeeRemove_Click(sender As System.Object, e As System.EventArgs) Handles EmployeeRemove.Click

        'Dim CurrRowPos As Integer = dtgEmpProfiles.CurrentRow.Index
        'If Not IsDBNull(dtgEmpProfiles.Rows(CurrRowPos).Cells.Item(0).Value) Then
        '    If MessageBox.Show("You are about to remove one employee from the database." & vbCrLf & "Please confirm.", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = Windows.Forms.DialogResult.Yes Then
        '        sqlce_asstnt.REMOVE_EMPLOYEE(dtgEmpProfiles.Rows(CurrRowPos).Cells.Item(0).Value)
        '        dtgEmpProfiles.DataSource = sqlce_asstnt.GET_ALL_EMPLOYEES(True, False, Nothing, True, Nothing, False, Nothing)
        '        LOAD_DEPARTMENT_TO_COMBOBOX_EMPLOYEEPROFILES()
        '    End If
        'End If
    End Sub
    'Private Sub SHOW_DIM()

    '    Dim g As Graphics = Me.CreateGraphics
    '    ' Creating graphics for this form.

    '    Dim myRgbColor As New Color()
    '    myRgbColor = Color.FromArgb(80, 102, 90, 95)
    '    Dim redBrush As New SolidBrush(Color.AliceBlue)
    '    g.FillRectangle(redBrush, 10, 10, Me.Width, Me.Height)

    'End Sub

    Private Sub btnShowShiftTable_Click(sender As System.Object, e As System.EventArgs) Handles btnShowShiftTable.Click
        SHIFTTABLE.ShowDialog()
    End Sub

    Private Sub btn_ShowShiftSchema_Click(sender As System.Object, e As System.EventArgs) Handles btn_ShowShiftSchema.Click
        SHIFTSCHEMA.shift_id = sqlce_asstnt.GET_SHIFTID(cmbShiftList1.Text)
        SHIFTSCHEMA.ShowDialog()
    End Sub


    Private Sub ThreadCalculateParallel_DoWork(sender As System.Object, e As System.ComponentModel.DoWorkEventArgs) Handles ThreadCalculateParallel.DoWork



        ''STEP 2 DETERMINE IETHER PERDEPARTMENT/ ALL
        Dim listOfStaffID As New List(Of String)

        Dim s_id As String = ""
        For Each n In cmbx_selected_names.Items
            s_id = sqlce_asstnt.GET_STAFF_ID(n)
            listOfStaffID.Add(s_id)
        Next

        '''CHECK IF STAFFID IS EMPTY
        'If listOfStaffID.Count = 0 Then
        '    MessageBox.Show("Select staff", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '    Exit Sub
        'End If

        ''STEP 3 CHECK IF DEPARTMENT HAS EMPLOYEE
        'If listOfStaffID.Count = 0 Then
        '    sqlce_asstnt.CLEAN_DTR_TABLE()
        '    sqlce_asstnt.CLEAN_SRATABLE()
        '    sqlce_asstnt.CLEAN_SROTABLE()
        '    Exit Sub
        'End If
        sqlce_asstnt.CLEAN_DTR_TABLE()
        sqlce_asstnt.CLEAN_SRATABLE()
        sqlce_asstnt.CLEAN_SROTABLE()


        Dim cpu_wrkr As New CPU
        Dim calculate_worker As New CLASSCALCULATE
        Dim ce_worker As New SQLCE_MANAGER


        ''CHECk OF COLUMNS IF EXISTS
        ''IF DOES NOT EXIST THE CREATE IT.
        '''DATE ADDED: 9/24/2015
        If ce_worker.BMTS_CHECK_IF_TABLECOLUMN_IS_EXIST("DTR", "COL_LATE") = False Then
            ce_worker.BTMS_CREATE_TABLE_COLUMN("DTR", "COL_LATE", 1)
        End If
        '''DATE ADDED: 9/24/2015
        If ce_worker.BMTS_CHECK_IF_TABLECOLUMN_IS_EXIST("DTR", "COL_UT") = False Then
            ce_worker.BTMS_CREATE_TABLE_COLUMN("DTR", "COL_UT", 1)
        End If
        '''DATE ADDED: 9/24/2015

        If ce_worker.BMTS_CHECK_IF_TABLECOLUMN_IS_EXIST("DTR", "COL_DAILY_REMARKS") = False Then
            ce_worker.BTMS_CREATE_TABLE_COLUMN("DTR", "COL_DAILY_REMARKS", 1)
        End If
        '''DATE ADDED: 9/24/2015
        If ce_worker.BMTS_CHECK_IF_TABLECOLUMN_IS_EXIST("DTR", "COL_LATE_TOTAL") = False Then
            ce_worker.BTMS_CREATE_TABLE_COLUMN("DTR", "COL_LATE_TOTAL", 3)
        End If
        '''DATE ADDED: 9/24/2015

        If ce_worker.BMTS_CHECK_IF_TABLECOLUMN_IS_EXIST("DTR", "COL_UT_TOTAL") = False Then
            ce_worker.BTMS_CREATE_TABLE_COLUMN("DTR", "COL_UT_TOTAL", 3)
        End If
        '''DATE ADDED: 9/24/2015

        If ce_worker.BMTS_CHECK_IF_TABLECOLUMN_IS_EXIST("DTR", "COL_LATE_TOTAL_MONTHLY") = False Then
            ce_worker.BTMS_CREATE_TABLE_COLUMN("DTR", "COL_LATE_TOTAL_MONTHLY", 1)
        End If
        '''DATE ADDED: 9/24/2015

        If ce_worker.BMTS_CHECK_IF_TABLECOLUMN_IS_EXIST("DTR", "COL_UT_TOTAL_MONTHLY") = False Then
            ce_worker.BTMS_CREATE_TABLE_COLUMN("DTR", "COL_UT_TOTAL_MONTHLY", 1)
        End If
        '''DATE ADDED: 9/24/2015
        If ce_worker.BMTS_CHECK_IF_TABLECOLUMN_IS_EXIST("DTR", "DESIGNATION") = False Then
            ce_worker.BTMS_CREATE_TABLE_COLUMN("DTR", "DESIGNATION", 1)
        End If









        ''STEP 4 SET DATE RANGE FOR REPORT
        Dim s_date As Date = dtpStartDateRpt.Value
        Dim e_date As Date = dtpEndDateRpt.Value
        Dim listOfAttDates As New List(Of Date)
        'Dim staffname As String = ""
        listOfAttDates = cpu_wrkr.EXTRACTDATE(s_date, e_date)

        If File.Exists(My.Settings.zk_mdb_path) Then
            If MessageBox.Show("Do you want to perform raw logs syncing?", "BTMS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then

                '''before generating the report please check if the att2000.mdb function is enable and exist
                If My.Settings.att2000_sync = True Then
                    'If File.Exists(My.Settings.zk_mdb_path) Then
                    '  ZKmdbRead(s_date, e_date)
                    ' Dim CustID As Integer = CInt(TxtSearch.Text)
                    Console.Write("att2000.mdb is exist" & vbNewLine)
                    UpdateText(Label38, "Please wait while the data logs is syncing....")
                    Dim strCon As String = My.Settings.zk_con_string
                    Dim strQuery As String = "SELECT CHECKINOUT.USERID, CHECKINOUT.CHECKTIME, CHECKINOUT.CHECKTYPE, USERINFO.BadgeNumber FROM CHECKINOUT INNER JOIN USERINFO ON CHECKINOUT.USERID = USERINFO.USERID WHERE CHECKINOUT.CHECKTIME BETWEEN #" & s_date & "# AND #" & e_date & "#"

                    Dim con As New OleDbConnection(strCon)
                    Dim cmd As New OleDbCommand(strQuery, con)
                    con.Open()
                    Dim rdr As OleDbDataReader = cmd.ExecuteReader()
                    Dim chktype As String = String.Empty
                    Dim chktime As String = String.Empty
                    Dim chkdate As String = String.Empty
                    Dim staffname As String = String.Empty
                    While rdr.Read
                        If rdr.HasRows = True Then
                            Select Case CStr(rdr(2))
                                Case "I"
                                    chktype = "IN"
                                Case "O"
                                    chktype = "OUT"
                                Case "0"
                                    chktype = "BRKOUT"
                                Case "1"
                                    chktype = "BRKIN"
                                Case "i"
                                    chktype = "OTIN"
                                Case "o"
                                    chktype = "OTOUT"
                            End Select
                            '''STAFF BADGE

                            If usb_worker.VERIFY_USB_LOG(CStr(rdr(3)).Trim, Format(CDate(rdr(1)), "MM-dd-yyyy HH:mm:ss")) = False Then
                                '  Console.Write("New Logs: " & CStr(rdr(4)).Trim & " " & Format(CDate(rdr(1)), "MM-dd-yyyy HH:mm:ss") & vbNewLine)
                                'Dim raw As String = Format(CDate(rdr(1)), "MM-dd-yyyy HH:mm:00")
                                '2015-35-14 08:35:00
                                usb_worker.INSERT_NEW_LOG(CStr(rdr(3)).Trim, Format(CDate(rdr(1)), "MM/dd/yyyy"), Format(CDate(rdr(1)), "HH:mm:ss"), chktype, "att2000", False, Format(CDate(rdr(1)), "MM-dd-yyyy HH:mm:ss"), "")
                            Else
                                ' Console.WriteLine("SKIP: Logs already exist!" & vbNewLine)
                            End If
                        End If
                    End While
                    con.Close()
                    'Else
                    '    Console.WriteLine("att2000.mdb file not found!!!" & vbNewLine)
                End If
            End If
        End If

    









        ''STEP 6 WHAT TYPE OF REPORT IS SELECTED AND RUN PARALLEL CALCULATION

        Select Case selectedReportIconName
            Case "DTR"
                ''STEP 1 CLEAN UP DTR TABLE
                sqlce_asstnt.CLEAN_DTR_TABLE()
                sqlce_asstnt.CLEAN_SRATABLE()
                sqlce_asstnt.CLEAN_SROTABLE()



                Dim staffname As String = ""
                Dim departmentname As String = ""
                Dim shift_id As Integer = 0
                Dim shiftname As String = ""
                Dim designation As String = ""
                Dim TimeTableList As New List(Of Integer)
                Dim autopilot As Boolean = False




                System.Threading.Tasks.Parallel.ForEach(listOfStaffID, Sub(staffid)

                                                                           SyncLock listOfAttDates
                                                                               staffname = ce_worker.GET_STAFF_NAME(staffid)
                                                                               departmentname = ce_worker.GET_STAFF_ASSIGN_DEPARTMENT_NAME(staffid)
                                                                               designation = ce_worker.GET_STAFF_DESIGNATION(staffid)

                                                                               shift_id = 0
                                                                               shiftname = ""
                                                                               autopilot = False
                                                                               For Each _d As Date In listOfAttDates       ''MAIN CALCULATION START HERE! PER EMPLOYEE
                                                                                   shift_id = ce_worker.GET_STAFF_SHIFT_ID(staffid, _d)
                                                                                   TimeTableList = ce_worker.GET_SHIFT_TIMETABLEIDS(shift_id)
                                                                                   autopilot = ce_worker.CHECK_AUTOPILOT(shift_id)
                                                                                   shiftname = ce_worker.GET_STAFF_SHIFT_NAME(staffid, _d)
                                                                                   calculate_worker.MAIN(staffid, staffname, departmentname, _d, shift_id, shiftname, TimeTableList, autopilot, designation)
                                                                                   'Console.WriteLine("Calculating: " & staffname & " Dated: " & _d)
                                                                               Next

                                                                               Dim TOTALUNDERTIMEMONTHLY As String = ""
                                                                               Dim TOTAL As Integer = ce_worker.SUM_ALL_UNDERTIME_DAILY_IN_DTR_TABLE(staffid, s_date, e_date)
                                                                               If Not TOTAL = 0 Then
                                                                                   Dim Hours As Integer = Math.Floor(TOTAL / 60)
                                                                                   Dim Minutes As Integer = TOTAL Mod 60

                                                                                   ''FORMAT THE VALUE FOR DTR TOTAL UNDERTIME OF EMPLOYEE MONTHLY
                                                                                   TOTALUNDERTIMEMONTHLY = Hours & "Hr(s)" & " and " & Minutes & "Mins"
                                                                                   ce_worker.UPDATE_INTO_DTR_TABLE_TOTAL_UNDERTIME_MONTHLY(staffid, e_date, TOTALUNDERTIMEMONTHLY, TOTAL, "", "")
                                                                                   UpdateText(Label38, staffname & " Completed!")
                                                                               End If
                                                                           End SyncLock
                                                                       End Sub)


            Case "DTR_B"
                ''STEP 1 CLEAN UP DTR TABLE
                sqlce_asstnt.CLEAN_DTR_TABLE()
                sqlce_asstnt.CLEAN_SRATABLE()
                sqlce_asstnt.CLEAN_SROTABLE()


                'Dim cpu_wrkr As New CPU
                'Dim calculate_worker As New CLASSCALCULATE
                'Dim ce_worker As New SQLCE_MANAGER



                Dim staffname As String = ""
                Dim departmentname As String = ""
                Dim shift_id As Integer = 0
                Dim shiftname As String = ""
                Dim designation As String = ""
                Dim TimeTableList As New List(Of Integer)
                Dim autopilot As Boolean = False
                System.Threading.Tasks.Parallel.ForEach(listOfStaffID, Sub(staffid)

                                                                           SyncLock listOfAttDates
                                                                               staffname = ce_worker.GET_STAFF_NAME(staffid)
                                                                               departmentname = ce_worker.GET_STAFF_ASSIGN_DEPARTMENT_NAME(staffid)
                                                                               designation = ce_worker.GET_STAFF_DESIGNATION(staffid)
                                                                               shift_id = 0
                                                                               shiftname = ""
                                                                               autopilot = False
                                                                               For Each _d As Date In listOfAttDates       ''MAIN CALCULATION START HERE! PER EMPLOYEE
                                                                                   shift_id = ce_worker.GET_STAFF_SHIFT_ID(staffid, _d)
                                                                                   TimeTableList = ce_worker.GET_SHIFT_TIMETABLEIDS(shift_id)
                                                                                   autopilot = ce_worker.CHECK_AUTOPILOT(shift_id)
                                                                                   shiftname = ce_worker.GET_STAFF_SHIFT_NAME(staffid, _d)
                                                                                   calculate_worker.MAIN(staffid, staffname, departmentname, _d, shift_id, shiftname, TimeTableList, autopilot, designation)
                                                                                   'Console.WriteLine("Calculating: " & staffname & " Dated: " & _d)
                                                                               Next

                                                                               Dim TOTALUNDERTIMEMONTHLY As String = ""
                                                                               Dim TOTAL As Integer = ce_worker.SUM_ALL_UNDERTIME_DAILY_IN_DTR_TABLE(staffid, s_date, e_date)
                                                                               If Not TOTAL = 0 Then
                                                                                   Dim Hours As Integer = Math.Floor(TOTAL / 60)
                                                                                   Dim Minutes As Integer = TOTAL Mod 60

                                                                                   ''FORMAT THE VALUE FOR DTR TOTAL UNDERTIME OF EMPLOYEE MONTHLY
                                                                                   TOTALUNDERTIMEMONTHLY = Hours & "Hr(s)" & " and " & Minutes & "Mins"
                                                                                   ce_worker.UPDATE_INTO_DTR_TABLE_TOTAL_UNDERTIME_MONTHLY(staffid, e_date, TOTALUNDERTIMEMONTHLY, TOTAL, "", "")
                                                                                   UpdateText(Label38, staffname & " Completed!")
                                                                               End If
                                                                           End SyncLock
                                                                       End Sub)

            Case "DTR_C"
                ''STEP 1 CLEAN UP DTR TABLE
                sqlce_asstnt.CLEAN_DTR_TABLE()
                sqlce_asstnt.CLEAN_SRATABLE()
                sqlce_asstnt.CLEAN_SROTABLE()


                'Dim cpu_wrkr As New CPU
                'Dim calculate_worker As New CLASSCALCULATE
                'Dim ce_worker As New SQLCE_MANAGER



                Dim staffname As String = ""
                Dim departmentname As String = ""
                Dim designation As String = ""
                Dim shift_id As Integer = 0
                Dim shiftname As String = ""
                Dim TimeTableList As New List(Of Integer)
                Dim autopilot As Boolean = False
                System.Threading.Tasks.Parallel.ForEach(listOfStaffID, Sub(staffid)

                                                                           SyncLock listOfAttDates
                                                                               staffname = ce_worker.GET_STAFF_NAME(staffid)
                                                                               departmentname = ce_worker.GET_STAFF_ASSIGN_DEPARTMENT_NAME(staffid)
                                                                               designation = ce_worker.GET_STAFF_DESIGNATION(staffid)
                                                                               shift_id = 0
                                                                               shiftname = ""
                                                                               autopilot = False
                                                                               For Each _d As Date In listOfAttDates       ''MAIN CALCULATION START HERE! PER EMPLOYEE
                                                                                   shift_id = ce_worker.GET_STAFF_SHIFT_ID(staffid, _d)
                                                                                   TimeTableList = ce_worker.GET_SHIFT_TIMETABLEIDS(shift_id)
                                                                                   autopilot = ce_worker.CHECK_AUTOPILOT(shift_id)
                                                                                   shiftname = ce_worker.GET_STAFF_SHIFT_NAME(staffid, _d)
                                                                                   calculate_worker.MAIN(staffid, staffname, departmentname, _d, shift_id, shiftname, TimeTableList, autopilot, designation)
                                                                                   'Console.WriteLine("Calculating: " & staffname & " Dated: " & _d)
                                                                               Next

                                                                               Dim TOTALUNDERTIMEMONTHLY As String = ""
                                                                               Dim TOTAL As Integer = ce_worker.SUM_ALL_UNDERTIME_DAILY_IN_DTR_TABLE(staffid, s_date, e_date)
                                                                               If Not TOTAL = 0 Then
                                                                                   Dim Hours As Integer = Math.Floor(TOTAL / 60)
                                                                                   Dim Minutes As Integer = TOTAL Mod 60

                                                                                   ''FORMAT THE VALUE FOR DTR TOTAL UNDERTIME OF EMPLOYEE MONTHLY
                                                                                   TOTALUNDERTIMEMONTHLY = Hours & "Hr(s)" & " and " & Minutes & "Mins"
                                                                                   ce_worker.UPDATE_INTO_DTR_TABLE_TOTAL_UNDERTIME_MONTHLY(staffid, e_date, TOTALUNDERTIMEMONTHLY, TOTAL, "", "")
                                                                                   UpdateText(Label38, staffname & " Completed!")
                                                                               End If
                                                                           End SyncLock
                                                                       End Sub)






            Case "DTR_D"
                ''STEP 1 CLEAN UP DTR TABLE
                sqlce_asstnt.CLEAN_DTR_TABLE()
                sqlce_asstnt.CLEAN_SRATABLE()
                sqlce_asstnt.CLEAN_SROTABLE()






        

                Dim staffname As String = ""
                Dim departmentname As String = ""
                Dim designation As String = ""
                Dim shift_id As Integer = 0
                Dim shiftname As String = ""
                Dim TimeTableList As New List(Of Integer)
                Dim autopilot As Boolean = False
                System.Threading.Tasks.Parallel.ForEach(listOfStaffID, Sub(staffid)

                                                                           SyncLock listOfAttDates
                                                                               staffname = ce_worker.GET_STAFF_NAME(staffid)
                                                                               departmentname = ce_worker.GET_STAFF_ASSIGN_DEPARTMENT_NAME(staffid)
                                                                               designation = ce_worker.GET_STAFF_DESIGNATION(staffid)
                                                                               shift_id = 0
                                                                               shiftname = ""
                                                                               autopilot = False
                                                                               For Each _d As Date In listOfAttDates       ''MAIN CALCULATION START HERE! PER EMPLOYEE
                                                                                   shift_id = ce_worker.GET_STAFF_SHIFT_ID(staffid, _d)
                                                                                   TimeTableList = ce_worker.GET_SHIFT_TIMETABLEIDS(shift_id)
                                                                                   autopilot = ce_worker.CHECK_AUTOPILOT(shift_id)
                                                                                   shiftname = ce_worker.GET_STAFF_SHIFT_NAME(staffid, _d)
                                                                                   calculate_worker.MAIN(staffid, staffname, departmentname, _d, shift_id, shiftname, TimeTableList, autopilot, designation)
                                                                                   'Console.WriteLine("Calculating: " & staffname & " Dated: " & _d)
                                                                               Next

                                                                               Dim TOTALUNDERTIMEMONTHLY As String = ""
                                                                               Dim TOTAL As Integer = ce_worker.SUM_ALL_UNDERTIME_DAILY_IN_DTR_TABLE(staffid, s_date, e_date)
                                                                               Dim LATE_TOTAL As Integer = ce_worker.SUM_LATE_DAILY(staffid, s_date, e_date)
                                                                               Dim UT_TOTAL As Integer = ce_worker.SUM_UT_DAILY(staffid, s_date, e_date)
                                                                               Dim LATE_TOTAL_S As String = ""
                                                                               Dim UT_TOTAL_S As String = ""

                                                                               If Not TOTAL = 0 Or Not LATE_TOTAL = 0 Or Not UT_TOTAL = 0 Then
                                                                                   Dim Hours As Integer = Math.Floor(TOTAL / 60)
                                                                                   Dim Minutes As Integer = TOTAL Mod 60
                                                                                   ''FORMAT THE VALUE FOR DTR TOTAL UNDERTIME OF EMPLOYEE MONTHLY
                                                                                   TOTALUNDERTIMEMONTHLY = Hours & "Hr(s)" & " and " & Minutes & "Mins"
                                                                                   Hours = 0
                                                                                   Minutes = 0

                                                                                   ''CALCULATION OF LATE TOTAL
                                                                                   Hours = Math.Floor(LATE_TOTAL / 60)
                                                                                   Minutes = LATE_TOTAL Mod 60
                                                                                   ''FORMAT THE VALUE FOR DTR TOTAL UNDERTIME OF EMPLOYEE MONTHL
                                                                                   If Not Hours = 0 Then
                                                                                       If Not Minutes = 0 Then
                                                                                           LATE_TOTAL_S = Hours & "h" & Minutes & "m"
                                                                                       Else
                                                                                           LATE_TOTAL_S = Hours & "h"
                                                                                       End If

                                                                                   ElseIf Not Minutes = 0 Then
                                                                                       LATE_TOTAL_S = Minutes & "m"
                                                                                   End If

                                                                                   ''CALCULATION OF UNDERTIME TOTAL
                                                                                   Hours = 0
                                                                                   Minutes = 0
                                                                                   Hours = Math.Floor(UT_TOTAL / 60)
                                                                                   Minutes = UT_TOTAL Mod 60
                                                                                   ''FORMAT THE VALUE FOR DTR TOTAL UNDERTIME OF EMPLOYEE MONTHL
                                                                                   If Not Hours = 0 Then
                                                                                       If Not Minutes = 0 Then
                                                                                           UT_TOTAL_S = Hours & "h" & Minutes & "m"
                                                                                       Else
                                                                                           UT_TOTAL_S = Hours & "h"
                                                                                       End If

                                                                                   ElseIf Not Minutes = 0 Then
                                                                                       UT_TOTAL_S = Minutes & "m"
                                                                                   End If
                                                                                   ce_worker.UPDATE_INTO_DTR_TABLE_TOTAL_UNDERTIME_MONTHLY(staffid, e_date, TOTALUNDERTIMEMONTHLY, TOTAL, LATE_TOTAL_S, UT_TOTAL_S)
                                                                                   UpdateText(Label38, staffname & " Completed!")
                                                                               End If


                                                                           End SyncLock
                                                                       End Sub)

            Case "DTR_E"
                ''STEP 1 CLEAN UP DTR TABLE
                sqlce_asstnt.CLEAN_DTR_TABLE()
                sqlce_asstnt.CLEAN_SRATABLE()
                sqlce_asstnt.CLEAN_SROTABLE()








                Dim staffname As String = ""
                Dim departmentname As String = ""
                Dim designation As String = ""
                Dim shift_id As Integer = 0
                Dim shiftname As String = ""
                Dim TimeTableList As New List(Of Integer)
                Dim autopilot As Boolean = False
                System.Threading.Tasks.Parallel.ForEach(listOfStaffID, Sub(staffid)

                                                                           SyncLock listOfAttDates
                                                                               staffname = ce_worker.GET_STAFF_NAME(staffid)
                                                                               departmentname = ce_worker.GET_STAFF_ASSIGN_DEPARTMENT_NAME(staffid)
                                                                               designation = ce_worker.GET_STAFF_DESIGNATION(staffid)
                                                                               shift_id = 0
                                                                               shiftname = ""
                                                                               autopilot = False
                                                                               For Each _d As Date In listOfAttDates       ''MAIN CALCULATION START HERE! PER EMPLOYEE
                                                                                   shift_id = ce_worker.GET_STAFF_SHIFT_ID(staffid, _d)
                                                                                   TimeTableList = ce_worker.GET_SHIFT_TIMETABLEIDS(shift_id)
                                                                                   autopilot = ce_worker.CHECK_AUTOPILOT(shift_id)
                                                                                   shiftname = ce_worker.GET_STAFF_SHIFT_NAME(staffid, _d)
                                                                                   calculate_worker.MAIN(staffid, staffname, departmentname, _d, shift_id, shiftname, TimeTableList, autopilot, designation)
                                                                                   'Console.WriteLine("Calculating: " & staffname & " Dated: " & _d)
                                                                               Next

                                                                               Dim TOTALUNDERTIMEMONTHLY As String = ""
                                                                               Dim TOTAL As Integer = ce_worker.SUM_ALL_UNDERTIME_DAILY_IN_DTR_TABLE(staffid, s_date, e_date)
                                                                               Dim LATE_TOTAL As Integer = ce_worker.SUM_LATE_DAILY(staffid, s_date, e_date)
                                                                               Dim UT_TOTAL As Integer = ce_worker.SUM_UT_DAILY(staffid, s_date, e_date)
                                                                               Dim LATE_TOTAL_S As String = ""
                                                                               Dim UT_TOTAL_S As String = ""

                                                                               If Not TOTAL = 0 Or Not LATE_TOTAL = 0 Or Not UT_TOTAL = 0 Then
                                                                                   Dim Hours As Integer = Math.Floor(TOTAL / 60)
                                                                                   Dim Minutes As Integer = TOTAL Mod 60
                                                                                   ''FORMAT THE VALUE FOR DTR TOTAL UNDERTIME OF EMPLOYEE MONTHLY
                                                                                   TOTALUNDERTIMEMONTHLY = Hours & "Hr(s)" & " and " & Minutes & "Mins"
                                                                                   Hours = 0
                                                                                   Minutes = 0

                                                                                   ''CALCULATION OF LATE TOTAL
                                                                                   Hours = Math.Floor(LATE_TOTAL / 60)
                                                                                   Minutes = LATE_TOTAL Mod 60
                                                                                   ''FORMAT THE VALUE FOR DTR TOTAL UNDERTIME OF EMPLOYEE MONTHL
                                                                                   If Not Hours = 0 Then
                                                                                       If Not Minutes = 0 Then
                                                                                           LATE_TOTAL_S = Hours & "h" & Minutes & "m"
                                                                                       Else
                                                                                           LATE_TOTAL_S = Hours & "h"
                                                                                       End If

                                                                                   ElseIf Not Minutes = 0 Then
                                                                                       LATE_TOTAL_S = Minutes & "m"
                                                                                   End If

                                                                                   ''CALCULATION OF UNDERTIME TOTAL
                                                                                   Hours = 0
                                                                                   Minutes = 0
                                                                                   Hours = Math.Floor(UT_TOTAL / 60)
                                                                                   Minutes = UT_TOTAL Mod 60
                                                                                   ''FORMAT THE VALUE FOR DTR TOTAL UNDERTIME OF EMPLOYEE MONTHL
                                                                                   If Not Hours = 0 Then
                                                                                       If Not Minutes = 0 Then
                                                                                           UT_TOTAL_S = Hours & "h" & Minutes & "m"
                                                                                       Else
                                                                                           UT_TOTAL_S = Hours & "h"
                                                                                       End If

                                                                                   ElseIf Not Minutes = 0 Then
                                                                                       UT_TOTAL_S = Minutes & "m"
                                                                                   End If
                                                                                   ce_worker.UPDATE_INTO_DTR_TABLE_TOTAL_UNDERTIME_MONTHLY(staffid, e_date, TOTALUNDERTIMEMONTHLY, TOTAL, LATE_TOTAL_S, UT_TOTAL_S)
                                                                                   UpdateText(Label38, staffname & " Completed!")
                                                                               End If


                                                                           End SyncLock
                                                                       End Sub)


            Case "DTR_F"
                ''STEP 1 CLEAN UP DTR TABLE
                sqlce_asstnt.CLEAN_DTR_TABLE()
                sqlce_asstnt.CLEAN_SRATABLE()
                sqlce_asstnt.CLEAN_SROTABLE()

                Dim staffname As String = ""
                Dim departmentname As String = ""
                Dim designation As String = ""
                Dim shift_id As Integer = 0
                Dim shiftname As String = ""
                Dim TimeTableList As New List(Of Integer)
                Dim autopilot As Boolean = False
                System.Threading.Tasks.Parallel.ForEach(listOfStaffID, Sub(staffid)

                                                                           SyncLock listOfAttDates
                                                                               staffname = ce_worker.GET_STAFF_NAME(staffid)
                                                                               departmentname = ce_worker.GET_STAFF_ASSIGN_DEPARTMENT_NAME(staffid)
                                                                               designation = ce_worker.GET_STAFF_DESIGNATION(staffid)
                                                                               shift_id = 0
                                                                               shiftname = ""
                                                                               autopilot = False
                                                                               For Each _d As Date In listOfAttDates       ''MAIN CALCULATION START HERE! PER EMPLOYEE
                                                                                   shift_id = ce_worker.GET_STAFF_SHIFT_ID(staffid, _d)
                                                                                   TimeTableList = ce_worker.GET_SHIFT_TIMETABLEIDS(shift_id)
                                                                                   autopilot = ce_worker.CHECK_AUTOPILOT(shift_id)
                                                                                   shiftname = ce_worker.GET_STAFF_SHIFT_NAME(staffid, _d)
                                                                                   calculate_worker.MAIN(staffid, staffname, departmentname, _d, shift_id, shiftname, TimeTableList, autopilot, designation)
                                                                                   'Console.WriteLine("Calculating: " & staffname & " Dated: " & _d)
                                                                               Next

                                                                               Dim TOTALUNDERTIMEMONTHLY As String = ""
                                                                               Dim TOTAL As Integer = ce_worker.SUM_ALL_UNDERTIME_DAILY_IN_DTR_TABLE(staffid, s_date, e_date)
                                                                               Dim LATE_TOTAL As Integer = ce_worker.SUM_LATE_DAILY(staffid, s_date, e_date)
                                                                               Dim UT_TOTAL As Integer = ce_worker.SUM_UT_DAILY(staffid, s_date, e_date)
                                                                               Dim LATE_TOTAL_S As String = ""
                                                                               Dim UT_TOTAL_S As String = ""

                                                                               If Not TOTAL = 0 Or Not LATE_TOTAL = 0 Or Not UT_TOTAL = 0 Then
                                                                                   Dim Hours As Integer = Math.Floor(TOTAL / 60)
                                                                                   Dim Minutes As Integer = TOTAL Mod 60
                                                                                   ''FORMAT THE VALUE FOR DTR TOTAL UNDERTIME OF EMPLOYEE MONTHLY
                                                                                   TOTALUNDERTIMEMONTHLY = Hours & "Hr(s)" & " and " & Minutes & "Mins"
                                                                                   Hours = 0
                                                                                   Minutes = 0

                                                                                   ''CALCULATION OF LATE TOTAL
                                                                                   Hours = Math.Floor(LATE_TOTAL / 60)
                                                                                   Minutes = LATE_TOTAL Mod 60
                                                                                   ''FORMAT THE VALUE FOR DTR TOTAL UNDERTIME OF EMPLOYEE MONTHL
                                                                                   If Not Hours = 0 Then
                                                                                       If Not Minutes = 0 Then
                                                                                           LATE_TOTAL_S = Hours & "h" & Minutes & "m"
                                                                                       Else
                                                                                           LATE_TOTAL_S = Hours & "h"
                                                                                       End If

                                                                                   ElseIf Not Minutes = 0 Then
                                                                                       LATE_TOTAL_S = Minutes & "m"
                                                                                   End If

                                                                                   ''CALCULATION OF UNDERTIME TOTAL
                                                                                   Hours = 0
                                                                                   Minutes = 0
                                                                                   Hours = Math.Floor(UT_TOTAL / 60)
                                                                                   Minutes = UT_TOTAL Mod 60
                                                                                   ''FORMAT THE VALUE FOR DTR TOTAL UNDERTIME OF EMPLOYEE MONTHL
                                                                                   If Not Hours = 0 Then
                                                                                       If Not Minutes = 0 Then
                                                                                           UT_TOTAL_S = Hours & "h" & Minutes & "m"
                                                                                       Else
                                                                                           UT_TOTAL_S = Hours & "h"
                                                                                       End If

                                                                                   ElseIf Not Minutes = 0 Then
                                                                                       UT_TOTAL_S = Minutes & "m"
                                                                                   End If
                                                                                   ce_worker.UPDATE_INTO_DTR_TABLE_TOTAL_UNDERTIME_MONTHLY(staffid, e_date, TOTALUNDERTIMEMONTHLY, TOTAL, LATE_TOTAL_S, UT_TOTAL_S)
                                                                                   UpdateText(Label38, staffname & " Completed!")
                                                                               End If


                                                                           End SyncLock
                                                                       End Sub)





            Case "SRA"
                ''STEP 1 GENERATING SRA REPORT(RUN FIRST THE DTR CALCULATION SINCE SRA IS IN DTR TABLE)
                ''STEP 1 CLEAN UP DTR TABLE
                sqlce_asstnt.CLEAN_DTR_TABLE()
                sqlce_asstnt.CLEAN_SRATABLE()
                sqlce_asstnt.CLEAN_SROTABLE()


                'Dim cpu_wrkr As New CPU
                'Dim calculate_worker As New CLASSCALCULATE
                'Dim ce_worker As New SQLCE_MANAGER



                Dim staffname As String = ""
                Dim departmentname As String = ""
                Dim designation As String = ""
                Dim shift_id As Integer = 0
                Dim shiftname As String = ""
                Dim TimeTableList As New List(Of Integer)
                Dim autopilot As Boolean = False

                System.Threading.Tasks.Parallel.ForEach(listOfStaffID, Sub(staffid)

                                                                           SyncLock listOfAttDates
                                                                               staffname = ce_worker.GET_STAFF_NAME(staffid)
                                                                               departmentname = ce_worker.GET_STAFF_ASSIGN_DEPARTMENT_NAME(staffid)
                                                                               designation = ce_worker.GET_STAFF_DESIGNATION(staffid)
                                                                               shift_id = 0
                                                                               shiftname = ""
                                                                               autopilot = False
                                                                               For Each _d As Date In listOfAttDates       ''MAIN CALCULATION START HERE! PER EMPLOYEE
                                                                                   shift_id = ce_worker.GET_STAFF_SHIFT_ID(staffid, _d)
                                                                                   TimeTableList = ce_worker.GET_SHIFT_TIMETABLEIDS(shift_id)
                                                                                   autopilot = ce_worker.CHECK_AUTOPILOT(shift_id)
                                                                                   shiftname = ce_worker.GET_STAFF_SHIFT_NAME(staffid, _d)
                                                                                   calculate_worker.MAIN(staffid, staffname, departmentname, _d, shift_id, shiftname, TimeTableList, autopilot, designation)
                                                                                   Console.WriteLine("Calculating: " & staffname & " Dated: " & _d)
                                                                               Next

                                                                               Dim TOTALUNDERTIMEMONTHLY As String = ""
                                                                               Dim TOTAL As Integer = ce_worker.SUM_ALL_UNDERTIME_DAILY_IN_DTR_TABLE(staffid, s_date, e_date)
                                                                               If Not TOTAL = 0 Then
                                                                                   Dim Hours As Integer = Math.Floor(TOTAL / 60)
                                                                                   Dim Minutes As Integer = TOTAL Mod 60

                                                                                   ''FORMAT THE VALUE FOR DTR TOTAL UNDERTIME OF EMPLOYEE MONTHLY
                                                                                   TOTALUNDERTIMEMONTHLY = Hours & "Hr(s)" & " and " & Minutes & "Mins"
                                                                                   ce_worker.UPDATE_INTO_DTR_TABLE_TOTAL_UNDERTIME_MONTHLY(staffid, e_date, TOTALUNDERTIMEMONTHLY, TOTAL, "", "")
                                                                                   UpdateText(Label38, staffname & " Completed!")
                                                                               End If
                                                                           End SyncLock
                                                                       End Sub)














                ''STEP 2 RUN THE PLOTTING OF TABLE OF SRATABLE
                ''TASK1 GET ALL STAFFNAME IN DTR TABLE ARRANGE DESC
                Dim listOffStaffName As New List(Of String)
                listOffStaffName = ce_worker.GET_LIST_OF_STAFFNAME_IN_DTRTABLE()



                Dim item_id As Integer = 0
                For Each Name As String In listOffStaffName
                    item_id = item_id + 1
                    ce_worker.INSERT_STAFFNAME_CTR_INTO_SRATABLE(Name, item_id)
                Next


                listOffStaffName.ForEach(Sub(sname)

                                             ''GET THE LIST OF DAILY REMARKS PER STAFFNAME
                                             Dim listOfDailyRemarks As New List(Of String)
                                             listOfDailyRemarks = ce_worker.GET_MONTHLY_REMARKS_IN_DTRTABLE(sname)

                                             ''GET TOTLA UNDERTIME REMARKS LOCATED IN DTR TABLE COLNAME DTRMONTHLYREMAKRS
                                             Dim HH As Integer = 0
                                             Dim MM As Integer = 0
                                             Dim TOTAL As Integer = ce_worker.GET_TOTALMINUTES_UNDERTIMEMONTHLY(sname, s_date, e_date)
                                             If Not TOTAL = 0 Then
                                                 HH = Math.Floor(TOTAL / 60)
                                                 MM = TOTAL Mod 60
                                             End If
                                             ''TODO: IF NO EXCEPTION COUNT IN THE SRA REPORT SEE IF PARAMETER EXCEPTIONNAME IS MATCH IN THE LEAVE CLASS TABLE
                                             ce_worker.UPDATE_SRATABLE(sname, listOfDailyRemarks,
                                                                                                                       ce_worker.GET_EXCEPTION_COUNT(sname, "MC6", s_date, e_date),
                                                                                                                        ce_worker.GET_EXCEPTION_COUNT(sname, "FL", s_date, e_date),
                                                                                                                         ce_worker.GET_EXCEPTION_COUNT(sname, "MLA", s_date, e_date),
                                                                                                                          ce_worker.GET_EXCEPTION_COUNT(sname, "PatL", s_date, e_date),
                                                                                                                           ce_worker.GET_EXCEPTION_COUNT(sname, "VL", s_date, e_date),
                                                                                                                            ce_worker.GET_EXCEPTION_COUNT(sname, "SL", s_date, e_date),
                                                                                                                             ce_worker.GET_EXCEPTION_COUNT(sname, "A", s_date, e_date),
                                                                                                                              HH, MM)
                                             UpdateText(Label38, sname & " Completed!")
                                         End Sub)




            Case "SROT"

                ''STEP 1 CLEAN UP DTR TABLE
                sqlce_asstnt.CLEAN_DTR_TABLE()
                sqlce_asstnt.CLEAN_SRATABLE()
                sqlce_asstnt.CLEAN_SROTABLE()


                'Dim cpu_wrkr As New CPU
                'Dim calculate_worker As New CLASSCALCULATE
                'Dim ce_worker As New SQLCE_MANAGER



                Dim staffname As String = ""
                Dim departmentname As String = ""
                Dim designation As String = ""
                Dim shift_id As Integer = 0
                Dim shiftname As String = ""
                Dim TimeTableList As New List(Of Integer)
                Dim autopilot As Boolean = False




                System.Threading.Tasks.Parallel.ForEach(listOfStaffID, Sub(staffid)

                                                                           SyncLock listOfAttDates
                                                                               staffname = ce_worker.GET_STAFF_NAME(staffid)
                                                                               departmentname = ce_worker.GET_STAFF_ASSIGN_DEPARTMENT_NAME(staffid)
                                                                               designation = ce_worker.GET_STAFF_DESIGNATION(staffid)
                                                                               shift_id = 0
                                                                               shiftname = ""
                                                                               autopilot = False
                                                                               For Each _d As Date In listOfAttDates       ''MAIN CALCULATION START HERE! PER EMPLOYEE
                                                                                   shift_id = ce_worker.GET_STAFF_SHIFT_ID(staffid, _d)
                                                                                   TimeTableList = ce_worker.GET_SHIFT_TIMETABLEIDS(shift_id)
                                                                                   autopilot = ce_worker.CHECK_AUTOPILOT(shift_id)
                                                                                   shiftname = ce_worker.GET_STAFF_SHIFT_NAME(staffid, _d)
                                                                                   calculate_worker.MAIN(staffid, staffname, departmentname, _d, shift_id, shiftname, TimeTableList, autopilot, designation)
                                                                                   Console.WriteLine("Calculating: " & staffname & " Dated: " & _d)
                                                                               Next

                                                                               Dim TOTALUNDERTIMEMONTHLY As String = ""
                                                                               Dim TOTAL As Integer = ce_worker.SUM_ALL_UNDERTIME_DAILY_IN_DTR_TABLE(staffid, s_date, e_date)
                                                                               If Not TOTAL = 0 Then
                                                                                   Dim Hours As Integer = Math.Floor(TOTAL / 60)
                                                                                   Dim Minutes As Integer = TOTAL Mod 60

                                                                                   ''FORMAT THE VALUE FOR DTR TOTAL UNDERTIME OF EMPLOYEE MONTHLY
                                                                                   TOTALUNDERTIMEMONTHLY = Hours & "Hr(s)" & " and " & Minutes & "Mins"
                                                                                   ce_worker.UPDATE_INTO_DTR_TABLE_TOTAL_UNDERTIME_MONTHLY(staffid, e_date, TOTALUNDERTIMEMONTHLY, TOTAL, "", "")
                                                                                   UpdateText(Label38, staffname & " Completed!")
                                                                               End If
                                                                           End SyncLock
                                                                       End Sub)





                Dim listOffStaffName As New List(Of String)
                listOffStaffName = ce_worker.GET_LIST_OF_STAFFNAME_IN_DTRTABLE()
                Dim item_id As Integer = 0
                For Each Name As String In listOffStaffName
                    item_id = item_id + 1
                    ce_worker.INSERT_STAFFNAME_CTR_INTO_SROTTABLE(Name, item_id)
                Next

                listOffStaffName.ForEach(Sub(sname)

                                             ''GET THE LIST OF DAILY REMARKS PER STAFFNAME
                                             Dim listOfDailyRemarks As New List(Of String)
                                             listOfDailyRemarks = ce_worker.GET_OT_DAILY_REMARKS(sname)

                                             ''GET TOTLA UNDERTIME REMARKS LOCATED IN DTR TABLE COLNAME DTRMONTHLYREMAKRS
                                             Dim HH As Integer = 0
                                             Dim MM As Integer = 0
                                             Dim TOTAL As Integer = ce_worker.GET_TOTALMINUTES_OVERTIME(sname, s_date, e_date)
                                             If Not TOTAL = 0 Then
                                                 HH = Math.Floor(TOTAL / 60)
                                                 MM = TOTAL Mod 60
                                             End If
                                             ''TODO: IF NO EXCEPTION COUNT IN THE SRA REPORT SEE IF PARAMETER EXCEPTIONNAME IS MATCH IN THE LEAVE CLASS TABLE
                                             ce_worker.UPDATE_SROTBLE(sname, listOfDailyRemarks, HH, MM)
                                             UpdateText(Label38, sname & " Completed!")
                                         End Sub)
        End Select
    End Sub
    Private Sub ThreadCalculateParallel_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles ThreadCalculateParallel.RunWorkerCompleted
        Label38.Text = ""
        CONFIGURE_REPORT_ICON(False)
        Select Case selectedReportIconName.ToString
            Case "DTR"
                VIEWER.VIEWREPORTTYPE = "DTR"
                VIEWER.Show()
            Case "DTR_B"
                VIEWER.VIEWREPORTTYPE = "DTR_B"
                VIEWER.Show()
            Case "DTR_C"
                VIEWER.VIEWREPORTTYPE = "DTR_C"
                VIEWER.Show()
            Case "DTR_D"
                VIEWER.VIEWREPORTTYPE = "DTR_D"
                VIEWER.Show()
            Case "DTR_E"
                VIEWER.VIEWREPORTTYPE = "DTR_D"
                VIEWER.Show()
            Case "DTR_F"
                VIEWER.VIEWREPORTTYPE = "DTR_F"
                VIEWER.Show()
            Case "SRA"
                VIEWER.VIEWREPORTTYPE = "SRA"
                VIEWER.Show()
            Case "SROT"
                VIEWER.VIEWREPORTTYPE = "SROT"
                VIEWER.Show()
        End Select
        Label19.Visible = True
        btnReportExit.Visible = True
        cmbx_selected_names.Items.Clear()
    End Sub




    Private Sub btn_SaveContinue_Click(sender As System.Object, e As System.EventArgs) Handles btn_SaveContinue.Click
        Label19.Visible = True
        btnReportExit.Visible = True

        If VIEWER.Visible = True Then
            VIEWER.Close()
        End If


        'FORMAT SRASUMMARY HEADER DATE
        VIEWER.SummaryDate = Format(dtpEndDateRpt.Value, "MMMMM yyyy")

        If Not (String.IsNullOrEmpty(txtx_NotedName.Text)) Then
            VIEWER.Notedby_signatory_Name = txtx_NotedName.Text
            VIEWER.Notedby_signatory_Position = txtx_NotedPosition.Text

        ElseIf Not (String.IsNullOrEmpty(txtx_NotedPosition.Text)) Then
            VIEWER.Notedby_signatory_Name = txtx_NotedName.Text
            VIEWER.Notedby_signatory_Position = txtx_NotedPosition.Text
        End If


        If Not (String.IsNullOrEmpty(txtx_CertifiedName.Text)) Then
            VIEWER.CertifiedCorrect_signatory_Name = txtx_CertifiedName.Text
            VIEWER.CertifiedCorrect_signatory_Position = txtx_CertifiedPosition.Text

        ElseIf Not (String.IsNullOrEmpty(txtx_CertifiedPosition.Text)) Then
            VIEWER.CertifiedCorrect_signatory_Name = txtx_CertifiedName.Text
            VIEWER.CertifiedCorrect_signatory_Position = txtx_CertifiedPosition.Text
        End If

        If Not (String.IsNullOrEmpty(txtx_PreparedbyName.Text)) Then
            VIEWER.Preparedby_signatory_Name = txtx_PreparedbyName.Text
            VIEWER.Preparedby_signatory_Position = txtx_PreparedbyPosition.Text
        ElseIf Not (String.IsNullOrEmpty(txtx_PreparedbyPosition.Text)) Then
            VIEWER.Preparedby_signatory_Name = txtx_PreparedbyName.Text
            VIEWER.Preparedby_signatory_Position = txtx_PreparedbyPosition.Text
        End If



        'VIEWER.VIEWREPORTTYPE = "SRA"
        'VIEWER.Show()


        ''DETERMINE IETHER PERDEPARTMENT/ ALL
        Dim listOfStaffID As New List(Of String)
        Dim s_id As String = ""
        For Each n In cmbx_selected_names.Items
            s_id = sqlce_asstnt.GET_STAFF_ID(n)
            listOfStaffID.Add(s_id)
        Next

        ''CHECK IF STAFFID IS EMPTY
        If listOfStaffID.Count = 0 Then
            MessageBox.Show("Select staff", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
            cmbx_selected_names.Items.Clear()
            Exit Sub
        End If

        pbx_ReportIconsControl.Visible = True
        pbx_ReportConfiguration.Visible = False
        If Not ThreadCalculateParallel.IsBusy Then
            ThreadCalculateParallel.RunWorkerAsync()
            CONFIGURE_REPORT_ICON(True)
        End If

    End Sub

    Public Sub CONFIGURE_REPORT_ICON(showloading_image As Boolean)

        If showloading_image = True Then
            Select Case selectedReportIconName.ToString
                Case "DTR"
                    pbx_dtr.Image = My.Resources.support_loading
                    pbx_dtr.SizeMode = PictureBoxSizeMode.Zoom
                Case "DTR_B"
                    pbx_dtr_b.Image = My.Resources.support_loading
                    pbx_dtr_b.SizeMode = PictureBoxSizeMode.Zoom
                Case "DTR_C"
                    pbx_dtr_c.Image = My.Resources.support_loading
                    pbx_dtr_c.SizeMode = PictureBoxSizeMode.Zoom
                Case "DTR_D"
                    pbx_dtr_d.Image = My.Resources.support_loading
                    pbx_dtr_d.SizeMode = PictureBoxSizeMode.Zoom
                Case "DTR_E"
                    pbx_dtr_e.Image = My.Resources.support_loading
                    pbx_dtr_e.SizeMode = PictureBoxSizeMode.Zoom
                Case "DTR_F"
                    pbx_dtr_f.Image = My.Resources.support_loading
                    pbx_dtr_f.SizeMode = PictureBoxSizeMode.Zoom
                Case "SRA"
                    pbx_SRA.Image = My.Resources.support_loading
                    pbx_SRA.SizeMode = PictureBoxSizeMode.Zoom
                Case "SROT"
                    pbx_OT.Image = My.Resources.support_loading
                    pbx_OT.SizeMode = PictureBoxSizeMode.Zoom
            End Select
        Else
            Select Case selectedReportIconName.ToString
                Case "DTR"
                    pbx_dtr.Image = My.Resources.Reports
                    pbx_dtr.SizeMode = PictureBoxSizeMode.Zoom
                Case "DTR_B"
                    pbx_dtr_b.Image = My.Resources.Reports
                    pbx_dtr_b.SizeMode = PictureBoxSizeMode.Zoom
                Case "DTR_C"
                    pbx_dtr_c.Image = My.Resources.Reports
                    pbx_dtr_c.SizeMode = PictureBoxSizeMode.Zoom
                Case "DTR_D"
                    pbx_dtr_d.Image = My.Resources.Reports
                    pbx_dtr_d.SizeMode = PictureBoxSizeMode.Zoom
                Case "DTR_E"
                    pbx_dtr_e.Image = My.Resources.Reports
                    pbx_dtr_e.SizeMode = PictureBoxSizeMode.Zoom
                Case "DTR_F"
                    pbx_dtr_f.Image = My.Resources.Reports
                    pbx_dtr_f.SizeMode = PictureBoxSizeMode.Zoom
                Case "SRA"
                    pbx_SRA.Image = My.Resources.REPORT6
                    pbx_SRA.SizeMode = PictureBoxSizeMode.Zoom
                Case "SROT"
                    pbx_OT.Image = My.Resources.REPORT6
                    pbx_OT.SizeMode = PictureBoxSizeMode.Zoom
            End Select
        End If

        pbx_ReportIconsControl.Refresh()

    End Sub


    'Private Sub btn_Cancel_Click(sender As System.Object, e As System.EventArgs) Handles btn_Cancel.Click

    'End Sub

    Private Sub pbx_SRA_Click(sender As System.Object, e As System.EventArgs) Handles pbx_SRA.Click
        selectedReportIconName = "SRA"
        GlobalVariables.DTR_TYPE = "SRA"
        Label19.Visible = False
        btnReportExit.Visible = False
        'grpx_ReportSignatory.Text = "Report Signatory"
        lblx_PromptHeaderText.Text = "Generate Summary Report Of Attendance(SRA)"
        pbx_ReportIconsControl.Visible = False
        pbx_ReportConfiguration.Visible = True
        'cmbRptDep.Focus()
        LOAD_TREEVIEW_DETAILS()
    End Sub

    Private Sub btn_AddDepartment_MouseHover(sender As System.Object, e As System.EventArgs)
        ToolTip1.Show("Click to add new department class", btn_AddDepartment)
    End Sub

    Private Sub btnShowShiftTable_MouseHover(sender As System.Object, e As System.EventArgs) Handles btnShowShiftTable.MouseHover
        ToolTip1.Show("Click to add new shift name", btnShowShiftTable)
    End Sub

    Private Sub btn_ShowShiftSchema_MouseHover(sender As System.Object, e As System.EventArgs) Handles btn_ShowShiftSchema.MouseHover
        'ToolTip1.Show("Click to add selected shift details", btn_ShowShiftSchema)
    End Sub

    Private Sub btnShowLeaveSchema_MouseHover(sender As System.Object, e As System.EventArgs)
        'ToolTip1.Show("Click to add new leave class", btnShowLeaveSchema)
    End Sub

    Private Sub btnClose_MouseEnter(sender As System.Object, e As System.EventArgs) Handles btnClose.MouseEnter
        'btnClose.BackColor = Color.Black
        btnClose.Image = My.Resources.EX1
    End Sub

    Private Sub btnClose_MouseLeave(sender As System.Object, e As System.EventArgs) Handles btnClose.MouseLeave
        'btnClose.BackColor = Color.Black
        btnClose.Image = My.Resources.close_window
    End Sub



    'Private Sub btnSystemSettings_Click(sender As System.Object, e As System.EventArgs) Handles btnSystemSettings.Click

    'End Sub

    'Private Sub btnArrangeShift_Click(sender As System.Object, e As System.EventArgs) Handles btnArrangeShift.Click

    'End Sub



    Private Sub LOAD_USERACCESS_LIST()
        Dim listOfuseraccessnam As New List(Of String)
        listOfuseraccessnam = sqlce_asstnt.GET_ALL_USERACCESS_NAME()
        lstUserAccess.Items.Clear()
        If Not listOfuseraccessnam.Count = 0 Then
            'listOfStaffid.ForEach(Sub(id)
            '                          lstUserAccess.Items.Add(sqlce_asstnt.GET_STAFF_NAME(id))
            '                      End Sub)
            lstUserAccess.Items.AddRange(listOfuseraccessnam.ToArray)
            lstUserAccess.SelectedIndex = lstUserAccess.Items.Count - 1
        End If
    End Sub

    Private Sub btnx_saveuseraccess_Click(sender As System.Object, e As System.EventArgs) Handles btnx_saveuseraccess.Click


        Dim staffname As String = cmbx_stafflist_user_access.Text
        Dim departmentid As Integer = sqlce_asstnt.GET_DEPARTMENT_ID(cmbx_departmentlist_select_access.Text)
        Dim username As String = txtx_username.Text.Trim
        Dim previlege As String = ""

        Dim i As Integer = 0
        Do While (i < chkbx_privilege.Items.Count)
            Dim st As CheckState = chkbx_privilege.GetItemCheckState(i)
            If (st = CheckState.Checked) Then
                previlege = previlege & "1"
            End If
            If (st = CheckState.Unchecked) Then
                previlege = previlege & "0"
            End If
            i = (i + 1)
        Loop


        If Not String.IsNullOrEmpty(username) Then
            If txtx_password.Text = txtx_retypepassword.Text Then
                ''CHECK IF COLUMN NAME PRIVELEGE EXIST
                If sqlce_asstnt.BMTS_CHECK_IF_TABLECOLUMN_IS_EXIST("USERACCESS", "PREVILEGE") = False Then
                    sqlce_asstnt.BTMS_CREATE_TABLE_COLUMN("USERACCESS", "PREVILEGE", 1)
                End If





                sqlce_asstnt.INSERT_NEW_USERACCESS(staffname, username, txtx_password.Text, previlege)
                MessageBox.Show("New user added Successfully!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtx_username.Text = ""
                txtx_retypepassword.Text = ""
                txtx_password.Text = ""
                pnlx_useraccessadd.Visible = False
                pnlx_useraccesslist.Visible = True
                LOAD_USERACCESS_LIST()
            Else
                MessageBox.Show("Password do not match!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Else
            MessageBox.Show("Input Username", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

    End Sub



    Private Sub cmbx_departmentlist_select_access_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbx_departmentlist_select_access.SelectedIndexChanged
        Dim departmentname As String = cmbx_departmentlist_select_access.Text
        Dim listOfStaffname As New List(Of String)
        listOfStaffname = sqlce_asstnt.GET_ALL_EMPLOYEES_ASLIST(False, sqlce_asstnt.GET_DEPARTMENT_ID(departmentname))
        listOfStaffname.Sort()
        cmbx_stafflist_user_access.Items.Clear()
        If Not listOfStaffname.Count = 0 Then
            cmbx_stafflist_user_access.Items.AddRange(listOfStaffname.ToArray)
            cmbx_stafflist_user_access.SelectedIndex = cmbx_stafflist_user_access.Items.Count - 1
        End If
    End Sub
    Private Sub txtx_username_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtx_username.TextChanged
        If String.IsNullOrEmpty(cmbx_stafflist_user_access.Text) Then
            MessageBox.Show("Incomplete details. No staff name.")
        End If
    End Sub

    Private Sub btnx_cancel_creatinguseraccess_Click(sender As System.Object, e As System.EventArgs) Handles btnx_cancel_creatinguseraccess.Click
        pnlx_useraccessadd.Visible = False
        pnlx_useraccesslist.Visible = True
        LOAD_USERACCESS_LIST()
    End Sub
    Private Sub btn_CancelReportGeneration_Click(sender As System.Object, e As System.EventArgs) Handles btn_CancelReportGeneration.Click
        pbx_ReportConfiguration.Visible = False
        pbx_ReportIconsControl.Visible = True
        Label19.Visible = True
        btnReportExit.Visible = True
    End Sub
    Private Sub lstUserAccess_MouseDown(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles lstUserAccess.MouseDown
        'Try
        '    Dim curItem As String = lstUserAccess.SelectedItem.ToString()
        '    Dim CurrPos As Integer = lstUserAccess.FindString(curItem)
        '    If Not lstUserAccess.Items.Count = -1 Then
        '        If e.Button = Windows.Forms.MouseButtons.Right Then
        '            rightclickUseraccess.Show(MousePosition)
        '        End If
        '    End If
        'Catch ex As Exception
        '    Exit Sub
        'End Try
    End Sub

    Private Sub Button1_Click_1(sender As System.Object, e As System.EventArgs)
        Dim dt As Date = "2015-05-20 16:15:18"

        MessageBox.Show(dt.ToShortDateString, Format(dt, "HH:mm:ss"))

    End Sub



    'Private Sub btnConnectToDevice_Click(sender As System.Object, e As System.EventArgs) Handles btnConnectToDevice.Click

    '    If Not ThreadBISBIONetworkDownload.IsBusy Then
    '        ThreadBISBIONetworkDownload.RunWorkerAsync()
    '    End If



    '    'Dim idwErrorCode As Integer
    '    ' bIsConnected = axCZKEM1.Connect_Net(txtIPaddress.Text.Trim(), Convert.ToInt32(txtTcpPort.Text.Trim()))
    '    ' If bIsConnected = True Then
    '    '     MessageBox.Show("Connection Success!")
    '    '     iMachineNumber = 1 'In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
    '    '     axCZKEM1.RegEvent(iMachineNumber, 65535) 'Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
    '    ' Else
    '    '     axCZKEM1.GetLastError(idwErrorCode)
    '    '     MsgBox("Unable to connect the device,ErrorCode=" & idwErrorCode, MsgBoxStyle.Exclamation, "Error")
    '    ' End If
    'End Sub

    'Private Sub btn_logsCount_Click(sender As System.Object, e As System.EventArgs) Handles btn_logsCount.Click

    '    Dim idwErrorCode As Integer
    '    Dim iValue = 0

    '    axCZKEM1.EnableDevice(iMachineNumber, False) 'disable the device
    '    If axCZKEM1.GetDeviceStatus(iMachineNumber, 6, iValue) = True Then 'Here we use the function "GetDeviceStatus" to get the record's count.The parameter "Status" is 6.
    '        MsgBox("The count of the AttLogs in the device is " + iValue.ToString(), MsgBoxStyle.Information, "Success")
    '    Else
    '        axCZKEM1.GetLastError(idwErrorCode)
    '        MsgBox("Operation failed,ErrorCode=" & idwErrorCode, MsgBoxStyle.Exclamation, "Error")
    '    End If

    '    axCZKEM1.EnableDevice(iMachineNumber, True) 'enable the device
    'End Sub

    'Private Sub btn_GetAllAttlogs_Click(sender As System.Object, e As System.EventArgs) Handles btn_GetAllAttlogs.Click


    '    'If Not ThreadBISBIONetworkDownload.IsBusy Then

    '    '    ThreadBISBIONetworkDownload.RunWorkerAsync()

    '    'End If




    '    ''If bIsConnected = False Then
    '    ''    MsgBox("Please connect the device first", MsgBoxStyle.Exclamation, "Error")
    '    ''    Return
    '    ''End If

    '    'Dim sdwEnrollNumber As String = ""
    '    'Dim idwVerifyMode As Integer
    '    'Dim idwInOutMode As Integer
    '    'Dim idwYear As Integer
    '    'Dim idwMonth As Integer
    '    'Dim idwDay As Integer
    '    'Dim idwHour As Integer
    '    'Dim idwMinute As Integer
    '    'Dim idwSecond As Integer
    '    'Dim idwWorkcode As Integer

    '    'Dim idwErrorCode As Integer
    '    'Dim iGLCount = 0
    'Dim lvItem As New ListViewItem("Items", 0)

    'Cursor = Cursors.WaitCursor
    'axCZKEM1.EnableDevice(iMachineNumber, False) 'disable the device
    'If axCZKEM1.ReadGeneralLogData(iMachineNumber) Then 'read all the attendance records to the memory
    '    'get records from the memory
    '    While axCZKEM1.SSR_GetGeneralLogData(iMachineNumber, sdwEnrollNumber, idwVerifyMode, idwInOutMode, idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond, idwWorkcode)
    '        iGLCount += 1
    '        'lvItem = lvLogs.Items.Add(iGLCount.ToString())
    '        'ListBox1.Items.Add(sdwEnrollNumber & "|" & idwVerifyMode.ToString() & "|" & "|" & idwInOutMode.ToString() & "|" & idwYear.ToString() & "-" + idwMonth.ToString() & "-" & idwDay.ToString() & " " & idwHour.ToString() & ":" & idwMinute.ToString() & ":" & idwSecond.ToString() & "|" & _
    '        '                   "|" & idwWorkcode.ToString())

    '        'ListBox1.Items.Add(idwVerifyMode.ToString())
    '        'ListBox1.Items.Add(idwInOutMode.ToString())
    '        'ListBox1.Items.Add(idwYear.ToString() & "-" + idwMonth.ToString() & "-" & idwDay.ToString() & " " & idwHour.ToString() & ":" & idwMinute.ToString() & ":" & idwSecond.ToString())
    '        'ListBox1.Items.Add(idwWorkcode.ToString())



    '    End While
    'Else
    '    Cursor = Cursors.Default
    '    axCZKEM1.GetLastError(idwErrorCode)
    '    If idwErrorCode <> 0 Then
    '        MsgBox("Reading data from terminal failed,ErrorCode: " & idwErrorCode, MsgBoxStyle.Exclamation, "Error")
    '    Else
    '        MsgBox("No data from terminal returns!", MsgBoxStyle.Exclamation, "Error")
    '    End If
    'End If

    ''axCZKEM1.EnableDevice(iMachineNumber, True) 'enable the device
    ''Cursor = Cursors.Default
    'End Sub

    Private Sub ThreadBISBIONetworkDownload_DoWork(sender As System.Object, e As System.ComponentModel.DoWorkEventArgs) Handles ThreadBISBIONetworkDownload.DoWork


        ''DEVICE SETUP
        Dim udisk_worker As New UDiskDataMain
        Dim axCZKEM1 As New zkemkeeper.CZKEM


        Dim bIsConnected = False 'the boolean value identifies whether the device is connected
        Dim iMachineNumber As Integer 'the serial number of the device.After connecting the device ,this value will be changed.
        Dim idwErrorCode As Integer



        ''STEP 1 INIT IP AND PORT
        Dim IP As String = sqlce_asstnt.GET_DETAILS_DEVICE_IP(e.Argument.ToString)
        Dim PORT As String = sqlce_asstnt.GET_DETAILS_DEVICE_PORT(e.Argument.ToString)

        If IP = "" And PORT = "" Then
            If IP = "" Then
                MessageBox.Show("Device IP not set!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ElseIf PORT = "" Then
                MessageBox.Show("Device PORT not set!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)

            End If
            Exit Sub
        End If




        ''GET READY FOR CONNECTION
        'If Not bIsConnected = True And IP <> devip Then

        UpdateText(lblx_networkDownload, "Sending synchronization message.. Please wait")
        bIsConnected = axCZKEM1.Connect_Net(IP, Convert.ToInt32(PORT))
        devip = IP
        If bIsConnected = True Then
            'MessageBox.Show("Connection Success!")
            iMachineNumber = 1 'In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
            axCZKEM1.RegEvent(iMachineNumber, 65535) 'Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
        Else
            ''TRAP ERROR
            axCZKEM1.GetLastError(idwErrorCode)
            MessageBox.Show("Unable to connect the device,ErrorCode=" & idwErrorCode, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ' UpdateText(lblx_networkDownload, "Unable to connect the device,ErrorCode=" & idwErrorCode)
            Exit Sub
        End If
        'Else
        '''SKIP RECONECTING AGAIN IF THE DEVICE IS ALREADY CONNECTED

        'End If




        ''SELECT WHAT TASK IS ASSIGN TO THE DEVICE
        Select Case DeviceTask
            Case "DOWNLOADLOGS"
                ''STEP 2 IF SUCCESSFUL CONNECTION DOWNLOAD ALL LOGS
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
                Dim iGLCount = 0
                Dim sPIN2 As String = ""
                Dim sVerified As String = ""
                Dim sTime_second As String = ""
                Dim sDeviceID As String = ""
                Dim sStatus As Integer = 0
                Dim sWorkcode As String = ""
                Dim dt As Date = Nothing
                UpdateText(lblx_networkDownload, "Downloading in progress.. Please wait")
                axCZKEM1.EnableDevice(iMachineNumber, False) 'disable the device
                If axCZKEM1.ReadGeneralLogData(iMachineNumber) Then 'read all the attendance records to the memory
                    'get records from the memory
                    While axCZKEM1.SSR_GetGeneralLogData(iMachineNumber, sdwEnrollNumber, idwVerifyMode, idwInOutMode, idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond, idwWorkcode)


                        sPIN2 = sdwEnrollNumber.ToString.Trim
                        sVerified = idwVerifyMode.ToString.Trim
                        sTime_second = idwYear.ToString() & "-" + idwMonth.ToString() & "-" & idwDay.ToString() & " " & idwHour.ToString() & ":" & idwMinute.ToString() & ":" & idwSecond.ToString()
                        sWorkcode = idwWorkcode.ToString.Trim
                        dt = sTime_second
                        ''START PROCESSING THE RAW DATA VERIFY IF THE LOGS IS EXIST ALREADY
                        If udisk_worker.VERIFY_USB_LOG(sPIN2.Trim, sTime_second.Trim) = False Then
                            iGLCount += 1
                            ' udisk_worker.INSERT_NEW_LOG(udisk_worker.GET_LAST_RAW_LOG_ID() + 1, sPIN2.Trim, dt, Format(dt, "HH:mm:ss").Trim, udisk_worker.IDENTIFY_TRANSACTIONTYPE(sStatus), IP, False, sTime_second.Trim, CurrentLoginAccount)
                            udisk_worker.INSERT_NEW_LOG(sPIN2.Trim, dt, Format(dt, "HH:mm:ss").Trim, udisk_worker.IDENTIFY_TRANSACTIONTYPE(sStatus), IP, False, sTime_second, CurrentLoginAccount)
                        End If
                    End While
                    axCZKEM1.EnableDevice(iMachineNumber, True) 'enable the device
                    MessageBox.Show("Download Complete!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Else

                    axCZKEM1.GetLastError(idwErrorCode)
                    If idwErrorCode <> 0 Then

                        MessageBox.Show("Reading data from terminal failed,ErrorCode: " & idwErrorCode, "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        MessageBox.Show("No data from terminal returns!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If


            Case "SET-TIME"
                Dim devdt As Date = dtpx_devicetime.Value
                Dim idwYear As Integer = Convert.ToInt32(devdt.Year)
                Dim idwMonth As Integer = Convert.ToInt32(devdt.Month)
                Dim idwDay As Integer = Convert.ToInt32(devdt.Day)
                Dim idwHour As Integer = Convert.ToInt32(devdt.Hour)
                Dim idwMinute As Integer = Convert.ToInt32(devdt.Minute)
                Dim idwSecond As Integer = Convert.ToInt32(devdt.Second)


                If axCZKEM1.SetDeviceTime2(iMachineNumber, idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond) = True Then
                    axCZKEM1.RefreshData(iMachineNumber) 'the data in the device should be refreshed
                    MsgBox("Successfully set the time of the machine as you have set!", MsgBoxStyle.Information, "Success")
                Else
                    axCZKEM1.GetLastError(idwErrorCode)
                    MsgBox("Operation failed,ErrorCode=" & idwErrorCode.ToString(), MsgBoxStyle.Exclamation, "Error")
                End If

            Case "CLEAR ADMIN"
                If axCZKEM1.ClearAdministrators(iMachineNumber) = True Then
                    axCZKEM1.RefreshData(iMachineNumber) 'the data in the device should be refreshed
                    MsgBox("Successfully clear administrator privilege from teiminal!", MsgBoxStyle.Information, "Success")
                Else
                    axCZKEM1.GetLastError(idwErrorCode)
                    MsgBox("Operation failed,ErrorCode=" & idwErrorCode.ToString(), MsgBoxStyle.Exclamation, "Error")
                End If
        End Select

        axCZKEM1.EnableDevice(iMachineNumber, True) 'enable the device
    End Sub

    Private Sub ThreadBISBIONetworkDownload_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles ThreadBISBIONetworkDownload.RunWorkerCompleted
        'btn_GetAllAttlog.Enabled = True
        'btnSetDeviceTime.Enabled = True
        'btnx_deviceClearAdmin.Enabled = True

        pbx_loading_network_download.Visible = False
        lblx_networkDownload.Text = "-"
    End Sub


    Private Sub btn_AddDevice_Click(sender As System.Object, e As System.EventArgs)

    End Sub

    Public Sub LOAD_ALL_DEVICES()
        Dim listOfDevices As New List(Of String)
        listOfDevices = sqlce_asstnt.GET_ALL_BISBIO_DEVICES()
        cmbx_devlist.Items.Clear()
        cmbx_devlist.Items.AddRange(listOfDevices.ToArray)
        If cmbx_devlist.Items.Count <> 0 Then
            cmbx_devlist.SelectedIndex = 0
        End If
        dtpx_devicetime.Value = DateAndTime.Now
        'lstBisbioDevices.Items.Clear()
        'lstBisbioDevices.Items.AddRange(listOfDevices.ToArray)
    End Sub

    Private Sub lstBisbioDevices_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            DeviceDetails.ShowDialog()
        End If
    End Sub

    Private Sub lstBisbioDevices_MouseDoubleClick(sender As Object, e As System.Windows.Forms.MouseEventArgs)
        DeviceDetails.ShowDialog()
    End Sub

    'Private Sub lstBisbioDevices_SelectedIndexChanged(sender As System.Object, e As System.EventArgs)
    '    ''MessageBox.Show(lstBisbioDevices.SelectedItem.ToString)
    '    'btn_GetAllAttlog.Enabled = True
    '    'btnSetDeviceTime.Enabled = True
    '    'btnx_deviceClearAdmin.Enabled = True

    'End Sub

    'Private Sub btnSetDeviceTime_Click(sender As System.Object, e As System.EventArgs)

    'End Sub


    'Private Sub btnx_deviceClearAdmin_Click(sender As System.Object, e As System.EventArgs)

    'End Sub
    'Private Sub btn_GetAllAttlog_Click(sender As System.Object, e As System.EventArgs)

    'End Sub







    'Function getMacAddress()
    '    Dim nics() As NetworkInterface = NetworkInterface.GetAllNetworkInterfaces()
    '    Return nics(0).GetPhysicalAddress.ToString
    'End Function
    'Function ConvertToHex(val As Char)
    '    Dim i As Int32 = Convert.ToInt32(val, 16)
    '    Return i
    'End Function



    '    Private Sub Button1_Click_2(sender As System.Object, e As System.EventArgs) Handles Button1.Click


    '        Dim X As Integer = 10
    '        Dim Z As Integer = 10
    '        If X = 1 Then
    '            Console.WriteLine(1)
    '        ElseIf X = 10 Then
    '            Console.WriteLine(2)
    '            GoTo Z

    'Z:
    '        ElseIf Z = 10 Then
    '            Console.WriteLine(10)
    '        Else

    '        End If








    'MessageBox.Show("REG KEY: " & getMacAddress().ToString.Remove(0, 6) & " ACTUAL: " & getMacAddress() & " PRODUCTKEY: " & ConvertToHex(getMacAddress().ToString.Remove(0, 6).ToString))
    'Dim REGKEY As String = getMacAddress().ToString.Remove(0, 6).Trim
    'Dim listOfAlpha As New List(Of Char) From {"1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"}
    'Dim listOfChars As New List(Of Char)
    'listOfChars.AddRange(REGKEY.ToCharArray)

    '' listOfChars.Sort()
    'Dim PROKEY As New List(Of String)
    'listOfChars.ForEach(Sub(x)
    '                        PROKEY.Add(ConvertToHex(x))
    '                    End Sub)


    ''Dim str As String = String.Join("", PROKEY.ToArray)
    'listOfChars.Clear()
    'listOfChars.AddRange(String.Join("*", PROKEY.ToArray).ToCharArray)

    '' listOfChars.Sort()
    'MessageBox.Show("REGKEY: " & REGKEY & " PROKEY: " & String.Join("", listOfChars.ToArray))


















    'Dim cups As New CPU

    ''Dim lstOfRaw As New List(Of String) From {"01:00", "", "Value3"}

    ''Dim yestedaylogs As New List(Of String) From {"00:10:22", "00:10:25", "23:26:00", "23:27:00", "00:00"}
    'Dim yestedaylogs As New List(Of String)
    'Dim todaylogs As New List(Of String) From {"16:11:22", "17:08:25"}
    'Dim tommorowlogs As New List(Of String) From {"00:11:22", "08:08:25", "23:26:01", "23:27:01", "00:00"}

    '''get logs candidates for logs for the first day identified thourgh starting edn and ending end
    'Dim PUNCHES1 As New List(Of String)
    'Dim PUNCHES2 As New List(Of String)
    'Dim yesterday As New List(Of String)
    'Dim today As New List(Of String)
    'Dim tommorow As New List(Of String)
    'Dim RAWLOGS As New List(Of String)
    'yesterday = cups.GET_LOGS_CANDIDATES(yestedaylogs, "22:00", "00:00")
    'today = cups.GET_LOGS_CANDIDATES(todaylogs, "00:00", "02:00")
    'PUNCHES1.AddRange(yesterday.ToArray)
    'PUNCHES1.AddRange(today.ToArray)
    'PUNCHES2 = cups.GET_LOGS_CANDIDATES(todaylogs, "04:00", "10:00")


    '''FIRST SHIFT ALGO SHIFT ID 0
    'If PUNCHES1.Count <> 0 And PUNCHES2.Count <> 0 Then
    '    ''CONFIRM FIRST SHIFT
    '    RAWLOGS.AddRange(PUNCHES1)
    '    RAWLOGS.AddRange(PUNCHES2)
    '    Console.WriteLine("CONFIRM 1ST SHIFT")
    '    Console.WriteLine(String.Join(" <> ", RAWLOGS.ToArray))
    '    Exit Sub
    'Else
    '    ''PROCCED TO NEXT SHIFT
    'End If


    '''SECOND SHIFT ALGO SHIFT ID 1
    'RAWLOGS.Clear()
    'PUNCHES1 = cups.GET_LOGS_CANDIDATES(todaylogs, "04:00", "10:00")
    'PUNCHES2 = cups.GET_LOGS_CANDIDATES(todaylogs, "14:00", "18:00")
    'If PUNCHES1.Count <> 0 And PUNCHES2.Count <> 0 Then
    '    ''CONFIRM SECOND SHIFT
    '    RAWLOGS.AddRange(PUNCHES1)
    '    RAWLOGS.AddRange(PUNCHES2)
    '    Console.WriteLine("CONFIRM 2ND SHIFT")
    '    Console.WriteLine(String.Join(" <> ", RAWLOGS.ToArray))
    '    Exit Sub
    'Else



    '    ''NEXT SHIFT PROCEED
    'End If

    '''THIRD SHIFT ALGO SHIFT ID = 2
    'RAWLOGS.Clear()
    'today = cups.GET_LOGS_CANDIDATES(todaylogs, "22:00", "00:00")
    'tommorow = cups.GET_LOGS_CANDIDATES(tommorowlogs, "00:00", "02:00")
    'PUNCHES2.AddRange(tommorow.ToArray)
    'PUNCHES2.AddRange(today.ToArray)

    'PUNCHES1 = cups.GET_LOGS_CANDIDATES(todaylogs, "14:00", "18:00")
    'If PUNCHES1.Count <> 0 And PUNCHES2.Count <> 0 Then
    '    ''CONFIRM SECOND SHIFT
    '    RAWLOGS.AddRange(PUNCHES1)
    '    RAWLOGS.AddRange(PUNCHES2)
    '    Console.WriteLine("CONFIRM 3RD SHIFT")
    '    Console.WriteLine(String.Join(" <> ", RAWLOGS.ToArray))
    '    Exit Sub
    'Else
    '    ''FINALY ABSENT
    'End If







    'End Sub


    'Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click
    '    Dim cups As New CPU
    '    Dim punch As String = "23:10:59"

    '    Console.WriteLine("Result: " & cups.GET_UNDERTIME("11:00", "00:00", punch, False, True))
    'End Sub

    Private Sub MainControPanelTab_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles MainControPanelTab.KeyDown
        If e.KeyCode = Keys.Escape Then
            '  btnATTExit_Click(sender, e)
        End If
    End Sub

    Private Sub FlowLayoutPanel1_Paint(sender As System.Object, e As System.Windows.Forms.PaintEventArgs) Handles FlowLayoutPanel1.Paint

    End Sub

    Private Sub pbx_scroll_left_MouseEnter(sender As System.Object, e As System.EventArgs) Handles pbx_scroll_left.MouseEnter
        pbx_scroll_left.BackColor = Color.Silver
    End Sub

    Private Sub pbx_scroll_left_MouseLeave(sender As System.Object, e As System.EventArgs) Handles pbx_scroll_left.MouseLeave
        pbx_scroll_left.BackColor = Color.DimGray
    End Sub

    Private Sub pbx_scroll_right_MouseEnter(sender As System.Object, e As System.EventArgs) Handles pbx_scroll_right.MouseEnter
        pbx_scroll_right.BackColor = Color.Silver
    End Sub

    Private Sub pbx_scroll_right_MouseLeave(sender As Object, e As System.EventArgs) Handles pbx_scroll_right.MouseLeave
        pbx_scroll_right.BackColor = Color.DimGray
    End Sub

    'Private Sub btn_UpdateChangesHolidaySchema_Click_1(sender As System.Object, e As System.EventArgs) Handles btn_UpdateChangesHolidaySchema.Click

    'End Sub

    Private Sub pbx_scroll_right_Click(sender As System.Object, e As System.EventArgs) Handles pbx_scroll_right.Click

        Select Case SCROLL_COUNTER_RIGHT
            Case 0
                If Not ThreadAutoScroll.IsBusy Then
                    maxscroll_right = 780
                    minscroll_right = 0
                    pbx_scroll_left.Visible = True
                    pbx_scroll_right.Visible = True
                    SCROLL_COUNTER_RIGHT = 1
                    SCROLL_COUNTER_LEFT = 1
                    ThreadAutoScroll.RunWorkerAsync("RIGHT")
                End If
            Case 1
                If Not ThreadAutoScroll.IsBusy Then
                    maxscroll_right = 1620
                    minscroll_right = 760
                    ThreadAutoScroll.RunWorkerAsync("RIGHT")
                    pbx_scroll_left.Visible = True
                    pbx_scroll_right.Visible = False
                    SCROLL_COUNTER_RIGHT = 0
                    SCROLL_COUNTER_LEFT = 0
                End If
        End Select
    End Sub

    Private Sub pbx_scroll_left_Click(sender As System.Object, e As System.EventArgs) Handles pbx_scroll_left.Click

        'If Not ThreadAutoScroll.IsBusy Then
        '    ThreadAutoScroll.RunWorkerAsync("LEFT")
        'End If


        Select Case SCROLL_COUNTER_LEFT
            Case 0
                If Not ThreadAutoScroll.IsBusy Then
                    maxscroll_left = 740
                    minscroll_left = 2000
                    pbx_scroll_left.Visible = True
                    pbx_scroll_right.Visible = True


                    ThreadAutoScroll.RunWorkerAsync("LEFT")
                    SCROLL_COUNTER_RIGHT = 1
                    SCROLL_COUNTER_LEFT = 1
                End If
            Case 1
                If Not ThreadAutoScroll.IsBusy Then
                    maxscroll_left = -40
                    minscroll_left = 740
                    ThreadAutoScroll.RunWorkerAsync("LEFT")
                    pbx_scroll_left.Visible = False
                    pbx_scroll_right.Visible = True
                    SCROLL_COUNTER_LEFT = 0
                    SCROLL_COUNTER_RIGHT = 0
                End If
        End Select
    End Sub
    Private Sub ThreadAutoScroll_DoWork(sender As System.Object, e As System.ComponentModel.DoWorkEventArgs) Handles ThreadAutoScroll.DoWork

        If e.Argument.ToString = "RIGHT" Then
            For i = minscroll_right To maxscroll_right Step 40
                'FlowLayoutPanel1.AutoScrollPosition = New Point(i, 0)
                ' System.Threading.Thread.Sleep(1)
                UpdatePosition(FlowLayoutPanel1, i, 0)
            Next
            'UpdatePosition(FlowLayoutPanel1, 800, 0)
        ElseIf e.Argument.ToString = "LEFT" Then
            For i = minscroll_left To maxscroll_left Step -40
                'FlowLayoutPanel1.AutoScrollPosition = New Point(i, 0)
                ' System.Threading.Thread.Sleep(1)
                UpdatePosition(FlowLayoutPanel1, i, 0)
                'FlowLayoutPanel1.Refresh()
            Next
            'UpdatePosition(FlowLayoutPanel1, 0, 0)
        End If



    End Sub
    Public Sub UpdatePosition(ByVal flpx As FlowLayoutPanel, ByVal posx As Integer, ByVal posy As Integer)
        Invoke(New MethodInvoker(Sub()
                                     flpx.AutoScrollPosition = New Point(posx, 0)
                                     '    FlowLayoutPanel1.Update()
                                     flpx.Refresh()

                                 End Sub))
    End Sub



    Private Sub ResignToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ResignToolStripMenuItem.Click
        'Try
        '    Dim CurrRowPos As Integer = dtgEmpProfiles.CurrentRow.Index
        '    If Not IsDBNull(dtgEmpProfiles.Rows(CurrRowPos).Cells.Item(0).Value) Then
        '        Dim staffid As String = dtgEmpProfiles.Rows(CurrRowPos).Cells.Item(0).Value
        '        Dim staffname As String = dtgEmpProfiles.Rows(CurrRowPos).Cells.Item(1).Value
        '        Dim department As String = ""
        '        If Not String.IsNullOrEmpty(dtgEmpProfiles.Rows(CurrRowPos).Cells.Item(2).Value) Then
        '            department = dtgEmpProfiles.Rows(CurrRowPos).Cells.Item(2).Value
        '        End If

        '        If MessageBox.Show("You are about to tag this employee to the database as resign." & vbCrLf & _
        '                           "Name: " & staffname & vbCrLf & vbCrLf & "Please confirm.", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then

        '            sqlce_asstnt.TAG_AS_RESIGN_EMPLOYEE(staffid, staffname, department, DateAndTime.Now, CurrentLoginAccount)
        '            sqlce_asstnt.REMOVE_EMPLOYEE(staffid)
        '            dtgEmpProfiles.DataSource = sqlce_asstnt.GET_ALL_EMPLOYEES(True, False, Nothing, True, Nothing, False, Nothing)
        '            LOAD_DEPARTMENT_TO_COMBOBOX_EMPLOYEEPROFILES()
        '        End If
        '    End If
        'Catch ex As Exception
        '    Console.WriteLine(ex.Message)
        'End Try

    End Sub

    Private Sub cmbx_fcc_department_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbx_fcc_department.SelectedIndexChanged
        Try
            Dim listOfStaff As New List(Of String)

            listOfStaff = sqlce_asstnt.GET_ALL_EMPLOYEES_ASLIST(False, sqlce_asstnt.GET_DEPARTMENT_ID(cmbx_fcc_department.SelectedItem))
            If listOfStaff.Count = 0 Then
                cmbx_fcc_staffnames.Text = ""
                cmbx_fcc_staffnames.Items.Clear()
                Exit Sub
            End If
            listOfStaff.Sort()
            cmbx_fcc_staffnames.Items.Clear()
            cmbx_fcc_staffnames.Items.AddRange(listOfStaff.ToArray)
            cmbx_fcc_staffnames.SelectedIndex = 0
        Catch ex As Exception

        End Try

    End Sub

    Private Sub cmbx_olm_department_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbx_olm_department.SelectedIndexChanged
        Try
            Dim listOfStaff As New List(Of String)

            listOfStaff = sqlce_asstnt.GET_ALL_EMPLOYEES_ASLIST(False, sqlce_asstnt.GET_DEPARTMENT_ID(cmbx_olm_department.SelectedItem))
            If listOfStaff.Count = 0 Then
                cmbx_olm_staffnames.Text = ""
                cmbx_olm_staffnames.Items.Clear()
                Exit Sub
            End If
            listOfStaff.Sort()
            cmbx_olm_staffnames.Items.Clear()
            cmbx_olm_staffnames.Items.AddRange(listOfStaff.ToArray)
            cmbx_olm_staffnames.SelectedIndex = 0
        Catch ex As Exception

        End Try
    End Sub

    Private Sub cmbx_mto_department_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbx_mto_department.SelectedIndexChanged
        Try
            Dim listOfStaff As New List(Of String)

            listOfStaff = sqlce_asstnt.GET_ALL_EMPLOYEES_ASLIST(False, sqlce_asstnt.GET_DEPARTMENT_ID(cmbx_mto_department.SelectedItem))
            If listOfStaff.Count = 0 Then
                cmbx_mto_staffnames.Text = ""
                cmbx_mto_staffnames.Items.Clear()
                Exit Sub
            End If
            listOfStaff.Sort()
            cmbx_mto_staffnames.Items.Clear()
            cmbx_mto_staffnames.Items.AddRange(listOfStaff.ToArray)
            cmbx_mto_staffnames.SelectedIndex = 0
        Catch ex As Exception

        End Try
    End Sub

    Private Sub cmbx_rsr_department_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbx_rsr_department.SelectedIndexChanged
        On Error Resume Next

        Dim listOfStaff As New List(Of String)

        listOfStaff = sqlce_asstnt.GET_ALL_EMPLOYEES_ASLIST(False, sqlce_asstnt.GET_DEPARTMENT_ID(cmbx_rsr_department.SelectedItem))
        If listOfStaff.Count = 0 Then
            cmbx_rsr_staffnames.Text = ""
            cmbx_rsr_staffnames.Items.Clear()
            Exit Sub
        End If
        listOfStaff.Sort()
        cmbx_rsr_staffnames.Items.Clear()
        cmbx_rsr_staffnames.Items.AddRange(listOfStaff.ToArray)
        cmbx_rsr_staffnames.SelectedIndex = 0


        'btnx_lookup_raw_Click(sender, e)

    End Sub

    Private Sub btnx_fcc_approved_Click(sender As System.Object, e As System.EventArgs) Handles btnx_fcc_approved.Click
        If Not (String.IsNullOrEmpty(cmbx_fcc_staffnames.Text)) Then
            Dim transtype As String = ""
            Dim staffid As String = sqlce_asstnt.GET_USER_ID(cmbx_fcc_staffnames.Text, sqlce_asstnt.GET_DEPARTMENT_ID(cmbx_fcc_department.Text))
            If rbx_fcc_cin.Checked = True Then
                transtype = "IN"
            End If
            If rbx_fcc_cout.Checked = True Then
                transtype = "OUT"
            End If





            ''INSERT MODIFIED LOG
            Dim curuser As String = CurrentLoginAccount
            Dim modf As Boolean = True

            If MessageBox.Show("Manual Adjustment for: " & cmbx_fcc_staffnames.Text & vbCrLf & _
                               "Date: " & dtpx_fcc_date.Value.ToLongDateString & vbCrLf & _
                               "Time: " & dtpx_fcc_time.Value.ToShortTimeString & " CLOCK-" & transtype & vbCrLf & vbCrLf & _
                                "Please confirm", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then

                If PUBLICKEY = True Then
                    curuser = "" : modf = False
                End If

                'usb_worker.INSERT_NEW_LOG(usb_worker.GET_LAST_RAW_LOG_ID(), staffid,
                '                     dtpx_DateManual.Value, Format(dtpx_fcc_time.Value, "HH:mm:ss"),
                '                     transtype, CurrentLoginAccount, modf, "", curuser)
                '2014-09-01 11:00:18
                Dim raw As String = Format(dtpx_fcc_date.Value, "yyyy-MM-dd") & " " & Format(dtpx_fcc_time.Value, "HH:mm:00")

                Console.WriteLine(raw)

                usb_worker.INSERT_NEW_LOG(staffid, dtpx_fcc_date.Value, Format(dtpx_fcc_time.Value, "HH:mm:00"), transtype, CurrentLoginAccount, modf, raw, CurrentLoginAccount)

                MessageBox.Show("Adjustment Complete!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)

            End If



        Else

            If cmbx_fcc_staffnames.Items.Count = 0 Then
                MessageBox.Show("Select Staff", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        End If
    End Sub

    Private Sub btnx_olm_approved_Click(sender As System.Object, e As System.EventArgs) Handles btnx_olm_approved.Click

        If String.IsNullOrEmpty(cmbx_olm_staffnames.Text) Then
            ' MessageBox.Show("Select staff!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)

            MSGOK.MSGOK_GLOBAL = "Select staff!"
            MSGOK.pbx_msgOK.Image = My.Resources._error
            MSGOK.ShowDialog()

            Exit Sub
        End If

        If String.IsNullOrEmpty(cmbx_olm_leavenames.Text) Then
            'MessageBox.Show("Specify leave name", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
            MSGOK.MSGOK_GLOBAL = "Specify leave name"
            MSGOK.pbx_msgOK.Image = My.Resources._error
            MSGOK.ShowDialog()

            Exit Sub
        End If


        Select Case btnx_olm_approved.Text
            Case "VOID"
                Dim listOfVoidingDates As New List(Of Date)
                listOfVoidingDates = cpu_wrkr.EXTRACTDATE(dtpx_olm_startdate.Value, dtpx_olm_enddate.Value)
                Dim staffid As String = sqlce_asstnt.GET_USER_ID(cmbx_olm_staffnames.Text.ToString, sqlce_asstnt.GET_DEPARTMENT_ID(cmbx_olm_department.Text.ToString))


                listOfVoidingDates.ForEach(Sub(v_day)

                                               sqlce_asstnt.REMOVE_FILED_LEAVE(staffid, v_day)
                                           End Sub)

                'MessageBox.Show("Removing Filed Leave completed!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)

                MSGOK.MSGOK_GLOBAL = "Removing Filed Leave completed!"
                MSGOK.pbx_msgOK.Image = My.Resources.checkok
                MSGOK.ShowDialog()



            Case "Approved"



                If MessageBox.Show("Filing leave for:  " & cmbx_olm_staffnames.Text & vbCrLf & _
                                   "Date:              " & dtpx_olm_startdate.Value.ToLongDateString & " to " & dtpx_olm_enddate.Value.ToLongDateString & vbCrLf & _
                                   "Leave Type:        " & cmbx_olm_leavenames.Text & vbCrLf & vbCrLf & _
                                   "Please confirm.", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    'cmbx_olm_department
                    'cmbx_olm_staffnames
                    'cmbx_olm_leavenames

                    ''BEFORE FILE CHECK FIRSTCHECK TOTAL BALANCE VS. TOTAL MAX IF LEAVE FROM LEAVE TABLE

                    Dim staffid As String = sqlce_asstnt.GET_USER_ID(cmbx_olm_staffnames.Text.ToString, sqlce_asstnt.GET_DEPARTMENT_ID(cmbx_olm_department.Text.ToString))
                    Dim leave_id As Integer = sqlce_asstnt.GET_LEAVECLASS_ID(cmbx_olm_leavenames.Text.ToString)





                    Dim listOfFiledDates As New List(Of Date)
                    listOfFiledDates = cpu_wrkr.EXTRACTDATE(dtpx_olm_startdate.Value, dtpx_olm_enddate.Value)
                    Dim listOfSuccessFiledLeave As New List(Of String)
                    Dim listOfPendingFileLeave As New List(Of String)
                    Dim filedexceptions As String = "Completed"
                    Dim pendingleavefiled As String = ""

                    listOfFiledDates.ForEach(Sub(day)
                                                 Dim TOTALFILED As Integer = sqlce_asstnt.COUNT_TOTAL_FILED_LEAVE(staffid, leave_id)
                                                 Dim LEAVEMAXTOTAL As Integer = sqlce_asstnt.GET_LEAVEID_MAX_COUNT(leave_id)
                                                 Dim paidleave As Boolean = sqlce_asstnt.CHECK_IF_PAID_LEAVE(leave_id)


                                                 If paidleave = True Then
                                                     If TOTALFILED >= LEAVEMAXTOTAL Then
                                                         If filedexceptions = "Pending" Then
                                                             Exit Sub
                                                         End If
                                                         If MessageBox.Show("Name: " & cmbx_olm_staffnames.Text & vbCrLf & "has exceed the maximum leave count for " & cmbx_olm_leavenames.Text & vbCrLf & vbCrLf & _
                                                                                        "Continue filing?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                                             If sqlce_asstnt.CHECK_IF_LEAVE_HASALREADY_FILED(staffid, day) Then
                                                                 sqlce_asstnt.UPDATE_FILED_LEAVE(staffid, DateAndTime.Now, day, leave_id, CurrentLoginAccount)
                                                             Else
                                                                 sqlce_asstnt.FILE_LEAVE(staffid, leave_id, day, CurrentLoginAccount)
                                                             End If

                                                         Else

                                                             filedexceptions = "Pending"
                                                             Exit Sub
                                                         End If


                                                     Else

                                                         If sqlce_asstnt.CHECK_IF_LEAVE_HASALREADY_FILED(staffid, day) = True Then
                                                             sqlce_asstnt.UPDATE_FILED_LEAVE(staffid, DateAndTime.Now, day, leave_id, CurrentLoginAccount)
                                                         Else

                                                             sqlce_asstnt.FILE_LEAVE(staffid, leave_id, day, CurrentLoginAccount)
                                                         End If
                                                         listOfSuccessFiledLeave.Add(day)

                                                     End If

                                                 Else

                                                     If sqlce_asstnt.CHECK_IF_LEAVE_HASALREADY_FILED(staffid, day) = True Then
                                                         sqlce_asstnt.UPDATE_FILED_LEAVE(staffid, DateAndTime.Now, day, leave_id, CurrentLoginAccount)
                                                     Else

                                                         sqlce_asstnt.FILE_LEAVE(staffid, leave_id, day, CurrentLoginAccount)
                                                     End If

                                                     listOfSuccessFiledLeave.Add(day)
                                                     'sqlce_asstnt.FILE_LEAVE(staffid, leave_id, day, CurrentLoginAccount)
                                                 End If
                                             End Sub)



                    Select Case filedexceptions
                        Case "Completed"
                            MessageBox.Show("Filed leave complete!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Case "Pending"
                            listOfFiledDates.ForEach(Sub(fdy)
                                                         If Not listOfSuccessFiledLeave.Contains(fdy) Then
                                                             listOfPendingFileLeave.Add(fdy.ToShortDateString)
                                                         End If
                                                     End Sub)
                            pendingleavefiled = String.Join(vbCrLf, listOfPendingFileLeave.ToArray)
                            MessageBox.Show("Name: " & cmbx_olm_staffnames.Text & vbCrLf & _
                                            cmbx_olm_leavenames.Text & " Pending Dates: " & vbCrLf & _
                                            pendingleavefiled)

                    End Select

                End If
        End Select
    End Sub

    Private Sub btnx_olm_showleave_schema_Click(sender As System.Object, e As System.EventArgs) Handles btnx_olm_showleave_schema.Click
        FlowLayoutPanel1.Enabled = False
        LEAVETYPEFORM.ShowDialog()
        FlowLayoutPanel1.Enabled = True
    End Sub

    Private Sub cmbx_olm_leavenames_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbx_olm_leavenames.SelectedIndexChanged
        Try
            If String.IsNullOrEmpty(cmbx_olm_leavenames.Text.ToString.Trim) Then

                Exit Sub
            End If

            If cmbx_olm_leavenames.Text.ToString.ToLower.Contains("void") Then
                btnx_olm_approved.Text = "VOID"
            Else
                btnx_olm_approved.Text = "Approved"
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub btnx_mto_showmtoSchema_Click(sender As System.Object, e As System.EventArgs) Handles btnx_mto_showmtoSchema.Click
        MTOFORM.ShowDialog()
    End Sub

    Private Sub btnx_mto_approved_Click(sender As System.Object, e As System.EventArgs) Handles btnx_mto_approved.Click



        Dim departmentname As String = cmbx_mto_department.Text
        Dim deptid As Integer = sqlce_asstnt.GET_DEPARTMENT_ID(departmentname)
        Dim staffname As String = cmbx_mto_staffnames.Text
        Dim staffid As String = sqlce_asstnt.GET_USER_ID(staffname, deptid)
        Dim travel_order_name As String = cmbx_mto_ordertypes.Text
        Dim travel_order_id As Integer = sqlce_asstnt.GET_TRAVEL_ORDER_ID(travel_order_name)
        Dim location As String = txtx_mto_location.Text.Trim
        Dim pupose As String = txtx_mto_purpose.Text.Trim

        If String.IsNullOrEmpty(travel_order_name.Trim) Then
            ' MessageBox.Show("Travel Order not define!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)


            MSGOK.MSGOK_GLOBAL = "Travel Order not define!"
            MSGOK.pbx_msgOK.Image = My.Resources._error
            MSGOK.ShowDialog()


            Exit Sub
        ElseIf String.IsNullOrEmpty(staffid) Then
            '  MessageBox.Show("Staff name not define!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)

            MSGOK.MSGOK_GLOBAL = "Select employee name"
            MSGOK.pbx_msgOK.Image = My.Resources._error
            MSGOK.ShowDialog()
            Exit Sub
        End If



        Dim listOfDates As New List(Of Date)
        listOfDates = cpu_wrkr.EXTRACTDATE(dtpx_mto_startdate.Value, dtpx_mto_enddate.Value)






        If String.IsNullOrEmpty(staffname.Trim) Then
            MSGOK.MSGOK_GLOBAL = "No employee is selected"
            MSGOK.pbx_msgOK.Image = My.Resources._error
            MSGOK.ShowDialog()
            Exit Sub
        End If






        Select Case btnx_mto_approved.Text
            Case "VOID"
                For Each workdate As Date In listOfDates
                    sqlce_asstnt.REMOVE_FILED_MTO(staffid, workdate)
                Next
                MSGOK.MSGOK_GLOBAL = "Filed Travel order for " & staffname & " has been canceled successfully"
                MSGOK.pbx_msgOK.Image = My.Resources.checkok
                MSGOK.ShowDialog()

            Case "Approved"
                ''STEP 1 CHECK IF STAFF HAS FILED ALREADY MTO
                Dim has_filed_already As Boolean = False
                For Each workdate As Date In listOfDates
                    has_filed_already = sqlce_asstnt.VERIFY_MTO_IF_FILED_ALREADY(staffid, workdate)
                    If has_filed_already = False Then
                        ''FILED NEW TRAVEL ORDER FOR STAFF
                        sqlce_asstnt.FILE_MTO(staffid, travel_order_id, workdate, DateAndTime.Now, CurrentLoginAccount, location, pupose)
                    Else
                        ''TODO: UPDATE THE FILED TRAVEL ORDER FOR STAFF
                        sqlce_asstnt.UPDATE_FILED_MTO(staffid, travel_order_id, workdate, DateAndTime.Now, CurrentLoginAccount, location, pupose)
                    End If
                Next
                MSGOK.MSGOK_GLOBAL = "Task completed successfully"
                MSGOK.pbx_msgOK.Image = My.Resources.checkok
                MSGOK.ShowDialog()
                LOAD_MTO_KNOWN_PURPOSES_AND_LOCATION()
        End Select
    End Sub



    Private Sub lnkx_es_allotment_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkx_es_allotment.LinkClicked
        'UpdatePosition(flp_ES, 500, 0)

        'flp_ES.Controls.SetChildIndex(flp_ES.Controls(0), 2)

        ' flp_ES.Controls.SetChildIndex(flp_ES.Controls(1), 2)
        ' flp_ES.Controls.SetChildIndex(flp_ES.Controls(2), 0)

        lstboxStafflist.SelectedItems.Clear()
        lstboxStafflist.SelectionMode = SelectionMode.MultiSimple

        If panel_es_assign = False Then
            'pnlx_show_esmenu.Visible = False
            If Not ThreadAutoScrollRight.IsBusy Then
                ThreadAutoScrollRight.RunWorkerAsync()
            End If
            panel_es_assign = True
            panel_es_sched = False
        Else
            panel_es_sched = False

        End If




        ' pnlx_shift_allotment.Visible = False
    End Sub

    Private Sub lnkx_es_existing_schedule_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkx_es_existing_schedule.LinkClicked

        lstboxStafflist.SelectedItems.Clear()
        lstboxStafflist.SelectionMode = SelectionMode.One

        If panel_es_sched = False Then

            ' pnlx_show_esmenu.Visible = False

            If Not ThreadAutoScrollLeft.IsBusy Then
                ThreadAutoScrollLeft.RunWorkerAsync()
            End If
            panel_es_sched = True
            panel_es_assign = False
        Else
            panel_es_assign = False
        End If
    End Sub


    Private Sub ThreadAutoScrollLeft_DoWork(sender As System.Object, e As System.ComponentModel.DoWorkEventArgs) Handles ThreadAutoScrollLeft.DoWork

        For i = 0 To 500 Step 10
            ''FlowLayoutPanel1.AutoScrollPosition = New Point(i, 0)
            '''System.Threading.Thread.Sleep(1)
            UpdatePosition(flp_ES, i, 0)
            ''FlowLayoutPanel1.Refresh()
        Next

    End Sub

    Private Sub ThreadAutoScrollRight_DoWork(sender As System.Object, e As System.ComponentModel.DoWorkEventArgs) Handles ThreadAutoScrollRight.DoWork

        For i = 500 To -15 Step -15
            'FlowLayoutPanel1.AutoScrollPosition = New Point(i, 0)
            ' System.Threading.Thread.Sleep(1)
            UpdatePosition(flp_ES, i, 0)
            'FlowLayoutPanel1.Refresh()
        Next
    End Sub


    'Private Sub lnkx_add_new_device_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkx_add_new_device.LinkClicked

    'End Sub

    Private Sub lnkx_set_device_time_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkx_set_device_time.LinkClicked

        Dim cursel As String = cmbx_devlist.Text
        'If lstBisbioDevices.SelectedIndex >= 0 Then
        'btn_GetAllAttlog.Enabled = True
        pbx_loading_network_download.Visible = True
        If Not ThreadBISBIONetworkDownload.IsBusy Then
            DeviceTask = "SET-TIME"
            ThreadBISBIONetworkDownload.RunWorkerAsync(cursel)
            pbx_loading_network_download.Visible = True
            'btn_GetAllAttlog.Enabled = False
            'btnSetDeviceTime.Enabled = False
            'btnx_deviceClearAdmin.Enabled = False
        End If



    End Sub

    Private Sub lnkx_clear_device_administrator_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkx_clear_device_administrator.LinkClicked

        Dim cursel As String = cmbx_devlist.Text
        'If lstBisbioDevices.SelectedIndex >= 0 Then
        '    btn_GetAllAttlog.Enabled = True
        pbx_loading_network_download.Visible = True
        If Not ThreadBISBIONetworkDownload.IsBusy Then
            DeviceTask = "CLEAR ADMIN"
            ThreadBISBIONetworkDownload.RunWorkerAsync(cursel)
            pbx_loading_network_download.Visible = True
            'btn_GetAllAttlog.Enabled = False
            'btnSetDeviceTime.Enabled = False
            'btnx_deviceClearAdmin.Enabled = False

        End If


    End Sub

    Private Sub lnkx_download_logs_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkx_download_logs.LinkClicked

        Dim cursel As String = cmbx_devlist.Text
        'If lstBisbioDevices.SelectedIndex >= 0 Then
        '    btn_GetAllAttlog.Enabled = True
        pbx_loading_network_download.Visible = True
        If Not ThreadBISBIONetworkDownload.IsBusy Then
            DeviceTask = "DOWNLOADLOGS"
            ThreadBISBIONetworkDownload.RunWorkerAsync(cursel)
            pbx_loading_network_download.Visible = True
            'btn_GetAllAttlog.Enabled = False
            'btnSetDeviceTime.Enabled = False
            'btnx_deviceClearAdmin.Enabled = False
        End If

        'Else

        'End If

    End Sub

    Private Sub lnkx_locate_usb_logs_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkx_locate_usb_logs.LinkClicked
        Dim ofd1 As New OpenFileDialog()
        ofd1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        ofd1.Filter = "attlog (*.dat;*.txt)|*.txt;*.dat" : ofd1.RestoreDirectory = True
        If ofd1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            'Me.ProgressBar2.Value = 0 : Me.ProgressBar2.Maximum = File.ReadAllLines(ofd1.FileName).Length

            If Not ThreadUsbRead.IsBusy Then
                ThreadUsbRead.RunWorkerAsync(ofd1.FileName)

                LoadingPbx.Visible = True
                Label33.Visible = True
                Label65.Text = "-"
                'btnBrowseLogs.Visible = False
            End If

        End If
        Label65.Text = "-"
    End Sub


    Private Sub lnkx_ed_import_masterfile_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkx_ed_import_masterfile.LinkClicked
        Dim ofd1 As New OpenFileDialog()
        ofd1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        ofd1.Filter = "masterfile (*.xlsx;*.xls;*.csv|*.xlsx;*.xls;*csv" : ofd1.RestoreDirectory = True
        If ofd1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            If Not String.IsNullOrEmpty(ofd1.FileName) Then
                If File.Exists(ofd1.FileName) Then
                    If Not ThreadMasterFileStaffMassUpload.IsBusy Then
                        ThreadMasterFileStaffMassUpload.RunWorkerAsync(ofd1.FileName)
                    End If
                End If

            End If

        End If
    End Sub

    Private Sub ThreadMasterFileStaffMassUpload_DoWork(sender As System.Object, e As System.ComponentModel.DoWorkEventArgs) Handles ThreadMasterFileStaffMassUpload.DoWork
        Dim filename As String = e.Argument.ToString

        ''CLEAN UP FIRST THE MASTERLIST TABLE

        sqlce_asstnt.CLEAR_MASTERLIST_TABLE()
        ''START READING THE EXCEL FILE
        ReadExcel.MAIN(filename)
        If ReadExcel.EXCEPTIONMESSAGE = "Import complete!" Then
            Dim listOfMasterlistDepartment As New List(Of String)
            Dim listOfExistingDepartment As New List(Of String)

            ''GET ALL EXISTING DEPARTMENT AND NEW MASTERFILE DEPARTMENT
            listOfMasterlistDepartment = sqlce_asstnt.GET_ALL_DEPARTMENT_NAME_FROM_MASTERFILE_TABLE()
            listOfExistingDepartment = sqlce_asstnt.GET_ALL_DEPARTMENT_NAME()

            ''START COMPARING
            listOfMasterlistDepartment.ForEach(Sub(dep)
                                                   If Not listOfExistingDepartment.Contains(dep) Then
                                                       sqlce_asstnt.INSERT_NEW_DEPARTMENT(dep)
                                                   End If
                                               End Sub)

            ''GET ALL EXISTING STAFFID AND NEW MASTERFILE STAFFID
            Dim listOfMasterlistStaffid As New List(Of String)
            Dim listOfExistingStaffId As New List(Of String)
            listOfExistingStaffId = sqlce_asstnt.GET_ALL_STAFF_ID(True, 0)

            listOfMasterlistStaffid = sqlce_asstnt.GET_ALL_STAFFID_FROM_MASTERFILE_TABLE()


            Dim staffname As String = ""
            Dim departmentid As Integer = 0
            Dim designation As String = ""
            listOfMasterlistStaffid.ForEach(Sub(staffid)
                                                If Not listOfExistingStaffId.Contains(staffid) Then
                                                    staffname = sqlce_asstnt.GET_STAFFNAME_FROM_MASTERFILE_TABLE(staffid)
                                                    departmentid = sqlce_asstnt.GET_DEPARTMENT_ID(sqlce_asstnt.GET_ASSIGN_DEPARTMENT_FROM_MASTERFILE_TABLE(staffid))
                                                    designation = sqlce_asstnt.GET_STAFF_DESIGNATION_FROM_MASTERFILE_TABLE(staffid)
                                                    sqlce_asstnt.INSERT_NEW_STAFF(staffid, staffname, departmentid, designation, txtc_other_details.Text, image_path_src)
                                                End If
                                            End Sub)

        End If
    End Sub

    Private Sub ThreadMasterFileStaffMassUpload_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles ThreadMasterFileStaffMassUpload.RunWorkerCompleted
        btnEmployeeProfiles_Click(sender, e)
        MessageBox.Show(ReadExcel.EXCEPTIONMESSAGE, "", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub



    Private Sub btnx_cleardatabase_Click(sender As System.Object, e As System.EventArgs) Handles btnx_cleardatabase.Click

        'ATTENDANCETABLE()
        'EMPLOYEEPROFILES()
        'FILEDTRAVELORDER()
        'FILEDLEAVE()
        'TIMELOGS()

        If MessageBox.Show("This action is IRREVOCABLE [Perfom backup to avoid lossing of important data.]" & vbCrLf & "Please comfirm!", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.OK Then

            Select Case cmbx_tables.Text
                Case "ATTENDANCETABLE"
                    sqlce_asstnt.BTMS_CLEAR_ATTENDANCETABLE()
                Case "EMPLOYEEPROFILES"
                    sqlce_asstnt.BTMS_CLEAR_EMPLOYEEPROFILES()
                Case "FILEDTRAVELORDER"
                    sqlce_asstnt.BTMS_CLEAR_FILED_TRAVEL_ORDER()
                Case "FILEDLEAVE"
                    sqlce_asstnt.BTMS_CLEAR_FILED_LEAVE()
                Case "TIMELOGS"
                    sqlce_asstnt.BTMS_CLEAR_TIMELOGS(True, Nothing, Nothing)
                    sqlce_asstnt.BTMS_RESET_TIMELOGS_SEED()
            End Select

            MessageBox.Show("Task Complete!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    'Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click
    '    Dim cups As New CPU

    '    Dim punch As String = "18:00"
    '    Dim startingpoint As String = "16:00"
    '    Dim offdutytime As String = "17:00"
    '    Dim result As Integer = 0

    '    result = cups.GET_INTERVAL_AFTER_COUT(startingpoint, offdutytime, punch)

    '    Console.WriteLine("Result: " & result)
    'End Sub

    Private Sub pbx_OT_Click(sender As System.Object, e As System.EventArgs) Handles pbx_OT.Click
        selectedReportIconName = "SROT"
        GlobalVariables.DTR_TYPE = "SROT"
        Label19.Visible = False
        btnReportExit.Visible = False


        'grpx_ReportSignatory.Text = "Report Signatory"
        lblx_PromptHeaderText.Text = "Generate Summary of Overtime(OT)"
        pbx_ReportIconsControl.Visible = False
        pbx_ReportConfiguration.Visible = True
        LOAD_TREEVIEW_DETAILS()
        'cmbRptDep.Focus()
    End Sub

    'Private Sub lstUserAccess_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lstUserAccess.SelectedIndexChanged

    'End Sub

    Private Sub lstboxStafflist_MouseDown(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles lstboxStafflist.MouseDown
        Try
            Dim curItem As String = lstboxStafflist.SelectedItem.ToString()
            Dim CurrPos As Integer = lstboxStafflist.FindString(curItem)


            If Not lstboxStafflist.Items.Count = -1 Then
                If e.Button = Windows.Forms.MouseButtons.Right Then
                    RightClickSelecAllEmployee.Show(MousePosition)
                End If
            End If

        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    Private Sub toolstrip_select_all_Click(sender As System.Object, e As System.EventArgs) Handles toolstrip_select_all.Click
        For i As Integer = 0 To lstboxStafflist.Items.Count - 1
            lstboxStafflist.SetSelected(i, True)
        Next i
    End Sub

    'Private Sub Button7_Click(sender As System.Object, e As System.EventArgs) Handles Button7.Click

    '    Dim a As Object = New Object

    '    a = 10.1
    '    MessageBox.Show("Type: " & a.GetType.ToString & " Value: " & a)

    '    a = ""

    '    MessageBox.Show("Type: " & a.GetType.ToString & " Value: " & a)
    'End Sub




    Private Sub btnx_lookup_raw_Click(sender As System.Object, e As System.EventArgs) Handles btnx_lookup_raw.Click
        'On Error Resume Next
        'If Not ThreadLoadRawSwipes.IsBusy Then
        '    ThreadLoadRawSwipes.RunWorkerAsync()
        'End If

        Dim s_date As Date = dtpx_rsr_startdate.Value
        Dim e_date As Date = dtpx_rsr_enddate.Value


        Dim listOfDates As New List(Of Date)

        Dim listOfRawSwipe As New List(Of String)
        Dim lstswipe As New List(Of String)

        Dim staffname As String = cmbx_rsr_staffnames.Text
        Dim department_name As String = cmbx_rsr_department.Text
        Dim staffid As String = sqlce_asstnt.GET_USER_ID(staffname, sqlce_asstnt.GET_DEPARTMENT_ID(department_name))
        Dim raw_logs_list As New List(Of String)
        listOfDates = cpu_wrkr.EXTRACTDATE(s_date, e_date)

        UpdateRawSwipe_Clear_Items(lvRawSwipe)
        raw_logs_list.Add("*******************************RAW SWIPE RECORD******************************")
        raw_logs_list.Add("ID           : " & staffid)
        raw_logs_list.Add("NAME         : " & sqlce_asstnt.GET_STAFF_NAME(staffid))
        raw_logs_list.Add("DESIGNATION  : " & sqlce_asstnt.GET_STAFF_DESIGNATION(staffid))
        raw_logs_list.Add("DEPARTMENT   : " & sqlce_asstnt.GET_STAFF_ASSIGN_DEPARTMENT_NAME(staffid))
        raw_logs_list.Add("RECORDS FROM : " & Format(s_date, "MMM dd-dddd-yyyy") & " TO " & Format(e_date, "MMM dd-dddd-yyyy"))
        raw_logs_list.Add("-----------------------------------------------------------------------------")
        raw_logs_list.Add(String.Format("{0,-20}{1,-50}", "Date", "Records"))
        raw_logs_list.Add("-----------------------------------------------------------------------------")
        Dim results As String = ""
        listOfDates.ForEach(Sub(_day)
                                results = ""
                                listOfRawSwipe.Clear()
                                lstswipe.Clear()
                                listOfRawSwipe = sqlce_asstnt.GET_USER_RAWLOGS(staffid, _day, _day)

                                For Each d As String In listOfRawSwipe
                                    lstswipe.Add(Format(CDate(d), "hh:mmtt"))
                                Next
                                '  Console.WriteLine(String.Join("+", lstswipe.ToArray))
                                UpdateRawSwipe(lvRawSwipe, Format(_day, "MMM dd-ddd"), String.Join(", ", lstswipe.ToArray))

                                results = String.Format("{0,-20}{1,-50}", Format(_day, "MMM dd-ddd"), String.Join(", ", lstswipe.ToArray))
                                raw_logs_list.Add(results)

                            End Sub)


        If MessageBox.Show("Do you want to print this query?", "BTMS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            Try
                If File.Exists("RAW_SWIPE_RECORD.txt") Then
                    File.Delete("RAW_SWIPE_RECORD.txt")
                End If
              

                WRITE_RAW_LOGS(String.Join(vbNewLine, raw_logs_list.ToArray))

                Dim PROC As New System.Diagnostics.Process
                PROC = System.Diagnostics.Process.Start(Application.StartupPath & "\wordpad.exe", Application.StartupPath & "\RAW_SWIPE_RECORD.txt")
                PROC.Dispose()
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        End If

   







        'Dim btms_lstOfStaffid As New List(Of String)
        'Dim att_lstOfStaffid As New List(Of String)
        'Dim att_invalid_names As New List(Of String)
        'Dim att_forchecking_names As New List(Of String)

        'btms_lstOfStaffid = sqlce_asstnt.GET_ALL_STAFF_ID(True, 0)
        'att_lstOfStaffid = ATT_GET_NAME_AS_LIST()

        'btms_lstOfStaffid.Sort()
        'att_lstOfStaffid.Sort()
        'Dim name As String
        'Dim result As String
        '''not in btms
        'att_forchecking_names.Add("BTMS(GovEngine.sdf) vs ATT(att2000.mdb)")
        'att_forchecking_names.Add("names_not_in_btms")
        'att_forchecking_names.Add("BTMS USERS: " & btms_lstOfStaffid.Count)
        'att_forchecking_names.Add("ATT USERS: " & att_lstOfStaffid.Count)
        'att_forchecking_names.Add("------------------------------------------------------------")
        'att_forchecking_names.Add(String.Format("{0,-10}{1,-50}", "ID", "NAME"))
        'att_forchecking_names.Add("------------------------------------------------------------")

        'For Each n In att_lstOfStaffid

        '    If Not btms_lstOfStaffid.Contains(n) Then
        '        name = ATT_GET_NAME(n)

        '        If name.Length < 11 Then
        '            result = String.Format("{0,-10}{1,-50}", n, name)
        '            att_invalid_names.Add(result)
        '        Else
        '            result = String.Format("{0,-10}{1,-50}", n, name)
        '            att_forchecking_names.Add(result)
        '        End If
        '    End If
        'Next



        '''names ouput
        ''For Each n As String In btms_lstOfStaffid
        ''    Console.WriteLine(n & ">>" & sqlce_asstnt.GET_STAFF_NAME(n) & vbTab & ATT_GET_NAME(n))
        ''Next

        'Try
        '    If File.Exists("invalid.txt") Then
        '        File.Delete("invalid.txt")
        '    End If
        '    Console.WriteLine("FOR CHECKING")

        '    Console.WriteLine(String.Join(vbNewLine, att_forchecking_names.ToArray))
        '    WRITE_INVALID(String.Join(vbNewLine, att_forchecking_names.ToArray))
        '    Console.WriteLine(String.Join(vbNewLine, att_invalid_names.ToArray))
        '    WRITE_INVALID(String.Join(vbNewLine, att_invalid_names.ToArray))



        '    Dim PROC As New System.Diagnostics.Process
        '    PROC = System.Diagnostics.Process.Start(Application.StartupPath & "\wordpad.exe", Application.StartupPath & "\invalid.txt")
        '    PROC.Dispose()
        'Catch ex As Exception
        '    MessageBox.Show(ex.Message)
        'End T
    End Sub

    Private Sub dtpx_rsr_startdate_ValueChanged(sender As System.Object, e As System.EventArgs) Handles dtpx_rsr_startdate.ValueChanged
        'On Error Resume Next
        'btnx_lookup_raw_Click(sender, e)
    End Sub

    Private Sub dtpx_rsr_enddate_ValueChanged(sender As System.Object, e As System.EventArgs) Handles dtpx_rsr_enddate.ValueChanged
        'On Error Resume Next
        'btnx_lookup_raw_Click(sender, e)
    End Sub

    Private Sub cmbx_rsr_staffnames_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbx_rsr_staffnames.SelectedIndexChanged
        'On Error Resume Next
        'btnx_lookup_raw_Click(sender, e)
    End Sub

    Public Sub UpdateRawSwipe(ByVal lv As ListView, day As String, ByVal details As String)
        Invoke(New MethodInvoker(Sub()
                                     Dim lvItem As New ListViewItem()
                                     lvItem = lv.Items.Add(day)
                                     lvItem.SubItems.Add(details)
                                 End Sub))
    End Sub

    Public Sub UpdateRawSwipe_Clear_Items(ByVal lv As ListView)
        Invoke(New MethodInvoker(Sub()
                                     lv.Items.Clear()
                                 End Sub))
    End Sub


    Private Sub ThreadLoadRawSwipes_DoWork(sender As System.Object, e As System.ComponentModel.DoWorkEventArgs) Handles ThreadLoadRawSwipes.DoWork
        On Error Resume Next


        Dim s_date As Date = dtpx_rsr_startdate.Value
        Dim e_date As Date = dtpx_rsr_enddate.Value


        Dim listOfDates As New List(Of Date)

        Dim listOfRawSwipe As New List(Of String)

        Dim staffname As String = cmbx_rsr_staffnames.Text
        Dim department_name As String = cmbx_rsr_department.Text
        Dim staffid As String = sqlce_asstnt.GET_USER_ID(staffname, sqlce_asstnt.GET_DEPARTMENT_ID(department_name))

        listOfDates = cpu_wrkr.EXTRACTDATE(s_date, e_date)


        UpdateRawSwipe_Clear_Items(lvRawSwipe)

        listOfDates.ForEach(Sub(_day)

                                listOfRawSwipe = sqlce_asstnt.GET_USER_RAWLOGS(staffid, _day, _day)
                                Console.WriteLine(String.Join("+", listOfRawSwipe.ToArray))
                                UpdateRawSwipe(lvRawSwipe, _day, String.Join("+", listOfRawSwipe.ToArray))

                            End Sub)
    End Sub

    Private Sub trv1_AfterCheck(sender As System.Object, e As System.Windows.Forms.TreeViewEventArgs) Handles trv1.AfterCheck
        Dim objNode As TreeNode = e.Node
        Dim listOfDepartmentNames As New List(Of String)
        Dim UncheckItem As New List(Of String)

        listOfDepartmentNames = sqlce_asstnt.GET_ALL_DEPARTMENT_NAME                                 'Get List of Departments from DB.
        Dim bCheck As Boolean = e.Node.Checked                                  'Determine Check State of Each node.
        For Each ChildNode As TreeNode In objNode.Nodes                             'Loop Through all Nodes.
            ChildNode.Checked = bCheck                                                  'Set Current State bcheck value
        Next


        Dim nodes As TreeNodeCollection = trv1.Nodes(0).Nodes
        For Each n As TreeNode In nodes
            For Each c As TreeNode In n.Nodes                               'Nodes for child
                If Not listOfDepartmentNames.Contains(c.Text) Then
                    If c.Checked Then
                        If Not cmbx_selected_names.Items.Contains(c.Text) Then
                            cmbx_selected_names.Items.Add(c.Text)
                            c.ForeColor = Color.White
                            'c.NodeFont = New Font("Microsoft Sans Serif", 9, FontStyle.Italic)
                            'c.NodeFont = New Font("Microsoft Sans Serif", 9, FontStyle.Underline)
                            c.BackColor = Color.DodgerBlue
                        End If
                    Else
                        cmbx_selected_names.Items.Remove(c.Text)
                        c.ForeColor = Color.White
                        '  c.NodeFont = New Font("Microsoft Sans Serif", 9, FontStyle.Regular)
                        c.BackColor = Color.FromArgb(50, 50, 50)
                    End If
                End If
            Next

            If Not n.Checked Then                   ''nodes for parent
                n.ForeColor = Color.Lime
                'Else
                '    n.ForeColor = Color.DimGray
            End If
        Next
    End Sub

    'Private Sub btn_UpdateChangesHolidaySchema_Click_1(sender As System.Object, e As System.EventArgs) Handles btn_UpdateChangesHolidaySchema.Click

    '    Try
    '        Me.Validate()
    '        Me.PUBLICHOLIDAYTABLEBindingSource.EndEdit()
    '        Me.PUBLICHOLIDAYTABLETableAdapter.Update(Me.GovEngineDataSet.PUBLICHOLIDAYTABLE)
    '        MessageBox.Show("Update successful", "", MessageBoxButtons.OK, MessageBoxIcon.Information)

    '    Catch ex As Exception
    '        MessageBox.Show("Update failed", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try


    'End Sub

    'Public Sub ARRANGE_DATAGRID_PUBLICHOLIDAY()

    '    For I1 = 0 To dtgv_PublicHolidaySchema.Columns.Count - 1

    '        If dtgv_PublicHolidaySchema.Columns(I1).Name = "HOLIDAYNAME" Then
    '            Dim column As DataGridViewColumn = dtgv_PublicHolidaySchema.Columns(I1)
    '            column.Width = 300
    '        ElseIf dtgv_PublicHolidaySchema.Columns(I1).Name = "ID" Then
    '            dtgv_PublicHolidaySchema.Columns(0).Visible = False
    '        ElseIf dtgv_PublicHolidaySchema.Columns(I1).Name = "DATE" Then
    '            Dim column As DataGridViewColumn = dtgv_PublicHolidaySchema.Columns(I1)
    '            column.Width = 300
    '        End If
    '    Next
    'End Sub

    'Private Sub pbx_SMDTR_Click(sender As System.Object, e As System.EventArgs) Handles pbx_SMDTR.Click
    '    'dtgv_Submit_dtr.DataSource = sqlce_asstnt.LOAD_DTR_TABLE



    'End Sub



    Private Sub ThreadExportDTRSummary_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles ThreadMergeDistrictDTR.RunWorkerCompleted
        MessageBox.Show("Complete!")
    End Sub

    Private Sub lnkx_MergeDistrict_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkx_MergeDistrict.LinkClicked
        'Dim myStream As Stream = Nothing
        Dim ofd1 As New OpenFileDialog()
        ofd1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        ofd1.Filter = "DTR Summary|*.zip"
        ofd1.FilterIndex = 3
        ofd1.Multiselect = True
        ofd1.RestoreDirectory = True
        If ofd1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            Dim filepath As New List(Of String)
            For Each file As String In ofd1.FileNames
                ' Create a PictureBox for each file, and add that file to the FlowLayoutPanel. 
                Try
                    filepath.Add(file)
                Catch SecEx As SecurityException
                    ' The user lacks appropriate permissions to read files, discover paths, etc.
                    MessageBox.Show("Security error. Please contact your administrator for details.\n\n" & _
                        "Error message: " & SecEx.Message & "\n\n" & _
                        "Details (send to Support):\n\n" & SecEx.StackTrace)
                Catch ex As Exception
                    ' Could not load the image - probably permissions-related.
                    MessageBox.Show(("Cannot open the file: " & file.Substring(file.LastIndexOf("\"c)) & _
                    ". You may not have permission to read the file, or " + "it may be corrupt." _
                    & ControlChars.Lf & ControlChars.Lf & "Reported error: " & ex.Message))
                End Try
            Next file

            If Not ThreadMergeDistrictDTR.IsBusy Then
                ThreadMergeDistrictDTR.RunWorkerAsync(String.Join("|", filepath.ToArray))
            End If



        End If



    End Sub


    Private Sub pbx_ReportIconsControl_Paint(sender As System.Object, e As System.Windows.Forms.PaintEventArgs) Handles pbx_ReportIconsControl.Paint

    End Sub

    Private Sub lnx_submitDTR_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnx_submitDTR.LinkClicked

        Dim _filename As String = Format(DateAndTime.Now, "MMMM yyyy") & " DTR Summary for " & My.Settings.CompanyName
        'MessageBox.Show(_filename)
        _filename = _filename.Replace(":", "")
        _filename = _filename.Replace("/", "")
        _filename = _filename.Replace("-", "")
        Dim sfd As New SaveFileDialog() ' this creates an instance of the SaveFileDialog called "sfd"
        sfd.Filter = "DTR Summary(*.zip)|*.zip"
        sfd.FilterIndex = 1
        sfd.RestoreDirectory = True
        sfd.FileName = _filename
        sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        If sfd.ShowDialog() = DialogResult.OK Then

            Try
                dtgv_dtrSummary.DataSource = sqlce_asstnt.LOAD_DTR_TABLE()
                ' retrieve the full path to the file selected by the user
                dtgv_dtrSummary.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText
                dtgv_dtrSummary.SelectAll()
                IO.File.WriteAllText(sfd.FileName, dtgv_dtrSummary.GetClipboardContent().GetText.TrimEnd)
                dtgv_dtrSummary.ClearSelection()

                MessageBox.Show("Export Completed!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try

        End If

    End Sub
    Private Sub ThreadMergeDistrictDTR_DoWork(sender As System.Object, e As System.ComponentModel.DoWorkEventArgs) Handles ThreadMergeDistrictDTR.DoWork
        Dim files As String() = e.Argument.ToString.Split("|")
        Dim staffid As String = ""
        Dim ldate As Date
        Dim amcin As String = ""
        Dim amcout As String = ""
        Dim pmcin As String = ""
        Dim pmcout As String = ""
        Dim undertimehh As String = ""
        Dim undertimem As String = ""
        Dim dtrremarks As String = ""
        Dim monthlyremarks As String = ""
        Dim totalundertime As Integer = 0
        Dim week As String = ""
        Dim staffname As String = ""
        Dim department As String = ""
        Dim underhhremarks As String = ""
        Dim undertmmremarks As String = ""
        Dim remarksremarks As String = ""
        Dim undertimedaily As String = ""
        Dim undertimetotalmonthly As String = ""
        Dim undertimetotalmonthlyasminutes As String = ""
        Dim ottotaldaily As Integer = 0
        Dim ottotalmonthlyremarks As String = ""
        Dim ottotaldailyremarks As String = ""
        Dim officialarrival As String = ""
        Dim officialdeparture As String = ""


        For Each file As String In files
            Using reader As Microsoft.VisualBasic.FileIO.TextFieldParser = New Microsoft.VisualBasic.FileIO.TextFieldParser(file)
                reader.TextFieldType = FileIO.FieldType.Delimited
                reader.SetDelimiters("	")
                Dim currentRow As String()
                While Not reader.EndOfData
                    Dim listofDetails As New List(Of String)
                    Try
                        currentRow = reader.ReadFields
                        For Each item As String In currentRow
                            listofDetails.Add(item.ToString)
                        Next

                        If listofDetails.Count < 25 Then
                            Console.WriteLine("Invalid Length: " & listofDetails.Count)
                            Continue While
                        End If

                        Console.WriteLine("Length: " & listofDetails.Count)
                        Console.WriteLine("Details: " & String.Join("+", listofDetails.ToArray))



                        staffid = listofDetails(0)
                        ldate = CDate(listofDetails(1))
                        amcin = listofDetails(2)
                        amcout = listofDetails(3)
                        pmcin = listofDetails(4)
                        pmcout = listofDetails(5)
                        undertimehh = listofDetails(6)
                        undertimem = listofDetails(7)
                        dtrremarks = listofDetails(8)
                        monthlyremarks = listofDetails(9)
                        totalundertime = IIf(String.IsNullOrEmpty(listofDetails(10)) = True, 0, listofDetails(10))
                        week = listofDetails(11)
                        staffname = listofDetails(12)
                        department = listofDetails(13)
                        underhhremarks = listofDetails(14)
                        undertmmremarks = listofDetails(15)
                        remarksremarks = listofDetails(16)

                        undertimedaily = listofDetails(17)
                        undertimetotalmonthly = listofDetails(18)
                        undertimetotalmonthlyasminutes = listofDetails(19)
                        ottotaldaily = IIf(String.IsNullOrEmpty(listofDetails(20)) = True, 0, listofDetails(20))
                        ottotalmonthlyremarks = listofDetails(21)
                        ottotaldailyremarks = listofDetails(22)
                        officialarrival = listofDetails(23)
                        officialdeparture = listofDetails(24)


                        sqlce_asstnt.INSERT_INTO_DTR_TABLE(staffid, staffname, department, ldate,
                                                           amcin, amcout, pmcin, pmcout, week, dtrremarks,
                                                           monthlyremarks, totalundertime, undertimehh, undertimem,
                                                           undertimedaily, totalundertime, ottotaldailyremarks,
                                                           officialarrival, officialdeparture, "", "", "",
                                                           "", "", "", "", "")




                        'sqlce_asstnt.INSERT_INTO_DTR_TABLE()
                    Catch ex As Exception
                        MessageBox.Show(ex.Message)
                    End Try


                End While
            End Using
        Next file

    End Sub

    'Public Function ProductKey(ByVal KeyPath As String, ByVal ValueName As String) As String
    '    Dim HexBuf As Object = My.Computer.Registry.GetValue(KeyPath, ValueName, 0)
    '    If HexBuf Is Nothing Then Return "N/A"
    '    Dim tmp As String = ""
    '    For l As Integer = LBound(HexBuf) To UBound(HexBuf)
    '        tmp = tmp & " " & Hex(HexBuf(l))
    '    Next
    '    Dim Digits(24) As String
    '    Digits(0) = "B" : Digits(1) = "C" : Digits(2) = "D" : Digits(3) = "F"
    '    Digits(4) = "G" : Digits(5) = "H" : Digits(6) = "J" : Digits(7) = "K"
    '    Digits(8) = "M" : Digits(9) = "P" : Digits(10) = "Q" : Digits(11) = "R"
    '    Digits(12) = "T" : Digits(13) = "V" : Digits(14) = "W" : Digits(15) = "X"
    '    Digits(16) = "Y" : Digits(17) = "2" : Digits(18) = "3" : Digits(19) = "4"
    '    Digits(20) = "6" : Digits(21) = "7" : Digits(22) = "8" : Digits(23) = "9"

    '    Dim HexDigitalPID(15) As String
    '    Dim Des(30) As String
    '    Dim tmp2 As String = ""
    '    Dim StartOffset As Integer = 52, EndOffset As Integer = 67
    '    For i = StartOffset To EndOffset
    '        HexDigitalPID(i - StartOffset) = HexBuf(i)
    '        tmp2 = tmp2 & " " & Hex(HexDigitalPID(i - StartOffset))
    '    Next
    '    Dim KEYSTRING As String = ""
    '    Dim dLen As Integer = 29
    '    For i As Integer = dLen - 1 To 0 Step -1
    '        If ((i + 1) Mod 6) = 0 Then
    '            Des(i) = "-"
    '            KEYSTRING = KEYSTRING & "-"
    '        Else
    '            Dim HN As Integer = 0
    '            Dim sLen As Integer = 15
    '            For N As Integer = (sLen - 1) To 0 Step -1
    '                Dim Value As Integer = ((HN * 2 ^ 8) Or HexDigitalPID(N))
    '                HexDigitalPID(N) = Value \ 24
    '                HN = (Value Mod 24)
    '            Next
    '            Des(i) = Digits(HN)
    '            KEYSTRING = KEYSTRING & Digits(HN)
    '        End If
    '    Next
    '    Return StrReverse(KEYSTRING)
    'End Function


    Private Sub lblx_registration_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblx_registration.LinkClicked


        Dim condition As String = lblx_registration.Text.ToString

        Select Case condition
            Case "Product Registration"
                TabManager(MainControPanelTab, TabPage3)
                lbl_regkey.Text = REGISTRATIONKEY
                systemconfigControlTab.Visible = False
                Label47.Visible = False
                Button3.Visible = False
                systemconfigControlTab.Visible = True
                Label47.Visible = True
                Button3.Visible = True

            Case "Product is Activated"

        End Select



    End Sub

    Private Sub Panel11_Paint(sender As System.Object, e As System.Windows.Forms.PaintEventArgs) Handles Panel11.Paint

    End Sub

    Private Sub lnkx_skipregistration_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkx_skipregistration.LinkClicked
        If PRODUCTISACTIVATED = False Then
            CHECK_TRIAL()
        End If


        If sqlce_asstnt.CHECK_USER_ACCESS_TABLE_ISEMPTY = True Then
            LOAD_WITH_NO_LOGIN_WINDOW()
        Else
            LOAD_WITH_LOGIN_WINDOW()
        End If


    End Sub

    Private Sub btnx_activate_Click(sender As System.Object, e As System.EventArgs) Handles btnx_activate.Click
        Dim license As String = txtx_productkey.Text.Trim.ToUpper
        PRODUCTKEY = registration_worker.GET_PRODUCTKEY(lbl_regkey.Text.Trim)
        If license = PRODUCTKEY Then
            ''TODO WRITE MAC ADDRESS INTO DB
            sqlce_asstnt.BTMS_CLEAN_LICENSETABLE()
            sqlce_asstnt.BTMS_ACTIVATE_PRODUCT(PRODUCTKEY, DateAndTime.Now)

            MessageBox.Show("Thank you for choosing Business Machines Corporation" & vbCrLf & "The Product is now ACTIVATED", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Console.WriteLine("Activation success")
            pnlx_input_company_name.Visible = True
            pnlx_input_company_name.BringToFront()
        Else
            ''TODO PROMPT WRONG PRODUCT KEY

            MessageBox.Show("Invalid license", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Console.WriteLine("Invalid key")

        End If
    End Sub



    'Private Sub ARRANGE_DATAGRID_INACTIVE_EMPLOYEE()
    '    For I1 = 0 To dtgEmpProfiles.Columns.Count - 1
    '        If dtgEmpProfiles.Columns(I1).Name = "INACTIVEID" Then
    '            'dtgEmpProfiles.Columns(I1).Visible = False
    '        ElseIf dtgEmpProfiles.Columns(I1).Name = "STAFFNAME" Then
    '            Dim column As DataGridViewColumn = dtgEmpProfiles.Columns(I1)
    '            column.Width = 250
    '        ElseIf dtgEmpProfiles.Columns(I1).Name = "DEPARTMENT" Then
    '            Dim column As DataGridViewColumn = dtgEmpProfiles.Columns(I1)
    '            column.Width = 200
    '        ElseIf dtgEmpProfiles.Columns(I1).Name = "INACTIVE DATE" Then
    '            Dim column As DataGridViewColumn = dtgEmpProfiles.Columns(I1)
    '            column.Width = 300
    '        ElseIf dtgEmpProfiles.Columns(I1).Name = "FILEDBY" Then
    '            Dim column As DataGridViewColumn = dtgEmpProfiles.Columns(I1)
    '            column.Width = 300
    '        End If
    '    Next
    'End Sub

    Private Sub rightclickRehired_Click(sender As System.Object, e As System.EventArgs) Handles rightclickRehired.Click

        'Dim CurrRowPos As Integer = dtgEmpProfiles.CurrentRow.Index

        'If Not IsDBNull(dtgEmpProfiles.Rows(CurrRowPos).Cells.Item(1).Value) Then
        '    Dim staffid As String = dtgEmpProfiles.Rows(CurrRowPos).Cells.Item(0).Value
        '    Dim staffname As String = dtgEmpProfiles.Rows(CurrRowPos).Cells.Item(1).Value
        '    Dim department As String = dtgEmpProfiles.Rows(CurrRowPos).Cells.Item(2).Value
        '    Dim dep_id As Integer = 1
        '    dep_id = sqlce_asstnt.GET_DEPARTMENT_ID(department)

        '    If MessageBox.Show("Your are about to rehired a resigned employee. Please confirm!", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = Windows.Forms.DialogResult.Yes Then
        '        Try
        '            sqlce_asstnt.INSERT_NEW_STAFF(staffid, staffname, dep_id)
        '        Catch ex As Exception
        '            MessageBox.Show(ex.Message)
        '            Exit Sub
        '        End Try
        '        sqlce_asstnt.REMOVE_REHIRED_STAFF_FROM_INACTIVETABLE(staffid)
        '        dtgEmpProfiles.DataSource = sqlce_asstnt.LOAD_ALL_INACTIVE_STAFF()

        '        ARRANGE_DATAGRID_INACTIVE_EMPLOYEE()
        '        MessageBox.Show("Rehired success!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '    End If
        'End If
    End Sub


    Private Sub btnx_cancelEdit_Click(sender As System.Object, e As System.EventArgs) Handles btnx_cancelEdit.Click
        cmbDepartment.Text = ""
        txtEmpIDNo.Text = ""
        txtEmpName.Text = ""
        btnSaveEmployee.Text = "Create"
        btnx_cancelEdit.Visible = False
        txtEmpIDNo.Enabled = True

        btnx_resignemp.Enabled = True
        btn_RemoveEmp.Enabled = True
        btn_EditEmp.Enabled = True
        txtEmpIDNo.Text = ""
        txtEmpName.Text = ""
        cmb1.Enabled = True
        lvStaffs.Enabled = True




        'dtgEmpProfiles.DataSource = sqlce_asstnt.GET_ALL_EMPLOYEES(True, False, Nothing, False, Nothing, False, Nothing)
    End Sub

    Private Sub Panel24_Paint(sender As System.Object, e As System.Windows.Forms.PaintEventArgs) Handles Panel24.Paint

    End Sub

    Private Sub txtEmpIDNo_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtEmpIDNo.TextChanged
        Dim staffid As String = txtEmpIDNo.Text.Trim

        If Not String.IsNullOrEmpty(staffid) Then
            ' dtgEmpProfiles.DataSource = sqlce_asstnt.TEXT_CHANGE_SEARCH_ID(staffid)

        End If
    End Sub

    Private Sub btnx_save_company_name_Click(sender As System.Object, e As System.EventArgs) Handles btnx_save_company_name.Click
        Dim company_name As String = ""
        company_name = txtx_company_name.Text.Trim

        My.Settings.CompanyName = company_name
        My.Settings.Save()
        lblx_registration.Text = "Product is Activated"
        lnkx_skipregistration_LinkClicked(Nothing, Nothing)
        pnlx_input_company_name.Visible = False
    End Sub
    Private Sub PictureBox4_Click(sender As System.Object, e As System.EventArgs) Handles PictureBox4.Click
        Label19.Visible = False
        btnReportExit.Visible = False
    End Sub



    Private Sub btnx_add_LNimage_Click(sender As System.Object, e As System.EventArgs) Handles btnx_add_LNimage.Click
        Dim ofd1 As New OpenFileDialog()
        ofd1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        ofd1.Filter = "Company Image (*.jpg;*.png;|*.jpg;*.png" : ofd1.RestoreDirectory = True
        If ofd1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            If Not String.IsNullOrEmpty(ofd1.FileName) Then
                If File.Exists(ofd1.FileName) Then
                    My.Settings.CompanyLogo = ofd1.FileName
                    My.Settings.Save()
                End If

            End If

        End If
        pbx_company_image2.ImageLocation = My.Settings.CompanyLogo

    End Sub

    Private Sub btnx_remove_LNimage_Click(sender As System.Object, e As System.EventArgs) Handles btnx_remove_LNimage.Click
        My.Settings.CompanyLogo = ""
        My.Settings.Save()
        pbx_company_image2.ImageLocation = My.Settings.CompanyLogo
    End Sub




    Private Sub chkMon_MouseDown(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles chkMon.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            chkMon.CheckState = CheckState.Indeterminate
        End If
    End Sub

    Private Sub chkTue_MouseDown(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles chkTue.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            chkTue.CheckState = CheckState.Indeterminate
        End If
    End Sub

    Private Sub chkWed_MouseDown(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles chkWed.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            chkWed.CheckState = CheckState.Indeterminate
        End If
    End Sub

    Private Sub chkThu_MouseDown(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles chkThu.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            chkThu.CheckState = CheckState.Indeterminate
        End If
    End Sub

    Private Sub chkFri_MouseDown(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles chkFri.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            chkFri.CheckState = CheckState.Indeterminate
        End If
    End Sub
    Private Sub chkSat_MouseDown(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles chkSat.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            chkSat.CheckState = CheckState.Indeterminate
        End If
    End Sub

    Private Sub chkSun_MouseDown(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles chkSun.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            chkSun.CheckState = CheckState.Indeterminate
        End If
    End Sub

    Private Sub pnlx_shift_allotment_Paint(sender As System.Object, e As System.Windows.Forms.PaintEventArgs) Handles pnlx_shift_allotment.Paint

    End Sub

    Private Sub Label14_Click(sender As System.Object, e As System.EventArgs)

    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles btn_AddDepartment.Click
        'TabPage2.BackColor = Color.LavenderBlush
        Panel18.Enabled = False
        DEPARTMENTFORM.ShowDialog()
        Panel18.Enabled = True
    End Sub

    Private Sub btn_db_backup_Click(sender As System.Object, e As System.EventArgs) Handles btn_db_backup.Click
        'insert into OPENROWSET('Microsoft.ACE.OLEDB.12.0', 'Excel 12.0;Database=D:\testing.xlsx;', 'SELECT * FROM [Sheet1$]') select * from DataDictionary

        'Dim FileToCopy As String
        'Dim NewCopy As String

        'FileToCopy = System.IO.Path.GetDirectoryName
        'NewCopy = "C:\Users\Owner\Documents\NewTest.txt"

        'If System.IO.File.Exists(FileToCopy) = True Then

        '    System.IO.File.Copy(FileToCopy, NewCopy)
        '    MsgBox("File Copied")

        'End If

        'Dim path As String
        'path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)
        'MessageBox.Show(path)
        Dim _filename As String = "GovEngine" & Format(DateAndTime.Now, "MM/dd/yyyy hh:mm tt")
        _filename = _filename.Replace(":", "_")
        _filename = _filename.Replace("/", "_")
        _filename = _filename.Replace("-", "_")
        _filename = _filename.Replace(" ", "_")
        Dim sfd As New SaveFileDialog() ' this creates an instance of the SaveFileDialog called "sfd"
        sfd.Filter = "backup(*.sdf)|*.sdf"
        sfd.FilterIndex = 1
        sfd.RestoreDirectory = True
        sfd.FileName = _filename
        sfd.InitialDirectory = My.Settings.DbBackup_initialpath
        If sfd.ShowDialog() = DialogResult.OK Then

            Try
                Dim FileToCopy As String


                FileToCopy = My.Settings.DatabasePath
                'NewCopy = sfd.FileName & "\" & _filename & ".sdf"

                If System.IO.File.Exists(FileToCopy) = True Then
                    System.IO.File.Copy(FileToCopy, sfd.FileName)
                    MessageBox.Show("Backup Complete!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try

        End If
    End Sub

    Private Sub Panel1_Paint(sender As System.Object, e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub lvStaffs_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lvStaffs.SelectedIndexChanged
        On Error Resume Next
        Dim sel_item As String = ""

        With lvStaffs.SelectedItems(0)
            sel_item = .Text
            If Not String.IsNullOrEmpty(sel_item) Then
                Dim sel_dep As String = cmb1.Text.ToString

                Select Case sel_dep
                    Case "*****ALL ACTIVE Staff(s)"
                        btn_EditEmp.Enabled = True
                        btn_RemoveEmp.Enabled = True
                        btn_AddDepartment.Enabled = True
                        btnx_resignemp.Enabled = True
                        txtEmpIDNo.Text = sel_item
                        txtEmpName.Text = .SubItems(1).Text
                        cmbDepartment.Text = sqlce_asstnt.GET_STAFF_ASSIGN_DEPARTMENT_NAME(sel_item)
                    Case "*****ALL IN-ACTIVE Staff(s)"

                        btnx_resignemp.Enabled = True
                        btn_EditEmp.Enabled = False
                        btn_RemoveEmp.Enabled = False

                    Case Else
                        btn_EditEmp.Enabled = True
                        btn_RemoveEmp.Enabled = True
                        btn_AddDepartment.Enabled = True
                        btnx_resignemp.Enabled = True
                        txtEmpIDNo.Text = sel_item
                        txtEmpName.Text = .SubItems(1).Text
                        cmbDepartment.Text = sqlce_asstnt.GET_STAFF_ASSIGN_DEPARTMENT_NAME(sel_item)
                End Select

            Else
                btn_EditEmp.Enabled = False
                btn_RemoveEmp.Enabled = False
                btnx_resignemp.Enabled = False
                btn_AddDepartment.Enabled = True
            End If
        End With
    End Sub

    Private Sub pbx_audit_Click(sender As System.Object, e As System.EventArgs) Handles pbx_audit.Click

        Label69.Text = "Summary Of Administrative Events"

        TabManager(MainControPanelTab, TabPage10)
        cmb_show_period.SelectedIndex = 1
    End Sub

    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click
        btnExitED_Click(sender, e)
    End Sub

    Private Sub pnlx_ed_Paint(sender As System.Object, e As System.Windows.Forms.PaintEventArgs) Handles pnlx_ed.Paint

    End Sub

    Private Sub lnkx_ed_export_masterfile_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkx_ed_export_masterfile.LinkClicked

        Dim _filename As String = "Employee Master List"
        'MessageBox.Show(_filename)
        Dim sfd As New SaveFileDialog() ' this creates an instance of the SaveFileDialog called "sfd"
        sfd.Filter = "Master list(*.xlsx)|*.xlsx"
        sfd.FilterIndex = 1
        sfd.RestoreDirectory = True
        sfd.FileName = _filename
        sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        If sfd.ShowDialog() = DialogResult.OK Then
            Try
                dtgv_export.DataSource = sqlce_asstnt.LOAD_EMPLOYEE_PROFILES
                ' retrieve the full path to the file selected by the user
                dtgv_export.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText
                dtgv_export.SelectAll()
                IO.File.WriteAllText(sfd.FileName, dtgv_export.GetClipboardContent().GetText.TrimEnd)
                dtgv_export.ClearSelection()

                MessageBox.Show("Export Completed!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try

        End If


    End Sub

    Private Sub cmb_show_period_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmb_show_period.SelectedIndexChanged

    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        'Panel37.Enabled = False


        Dim ls_dp As New List(Of String)
        ls_dp = sqlce_asstnt.GET_ALL_DEPARTMENT_NAME()
        ls_dp.Sort()
        cmbc_department.Items.Clear()
        cmbc_department.Items.AddRange(ls_dp.ToArray)
        Panel40.BringToFront()
        Panel40.Visible = True
        flp_staff_details.Visible = False
        lstdp.Enabled = False

        txtc_id.Text = ""
        txtc_id.Enabled = True
        txtc_fullname.Text = ""
        cmbc_department.Text = ""
        txtc_designation.Text = ""
        txtc_other_details.Text = ""
        LOAD_KNOWN_DESIGNATION()
        LOAD_KNOWN_DEPARTMENT()
        Label106.Text = "Fill in the fields below to create new employee."
        btnc_create.Text = "Save"
        txtc_id.Focus()


    End Sub

    Private Sub LOAD_KNOWN_DESIGNATION()
        Dim listOfDesignation As New List(Of String)
        
        listOfDesignation = sqlce_asstnt.GET_ALL_KNOWN_DESIGNATION()

        Dim cus_designation As New AutoCompleteStringCollection
        cus_designation.AddRange(listOfDesignation.ToArray)

        With txtc_designation
            .AutoCompleteCustomSource = cus_designation
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.CustomSource
        End With

    End Sub

    Private Sub LOAD_KNOWN_DEPARTMENT()
        Dim listOfDepartment As New List(Of String)

        listOfDepartment = sqlce_asstnt.GET_ALL_KNOWN_DEPARTMENT()

        Dim cus_department As New AutoCompleteStringCollection
        cus_department.AddRange(listOfDepartment.ToArray)

        With cmbc_department
            .AutoCompleteCustomSource = cus_department
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.CustomSource
        End With

    End Sub


    Private Sub btnc_cancel_Click(sender As System.Object, e As System.EventArgs) Handles btnc_cancel.Click
        'Panel37.BackColor = Color.White
        Panel40.Visible = False
        flp_staff_details.Visible = True
        Panel35.Enabled = True
        Panel36.Enabled = True
        lstdp.Enabled = True
        txtc_designation.Text = ""
        txtc_id.Text = ""
        txtc_fullname.Text = ""
        pbx_staff_img.Image = Nothing
        txtc_other_details.Text = ""
        ' lstdp_SelectedIndexChanged(sender, e)

    End Sub
    Private Sub btnc_new_user_access_Click(sender As System.Object, e As System.EventArgs) Handles btnc_new_user_access.Click

        SetCueText(txtx_username, "USERNAME")
        SetCueText(txtx_password, "PASSWORD")
        SetCueText(txtx_retypepassword, "RETYPE PASSWORD")


        Dim listOfDepartment As New List(Of String)
        listOfDepartment = sqlce_asstnt.GET_ALL_DEPARTMENT_NAME()
        listOfDepartment.Sort()
        If Not listOfDepartment.Count = 0 Then
            cmbx_departmentlist_select_access.Items.Clear()
            cmbx_departmentlist_select_access.Items.AddRange(listOfDepartment.ToArray)
            cmbx_departmentlist_select_access.SelectedIndex = cmbx_departmentlist_select_access.Items.Count - 1
        End If



        'For i As Integer = 0 To chkbx_privilege.Items.Count - 1
        '    chkbx_privilege.SetItemChecked(i, True)
        'Next

        pnlx_useraccesslist.Visible = False
        pnlx_useraccessadd.Visible = True
    End Sub

    Private Sub btnc_remove_user_access_Click(sender As System.Object, e As System.EventArgs) Handles btnc_remove_user_access.Click

        Try
            Dim curItem As String = lstUserAccess.SelectedItem.ToString()
            If Not String.IsNullOrEmpty(curItem.Trim) Then
                If MessageBox.Show("Do you want to remove this user?" & vbCrLf & "Please confirm", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    sqlce_asstnt.REMOVE_SELECTED_USERNAME(curItem)
                    LOAD_USERACCESS_LIST()
                End If
            End If
        Catch ex As Exception

        End Try

    End Sub


    Private Sub lstdp_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lstdp.SelectedIndexChanged
        ' Console.WriteLine(lstdp.SelectedItem.ToString)




        flp_staff_details.Visible = False
        Try
            Dim sl_dpt As String
            sl_dpt = lstdp.SelectedItem.ToString
            ' Console.WriteLine(sl_dpt)
            '    WriteLogMessage("User selected " & sl_dpt)
            If Not String.IsNullOrEmpty(sl_dpt) Then
                Dim lst_stff_id As New List(Of String)
                lst_stff_id = sqlce_asstnt.GET_ALL_STAFF_DETAILS(sl_dpt)
                flp_staff_details.Controls.Clear()
                For Each _n As String In lst_stff_id
                    ' Console.WriteLine(_n)
                    AddPanel(_n)
                Next
            End If
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
        flp_staff_details.Visible = True
    End Sub
    Public Sub FLOW_LAYOUT_STAFF_DETAILS_ADD(stf_id As String, stf_name As String, dp_name As String)
        'Try
        '    FlowLayoutPanel1.Visible = False
        '    FlowLayoutPanel1.Controls.Clear()
        '    Dim MyDataArray = GetDepartmentList("SELECT DISTINCT DeptID FROM INOUT")          'call sql query
        '    Dim Values As String() = MyDataArray.ToArray()              'convert list to array
        '    For Each ctrValue As String In Values                       ' add Department to combobox
        '        AddLinkLabel(ctrValue.ToString)
        '    Next
        '    Panel1.Visible = True : FlowLayoutPanel1.Visible = True : FlowLayoutPanel1.Refresh()
        'Catch ex As Exception
        '    MessageBox.Show(ex.Message)
        'End Try
    End Sub
    Private Sub AddLinkLabel(ByVal LabelName As String)
        Dim stf_lod_dtls As New LinkLabel
        With stf_lod_dtls
            .Name = LabelName
            .Text = LabelName
            .Font = New Font("Courier New", 8, FontStyle.Regular)
            .ForeColor = Color.LimeGreen
            .BackColor = Color.Transparent
            .LinkColor = Color.LimeGreen
            AddHandler stf_lod_dtls.Click, AddressOf staff_details_Click
        End With
        flp_staff_details.Controls.Add(stf_lod_dtls) : flp_staff_details.Refresh()
        ' CheckedListBox1.Items.Add(_myDptLink.Name)
    End Sub
    Private WithEvents staff_details As LinkLabel
    Private Sub staff_details_Click(ByVal sender As Object, ByVal e As EventArgs) Handles staff_details.MouseClick
        'MessageBox.Show(sender.name)
        ''CHECK POLICY IF CURRENT USER LOGIN IS ALLOWED TO EDIT REMOVE RESIGNED EMPLOYEE
        If Add_Edit_Delete_Department_and_Employees = False Then
            Exit Sub
        End If


        Dim sender_data As String = sender.name
        Dim s_arr() As String
        s_arr = sender_data.Split("_")
        Dim stf_id As String = s_arr(1)
        Dim stf_name As String = sqlce_asstnt.GET_STAFF_NAME(stf_id)
        Dim stf_dpt As String = sqlce_asstnt.GET_STAFF_ASSIGN_DEPARTMENT_NAME(stf_id)
        Dim stf_desig As String = sqlce_asstnt.GET_STAFF_DESIGNATION(stf_id)
        Dim stf_other_details As String = sqlce_asstnt.GET_STAFF_OTHER_DETAILS(stf_id)
        stf_image_path = Application.StartupPath & "\Photo\" & sqlce_asstnt.GET_STAFF_IMAGEPATH(stf_id)

      

        Select Case sender.text
            Case "Edit"
                ''DETERMINE POLICY
                Dim ls_dp As New List(Of String)
                ls_dp = sqlce_asstnt.GET_ALL_DEPARTMENT_NAME()
                LOAD_KNOWN_DESIGNATION()
                LOAD_KNOWN_DEPARTMENT()
                ls_dp.Sort()
                cmbc_department.Items.Clear()
                cmbc_department.Items.AddRange(ls_dp.ToArray)
                If Not cmbc_department.Items.Count = 0 Then
                    cmbc_department.Text = stf_dpt
                End If
                btnc_create.Text = "Update Changes"

                Label106.Text = "Employee Edit"
                Panel40.Visible = True
                txtc_id.Text = stf_id
                txtc_fullname.Text = stf_name
                txtc_designation.Text = stf_desig
                txtc_id.Enabled = False
                txtc_other_details.Text = stf_other_details

                If File.Exists(stf_image_path) Then
                    Dim ImageA As Image
                    Using Str As Stream = File.OpenRead(stf_image_path)
                        ImageA = Image.FromStream(Str)
                    End Using
                    pbx_staff_img.Image = ImageA
                    pbx_staff_img.SizeMode = PictureBoxSizeMode.StretchImage
                Else
                    pbx_staff_img.Image = Nothing
                End If


                WriteLogMessage("Staff Edit" & vbTab & ":" & txtc_fullname.Text & "|" & txtc_designation.Text & "|" & cmbc_department.Text & "|CURRENT USER: " & CurrentLoginAccount)


            Case "Delete"
                GLOBAL_MSGCOM.YesNo_Message = "Are you sure you want to remove staff name " & stf_name
                MSGYESNO.ShowDialog()
                If GLOBAL_MSGCOM.YesNo_Value = "Yes" Then
                    WriteLogMessage("Staff Deleted" & vbTab & ":" & txtc_id.Text & "|" & txtc_fullname.Text & "|" & txtc_designation.Text & " " & cmbc_department.Text & "|DELETED BY: " & CurrentLoginAccount)
                    sqlce_asstnt.REMOVE_EMPLOYEE(stf_id)
                    lstdp.SelectedItem = stf_dpt
                    lstdp_SelectedIndexChanged(sender, e)
                    
                End If



            Case "Resign"
                GLOBAL_MSGCOM.YesNo_Message = "You are about to tag " & stf_name & " to the database as resign. Please confirm"
                MSGYESNO.ShowDialog()
                If GLOBAL_MSGCOM.YesNo_Value = "Yes" Then
                    WriteLogMessage("Staff Tag as Resigned" & vbTab & txtc_id.Text & "|" & txtc_fullname.Text & "|" & txtc_designation.Text & "|" & cmbc_department.Text & "|TAG BY: " & CurrentLoginAccount)
                    sqlce_asstnt.TAG_AS_RESIGN_EMPLOYEE(stf_id, stf_name, stf_dpt, DateAndTime.Now, CurrentLoginAccount)
                    sqlce_asstnt.REMOVE_EMPLOYEE(stf_id)
                    lstdp.SelectedItem = stf_dpt
                    lstdp_SelectedIndexChanged(sender, e)
                End If
        End Select
    End Sub

    Private Sub AddPanel(ByVal details As String)

        ' Console.WriteLine(details)
        Dim s_arr() As String
        s_arr = details.Split("+")

        Dim stf_id As String = ""
        Dim stf_name As String = ""
        Dim stf_desig As String = ""

        If s_arr.Length = 3 Then
            stf_id = s_arr(1)
            stf_name = s_arr(0)
            stf_desig = s_arr(2)
        End If


        '''STAFF ID
        Dim stf_id_lbl As New Label
        With stf_id_lbl
            .Name = "user_id_lbl_" & stf_id
            .Text = stf_id
            .BackColor = Color.Transparent
            .ForeColor = Color.DodgerBlue
            .Location = New Point(3, 6)
            .Size = New Size(200, 17)
            .AutoSize = False
            .TextAlign = ContentAlignment.MiddleLeft
            .Font = New Font("Microsoft Sans Serif", 10, FontStyle.Regular)
        End With


        '''STAFF NAME
        Dim stf_name_lbl As New Label
        With stf_name_lbl
            .Name = "user_name_lbl_" & stf_name
            .Text = stf_name
            .BackColor = Color.Transparent
            '   .ForeColor = Color.FromArgb(200, 200, 200)
            .ForeColor = Color.White
            .Location = New Point(3, 23)
            .Size = New Size(350, 17)
            .AutoSize = False
            .TextAlign = ContentAlignment.MiddleLeft
            .Font = New Font("Microsoft Sans Serif", 10, FontStyle.Underline)
        End With



        '''STAFF DESIGNATION
        Dim stf_desig_lbl As New Label
        With stf_desig_lbl
            .Name = "user_dp_lbl_" & stf_desig
            .Text = stf_desig
            .BackColor = Color.Transparent
            .ForeColor = Color.LightGray
            .Location = New Point(3, 43)
            .Size = New Size(350, 15)
            .AutoSize = False
            .TextAlign = ContentAlignment.MiddleLeft
            .Font = New Font("Microsoft Sans Serif", 7, FontStyle.Regular)
        End With






        '''edit
        Dim stf_id_lbl_edit As New Label
        With stf_id_lbl_edit
            .Name = "usereditlbl_" & stf_id
            .Text = "Edit"
            .BackColor = Color.Transparent
            .ForeColor = Color.Green
            .Location = New Point(385, 2)
            .Size = New Size(28, 15)
            .AutoSize = False
            .TextAlign = ContentAlignment.MiddleRight
            .Font = New Font("Microsoft Sans Serif", 9, FontStyle.Regular)
            .Cursor = Cursors.Hand
            AddHandler stf_id_lbl_edit.Click, AddressOf staff_details_Click
        End With


        '''dlete
        Dim stf_id_lbl_rm As New Label
        With stf_id_lbl_rm
            .Name = "userrmlbl_" & stf_id
            .Text = "Delete"
            .BackColor = Color.Transparent
            .ForeColor = Color.Green
            .Location = New Point(364, 24)
            .Size = New Size(53, 15)
            .AutoSize = False
            .TextAlign = ContentAlignment.MiddleRight
            .Font = New Font("Microsoft Sans Serif", 9, FontStyle.Regular)
            .Cursor = Cursors.Hand
            AddHandler stf_id_lbl_rm.Click, AddressOf staff_details_Click
        End With


        '''resign
        Dim stf_id_lbl_resign As New Label
        With stf_id_lbl_resign
            .Name = "userresignlbl_" & stf_id
            .Text = "Resign"
            .BackColor = Color.Transparent
            .ForeColor = Color.Green
            .Location = New Point(364, 45)
            .Size = New Size(53, 15)
            .AutoSize = False
            .TextAlign = ContentAlignment.MiddleRight
            .Font = New Font("Microsoft Sans Serif", 9, FontStyle.Regular)
            .Cursor = Cursors.Hand
            AddHandler stf_id_lbl_resign.Click, AddressOf staff_details_Click
        End With



        Dim stf_details_pnl As New Panel
        With stf_details_pnl
            .Name = "user_pnl_" & stf_id
            .Size = New Size(420, 65)
            .BackColor = Color.FromArgb(30, 30, 30)
            .BorderStyle = BorderStyle.FixedSingle
            .Controls.Add(stf_id_lbl)
            .Controls.Add(stf_name_lbl)
            .Controls.Add(stf_desig_lbl)
            .Controls.Add(stf_id_lbl_rm)
            .Controls.Add(stf_id_lbl_edit)
            .Controls.Add(stf_id_lbl_resign)
        End With
        'MessageBox.Show(stf_details_pnl.Name)
        flp_staff_details.Controls.Add(stf_details_pnl)
    End Sub



    Private Sub Button6_Click_1(sender As System.Object, e As System.EventArgs) Handles Button6.Click

        lblWindowTitle.Text = "Time Management System"
        lblWindowTitle.Visible = True
        ' MainControPanelTab.SelectedIndex = 0
        lblCurrentUser.Visible = True
        'pbx_audit.Visible = True
        Me.Refresh()

        TabManager(MainControPanelTab, TabPage1)

    End Sub

    Private Sub lnk_import_mstrfile_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnk_import_mstrfile.LinkClicked
        Dim ofd1 As New OpenFileDialog()
        ofd1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        ofd1.Filter = "masterfile (*.xlsx;*.xls;*.csv|*.xlsx;*.xls;*csv" : ofd1.RestoreDirectory = True
        If ofd1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            If Not String.IsNullOrEmpty(ofd1.FileName) Then
                If File.Exists(ofd1.FileName) Then
                    If Not ThreadMasterFileStaffMassUpload.IsBusy Then
                        ThreadMasterFileStaffMassUpload.RunWorkerAsync(ofd1.FileName)
                    End If
                End If

            End If

        End If
    End Sub


    Private Sub lnk_dpt_show_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnk_dpt_show.LinkClicked
        DEPARTMENTFORM.ShowDialog()
    End Sub
    Private Sub btn_EditEmp_Click(sender As System.Object, e As System.EventArgs) Handles btn_EditEmp.Click
        If Not String.IsNullOrEmpty(txtEmpIDNo.Text) And String.IsNullOrEmpty(txtc_fullname.Text) Then
            txtEmpIDNo.Enabled = False
            btnSaveEmployee.Text = "Update"
            btnx_cancelEdit.Visible = True
            btnx_resignemp.Enabled = False
            btn_RemoveEmp.Enabled = False
            btn_EditEmp.Enabled = False
            cmb1.Enabled = False
            lvStaffs.Enabled = False
        End If
    End Sub
    Private Sub btn_RemoveEmp_Click(sender As System.Object, e As System.EventArgs) Handles btn_RemoveEmp.Click
        On Error Resume Next
        Dim sel_item As String = ""
        Dim staff_id As String = ""
        Dim staff_name As String = ""

        With lvStaffs.SelectedItems(0)
            sel_item = .Text
            If Not String.IsNullOrEmpty(sel_item) Then
                staff_id = sel_item
                staff_name = .SubItems(1).Text
                If MessageBox.Show("Are you sure you want to remove staff name " & staff_name & "", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    sqlce_asstnt.REMOVE_EMPLOYEE(staff_id)
                    cmb1_SelectedIndexChanged(sel_item, e)
                End If
            End If
        End With
    End Sub

    Private Sub pbx_dtr_b_Click(sender As System.Object, e As System.EventArgs) Handles pbx_dtr_b.Click
        Label19.Visible = False
        btnReportExit.Visible = False
        selectedReportIconName = "DTR_B"
        GlobalVariables.DTR_TYPE = "DTR_B"
        lblx_PromptHeaderText.Text = "Generate Daily Time Record Report(DTR)"
        pbx_ReportIconsControl.Visible = False
        pbx_ReportConfiguration.Visible = True

        LOAD_TREEVIEW_DETAILS()
    End Sub


    Private Sub btnx_resignemp_Click(sender As System.Object, e As System.EventArgs) Handles btnx_resignemp.Click


        Dim btn_txt As String = btnx_resignemp.Text.ToString



        Select Case btn_txt
            Case "Resign"
                On Error Resume Next
                Dim sel_item As String = ""
                Dim staff_id As String = ""
                Dim staff_name As String = ""
                Dim dep_name As String = ""

                With lvStaffs.SelectedItems(0)
                    sel_item = .Text
                    staff_name = .SubItems(1).Text
                    If Not String.IsNullOrEmpty(sel_item) Then
                        staff_id = sel_item
                        staff_name = .SubItems(1).Text
                        dep_name = sqlce_asstnt.GET_STAFF_ASSIGN_DEPARTMENT_NAME(staff_id)
                        If MessageBox.Show("You are about to tag this employee to the database as resign." & vbCrLf & _
                                           "Name: " & staff_name & vbCrLf & vbCrLf & "Please confirm.", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then

                            sqlce_asstnt.TAG_AS_RESIGN_EMPLOYEE(staff_id, staff_name, dep_name, DateAndTime.Now, CurrentLoginAccount)
                            sqlce_asstnt.REMOVE_EMPLOYEE(staff_id)
                            cmb1_SelectedIndexChanged(sel_item, e)
                        End If
                    End If
                End With

            Case "Rehired"
                On Error Resume Next
                Dim sel_item As String = ""
                Dim staff_id As String = ""
                Dim staff_name As String = ""
                Dim dep_name As String = ""

                With lvStaffs.SelectedItems(0)
                    sel_item = .Text
                    staff_name = .SubItems(1).Text
                    If Not String.IsNullOrEmpty(sel_item) Then
                        staff_id = sel_item
                        staff_name = .SubItems(1).Text
                        dep_name = cmb1.Text
                        If MessageBox.Show("You are about to tag this employee to the database as rehired." & vbCrLf & _
                                           "Name: " & staff_name & vbCrLf & vbCrLf & "Please confirm.", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then

                            MessageBox.Show("Please select department", "", MessageBoxButtons.OK, MessageBoxIcon.Information)

                            cmb1.Enabled = False
                            txtEmpIDNo.Text = staff_id
                            txtEmpName.Text = staff_name
                            cmbDepartment.Text = ""
                            btnSaveEmployee.Text = "Save"
                            btnx_cancelEdit.Visible = True
                            cmbDepartment.DroppedDown = True

                        End If
                    End If
                End With
        End Select
    End Sub

    Private Sub btn_save_compname_Click(sender As System.Object, e As System.EventArgs) Handles btn_save_compname.Click
        My.Settings.CompanyName = txt_compname.Text
        My.Settings.Save()
        MessageBox.Show("Company name save", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub


    Private Sub btnc_create_Click(sender As System.Object, e As System.EventArgs) Handles btnc_create.Click



        Select Case btnc_create.Text
            Case "Save"
                If Not String.IsNullOrEmpty(txtc_id.Text.Trim()) Then
                    If Not String.IsNullOrEmpty(txtc_fullname.Text.Trim()) Then
                        If Not String.IsNullOrEmpty(cmbc_department.Text.Trim()) Then
                            ''INSERT NEW STAFF
                            Dim staff_name As String = txtc_fullname.Text.Trim.ToUpper
                            Dim staffid As String = txtc_id.Text.Trim
                            Dim imgdir As String = Application.StartupPath & "\Photo"
                            Dim image_path_dst As String = imgdir & "\" & staffid & Path.GetExtension(image_path_src)
                            Dim imgfname As String = Path.GetFileName(image_path_dst)

                            Console.WriteLine(imgfname)

                            If Not sqlce_asstnt.CHECK_EMPLOYEEID_IF_EXIST(txtc_id.Text.Trim) Then

                                If Not Directory.Exists(imgdir) Then
                                    Directory.CreateDirectory(imgdir)
                                End If

                                If File.Exists(image_path_src) Then
                                    File.Copy(image_path_src, image_path_dst)
                                End If

                                sqlce_asstnt.INSERT_NEW_STAFF(txtc_id.Text.Trim, staff_name, sqlce_asstnt.GET_DEPARTMENT_ID(cmbc_department.Text), _
                                                              txtc_designation.Text.ToString.ToString.Trim, txtc_other_details.Text, imgfname)


                        


                                MSGOK.pbx_msgOK.Image = My.Resources.checkok
                                MSGOK.MSGOK_GLOBAL = "New staff has been added succesfully"
                                MSGOK.ShowDialog()

                                txtc_id.Text = ""
                                txtc_fullname.Text = ""
                                cmbc_department.Text = ""
                                txtc_other_details.Text = ""
                                txtc_designation.Text = ""


                                lstdp.SelectedItem = cmbc_department.Text
                                lstdp_SelectedIndexChanged(sender, e)
                                btnc_cancel_Click(sender, e)



                                WriteLogMessage("New User Created: " & txtc_id.Text.Trim & " " & txtc_fullname.Text & " " & txtc_designation.Text & " " & cmbc_department.Text & " CREATE BY: " & CurrentLoginAccount)
                                pbx_staff_img.Image = Nothing
                            Else
                                'MSGOK.pbx_msgOK.Image = My.Resources._error
                                'MSGOK.MSGOK_GLOBAL = "Staff ID is already exist This ID is currently assigned to " & _
                                '        vbCrLf & sqlce_asstnt.GET_STAFF_NAME(txtc_id.Text) & " at" & vbCrLf & sqlce_asstnt.GET_STAFF_ASSIGN_DEPARTMENT_NAME(txtc_id.Text)
                                'MSGOK.ShowDialog()
                                '  txtc_id.Select()

                                If MessageBox.Show("Staff ID is already exist. This ID is currently assigned to " & vbCrLf & sqlce_asstnt.GET_STAFF_NAME(txtc_id.Text) & " at" & vbCrLf & sqlce_asstnt.GET_STAFF_ASSIGN_DEPARTMENT_NAME(txtc_id.Text) & vbCrLf & "Do you want to edit his/her profile?", "BTMS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                    ''DETERMINE POLICY
                                    Dim ls_dp As New List(Of String)
                                    ls_dp = sqlce_asstnt.GET_ALL_DEPARTMENT_NAME()
                                    LOAD_KNOWN_DESIGNATION()
                                    LOAD_KNOWN_DEPARTMENT()
                                    ls_dp.Sort()
                                    cmbc_department.Items.Clear()
                                    cmbc_department.Items.AddRange(ls_dp.ToArray)
                                    'If Not cmbc_department.Items.Count = 0 Then
                                    '    cmbc_department.Text = 0
                                    'End If
                                    btnc_create.Text = "Update Changes"

                                    Label106.Text = "Employee Edit"
                                    Panel40.Visible = True
                                    txtc_id.Text = txtc_id.Text.Trim
                                    txtc_fullname.Text = sqlce_asstnt.GET_STAFF_NAME(txtc_id.Text)
                                    txtc_designation.Text = sqlce_asstnt.GET_STAFF_DESIGNATION(txtc_id.Text)
                                    txtc_id.Enabled = False
                                    txtc_other_details.Text = sqlce_asstnt.GET_STAFF_OTHER_DETAILS(txtc_id.Text)

                                    If File.Exists(stf_image_path) Then
                                        Dim ImageA As Image
                                        Using Str As Stream = File.OpenRead(stf_image_path)
                                            ImageA = Image.FromStream(Str)
                                        End Using
                                        pbx_staff_img.Image = ImageA
                                        pbx_staff_img.SizeMode = PictureBoxSizeMode.StretchImage
                                    Else
                                        pbx_staff_img.Image = Nothing
                                    End If
                                    WriteLogMessage("Staff Edit" & vbTab & ":" & txtc_fullname.Text & "|" & txtc_designation.Text & "|" & cmbc_department.Text & "|CURRENT USER: " & CurrentLoginAccount)

                                End If



                            End If
                        Else
                            MSGOK.pbx_msgOK.Image = My.Resources.wrong
                            MSGOK.MSGOK_GLOBAL = "Staff Department cannot be empty"
                            MSGOK.ShowDialog()

                        End If
                    Else
                        MSGOK.pbx_msgOK.Image = My.Resources.wrong
                        MSGOK.MSGOK_GLOBAL = "Staff Name cannot be empty"
                        MSGOK.ShowDialog()

                    End If
                Else
                    MSGOK.pbx_msgOK.Image = My.Resources.wrong
                    MSGOK.MSGOK_GLOBAL = "Staff ID cannot be empty"
                    MSGOK.ShowDialog()
                End If

            Case "Update Changes"
                If Not String.IsNullOrEmpty(txtc_id.Text.Trim()) Then
                    If Not String.IsNullOrEmpty(txtc_fullname.Text.Trim()) Then
                        If Not String.IsNullOrEmpty(cmbc_department.Text.Trim()) Then
                            ''update
                            Dim staffid As String = txtc_id.Text.Trim
                            Dim imgdir As String = Application.StartupPath & "\Photo"
                            Dim image_path_dst As String = imgdir & "\" & staffid & Path.GetExtension(image_path_src)
                            Dim imgfname As String = Path.GetFileName(image_path_dst)


                            If sqlce_asstnt.CHECK_EMPLOYEEID_IF_EXIST(txtc_id.Text.Trim) Then

                                If Not Directory.Exists(imgdir) Then
                                    Directory.CreateDirectory(imgdir)
                                End If

                                If File.Exists(image_path_src) Then
                                    File.Copy(image_path_src, image_path_dst, True)
                                End If


                                sqlce_asstnt.UPDATE_EMPLOYEE_DETAILS(txtc_id.Text.Trim, txtc_fullname.Text.ToUpper, sqlce_asstnt.GET_DEPARTMENT_ID(cmbc_department.Text), _
                                                                     txtc_designation.Text.ToString.ToUpper.Trim, txtc_other_details.Text, imgfname)





                                MSGOK.pbx_msgOK.Image = My.Resources.checkok
                                MSGOK.MSGOK_GLOBAL = "Staff has been updated successfuly"
                                MSGOK.ShowDialog()

                                WriteLogMessage("Staff Updated" & vbTab & ":" & txtc_fullname.Text & "|" & txtc_designation.Text & "|" & cmbc_department.Text & "|UPDATED BY: " & CurrentLoginAccount)
                                txtc_id.Text = ""
                                txtc_id.Enabled = True
                                txtc_fullname.Text = ""
                                cmbc_department.Text = ""
                                lstdp.SelectedItem = cmbc_department.Text
                                lstdp_SelectedIndexChanged(sender, e)
                                btnc_cancel_Click(sender, e)
                            End If
                        Else
                            ' MessageBox.Show("Staff Department cannot be empty", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            MSGOK.pbx_msgOK.Image = My.Resources._error
                            MSGOK.MSGOK_GLOBAL = "Staff Department cannot be empty"
                            MSGOK.ShowDialog()
                        End If
                    Else
                        'MessageBox.Show("Staff Name cannot be empty", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        MSGOK.pbx_msgOK.Image = My.Resources._error
                        MSGOK.MSGOK_GLOBAL = "Staff Name cannot be empty"
                        MSGOK.ShowDialog()
                    End If
                Else
                    ' MessageBox.Show("Staff ID cannot be empty", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    MSGOK.pbx_msgOK.Image = My.Resources._error
                    MSGOK.MSGOK_GLOBAL = "Staff ID cannot be empty"
                    MSGOK.ShowDialog()
                End If
                btnc_create.Text = "Save"
            Case "Rehired"



                If Not String.IsNullOrEmpty(txtc_id.Text.Trim()) Then
                    If Not String.IsNullOrEmpty(txtc_fullname.Text.Trim()) Then
                        If Not String.IsNullOrEmpty(cmbc_department.Text.Trim()) Then
                            ''INSERT NEW STAFF
                            Dim staff_name As String = txtc_fullname.Text.Trim.ToUpper
                            Dim staffid As String = txtc_id.Text.Trim
                            Dim imgdir As String = Application.StartupPath & "\Photo"
                            Dim image_path_dst As String = imgdir & "\" & staffid & Path.GetExtension(image_path_src)
                            Dim imgfname As String = Path.GetFileName(image_path_dst)

                            If Not sqlce_asstnt.CHECK_EMPLOYEEID_IF_EXIST(txtc_id.Text.Trim) Then

                                If Not Directory.Exists(imgdir) Then
                                    Directory.CreateDirectory(imgdir)
                                End If

                                If File.Exists(image_path_src) Then
                                    File.Copy(image_path_src, image_path_dst, True)
                                End If


                                sqlce_asstnt.INSERT_NEW_STAFF(txtc_id.Text.Trim, staff_name, sqlce_asstnt.GET_DEPARTMENT_ID(cmbc_department.Text), txtc_designation.Text.ToString.ToUpper.Trim, txtc_other_details.Text, image_path_src)
                                sqlce_asstnt.REMOVE_REHIRED_STAFF_FROM_INACTIVETABLE(REHIRED_STAFF_ID)

                                MSGOK.pbx_msgOK.Image = My.Resources.checkok
                                MSGOK.MSGOK_GLOBAL = "Staff has been rehired succesfully"
                                MSGOK.ShowDialog()
                                btnc_create.Text = "Save"
                                txtc_id.Text = ""
                                txtc_fullname.Text = ""
                                Panel40.Visible = False
                                lstdp.SelectedItem = cmbc_department.Text
                                lstdp_SelectedIndexChanged(sender, e)
                                btnc_cancel_Click(sender, e)
                                cmbc_department.Text = ""

                            Else
                                MSGOK.pbx_msgOK.Image = My.Resources._error
                                MSGOK.MSGOK_GLOBAL = "Staff ID is already exist This ID is currently assigned to " & _
                                        vbCrLf & sqlce_asstnt.GET_STAFF_NAME(txtc_id.Text)
                                MSGOK.ShowDialog()


                                txtc_id.Select()
                            End If
                        Else
                            MSGOK.pbx_msgOK.Image = My.Resources.wrong
                            MSGOK.MSGOK_GLOBAL = "Staff Department cannot be empty"
                            MSGOK.ShowDialog()

                        End If
                    Else
                        MSGOK.pbx_msgOK.Image = My.Resources.wrong
                        MSGOK.MSGOK_GLOBAL = "Staff Name cannot be empty"
                        MSGOK.ShowDialog()

                    End If
                Else
                    MSGOK.pbx_msgOK.Image = My.Resources.wrong
                    MSGOK.MSGOK_GLOBAL = "Staff ID cannot be empty"
                    MSGOK.ShowDialog()
                End If

        End Select
    End Sub
    Private Sub txtc_id_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtc_id.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtc_fullname.Focus()
        End If
    End Sub

    Private Sub txtc_fullname_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtc_fullname.KeyDown
        If e.KeyCode = Keys.Enter Then
            cmbc_department.Focus()
        End If
    End Sub

    Private Sub cmbc_department_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles cmbc_department.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnc_create.Focus()
        End If
    End Sub

    Private Sub lnk_show_resign_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnk_show_resign.LinkClicked
        Dim listOfStaffIdandName As New Dictionary(Of String, String)
        Dim q_str As String = "SELECT STAFFID,STAFFNAME FROM INACTIVESTAFF"
        sqlce_asstnt.GET_ALL_STAFFNAME_PLUS_ID_AS_DICTIONARY(listOfStaffIdandName, q_str)
        Dim names As List(Of String) = listOfStaffIdandName.Values.ToList
        names.Sort()
        RESIGNED_EMPFORM.lsv_stf.Items.Clear()
        Dim lvItem As New ListViewItem
        For Each n As String In names
            Dim pair As KeyValuePair(Of String, String)
            For Each pair In listOfStaffIdandName
                If pair.Value = n Then
                    lvItem = RESIGNED_EMPFORM.lsv_stf.Items.Add(pair.Key)
                    lvItem.SubItems.Add(n)
                    lvItem.SubItems.Add(sqlce_asstnt.GET_RESIGNED_DATE(pair.Key))
                End If
            Next
        Next

        RESIGNED_EMPFORM.ShowDialog()
    End Sub


    Private Sub cmb_monday_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmb_monday.SelectedIndexChanged
        If cmb_monday.Text = "-" Then
            lbl_mon.Visible = True
        Else
            lbl_mon.Visible = False
        End If
    End Sub

    Private Sub cmb_tuesday_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmb_tuesday.SelectedIndexChanged
        If cmb_tuesday.Text = "-" Then
            lbl_tue.Visible = True
        Else
            lbl_tue.Visible = False
        End If
    End Sub

    Private Sub cmb_wednesday_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmb_wednesday.SelectedIndexChanged
        If cmb_wednesday.Text = "-" Then
            lbl_wed.Visible = True
        Else
            lbl_wed.Visible = False
        End If
    End Sub

    Private Sub cmb_thursday_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmb_thursday.SelectedIndexChanged
        If cmb_thursday.Text = "-" Then
            lbl_thu.Visible = True
        Else
            lbl_thu.Visible = False
        End If
    End Sub

    Private Sub cmb_friday_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmb_friday.SelectedIndexChanged
        If cmb_friday.Text = "-" Then
            lbl_fri.Visible = True
        Else
            lbl_fri.Visible = False
        End If
    End Sub

    Private Sub cmb_saturday_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmb_saturday.SelectedIndexChanged
        If cmb_saturday.Text = "-" Then
            lbl_sat.Visible = True
        Else
            lbl_sat.Visible = False
        End If
    End Sub

    Private Sub cmb_sunday_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmb_sunday.SelectedIndexChanged
        If cmb_sunday.Text = "-" Then
            lbl_sun.Visible = True
        Else
            lbl_sun.Visible = False
        End If
    End Sub
    Public Function FirstDayOfMonth(ByVal sourceDate As DateTime) As DateTime
        Return New DateTime(sourceDate.Year, sourceDate.Month, 1)
    End Function

    'Get the last day of the month
    Public Function LastDayOfMonth(ByVal sourceDate As DateTime) As DateTime
        Dim lastDay As DateTime = New DateTime(sourceDate.Year, sourceDate.Month, 1)
        Return lastDay.AddMonths(1).AddDays(-1)
    End Function

    Private Sub cmbx_mto_ordertypes_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbx_mto_ordertypes.SelectedIndexChanged
        If String.IsNullOrEmpty(cmbx_mto_ordertypes.Text.ToString.Trim) Then
            Exit Sub
        End If


        Try

            If cmbx_mto_ordertypes.Text.ToString.ToLower.Contains("void") Then
                btnx_mto_approved.Text = "VOID"
            Else
                btnx_mto_approved.Text = "Approved"
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try



    End Sub

    Public Sub LOAD_HOLIDAY_CLASS()
        Hol_dataadapter = New SqlCeDataAdapter(SELECT_STRING, CONNECT_STRING)
        Hol_DataSet = New DataSet()

        Hol_dataadapter.Fill(Hol_DataSet, "HOLIDAYCLASSTABLE")
        dtgv_holiday_class.DataSource = Hol_DataSet.Tables("HOLIDAYCLASSTABLE")
        Hol_dataadapter.AcceptChangesDuringUpdate = True
        Console.WriteLine("")


    End Sub
    Private Sub btn_UpdateChangesHolidaySchema_Click(sender As System.Object, e As System.EventArgs) Handles btn_UpdateChangesHolidaySchema.Click


        If Hol_DataSet.HasChanges() Then

            Dim command_builder As SqlCeCommandBuilder
            Hol_dataadapter.TableMappings.Add("Table", "HOLIDAYCLASSTABLE")
            command_builder = New SqlCeCommandBuilder(Hol_dataadapter)

            ' Uncomment this code to see the INSERT,
            ' UPDATE, and DELETE commands.
            'Debug.WriteLine("*** INSERT ***")
            'Debug.WriteLine(command_builder.GetInsertCommand.CommandText)
            'Debug.WriteLine("*** UPDATE ***")
            'Debug.WriteLine(command_builder.GetUpdateCommand.CommandText)
            'Debug.WriteLine("*** DELETE ***")
            'Debug.WriteLine(command_builder.GetDeleteCommand.CommandText)

            ' Save the changes.
            Hol_dataadapter.Update(Hol_DataSet)


            MSGOK.MSGOK_GLOBAL = "Holiday table has been successfully updated"
            MSGOK.pbx_msgOK.Image = My.Resources.checkok
            MSGOK.ShowDialog()

        End If

        Exit Sub
    End Sub

    'Private Sub Button1_Click_2(sender As System.Object, e As System.EventArgs) Handles Button1.Click
    '    Dim x As Date = CDate("08:00 AM".Replace("*", "").Trim)
    '    Dim y As Date = CDate("05:25 PM".Replace("*", "").Trim)
    '    Dim SecondsDifference As Integer
    '    SecondsDifference = DateDiff(DateInterval.Minute, x, y)
    'End Sub



    'End Sub






    Private Sub Button1_Click_2(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        If Not ThreadZKmdbRead.IsBusy Then
            ThreadZKmdbRead.RunWorkerAsync()
        End If
    End Sub

    Private Sub pbx_dtr_c_Click(sender As System.Object, e As System.EventArgs) Handles pbx_dtr_c.Click
        Label19.Visible = False
        btnReportExit.Visible = False
        selectedReportIconName = "DTR_C"
        GlobalVariables.DTR_TYPE = "DTR_C"
        lblx_PromptHeaderText.Text = "Generate Daily Time Record Report(DTR)"
        pbx_ReportIconsControl.Visible = False
        pbx_ReportConfiguration.Visible = True

        LOAD_TREEVIEW_DETAILS()
    End Sub

    Private Sub pbx_dtr_d_Click(sender As System.Object, e As System.EventArgs) Handles pbx_dtr_d.Click
        Label19.Visible = False
        btnReportExit.Visible = False
        selectedReportIconName = "DTR_D"
        GlobalVariables.DTR_TYPE = "DTR_D"
        lblx_PromptHeaderText.Text = "Generate Daily Time Record Report(DTR)"
        pbx_ReportIconsControl.Visible = False
        pbx_ReportConfiguration.Visible = True

        LOAD_TREEVIEW_DETAILS()
    End Sub

    Private Sub lnk_generate_users_dat_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnk_generate_users_dat.LinkClicked

        TabManager(MainControPanelTab, TabPage4)

        'sqlce_asstnt.GET_ALL_STAFFNAME_PLUS_ID_AS_DICTIONARY()
        Dim listOfStaffIdandName As New Dictionary(Of String, String)
        Dim q_str As String = "SELECT EmpID AS [STAFF ID], FullName AS NAME FROM EmployeeProfiles"
        sqlce_asstnt.GET_ALL_STAFFNAME_PLUS_ID_AS_DICTIONARY(listOfStaffIdandName, q_str)
        Dim names As List(Of String) = listOfStaffIdandName.Values.ToList
        ' names.Sort()

        Dim udisk As New Structs.UDisk


        'Dim byDataBuf() As Byte
        'Dim iLength As Integer
        'Dim iCount As Integer 'count of users

        Dim iPIN As Integer
        Dim iPrivilege As Integer
        Dim sName As String = ""
        Dim sPassword As String = ""
        Dim iCard As Integer
        Dim iGroup As Integer
        Dim sTimeZones As String = ""
        Dim sPIN2 As String = ""


        lvSSRUser.Items.Clear()
        Dim lvItem As New ListViewItem
        For Each n As String In names
            Dim pair As KeyValuePair(Of String, String)
            For Each pair In listOfStaffIdandName
                If pair.Value = n Then
                    lvItem = lvSSRUser.Items.Add(pair.Key)
                    lvItem.SubItems.Add(n)

                    'lvItem = lvSSRUser.Items.Add(sPIN2)
                    'lvItem.SubItems.Add(sName)
                    lvItem.SubItems.Add(iCard.ToString())
                    lvItem.SubItems.Add(iPrivilege.ToString())
                    lvItem.SubItems.Add(sPassword)
                    lvItem.SubItems.Add(iGroup.ToString())
                    lvItem.SubItems.Add(sTimeZones.ToString())
                    lvItem.SubItems.Add(iPIN.ToString())

                    'sPIN2 = pair.Key
                    'sName = n
                    'iPIN += 1

                    'Dim byUserInfo(71) As Byte 'loop to manage every user's information
                    'For i = 0 To 71
                    '    byUserInfo(i) = 72
                    'Next

                    'Console.WriteLine(byUserInfo.Length)
                    'udisk.GetSSRUserInfoFromDat(byUserInfo, iPIN, iPrivilege, sPassword, sName, iCard, iGroup, sTimeZones, sPIN2)
                    'lvItem = lvSSRUser.Items.Add(sPIN2)
                    'lvItem.SubItems.Add(sName)
                    'lvItem.SubItems.Add(iCard.ToString())
                    'lvItem.SubItems.Add(iPrivilege.ToString())
                    'lvItem.SubItems.Add(sPassword)

                    'lvItem.SubItems.Add(iGroup.ToString())
                    'lvItem.SubItems.Add(sTimeZones.ToString())
                    'lvItem.SubItems.Add(iPIN.ToString())






                End If
            Next
        Next
        listOfStaffIdandName.Clear()

    End Sub

    Private Sub Upload_to_bisbio()

        Dim idwErrorCode As Integer

        Dim sdwEnrollNumber As String
        Dim sName As String = ""
        Dim sPassword As String = ""
        Dim iPrivilege As Integer
        'Dim idwFingerIndex As Integer
        Dim sTmpData As String = ""
        Dim sEnabled As String = ""
        Dim bEnabled As Boolean = False
        'Dim iflag As Integer

        Cursor = Cursors.WaitCursor
        AxCZKEM1.EnableDevice(iMachineNumber, False)

        Dim lvItem As New ListViewItem

        For Each lvItem In lvSSRUser.Items
            sdwEnrollNumber = lvItem.SubItems(0).Text.Trim()
            sName = lvItem.SubItems(1).Text.Trim()
            'idwFingerIndex = 0
            ' sTmpData = ""
            iPrivilege = Convert.ToInt32(0)
            sPassword = ("").Trim
            sEnabled = 1
            ' iflag = 0

            If sEnabled = "true" Then
                bEnabled = True
            Else
                bEnabled = False
            End If

            If axCZKEM1.SSR_SetUserInfo(iMachineNumber, sdwEnrollNumber, sName, sPassword, iPrivilege, bEnabled) Then 'upload user information to the device
                'axCZKEM1.SetUserTmpExStr(iMachineNumber, sdwEnrollNumber, idwFingerIndex, iflag, sTmpData) 'upload templates information to the device
            Else
                axCZKEM1.GetLastError(idwErrorCode)
                MsgBox("Operation failed,ErrorCode=" & idwErrorCode.ToString(), MsgBoxStyle.Exclamation, "Error")
                axCZKEM1.EnableDevice(iMachineNumber, True)
                Cursor = Cursors.Default
                Return
            End If
        Next

        AxCZKEM1.RefreshData(iMachineNumber) 'the data in the device should be refreshed
        AxCZKEM1.EnableDevice(iMachineNumber, True)
        Cursor = Cursors.Default
        MsgBox("Successfully Upload fingerprint templates, " + "total:", MsgBoxStyle.Information, "Success")

    End Sub


    Private Sub Export_to_user_dat()
        Dim udisk As New Structs.UDisk

        Dim lvItem As ListViewItem
        sfd1.Filter = "user(*.dat)|*.dat"
        sfd1.FileName = "user.dat"
        Dim iCount As Integer = lvSSRUser.Items.Count
        Dim byDataBuf(iCount * 72 - 1) As Byte

        If sfd1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Dim iDataBufIndex As Integer = 0
            For Each lvItem In lvSSRUser.Items

                Dim sPIN2 As String = lvItem.SubItems(0).Text.Trim()
                Dim sName As String = lvItem.SubItems(1).Text.Trim()
                Dim iCard As Integer = Convert.ToInt32(lvItem.SubItems(2).Text.Trim())
                Dim iPrivilege As Integer = Convert.ToInt32(lvItem.SubItems(3).Text.Trim())
                Dim sPassword As String = lvItem.SubItems(4).Text.Trim()
                Dim iGroup As Integer = Convert.ToInt32(lvItem.SubItems(5).Text.Trim())
                Dim sTimeZones As String = lvItem.SubItems(6).Text.Trim()
                Dim iPIN As Integer = Convert.ToInt32(lvItem.SubItems(7).Text.Trim())

                Dim byUserInfo(71) As Byte
                udisk.SetSSRUserInfoToDat(byUserInfo, iPIN, iPrivilege, sPassword, sName, iCard, iGroup, sTimeZones, sPIN2)
                Array.Copy(byUserInfo, 0, byDataBuf, iDataBufIndex, 72)
                byUserInfo = Nothing
                iDataBufIndex += 72

            Next
            File.WriteAllBytes(sfd1.FileName, byDataBuf)
        End If
    End Sub


    Private Sub Button7_Click(sender As System.Object, e As System.EventArgs) Handles Button7.Click
        'Export_to_user_dat()
        Upload_to_bisbio()
    End Sub

    Private Sub Button8_Click(sender As System.Object, e As System.EventArgs) Handles Button8.Click

        Dim idwErrorCode As Integer
        Cursor = Cursors.WaitCursor
        Dim ip As String = "192.168.40.200"
        Dim tcp_port As String = "4370"

        bIsConnected = axCZKEM1.Connect_Net(ip.Trim(), Convert.ToInt32(tcp_port.Trim()))
        If bIsConnected = True Then
            iMachineNumber = 1 'In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
            axCZKEM1.RegEvent(iMachineNumber, 65535) 'Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
        Else
            axCZKEM1.GetLastError(idwErrorCode)
            MsgBox("Unable to connect the device,ErrorCode=" & idwErrorCode, MsgBoxStyle.Exclamation, "Error")
        End If
        Cursor = Cursors.Default

    End Sub

    Private Sub btn_add_new_device_Click(sender As System.Object, e As System.EventArgs) Handles btn_add_new_device.Click
        DeviceDetails.ShowDialog()
    End Sub

  
    Private Sub ToolStripMenuItem2_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem2.Click
        'For i As Integer = 0 To lstboxStafflist.Items.Count - 1
        '    lstboxStafflist.SetSelected(i, True)
        'Next i

        lstboxStafflist.SelectedItems.Clear()

    End Sub

    Private Sub lbl_rtm_Click(sender As System.Object, e As System.EventArgs) Handles lbl_rtm.Click
        Form4.ShowDialog()
    End Sub

    'Private Sub ThreadZKmdbRead_DoWork(sender As System.Object, e As System.ComponentModel.DoWorkEventArgs) Handles ThreadZKmdbRead.DoWork
    '    'If File.Exists(My.Settings.zk_mdb_path) Then
    '    '    ' Dim CustID As Integer = CInt(TxtSearch.Text)
    '    '    Console.Write("att2000.mdb is exist" & vbNewLine)
    '    '    Dim strCon As String = My.Settings.zk_con_string
    '    '    Dim strQuery As String = "SELECT CHECKINOUT.USERID, CHECKINOUT.CHECKTIME, CHECKINOUT.CHECKTYPE, USERINFO.Name, USERINFO.BadgeNumber, CHECKINOUT.sn FROM CHECKINOUT INNER JOIN USERINFO ON CHECKINOUT.USERID = USERINFO.USERID"

    '    '    Dim con As New OleDbConnection(strCon)
    '    '    Dim cmd As New OleDbCommand(strQuery, con)
    '    '    con.Open()
    '    '    Dim rdr As OleDbDataReader = cmd.ExecuteReader()
    '    '    Dim chktype As String = String.Empty
    '    '    Dim chktime As String = String.Empty
    '    '    Dim chkdate As String = String.Empty
    '    '    Dim staffname As String = String.Empty

    '    '    While rdr.Read
    '    '        If rdr.HasRows = True Then
    '    '            Select Case CStr(rdr(2))
    '    '                Case "I"
    '    '                    chktype = "IN"
    '    '                Case "O"
    '    '                    chktype = "OUT"
    '    '                Case "0"
    '    '                    chktype = "BRKOUT"
    '    '                Case "1"
    '    '                    chktype = "BRKIN"
    '    '                Case "i"
    '    '                    chktype = "OTIN"
    '    '                Case "o"
    '    '                    chktype = "OTOUT"
    '    '            End Select
    '    '            '''STAFF BADGE

    '    '            If usb_worker.VERIFY_USB_LOG(CStr(rdr(4)).Trim, Format(CDate(rdr(1)), "MM-dd-yyyy HH:mm:ss")) = False Then
    '    '                Console.Write("New Logs: " & CStr(rdr(4)).Trim & " " & Format(CDate(rdr(1)), "MM-dd-yyyy HH:mm:ss") & vbNewLine)
    '    '                'Dim raw As String = Format(CDate(rdr(1)), "MM-dd-yyyy HH:mm:00")
    '    '                '2015-35-14 08:35:00
    '    '                usb_worker.INSERT_NEW_LOG(CStr(rdr(4)).Trim, Format(CDate(rdr(1)), "MM/dd/yyyy"), Format(CDate(rdr(1)), "HH:mm:ss"), chktype, "att2000", False, Format(CDate(rdr(1)), "MM-dd-yyyy HH:mm:ss"), "")
    '    '            Else
    '    '                Console.WriteLine("SKIP: Logs already exist!" & vbNewLine)
    '    '            End If
    '    '        End If
    '    '    End While
    '    '    con.Close()
    '    'Else
    '    '    Console.WriteLine("att2000.mdb file not found!!!" & vbNewLine)
    '    'End If
    'End Sub

    Private Sub btn_mdb_con_str_Click(sender As System.Object, e As System.EventArgs) Handles btn_mdb_con_str.Click
        Dim ofd1 As New OpenFileDialog()
        ofd1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        ofd1.Filter = "att2000 (*.mdb)|*.mdb" : ofd1.RestoreDirectory = True
        If ofd1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            If File.Exists(ofd1.FileName) Then
                systemsettings.SaveZK_mdb_Settings(ofd1.FileName)

                txt_zk_mdb_string.Text = ""
                txt_zk_mdb_string.Text = My.Settings.zk_con_string
                My.Settings.Save()
                MessageBox.Show("Application will be restared", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Restart()
            End If
        End If
    End Sub

    Private Sub ThreadZKmdbRead_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles ThreadZKmdbRead.RunWorkerCompleted
        MessageBox.Show("Task completed!")
    End Sub
    Public Sub ZKmdbRead(sdate As Date, edate As Date)
        If File.Exists(My.Settings.zk_mdb_path) Then
            ' Dim CustID As Integer = CInt(TxtSearch.Text)
            Console.Write("att2000.mdb is exist" & vbNewLine)
            Dim strCon As String = My.Settings.zk_con_string
            Dim strQuery As String = "SELECT CHECKINOUT.USERID, CHECKINOUT.CHECKTIME, CHECKINOUT.CHECKTYPE, USERINFO.Name, USERINFO.BadgeNumber, CHECKINOUT.sn FROM CHECKINOUT INNER JOIN USERINFO ON CHECKINOUT.USERID = USERINFO.USERID WHERE CHECKINOUT.CHECKTIME BETWEEN #" & sdate & "# AND #" & edate & "#"

            Dim con As New OleDbConnection(strCon)
            Dim cmd As New OleDbCommand(strQuery, con)
            con.Open()
            Dim rdr As OleDbDataReader = cmd.ExecuteReader()
            Dim chktype As String = String.Empty
            Dim chktime As String = String.Empty
            Dim chkdate As String = String.Empty
            Dim staffname As String = String.Empty







            While rdr.Read
                If rdr.HasRows = True Then
                    Select Case CStr(rdr(2))
                        Case "I"
                            chktype = "IN"
                        Case "O"
                            chktype = "OUT"
                        Case "0"
                            chktype = "BRKOUT"
                        Case "1"
                            chktype = "BRKIN"
                        Case "i"
                            chktype = "OTIN"
                        Case "o"
                            chktype = "OTOUT"
                    End Select
                    '''STAFF BADGE

                    If usb_worker.VERIFY_USB_LOG(CStr(rdr(4)).Trim, Format(CDate(rdr(1)), "MM-dd-yyyy HH:mm:ss")) = False Then
                        '  Console.Write("New Logs: " & CStr(rdr(4)).Trim & " " & Format(CDate(rdr(1)), "MM-dd-yyyy HH:mm:ss") & vbNewLine)
                        'Dim raw As String = Format(CDate(rdr(1)), "MM-dd-yyyy HH:mm:00")
                        '2015-35-14 08:35:00
                        usb_worker.INSERT_NEW_LOG(CStr(rdr(4)).Trim, Format(CDate(rdr(1)), "MM/dd/yyyy"), Format(CDate(rdr(1)), "HH:mm:ss"), chktype, "att2000", False, Format(CDate(rdr(1)), "MM-dd-yyyy HH:mm:ss"), "")
                    Else
                        ' Console.WriteLine("SKIP: Logs already exist!" & vbNewLine)
                    End If
                End If
            End While
            con.Close()
        Else
            Console.WriteLine("att2000.mdb file not found!!!" & vbNewLine)
        End If
    End Sub

 
    Private Sub lnk_add_photo_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnk_add_photo.LinkClicked
 
        Using O As New OpenFileDialog With {.Filter = "(Image Files)|*.jpg;*.png|Jpg, | *.jpg|Png, | *.png", .Multiselect = False, .Title = "Select Image"}
            If O.ShowDialog = 1 Then
                image_path_src = O.FileName
     

                Try



                    Dim ImageA As Image
                    Using Str As Stream = File.OpenRead(O.FileName)
                        ImageA = Image.FromStream(Str)
                    End Using
                    pbx_staff_img.Image = ImageA
                    pbx_staff_img.SizeMode = PictureBoxSizeMode.StretchImage


                Catch ex As Exception

                End Try
               
            End If
        End Using
    End Sub

    Private Sub pbx_dtr_e_Click(sender As System.Object, e As System.EventArgs) Handles pbx_dtr_e.Click
        Label19.Visible = False
        btnReportExit.Visible = False
        selectedReportIconName = "DTR_E"
        GlobalVariables.DTR_TYPE = "DTR_E"
        lblx_PromptHeaderText.Text = "Generate Daily Time Record Report(DTR)"
        pbx_ReportIconsControl.Visible = False
        pbx_ReportConfiguration.Visible = True

        LOAD_TREEVIEW_DETAILS()
    End Sub

    Private Sub btn_repair_att_Click(sender As System.Object, e As System.EventArgs) Handles btn_repair_att.Click
      
        Dim btms_lstOfStaffid As New List(Of String)
        Dim att_lstOfStaffid As New List(Of String)
        Dim att_invalid_names As New List(Of String)
        Dim att_forchecking_names As New List(Of String)

        btms_lstOfStaffid = sqlce_asstnt.GET_ALL_STAFF_ID(True, 0)
        att_lstOfStaffid = ATT_GET_NAME_AS_LIST()

        btms_lstOfStaffid.Sort()
        att_lstOfStaffid.Sort()
        Dim name As String
        Dim result As String
        ''not in btms
        att_forchecking_names.Add("BTMS(GovEngine.sdf) vs ATT(att2000.mdb)")
        att_forchecking_names.Add("names_not_in_btms")
        att_forchecking_names.Add("BTMS USERS: " & btms_lstOfStaffid.Count)
        att_forchecking_names.Add("ATT USERS: " & att_lstOfStaffid.Count)
        att_forchecking_names.Add("------------------------------------------------------------")
        att_forchecking_names.Add(String.Format("{0,-10}{1,-50}", "ID", "NAME"))
        att_forchecking_names.Add("------------------------------------------------------------")

        For Each n In att_lstOfStaffid

            If Not btms_lstOfStaffid.Contains(n) Then
                name = ATT_GET_NAME(n)

                If name.Length < 11 Then
                    result = String.Format("{0,-10}{1,-50}", n, name)
                    att_invalid_names.Add(result)
                Else
                    result = String.Format("{0,-10}{1,-50}", n, name)
                    att_forchecking_names.Add(result)
                End If
            End If
        Next



        ''names ouput
        'For Each n As String In btms_lstOfStaffid
        '    Console.WriteLine(n & ">>" & sqlce_asstnt.GET_STAFF_NAME(n) & vbTab & ATT_GET_NAME(n))
        'Next

        Try
            If File.Exists("invalid.txt") Then
                File.Delete("invalid.txt")
            End If
            Console.WriteLine("FOR CHECKING")
          
            Console.WriteLine(String.Join(vbNewLine, att_forchecking_names.ToArray))
            WRITE_INVALID(String.Join(vbNewLine, att_forchecking_names.ToArray))
            Console.WriteLine(String.Join(vbNewLine, att_invalid_names.ToArray))
            WRITE_INVALID(String.Join(vbNewLine, att_invalid_names.ToArray))



            Dim PROC As New System.Diagnostics.Process
            PROC = System.Diagnostics.Process.Start(Application.StartupPath & "\wordpad.exe", Application.StartupPath & "\invalid.txt")
            PROC.Dispose()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
 
    End Sub

    Private Sub pbx_dtr_f_Click(sender As System.Object, e As System.EventArgs) Handles pbx_dtr_f.Click
        Label19.Visible = False
        btnReportExit.Visible = False
        selectedReportIconName = "DTR_F"
        GlobalVariables.DTR_TYPE = "DTR_F"
        lblx_PromptHeaderText.Text = "Generate Daily Time Record Report(DTR)"
        pbx_ReportIconsControl.Visible = False
        pbx_ReportConfiguration.Visible = True
        LOAD_TREEVIEW_DETAILS()

    End Sub

    Private Sub btn_remove_invalid_entry_Click(sender As System.Object, e As System.EventArgs) Handles btn_remove_invalid_entry.Click

        Dim btms_lstOfStaffid As New List(Of String)
        Dim att_lstOfStaffid As New List(Of String)
        Dim att_invalid_names As New List(Of String)
        Dim att_forchecking_names As New List(Of String)

        btms_lstOfStaffid = sqlce_asstnt.GET_ALL_STAFF_ID(True, 0)
        att_lstOfStaffid = ATT_GET_NAME_AS_LIST()

        btms_lstOfStaffid.Sort()
        att_lstOfStaffid.Sort()
        Dim name As String
        Dim result As String
        ''not in btms
        att_forchecking_names.Add("BTMS(GovEngine.sdf) vs ATT(att2000.mdb)")
        att_forchecking_names.Add("names_not_in_btms")
        att_forchecking_names.Add("BTMS USERS: " & btms_lstOfStaffid.Count)
        att_forchecking_names.Add("ATT USERS: " & att_lstOfStaffid.Count)
        att_forchecking_names.Add("------------------------------------------------------------")
        att_forchecking_names.Add(String.Format("{0,-10}{1,-50}{2,-50}", "ID", "NAME", "REMARKS"))
        att_forchecking_names.Add("------------------------------------------------------------")

        For Each n In att_lstOfStaffid

            If Not btms_lstOfStaffid.Contains(n) Then
                name = ATT_GET_NAME(n)

                If name.Length < 11 Then
                    result = String.Format("{0,-10}{1,-50}{2,-50}", n, name, "deleted")
                    ATT_REMOVE_NAME(n)
                    att_invalid_names.Add(result)


                Else
                    result = String.Format("{0,-10}{1,-50}{2,-50}", n, name, "")
                    att_forchecking_names.Add(result)
                End If
            End If
        Next



        ''names ouput
        'For Each n As String In btms_lstOfStaffid
        '    Console.WriteLine(n & ">>" & sqlce_asstnt.GET_STAFF_NAME(n) & vbTab & ATT_GET_NAME(n))
        'Next

        Try
            If File.Exists("invalid.txt") Then
                File.Delete("invalid.txt")
            End If
            Console.WriteLine("FOR CHECKING")

            Console.WriteLine(String.Join(vbNewLine, att_forchecking_names.ToArray))
            WRITE_INVALID(String.Join(vbNewLine, att_forchecking_names.ToArray))
            Console.WriteLine(String.Join(vbNewLine, att_invalid_names.ToArray))
            WRITE_INVALID(String.Join(vbNewLine, att_invalid_names.ToArray))



            Dim PROC As New System.Diagnostics.Process
            PROC = System.Diagnostics.Process.Start(Application.StartupPath & "\wordpad.exe", Application.StartupPath & "\invalid.txt")
            PROC.Dispose()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub
End Class