Imports System.Text
Imports System.Configuration
Imports System.Data
Imports System.Web.UI.WebControls
Imports System.Web.UI.DataVisualization.Charting

Partial Class Student_scores
    Inherits System.Web.UI.Page



    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not IsPostBack Then
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject from subjectreg Inner join subjects on subjectreg.subjectsofferred = subjects.id inner join classsubjects on classsubjects.subject = subjectreg.subjectsofferred and classsubjects.class = subjectreg.class where subjectreg.student = '" & Session("Studentid") & "' and subjectreg.session = '" & Session("sessionid") & "' order by classsubjects.id", con)
                Dim subjectreader As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader
                Do While subjectreader.Read
                    chkSubjects.Items.Add(subjectreader(0).ToString)
                Loop
                con.close()            End Using
            If Request.QueryString.ToString <> Nothing Then
                For Each item As ListItem In chkSubjects.Items
                    If item.Text = Replace(Request.QueryString.ToString, "+", " ") Then
                        item.Selected = True
                    End If
                Next
            Else
                For Each item As ListItem In chkSubjects.Items

                    item.Selected = True

                Next
            End If
        End If
        Load_Chart()
    End Sub

    Private Sub Load_Chart()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            For Each item As ListItem In chkSubjects.Items
                If item.Selected = True Then
                    Dim scores As New ArrayList
                    Dim terms As New ArrayList
                    Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT session.session, session.term, subjectreg.total, studentsprofile.surname, studentsprofile.passport from studentsummary Inner join session on session.id = studentsummary.session inner join studentsprofile on studentsprofile.admno = studentsummary.student inner join (subjectreg inner join subjects on subjects.id = subjectreg.subjectsofferred)  on studentsummary.student = subjectreg.student where studentsummary.student = '" & Session("studentid") & "' and subjects.subject = '" & item.Text & "' order by studentsummary.session", con)
                    Dim subjectreader As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader
                    Do While subjectreader.Read
                        scores.Add(subjectreader(2))
                        terms.Add(subjectreader(0) & " - " & subjectreader(1))
                    Loop

                    Chart1.Series.Add(New Series(item.Text))
                    Chart1.Series(item.Text).IsValueShownAsLabel = True
                    Chart1.Series(item.Text).BorderWidth = 2
                    Chart1.Series(item.Text).ChartType = SeriesChartType.Line
                    Chart1.Series(item.Text).Points.DataBindXY(terms, scores)
                    Chart1.Legends(0).Enabled = True
                    lblname.Text = subjectreader(3).ToString
                    lblClass.Text = Session("ClassName")
                    lblID.Text = Session("StudentID")
                    img1.Src = subjectreader(4).ToString
                    subjectreader.Close()
                End If
            Next
            con.close()        End Using
        Load_Average()

    End Sub
    Private Sub Load_Average()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()

            Dim scores As New ArrayList
            Dim terms As New ArrayList
            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT session.session, session.term, studentsummary.average, studentsprofile.surname, studentsprofile.passport from studentsummary Inner join session on session.id = studentsummary.session inner join studentsprofile on studentsprofile.admno = studentsummary.student  where studentsummary.student = '" & Session("studentid") & "' order by studentsummary.session", con)
            Dim subjectreader As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader
            Do While subjectreader.Read
                scores.Add(subjectreader(2))
                terms.Add(subjectreader(0) & " - " & subjectreader(1))
            Loop

            Chart1.Series.Add(New Series("Average"))
            Chart1.Series("Average").IsValueShownAsLabel = True
            Chart1.Series("Average").BorderWidth = 2
            Chart1.Series("Average").ChartType = SeriesChartType.Line
            Chart1.Series("Average").Points.DataBindXY(terms, scores)
            Chart1.Legends(0).Enabled = True
            lblname.Text = subjectreader(3).ToString
            lblClass.Text = Session("ClassName")
            lblID.Text = Session("StudentID")
            img1.Src = subjectreader(4).ToString
            con.close()        End Using
    End Sub
    Protected Sub Chart1_Load(sender As Object, e As EventArgs) Handles Chart1.Load

    End Sub

    Protected Sub chkSubjects_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chkSubjects.SelectedIndexChanged

    End Sub
End Class
