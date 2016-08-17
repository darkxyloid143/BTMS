Imports Microsoft.Office.Interop.Excel
Module ReadExcel
    ''MASS UPLOAD MODULE FOR STAFF MASTER LIST
    Public EXCEPTIONMESSAGE As String = "Invalid file!"
    Dim sql_worker As New SQLCE_MANAGER
    Public Sub MAIN(excelpath As String)

        Dim excel As Application = New Application
        Dim w As Workbook = excel.Workbooks.Open(excelpath)

        ' Loop over all sheets.
        For i As Integer = 1 To w.Sheets.Count
            Dim sheet As Worksheet = w.Sheets(i)
            Dim r As Range = sheet.UsedRange
            Dim array(,) As Object = r.Value(XlRangeValueDataType.xlRangeValueDefault)
            Dim staffid As String = ""
            Dim staffname As String = ""
            Dim departmentname As String = ""
            Dim designation As String = ""
            Dim listOfDetails As New List(Of String)
            If array IsNot Nothing Then ' Scan the cells.
                Dim rows As Integer = array.GetUpperBound(0) ' Get bounds of the array.
                Dim cells As Integer = array.GetUpperBound(1)
                Dim RowListValues As New List(Of String)
                Dim validfile As Boolean = False
                For row As Integer = 1 To rows
                    For cell As Integer = 1 To cells


                        'If String.IsNullOrEmpty(array(row, cell)) Then
                        listOfDetails.Add(array(row, cell))

                        'End If





                        'MessageBox.Show(array(row, cell))
                        'If String.IsNullOrEmpty(array(row, cell)) Then
                        '    RowListValues.Add("NULL")
                        'End If
                        'Dim type As VariantType = VarType(array(row, cell))
                        'If IsDate(array(row, cell)) Then
                        '    RowListValues.Add(array(row, cell))
                        'End If
                        'If TypeOf (array(row, cell)) Is String Then
                        '    RowListValues.Add(array(row, cell))
                        'End If
                        'If TypeOf (array(row, cell)) Is Double Then
                        '    RowListValues.Add(DateTime.FromOADate(array(row, cell).ToString))
                        'End If
                    Next

                    ''TODO: INSERT DATE HERE
                    ''DETERMIN IF IT VALID FILE FROM
                    If listOfDetails.Contains("ID") And listOfDetails.Contains("NAME") And listOfDetails.Contains("DEPARTMENT") And listOfDetails.Contains("DESIGNATION") And listOfDetails.Count = 4 Or listOfDetails.Contains("Eployee Master List") Then
                        validfile = True
                        Console.WriteLine("VALID FILE")
                        listOfDetails.Clear()
                        Continue For
                    End If

                    If validfile = True Then
                        EXCEPTIONMESSAGE = "Import complete!"
                        Console.WriteLine(listOfDetails(0) & " " & listOfDetails(1) & " " & listOfDetails(2) & " " & listOfDetails(3))
                        staffid = listOfDetails(0)
                        staffname = listOfDetails(1)
                        departmentname = listOfDetails(2)
                        designation = listOfDetails(3)

                        If String.IsNullOrEmpty(staffid) And String.IsNullOrEmpty(staffname) And String.IsNullOrEmpty(departmentname) Then
                            Console.WriteLine("INCOMPLETE DETAILS SKIP AT ROW # " & row)
                            listOfDetails.Clear()
                            Continue For
                        Else
                            If listOfDetails.Contains("NAME") Or listOfDetails.Contains("DEPARTMENT") Or listOfDetails.Contains("ID") Then
                                listOfDetails.Clear()
                                Continue For
                            End If
                            'IIf(IsNothing(staffid), , designation = designation.Trim.ToUpper)

                            If IsNothing(staffid) Or IsNothing(staffname) Then
                                Continue For
                            End If

                            IIf(IsNothing(departmentname), departmentname = "", departmentname = departmentname.Trim.ToUpper)
                            'IIf(IsNothing(designation), Console.WriteLine("designation is nothing"), Nothing)

                            If Not String.IsNullOrEmpty(designation) Then
                                designation = designation.ToUpper.Trim
                                Console.WriteLine(designation)
                            Else
                                Console.WriteLine("designation is nothing")
                                designation = ""
                            End If

                            If IsNothing(departmentname) Then
                                Console.WriteLine("department name is nothing")
                                departmentname = ""
                            Else
                                departmentname = departmentname.ToUpper.Trim
                            End If


                            '  Console.WriteLine(designation)
                            sql_worker.INSERT_TO_STAFF_MASTERFILE(staffid.Trim, staffname.Trim.ToUpper, departmentname, designation)
                            listOfDetails.Clear()
                        End If
                    End If
                    Console.WriteLine("NEXT ROW")
                    listOfDetails.Clear()
                Next

            End If
        Next
        w.Close()
    End Sub
End Module
