Imports BTMS
Imports BTMS.MSGYESNO
Public Class RESIGNED_EMPFORM

    Private Sub btn_close_Click(sender As System.Object, e As System.EventArgs) Handles btn_close.Click
        Me.Close()
    End Sub

    Private Sub btn_rehired_Click(sender As System.Object, e As System.EventArgs) Handles btn_rehired.Click
        On Error Resume Next
        Dim sel_item As String = ""
        Dim staff_id As String = ""
        Dim staff_name As String = ""
        Dim dep_name As String = ""

        With lsv_stf.SelectedItems(0)
            staff_name = .SubItems(1).Text
            staff_id = .Text

            Console.WriteLine(staff_id.ToString)
            If String.IsNullOrEmpty(staff_id.Trim) Then
                Exit Sub
            End If


            BTMS.GLOBAL_MSGCOM.YesNo_Message = "You are about to tag " & staff_name & " to the database as rehired. Please confirm."
            BTMS.MSGYESNO.ShowDialog()
            If BTMS.GLOBAL_MSGCOM.YesNo_Value = "Yes" Then

                BTMS.Form3.REHIRED_STAFF_ID = staff_id
                BTMS.Form3.txtc_id.Text = staff_id
                BTMS.Form3.txtc_fullname.Text = staff_name
                BTMS.Form3.Panel40.Visible = True


                Dim ls_dp As New List(Of String)
                ls_dp = BTMS.Form3.sqlce_asstnt.GET_ALL_DEPARTMENT_NAME()
                ls_dp.Sort()
                BTMS.Form3.cmbc_department.Items.Clear()
                BTMS.Form3.cmbc_department.Items.AddRange(ls_dp.ToArray)
                BTMS.Form3.btnc_create.Text = "Rehired"

                Me.Close()
            End If
        End With
    End Sub
End Class