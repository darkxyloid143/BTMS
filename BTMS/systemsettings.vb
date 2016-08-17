
Module systemsettings
    Public Sub SaveDbSettings(ByVal dbpath As String)
        If MessageBox.Show("Saving this settings will affect the communication of this software. Would you like to proceed?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = Windows.Forms.DialogResult.Yes Then

            'Data Source=C:\Documents and Settings\Administrator\Desktop\GovEngine.sdf;Persist Security Info=False;

            My.Settings.LocalConnectionString = "Data Source=" & dbpath.Trim & ";Persist Security Info=False;"
            My.Settings.DatabasePath = dbpath.Trim
            My.Settings.Save()
        End If
    End Sub
    Public Sub SaveZK_mdb_Settings(ByVal dbpath As String)
        If MessageBox.Show("Saving this settings will affect the communication of this software. Would you like to proceed?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = Windows.Forms.DialogResult.Yes Then
            '  "Provider=Microsoft.Jet.OLEDB.4.0;Persist Security Info=False;Data Source=E:\Database.mdb;Jet OLEDB:Database Password=b10w"
            My.Settings.zk_con_string = "Provider=microsoft.Jet.oledb.4.0;Persist Security Info=False;Data Source='" & dbpath.Trim & "';"
            My.Settings.zk_mdb_path = dbpath.Trim
            ' My.Settings.zk_con_string = dbpath.Trim
            My.Settings.Save()
        End If
    End Sub
End Module
