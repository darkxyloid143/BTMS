Imports System.Data.SqlServerCe
Imports System.Data
Imports System.IO

Public Class MTOFORM
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
    Private Sub LoadGrid()
        dtgv_mtoschema.DataSource = SQL.SQLDS.Tables(0)
        dtgv_mtoschema.Rows(0).Selected = True
        SQL.SQLDA.UpdateCommand = New SqlServerCe.SqlCeCommandBuilder(SQL.SQLDA).GetUpdateCommand

    End Sub

    Private Sub MTOFORM_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        SQL.ExecQuery("SELECT * FROM TRAVELORDERCLASSTABLE")
        btnSaveSchema.Enabled = False
        LoadGrid()
        ARRANGE_DATAGRID_MTO()
        btnSaveSchema.Enabled = False
    End Sub

    Public Sub ARRANGE_DATAGRID_MTO()

        For I1 = 0 To dtgv_mtoschema.Columns.Count - 1

            If dtgv_mtoschema.Columns(I1).Name = "SYMBOL" Then
                Dim column As DataGridViewColumn = dtgv_mtoschema.Columns(I1)
                column.Width = 80

            ElseIf dtgv_mtoschema.Columns(I1).Name = "DEFINITION" Then
                Dim column As DataGridViewColumn = dtgv_mtoschema.Columns(I1)
                column.Width = 200

            ElseIf dtgv_mtoschema.Columns(I1).Name = "PAID" Then
                Dim column As DataGridViewColumn = dtgv_mtoschema.Columns(I1)
                column.Width = 50

            ElseIf dtgv_mtoschema.Columns(I1).Name = "TROID" Then
                Dim column As DataGridViewColumn = dtgv_mtoschema.Columns(I1)
                dtgv_mtoschema.Columns(0).Visible = False
                column.Width = 100
            End If
        Next


    End Sub


    Private Sub dtgv_mtoschema_CellValueChanged(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dtgv_mtoschema.CellValueChanged
        btnSaveSchema.Enabled = True
    End Sub



    Private Sub dtgv_mtoschema_RowsRemoved(sender As Object, e As System.Windows.Forms.DataGridViewRowsRemovedEventArgs) Handles dtgv_mtoschema.RowsRemoved
        btnSaveSchema.Enabled = True
    End Sub

    Private Sub btnx_cancelEdit_Click(sender As System.Object, e As System.EventArgs) Handles btnx_cancelEdit.Click
        Form3.LOAD_MTO_KNOWN_PURPOSES_AND_LOCATION()
        Me.Close()
    End Sub

End Class