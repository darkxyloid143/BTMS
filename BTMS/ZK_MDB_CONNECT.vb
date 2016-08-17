Imports System.Data.OleDb
Imports System.IO
Module ZK_MDB_CONNECT

    'Dim ZKmdb_con As New OleDbConnection("Provider=microsoft.Jet.oledb.4.0DataSource=" & My.Settings.zk_con_string & ";")
    ''SELECT CHECKINOUT.USERID, CHECKINOUT.CHECKTIME, CHECKINOUT.CHECKTYPE, USERINFO.Name, USERINFO.BadgeNumber, CHECKINOUT.sn
    'FROM CHECKINOUT INNER JOIN USERINFO ON CHECKINOUT.USERID = USERINFO.USERID
    Dim strCon As String = My.Settings.zk_con_string
    Dim con As New OleDbConnection(strCon)
    Dim rdr As OleDbDataReader
    Dim cmd As OleDbCommand
    Dim strQuery As String

    Public Sub Main()
        ' Dim CustID As Integer = CInt(TxtSearch.Text)
        Console.Write("att2000.mdb is exist")
        Dim strCon As String = My.Settings.zk_con_string
        Dim strQuery As String = "SELECT CHECKINOUT.USERID, CHECKINOUT.CHECKTIME, CHECKINOUT.CHECKTYPE, USERINFO.Name, USERINFO.BadgeNumber, CHECKINOUT.sn FROM CHECKINOUT INNER JOIN USERINFO ON CHECKINOUT.USERID = USERINFO.USERID"

        Dim con As New OleDbConnection(strCon)
        Dim cmd As New OleDbCommand(strQuery, con)
        con.Open()
        Dim rdr As OleDbDataReader = cmd.ExecuteReader()
        While rdr.Read

            If rdr.HasRows = True Then
                '                txtFirstName.Text = CStr(rdr("FirstName"))
                '                txtLastName.Text = CStr(rdr("LastName"))
                'txtAddress.Text = CStr(rdr(Address("Address"))
                '                txtAppointment.Text = CStr(rdr("Appointment"))
                'Console.Write(rdr(3))
                MessageBox.Show(rdr(3))
            Else
                MsgBox("No Records Found")
            End If
        End While
        con.Close()
    End Sub


    Function ATT_GET_NAME(id As String) As String
        ' Console.Write("att2000.mdb is exist")
     
        strQuery = "SELECT NAME FROM USERINFO WHERE BADGENUMBER = '" & id & "'"

        cmd = New OleDbCommand(strQuery, con)

        If Not con.State = ConnectionState.Open Then
            con.Open()
        End If

        rdr = cmd.ExecuteReader()
        While rdr.Read
            If rdr.HasRows = True Then
                Return rdr(0)
            Else
                'MsgBox("No Records Found")
            End If
        End While
        '  con.Close()
        'cmd.Dispose()
        'con.Dispose()
        con.Close()
        GC.Collect()
        rdr.Close()
        Return ""
    End Function


    Public Sub ATT_REMOVE_NAME(id As String)
        ' Console.Write("att2000.mdb is exist")

        strQuery = "DELETE FROM USERINFO WHERE BADGENUMBER = '" & id & "'"

        cmd = New OleDbCommand(strQuery, con)

        If Not con.State = ConnectionState.Open Then
            con.Open()
        End If

        cmd.ExecuteNonQuery()
        con.Close()
        GC.Collect()
        rdr.Close()
    End Sub


    Function ATT_GET_NAME_AS_LIST() As List(Of String)
        ' Console.Write("att2000.mdb is exist")
        Dim lstStaffid As New List(Of String)
        Dim strQuery As String = "SELECT BADGENUMBER FROM USERINFO"

        Dim cmd As New OleDbCommand(strQuery, con)

        If Not con.State = ConnectionState.Open Then
            con.Open()
        End If

        Dim rdr As OleDbDataReader = cmd.ExecuteReader()
        While rdr.Read
            If rdr.HasRows = True Then
                lstStaffid.Add(rdr(0))
            Else
                'MsgBox("No Records Found")
            End If
        End While
        '  con.Close()
        'cmd.Dispose()
        'con.Dispose()
        'GC.Collect()
        Return lstStaffid
    End Function

End Module


