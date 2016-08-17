Imports System.Data.SqlServerCe
Imports System.Data
Imports System.IO

Public Class SHIFTTABLE
    Private SQL As New SQLControl
    Private Sub btnUpdateChanges_Click(sender As System.Object, e As System.EventArgs) Handles btnUpdateChanges.Click
        Try
            Sql.SQLDA.Update(Sql.SQLDS)

            MessageBox.Show("Update Success!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Form3.LOAD_ALL_SHIFTNAME()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Console.WriteLine(ex.Message)
        End Try
    End Sub
    Public Sub ARRANGE_DATAGRID_SHIFTABLE()

        For I1 = 0 To dtgv_shiftTable.Columns.Count - 1

            If dtgv_shiftTable.Columns(I1).Name = "SHIFT NAME" Then
                Dim column As DataGridViewColumn = dtgv_shiftTable.Columns(I1)
                column.Width = 250
            ElseIf dtgv_shiftTable.Columns(I1).Name = "SHIFTID" Then
                Dim column As DataGridViewColumn = dtgv_shiftTable.Columns(I1)
                dtgv_shiftTable.Columns(0).Visible = False
                column.Width = 100
            ElseIf dtgv_shiftTable.Columns(I1).Name = "INTERVAL OF LEAVING COUNT AS OT(minutes)" Then
                Dim column As DataGridViewColumn = dtgv_shiftTable.Columns(I1)
                column.Width = 100
            End If
        Next
    End Sub
    Private Sub LoadGrid()
        dtgv_shiftTable.DataSource = SQL.SQLDS.Tables(0)
        dtgv_shiftTable.Rows(0).Selected = True
        SQL.SQLDA.UpdateCommand = New SqlServerCe.SqlCeCommandBuilder(SQL.SQLDA).GetUpdateCommand


    End Sub
    Private Sub SHIFTTABLE_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        SQL.ExecQuery("SELECT SHIFTID, SHIFTNAME AS [SHIFT NAME], AUTOPILOT, INTERVALOFLEAVINGCOUNTASOT AS [INTERVAL OF LEAVING COUNT AS OT(minutes)], FLEXIBLE FROM SHIFTTABLE")
        btnUpdateChanges.Enabled = False
        LoadGrid()
        ARRANGE_DATAGRID_SHIFTABLE()
    End Sub



    Private Sub dtgv_shiftTable_CellValueChanged(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dtgv_shiftTable.CellValueChanged
        btnUpdateChanges.Enabled = True
    End Sub

    Private Sub dtgv_shiftTable_RowsRemoved(sender As Object, e As System.Windows.Forms.DataGridViewRowsRemovedEventArgs) Handles dtgv_shiftTable.RowsRemoved
        btnUpdateChanges.Enabled = True
    End Sub

    Private Sub btnx_cancelEdit_Click(sender As System.Object, e As System.EventArgs) Handles btnx_cancelEdit.Click
        Me.Close()
    End Sub
End Class