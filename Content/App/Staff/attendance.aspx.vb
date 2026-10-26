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

                    Dim ref As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from classteacher inner join class on class.id = classteacher.class where classteacher.teacher = '" & Session("staffid") & "'", con)
                    Dim readref As MySql.Data.MySqlClient.MySqlDataReader = ref.ExecuteReader
                    cboClass.Items.Clear()
                    Do While readref.Read
                        cboClass.Items.Add(readref.Item(0))
                    Loop
                    readref.Close()
                    Dim cmdLoad10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id from class where class = ?", con)
                    cmdLoad10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", cboClass.Text))
                    Dim student04 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10.ExecuteReader
                    student04.Read()
                    Session("classid") = student04.Item(0)
                    student04.Close()
                    Dim ref2 As New MySql.Data.MySqlClient.MySqlCommand("Select attendance.week from attendance inner join class on class.id = attendance.class where class.class = '" & cboClass.Text & "' and attendance.term = '" & Session("SessionId") & "'", con)
                    Dim readref2 As MySql.Data.MySqlClient.MySqlDataReader = ref2.ExecuteReader
                    cboWeek.Items.Clear()
                    Dim weeks As New ArrayList
                    Do While readref2.Read
                        If Not weeks.Contains(readref2.Item(0)) Then
                            weeks.Add(readref2.Item(0))
                            cboWeek.Items.Add(readref2.Item(0))
                        End If
                        cboWeek.Text = readref2.Item(0)
                    Loop

                    readref2.Close()
                    Dim ref20 As New MySql.Data.MySqlClient.MySqlCommand("Select attendance.date from attendance inner join class on class.id = attendance.class where class.class = '" & cboClass.Text & "' and attendance.term = '" & Session("SessionId") & "' and attendance.week = '" & cboWeek.Text & "'", con)
                    Dim readref20 As MySql.Data.MySqlClient.MySqlDataReader = ref20.ExecuteReader
                    Dim days As New ArrayList
                    Do While readref20.Read
                        If Not days.Contains(readref20.Item(0)) Then
                            days.Add(readref20.Item(0))
                        End If
                    Loop
                    readref20.Close()
                    Dim ds As New DataTable
                    ds.Columns.Add("Date")
                    ds.Columns.Add("Day")
                    ds.Columns.Add("Morning")
                    ds.Columns.Add("Afternoon")
                    ds.Columns.Add("Percent")
                    ds.Columns.Add("Remarks")
                    Dim weeklypercent As Double = 0
                    Dim ct As Integer
                    For Each item As Date In days
                        Dim ref3 As New MySql.Data.MySqlClient.MySqlCommand("Select attendance.morning, attendance.afternoon, attendance.holiday, attendance.remarks from attendance inner join class on class.id = attendance.class where class.class = '" & cboClass.Text & "' and attendance.term = '" & Session("SessionId") & "' and attendance.date = '" & item.ToLongDateString & "'", con)
                        Dim readref3 As MySql.Data.MySqlClient.MySqlDataReader = ref3.ExecuteReader
                        Dim afternoon As Integer = 0
                        Dim morning As Integer = 0
                        Dim n As Integer = 0
                        Dim holiday As String = Nothing
                        Do While readref3.Read
                            If readref3.Item(2) = 1 Then
                                holiday = readref3.Item(3)
                            End If
                            If readref3.Item(0) = True Then
                                morning = morning + 1
                            End If
                            If readref3.Item(1) = True Then
                                afternoon = afternoon + 1
                            End If
                            n = n + 1
                        Loop
                        readref3.Close()

                        Dim percentage As Double = FormatNumber(((morning + afternoon) / (2 * n)) * 100, 2)
                        weeklypercent = weeklypercent + percentage
                        If holiday = Nothing Then
                            ds.Rows.Add(item.ToLongDateString, item.DayOfWeek.ToString, morning, afternoon, percentage & "%", "None")
                            ct = ct + 1
                        Else
                            ds.Rows.Add(item.ToLongDateString, item.DayOfWeek.ToString, "-", "-", "-", holiday)

                        End If

                    Next
                    weeklypercent = weeklypercent / ct
                    lblweekly.Text = "Weekly Percentage: " & FormatNumber(weeklypercent, 2) & "%"
                    GridView1.DataSource = ds
                    GridView1.DataBind()
                    con.Close()                End Using

            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub


    Protected Sub GridView1_PageIndexChanged(sender As Object, e As EventArgs) Handles GridView1.PageIndexChanged

    End Sub

    Protected Sub GridView1_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GridView1.PageIndexChanging



    End Sub

    Protected Sub GridView1_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub GridView1_SelectedIndexChanged1(sender As Object, e As EventArgs) Handles GridView1.SelectedIndexChanged

    End Sub

    Protected Sub GridView1_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs)

    End Sub



    Protected Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnTake.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id from class where class = ?", con)
                cmdLoad10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", cboClass.Text))
                Dim student04 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10.ExecuteReader
                student04.Read()
                Session("classid") = student04.Item(0)
                student04.Close()
                con.close()            End Using
            Response.Redirect("~/content/app/Staff/newattendance.aspx")
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub



    Protected Sub cboClass_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboWeek.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id from class where class = ?", con)
                cmdLoad10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", cboClass.Text))
                Dim student04 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10.ExecuteReader
                student04.Read()
                Session("classid") = student04.Item(0)
                student04.Close()
                Dim ref20 As New MySql.Data.MySqlClient.MySqlCommand("Select attendance.date from attendance inner join class on class.id = attendance.class where class.class = '" & cboClass.Text & "' and attendance.term = '" & Session("SessionId") & "' and attendance.week = '" & cboWeek.Text & "'", con)
                Dim readref20 As MySql.Data.MySqlClient.MySqlDataReader = ref20.ExecuteReader
                Dim days As New ArrayList
                Do While readref20.Read
                    If Not days.Contains(readref20.Item(0)) Then
                        days.Add(readref20.Item(0))
                    End If
                Loop
                readref20.Close()
                Dim ds As New DataTable
                ds.Columns.Add("Date")
                ds.Columns.Add("Day")
                ds.Columns.Add("Morning")
                ds.Columns.Add("Afternoon")
                ds.Columns.Add("Percent")
                ds.Columns.Add("Remarks")
                Dim weeklypercent As Double = 0
                Dim ct As Integer
                For Each item As Date In days
                    Dim ref3 As New MySql.Data.MySqlClient.MySqlCommand("Select attendance.morning, attendance.afternoon, attendance.holiday, attendance.remarks from attendance inner join class on class.id = attendance.class where class.class = '" & cboClass.Text & "' and attendance.term = '" & Session("SessionId") & "' and attendance.date = '" & item.ToLongDateString & "'", con)
                    Dim readref3 As MySql.Data.MySqlClient.MySqlDataReader = ref3.ExecuteReader
                    Dim afternoon As Integer = 0
                    Dim morning As Integer = 0
                    Dim n As Integer = 0
                    Dim holiday As String = Nothing

                    Do While readref3.Read
                        If readref3.Item(2) = 1 Then
                            holiday = readref3.Item(3)
                        End If
                        If readref3.Item(0) = True Then
                            morning = morning + 1
                        End If
                        If readref3.Item(1) = True Then
                            afternoon = afternoon + 1
                        End If
                        n = n + 1
                    Loop
                    readref3.Close()
                    Dim percentage As Double = FormatNumber(((morning + afternoon) / (2 * n)) * 100, 2)
                    weeklypercent = weeklypercent + percentage
                    If holiday = Nothing Then
                        ds.Rows.Add(item.ToLongDateString, item.DayOfWeek.ToString, morning, afternoon, percentage & "%", "None")
                        ct = ct + 1
                    Else
                        ds.Rows.Add(item.ToLongDateString, item.DayOfWeek.ToString, "-", "-", "-", holiday)

                    End If

                Next
                weeklypercent = weeklypercent / ct
                lblweekly.Text = "Weekly Percentage: " & FormatNumber(weeklypercent, 2) & "%"
                GridView1.DataSource = ds
                GridView1.DataBind()
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try

    End Sub

    Protected Sub cboClass_SelectedIndexChanged1(sender As Object, e As EventArgs) Handles cboClass.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id from class where class = ?", con)
                cmdLoad10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", cboClass.Text))
                Dim student04 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10.ExecuteReader
                student04.Read()
                Session("classid") = student04.Item(0)
                student04.Close()
                Dim ref2 As New MySql.Data.MySqlClient.MySqlCommand("Select attendance.week from attendance inner join class on class.id = attendance.class where class.class = '" & cboClass.Text & "' and attendance.term = '" & Session("SessionId") & "'", con)
                Dim readref2 As MySql.Data.MySqlClient.MySqlDataReader = ref2.ExecuteReader
                cboWeek.Items.Clear()
                Dim weeks As New ArrayList
                Do While readref2.Read
                    If Not weeks.Contains(readref2.Item(0)) Then
                        weeks.Add(readref2.Item(0))
                        cboWeek.Items.Add(readref2.Item(0))
                    End If
                    cboWeek.Text = readref2.Item(0)
                Loop

                readref2.Close()
                Dim ref20 As New MySql.Data.MySqlClient.MySqlCommand("Select attendance.date from attendance inner join class on class.id = attendance.class where class.class = '" & cboClass.Text & "' and attendance.term = '" & Session("SessionId") & "' and attendance.week = '" & cboWeek.Text & "'", con)
                Dim readref20 As MySql.Data.MySqlClient.MySqlDataReader = ref20.ExecuteReader
                Dim days As New ArrayList
                Do While readref20.Read
                    If Not days.Contains(readref20.Item(0)) Then
                        days.Add(readref20.Item(0))
                    End If
                Loop
                readref20.Close()
                Dim ds As New DataTable
                ds.Columns.Add("Date")
                ds.Columns.Add("Day")
                ds.Columns.Add("Morning")
                ds.Columns.Add("Afternoon")
                ds.Columns.Add("Percent")
                ds.Columns.Add("Remarks")
                Dim weeklypercent As Double = 0
                Dim ct As Integer
                For Each item As Date In days
                    Dim ref3 As New MySql.Data.MySqlClient.MySqlCommand("Select attendance.morning, attendance.afternoon, attendance.holiday, attendance.remarks from attendance inner join class on class.id = attendance.class where class.class = '" & cboClass.Text & "' and attendance.term = '" & Session("SessionId") & "' and attendance.date = '" & item.ToLongDateString & "'", con)
                    Dim readref3 As MySql.Data.MySqlClient.MySqlDataReader = ref3.ExecuteReader
                    Dim afternoon As Integer = 0
                    Dim morning As Integer = 0
                    Dim n As Integer = 0
                    Dim holiday As String = Nothing

                    Do While readref3.Read
                        If readref3.Item(2) = 1 Then
                            holiday = readref3.Item(3)
                        End If
                        If readref3.Item(0) = True Then
                            morning = morning + 1
                        End If
                        If readref3.Item(1) = True Then
                            afternoon = afternoon + 1
                        End If
                        n = n + 1
                    Loop
                    readref3.Close()
                    Dim percentage As Double = FormatNumber(((morning + afternoon) / (2 * n)) * 100, 2)
                    weeklypercent = weeklypercent + percentage
                    If holiday = Nothing Then
                        ds.Rows.Add(item.ToLongDateString, item.DayOfWeek.ToString, morning, afternoon, percentage & "%", "None")
                        ct = ct + 1
                    Else
                        ds.Rows.Add(item.ToLongDateString, item.DayOfWeek.ToString, "-", "-", "-", holiday)

                    End If

                Next
                weeklypercent = weeklypercent / ct
                lblweekly.Text = "Weekly Percentage: " & weeklypercent & "%"
                GridView1.DataSource = ds
                GridView1.DataBind()
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
End Class
