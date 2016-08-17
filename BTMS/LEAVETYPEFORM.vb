Imports System.Data.SqlServerCe
Imports System.Data
Imports System.IO

Public Class LEAVETYPEFORM
    Private SQL As New SQLControl



    Private Sub btnSaveSchema_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveSchema.Click
        Try
            SQL.SQLDA.Update(SQL.SQLDS)
            MessageBox.Show("Update Success!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Console.WriteLine(ex.Message)
        End Try
    End Sub


    Private Sub LEAVETYPEFORM_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        SQL.ExecQuery("SELECT ID, SYMBOL,NAME AS [DEFINITION],ALLOWEDTOTAL AS [ALLOWED TOTAL], PAIDLEAVE FROM LEAVECLASSTABLE")
        btnSaveSchema.Enabled = False
        LoadGrid()
        ARRANGE_DATAGRID_LEAVETYPE()

    End Sub
    Private Sub LoadGrid()
        dtgv_leaveSchema.DataSource = SQL.SQLDS.Tables(0)
        dtgv_leaveSchema.Rows(0).Selected = True
        SQL.SQLDA.UpdateCommand = New SqlServerCe.SqlCeCommandBuilder(SQL.SQLDA).GetUpdateCommand

    End Sub

  
    Private Sub dtgv_leaveSchema_CellValueChanged(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dtgv_leaveSchema.CellValueChanged
        btnSaveSchema.Enabled = True
    End Sub

    Private Sub dtgv_leaveSchema_RowsRemoved(sender As Object, e As System.Windows.Forms.DataGridViewRowsRemovedEventArgs) Handles dtgv_leaveSchema.RowsRemoved
        btnSaveSchema.Enabled = True
    End Sub
    Public Sub ARRANGE_DATAGRID_LEAVETYPE()

        For I1 = 0 To dtgv_leaveSchema.Columns.Count - 1

            If dtgv_leaveSchema.Columns(I1).Name = "ALLOWED TOTAL" Then
                Dim column As DataGridViewColumn = dtgv_leaveSchema.Columns(I1)
                column.Width = 50
            ElseIf dtgv_leaveSchema.Columns(I1).Name = "ID" Then
                Dim column As DataGridViewColumn = dtgv_leaveSchema.Columns(I1)
                dtgv_leaveSchema.Columns(0).Visible = False
                column.Width = 100
            End If
        Next
    End Sub

    Private Sub btnx_cancelEdit_Click(sender As System.Object, e As System.EventArgs) Handles btnx_cancelEdit.Click
        Form3.LOAD_ALL_OLM_LEAVETYPES()
        Me.Close()
    End Sub
End Class