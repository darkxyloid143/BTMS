Imports System.Data.SqlServerCe
Imports System.Data
Imports System.IO

Public Class DeviceDetails
    Private SQL As New SQLControl
    Private Sub DeviceDetails_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ' SQL.ExecQuery("SELECT DEVICENAME AS [DEVICE NAME], DEVICEIP AS [IP ADDRESS], COMPORT AS [COMMUNICATION PORT], COMKEY AS [COMMUNICATION KEY] FROM DEVICES")
        SQL.ExecQuery("SELECT * FROM DEVICES")
        btn_DeviceUpdate.Enabled = False
        LoadGrid()
        ARRANGE_DATAGRID_DEVICES()
    End Sub

    Private Sub LoadGrid()
        dtgv_bisbiodevices.DataSource = SQL.SQLDS.Tables(0)
        dtgv_bisbiodevices.Rows(0).Selected = True
        SQL.SQLDA.UpdateCommand = New SqlServerCe.SqlCeCommandBuilder(SQL.SQLDA).GetUpdateCommand

    End Sub
    Public Sub ARRANGE_DATAGRID_DEVICES()

        For I1 = 0 To dtgv_bisbiodevices.Columns.Count - 1

            If dtgv_bisbiodevices.Columns(I1).Name = "DEVICENAME" Then
                Dim column As DataGridViewColumn = dtgv_bisbiodevices.Columns(I1)
                column.Width = 230
            ElseIf dtgv_bisbiodevices.Columns(I1).Name = "DEVID" Then
                Dim column As DataGridViewColumn = dtgv_bisbiodevices.Columns(I1)
                dtgv_bisbiodevices.Columns(I1).Visible = False
                column.Width = 100
            ElseIf dtgv_bisbiodevices.Columns(I1).Name = "FIRMWAREVERSION" Then
                Dim column As DataGridViewColumn = dtgv_bisbiodevices.Columns(I1)
                dtgv_bisbiodevices.Columns(I1).Visible = False
                column.Width = 100
            ElseIf dtgv_bisbiodevices.Columns(I1).Name = "DEVICEMODEL" Then
                Dim column As DataGridViewColumn = dtgv_bisbiodevices.Columns(I1)
                dtgv_bisbiodevices.Columns(I1).Visible = False
                column.Width = 100
            ElseIf dtgv_bisbiodevices.Columns(I1).Name = "" Then
                Dim column As DataGridViewColumn = dtgv_bisbiodevices.Columns(I1)
                column.Width = 100
            End If
        Next
    End Sub

    Private Sub btn_DeviceUpdate_Click(sender As System.Object, e As System.EventArgs) Handles btn_DeviceUpdate.Click
        Try
            SQL.SQLDA.Update(SQL.SQLDS)
            MessageBox.Show("Update Success!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Console.WriteLine(ex.Message)
        End Try
    End Sub
    Private Sub dtgv_bisbiodevices_CellValueChanged(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dtgv_bisbiodevices.CellValueChanged
        btn_DeviceUpdate.Enabled = True
    End Sub

    Private Sub dtgv_bisbiodevices_RowsRemoved(sender As Object, e As System.Windows.Forms.DataGridViewRowsRemovedEventArgs) Handles dtgv_bisbiodevices.RowsRemoved
        btn_DeviceUpdate.Enabled = True
    End Sub

    Private Sub btnx_cancelEdit_Click(sender As System.Object, e As System.EventArgs) Handles btnx_cancelEdit.Click
        Form3.LOAD_ALL_DEVICES()
        Me.Close()
    End Sub
End Class