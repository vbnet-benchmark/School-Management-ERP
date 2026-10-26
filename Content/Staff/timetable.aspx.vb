Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls



Partial Class Admin_staffprofile
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
        If check.Check_Subject(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try

       
            If Not IsPostBack Then
                logify.Read_notification("~/content/staff/timetable.aspx", Session("staffid"))
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.open()
                    Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.class, subjects.subject, classsubjects.periods from classsubjects Inner Join Class on Class.ID = classsubjects.class Inner Join Subjects on Subjects.ID = classsubjects.subject  WHERE classsubjects.teacher = ?", con)
                    cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Session("Staffid")))
                    Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
                    Dim total As Integer = 0
                    Dim sda As New DataTable
                    sda.Columns.Add("subject")
                    sda.Columns.Add("class")
                    sda.Columns.Add("periods")

                    Do While reader2.Read
                        sda.Rows.Add(reader2(1), reader2(0), reader2(2))
                        total = total + Val(reader2(2))
                    Loop
                    reader2.Close()
                    sda.Rows.Add("TOTAL PERIODS PER WEEK", "", total)
                    GridView2.DataSource = sda
                    GridView2.DataBind()
                    con.Close()
                End Using
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub cboDay_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDay.SelectedIndexChanged
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
        Dim subjects As New ArrayList
        Dim classes As New ArrayList
            Dim periods As New ArrayList
            Dim timetable As New ArrayList
        Dim range As New ArrayList
            Dim cmdLo As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject, timetable.period, class.class, timetable.tname from timetable inner join ttname on timetable.tname = ttname.id inner join class on timetable.class = class.id inner join subjects on timetable.subject = subjects.id where timetable.teacher = '" & Session("staffid") & "' and timetable.day = '" & cboDay.Text & "' and ttname.default = '" & 1 & "' order by timetable.period", con)
        Dim studen As MySql.Data.MySqlClient.MySqlDataReader = cmdLo.ExecuteReader
        Do While studen.Read
            subjects.Add(studen(0).ToString)
            periods.Add(studen(1).ToString)
                classes.Add(studen(2).ToString)
                timetable.Add(studen(3).ToString)
            Loop
          
        studen.Close()
        Dim ds3 As New DataTable
        ds3.Columns.Add("time")
        ds3.Columns.Add("subject")
        ds3.Columns.Add("class")
        Dim count As Integer = 0
        For Each item As Integer In periods
                Dim cmdLoad10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT tperiods.timestart, tperiods.timeend, tperiods.activity from tperiods inner join ttname on ttname.id = tperiods.timetable where ttname.default = '" & 1 & "' and tperiods.day = '" & cboDay.Text & "' and timetable = '" & timetable(count) & "' order by tperiods.timestart", con)
            Dim student10 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10.ExecuteReader
            Dim period As Integer = 0
            Do While student10.Read
                If student10(2).ToString = "Tutorial" Then
                    period = period + 1
                End If
                If period = item Then
                    ds3.Rows.Add(student10(0).ToString & " - " & student10(1).ToString, subjects(count), classes(count))
                    Exit Do
                End If

            Loop
            student10.Close()
                count = count + 1
                period = Nothing
        Next
        GridView1.DataSource = ds3
        GridView1.DataBind()
            con.close()        End Using
    End Sub
End Class
