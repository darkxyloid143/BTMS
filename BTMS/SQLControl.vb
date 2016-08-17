Imports System.Data.SqlServerCe
Imports System.Data
Public Class SQLControl
    ''CONNECTION
    Private SQLConn As New SqlCeConnection(My.Settings.LocalConnectionString)
    Private SQLCmd As SqlCeCommand

    ''SQL DATA
    Public SQLDA As SqlCeDataAdapter
    Public SQLDS As DataSet

    ''SQL QUERY PARAMETERS
    Public Params As New List(Of SqlCeParameter)

    ''SQL QUERY STATISTICS
    Public RecordCount As Integer
    Public Exception As String

    Public Sub ExecQuery(Query As String)
        Try
            If SQLConn.State = ConnectionState.Closed Then SQLConn.Open()

            ''CREATE SQLCE COMMAND
            SQLCmd = New SqlCeCommand(Query, SQLConn)


            ''LOAD PARAMETERS INTO SQLCOMMAND
            Params.ForEach(Sub(x) SQLCmd.Parameters.Add(x))

            ''CLEAR PARAM LIST
            Params.Clear()

            ''EXECUTE COMMAND AND FILL DATASET
            SQLDS = New DataSet

            SQLDA = New SqlCeDataAdapter(SQLCmd)
            RecordCount = SQLDA.Fill(SQLDS)



        Catch ex As Exception
            Exception = ex.Message
        End Try
        If SQLConn.State = ConnectionState.Open Then SQLConn.Close()
    End Sub



End Class
