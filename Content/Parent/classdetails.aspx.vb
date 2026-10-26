Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Account_Default
    Inherits System.Web.UI.Page
    Dim optionalfeeb As String
    Dim optionalfeef As String
    Dim optionalfeet As String
    Dim optionalfeec As New ArrayList
    Dim optionalfeenc As New ArrayList
    Dim hostel As Boolean
    Dim transport As String
    Dim feeding As String
    Dim minimum As Double
    Dim i As Integer

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
        If check.Check_Parent(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try

       
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                panel3.Visible = False
                logify.Read_notification("~/content/parent/classdetails.aspx?" & Request.QueryString.ToString, Session("parentid"))

                Dim ds As New DataTable
                ds.Columns.Add("passport")
                ds.Columns.Add("staffname")
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.passport, studentsprofile.admno, studentsprofile.surname from parentward inner join studentsprofile on parentward.ward = studentsprofile.admno WHERE parentward.parent = ?", con)
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Session("parentID")))
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
                Do While student0.Read
                    ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                Loop
                student0.Close()
                gridview1.DataSource = ds
                gridview1.DataBind()
                con.close()            End Using
            If Request.QueryString.ToString <> Nothing Then
                Session("studentadd") = Request.QueryString.ToString
            End If
            If Session("studentadd") <> Nothing Then
                Student_Details()
            End If

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub





    Private Sub Student_Details()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Session("studentid") = Session("studentAdd")
            Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.surname, class.class, studentsummary.trans, studentsummary.status, studentsummary.class From Studentsummary INNER JOIN Studentsprofile on studentsummary.student = studentsprofile.admno inner join class on studentsummary.class = class.Id WHERE StudentSummary.student = ? And StudentSummary.Session = ?", con)
            cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class.ID", Session("Studentadd")))
            cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter(" StudentSummary.Session ", Session("SessionID")))
            Dim studentsReader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader()
            studentsReader.Read()
            lblClass.Text = studentsReader.Item(0).ToString & "'S CLASS"
            Session("ClassId") = studentsReader.Item(4)

            studentsReader.Close()
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.class as Class, studentsummary.classNo as 'No of Students', class.type as 'Type', depts.dept as 'Department' from class inner join depts on class.superior = depts.id left join studentsummary on class.id = studentsummary.class where class.id = '" & Session("ClassId") & "' And StudentSummary.Session = '" & Session("sessionid") & "' order by studentsummary.session desc", con)
            Dim ds1 As New DataTable
            Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
            adapter1.SelectCommand = cmdLoad1
            adapter1.Fill(ds1)
            DetailsView1.DataSource = ds1
            DetailsView1.DataBind()
            Dim cmdLoads As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname, staffprofile.phone, staffprofile.passport from classteacher inner join staffprofile on classteacher.teacher = staffprofile.staffid where classteacher.class = '" & Session("ClassId") & "'", con)
            Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoads.ExecuteReader
            Dim ds2 As New DataTable
            ds2.Columns.Add("s/n")
            ds2.Columns.Add("Name")
            ds2.Columns.Add("phone")
            ds2.Columns.Add("passport")

            Dim i As Integer = 1
            Do While reader.Read
                ds2.Rows.Add(i.ToString & ".  ", reader.Item(0), reader.Item(1), reader.Item(2))
                i = i + 1
            Loop
            GridView2.DataSource = ds2
            GridView2.DataBind()
            reader.Close()

            Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.staffid, staffprofile.surname, subjects.subject, staffprofile.phone, staffprofile.passport from subjectreg inner join studentsprofile on studentsprofile.admno = subjectreg.student inner join (classsubjects left join staffprofile on staffprofile.staffid = classsubjects.teacher) on subjectreg.subjectsofferred = classsubjects.subject and subjectreg.class = classsubjects.class inner join subjects on subjects.id = subjectreg.subjectsofferred where subjectreg.student = '" & Session("StudentId") & "' and subjectreg.session = '" & Session("SessionId") & "'", con)
            Dim reader0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
            Dim ds3 As New DataTable
            ds3.Columns.Add("s/n")
            ds3.Columns.Add("Name")
            ds3.Columns.Add("Subject")
            ds3.Columns.Add("phone")
            ds3.Columns.Add("passport")

            Dim j As Integer = 1
            Do While reader0.Read
                ds3.Rows.Add(j.ToString & ".    ", reader0.Item(1), reader0.Item(2), reader0.Item(3), reader0.Item(4))
                j = j + 1
            Loop
            reader0.Close()
            Dim cmdLoad0s As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.staffid, staffprofile.surname, subjects.subject, staffprofile.phone, staffprofile.passport from classsubjects left join staffprofile on staffprofile.staffid = classsubjects.teacher inner join subjects on subjects.id = classsubjects.subject where classsubjects.class = '" & Session("ClassId") & "'  and classsubjects.subjectnest <> '" & 0 & "'", con)
            Dim reader0s As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0s.ExecuteReader
            Do While reader0s.Read
                ds3.Rows.Add(j.ToString & ".    ", reader0s.Item(1), reader0s.Item(2), reader0s.Item(3), reader0s.Item(4))
                j = j + 1
            Loop
            reader0s.Close()
            GridView3.DataSource = ds3
            GridView3.DataBind()

            panel3.Visible = True
            pnlAll.Visible = False
            gridview1.SelectedIndex = -1
            con.close()        End Using
    End Sub

    Protected Sub gridview1_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles gridview1.SelectedIndexChanging
        Try
            Dim x As Array = Split(gridview1.Rows(e.NewSelectedIndex).Cells(1).Text, " - ")
            Session("studentadd") = RTrim(x(0))
            pnlAll.Visible = False
            panel3.Visible = True
            gridview1.SelectedIndex = -1
            Student_Details()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub lnkTimeTable_Click(sender As Object, e As EventArgs) Handles lnkTimeTable.Click
        Try
            Dim id As Integer
            Dim exists As Boolean = True
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()

                Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT ttname.id, class.class from ttname inner join class on class.id = ttname.class where ttname.default = '" & 1 & "' and class.id = ?", con)
                cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("classid")))
                Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                If student.Read() Then
                    Session("classselect") = student(1).ToString
                    id = student(0)
                    student.Close()
                Else
                    student.Close()
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ttname.id, class.class from ttname inner join (depts inner join class on class.superior = depts.id) on ttname.school = depts.id where ttname.default = '" & 1 & "' and class.id = ?", con)
                    cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("classid")))
                    Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    If student1.Read() Then
                        Session("classselect") = student1(1).ToString
                        id = student1(0)
                        student1.Close()
                    Else
                        exists = False
                    End If
                End If
                con.Close()
            End Using
            If exists = True Then
                Response.Redirect("~/content/student/viewtimetable.aspx?timetable=" & id)

            Else
                Show_Alert(False, "Time Table not Active yet")
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        pnlAll.Visible = True
        panel3.Visible = False
        gridview1.SelectedIndex = -1
    End Sub
End Class
