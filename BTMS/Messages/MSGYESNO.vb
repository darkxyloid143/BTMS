Imports BTMS.GLOBAL_MSGCOM

Public Class MSGYESNO
    Private Sub MSGYESNO_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        lblMsg.Text = YesNo_Message
    End Sub

    Private Sub msgbtn_yes_Click(sender As System.Object, e As System.EventArgs) Handles msgbtn_yes.Click
        YesNo_Value = "Yes"
        Me.Close()
    End Sub

    Private Sub msgbtn_no_Click(sender As System.Object, e As System.EventArgs) Handles msgbtn_no.Click
        YesNo_Value = "No"
        Me.Close()
    End Sub
End Class