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
    Public Function Get_subordinates(ByVal dept As String) As ArrayList
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.Open()

            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dept from depts where superior = '" & dept & "'", con)
            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            Dim subo As New ArrayList
            Do While student1.Read
                subo.Add(student1.Item(0))
            Loop
            student1.Close()
            Return subo
            con.Close()
        End Using
    End Function
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("staffid") = Nothing Then            Dim x As ArrayList = check.Check_app_staff(Request.QueryString("username"), Request.QueryString("password"))            If x.Count <> 0 Then                Session("staffid") = x.Item(0)                Session("sessionid") = x.Item(1)            End If        End If
        Try


            If Not IsPostBack Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.Open()
                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dept, headtitle from depts where head = '" & Session("staffid") & "'", con)
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                    Dim deptshead As New ArrayList
                    Do While student0.Read
                        deptshead.Add(student0.Item(0))
                    Loop
                    student0.Close()
                    Dim classcontroled As New ArrayList
                    Dim secsub As New ArrayList
                    Dim mysub As New ArrayList
                    For Each item As String In deptshead
                        classcontroled.Add(item)
                        mysub = Get_subordinates(item)

                        For Each subitem As String In mysub
                            classcontroled.Add(subitem)
                            secsub.Add(subitem)
                        Next
                    Next
                    Dim thirdsub As New ArrayList
                    For Each item As String In secsub

                        mysub = Get_subordinates(item)
                        For Each subitem As String In mysub
                            classcontroled.Add(subitem)
                            thirdsub.Add(subitem)
                        Next
                    Next
                    Dim fourthsub As New ArrayList
                    For Each item As String In thirdsub

                        mysub = Get_subordinates(item)
                        For Each subitem As String In mysub
                            classcontroled.Add(subitem)
                            fourthsub.Add(subitem)
                        Next
                    Next
                    Dim fifthsub As New ArrayList
                    For Each item As String In fourthsub

                        mysub = Get_subordinates(item)
                        For Each subitem As String In mysub
                            classcontroled.Add(subitem)
                        Next
                    Next
                    Dim classgroups As New ArrayList
                    For Each item As String In deptshead
                        Dim classes As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from class inner join depts on class.superior = depts.Id where depts.dept = '" & item & "'", con)
                        Dim schclass As MySql.Data.MySqlClient.MySqlDataReader = classes.ExecuteReader
                        Dim d As Boolean = False
                        Do While schclass.Read
                            d = True
                        Loop
                        If d = True Then
                            classgroups.Add(item)
                        End If
                        schclass.Close()
                    Next
                    Dim teachingstaff As New ArrayList
                    Dim deptstaff As New ArrayList
                    For Each item As String In classcontroled
                        Dim f As Boolean = False
                        Dim classes As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from class inner join depts on class.superior = depts.Id where depts.dept = '" & item & "'", con)
                        Dim schclass As MySql.Data.MySqlClient.MySqlDataReader = classes.ExecuteReader
                        Do While schclass.Read
                            f = True
                        Loop
                        If f = True Then
                            classgroups.Add(item)
                        End If
                        schclass.Close()
                        Dim classes1 As New MySql.Data.MySqlClient.MySqlCommand("Select staff from staffdept inner join depts on staffdept.dept = depts.id where depts.dept = '" & item & "'", con)
                        Dim schclass1 As MySql.Data.MySqlClient.MySqlDataReader = classes1.ExecuteReader
                        Do While schclass1.Read
                            deptstaff.Add(schclass1.Item(0))
                        Loop
                        schclass1.Close()
                        DropDownList1.Items.Clear()
                        For Each sitem As String In deptstaff
                            Dim classes11 As New MySql.Data.MySqlClient.MySqlCommand("Select staffprofile.surname from classsubjects inner join staffprofile on classsubjects.teacher = staffprofile.staffid where classsubjects.teacher = '" & sitem & "'  and staffprofile.activated = '" & 1 & "'", con)
                            Dim schclass11 As MySql.Data.MySqlClient.MySqlDataReader = classes11.ExecuteReader

                            If schclass11.Read Then
                                DropDownList1.Items.Add(schclass11.Item(0).ToString)
                            End If
                            schclass11.Close()
                        Next
                    Next
                    Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.class, subjects.subject, classsubjects.periods from classsubjects inner join staffprofile on staffprofile.staffid = classsubjects.teacher Inner Join Class on Class.ID = classsubjects.class Inner Join Subjects on Subjects.ID = classsubjects.subject  WHERE staffprofile.surname = ?", con)
                    cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", DropDownList1.Text))
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
                    sda.Rows.Add("Total periods", "", total)
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
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim subjects As New ArrayList
                Dim classes As New ArrayList
                Dim periods As New ArrayList
                Dim range As New ArrayList
                Dim cmdLo As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject, timetable.period, class.class from timetable inner join staffprofile on staffprofile.staffid = timetable.teacher inner join ttname on timetable.tname = ttname.id inner join class on timetable.class = class.id inner join subjects on timetable.subject = subjects.id where staffprofile.surname = '" & DropDownList1.Text & "' and timetable.day = '" & cboDay.Text & "' and ttname.default = '" & 1 & "' order by timetable.period", con)
                Dim studen As MySql.Data.MySqlClient.MySqlDataReader = cmdLo.ExecuteReader
                Do While studen.Read
                    subjects.Add(studen(0).ToString)
                    periods.Add(studen(1).ToString)
                    classes.Add(studen(2).ToString)
                Loop
                studen.Close()
                Dim ds3 As New DataTable
                ds3.Columns.Add("time")
                ds3.Columns.Add("subject")
                ds3.Columns.Add("class")
                Dim count As Integer = 0
                For Each item As Integer In periods
                    Dim cmdLoad10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT tperiods.timestart, tperiods.timeend, tperiods.activity from tperiods inner join ttname on ttname.id = tperiods.timetable where ttname.default = '" & 1 & "' and tperiods.day = '" & cboDay.Text & "' order by tperiods.timestart", con)
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
                Next
                GridView1.DataSource = ds3
                GridView1.DataBind()
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.class, subjects.subject, classsubjects.periods from classsubjects inner join staffprofile on staffprofile.staffid = classsubjects.teacher Inner Join Class on Class.ID = classsubjects.class Inner Join Subjects on Subjects.ID = classsubjects.subject  WHERE staffprofile.surname = ?", con)
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", DropDownList1.Text))
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
                sda.Rows.Add("Total periods", "", total)
                GridView2.DataSource = sda
                GridView2.DataBind()
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
End Class
