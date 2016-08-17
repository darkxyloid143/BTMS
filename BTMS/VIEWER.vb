Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports System.Data.SqlServerCe
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Globalization
Imports System.Reflection






Public Class VIEWER
    Dim drag As Boolean
    Dim mousex As Integer
    Dim mousey As Integer
    ''DEFAULT CONFIGURATION FOR REPORT
    Public Shared VIEWREPORTTYPE As String = String.Empty

    Public ce_query_param As String = String.Empty
    ''DTR SIGNATORY
    Public Shared Preparedby_signatory_Name As String = ""
    Public Shared Preparedby_signatory_Position As String = My.Application.Info.CompanyName.ToString()

    ''SRA SIGNATORY WITH DTR COMBINATION
    Public Shared CertifiedCorrect_signatory_Name As String = ""
    Public Shared CertifiedCorrect_signatory_Position As String = My.Application.Info.CompanyName.ToString()

    Public Shared Notedby_signatory_Name As String = ""
    Public Shared Notedby_signatory_Position As String = My.Application.Info.CompanyName.ToString()


    Public Shared SummaryDate As String = ""


    Private Sub VIEWER_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Dim x As Integer = Me.Size.Width + 100
        Dim y As Integer = Me.Size.Height + 100
        Me.Size = New Size(x, y)
        Me.WindowState = FormWindowState.Maximized

        ''DETERMIN WHAT REPORT WELL BE SHOWN
        Select Case VIEWREPORTTYPE.ToString
            Case "DTR"
                LoadDTR("SELECT * FROM DTR")
            Case "DTR_B"
                LoadDTR_B("SELECT * FROM DTR")
            Case "DTR_C"
                LoadDTR_C("SELECT * FROM DTR")
            Case "DTR_D"
                LoadDTR_D("SELECT * FROM DTR")
            Case "DTR_F"
                LoadDTR_F("SELECT * FROM DTR")
            Case "SRA"
                LoadSRA("SELECT * FROM SRATABLE")
            Case "SROT"
                LoadSROT("SELECT * FROM SROT")
        End Select
    End Sub





#Region "SROT"
    Private Sub LoadSROT(QUERY As String)
        Dim ce_cnn As SqlCeConnection = New SqlCeConnection(My.Settings.LocalConnectionString)
        Dim ce_da As SqlCeDataAdapter
        'Dim ce_query As String = String.Empty
        Try
            Dim myDataSet As DataSet = New GovEngineDataSet
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            ce_da = New SqlCeDataAdapter(QUERY, ce_cnn)
            ce_da.Fill(myDataSet, "SROT")
            Dim cr As OVERTIMESUMMARY = New OVERTIMESUMMARY
            cr.SetDataSource(myDataSet.Tables("SROT"))
            CrystalReportViewer1.ReportSource = cr
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None
            CrystalReportViewer1.ShowExportButton = True
            CrystalReportViewer1.Show()

            Dim myTextObjectOnReport As CrystalDecisions.CrystalReports.Engine.TextObject
            myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text5"), CrystalDecisions.CrystalReports.Engine.TextObject)
            myTextObjectOnReport.Text = Preparedby_signatory_Name & vbCrLf & Preparedby_signatory_Position
            myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text11"), CrystalDecisions.CrystalReports.Engine.TextObject)
            myTextObjectOnReport.Text = Notedby_signatory_Name & vbCrLf & Notedby_signatory_Position
            myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text19"), CrystalDecisions.CrystalReports.Engine.TextObject)
            myTextObjectOnReport.Text = CertifiedCorrect_signatory_Name & vbCrLf & CertifiedCorrect_signatory_Position

            myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text13"), CrystalDecisions.CrystalReports.Engine.TextObject)
            myTextObjectOnReport.Text = SummaryDate

        Catch ex As Exception
            MessageBox.Show("[LoadSROT()] ", ex.Message)
        End Try
    End Sub
#End Region





#Region "SRA"
    Private Sub LoadSRA(QUERY As String)
        Dim ce_cnn As SqlCeConnection = New SqlCeConnection(My.Settings.LocalConnectionString)
        Dim ce_da As SqlCeDataAdapter
        'Dim ce_query As String = String.Empty
        Try
            Dim myDataSet As DataSet = New GovEngineDataSet
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            ce_da = New SqlCeDataAdapter(QUERY, ce_cnn)
            ce_da.Fill(myDataSet, "SRATABLE")
            Dim cr As SRAMONTHLY = New SRAMONTHLY
            cr.SetDataSource(myDataSet.Tables("SRATABLE"))
            CrystalReportViewer1.ReportSource = cr
            CrystalReportViewer1.ShowExportButton = True
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None
            CrystalReportViewer1.Show()

            Dim myTextObjectOnReport As CrystalDecisions.CrystalReports.Engine.TextObject
            myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text5"), CrystalDecisions.CrystalReports.Engine.TextObject)
            myTextObjectOnReport.Text = Preparedby_signatory_Name & vbCrLf & Preparedby_signatory_Position
            myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text11"), CrystalDecisions.CrystalReports.Engine.TextObject)
            myTextObjectOnReport.Text = Notedby_signatory_Name & vbCrLf & Notedby_signatory_Position
            myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text19"), CrystalDecisions.CrystalReports.Engine.TextObject)
            myTextObjectOnReport.Text = CertifiedCorrect_signatory_Name & vbCrLf & CertifiedCorrect_signatory_Position

            myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text13"), CrystalDecisions.CrystalReports.Engine.TextObject)
            myTextObjectOnReport.Text = SummaryDate



            myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text9"), CrystalDecisions.CrystalReports.Engine.TextObject)
            myTextObjectOnReport.Text = My.Settings.CompanyName



        Catch ex As Exception
            MessageBox.Show("[LoadSRA()] ", ex.Message)
        End Try
    End Sub
#End Region


#Region "DTR_A"
    Private Sub LoadDTR(QUERY As String)

        Dim ce_cnn As SqlCeConnection = New SqlCeConnection(My.Settings.LocalConnectionString)
        Dim ce_da As SqlCeDataAdapter
        'Dim ce_query As String = String.Empty
        Try
            Dim myDataSet As DataSet = New GovEngineDataSet
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            ce_da = New SqlCeDataAdapter(QUERY, ce_cnn)
            ce_da.Fill(myDataSet, "DTR")
            Dim cr As DTR_REPORT = New DTR_REPORT
            cr.SetDataSource(myDataSet.Tables("DTR"))
            CrystalReportViewer1.ReportSource = cr
            CrystalReportViewer1.ShowExportButton = False
            'CrystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.GroupTree

            CrystalReportViewer1.Show()

            Dim myTextObjectOnReport As CrystalDecisions.CrystalReports.Engine.TextObject
            Console.WriteLine(Preparedby_signatory_Name)
            Console.WriteLine(Preparedby_signatory_Position)

            myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text15"), CrystalDecisions.CrystalReports.Engine.TextObject)
            myTextObjectOnReport.Text = Preparedby_signatory_Name & vbCrLf & Preparedby_signatory_Position

            myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text20"), CrystalDecisions.CrystalReports.Engine.TextObject)
            myTextObjectOnReport.Text = Preparedby_signatory_Name & vbCrLf & Preparedby_signatory_Position


            'DirectCast(cr.ReportDefinition.ReportObjects("Text11"), TextObject).Text = "Your Text"
            ''DirectCast(cr.ReportDefinition.ReportObjects("text_sig2"), TextObject).Text = "Your Second Text"



        Catch ex As Exception
            MessageBox.Show("[LoadDTR()] ", ex.Message)
        End Try

    End Sub
#End Region



#Region "DTR_B"
    Private Sub LoadDTR_B(QUERY As String)

        Dim ce_cnn As SqlCeConnection = New SqlCeConnection(My.Settings.LocalConnectionString)
        Dim ce_da As SqlCeDataAdapter
        'Dim ce_query As String = String.Empty
        Try
            Dim myDataSet As DataSet = New GovEngineDataSet
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            ce_da = New SqlCeDataAdapter(QUERY, ce_cnn)
            ce_da.Fill(myDataSet, "DTR")
            Dim cr As DTR_REPORT_B = New DTR_REPORT_B
            cr.SetDataSource(myDataSet.Tables("DTR"))
            CrystalReportViewer1.ReportSource = cr
            CrystalReportViewer1.ShowExportButton = False
            'CrystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.GroupTree

            CrystalReportViewer1.Show()

            Dim myTextObjectOnReport As CrystalDecisions.CrystalReports.Engine.TextObject
            Console.WriteLine(Preparedby_signatory_Name)
            Console.WriteLine(Preparedby_signatory_Position)

            myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text15"), CrystalDecisions.CrystalReports.Engine.TextObject)
            myTextObjectOnReport.Text = Preparedby_signatory_Name & vbCrLf & Preparedby_signatory_Position

            myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text20"), CrystalDecisions.CrystalReports.Engine.TextObject)
            myTextObjectOnReport.Text = Preparedby_signatory_Name & vbCrLf & Preparedby_signatory_Position

            myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text18"), CrystalDecisions.CrystalReports.Engine.TextObject)
            Dim s_d As String = Form3.dtpStartDateRpt.Value.ToString("MMMM d", CultureInfo.InvariantCulture)
            Dim e_d As String = Form3.dtpEndDateRpt.Value.ToString("d, yyyy", CultureInfo.InvariantCulture)
            myTextObjectOnReport.Text = s_d & "-" & e_d


            myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text19"), CrystalDecisions.CrystalReports.Engine.TextObject)
            myTextObjectOnReport.Text = s_d & "-" & e_d


            'myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text35"), CrystalDecisions.CrystalReports.Engine.TextObject)
            'myTextObjectOnReport.Text = My.Settings.CompanyName


            'myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text70"), CrystalDecisions.CrystalReports.Engine.TextObject)
            'myTextObjectOnReport.Text = My.Settings.CompanyName

            'DirectCast(cr.ReportDefinition.ReportObjects("Text11"), TextObject).Text = "Your Text"
            ''DirectCast(cr.ReportDefinition.ReportObjects("text_sig2"), TextObject).Text = "Your Second Text"



        Catch ex As Exception
            MessageBox.Show("[LoadDTR_B()] ", ex.Message)
        End Try

    End Sub
#End Region



#Region "DTR_C"

    Private Sub LoadDTR_C(QUERY As String)
        Dim ce_cnn As SqlCeConnection = New SqlCeConnection(My.Settings.LocalConnectionString)
        Dim ce_da As SqlCeDataAdapter
        'Dim ce_query As String = String.Empty
        Try
            Dim myDataSet As DataSet = New GovEngineDataSet
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            ce_da = New SqlCeDataAdapter(QUERY, ce_cnn)
            ce_da.Fill(myDataSet, "DTR")
            Dim cr As DTR_REPORT_C = New DTR_REPORT_C
            cr.SetDataSource(myDataSet.Tables("DTR"))
            CrystalReportViewer1.ReportSource = cr
            CrystalReportViewer1.ShowExportButton = False
            'CrystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.GroupTree

            CrystalReportViewer1.Show()

            Dim myTextObjectOnReport As CrystalDecisions.CrystalReports.Engine.TextObject
            Console.WriteLine(Preparedby_signatory_Name)
            Console.WriteLine(Preparedby_signatory_Position)

            'myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text15"), CrystalDecisions.CrystalReports.Engine.TextObject)
            'myTextObjectOnReport.Text = Preparedby_signatory_Name & vbCrLf & Preparedby_signatory_Position

            myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text20"), CrystalDecisions.CrystalReports.Engine.TextObject)
            myTextObjectOnReport.Text = Preparedby_signatory_Name & vbCrLf & Preparedby_signatory_Position



            myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text174"), CrystalDecisions.CrystalReports.Engine.TextObject)
            myTextObjectOnReport.Text = My.Settings.CompanyName

            'myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text18"), CrystalDecisions.CrystalReports.Engine.TextObject)
            'Dim s_d As String = Form3.dtpStartDateRpt.Value.ToString("MMMM d", CultureInfo.InvariantCulture)
            'Dim e_d As String = Form3.dtpEndDateRpt.Value.ToString("d, yyyy", CultureInfo.InvariantCulture)
            'myTextObjectOnReport.Text = s_d & "-" & e_d


            'myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text19"), CrystalDecisions.CrystalReports.Engine.TextObject)
            'myTextObjectOnReport.Text = s_d & "-" & e_d


            'myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text35"), CrystalDecisions.CrystalReports.Engine.TextObject)
            'myTextObjectOnReport.Text = My.Settings.CompanyName


            'myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text70"), CrystalDecisions.CrystalReports.Engine.TextObject)
            'myTextObjectOnReport.Text = My.Settings.CompanyName

            'DirectCast(cr.ReportDefinition.ReportObjects("Text11"), TextObject).Text = "Your Text"
            ''DirectCast(cr.ReportDefinition.ReportObjects("text_sig2"), TextObject).Text = "Your Second Text"



        Catch ex As Exception
            MessageBox.Show("[LoadDTR_C()] ", ex.Message)
        End Try

    End Sub


#End Region





#Region "DTR_D"

    Private Sub LoadDTR_D(QUERY As String)
        Dim ce_cnn As SqlCeConnection = New SqlCeConnection(My.Settings.LocalConnectionString)
        Dim ce_da As SqlCeDataAdapter
        'Dim ce_query As String = String.Empty
        Try
            Dim myDataSet As DataSet = New GovEngineDataSet
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            ce_da = New SqlCeDataAdapter(QUERY, ce_cnn)
            ce_da.Fill(myDataSet, "DTR")
            Dim cr As DTR_REPORT_D = New DTR_REPORT_D
            cr.SetDataSource(myDataSet.Tables("DTR"))
            CrystalReportViewer1.ReportSource = cr
            CrystalReportViewer1.ShowExportButton = False
            'CrystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.GroupTree

            CrystalReportViewer1.Show()

            Dim myTextObjectOnReport As CrystalDecisions.CrystalReports.Engine.TextObject
            Console.WriteLine(Preparedby_signatory_Name)
            Console.WriteLine(Preparedby_signatory_Position)

            'myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text15"), CrystalDecisions.CrystalReports.Engine.TextObject)
            'myTextObjectOnReport.Text = Preparedby_signatory_Name & vbCrLf & Preparedby_signatory_Position

            myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text22"), CrystalDecisions.CrystalReports.Engine.TextObject)
            myTextObjectOnReport.Text = My.Settings.CompanyName.ToString.ToUpper
            myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text23"), CrystalDecisions.CrystalReports.Engine.TextObject)
            myTextObjectOnReport.Text = My.Settings.CompanyName.ToString.ToUpper




            myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text30"), CrystalDecisions.CrystalReports.Engine.TextObject)
            Dim s_d As String = Form3.dtpStartDateRpt.Value.ToString("MMMM  d", CultureInfo.InvariantCulture)
            Dim e_d As String = Form3.dtpEndDateRpt.Value.ToString("d, yyyy", CultureInfo.InvariantCulture)
            myTextObjectOnReport.Text = s_d & "-" & e_d
            myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text31"), CrystalDecisions.CrystalReports.Engine.TextObject)
            myTextObjectOnReport.Text = s_d & "-" & e_d


            myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text20"), CrystalDecisions.CrystalReports.Engine.TextObject)
            myTextObjectOnReport.Text = Preparedby_signatory_Name & vbCrLf & Preparedby_signatory_Position
            myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text15"), CrystalDecisions.CrystalReports.Engine.TextObject)
            myTextObjectOnReport.Text = Preparedby_signatory_Name & vbCrLf & Preparedby_signatory_Position

            'myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text18"), CrystalDecisions.CrystalReports.Engine.TextObject)
            'Dim s_d As String = Form3.dtpStartDateRpt.Value.ToString("MMMM d", CultureInfo.InvariantCulture)
            'Dim e_d As String = Form3.dtpEndDateRpt.Value.ToString("d, yyyy", CultureInfo.InvariantCulture)
            'myTextObjectOnReport.Text = s_d & "-" & e_d


            'myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text19"), CrystalDecisions.CrystalReports.Engine.TextObject)
            'myTextObjectOnReport.Text = s_d & "-" & e_d


            'myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text35"), CrystalDecisions.CrystalReports.Engine.TextObject)
            'myTextObjectOnReport.Text = My.Settings.CompanyName


            'myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text70"), CrystalDecisions.CrystalReports.Engine.TextObject)
            'myTextObjectOnReport.Text = My.Settings.CompanyName

            'DirectCast(cr.ReportDefinition.ReportObjects("Text11"), TextObject).Text = "Your Text"
            ''DirectCast(cr.ReportDefinition.ReportObjects("text_sig2"), TextObject).Text = "Your Second Text"



        Catch ex As Exception
            MessageBox.Show("[LoadDTR_D()] ", ex.Message)
        End Try

    End Sub
    Private Sub LoadDTR_F(QUERY As String)
        Dim ce_cnn As SqlCeConnection = New SqlCeConnection(My.Settings.LocalConnectionString)
        Dim ce_da As SqlCeDataAdapter
        'Dim ce_query As String = String.Empty
        Try
            Dim myDataSet As DataSet = New GovEngineDataSet
            If ce_cnn.State = ConnectionState.Closed Then ce_cnn.Open()
            ce_da = New SqlCeDataAdapter(QUERY, ce_cnn)
            ce_da.Fill(myDataSet, "DTR")
            Dim cr As DTR_REPORT_F = New DTR_REPORT_F
            cr.SetDataSource(myDataSet.Tables("DTR"))
            CrystalReportViewer1.ReportSource = cr
            CrystalReportViewer1.ShowExportButton = False
            'CrystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.GroupTree

            CrystalReportViewer1.Show()

            Dim myTextObjectOnReport As CrystalDecisions.CrystalReports.Engine.TextObject
            Console.WriteLine(Preparedby_signatory_Name)
            Console.WriteLine(Preparedby_signatory_Position)

            'myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text15"), CrystalDecisions.CrystalReports.Engine.TextObject)
            'myTextObjectOnReport.Text = Preparedby_signatory_Name & vbCrLf & Preparedby_signatory_Position

            myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text22"), CrystalDecisions.CrystalReports.Engine.TextObject)
            myTextObjectOnReport.Text = My.Settings.CompanyName.ToString.ToUpper
            myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text23"), CrystalDecisions.CrystalReports.Engine.TextObject)
            myTextObjectOnReport.Text = My.Settings.CompanyName.ToString.ToUpper




            myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text30"), CrystalDecisions.CrystalReports.Engine.TextObject)
            Dim s_d As String = Form3.dtpStartDateRpt.Value.ToString("MMMM  d", CultureInfo.InvariantCulture)
            Dim e_d As String = Form3.dtpEndDateRpt.Value.ToString("d, yyyy", CultureInfo.InvariantCulture)
            myTextObjectOnReport.Text = s_d & "-" & e_d
            myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text31"), CrystalDecisions.CrystalReports.Engine.TextObject)
            myTextObjectOnReport.Text = s_d & "-" & e_d


            myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text20"), CrystalDecisions.CrystalReports.Engine.TextObject)
            myTextObjectOnReport.Text = Preparedby_signatory_Name & vbCrLf & Preparedby_signatory_Position
            myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text15"), CrystalDecisions.CrystalReports.Engine.TextObject)
            myTextObjectOnReport.Text = Preparedby_signatory_Name & vbCrLf & Preparedby_signatory_Position

            'myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text18"), CrystalDecisions.CrystalReports.Engine.TextObject)
            'Dim s_d As String = Form3.dtpStartDateRpt.Value.ToString("MMMM d", CultureInfo.InvariantCulture)
            'Dim e_d As String = Form3.dtpEndDateRpt.Value.ToString("d, yyyy", CultureInfo.InvariantCulture)
            'myTextObjectOnReport.Text = s_d & "-" & e_d


            'myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text19"), CrystalDecisions.CrystalReports.Engine.TextObject)
            'myTextObjectOnReport.Text = s_d & "-" & e_d


            'myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text35"), CrystalDecisions.CrystalReports.Engine.TextObject)
            'myTextObjectOnReport.Text = My.Settings.CompanyName


            'myTextObjectOnReport = CType(cr.ReportDefinition.ReportObjects.Item("Text70"), CrystalDecisions.CrystalReports.Engine.TextObject)
            'myTextObjectOnReport.Text = My.Settings.CompanyName

            'DirectCast(cr.ReportDefinition.ReportObjects("Text11"), TextObject).Text = "Your Text"
            ''DirectCast(cr.ReportDefinition.ReportObjects("text_sig2"), TextObject).Text = "Your Second Text"



        Catch ex As Exception
            MessageBox.Show("[LoadDTR_F()] ", ex.Message)
        End Try

    End Sub



#End Region










    Private Sub lnkClose_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkClose.LinkClicked
        Me.Close()
    End Sub


    Private Sub VIEWER_MouseDoubleClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDoubleClick

        If Me.WindowState = FormWindowState.Maximized Then
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.WindowState = FormWindowState.Normal
            Exit Sub
        ElseIf Me.WindowState = FormWindowState.Normal Then
            Me.WindowState = FormWindowState.Maximized
        End If

    End Sub

    Private Sub VIEWER_MouseDown(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseDown
        drag = True                                             'Sets the variable drag to true.
        mousex = Windows.Forms.Cursor.Position.X - Me.Left          'Sets variable mousex
        mousey = Windows.Forms.Cursor.Position.Y - Me.Top               'Sets variable mousey
    End Sub

    Private Sub VIEWER_MouseUp(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseUp
        drag = False
    End Sub

    Private Sub VIEWER_MouseMove(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseMove
        If drag Then
            Me.Top = Windows.Forms.Cursor.Position.Y - mousey
            Me.Left = Windows.Forms.Cursor.Position.X - mousex
        End If
    End Sub


    Private Sub DTR_REPORT1_InitReport(sender As System.Object, e As System.EventArgs) Handles DTR_REPORT1.InitReport

    End Sub

    Private Sub SRAMONTHLY1_InitReport(sender As System.Object, e As System.EventArgs) Handles SRAMONTHLY1.InitReport

    End Sub
End Class