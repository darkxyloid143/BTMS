Public Class FormMessage

    Dim drag As Boolean
    Dim mousex As Integer
    Dim mousey As Integer

    Public btnOkenabled As Boolean = False
    Public btnCancelenabled As Boolean = False
    Public btnYesenabled As Boolean = False
    Public btnNoenabled As Boolean = False
    Public box_message As String = ""
    Public box_title As String = ""


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


    Private Sub btnYes_Click(sender As System.Object, e As System.EventArgs) Handles btnYes.Click
        If btnYes.Text = "OK" Then
            Me.Close()
        End If
    End Sub


    Private Sub FormMessage_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load





    End Sub
End Class