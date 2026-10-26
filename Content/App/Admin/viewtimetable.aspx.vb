Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Partial Class Default2
    Inherits System.Web.UI.Page

    Dim cmd As New MySql.Data.MySqlClient.MySqlCommand
    Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand
    Dim da As New MySql.Data.MySqlClient.MySqlDataAdapter
    Dim ds As New DataSet
    Dim dr As MySql.Data.MySqlClient.MySqlDataReader
    Dim dr2 As MySql.Data.MySqlClient.MySqlDataReader
    Dim gridview1 As New GridView
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
                    Dim cmdInsert137a As New MySql.Data.MySqlClient.MySqlCommand("SELECT ttname.name, depts.dept, ttname.default, class.class, ttname.class, ttname.school from ttname left join depts on depts.id = ttname.school left join class on ttname.class = class.id where ttname.id = '" & Request.QueryString("timetable") & "'", con)
                    Dim student0d As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert137a.ExecuteReader
                    Dim ttname As String
                    student0d.Read()
                    If student0d(5) <> 0 Then
                        lblTimetable.Text = student0d(0).ToString & " - " & student0d(1).ToString
                        ttname = student0d(0)
                        If student0d(2) = 1 Then
                            lblStatus.Text = "In Use"
                            lblStatus.ForeColor = Drawing.Color.Green
                        Else
                            lblStatus.Text = "Not In Use"
                            lblStatus.ForeColor = Drawing.Color.Red
                        End If
                    Else
                        lblTimetable.Text = student0d(0).ToString & " - " & student0d(3).ToString
                        ttname = student0d(0)
                        If student0d(2) = 1 Then
                            lblStatus.Text = "In Use"
                            lblStatus.ForeColor = Drawing.Color.Green
                        Else
                            lblStatus.Text = "Not In Use"
                            lblStatus.ForeColor = Drawing.Color.Red
                        End If
                    End If
                    student0d.Close()
                    Dim ds As New DataTable
                    ds.Columns.Add("passport")
                    ds.Columns.Add("staffname")
                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport, staffid, surname from staffprofile", con)
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                    Do While student0.Read
                        ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                    Loop
                    student0.Close()
                    gridview2.DataSource = ds
                    gridview2.DataBind()


                    If Session("staffadd") <> Nothing Then
                        If Request.QueryString("day") <> Nothing Then
                            cboDay.Text = Request.QueryString("day")
                        End If
                        Staff_Details()
                    End If
                    If Request.QueryString("period") <> Nothing Then
                        btnCS.Visible = True
                        Dim cmdInsert12x As New MySql.Data.MySqlClient.MySqlCommand("SELECT timetable.subject, timetable.teacher, classsubjects.cgroup FROM timetable inner join classsubjects on timetable.class = classsubjects.class and timetable.subject = classsubjects.subject Where timetable.Day = '" & Request.QueryString("day") & "' And timetable.class = '" & Request.QueryString("class") & "' and timetable.period = '" & Request.QueryString("period") & "' and timetable.tname = '" & Request.QueryString("timetable") & "'", con)
                        Dim reader12x As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert12x.ExecuteReader
                        If reader12x.Read() Then
                            If reader12x(2).ToString <> 0 Then
                                Show_Alert(False, "The period selected is inculdes subjects offerred together and cannot be swapped.")
                                con.Close()
                                cboDay.Text = Request.QueryString("day")

                                load_time_table()
                                con.Close()                                Exit Sub
                            End If
                            reader12x.Close()
                            If Session("swap") = "start" Then
                                If Session("swapclass") <> Request.QueryString("class") Then
                                    Show_Alert(False, "Please select a subject in the same class")
                                    cboDay.Text = Request.QueryString("day")
                                    load_time_table()
                                    con.Close()                                    Exit Sub
                                Else
                                    If Session("tutorial") = True Then
                                        Dim groupedsubjects As New ArrayList
                                        Dim teachers As New ArrayList
                                        Dim cmdInsert12 As New MySql.Data.MySqlClient.MySqlCommand("SELECT timetable.subject, timetable.teacher FROM timetable inner join class on timetable.class = class.id Where timetable.Day = ? And class.id = ? and timetable.period = ? and timetable.tname = ?", con)
                                        cmdInsert12.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", Session("swapday")))
                                        cmdInsert12.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", Session("swapclass")))
                                        cmdInsert12.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Teacher", Session("swapperiod")))
                                        cmdInsert12.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Ter", Request.QueryString("timetable")))
                                        Dim reader12 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert12.ExecuteReader
                                        Do While reader12.Read
                                            groupedsubjects.Add(reader12(0))
                                            teachers.Add(reader12(1))
                                        Loop
                                        reader12.Close()
                                        Dim clt As Integer
                                        For Each item As Integer In groupedsubjects
                                            Dim cmdInsert88 As New MySql.Data.MySqlClient.MySqlCommand("SELECT classsubjects.subject, staffprofile.surname FROM texemptions inner join (classsubjects inner join staffprofile on staffprofile.staffid = classsubjects.teacher) on classsubjects.teacher = texemptions.teacher Where texemptions.day = ? And Classsubjects.class = ? and classsubjects.subject = '" & item & "'", con)
                                            cmdInsert88.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", Request.QueryString("day")))
                                            cmdInsert88.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("swapclass")))
                                            Dim reader88 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert88.ExecuteReader
                                            If reader88.Read Then
                                                Show_Alert(False, "Unable to swap. " & reader88(1).ToString & " is exempted from " & Request.QueryString("day"))
                                                reader88.Close()
                                                con.Close()
                                                cboDay.Text = Request.QueryString("day")
                                                load_time_table()
                                                con.Close()                                                Exit Sub
                                            End If
                                            reader88.Close()

                                            Dim cmdInsert137 As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname FROM timetable inner join staffprofile on staffprofile.staffid = timetable.teacher Where timetable.Day = ? And timetable.Period = ? And timetable.Teacher = ? and timetable.tname = ? and class <> '" & Session("swapclass") & "'", con)
                                            cmdInsert137.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", Request.QueryString("day")))
                                            cmdInsert137.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", Request.QueryString("period")))
                                            cmdInsert137.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Teacher", teachers(clt)))
                                            cmdInsert137.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Ter", Request.QueryString("timetable")))
                                            Dim reader137 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert137.ExecuteReader

                                            If reader137.Read Then
                                                Show_Alert(False, "Unable to swap. There is a clash for " & reader137(0).ToString)
                                                reader137.Close()
                                                con.Close()
                                                cboDay.Text = Request.QueryString("day")
                                                load_time_table()
                                                con.Close()                                                Exit Sub
                                            End If
                                            reader137.Close()

                                            Dim cmdInsert312 As New MySql.Data.MySqlClient.MySqlCommand("SELECT timetable.period, subjects.subject FROM timetable inner join subjects on subjects.id = timetable.subject Where timetable.Day = ? And timetable.Class = ? and timetable.subject = '" & item & "' and timetable.tname = '" & Request.QueryString("timetable") & "'", con)
                                            cmdInsert312.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", Request.QueryString("day")))
                                            cmdInsert312.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("swapclass")))
                                            Dim reader312 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert312.ExecuteReader
                                            Dim offered As Boolean = False
                                            Dim subject As String = ""
                                            Dim offperiod As Integer
                                            Dim offcount As Integer = 0
                                            Do While reader312.Read()
                                                offered = True
                                                offperiod = reader312(0).ToString
                                                subject = reader312(1).ToString
                                                offcount = offcount + 1
                                            Loop
                                            If offcount > 1 Then
                                                reader312.Close()
                                                Show_Alert(False, subject & " is already having a double period on " & Request.QueryString("day"))
                                                con.Close()
                                                cboDay.Text = Request.QueryString("day")
                                                load_time_table()
                                                con.Close()                                                Exit Sub
                                            End If
                                            reader312.Close()
                                            If offered = True And Session("swapday") <> Request.QueryString("day") Then
                                                Dim periodbeforebreaks As New ArrayList
                                                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from tperiods where day = '" & Request.QueryString("day") & "' and timetable = '" & Request.QueryString("timetable") & "'  order by timestart", con)
                                                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                                                Dim pb As Integer = 0
                                                Dim cxt As Integer = 0
                                                Do While reader.Read
                                                    If Not reader("activity").ToString = "Tutorial" Then
                                                        periodbeforebreaks.Add(cxt + 1)
                                                    Else
                                                        cxt = cxt + 1
                                                    End If
                                                Loop
                                                reader.Close()
                                                If Request.QueryString("period") <> offperiod + 1 Or periodbeforebreaks.Contains(Request.QueryString("period")) Then
                                                    Show_Alert(False, subject & " is already offerred on " & Request.QueryString("day") & " and is not a double period.")

                                                    cboDay.Text = Request.QueryString("day")
                                                    load_time_table()
                                                    con.Close()                                                    Exit Sub
                                                End If
                                                cxt = Nothing
                                            End If
                                            offperiod = Nothing
                                            offcount = Nothing
                                            offered = Nothing
                                            clt = clt + 1
                                        Next
                                        Dim groupedsubjects2 As New ArrayList
                                        Dim teachers2 As New ArrayList
                                        Dim cmdInsert1d As New MySql.Data.MySqlClient.MySqlCommand("SELECT timetable.subject, timetable.teacher FROM timetable inner join class on timetable.class = class.id Where timetable.Day = ? And class.id = ? and timetable.period = ? and timetable.tname = ?", con)
                                        cmdInsert1d.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", Request.QueryString("day")))
                                        cmdInsert1d.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", Replace(Request.QueryString("class"), "%", " ")))
                                        cmdInsert1d.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Teacher", Request.QueryString("period")))
                                        cmdInsert1d.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Ter", Request.QueryString("timetable")))
                                        Dim reader1d As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert1d.ExecuteReader
                                        Do While reader1d.Read
                                            groupedsubjects2.Add(reader1d(0))
                                            teachers2.Add(reader1d(1))
                                        Loop
                                        reader1d.Close()
                                        Dim clt2 As Integer
                                        For Each item As Integer In groupedsubjects2
                                            Dim cmdInsert88 As New MySql.Data.MySqlClient.MySqlCommand("SELECT classsubjects.subject, staffprofile.surname FROM texemptions inner join (classsubjects inner join staffprofile on staffprofile.staffid = classsubjects.teacher) on classsubjects.teacher = texemptions.teacher Where texemptions.day = ? And Classsubjects.class = ? and classsubjects.subject = '" & item & "'", con)
                                            cmdInsert88.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", Session("swapday")))
                                            cmdInsert88.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("swapclass")))
                                            Dim reader88 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert88.ExecuteReader
                                            If reader88.Read Then
                                                Show_Alert(False, "Unable to swap. " & reader88(1).ToString & " is exempted from " & Session("swapday"))
                                                reader88.Close()

                                                cboDay.Text = Request.QueryString("day")
                                                load_time_table()
                                                con.Close()                                                Exit Sub
                                            End If
                                            reader88.Close()

                                            Dim cmdInsert137 As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname FROM timetable inner join staffprofile on staffprofile.staffid = timetable.teacher Where timetable.Day = ? And timetable.Period = ? And timetable.Teacher = ? and timetable.tname = ? and class <> '" & Session("swapclass") & "'", con)
                                            cmdInsert137.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", Session("swapday")))
                                            cmdInsert137.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", Session("swapperiod")))
                                            cmdInsert137.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Teacher", teachers2(clt2)))
                                            cmdInsert137.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Ter", Request.QueryString("timetable")))
                                            Dim reader137 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert137.ExecuteReader
                                            If reader137.Read Then
                                                Show_Alert(False, "Unable to swap. There is a clash for " & reader137(0).ToString)
                                                reader137.Close()

                                                cboDay.Text = Request.QueryString("day")
                                                load_time_table()
                                                con.Close()                                                Exit Sub
                                            End If
                                            reader137.Close()

                                            Dim cmdInsert312 As New MySql.Data.MySqlClient.MySqlCommand("SELECT timetable.period, subjects.subject FROM timetable inner join subjects on subjects.id = timetable.subject Where timetable.Day = ? And timetable.Class = ? and timetable.subject = '" & item & "' and timetable.tname = '" & Request.QueryString("timetable") & "'", con)
                                            cmdInsert312.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", Session("swapday")))
                                            cmdInsert312.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("swapclass")))
                                            Dim reader312 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert312.ExecuteReader
                                            Dim offered2 As Boolean = False
                                            Dim subject2 As String = ""
                                            Dim offperiod2 As Integer
                                            Dim offcount2 As Integer = 0
                                            Do While reader312.Read()
                                                offered2 = True
                                                offperiod2 = reader312(0).ToString
                                                subject2 = reader312(1).ToString
                                                offcount2 = offcount2 + 1
                                            Loop
                                            If offcount2 > 1 Then
                                                reader312.Close()
                                                Show_Alert(False, subject2 & " is already having a double period on " & Session("swapday"))

                                                cboDay.Text = Request.QueryString("day")
                                                load_time_table()
                                                con.Close()                                                Exit Sub
                                            End If
                                            reader312.Close()
                                            If offered2 = True And Session("swapday") <> Request.QueryString("day") Then
                                                Dim periodbeforebreaks As New ArrayList
                                                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from tperiods where day = '" & Session("swapday") & "' and timetable = '" & Request.QueryString("timetable") & "'  order by timestart", con)
                                                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                                                Dim pb2 As Integer = 0
                                                Dim cxt2 As Integer = 0
                                                Do While reader.Read
                                                    If Not reader("activity").ToString = "Tutorial" Then
                                                        periodbeforebreaks.Add(cxt2 + 1)
                                                    Else
                                                        cxt2 = cxt2 + 1
                                                    End If
                                                Loop
                                                reader.Close()
                                                If Session("swapperiod") <> offperiod2 + 1 Or periodbeforebreaks.Contains(Session("swapperiod")) Then
                                                    Show_Alert(False, subject2 & " is already offerred on " & Session("swapday") & " and is not a double period.")

                                                    cboDay.Text = Request.QueryString("day")
                                                    load_time_table()
                                                    con.Close()                                                    Exit Sub
                                                End If
                                                cxt2 = Nothing
                                            End If

                                            offperiod2 = Nothing
                                            offcount2 = Nothing
                                            offered2 = Nothing
                                            clt2 = clt2 + 1
                                        Next
                                        Dim cmdInsert162 As New MySql.Data.MySqlClient.MySqlCommand("delete from timetable where tname = '" & Request.QueryString("timetable") & "' and day = '" & Session("swapday") & "' and class = '" & Session("swapclass") & "' and period = '" & Session("swapperiod") & "'", con)
                                        cmdInsert162.ExecuteNonQuery()
                                        Dim dn As Integer
                                        For Each item As Integer In groupedsubjects2
                                            Dim cmdInsert35 As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into timetable (tname, Class, Period, Subject, Doubles, Day, Teacher) Values(?,?,?,?,?,?,?)", con)
                                            cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ss", Request.QueryString("timetable")))
                                            cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", Session("swapclass")))
                                            cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", Session("swapperiod")))
                                            cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", item))
                                            cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Doubles", 0))
                                            cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", Session("swapday")))
                                            cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Teacher", teachers2(dn)))
                                            cmdInsert35.ExecuteNonQuery()
                                            dn = dn + 1

                                        Next
                                        Dim cmdInsert163 As New MySql.Data.MySqlClient.MySqlCommand("delete from timetable where tname = '" & Request.QueryString("timetable") & "' and day = '" & Request.QueryString("day") & "' and class = '" & Request.QueryString("class") & "' and period = '" & Request.QueryString("period") & "'", con)
                                        cmdInsert163.ExecuteNonQuery()
                                        Dim dn2 As Integer
                                        For Each item As Integer In groupedsubjects
                                            Dim cmdInsert35 As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into timetable (tname, Class, Period, Subject, Doubles, Day, Teacher) Values(?,?,?,?,?,?,?)", con)
                                            cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ss", Request.QueryString("timetable")))
                                            cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", Session("swapclass")))
                                            cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", Request.QueryString("period")))
                                            cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", item))
                                            cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Doubles", 0))
                                            cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", Request.QueryString("day")))
                                            cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Teacher", teachers(dn2)))
                                            cmdInsert35.ExecuteNonQuery()
                                            dn2 = dn2 + 1
                                        Next
                                        Dim teachingstaff As New ArrayList
                                        Dim classes112 As New MySql.Data.MySqlClient.MySqlCommand("Select classsubjects.teacher from classsubjects inner join class on class.id = classsubjects.class where class.id = '" & Session("swapclass") & "'", con)
                                        Dim schclass112 As MySql.Data.MySqlClient.MySqlDataReader = classes112.ExecuteReader
                                        Do While schclass112.Read
                                            If Not teachingstaff.Contains(schclass112(0)) Then
                                                teachingstaff.Add(schclass112(0))
                                            End If
                                        Loop
                                        schclass112.Close()
                                        Dim students As New ArrayList
                                        Dim classes112s As New MySql.Data.MySqlClient.MySqlCommand("Select studentsummary.student, studentsprofile.surname, class.class from studentsummary inner join studentsprofile on studentsprofile.admno = studentsummary.student inner join class on class.id = studentsummary.class where class.id = '" & Session("swapclass") & "' and studentsummary.session = '" & Session("sessionid") & "'", con)
                                        Dim schclass112s As MySql.Data.MySqlClient.MySqlDataReader = classes112s.ExecuteReader
                                        Dim studentname As New ArrayList
                                        Dim classes As New ArrayList
                                        Do While schclass112s.Read
                                            students.Add(schclass112s(0))
                                            studentname.Add(schclass112s(1))
                                            classes.Add(schclass112s(2))
                                        Loop
                                        schclass112s.Close()

                                        Dim no As Integer = 0
                                        For Each staff As String In teachingstaff
                                            logify.Notifications("There was a change in the " & ttname & " Please check your schedule.", staff, Session("staffid"), "Admin", "~/content/staff/timetable.aspx", "")
                                        Next
                                        For Each student As String In students
                                            logify.Notifications("There was a change in the " & ttname & " Please check your schedule.", student, Session("staffid"), "Admin", "~/content/student/viewtimetable.aspx?timetable=" & Request.QueryString("timetable") & "&class=" & classes(no), "")
                                            logify.Notifications("There was a change in " & studentname(no) & " time table.", par.getparent(student), Session("staffid"), "Admin", "~/content/student/viewtimetable.aspx?timetable=" & Request.QueryString("timetable") & "&class=" & classes(no), "")
                                            no = no + 1
                                        Next
                                        logify.log(Session("staffid"), ttname & " was adjusted.")

                                        Show_Alert(True, "Swap Successful.")
                                        btnCS.Visible = False
                                        Session("swap") = Nothing
                                        Session("swapday") = Nothing
                                        Session("swapperiod") = Nothing
                                        Session("swapclass") = Nothing
                                        Session("Tutorial") = Nothing
                                        cboDay.Text = Request.QueryString("day")
                                        load_time_table()
                                        con.Close()                                        Exit Sub

                                    Else
                                        Dim cmdInsert1d As New MySql.Data.MySqlClient.MySqlCommand("SELECT timetable.subject, timetable.teacher FROM timetable inner join class on timetable.class = class.id Where timetable.Day = ? And class.id = ? and timetable.period = ? and timetable.tname = ?", con)
                                        cmdInsert1d.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", Request.QueryString("day")))
                                        cmdInsert1d.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", Replace(Request.QueryString("class"), "%", " ")))
                                        cmdInsert1d.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Teacher", Request.QueryString("period")))
                                        cmdInsert1d.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Ter", Request.QueryString("timetable")))
                                        Dim reader1d As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert1d.ExecuteReader
                                        Dim thissubject As Integer
                                        Dim teacher As String
                                        Do While reader1d.Read
                                            thissubject = reader1d(0)
                                            teacher = reader1d(1)
                                        Loop
                                        reader1d.Close()
                                        Dim cmdInsert88 As New MySql.Data.MySqlClient.MySqlCommand("SELECT classsubjects.subject, staffprofile.surname FROM texemptions inner join (classsubjects inner join staffprofile on staffprofile.staffid = classsubjects.teacher) on classsubjects.teacher = texemptions.teacher Where texemptions.day = ? And Classsubjects.class = ? and classsubjects.subject = '" & thissubject & "'", con)
                                        cmdInsert88.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", Session("swapday")))
                                        cmdInsert88.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("swapclass")))
                                        Dim reader88 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert88.ExecuteReader
                                        If reader88.Read Then
                                            Show_Alert(False, "Unable to swap. " & reader88(1).ToString & " is exempted from " & Session("swapday"))
                                            reader88.Close()
                                            cboDay.Text = Request.QueryString("day")
                                            load_time_table()
                                            con.Close()                                            Exit Sub
                                        End If
                                        reader88.Close()

                                        Dim cmdInsert137 As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname FROM timetable inner join staffprofile on staffprofile.staffid = timetable.teacher Where timetable.Day = ? And timetable.Period = ? And timetable.Teacher = ? and timetable.tname = ? and class <> '" & Session("swapclass") & "'", con)
                                        cmdInsert137.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", Session("swapday")))
                                        cmdInsert137.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", Session("swapperiod")))
                                        cmdInsert137.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Teacher", teacher))
                                        cmdInsert137.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Ter", Request.QueryString("timetable")))
                                        Dim reader137 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert137.ExecuteReader
                                        If reader137.Read Then
                                            Show_Alert(False, "Unable to swap. There is a clash for " & reader137(0).ToString)
                                            reader137.Close()

                                            cboDay.Text = Request.QueryString("day")
                                            load_time_table()
                                            con.Close()                                            Exit Sub
                                        End If
                                        reader137.Close()

                                        Dim cmdInsert312 As New MySql.Data.MySqlClient.MySqlCommand("SELECT timetable.period, subjects.subject FROM timetable inner join subjects on subjects.id = timetable.subject Where timetable.Day = ? And timetable.Class = ? and timetable.subject = '" & thissubject & "' and timetable.tname = '" & Request.QueryString("timetable") & "'", con)
                                        cmdInsert312.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", Session("swapday")))
                                        cmdInsert312.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("swapclass")))
                                        Dim reader312 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert312.ExecuteReader
                                        Dim offered2 As Boolean = False
                                        Dim subject2 As String = ""
                                        Dim offperiod2 As Integer
                                        Dim offcount2 As Integer = 0
                                        Do While reader312.Read()
                                            offered2 = True
                                            offperiod2 = reader312(0).ToString
                                            subject2 = reader312(1).ToString
                                            offcount2 = offcount2 + 1
                                        Loop
                                        If offcount2 > 1 Then
                                            reader312.Close()
                                            Show_Alert(False, subject2 & " is already having a double period on " & Session("swapday"))

                                            cboDay.Text = Request.QueryString("day")
                                            load_time_table()
                                            con.Close()                                            Exit Sub
                                        End If
                                        reader312.Close()
                                        If offered2 = True And Session("swapday") <> Request.QueryString("day") Then
                                            Dim periodbeforebreaks As New ArrayList
                                            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from tperiods where day = '" & Session("swapday") & "' and timetable = '" & Request.QueryString("timetable") & "'  order by timestart", con)
                                            Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                                            Dim pb2 As Integer = 0
                                            Dim cxt2 As Integer = 0
                                            Do While reader.Read
                                                If Not reader("activity").ToString = "Tutorial" Then
                                                    periodbeforebreaks.Add(cxt2 + 1)
                                                Else
                                                    cxt2 = cxt2 + 1
                                                End If
                                            Loop
                                            reader.Close()
                                            If Session("swapperiod") <> offperiod2 + 1 Or periodbeforebreaks.Contains(Session("swapperiod")) Then
                                                Show_Alert(False, subject2 & " is already offerred on " & Session("swapday") & " and is not a double period.")

                                                cboDay.Text = Request.QueryString("day")
                                                load_time_table()
                                                con.Close()                                                Exit Sub
                                            End If
                                            cxt2 = Nothing
                                        End If

                                        offperiod2 = Nothing
                                        offcount2 = Nothing
                                        offered2 = Nothing

                                        Dim cmdInsert35 As New MySql.Data.MySqlClient.MySqlCommand("Update timetable set period = ?, day = ? where tname = ? and class = ? and period = ? and day = ?", con)
                                        cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sds", Session("swapperiod")))
                                        cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fhfss", Session("swapday")))
                                        cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ss", Request.QueryString("timetable")))
                                        cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", Session("swapclass")))
                                        cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", Request.QueryString("period")))
                                        cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", Request.QueryString("day")))
                                        cmdInsert35.ExecuteNonQuery()

                                        Dim cmdLoad10c As New MySql.Data.MySqlClient.MySqlCommand("SELECT id from tperiods where timetable = '" & Request.QueryString("timetable") & "' and day = '" & Session("swapday") & "' and activity = 'Tutorial' order by timestart", con)
                                        Dim student10c As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10c.ExecuteReader
                                        Dim cont As Integer = 1
                                        Dim tpid As Integer
                                        Do While student10c.Read
                                            If Session("swapperiod") = cont Then
                                                tpid = student10c(0)
                                            End If
                                            cont = cont + 1
                                        Loop
                                        student10c.Close()

                                        Dim cmdLoad10cs As New MySql.Data.MySqlClient.MySqlCommand("SELECT id from tperiods where timetable = '" & Request.QueryString("timetable") & "' and day = '" & Request.QueryString("day") & "' and activity = 'Tutorial' order by timestart", con)
                                        Dim student10cs As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10cs.ExecuteReader
                                        Dim cont2 As Integer = 1
                                        Dim tpid2 As Integer
                                        Do While student10cs.Read
                                            If Request.QueryString("period") = cont2 Then
                                                tpid2 = student10cs(0)
                                            End If
                                            cont2 = cont2 + 1
                                        Loop
                                        student10cs.Close()
                                        Dim cmdLoad10ct As New MySql.Data.MySqlClient.MySqlCommand("Update classperiods set period = '" & tpid2 & "' where period = '" & tpid & "'", con)
                                        cmdLoad10ct.ExecuteNonQuery()
                                        logify.log(Session("staffid"), ttname & " was adjusted.")

                                        Show_Alert(True, "Swap Successful.")
                                        btnCS.Visible = False
                                        Session("swap") = Nothing
                                        Session("swapday") = Nothing
                                        Session("swapperiod") = Nothing
                                        Session("swapclass") = Nothing
                                        Session("Tutorial") = Nothing
                                        cboDay.Text = Request.QueryString("day")
                                        load_time_table()
                                        con.Close()                                        Exit Sub
                                    End If
                                End If
                            End If
                            If Session("staffadd") = Nothing Then
                                If Request.QueryString("day") <> Nothing Then
                                    cboDay.Text = Request.QueryString("day")
                                End If



                            End If
                            load_time_table()

                            Dim cmdLoad10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT tperiods.activity  from tperiods where timetable = '" & Request.QueryString("timetable") & "' and day = '" & Request.QueryString("day") & "'order by tperiods.timestart", con)
                            Dim student10 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10.ExecuteReader
                            Dim j As Integer = 1
                            Dim period As Integer = Request.QueryString("period")
                            Do While student10.Read
                                If student10(0).ToString <> "Tutorial" And j <= period Then
                                    period = period + 1
                                End If
                                j = j + 1
                            Loop
                            student10.Close()

                            Dim cmdInsert84 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class from class where id = '" & Request.QueryString("class") & "'", con)
                            Dim reader84 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert84.ExecuteReader
                            Dim cla As String
                            reader84.Read()
                            cla = reader84(0).ToString
                            reader84.Close()


                            For Each row As GridViewRow In gridview1.Rows
                                If row.Cells(0).Text = cla Then

                                    For i As Integer = 1 To gridview1.Columns.Count - 1

                                        If i = period Then

                                            row.Cells(i).BackColor = Drawing.Color.LightBlue
                                            If Session("swap") = Nothing Then
                                                Session("swap") = "start"
                                                Session("swapperiod") = Request.QueryString("period")
                                                Session("swapperiod2") = period
                                                Session("swapday") = cboDay.Text
                                                Session("swapclass") = Request.QueryString("class")
                                                Session("tutorial") = True
                                                Show_Alert(True, "Select a subject in " + row.Cells(0).Text + " to swap with.")

                                            End If
                                        End If
                                    Next
                                End If

                            Next
                        Else
                            reader12x.Close()

                            If Session("swap") = "start" Then
                                If Session("swapclass") <> Request.QueryString("class") Then
                                    Show_Alert(False, "Please select a subject in the same class")
                                    cboDay.Text = Request.QueryString("day")
                                    load_time_table()
                                    con.Close()                                    Exit Sub
                                Else
                                    If Session("tutorial") = True Then

                                        Dim cmdInsert84s As New MySql.Data.MySqlClient.MySqlCommand("SELECT class from class where id = '" & Request.QueryString("class") & "'", con)
                                        Dim reader84s As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert84s.ExecuteReader
                                        Dim clas As String
                                        reader84s.Read()
                                        clas = reader84s(0).ToString
                                        reader84s.Close()
                                        Dim cmdLoad10s As New MySql.Data.MySqlClient.MySqlCommand("SELECT tperiods.activity  from tperiods where timetable = '" & Request.QueryString("timetable") & "' and day = '" & Request.QueryString("day") & "'order by tperiods.timestart", con)
                                        Dim student10s As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10s.ExecuteReader
                                        Dim js As Integer = 1
                                        Dim periods As Integer = Request.QueryString("period")
                                        Do While student10s.Read
                                            If student10s(0).ToString <> "Tutorial" And js <= periods Then
                                                periods = periods + 1
                                            End If
                                            js = js + 1
                                        Loop
                                        student10s.Close()

                                        Dim cmdInsert1d As New MySql.Data.MySqlClient.MySqlCommand("SELECT timetable.subject, timetable.teacher FROM timetable inner join class on timetable.class = class.id Where timetable.Day = ? And class.id = ? and timetable.period = ? and timetable.tname = ?", con)
                                        cmdInsert1d.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", Session("swapday")))
                                        cmdInsert1d.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", Session("swapclass")))
                                        cmdInsert1d.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Teacher", Session("swapperiod")))
                                        cmdInsert1d.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Ter", Request.QueryString("timetable")))
                                        Dim reader1d As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert1d.ExecuteReader
                                        Dim thissubject As Integer
                                        Dim teacher As String
                                        Do While reader1d.Read
                                            thissubject = reader1d(0)
                                            teacher = reader1d(1)
                                        Loop
                                        reader1d.Close()
                                        Dim cmdInsert88 As New MySql.Data.MySqlClient.MySqlCommand("SELECT classsubjects.subject, staffprofile.surname FROM texemptions inner join (classsubjects inner join staffprofile on staffprofile.staffid = classsubjects.teacher) on classsubjects.teacher = texemptions.teacher Where texemptions.day = ? And Classsubjects.class = ? and classsubjects.subject = '" & thissubject & "'", con)
                                        cmdInsert88.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", Request.QueryString("day")))
                                        cmdInsert88.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Replace(Request.QueryString("class"), "%", " ")))
                                        Dim reader88 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert88.ExecuteReader
                                        If reader88.Read Then
                                            Show_Alert(False, "Unable to swap. " & reader88(1).ToString & " is exempted from " & Session("swapday"))
                                            reader88.Close()
                                            cboDay.Text = Request.QueryString("day")
                                            load_time_table()
                                            con.Close()                                            Exit Sub
                                        End If
                                        reader88.Close()

                                        Dim cmdInsert137 As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname FROM timetable inner join staffprofile on staffprofile.staffid = timetable.teacher Where timetable.Day = ? And timetable.Period = ? And timetable.Teacher = ? and timetable.tname = ? and class <> '" & Session("swapclass") & "'", con)
                                        cmdInsert137.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", Request.QueryString("day")))
                                        cmdInsert137.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", Request.QueryString("period")))
                                        cmdInsert137.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Teacher", teacher))
                                        cmdInsert137.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Ter", Request.QueryString("timetable")))
                                        Dim reader137 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert137.ExecuteReader
                                        If reader137.Read Then
                                            Show_Alert(False, "Unable to swap. There is a clash for " & reader137(0).ToString)
                                            reader137.Close()

                                            cboDay.Text = Request.QueryString("day")
                                            load_time_table()
                                            con.Close()                                            Exit Sub
                                        End If
                                        reader137.Close()

                                        Dim cmdInsert312 As New MySql.Data.MySqlClient.MySqlCommand("SELECT timetable.period, subjects.subject FROM timetable inner join subjects on subjects.id = timetable.subject Where timetable.Day = ? And timetable.Class = ? and timetable.subject = '" & thissubject & "' and timetable.tname = '" & Request.QueryString("timetable") & "'", con)
                                        cmdInsert312.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", Request.QueryString("day")))
                                        cmdInsert312.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Request.QueryString("class")))
                                        Dim reader312 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert312.ExecuteReader
                                        Dim offered2 As Boolean = False
                                        Dim subject2 As String = ""
                                        Dim offperiod2 As Integer
                                        Dim offcount2 As Integer = 0
                                        Do While reader312.Read()
                                            offered2 = True
                                            offperiod2 = reader312(0).ToString
                                            subject2 = reader312(1).ToString
                                            offcount2 = offcount2 + 1
                                        Loop
                                        If offcount2 > 1 Then
                                            reader312.Close()
                                            Show_Alert(False, subject2 & " is already having a double period on " & Request.QueryString("day"))

                                            cboDay.Text = Request.QueryString("day")
                                            load_time_table()
                                            con.Close()                                            Exit Sub
                                        End If
                                        reader312.Close()
                                        If offered2 = True And Session("swapday") <> Request.QueryString("day") Then
                                            Dim periodbeforebreaks As New ArrayList
                                            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from tperiods where day = '" & Request.QueryString("day") & "' and timetable = '" & Request.QueryString("timetable") & "'  order by timestart", con)
                                            Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                                            Dim pb2 As Integer = 0
                                            Dim cxt2 As Integer = 0
                                            Do While reader.Read
                                                If Not reader("activity").ToString = "Tutorial" Then
                                                    periodbeforebreaks.Add(cxt2 + 1)
                                                Else
                                                    cxt2 = cxt2 + 1
                                                End If
                                            Loop
                                            reader.Close()
                                            If Request.QueryString("period") <> offperiod2 + 1 Or periodbeforebreaks.Contains(Request.QueryString("period")) Then
                                                Show_Alert(False, subject2 & " is already offerred on " & Request.QueryString("day") & " and is not a double period.")

                                                cboDay.Text = Request.QueryString("day")
                                                load_time_table()
                                                con.Close()                                                Exit Sub
                                            End If
                                            cxt2 = Nothing
                                        End If

                                        offperiod2 = Nothing
                                        offcount2 = Nothing
                                        offered2 = Nothing

                                        Dim cmdInsert35 As New MySql.Data.MySqlClient.MySqlCommand("Update timetable set period = ?, day = ? where tname = ? and class = ? and period = ? and day = ?", con)
                                        cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sds", Request.QueryString("period")))
                                        cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fhfss", Request.QueryString("day")))
                                        cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ss", Request.QueryString("timetable")))
                                        cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", Replace(Request.QueryString("class"), "%", " ")))
                                        cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", Session("swapperiod")))
                                        cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", Session("swapday")))
                                        cmdInsert35.ExecuteNonQuery()

                                        Dim cmdLoad10c As New MySql.Data.MySqlClient.MySqlCommand("SELECT id from tperiods where timetable = '" & Request.QueryString("timetable") & "' and day = '" & Request.QueryString("day") & "' and activity = 'Tutorial' order by timestart", con)
                                        Dim student10c As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10c.ExecuteReader
                                        Dim cont As Integer = 1
                                        Dim tpid As Integer
                                        Do While student10c.Read
                                            If Request.QueryString("period") = cont Then
                                                tpid = student10c(0)
                                            End If
                                            cont = cont + 1
                                        Loop
                                        student10c.Close()

                                        Dim cmdLoad10cs As New MySql.Data.MySqlClient.MySqlCommand("SELECT id from tperiods where timetable = '" & Request.QueryString("timetable") & "' and day = '" & Session("swapday") & "' and activity = 'Tutorial' order by timestart", con)
                                        Dim student10cs As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10cs.ExecuteReader
                                        Dim cont2 As Integer = 1
                                        Dim tpid2 As Integer
                                        Do While student10cs.Read
                                            If Session("swapperiod") = cont2 Then
                                                tpid2 = student10cs(0)
                                            End If
                                            cont2 = cont2 + 1
                                        Loop
                                        student10cs.Close()
                                        Dim cmdLoad10ct As New MySql.Data.MySqlClient.MySqlCommand("Update classperiods set period = '" & tpid2 & "' where period = '" & tpid & "'", con)
                                        cmdLoad10ct.ExecuteNonQuery()
                                        logify.log(Session("staffid"), ttname & " was adjusted.")

                                        Show_Alert(True, "Swap Successful.")
                                        btnCS.Visible = False
                                        Session("swap") = Nothing
                                        Session("swapday") = Nothing
                                        Session("swapperiod") = Nothing
                                        Session("swapclass") = Nothing
                                        Session("Tutorial") = Nothing
                                        cboDay.Text = Request.QueryString("day")
                                        load_time_table()
                                        con.Close()                                        Exit Sub





                                    Else

                                        Dim cmdLoad10c As New MySql.Data.MySqlClient.MySqlCommand("SELECT tperiods.id, classperiods.activity from tperiods left join classperiods on classperiods.period = tperiods.id where tperiods.timetable = '" & Request.QueryString("timetable") & "' and tperiods.day = '" & Request.QueryString("day") & "' and tperiods.activity = 'Tutorial' order by timestart", con)
                                        Dim student10c As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10c.ExecuteReader
                                        Dim cont As Integer = 1
                                        Dim tpid As Integer
                                        Dim activity As String
                                        Do While student10c.Read
                                            If Request.QueryString("period") = cont Then
                                                tpid = student10c(0)
                                                activity = student10c(1)
                                            End If
                                            cont = cont + 1
                                        Loop
                                        student10c.Close()

                                        Dim cmdLoad10cs As New MySql.Data.MySqlClient.MySqlCommand("SELECT tperiods.id, classperiods.activity from tperiods left join classperiods on classperiods.period = tperiods.id where tperiods.timetable = '" & Request.QueryString("timetable") & "' and tperiods.day = '" & Session("swapday") & "' and tperiods.activity = 'Tutorial' order by timestart", con)
                                        Dim student10cs As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10cs.ExecuteReader
                                        Dim cont2 As Integer = 1
                                        Dim tpid2 As Integer
                                        Dim activity2 As String
                                        Do While student10cs.Read
                                            If Session("swapperiod") = cont2 Then
                                                tpid2 = student10cs(0)
                                                activity2 = student10cs(1)
                                            End If
                                            cont2 = cont2 + 1
                                        Loop
                                        student10cs.Close()
                                        Dim cmdLoad10ct As New MySql.Data.MySqlClient.MySqlCommand("Update classperiods set activity = '" & activity2 & "' where period = '" & tpid & "'", con)
                                        cmdLoad10ct.ExecuteNonQuery()

                                        Dim cmdLoad10cst As New MySql.Data.MySqlClient.MySqlCommand("Update classperiods set activity = '" & activity & "' where period = '" & tpid2 & "'", con)
                                        cmdLoad10cst.ExecuteNonQuery()
                                        logify.log(Session("staffid"), ttname & " was adjusted.")

                                        Show_Alert(True, "Swap Successful.")
                                        btnCS.Visible = False
                                        Session("swap") = Nothing
                                        Session("swapday") = Nothing
                                        Session("swapperiod") = Nothing
                                        Session("swapclass") = Nothing
                                        Session("Tutorial") = Nothing
                                        cboDay.Text = Request.QueryString("day")
                                        load_time_table()
                                        con.Close()                                        Exit Sub





                                    End If
                                End If

                            Else
                                cboDay.Text = Request.QueryString("day")
                                load_time_table()
                                Dim cmdInsert84 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class from class where id = '" & Request.QueryString("class") & "'", con)
                                Dim reader84 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert84.ExecuteReader
                                Dim cla As String
                                reader84.Read()
                                cla = reader84(0).ToString
                                reader84.Close()
                                Dim cmdLoad10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT tperiods.activity  from tperiods where timetable = '" & Request.QueryString("timetable") & "' and day = '" & Request.QueryString("day") & "'order by tperiods.timestart", con)
                                Dim student10 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10.ExecuteReader
                                Dim j As Integer = 1
                                Dim period As Integer = Request.QueryString("period")
                                Do While student10.Read
                                    If student10(0).ToString <> "Tutorial" And j <= period Then
                                        period = period + 1
                                    End If
                                    j = j + 1
                                Loop
                                student10.Close()
                                For Each row As GridViewRow In gridview1.Rows
                                    If row.Cells(0).Text = cla Then

                                        For i As Integer = 1 To gridview1.Columns.Count - 1

                                            If i = period Then

                                                row.Cells(i).BackColor = Drawing.Color.LightBlue
                                                If Session("swap") = Nothing Then
                                                    Session("swap") = "start"
                                                    Session("swapperiod") = Request.QueryString("period")
                                                    Session("swapperiod2") = period
                                                    Session("swapday") = cboDay.Text
                                                    Session("swapclass") = Request.QueryString("class")
                                                    Session("tutorial") = False
                                                    Show_Alert(True, "Select a subject in " + row.Cells(0).Text + " to swap with.")

                                                End If
                                            End If
                                        Next
                                    End If

                                Next
                            End If
                        End If

                    Else
                        Session("swap") = Nothing
                        Session("swapday") = Nothing
                        Session("swapperiod") = Nothing
                        Session("swapperiod2") = Nothing
                        Session("swapclass") = Nothing
                        Session("tutorial") = Nothing
                    End If
                    con.Close()
                End Using

            End If

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Private Sub Staff_Details()

        For Each row As GridViewRow In gridview2.Rows
            Dim x As Array = Split(row.Cells(1).Text, " - ")
            If x(0) = Session("staffadd") Then
                gridview1.SelectedIndex = row.RowIndex
            End If
        Next

        For Each row As GridViewRow In gridview1.Rows
            For i = 1 To gridview1.Columns.Count - 1
                If Not row.Cells(i).BackColor = Drawing.Color.LightBlue Then
                    row.Cells(i).BackColor = Drawing.Color.Empty
                Else
                    Continue For
                End If
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.open()
                    Dim cmdLoad10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT tperiods.activity  from tperiods where timetable = '" & Request.QueryString("timetable") & "' and day = '" & cboDay.Text & "'order by tperiods.timestart", con)
                    Dim student10 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10.ExecuteReader
                    Dim period As Integer = 1
                    Do While student10.Read
                        If student10(0).ToString <> "Tutorial" And i = period Then
                           
                Continue For
                        End If
                period = period + 1
                    Loop
            student10.Close()
          
            Dim grouped As Boolean = False
            If TryCast(row.Cells(i).Controls(0), HyperLink).Text <> "" Then
                Dim test As Array = Split(TryCast(row.Cells(i).Controls(0), HyperLink).Text)

                For Each s As String In test
                    If Trim(s) = "/" Then
                        grouped = True
                    End If
                Next
                If grouped = True Then
                    Dim first As Array = Split(TryCast(row.Cells(i).Controls(0), HyperLink).Text, " / ")
                    For Each item As String In first
                        Dim second As Array = Split(item, " - ")
                        If second(1) = Session("staffadd") Then
                            row.Cells(i).BackColor = Drawing.Color.Maroon
                        End If
                        second = Nothing

                    Next
                    first = Nothing
                Else
                    Dim second As Array = Split(TryCast(row.Cells(i).Controls(0), HyperLink).Text, " - ")
                            If second.Length = 2 Then
                                If second(1) = Session("staffadd") Then
                                    row.Cells(i).BackColor = Drawing.Color.Maroon
                                End If
                            End If
                            second = Nothing
                End If
                test = Nothing
            End If


                    grouped = Nothing
                    con.Close()
                End Using
        Next
        Next
    End Sub
    Protected Sub cboDay_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDay.SelectedIndexChanged
        Try
            load_time_table()
            pnltt.Visible = True
            If Session("Staffadd") <> Nothing Then
                For Each row As GridViewRow In gridview1.Rows
                    For i = 1 To gridview1.Columns.Count - 1
                        If Not row.Cells(i).BackColor = Drawing.Color.LightBlue Then
                            row.Cells(i).BackColor = Drawing.Color.Empty
                        Else
                            Continue For
                        End If
                        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                            con.open()
                            Dim cmdLoad10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT tperiods.activity  from tperiods where timetable = '" & Request.QueryString("timetable") & "' and day = '" & cboDay.Text & "'order by tperiods.timestart", con)
                            Dim student10 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10.ExecuteReader
                            Dim period As Integer = 1
                            Do While student10.Read
                                If student10(0).ToString <> "Tutorial" And i = period Then
                                   
                        Continue For
                                End If
            period = period + 1
                            Loop
        student10.Close()
       
        Dim grouped As Boolean = False
        If TryCast(row.Cells(i).Controls(0), HyperLink).Text <> "" Then
            Dim test As Array = Split(TryCast(row.Cells(i).Controls(0), HyperLink).Text)

            For Each s As String In test
                If Trim(s) = "/" Then
                    grouped = True
                End If
            Next
            If grouped = True Then
                Dim first As Array = Split(TryCast(row.Cells(i).Controls(0), HyperLink).Text, " / ")
                For Each item As String In first
                    Dim second As Array = Split(item, " - ")
                    If second(1) = Session("staffadd") Then
                        row.Cells(i).BackColor = Drawing.Color.Maroon
                    End If
                    second = Nothing

                Next
                first = Nothing
            Else
                Dim second As Array = Split(TryCast(row.Cells(i).Controls(0), HyperLink).Text, " - ")
                If second(1) = Session("staffadd") Then
                    row.Cells(i).BackColor = Drawing.Color.Maroon

                End If
                second = Nothing
            End If
            test = Nothing
        End If


                            grouped = Nothing
                            con.Close()
                        End Using
                    Next
                Next
        For Each row As GridViewRow In gridview2.Rows
            Dim x As Array = Split(row.Cells(1).Text, " - ")
            If x(0) = Session("staffadd") Then
                gridview2.SelectedIndex = row.RowIndex
            End If
        Next
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Private Sub load_time_table()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            pnltt.Visible = True
            Dim cla As String
            If Session("swap") <> Nothing Then
                Dim cmdInsert84 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class from class where id = '" & Session("swapclass") & "'", con)
                Dim reader84 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert84.ExecuteReader

                reader84.Read()
                cla = reader84(0).ToString
                reader84.Close()
            End If
            gridview1.AutoGenerateColumns = False
            gridview1.CssClass = "table"
            Dim ds As New DataTable
            ds.Columns.Clear()
            ds.Rows.Clear()
            ds.Columns.Add("Class")
            Dim clas As New BoundField
            clas.HeaderText = "Class"
            clas.DataField = "Class"
            gridview1.Columns.Add(clas)

            If cboDay.Text <> "Select Day" Then
                Dim classes As New ArrayList
                If Request.QueryString("school") <> 0 Then
                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.class from class inner join ttname on ttname.school = class.superior where ttname.id = '" & Request.QueryString("timetable") & "' order by class.class", con)
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                    Do While student0.Read
                        ds.Rows.Add(student0(0).ToString)
                        classes.Add(student0(0).ToString)
                    Loop
                    student0.Close()
                Else
                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.class from class inner join ttname on ttname.class = class.id where ttname.id = '" & Request.QueryString("timetable") & "' order by class.class", con)
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                    Do While student0.Read
                        ds.Rows.Add(student0(0).ToString)
                        classes.Add(student0(0).ToString)
                    Loop
                    student0.Close()
                End If
                Dim cmdLoad10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT tperiods.timestart, tperiods.timeend, tperiods.activity  from tperiods where timetable = '" & Request.QueryString("timetable") & "' and day = '" & cboDay.Text & "'order by tperiods.timestart", con)
                Dim student10 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10.ExecuteReader
                Dim j As Integer = 1
                Dim nontutorial As New ArrayList
                Do While student10.Read

                    If student10(2).ToString = "Tutorial" Then
                        ds.Columns.Add(student10(0).ToString & " - " & student10(1).ToString)
                        Dim hype As New HyperLinkField
                        Dim array() As String = {"edit" & j, "edit" & j + 1}
                        hype.DataTextField = student10(0).ToString & " - " & student10(1).ToString
                        ds.Columns.Add("edit" & j)
                        ds.Columns.Add("edit" & j + 1)
                        hype.DataNavigateUrlFormatString = "~/content/App/Admin/viewtimetable.aspx?timetable=" + Request.QueryString("timetable") + "&school=" + Request.QueryString("school") + "&day=" + cboDay.Text + "&period={0}&class={1}"
                        hype.DataNavigateUrlFields = array
                        hype.HeaderText = student10(0).ToString & " - " & student10(1).ToString
                        gridview1.Columns.Add(hype)

                        j = j + 3
                    Else
                        ds.Columns.Add(student10(0).ToString & " - " & student10(1).ToString)
                        Dim nonteaching As New BoundField
                        nonteaching.HeaderText = student10(0).ToString & " - " & student10(1).ToString
                        nonteaching.DataField = student10(0).ToString & " - " & student10(1).ToString
                        gridview1.Columns.Add(nonteaching)
                        For Each row As DataRow In ds.Rows

                            row.Item(j) = student10(2).ToString
                        Next
                        nontutorial.Add(j)
                        j = j + 1
                    End If
                Loop
                student10.Close()
                Dim c As Integer = 0
                For Each item As String In classes
                    Dim enterred As New ArrayList
                    Dim cmdLo As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.alias, timetable.teacher, timetable.period, timetable.class from timetable inner join class on timetable.class = class.id inner join subjects on timetable.subject =   subjects.id where class.class = '" & item & "' and timetable.day = '" & cboDay.Text & "' and timetable.tname = '" & Request.QueryString("timetable") & "' order by timetable.period", con)
                    Dim studen As MySql.Data.MySqlClient.MySqlDataReader = cmdLo.ExecuteReader
                    Dim hx As Integer = 1
                    Do While studen.Read
                        Dim nonotlink As Integer = 0
                        For Each vb As Integer In nontutorial
                            If vb <= hx Then
                                nonotlink = nonotlink + 1
                            End If
                        Next
                        If ((studen(2) * 3) + nonotlink) - 2 <> hx And Not nontutorial.Contains(hx) And Not enterred.Contains(studen(2).ToString) Then
                            hx = hx + 3
                        End If
                        If Not nontutorial.Contains(hx) Then
                            If Not enterred.Contains(studen(2).ToString) Then
                                ds(c).Item(hx) = studen(0).ToString & " - " & studen(1).ToString
                                ds(c).Item(hx + 1) = studen(2).ToString
                                ds(c).Item(hx + 2) = studen(3).ToString
                                enterred.Add(studen(2).ToString)
                                hx = hx + 3
                            Else
                                ds(c).Item(hx - 3) = ds(c).Item(hx - 3) & " / " & studen(0).ToString & " - " & studen(1).ToString
                            End If
                        Else

                            If Not enterred.Contains(studen(2).ToString) Then
                                hx = hx + 1
                                nonotlink = 0
                                For Each vb As Integer In nontutorial
                                    If vb <= hx Then
                                        nonotlink = nonotlink + 1
                                    End If
                                Next
                                If ((studen(2) * 3) + nonotlink) - 2 <> hx Then hx = hx + 3
                                ds(c).Item(hx) = studen(0).ToString & " - " & studen(1).ToString
                                ds(c).Item(hx + 1) = studen(2).ToString
                                ds(c).Item(hx + 2) = studen(3).ToString
                                enterred.Add(studen(2).ToString)
                                hx = hx + 3
                            Else
                                ds(c).Item(hx - 3) = ds(c).Item(hx - 3) & " / " & studen(0).ToString & " - " & studen(1).ToString
                            End If

                        End If

                        nonotlink = Nothing
                    Loop
                    studen.Close()
                    c = c + 1
                    hx = Nothing
                    enterred = Nothing
                Next
                c = Nothing
                For Each item As String In classes

                    Dim cmdLoad10z As New MySql.Data.MySqlClient.MySqlCommand("SELECT classperiods.period, classperiods.activity, tperiods.timestart, tperiods.timeend, classperiods.class from tperiods inner join (classperiods inner join class on class.id = classperiods.class) on classperiods.period = tperiods.id where tperiods.timetable = '" & Request.QueryString("timetable") & "' and class.class = '" & item & "' and tperiods.day = '" & cboDay.Text & "'", con)
                    Dim student10z As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10z.ExecuteReader
                    Do While student10z.Read


                        For h = 1 To ds.Columns.Count - 1
                            Dim nonotlink As Integer = 0
                            If ds.Columns(h).ColumnName = student10z(2).ToString & " - " & student10z(3).ToString Then
                                For Each vb As Integer In nontutorial
                                    If vb <= h Then
                                        nonotlink = nonotlink + 1
                                    End If
                                Next
                                ds(c).Item(h) = student10z(1).ToString
                                ds(c).Item(h + 1) = ((h - nonotlink + 2) / 3)
                                ds(c).Item(h + 2) = student10z(4).ToString
                            End If
                            nonotlink = Nothing
                        Next

                    Loop

                    student10z.Close()

                    c = c + 1
                Next
                gridview1.DataSource = ds
                gridview1.DataBind()
                Panel1.Controls.Add(gridview1)

                Staff_Details()
                If Session("swap") = Nothing Then
                    For Each row As GridViewRow In gridview1.Rows
                        For i = 1 To gridview1.Columns.Count - 1
                            If row.Cells(i).BackColor = Drawing.Color.LightBlue Then
                                row.Cells(i).BackColor = Drawing.Color.Empty
                            End If
                        Next
                    Next
                Else
                    If cboDay.Text <> Session("swapday") Then
                        For Each row As GridViewRow In gridview1.Rows
                            For i = 1 To gridview1.Columns.Count - 1
                                If row.Cells(i).BackColor = Drawing.Color.LightBlue Then
                                    row.Cells(i).BackColor = Drawing.Color.Empty
                                End If
                            Next
                        Next
                    Else
                        For Each row As GridViewRow In gridview1.Rows
                            If row.Cells(0).Text = cla Then
                                For i As Integer = 1 To gridview1.Columns.Count - 1
                                    If i = Session("swapperiod2") Then
                                        row.Cells(i).BackColor = Drawing.Color.LightBlue
                                    End If
                                Next
                            End If
                        Next

                    End If
                End If
            End If

            con.Close()
        End Using
    End Sub

    Protected Sub gridview2_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles gridview2.SelectedIndexChanging
        Try
            Dim x As Array = Split(gridview2.Rows(e.NewSelectedIndex).Cells(1).Text, " - ")
            Session("staffadd") = RTrim(x(0))
            load_time_table()
            pnlAll.Visible = False
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnChange_Click(sender As Object, e As EventArgs) Handles btnChange.Click
        Try
            Dim status As Integer = 0
            If lblStatus.Text = "Not In Use" Then status = 1
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLo As New MySql.Data.MySqlClient.MySqlCommand("Update ttname set ttname.default = '" & status & "' where ttname.id = '" & Request.QueryString("timetable") & "'", con)
                cmdLo.ExecuteNonQuery()
                If Request.QueryString("school") = 0 Then
                    Dim cmdLow As New MySql.Data.MySqlClient.MySqlCommand("Update class set timetable = '" & Request.QueryString("id") & "' where id = '" & Request.QueryString("class") & "'", con)
                    cmdLow.ExecuteNonQuery()
                End If
                Dim cmdInsert137a As New MySql.Data.MySqlClient.MySqlCommand("SELECT ttname.name, depts.dept, ttname.default, class.class, ttname.class, ttname.school from ttname left join depts on depts.id = ttname.school left join class on ttname.class = class.id where ttname.id = '" & Request.QueryString("timetable") & "'", con)
                Dim student0d As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert137a.ExecuteReader
                Dim ttname As String
                student0d.Read()
                If student0d(5) <> 0 Then
                    lblTimetable.Text = student0d(0).ToString & " - " & student0d(1).ToString
                    ttname = student0d(0)
                    If student0d(2) = 1 Then
                        lblStatus.Text = "In Use"
                        lblStatus.ForeColor = Drawing.Color.Green
                    Else
                        lblStatus.Text = "Not In Use"
                        lblStatus.ForeColor = Drawing.Color.Red
                    End If
                Else
                    lblTimetable.Text = student0d(0).ToString & " - " & student0d(3).ToString
                    ttname = student0d(0)
                    If student0d(2) = 1 Then
                        lblStatus.Text = "In Use"
                        lblStatus.ForeColor = Drawing.Color.Green
                    Else
                        lblStatus.Text = "Not In Use"
                        lblStatus.ForeColor = Drawing.Color.Red
                    End If
                End If
                student0d.Close()

                Dim teachingstaff As New ArrayList
                Dim classes112 As New MySql.Data.MySqlClient.MySqlCommand("Select classsubjects.teacher from classsubjects inner join class on class.id = classsubjects.class where class.superior = '" & Request.QueryString("school") & "' or class.id = '" & Request.QueryString("class") & "'", con)
                Dim schclass112 As MySql.Data.MySqlClient.MySqlDataReader = classes112.ExecuteReader
                Do While schclass112.Read
                    If Not teachingstaff.Contains(schclass112(0)) Then
                        teachingstaff.Add(schclass112(0))
                    End If
                Loop
                schclass112.Close()
                Dim students As New ArrayList
                Dim classes112s As New MySql.Data.MySqlClient.MySqlCommand("Select studentsummary.student, studentsprofile.surname, class.class from studentsummary inner join studentsprofile on studentsprofile.admno = studentsummary.student inner join class on class.id = studentsummary.class where class.superior = '" & Request.QueryString("school") & "' or class.id = '" & Request.QueryString("class") & "' and studentsummary.session = '" & Session("sessionid") & "'", con)
                Dim schclass112s As MySql.Data.MySqlClient.MySqlDataReader = classes112s.ExecuteReader
                Dim studentname As New ArrayList
                Dim classes As New ArrayList
                Do While schclass112s.Read
                    students.Add(schclass112s(0))
                    studentname.Add(schclass112s(1))
                    classes.Add(schclass112s(2))
                Loop
                schclass112s.Close()
                If status = 1 Then
                    Dim no As Integer = 0
                    For Each staff As String In teachingstaff
                        logify.Notifications(ttname & " has been published for use. Please check your schedule", staff, Session("staffid"), "Admin", "~/content/staff/timetable.aspx", "")
                    Next
                    For Each student As String In students
                        logify.Notifications(ttname & " has been published for use. Please check your schedule", student, Session("staffid"), "Admin", "~/content/student/viewtimetable.aspx?timetable=" & Request.QueryString("timetable") & "&class=" & classes(no), "")
                        logify.Notifications(ttname & " for " & studentname(no) & " has been published for use.", par.getparent(student), Session("staffid"), "Admin", "~/content/student/viewtimetable.aspx?timetable=" & Request.QueryString("timetable") & "&class=" & classes(no), "")
                        no = no + 1
                    Next
                    logify.log(Session("staffid"), ttname & " was published for use")
                Else
                    Dim no As Integer = 0
                    For Each staff As String In teachingstaff
                        logify.Notifications(ttname & " is no more in use. A  new one will be published soon.", staff, Session("staffid"), "Admin", "~/content/staff/timetable.aspx", "")
                    Next
                    For Each student As String In students
                        logify.Notifications(ttname & " is no more in use. A  new one will be published soon.", student, Session("staffid"), "Admin", "~/content/student/viewtimetable.aspx?timetable=" & Request.QueryString("timetable") & "&class=" & classes(no), "")
                        logify.Notifications(ttname & " for " & studentname(no) & " is no more in use. A  new one will be published soon.", par.getparent(student), Session("staffid"), "Admin", "~/content/student/viewtimetable.aspx?timetable=" & Request.QueryString("timetable") & "&class=" & classes(no), "")
                        no = no + 1
                    Next
                    logify.log(Session("staffid"), ttname & " was unpublished.")
                End If
                con.close()            End Using
            load_time_table()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        Response.Redirect("~/content/App/Admin/managetimetable.aspx")
    End Sub

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        pnlAll.Visible = True
        load_time_table()
    End Sub

    Protected Sub btnCS_Click(sender As Object, e As EventArgs) Handles btnCS.Click
        Session("swap") = Nothing
        Session("swapday") = Nothing
        Session("swapperiod") = Nothing
        Session("swapclass") = Nothing
        btnCS.Visible = False
        cboDay.Text = Request.QueryString("day")
        load_time_table()
    End Sub
End Class
