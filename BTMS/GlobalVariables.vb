Module GlobalVariables
    Public DTR_TYPE As String = ""
    Public OFFICIAL_ARRIVAL = ""
    Public OFFICIAL_DEPARTURE = ""
    Public DILG_M_RANGE As New List(Of String)
    Public DILG_T_W_TH_F_RANGE As New List(Of String)
    Public Add_Edit_Delete_Department_and_Employees As Boolean = True
    Public image_path_src As String = ""
    Public stf_image_path As String = ""
    Public isModifiedLogs As Boolean = False
    '  Public isAtt2000Sync As Boolean = False
    '0 = Add/Edit/Delete Manage Travel Order
    '0 = Add/Edit/Delete BISBIO Devices
    '0 = Add/Edit/Delete Holiday List
    '0 = Add/Edit/Delete Leave Class
    '0 = Add_Edit_Delete_Department_and_Employees
    '0 = Add/Remove User Access Accounts
    '0 = Calcutating of Reports
    '0 = Download attendance logs
    '0 = Filing of Employee Leaving on Business/Asking for leave
    '0 = Filing of Employee Forgetting to Clock in/out
    '0 = Maintenance Options(BackupDB,Company Logo, DB Path,Clear DB Tables)
    '0 = Employee Schedules(Timetable/Shift Schedules/Assign)
End Module
