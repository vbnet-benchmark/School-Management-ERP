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
                    DropDownList1.Items.Clear()
                    Dim clasadd As New ArrayList
                    For Each item As String In classcontroled
                        Dim classes As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from class inner join depts on class.superior = depts.Id where depts.dept = '" & item & "'", con)
                        Dim schclass As MySql.Data.MySqlClient.MySqlDataReader = classes.ExecuteReader

                        Do While schclass.Read
                            If Not clasadd.Contains(schclass(0)) Then
                                clasadd.Add(schclass(0))
                                DropDownList1.Items.Add(schclass.Item(0).ToString)
                            End If
                        Loop
                        schclass.Close()
                    Next
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
                    con.Close()                End Using
                If Request.QueryString.ToString <> Nothing Then
                    Session("studentadd") = Request.QueryString.ToString
                End If
                If Session("studentadd") <> Nothing Then
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
            Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.surname, class.class, studentsummary.trans, studentsummary.status, studentsummary.class, class.type From Studentsummary INNER JOIN Studentsprofile on studentsummary.student = studentsprofile.admno inner join class on studentsummary.class = class.Id WHERE StudentSummary.student = ? And StudentSummary.Session = ?", con)
            cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class.ID", Session("Studentadd")))
            cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter(" StudentSummary.Session ", Session("SessionID")))
            Dim studentsReader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader()
            studentsReader.Read()
            lblClass.Text = studentsReader.Item(0).ToString
            Session("ClassId") = studentsReader.Item(4)
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
            Dim cmd21 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from studentsummary where student = ? and class = ? and session = ? ", con)
            cmd21.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", Session("Studentadd")))
            cmd21.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("ClassId")))
            cmd21.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("term", Session("Sessionid")))
            Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd21.ExecuteReader()
            reader2.Read()
            lblAverage.Text = reader2("Average")
            lblPosition.Text = reader2("position")
            lblComments.Text = reader2("principalRemarks")
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
                    If CA1 = True Then

                        scorearray.Add(subjectreader.Item(1))
                        If i = 0 Then
                            ds2.Columns.Add("Subject")
                            ds2.Columns.Add("1st CA")
                        End If
                    End If
                    If CA2 = True Then
                        scorearray.Add(subjectreader.Item(2))
                        If i = 0 Then
                            ds2.Columns.Add("2nd CA")
                        End If
                    End If
                    If CA3 = True Then
                        scorearray.Add(subjectreader.Item(3))
                        If i = 0 Then
                            ds2.Columns.Add("3rd CA")
                        End If
                    End If
                    If project = True Then
                        scorearray.Add(subjectreader.Item(4))

                        If i = 0 Then
                            ds2.Columns.Add("Project")

                        End If
                    End If
                    If exams = True Then

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
                    If CA1 = True Then

                        scorearray.Add(subjectreadersss3.Item(1))

                    End If
                    If CA2 = True Then
                        scorearray.Add(subjectreadersss3.Item(2))

                    End If
                    If CA3 = True Then
                        scorearray.Add(subjectreadersss3.Item(3))

                    End If
                    If project = True Then
                        scorearray.Add(subjectreadersss3.Item(4))


                    End If
                    If exams = True Then

                        scorearray.Add(subjectreadersss3.Item(5))
                        scorearray.Add(subjectreadersss3.Item(6))
                        scorearray.Add(subjectreadersss3.Item(7))
                        scorearray.Add(subjectreadersss3.Item(8))
                        scorearray.Add(subjectreadersss3.Item(9))


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


                    scorearray.Clear()
                Loop
                subjectreadersss3.Close()



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


            End If
            pnlAll.Visible = False

            con.close()        End Using
    End Sub

    Protected Sub gridview1_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles gridview1.SelectedIndexChanging
        Try
            Dim x As Array = Split(gridview1.Rows(e.NewSelectedIndex).Cells(1).Text, " - ")
            Session("studentadd") = RTrim(x(0))
            Student_Details()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        pnlAll.Visible = True
        panel3.Visible = False
        gridview1.SelectedIndex = -1
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

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Session("rsSession") = Session("sessionid")
        Session("studentid") = Session("studentadd")
        Session("rsClass") = Session("classid")
        Response.Redirect("~/content/app/Student/result.aspx")
    End Sub
End Class
