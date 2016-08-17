Public Class MSGOK
    Public MSGOK_GLOBAL As String = ""


    Private Sub msgbtn_exit_Click(sender As System.Object, e As System.EventArgs) Handles msgbtn_exit.Click
        Me.Close()
    End Sub

    Private Sub MSGOK_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        lblMsg.Text = MSGOK_GLOBAL
    End Sub
End Class