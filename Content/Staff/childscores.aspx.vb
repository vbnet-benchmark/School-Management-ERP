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
    Sub Check_Strange()
        Dim db As New DB_Interface
        Dim a As ArrayList = db.Select_1D("select subjectsofferred from subjectreg where session = '" & Session("sessionid") & "'")
        Dim s As ArrayList = db.Select_1D("select id from subjects")
        Dim b As New ArrayList
        For Each j In a
            If Not b.Contains(j) Then b.Add(j)
        Next
        Dim jk As String
        For Each x In b
            If Not s.Contains(x) Then
                jk = jk & "," & x
            End If
        Next
        Show_Alert(True, jk)
    End Sub
   Sub Compute_Average()
        Dim studentID As String
        Dim rrr As Integer
        Dim db As New DB_Interface
        Dim classes As ArrayList = db.Select_1D("SELECT class.id from class where type <> 'Early Years' and id <> 8 and id <> 9")

        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            For Each cl In classes
                Try
                    Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM SubjectReg WHERE Student = ? And Session = ? And Class = ?", con)
                    Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM StudentSummary WHERE Class = ? And Session = ?", con)
                    cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cl))
                    cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("sessionid")))
                    Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmd3.ExecuteReader

                    Dim i As Integer = 0
                    Dim students As New ArrayList
                    Do While reader3.Read
                        students.Add(reader3.Item("student"))
                    Loop
                    reader3.Close()
                    Dim gtotals As New ArrayList
                    Dim ct As Integer = 0
                    For Each item As String In students
                        studentID = item
                        Dim param As MySql.Data.MySqlClient.MySqlParameter = cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", studentID))
                        Dim param4 As MySql.Data.MySqlClient.MySqlParameter = cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Sessionid")))
                        Dim param5 As MySql.Data.MySqlClient.MySqlParameter = cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cl))
                        Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader()
                        Dim total As Double = 0
                        Dim average As Double
                        Dim count As Integer
                        Do While reader.Read()
                            count = count + 1
                            total = total + Val(reader.Item("Total"))
                        Loop

                        gtotals.Add(total)
                        reader.Close()
                        average = total / count
                        average = FormatNumber(average, 2)


                        Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("UPDATE StudentSummary SET Average = ? WHERE student = ? And Session = ? And Class = ?", con)
                        Dim param2 As MySql.Data.MySqlClient.MySqlParameter = cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Average", average))
                        Dim param3 As MySql.Data.MySqlClient.MySqlParameter = cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", studentID))
                        Dim param6 As MySql.Data.MySqlClient.MySqlParameter = cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Sessionid")))
                        Dim param7 As MySql.Data.MySqlClient.MySqlParameter = cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cl))

                        cmd2.ExecuteNonQuery()
                        cmd.Parameters.Remove(param)
                        cmd2.Parameters.Remove(param2)
                        cmd2.Parameters.Remove(param3)
                        cmd.Parameters.Remove(param4)
                        cmd.Parameters.Remove(param5)
                        cmd2.Parameters.Remove(param6)
                        cmd2.Parameters.Remove(param7)
                        total = Nothing
                        count = Nothing
                        ct = ct + 1
                    Next
                    rrr = ct
                    gtotals.Sort()
                    
                    Dim cmd2x As New MySql.Data.MySqlClient.MySqlCommand("UPDATE StudentSummary SET classhigh = '" & gtotals(ct - 1) & "', classlow = '" & gtotals(0) & "' where Session = ? And Class = ?", con)
                    Dim param6aaa As MySql.Data.MySqlClient.MySqlParameter = cmd2x.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Sessionid")))
                    Dim param7aa As MySql.Data.MySqlClient.MySqlParameter = cmd2x.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cl))
                    cmd2x.ExecuteNonQuery()
                Catch ex As Exception
                    Show_Alert(False, ex.Message & " - " & cl & " avg")
                    Exit Sub
                End Try
            Next

            con.Close()
        End Using


    End Sub



    Sub Class_Position()
      
        Dim db As New DB_Interface
        Dim classes As ArrayList = db.Select_1D("SELECT class.id from class where type <> 'Early Years'")
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            For Each cl In classes
                Dim positionNo As Integer
                Dim position As String
                Dim highest As Integer
                Dim lowest As Integer
                Try
                    Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT Average, student from StudentSummary WHERE Session = ? And Class = ? ORDER BY Average DESC", con)
                    Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("UPDATE StudentSummary SET Position = ? WHERE student = ? And Session = ? And Class = ?", con)
                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("sessionid")))
                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cl))
                    Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader
                    Dim i As Integer = 0
                    Dim students As New ArrayList
                    Do While reader.Read
                        students.Add(reader.Item("student"))
                    Loop
                    reader.Close()
                    For Each item As String In students
                        positionNo = positionNo + 1
                        Select Case positionNo
                            Case 1, 21, 31, 41, 51, 61, 71, 81, 91, 101
                                If positionNo = 1 Then
                                End If
                                position = positionNo.ToString + "st"
                            Case 2, 22, 32, 42, 52, 62, 72, 82, 92, 102
                                position = positionNo.ToString + "nd"
                            Case 3, 23, 33, 43, 53, 63, 73, 83, 93, 103
                                position = positionNo.ToString + "rd"
                            Case Else
                                position = positionNo.ToString + "th"
                        End Select
                        Dim param As MySql.Data.MySqlClient.MySqlParameter = cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Position", position))
                        Dim param2 As MySql.Data.MySqlClient.MySqlParameter = cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", item))
                        Dim param3 As MySql.Data.MySqlClient.MySqlParameter = cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("sessionid")))
                        Dim param4 As MySql.Data.MySqlClient.MySqlParameter = cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cl))
                        cmd2.ExecuteNonQuery()
                        cmd2.Parameters.Remove(param)
                        cmd2.Parameters.Remove(param2)
                        cmd2.Parameters.Remove(param3)
                        cmd2.Parameters.Remove(param4)

                    Next
                    position = ""
                    positionNo = 0
                Catch ex As Exception
                    Show_Alert(False, ex.Message & " - " & cl & " pos")
                    Exit Sub
                End Try
            Next
            con.Close()
        End Using

    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
       
        If check.Check_Class(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try
            Dim type As String
            If Not IsPostBack Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.Open()
                    Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.class from classteacher inner join class on class.id = classteacher.class where classteacher .teacher = ?", con)
                    cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("staffid")))
                    Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader

                    Do While student.Read
                        DropDownList1.Items.Add(student.Item(0).ToString)
                    Loop
                    panel3.Visible = False
                    student.Close()

                    Dim ds As New DataTable
                    ds.Columns.Add("passport")
                    ds.Columns.Add("staffname")
                    Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.passport, studentsprofile.admno, studentsprofile.surname from studentsummary inner join studentsprofile on studentsummary.student = studentsprofile.admno inner join class on studentsummary.class = class.id WHERE class.class = ? and studentsummary.session = '" & Session("sessionid") & "'", con)
                    cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", DropDownList1.Text))
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
                    Do While student0.Read
                        ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                    Loop
                    student0.Close()
                    Dim cmd21 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.type, class.id from class where class.class = ?", con)
                    cmd21.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", DropDownList1.Text))
                    Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd21.ExecuteReader()
                    reader2.Read()
                    type = reader2(0)
                    Session("rsClass") = reader2(1)
                    reader2.Close()
                    gridview1.DataSource = ds
                    gridview1.DataBind()
                    con.Close()                End Using
                If Request.QueryString.ToString <> Nothing Then
                    Session("studentadd") = Request.QueryString.ToString
                End If
                If Session("studentadd") <> Nothing And type <> "Early Years" Then
                    Student_Details()
                End If
            End If


        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try

    End Sub





    Private Sub Student_Details()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport from StudentsProfile where admno = ?", con)
            cmdLoad4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("studentadd")))
            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
            Dim pass As String = ""
            If student.Read() Then pass = student.Item("passport").ToString
            student.Close()
            If pass = "" Then
                pass = "~/image/noimage.jpg"
            End If
            Image1.ImageUrl = pass
            Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.surname, class.class, studentsummary.trans, studentsummary.status, studentsummary.class, class.type, class.cano From Studentsummary INNER JOIN Studentsprofile on studentsummary.student = studentsprofile.admno inner join class on studentsummary.class = class.Id WHERE StudentSummary.student = ? And StudentSummary.Session = ?", con)
            cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class.ID", Session("Studentadd")))
            cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter(" StudentSummary.Session ", Session("SessionID")))
            Dim studentsReader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader()
            studentsReader.Read()
            lblClass.Text = studentsReader.Item(0).ToString
            Session("ClassId") = studentsReader.Item(4)
            Dim cano As Integer = studentsReader.Item(6)
            Dim clatype As String = studentsReader(5).ToString
            studentsReader.Close()

            Dim ds2 As New DataTable
            Dim CA1 As Boolean
            Dim CA2 As Boolean
            Dim CA3 As Boolean
            Dim project As Boolean
            Dim exams As Boolean
            Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from scorespublish where term = '" & Session("SessionId") & "'", con)
            Dim subjectreader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
            subjectreader2.Read()
            CA1 = subjectreader2("CA1")
            CA2 = subjectreader2("CA2")
            CA3 = subjectreader2("CA3")
            exams = subjectreader2("Exams")
            project = subjectreader2.Item("project")
            subjectreader2.Close()
            Dim cmd21 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from studentsummary where student = ? and session = ? ", con)
            cmd21.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", Session("Studentadd")))
            cmd21.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("term", Session("Sessionid")))
            Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd21.ExecuteReader()
            reader2.Read()
            lblAverage.Text = reader2("Average")
            lblPosition.Text = reader2("position")
            txtRem.Text = reader2("principalremarks").ToString
           


            reader2.Close()
            If clatype <> "K.G 1 Special" Then
                Dim scorearray As New ArrayList
                Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject as 'Subject', subjectreg.CA1 as '1st CA', subjectreg.CA2 as '2nd CA', subjectreg.CA3 as '3rd CA', subjectreg.project as '4th CA', subjectreg.testtotal as 'Total CA', subjectreg.examination as 'Examination', subjectreg.total as 'Term total', subjectreg.grade as 'Grade', subjectreg.remarks as 'Remarks' from subjectreg Inner join Subjects on subjects.ID = subjectreg.subjectsofferred where subjectreg.student = ? and subjectreg.session = ? and subjectreg.nested = '" & 0 & "' order by subjectreg.id", con)
                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", Session("StudentAdd")))
                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("term", Session("SessionId")))
                Dim subjectreader As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader
                Dim i As Integer
                Do While subjectreader.Read
                    scorearray.Add(subjectreader.Item(0))


                    scorearray.Add(subjectreader.Item(1))
                    If i = 0 Then
                        ds2.Columns.Add("Subject")
                        ds2.Columns.Add("1st CA")
                    End If

                    scorearray.Add(subjectreader.Item(2))
                    If i = 0 Then
                        ds2.Columns.Add("2nd CA")
                    End If

                    If cano >= 3 Then
                        scorearray.Add(subjectreader.Item(3))
                        If i = 0 Then
                            ds2.Columns.Add("3rd CA")
                        End If
                    End If
                    If cano > 3 Then
                        scorearray.Add(subjectreader.Item(4))

                        If i = 0 Then
                            ds2.Columns.Add("Project")

                        End If
                    End If

                    scorearray.Add(subjectreader.Item(5))
                    scorearray.Add(subjectreader.Item(6))
                    scorearray.Add(subjectreader.Item(7))
                    scorearray.Add(subjectreader.Item(8))
                    scorearray.Add(subjectreader.Item(9))
                    If i = 0 Then
                        ds2.Columns.Add("Total CA")
                        ds2.Columns.Add("Exams")
                        ds2.Columns.Add("Total")
                        ds2.Columns.Add("Grade")
                        ds2.Columns.Add("Remarks")


                    End If
                  

                    If scorearray.Count = 1 Then
                        Show_Alert(False, "No CA published yet.")
                    ElseIf scorearray.Count = 2 Then
                        ds2.Rows.Add(scorearray(0), scorearray(1))
                    ElseIf scorearray.Count = 3 Then
                        ds2.Rows.Add(scorearray(0), scorearray(1), scorearray(2))
                    ElseIf scorearray.Count = 4 Then
                        ds2.Rows.Add(scorearray(0), scorearray(1), scorearray(2), scorearray(3))
                    ElseIf scorearray.Count = 5 Then
                        ds2.Rows.Add(scorearray(0), scorearray(1), scorearray(2), scorearray(3), scorearray(4))
                    ElseIf scorearray.Count = 8 Then
                        ds2.Rows.Add(scorearray(0), scorearray(1), scorearray(2), scorearray(3), scorearray(4), scorearray(5), scorearray(6), scorearray(7))
                    ElseIf scorearray.Count = 9 Then
                        ds2.Rows.Add(scorearray(0), scorearray(1), scorearray(2), scorearray(3), scorearray(4), scorearray(5), scorearray(6), scorearray(7), scorearray(8))

                    ElseIf scorearray.Count = 10 Then
                        ds2.Rows.Add(scorearray(0), scorearray(1), scorearray(2), scorearray(3), scorearray(4), scorearray(5), scorearray(6), scorearray(7), scorearray(8), scorearray(9))

                        panel4.Visible = True
                    End If

                    i = i + 1
                    scorearray.Clear()
                Loop
                subjectreader.Close()
                Dim cmd234 As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjectnest.name as 'Subject', subjectreg.CA1 as '1st CA', subjectreg.CA2 as '2nd CA', subjectreg.CA3 as '3rd CA', subjectreg.project as '4th CA', subjectreg.testtotal as 'Total CA', subjectreg.examination as 'Examination', subjectreg.total as 'Term total', subjectreg.grade as 'Grade', subjectreg.remarks as 'Remarks' from subjectreg Inner join Subjectnest  on subjectnest.ID = subjectreg.subjectsofferred where subjectreg.student = ? and subjectreg.session = ? and subjectreg.nested <> '" & 0 & "' order by subjectreg.id", con)
                cmd234.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", Session("StudentAdd")))
                cmd234.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("term", Session("SessionId")))
                Dim subjectreadersss3 As MySql.Data.MySqlClient.MySqlDataReader = cmd234.ExecuteReader

                Do While subjectreadersss3.Read
                    scorearray.Add(subjectreadersss3.Item(0))


                    scorearray.Add(subjectreadersss3.Item(1))


                    scorearray.Add(subjectreadersss3.Item(2))

                    If cano >= 3 Then
                        scorearray.Add(subjectreadersss3.Item(3))
                    End If
                    If cano > 3 Then
                        scorearray.Add(subjectreadersss3.Item(4))
                    End If



                    scorearray.Add(subjectreadersss3.Item(5))
                    scorearray.Add(subjectreadersss3.Item(6))
                    scorearray.Add(subjectreadersss3.Item(7))
                    scorearray.Add(subjectreadersss3.Item(8))
                    scorearray.Add(subjectreadersss3.Item(9))





                    If scorearray.Count = 1 Then
                        Show_Alert(False, "No CA published yet.")
                    ElseIf scorearray.Count = 2 Then
                        ds2.Rows.Add(scorearray(0), scorearray(1))
                    ElseIf scorearray.Count = 3 Then
                        ds2.Rows.Add(scorearray(0), scorearray(1), scorearray(2))
                    ElseIf scorearray.Count = 4 Then
                        ds2.Rows.Add(scorearray(0), scorearray(1), scorearray(2), scorearray(3))
                    ElseIf scorearray.Count = 5 Then
                        ds2.Rows.Add(scorearray(0), scorearray(1), scorearray(2), scorearray(3), scorearray(4))
                    ElseIf scorearray.Count = 8 Then
                        ds2.Rows.Add(scorearray(0), scorearray(1), scorearray(2), scorearray(3), scorearray(4), scorearray(5), scorearray(6), scorearray(7))
                    ElseIf scorearray.Count = 9 Then
                        ds2.Rows.Add(scorearray(0), scorearray(1), scorearray(2), scorearray(3), scorearray(4), scorearray(5), scorearray(6), scorearray(7), scorearray(8))

                    ElseIf scorearray.Count = 10 Then
                        ds2.Rows.Add(scorearray(0), scorearray(1), scorearray(2), scorearray(3), scorearray(4), scorearray(5), scorearray(6), scorearray(7), scorearray(8), scorearray(9))

                        panel4.Visible = True
                    End If


                    scorearray.Clear()
                Loop
                subjectreadersss3.Close()

                tblstats.Visible = True

                GridView2.DataSource = ds2
                GridView2.DataBind()
                panel3.Visible = True
            Else
                Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject as 'Subject', kcourseoutline.topic as 'Topic', kscoresheet.score as 'Score', kscoresheet.grade as 'Grade', kscoresheet.remarks as 'Remarks' from kscoresheet inner join kcourseoutline on kcourseoutline.id = kscoresheet.topic inner join subjects on kscoresheet.subject = subjects.id  where kscoresheet.student = ? and kscoresheet.session = ? order by subjects.id", con)
                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", Session("StudentAdd")))
                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("term", Session("SessionId")))
                Dim dsc As New DataTable
                Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
                adapter1.SelectCommand = cmd
                adapter1.Fill(dsc)
                GridView2.DataSource = dsc
                GridView2.DataBind()
                panel3.Visible = True

                tblstats.Visible = False
            End If
            pnlAll.Visible = False

            con.close()        End Using
    End Sub

    Protected Sub gridview1_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles gridview1.SelectedIndexChanging
        Try
            Dim x As Array = Split(gridview1.Rows(e.NewSelectedIndex).Cells(1).Text, " - ")
            Session("studentadd") = RTrim(x(0))
            Dim type As String
            Dim exams As Boolean
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from scorespublish where term = '" & Session("SessionId") & "'", con)
                Dim subjectreader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                subjectreader2.Read()
                exams = subjectreader2("Exams")
                subjectreader2.Close()
                Dim cmd21 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.type, class.id from studentsummary inner join class on class.id = studentsummary.class where studentsummary.student = ? and studentsummary.session = '" & Session("sessionid") & "'", con)
                cmd21.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", Session("Studentadd")))
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd21.ExecuteReader()
                reader2.Read()
                type = reader2(0)
                Session("rsClass") = reader2(1)
                reader2.Close()
                con.Close()
            End Using
            If type = "Early Years" Then
                Session("rsSession") = Session("sessionid")
                Session("studentid") = Session("studentadd")
                Response.Redirect("~/content/Student/result.aspx")
            Else
                Student_Details()
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

   

    Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()

            panel3.Visible = False

            Dim ds As New DataTable
            ds.Columns.Add("passport")
            ds.Columns.Add("staffname")
            Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.passport, studentsprofile.admno, studentsprofile.surname from studentsummary inner join studentsprofile on studentsummary.student = studentsprofile.admno inner join class on studentsummary.class = class.id WHERE class.class = ? and studentsummary.session = '" & Session("sessionid") & "'", con)
            cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", DropDownList1.Text))
            Dim studentox As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
            Do While studentox.Read
                ds.Rows.Add(studentox.Item(0).ToString, studentox.Item(1) & " - " & studentox.Item(2).ToString)
            Loop
            studentox.Close()
            gridview1.DataSource = ds
            gridview1.DataBind()
            con.close()        End Using
    End Sub
    Protected Sub lnkResult_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Session("rPeriod") = "Full Term"
        Session("rsSession") = Session("sessionid")
        Session("studentid") = Session("studentadd")
        Session("rsClass") = Session("classid")
        Response.Redirect("~/content/Student/result.aspx")
    End Sub

    Protected Sub btnMid_Click(sender As Object, e As EventArgs) Handles btnMid.Click
        Session("rPeriod") = "Half Term"
        Session("rsSession") = Session("sessionid")
        Session("studentid") = Session("studentadd")
        Session("rsClass") = Session("classid")
        Response.Redirect("~/content/Student/result.aspx")
    End Sub
End Class
