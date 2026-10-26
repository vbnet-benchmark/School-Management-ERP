Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Admin_allstudents
    Inherits System.Web.UI.Page
    Dim pagebefore As String
    Dim studentId As Integer
    Dim termID As Integer
    Dim checkedSubjects As New ArrayList
    Dim uncheckedSubjects As New ArrayList

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

    Sub attendance()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.Open()
            Dim present As Integer = 0
            Dim absent As Integer = 0
            Dim opened As Integer = 0
            Dim ref As New MySql.Data.MySqlClient.MySqlCommand("Select morning, afternoon from attendance where term = '" & Session("SessionId") & "' and student = '" & "19/018" & "' and week = '4' order by id desc", con)
            Dim readref As MySql.Data.MySqlClient.MySqlDataReader = ref.ExecuteReader
            Do While readref.Read
                present = present - Val(readref.Item(0)) - Val(readref.Item(1))
            Loop
            readref.Close()

            Dim refs As New MySql.Data.MySqlClient.MySqlCommand("Select date from attendance where term = '" & Session("SessionId") & "' and class = '10' and week = '4'", con)
            Dim readrefs As MySql.Data.MySqlClient.MySqlDataReader = refs.ExecuteReader
            Dim dates As New ArrayList
            Do While readrefs.Read
                If Not dates.Contains(readrefs.Item(0)) Then
                    dates.Add(readrefs.Item(0))
                End If
            Loop
            readrefs.Close()
            opened = dates.Count * 2
            absent = opened - present
            Dim s As String
            For Each d In dates

                s += d
            Next
            Show_Alert(True, s)
            Exit Sub

            con.Close()
        End Using
    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        attendance()
        Exit Sub
        If check.Check_Subject(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try


            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                If Not IsPostBack Then
                    Session("NSessionId") = Session("SessionId")

                    If Session("classub") <> Nothing Then
                        Dim refs As New MySql.Data.MySqlClient.MySqlCommand("Select subjects.subject from classsubjects inner join subjects on classsubjects.subject = subjects.ID where classsubjects.teacher = '" & Session("StaffId") & "'", con)
                        Dim readrefs As MySql.Data.MySqlClient.MySqlDataReader = refs.ExecuteReader
                        cboSubject2.Items.Clear()
                        Dim subj As New ArrayList
                        cboSubject2.Items.Add("SELECT")
                        Do While readrefs.Read
                            If Not subj.Contains(readrefs(0)) Then
                                cboSubject2.Items.Add(readrefs.Item(0))
                                subj.Add(readrefs(0))
                            End If
                        Loop
                        readrefs.Close()
                        Dim cl As String
                        Dim cmdCheck200 As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject, class.class FROM classsubjects inner join subjects on subjects.id = classsubjects.subject inner join class on class.id = classsubjects.class where classsubjects.id = '" & Session("classub") & "'", con)
                        Dim reader200 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck200.ExecuteReader()
                        If reader200.Read() Then
                            cboSubject2.Text = reader200.Item(0)
                            cl = reader200.Item(1).ToString
                        End If
                        reader200.Close()


                        Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from classsubjects inner join class on classsubjects.class = class.id inner join subjects on classsubjects.subject = subjects.Id where subjects.subject = '" & cboSubject2.Text & "'", con)
                        Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                        cboClass2.Items.Clear()
                        Do While student0.Read
                            cboClass2.Items.Add(student0.Item(0))
                        Loop
                        student0.Close()
                        cboClass2.Text = cl
                        Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT week, topic, content from courseoutline inner join subjects on subjects.id = courseoutline.subject inner join session on session.id = courseoutline.session inner join class on class.id = courseoutline.class where subjects.subject = '" & cboSubject2.Text & "' and class.class = '" & cboClass2.Text & "' and courseoutline.session = '" & Session("NSessionId") & "' order by courseoutline.week asc", con)
                        Dim ds As New DataTable
                        ds.Columns.Add("Week")
                        ds.Columns.Add("Topic")
                        ds.Columns.Add("Content")

                        Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                        Do While reader.Read
                            ds.Rows.Add(reader.Item(0), reader.Item(1), reader.Item(2))
                        Loop
                        reader.Close()
                        GridView1.DataSource = ds
                        GridView1.DataBind()

                    Else
                        Dim ref As New MySql.Data.MySqlClient.MySqlCommand("Select subjects.subject from classsubjects inner join subjects on classsubjects.subject = subjects.ID where classsubjects.teacher = '" & Session("StaffId") & "'", con)
                        Dim readref As MySql.Data.MySqlClient.MySqlDataReader = ref.ExecuteReader
                        cboSubject2.Items.Clear()
                        Dim subj As New ArrayList
                        cboSubject2.Items.Add("SELECT")
                        Do While readref.Read
                            If Not subj.Contains(readref(0)) Then
                                cboSubject2.Items.Add(readref.Item(0))
                                subj.Add(readref(0))
                            End If
                        Loop
                        readref.Close()
                    End If




                End If
                con.Close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub



    Protected Sub GridView1_PageIndexChanged(sender As Object, e As EventArgs) Handles GridView1.PageIndexChanged

    End Sub

    Protected Sub GridView1_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GridView1.PageIndexChanging

        GridView1.PageIndex = e.NewPageIndex
        GridView1.DataBind()



    End Sub









    Protected Sub cboSubject2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSubject2.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from classsubjects inner join class on classsubjects.class = class.id inner join subjects on classsubjects.subject = subjects.Id where subjects.subject = '" & cboSubject2.Text & "' and classsubjects.teacher = '" & Session("staffid") & "'", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                cboClass2.Items.Clear()
                cboClass2.Items.Add("SELECT")
                Do While student0.Read
                    cboClass2.Items.Add(student0.Item(0))
                Loop
                student0.Close()
                GridView1.DataBind()
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub cboClass2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboClass2.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT week, topic, content from courseoutline inner join subjects on subjects.id = courseoutline.subject inner join session on session.id = courseoutline.session inner join class on class.id = courseoutline.class where subjects.subject = '" & cboSubject2.Text & "' and class.class = '" & cboClass2.Text & "' and courseoutline.session = '" & Session("NSessionId") & "' order by courseoutline.week asc", con)
                Dim ds As New DataTable
                ds.Columns.Add("Week")
                ds.Columns.Add("Topic")
                ds.Columns.Add("Content")

                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Do While reader.Read
                    ds.Rows.Add(reader.Item(0), reader.Item(1), reader.Item(2))
                Loop
                reader.Close()
                GridView1.DataSource = ds
                GridView1.DataBind()


                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("Select classsubjects.id from classsubjects inner join class on classsubjects.class = class.id inner join subjects on classsubjects.subject = subjects.Id where subjects.subject = '" & cboSubject2.Text & "' and class.class = '" & cboClass2.Text & "'", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                If student0.Read() Then Session("classub") = student0.Item(0)
                student0.Close()
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub



    Protected Sub lnknew_Click(sender As Object, e As EventArgs) Handles lnkNew.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("Select classsubjects.id from classsubjects inner join class on classsubjects.class = class.id inner join subjects on classsubjects.subject = subjects.Id where subjects.subject = '" & cboSubject2.Text & "' and class.class = '" & cboClass2.Text & "'", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                If student0.Read() Then Session("classub") = student0.Item(0)
                student0.Close()
                con.close()            End Using
            Response.Redirect("~/content/staff/newcourseoutline.aspx")
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub GridView1_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GridView1.RowDeleting

        Try
            Dim row As GridViewRow = GridView1.Rows(e.RowIndex)
            Dim week As String = row.Cells(0).Text
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim id As String
                Dim cmdCheck20 As New MySql.Data.MySqlClient.MySqlCommand("SELECT courseoutline.id FROM courseoutline inner join subjects on subjects.id = courseoutline.subject inner join session on session.id = courseoutline.session inner join class on class.id = courseoutline.class where courseoutline.week = '" & week & "' and subjects.subject = '" & cboSubject2.Text & "' and class.class = '" & cboClass2.Text & "'", con)
                Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck20.ExecuteReader()
                If reader20.Read() Then
                    id = reader20(0).ToString
                End If
                reader20.Close()
                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("delete from courseoutline where id = '" & id & "'", con)
                cmdLoad0.ExecuteNonQuery()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT week, topic, content from courseoutline inner join subjects on subjects.id = courseoutline.subject inner join session on session.id = courseoutline.session inner join class on class.id = courseoutline.class where subjects.subject = '" & cboSubject2.Text & "' and class.class = '" & cboClass2.Text & "' and courseoutline.session = '" & Session("NSessionId") & "' order by courseoutline.week asc", con)
                Dim ds As New DataTable
                ds.Columns.Add("Week")
                ds.Columns.Add("Topic")
                ds.Columns.Add("Content")

                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Do While reader.Read
                    ds.Rows.Add(reader.Item(0), reader.Item(1), reader.Item(2))
                Loop
                reader.Close()
                GridView1.DataSource = ds
                GridView1.DataBind()

                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
End Class
