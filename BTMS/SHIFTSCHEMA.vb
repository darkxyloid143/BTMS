Imports System.Data.SqlServerCe
Imports System.Data
Imports System.IO


Public Class SHIFTSCHEMA
    Private SQL As New SQLControl
    Public shift_id As String = 0

    Private Sub SHIFTSCHEMA_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ' SQL.ExecQuery("SELECT TIMETABLEID, SHIFTID as [SHIFT ID], TIMETABLENAME as [TIME TABLE NAME], ONDUTYTIME as [ON-DUTY TIME], OFFDUTYTIME as [OFF-DUTYTIME], " & _
        '              "BEGININGIN as [BEGINING-IN], ENDINGIN as [ENDING-IN], BEGININGOUT as BEGINING-OUT, ENDINGOUT as [ENDING-OUT], WORKINGHOURS as [WORKING HOURS], REGULAR FROM SHIFTSCHEMATABLE")
        SQL.ExecQuery("SELECT * FROM SHIFTSCHEMATABLE WHERE SHIFTID = '" & shift_id & "'")

        LoadGrid()
        ARRANGE_DATAGRID_SHIFTSCHEMA()
        lblShiftID.Text = shift_id
        btnSaveSchema.Enabled = False
    End Sub
    Private Sub LoadGrid()
        dtgv_ShiftSchema.DataSource = SQL.SQLDS.Tables(0)
        'dtgv_ShiftSchema.Rows(0).Selected = True
        SQL.SQLDA.UpdateCommand = New SqlServerCe.SqlCeCommandBuilder(SQL.SQLDA).GetUpdateCommand


    End Sub
    Public Sub ARRANGE_DATAGRID_SHIFTSCHEMA()

        For I1 = 0 To dtgv_ShiftSchema.Columns.Count - 1

            If dtgv_ShiftSchema.Columns(I1).Name = "ALLOWED TOTAL" Then
                Dim column As DataGridViewColumn = dtgv_ShiftSchema.Columns(I1)
                column.Width = 50
            ElseIf dtgv_ShiftSchema.Columns(I1).Name = "TIMETABLEID" Then
                Dim column As DataGridViewColumn = dtgv_ShiftSchema.Columns(I1)
                dtgv_ShiftSchema.Columns(0).Visible = False
                column.Width = 100
            End If
        Next
    End Sub

    Private Sub btnSaveSchema_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveSchema.Click
        Try
            SQL.SQLDA.Update(SQL.SQLDS)
 
            MessageBox.Show("Update Success!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Console.WriteLine(ex.Message)
        End Try
    End Sub


    Private Sub dtgv_ShiftSchema_CellValueChanged(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dtgv_ShiftSchema.CellValueChanged
        btnSaveSchema.Enabled = True
    End Sub

 
    Private Sub dtgv_ShiftSchema_RowsRemoved(sender As Object, e As System.Windows.Forms.DataGridViewRowsRemovedEventArgs) Handles dtgv_ShiftSchema.RowsRemoved
        btnSaveSchema.Enabled = True
    End Sub



    Private Sub dtgv_ShiftSchema_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dtgv_ShiftSchema.CellContentClick

    End Sub

    Private Sub btnx_cancelEdit_Click(sender As System.Object, e As System.EventArgs) Handles btnx_cancelEdit.Click
        Me.Close()
    End Sub
End Class