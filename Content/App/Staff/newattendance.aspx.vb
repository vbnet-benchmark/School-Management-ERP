Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Admin_allstudents
    Inherits System.Web.UI.Page
    Dim pagebefore As String
    Dim studentId As Integer
    Dim termID As Integer
    Dim checkedSubjects As New ArrayList
    Dim uncheckedSubjects As New ArrayList

    Dim alert As New Literal
    Dim alertmsg As New Alerts
    Dim logify As New notify
    Dim check As New CheckUser
    Dim alertPLC As New PlaceHolder
    Dim par As New parentcheck
    Private Sub Show_Alert(type As Boolean, msg As String)
        alertPLC = Me.Master.FindControl("plcAlert")
        If type = True Then
            alert.Text = alertmsg.success_message(msg)
        Else
            alert.Text = alertmsg.error_message(msg)
        End If
        alertPLC.Controls.Add(alert)
    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("staffid") = Nothing Then            Dim x As ArrayList = check.Check_app_staff(Request.QueryString("username"), Request.QueryString("password"))            If x.Count <> 0 Then                Session("staffid") = x.Item(0)                Session("sessionid") = x.Item(1)            End If        End If
        Try


            If Not IsPostBack Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.Open()
                    Dim cmdCheck20 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Class where ID = '" & Session("ClassId") & "'", con)
                    Dim reader0 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck20.ExecuteReader()
                    reader0.Read()
                    txtClass.Text = reader0.Item(1).ToString
                    reader0.Close()



                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session where ID = '" & Session("SessionId") & "'", con)
                    Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader()
                    reader.Read()
                    txtTerm.Text = reader.Item(1).ToString & " - " & reader.Item(2).ToString
                    reader.Close()
                    If Request.QueryString.ToString <> Nothing Then
                        txtDate.Text = Replace(Request.QueryString.ToString, "+", " ")
                        txtDate.Text = Replace(txtDate.Text, "%2c", ",")
                        Dim thatday As Date = txtDate.Text
                        txtDay.Text = thatday.DayOfWeek.ToString
                        Dim ref3 As New MySql.Data.MySqlClient.MySqlCommand("Select studentsprofile.admno, studentsprofile.surname, attendance.morning, attendance.afternoon, attendance.remarks, attendance.holiday, attendance.week from attendance inner join studentsprofile on attendance.student = studentsprofile.admno where attendance.date = '" & txtDate.Text & "' and attendance.class = '" & Session("ClassId") & "' and attendance.term = '" & Session("SessionId") & "'", con)
                        Dim readref3 As MySql.Data.MySqlClient.MySqlDataReader = ref3.ExecuteReader
                        Dim ds As New DataTable
                        ds.Columns.Add("serial")
                        ds.Columns.Add("admno")
                        ds.Columns.Add("sname")
                        ds.Columns.Add("morning")
                        ds.Columns.Add("afternoon")
                        Dim holiday As String = Nothing
                        Dim n As Integer = 1
                        Do While readref3.Read
                            txtWeek.Text = readref3.Item(6)
                            If readref3.Item(5) = True Then
                                holiday = readref3.Item(4)
                                Exit Do
                            End If
                            ds.Rows.Add(n, readref3.Item(0), readref3.Item(1), readref3.Item(2), readref3.Item(3))
                            n = n + 1
                        Loop
                        readref3.Close()
                        GridView1.DataSource = ds
                        GridView1.DataBind()
                        Dim ref3x As New MySql.Data.MySqlClient.MySqlCommand("select remarks from attendance where term = '" & Session("SessionId") & "' and date = '" & txtDate.Text & "' and class = '" & Session("ClassId") & "'", con)
                        Dim readref3x As MySql.Data.MySqlClient.MySqlDataReader = ref3x.ExecuteReader
                        readref3x.Read()
                        If readref3x(0).ToString <> "" Then
                            chkHoliday.Checked = True
                            TextBox1.Text = readref3x(0)
                            GridView1.Visible = False
                            TextBox1.Enabled = True
                        End If
                        readref3x.Close()

                    Else



                        Dim ref As New MySql.Data.MySqlClient.MySqlCommand("Select date, week from attendance where term = '" & Session("SessionId") & "' and class = '" & Session("ClassId") & "' order by id desc", con)
                        Dim readref As MySql.Data.MySqlClient.MySqlDataReader = ref.ExecuteReader
                        If readref.Read() Then

                            Dim nextattendance As Date = readref.Item(0)
                            Dim week As Integer = readref.Item(1)
                            Do
                                nextattendance = nextattendance.AddDays(1)
                            Loop While nextattendance.DayOfWeek.ToString = "Saturday" Or nextattendance.DayOfWeek.ToString = "Sunday"
                            readref.Close()
                            If nextattendance.DayOfWeek.ToString = "Monday" Then week = week + 1
                            txtDate.Text = nextattendance.ToLongDateString
                            txtDay.Text = nextattendance.DayOfWeek.ToString
                            txtWeek.Text = week
                        Else
                            readref.Close()
                            Dim ref2 As New MySql.Data.MySqlClient.MySqlCommand("Select opendate from session where ID = '" & Session("SessionId") & "'", con)
                            Dim readref2 As MySql.Data.MySqlClient.MySqlDataReader = ref2.ExecuteReader
                            readref2.Read()
                            Dim firstattendance As Date = DateTime.ParseExact(readref2(0).ToString, "dd/MM/yyyy", Nothing)
                            readref2.Close()
                            txtDate.Text = firstattendance.ToLongDateString
                            DayNameFormat.Full.ToString()
                            txtDay.Text = firstattendance.DayOfWeek.ToString
                            txtWeek.Text = 1
                        End If
                        Dim ref3 As New MySql.Data.MySqlClient.MySqlCommand("Select studentsprofile.admno, studentsprofile.surname from studentsummary inner join studentsprofile on studentsummary.student = studentsprofile.admno where studentsummary.class = '" & Session("ClassId") & "' and studentsummary.session = '" & Session("SessionId") & "'", con)
                        Dim readref3 As MySql.Data.MySqlClient.MySqlDataReader = ref3.ExecuteReader
                        Dim ds As New DataTable
                        ds.Columns.Add("serial")
                        ds.Columns.Add("admno")
                        ds.Columns.Add("sname")
                        ds.Columns.Add("morning")
                        ds.Columns.Add("afternoon")
                        Dim n As Integer = 1
                        Do While readref3.Read
                            ds.Rows.Add(n, readref3.Item(0), readref3.Item(1), True, True)
                            n = n + 1
                        Loop
                        readref3.Close()
                        GridView1.DataSource = ds
                        GridView1.DataBind()

                    End If
                    con.Close()
                End Using
            End If

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub


    Protected Sub GridView1_PageIndexChanged(sender As Object, e As EventArgs) Handles GridView1.PageIndexChanged

    End Sub



    Protected Sub GridView1_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub GridView1_SelectedIndexChanged1(sender As Object, e As EventArgs) Handles GridView1.SelectedIndexChanged

    End Sub

    Protected Sub GridView1_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs)

    End Sub



    Protected Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()

                If Request.QueryString.ToString <> Nothing Then

                    If chkHoliday.Checked = True Then
                        For Each row As GridViewRow In GridView1.Rows
                            Dim ref3 As New MySql.Data.MySqlClient.MySqlCommand("Update attendance set morning = '" & -Val(False) & "', afternoon = '" & -Val(False) & "', remarks = '" & TextBox1.Text & "', holiday = '" & -Val(True) & "' where term = '" & Session("SessionId") & "' and date = '" & txtDate.Text & "' and class = '" & Session("ClassId") & "'", con)
                            ref3.ExecuteNonQuery()
                        Next
                    Else
                        For Each row As GridViewRow In GridView1.Rows
                            Dim morning As Boolean = TryCast(row.Cells(3).Controls(0), CheckBox).Checked
                            Dim afternoon As Boolean = TryCast(row.Cells(4).Controls(0), CheckBox).Checked
                            Dim ref3 As New MySql.Data.MySqlClient.MySqlCommand("Update attendance set morning = '" & -Val(morning) & "', afternoon = '" & -Val(afternoon) & "' where date = '" & txtDate.Text & "' and student = '" & row.Cells(1).Text & "'", con)
                            ref3.ExecuteNonQuery()
                        Next
                    End If
                    logify.log(Session("staffid"), "Attendance on " & txtDate.Text & " was updated for " & txtClass.Text)

                Else
                    Dim ref3z As New MySql.Data.MySqlClient.MySqlCommand("delete from attendance where date = '" & txtDate.Text & "' and term = '" & Session("SessionId") & "' and class = '" & Session("ClassId") & "'", con)
                    ref3z.ExecuteNonQuery()
                    If chkHoliday.Checked = True Then
                        For Each row As GridViewRow In GridView1.Rows
                            Dim ref3 As New MySql.Data.MySqlClient.MySqlCommand("Insert into attendance (week, student, term, date, morning, afternoon, remarks, class, holiday) values ('" & txtWeek.Text & "','" & row.Cells(1).Text & "','" & Session("SessionId") & "','" & txtDate.Text & "','" & -Val(False) & "','" & -Val(False) & "','" & TextBox1.Text & "','" & Session("ClassId") & "','" & -Val(True) & "')", con)
                            ref3.ExecuteNonQuery()
                        Next
                    Else
                        For Each row As GridViewRow In GridView1.Rows
                            Dim morning As Boolean = TryCast(row.Cells(3).Controls(0), CheckBox).Checked
                            Dim afternoon As Boolean = TryCast(row.Cells(4).Controls(0), CheckBox).Checked

                            Dim ref3 As New MySql.Data.MySqlClient.MySqlCommand("Insert into attendance (week, student, term, date, morning, afternoon, class) values ('" & txtWeek.Text & "','" & row.Cells(1).Text & "','" & Session("SessionId") & "','" & txtDate.Text & "','" & -Val(morning) & "','" & -Val(afternoon) & "','" & Session("ClassId") & "')", con)
                            ref3.ExecuteNonQuery()
                        Next
                    End If
                    logify.log(Session("staffid"), "Attendance on " & txtDate.Text & " was taken for " & txtClass.Text)
                End If
                Dim ref30 As New MySql.Data.MySqlClient.MySqlCommand("Select studentsprofile.admno, studentsprofile.surname from studentsummary inner join studentsprofile on studentsummary.student = studentsprofile.admno where studentsummary.class = '" & Session("ClassId") & "' and studentsummary.session = '" & Session("SessionId") & "'", con)
                Dim readref30 As MySql.Data.MySqlClient.MySqlDataReader = ref30.ExecuteReader
                Dim students As New ArrayList
                Do While readref30.Read
                    students.Add(readref30.Item(0))
                Loop
                readref30.Close()
                For Each student As String In students
                    Dim present As Integer = 0
                    Dim absent As Integer = 0
                    Dim opened As Integer = 0
                    Dim ref As New MySql.Data.MySqlClient.MySqlCommand("Select morning, afternoon from attendance where term = '" & Session("SessionId") & "' and student = '" & student & "' order by id desc", con)
                    Dim readref As MySql.Data.MySqlClient.MySqlDataReader = ref.ExecuteReader
                    Do While readref.Read
                        present = present - Val(readref.Item(0)) - Val(readref.Item(1))
                    Loop
                    readref.Close()
                    Dim refs As New MySql.Data.MySqlClient.MySqlCommand("Select date from attendance where term = '" & Session("SessionId") & "' and class = '" & Session("classid") & "' and holiday = '" & -Val(False) & "'", con)
                    Dim readrefs As MySql.Data.MySqlClient.MySqlDataReader = refs.ExecuteReader
                    Dim dates As New ArrayList
                    Do While readrefs.Read
                        If Not dates.Contains(readrefs.Item(0)) Then
                            dates.Add(readrefs.Item(0))
                        End If
                    Loop
                    readrefs.Close()
                    opened = dates.Count * 2
                    absent = opened - present
                    Dim ref3 As New MySql.Data.MySqlClient.MySqlCommand("Update studentsummary set present = '" & present & "', absent = '" & absent & "' where student = '" & student & "' and session = '" & Session("sessionid") & "'", con)
                    ref3.ExecuteNonQuery()
                Next
                con.close()            End Using
            Response.Redirect("~/content/app/Staff/attendance.aspx")
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub





    Protected Sub GridView1_RowEditing(sender As Object, e As GridViewEditEventArgs)
        Try
            GridView1.EditIndex = e.NewEditIndex
            Dim ds As New DataTable
            ds.Columns.Add("serial")
            ds.Columns.Add("admno")
            ds.Columns.Add("sname")
            ds.Columns.Add("morning")
            ds.Columns.Add("afternoon")
            Dim o As Integer = 0
            For Each grow As GridViewRow In GridView1.Rows

                ds.Rows.Add(grow.Cells(0).Text, grow.Cells(1).Text, grow.Cells(2).Text, TryCast(grow.Cells(3).Controls(0), CheckBox).Checked, TryCast(grow.Cells(4).Controls(0), CheckBox).Checked)


            Next


            GridView1.DataSource = ds
            GridView1.DataBind()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub GridView1_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)
        GridView1.EditIndex = -1

    End Sub

    Protected Sub GridView1_RowUpdating(sender As Object, e As GridViewUpdateEventArgs)
        Try
            GridView1.EditIndex = -1

            Dim row As GridViewRow = GridView1.Rows(e.RowIndex)
            Dim morning As Boolean = TryCast(row.Cells(3).Controls(0), CheckBox).Checked
            Dim afternoon As Boolean = TryCast(row.Cells(4).Controls(0), CheckBox).Checked

            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim ds As New DataTable
                ds.Columns.Add("serial")
                ds.Columns.Add("admno")
                ds.Columns.Add("sname")
                ds.Columns.Add("morning")
                ds.Columns.Add("afternoon")
                Dim o As Integer = 0
                For Each grow As GridViewRow In GridView1.Rows
                    If o = e.RowIndex Then
                        ds.Rows.Add(grow.Cells(0).Text, grow.Cells(1).Text, grow.Cells(2).Text, morning, afternoon)
                        o = o + 1
                    Else
                        ds.Rows.Add(grow.Cells(0).Text, grow.Cells(1).Text, grow.Cells(2).Text, TryCast(grow.Cells(3).Controls(0), CheckBox).Checked, TryCast(grow.Cells(4).Controls(0), CheckBox).Checked)
                        o = o + 1

                    End If
                Next


                GridView1.DataSource = ds

                GridView1.DataBind()

                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try

    End Sub

    Protected Sub chkHoliday_CheckedChanged(sender As Object, e As EventArgs) Handles chkHoliday.CheckedChanged
        If chkHoliday.Checked = True Then
            TextBox1.Enabled = True
            GridView1.Visible = False
            TextBox1.Text = ""
        Else
            TextBox1.Enabled = False
            GridView1.Visible = True
            TextBox1.Text = ""
        End If

    End Sub
End Class
