Imports System.Text
Imports System.Configuration
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Staff_performance
    Inherits System.Web.UI.Page
    Dim pagebefore As String
    Dim studentId As String
    Dim termID As Integer
    Dim checkedSubjects As New ArrayList
    Dim uncheckedSubjects As New ArrayList


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        lblError.Text = ""
        lblSuccess.Text = ""
        If Not IsPostBack Then
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session", con)
                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader()
                Do While reader.Read()
                    cboYr.Items.Add(reader.Item(1).ToString & "   " & reader.Item(2).ToString)
                Loop
                cboYr.Text = reader.Item(1).ToString & "   " & reader.Item(2).ToString
                reader.Close()
                If Session("ClassID") Is Nothing Then
                    Session("ClassID") = Session("AddClass")
                End If

                Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT StudentsProfile.surname FROM StudentsProfile INNER JOIN StudentSummary ON StudentsProfile.admno = StudentSummary.student WHERE StudentSummary.Class = ? And StudentSummary.Session = ? ORDER BY StudentsProfile.surname", con)
                cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class.ID", Session("ClassID")))
                cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter(" StudentSummary.Session ", Session("SessionID")))
                Dim studentsReader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader()
                DropDownList1.Items.Clear()
                DropDownList1.Items.Add("Select a student")
                Do While studentsReader.Read
                    DropDownList1.Items.Add(studentsReader.Item(0).ToString)
                Loop
                studentsReader.Close()
                con.close()            End Using
            If Not Session("StudentId") Is Nothing Then
                DropDownList1.Text = Session("StudentId").ToString
                Load_result()
            End If

        End If
    End Sub


    Protected Sub Load_result()
        Dim sArray() As String = Split(cboYr.Text, "  ")
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session where session = ? and term = ?", con)
            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Trim(sArray(0))))
            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("closing", Trim(sArray(1))))
            Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader()
            reader.Read()
            Dim newsession As Integer = reader.Item(0)
            reader.Close()

            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from StudentsProfile where surname = ?", con)
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Trim(DropDownList1.Text)))
            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            student.Read()
            studentId = student.Item(0)
            Session("studentId") = student.Item(0)
            student.Close()

            Dim ds2 As New DataTable
            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject, subjectreg.CA1, subjectreg.CA2, subjectreg.CA3, subjectreg.project, subjectreg.testtotal, subjectreg.examination, subjectreg.total, subjectreg.avg, subjectreg.highest, lowest, subjectreg.pos, subjectreg.grade, subjectreg.remarks from subjectreg Inner join Subjects on subjects.ID = subjectreg.subjectsofferred where subjectreg.student = ? and subjectreg.class = ? and subjectreg.session = ? ", con)
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", studentId))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("ClassId")))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("term", newsession))
            Dim adapter2 As New MySql.Data.MySqlClient.MySqlDataAdapter
            adapter2.SelectCommand = cmd
            adapter2.Fill(ds2)
            GridView2.DataSource = ds2
            GridView2.DataBind()

            Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from studentsummary where student = ? and class = ? and session = ? ", con)
            cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", studentId))
            cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("ClassId")))
            cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("term", newsession))
            Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader()
            reader2.Read()
            txtRem.Text = reader2("classTeacherRemarks")
            txtHandwriting.Text = reader2("handwriting")
            txtFluency.Text = reader2("fluency")
            txtGames.Text = reader2("games")
            txtSports.Text = reader2("sports")
            txtGymnastics.Text = reader2("gymnastics")
            txttools.Text = reader2("tools")
            txtDrawing.Text = reader2("drawing")
            txtCrafts.Text = reader2("crafts")
            txtMusical.Text = reader2("musical")
            txtPunctual.Text = reader2("punctual")
            txtAttendance.Text = reader2("attendance")
            txtReliability.Text = reader2("reliability")
            txtNeatness.Text = reader2("neatness")
            txtPolite.Text = reader2("polite")
            txtHonest.Text = reader2("honesty")
            txtRelate.Text = reader2("relate")
            txtSelfControl.Text = reader2("selfcontrol")
            txtCooperate.Text = reader2("cooperation")
            txtResponsible.Text = reader2("responsibility")
            txtAttentive.Text = reader2("attentiveness")
            txtInitiative.Text = reader2("initiative")
            txtOrganized.Text = reader2("organization")
            txtPersevere.Text = reader2("perseverance")
            reader2.Close()
            con.close()        End Using
    End Sub

    Protected Sub cboYr_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboYr.SelectedIndexChanged
        Load_result()
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim sArray() As String = Split(cboYr.Text, "  ")
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session where session = ? and term = ?", con)
            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Trim(sArray(0))))
            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("closing", Trim(sArray(1))))
            Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader()
            reader.Read()
            Dim newsession As Integer = reader.Item(0)
            reader.Close()
            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("Update studentsummary set classTeacherRemarks = ?, handWriting = ?, fluency = ?, games = ?, sports = ?, gymnastics = ?, tools = ?, drawing = ?, crafts = ?, musical = ?, punctual = ?, attendance = ?, reliability = ?, neatness = ?, polite = ?, honesty = ?, relate = ?, selfcontrol = ?, cooperation = ?, responsibility = ?, attentiveness = ?, initiative = ?, organization = ?, perseverance = ? where Student = ? and Class = ? and Session = ? ", con)
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("remarks", txtRem.Text))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("writ", txtHandwriting.Text))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fluent", txtFluency.Text))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("games", txtGames.Text))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sports", txtSports.Text))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("gymnastics", txtGymnastics.Text))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("tools", txttools.Text))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("drawing", txtDrawing.Text))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("crafts", txtCrafts.Text))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("music", txtMusical.Text))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("punctual", txtPunctual.Text))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("attendance", txtAttendance.Text))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("reliability", txtReliability.Text))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("neatness", txtNeatness.Text))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("polite", txtPolite.Text))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("honesty", txtHonest.Text))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("relate", txtRelate.Text))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("selfcon", txtSelfControl.Text))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cooperate", txtCooperate.Text))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("response", txtResponsible.Text))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("attentive", txtAttentive.Text))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("initiative", txtInitiative.Text))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("organization", txtOrganized.Text))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("persevere", txtPersevere.Text))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", Session("StudentId")))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("ClassId")))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("term", newsession))
            cmd.ExecuteNonQuery()
            con.close()        End Using
        lblSuccess.Text = "Update successful"

    End Sub

    Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged
        Load_result()
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim sArray() As String = Split(cboYr.Text, "  ")
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session where session = ? and term = ?", con)
            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Trim(sArray(0))))
            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("closing", Trim(sArray(1))))
            Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader()
            reader.Read()
            Dim newsession As Integer = reader.Item(0)
            reader.Close()
            Session("rsClass") = Session("ClassId")
            Session("rsSession") = newsession
            con.close()        End Using
        Response.Redirect("~/Staff/classreport.aspx")
    End Sub
End Class
