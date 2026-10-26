Imports System.Text
Imports System.Configuration
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Student_scores
    Inherits System.Web.UI.Page



    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not IsPostBack Then
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()

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
                Dim scorearray As New ArrayList
                Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject, subjectreg.CA1, subjectreg.CA2, subjectreg.CA3, subjectreg.project, subjectreg.testtotal, subjectreg.examination, subjectreg.total from subjectreg Inner join Subjects on subjects.ID = subjectreg.subjectsofferred where subjectreg.student = ? and subjectreg.session = ? ", con)
                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", Session("StudentId")))
                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("term", Session("SessionId")))
                Dim subjectreader As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader
                Dim i As Integer
                Do While subjectreader.Read
                    If CA1 = True Then
                        scorearray.Add(subjectreader.Item(0))
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
                        scorearray.Add(subjectreader.Item(5))
                        If i = 0 Then
                            ds2.Columns.Add("Project")
                            ds2.Columns.Add("Total CA")
                        End If
                    End If
                    If exams = True Then
                        scorearray.Add(subjectreader.Item(6))
                        scorearray.Add(subjectreader.Item(7))
                        If i = 0 Then
                            ds2.Columns.Add("Exams")
                            ds2.Columns.Add("Total")
                        End If
                    End If
                    If scorearray.Count = 1 Then
                        lblError.Text = "No CA published yet."
                    ElseIf scorearray.Count = 2 Then
                        ds2.Rows.Add(scorearray(0), scorearray(1))
                    ElseIf scorearray.Count = 3 Then
                        ds2.Rows.Add(scorearray(0), scorearray(1), scorearray(2))
                    ElseIf scorearray.Count = 4 Then
                        ds2.Rows.Add(scorearray(0), scorearray(1), scorearray(2), scorearray(3))
                    ElseIf scorearray.Count = 6 Then
                        ds2.Rows.Add(scorearray(0), scorearray(1), scorearray(2), scorearray(3), scorearray(4), scorearray(5))
                    ElseIf scorearray.Count = 8 Then
                        ds2.Rows.Add(scorearray(0), scorearray(1), scorearray(2), scorearray(3), scorearray(4), scorearray(5), scorearray(6), scorearray(7))
                    End If

                    i = i + 1
                    scorearray.Clear()
                Loop
                GridView1.DataSource = ds2
                GridView1.DataBind()
                lblname.Text = Session("student")
                lblClass.Text = Session("ClassName")
                lblID.Text = Session("StudentID")
                img1.Src = Session("studentpass")
                con.close()            End Using
        End If
    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        Dim sArray() As String = Split(cboYr.Text, "  ")
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From class where class = ?", con)
            cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", lblClass.Text))
            Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader
            reader3.Read()
            Session("rsClass") = reader3.Item(0).ToString
            reader3.Close()

            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session where session = ? and term = ?", con)
            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Trim(sArray(0))))
            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("closing", Trim(sArray(1))))
            Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader()
            reader.Read()
            If reader.Item(6) = "Unpublished" Then
                lblError.Text = "Results not yet available for this term"
                con.close()end using
            Else
        Session("rsSession") = reader.Item(0)
        con.close()end using
        Response.Redirect("~/Student/result.aspx")
            End If
    End Sub

    Protected Sub cboYr_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboYr.SelectedIndexChanged
        Dim sArray() As String = Split(cboYr.Text, "  ")
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session where session = ? and term = ?", con)
            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Trim(sArray(0))))
            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("closing", Trim(sArray(1))))
            Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader()
            reader.Read()
            Dim newsession As Integer = reader.Item(0)
            reader.Close()
            Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.class from studentsummary inner join class on studentsummary.class = class.Id where studentsummary.student = ? and studentsummary.session = ?", con)
            cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", Session("StudentId")))
            cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("term", newsession))
            Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader()
            reader2.Read()
            lblClass.Text = reader2.Item(0)
            reader2.Close()
            Dim ds2 As New DataTable
            Dim CA1 As Boolean
            Dim CA2 As Boolean
            Dim CA3 As Boolean
            Dim project As Boolean
            Dim exams As Boolean
            Dim cmd20 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from scorespublish where term = '" & newsession & "'", con)
            Dim subjectreader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd20.ExecuteReader
            subjectreader2.Read()
            CA1 = subjectreader2("CA1")
            CA2 = subjectreader2("CA2")
            CA3 = subjectreader2("CA3")
            exams = subjectreader2("Exams")
            project = subjectreader2.Item("project")
            subjectreader2.Close()
            Dim scorearray As New ArrayList
            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject, subjectreg.CA1, subjectreg.CA2, subjectreg.CA3, subjectreg.project, subjectreg.testtotal, subjectreg.examination, subjectreg.total from subjectreg Inner join Subjects on subjects.ID = subjectreg.subjectsofferred where subjectreg.student = ? and subjectreg.session = ? ", con)
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", Session("StudentId")))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("term", newsession))
            Dim subjectreader As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader
            Dim i As Integer
            Do While subjectreader.Read
                If CA1 = True Then
                    scorearray.Add(subjectreader.Item(0))
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
                    scorearray.Add(subjectreader.Item(5))
                    If i = 0 Then
                        ds2.Columns.Add("Project")
                        ds2.Columns.Add("Total CA")
                    End If
                End If
                If exams = True Then
                    scorearray.Add(subjectreader.Item(6))
                    scorearray.Add(subjectreader.Item(7))
                    If i = 0 Then
                        ds2.Columns.Add("Exams")
                        ds2.Columns.Add("Total")
                    End If
                End If
                If scorearray.Count = 1 Then
                    lblError.Text = "No CA published yet."
                ElseIf scorearray.Count = 2 Then
                    ds2.Rows.Add(scorearray(0), scorearray(1))
                ElseIf scorearray.Count = 3 Then
                    ds2.Rows.Add(scorearray(0), scorearray(1), scorearray(2))
                ElseIf scorearray.Count = 4 Then
                    ds2.Rows.Add(scorearray(0), scorearray(1), scorearray(2), scorearray(3))
                ElseIf scorearray.Count = 6 Then
                    ds2.Rows.Add(scorearray(0), scorearray(1), scorearray(2), scorearray(3), scorearray(4), scorearray(5))
                ElseIf scorearray.Count = 8 Then
                    ds2.Rows.Add(scorearray(0), scorearray(1), scorearray(2), scorearray(3), scorearray(4), scorearray(5), scorearray(6), scorearray(7))
                End If

                i = i + 1
                scorearray.Clear()
            Loop
            GridView1.DataSource = ds2
            GridView1.DataBind()
            lblname.Text = Session("student")
            lblID.Text = Session("StudentID")
            img1.Src = Session("studentpass")
            con.close()        End Using
    End Sub
End Class
