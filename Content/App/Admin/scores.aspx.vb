Imports System.Text
Imports System.Configuration
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Student_scores
    Inherits System.Web.UI.Page



    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not IsPostBack Then
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from StudentsProfile where admno = ?", con)
                cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Request.QueryString.ToString))
                Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                student.Read()
                lblname.Text = student.Item(1)
                lblID.Text = student.Item(0)
                Dim pass As String
                Try
                    pass = student("passport")
                Catch ex As Exception

                End Try

                student.Close()

                Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM sessioncreate", con)
                Dim reader1 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck.ExecuteReader
                cboYr.Items.Clear()

                reader1.Close()

                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session", con)
                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader()
                Do While reader.Read()
                    cboYr.Items.Add(reader.Item(1).ToString & "   " & reader.Item(2).ToString)
                Loop
                cboYr.Text = reader.Item(1).ToString & "   " & reader.Item(2).ToString
                reader.Close()

                Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.class from studentsummary inner join class on studentsummary.class = class.Id where studentsummary.student = ? and studentsummary.session = ?", con)
                cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", Request.QueryString.ToString))
                cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("term", Session("SessionId")))
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader()
                reader2.Read()
                lblClass.Text = reader2.Item(0)
                reader2.Close()

                Dim ds2 As New DataTable
                Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject, subjectreg.CA1, subjectreg.CA2, subjectreg.CA3, subjectreg.project, subjectreg.testtotal, subjectreg.examination, subjectreg.total from subjectreg Inner join Subjects on subjects.ID = subjectreg.subjectsofferred where subjectreg.student = ? and subjectreg.session = ? ", con)
                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", Request.QueryString.ToString))
                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("term", Session("SessionId")))
                Dim adapter2 As New MySql.Data.MySqlClient.MySqlDataAdapter
                adapter2.SelectCommand = cmd
                adapter2.Fill(ds2)
                GridView1.DataSource = ds2
                GridView1.DataBind()
                img1.Src = pass
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
        Response.Redirect("~/Admin/result.aspx?" & Request.QueryString.ToString)
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
            Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from StudentsProfile where admno = ?", con)
            cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Request.QueryString.ToString))
            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
            student.Read()
            lblname.Text = student.Item(1)
            lblID.Text = student.Item(0)
            Dim pass As String = student("passport")
            student.Close()
            Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.class from studentsummary inner join class on studentsummary.class = class.Id where studentsummary.student = ? and studentsummary.session = ?", con)
            cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", Request.QueryString.ToString))
            cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("term", newsession))
            Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader()
            reader2.Read()
            lblClass.Text = reader2.Item(0)
            reader2.Close()
            Dim ds2 As New DataTable
            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject, subjectreg.CA1, subjectreg.CA2, subjectreg.CA3, subjectreg.project, subjectreg.testtotal, subjectreg.examination, subjectreg.total from subjectreg Inner join Subjects on subjects.ID = subjectreg.subjectsofferred where subjectreg.student = ? and subjectreg.session = ? ", con)
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", Request.QueryString.ToString))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("term", newsession))
            Dim adapter2 As New MySql.Data.MySqlClient.MySqlDataAdapter
            adapter2.SelectCommand = cmd
            adapter2.Fill(ds2)
            GridView1.DataSource = ds2
            GridView1.DataBind()
            img1.Src = pass
            con.close()        End Using
    End Sub
End Class
