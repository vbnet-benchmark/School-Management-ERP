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
                Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT session.session, session.term, studentsummary.average from studentsummary Inner join session on session.id = studentsummary.session where studentsummary.student = ?", con)
                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", Session("StudentId")))
                Dim subjectreader As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader
                Dim i As Integer
                ds2.Columns.Add("Term")
                ds2.Columns.Add("Average")
                Do While subjectreader.Read
                    scorearray.Add(subjectreader.Item(0) & " - " & subjectreader.Item(1))
                    scorearray.Add(subjectreader.Item(2))
                    ds2.Rows.Add(scorearray(0), scorearray(1))
                    scorearray.Clear()
                Loop

                Chart1.DataSource = ds2
                Chart1.DataBind()
                lblname.Text = Session("student")
                lblClass.Text = Session("ClassName")
                lblID.Text = Session("StudentID")
                img1.Src = Session("studentpass")
                con.close()            End Using
        End If
    End Sub

   
End Class
