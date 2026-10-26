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
        Try

       
        If Request.QueryString.ToString <> Nothing Then
            Dim a As Array = Split(Request.QueryString.ToString, "%2c")
            Session("Studentid") = a(0)
                Session("Sessionid") = a(1)
                logify.Read_notification("~/content/student/myscores.aspx?" & Session("Studentid") & "," & Session("Sessionid"), Session("parentid"))
                logify.Read_notification("~/content/student/myscores.aspx?" & Session("Studentid") & "," & Session("Sessionid"), Session("studentid"))

        End If
        Student_Details()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

   
    


    Private Sub Student_Details()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport from StudentsProfile where admno = ?", con)
            cmdLoad4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("studentid")))
            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
            Dim pass As String = ""
            If student.Read() Then pass = student.Item("passport").ToString
            student.Close()
            If Not pass = "" Then Image1.ImageUrl = pass
            Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.surname, class.class, studentsummary.trans, studentsummary.status, studentsummary.class, class.type From Studentsummary INNER JOIN Studentsprofile on studentsummary.student = studentsprofile.admno inner join class on studentsummary.class = class.Id WHERE StudentSummary.student = ? And StudentSummary.Session = ?", con)
            cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class.ID", Session("StudentId")))
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
            cmd21.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", Session("StudentId")))
            cmd21.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("ClassId")))
            cmd21.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("term", Session("Sessionid")))
            Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd21.ExecuteReader()
            reader2.Read()
            lblAverage.Text = reader2("Average")
            lblComments.Text = reader2("principalRemarks")
            Dim status As Boolean = reader2("status")
            reader2.Close()
            If clatype <> "K.G 1 Special" Then
                Dim scorearray As New ArrayList
                Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject as 'Subject', subjectreg.CA1 as '1st CA', subjectreg.CA2 as '2nd CA', subjectreg.CA3 as '3rd CA', subjectreg.project as 'Project', subjectreg.testtotal as 'Total CA', subjectreg.examination as 'Examination', subjectreg.total as 'Term total', subjectreg.grade as 'Grade', subjectreg.remarks as 'Remarks' from subjectreg Inner join Subjects on subjects.ID = subjectreg.subjectsofferred where subjectreg.student = ? and subjectreg.session = ? and subjectreg.nested = '" & 0 & "' order by subjectreg.id", con)
                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", Session("StudentId")))
                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("term", Session("SessionId")))
                Dim subjectreader As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader
                Dim i As Integer
                Do While subjectreader.Read
                    scorearray.Add(subjectreader.Item(0))


                    If i = 0 Then
                        ds2.Columns.Add("Subject")
                    End If


                    If project = True Then
                        scorearray.Add(subjectreader.Item(5))

                        If i = 0 Then
                            ds2.Columns.Add("Total CA")

                        End If
                    End If
                    If exams = True Then


                        scorearray.Add(subjectreader.Item(6))
                        scorearray.Add(subjectreader.Item(7))
                        scorearray.Add(subjectreader.Item(8))
                        scorearray.Add(subjectreader.Item(9))
                        If i = 0 Then
                           
                                ds2.Columns.Add("Exams")
                                ds2.Columns.Add("Total")
                                ds2.Columns.Add("Grade")
                                ds2.Columns.Add("Remarks")
                                panel4.Visible = True
                           
                        End If

                    End If


                    If scorearray.Count = 1 Then
                        Show_Alert(False, "No CA published yet.")
                    ElseIf scorearray.Count = 2 Then
                        ds2.Rows.Add(scorearray(0), scorearray(1))

                    ElseIf scorearray.Count = 6 Then
                        ds2.Rows.Add(scorearray(0), scorearray(1), scorearray(2), scorearray(3), scorearray(4), scorearray(5))
                        Button1.Visible = True
                        panel4.Visible = True
                    End If

                    i = i + 1
                    scorearray.Clear()
                Loop
                subjectreader.Close()
                Dim cmdsf As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjectnest.name as 'Subject', subjectreg.CA1 as '1st CA', subjectreg.CA2 as '2nd CA', subjectreg.CA3 as '3rd CA', subjectreg.project as 'Project', subjectreg.testtotal as 'Total CA', subjectreg.examination as 'Examination', subjectreg.total as 'Term total', subjectreg.grade as 'Grade', subjectreg.remarks as 'Remarks' from subjectreg Inner join Subjectnest on subjectnest.ID = subjectreg.subjectsofferred where subjectreg.student = ? and subjectreg.session = ? and subjectreg.nested = '" & 1 & "' order by subjectreg.id", con)
                cmdsf.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", Session("StudentId")))
                cmdsf.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("term", Session("SessionId")))
                Dim subjectreaderssff As MySql.Data.MySqlClient.MySqlDataReader = cmdsf.ExecuteReader

                Do While subjectreaderssff.Read
                    scorearray.Add(subjectreaderssff.Item(0))


                    If project = True Then
                        scorearray.Add(subjectreaderssff.Item(5))


                    End If
                    If exams = True Then

                        
                            scorearray.Add(subjectreaderssff.Item(6))
                            scorearray.Add(subjectreaderssff.Item(7))
                            scorearray.Add(subjectreaderssff.Item(8))
                            scorearray.Add(subjectreaderssff.Item(9))
                       
                    End If


                    If scorearray.Count = 1 Then
                        Show_Alert(False, "No CA published yet.")
                    ElseIf scorearray.Count = 2 Then
                        ds2.Rows.Add(scorearray(0), scorearray(1))
                    ElseIf scorearray.Count = 6 Then
                        ds2.Rows.Add(scorearray(0), scorearray(1), scorearray(2), scorearray(3), scorearray(4), scorearray(5))
                        Button1.Visible = True
                        panel4.Visible = True
                    End If


                    scorearray.Clear()
                Loop
                subjectreaderssff.Close()

                GridView2.DataSource = ds2
                GridView2.DataBind()
                panel3.Visible = True
            Else
                If exams = True  Then
                    Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject as 'Subject', kcourseoutline.topic as 'Topic', kscoresheet.grade as 'Grade', kscoresheet.remarks as 'Proficiency Observed' from kscoresheet inner join kcourseoutline on kcourseoutline.id = kscoresheet.topic inner join subjects on kscoresheet.subject = subjects.id  where kscoresheet.student = ? and kscoresheet.session = ? order by subjects.id", con)
                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", Session("Studentid")))
                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("term", Session("SessionId")))
                    Dim dsc As New DataTable
                    Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
                    adapter1.SelectCommand = cmd
                    adapter1.Fill(dsc)
                    GridView2.DataSource = dsc
                    GridView2.DataBind()
                    panel3.Visible = True
                    panel4.Visible = True
                    Button1.Visible = True
                    tblstats.Visible = False
                End If
            End If
            con.close()        End Using
    End Sub

   

    Protected Sub lnkResult_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Session("rsSession") = Session("sessionid")
        Session("rsClass") = Session("classid")
        Response.Redirect("~/Content/Student/result.aspx")
    End Sub
End Class
