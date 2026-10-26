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
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If check.Check_Student(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try

        
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim ref As New ArrayList
                Dim dates As New ArrayList
                Dim subject As New ArrayList
                Dim title As New ArrayList
                Dim deadline As New ArrayList

                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT assignments.id, assignments.date, subjects.subject, assignments.title, assignments.deadline from assignments inner join subjects on subjects.Id = assignments.subject inner join class on class.id = assignments.class where assignments.class = '" & Session("ClassId") & "'  order by assignments.date desc", con)
                Dim ds As New DataTable
                ds.Columns.Add("Reference")
                ds.Columns.Add("Date")
                ds.Columns.Add("Subject")
                ds.Columns.Add("Title")
                ds.Columns.Add("Deadline")
                ds.Columns.Add("status")
                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Do While reader.Read
                    ref.Add(reader.Item(0))
                    dates.Add(reader.Item(1))
                    subject.Add(reader.Item(2))
                    title.Add(reader.Item(3))
                    deadline.Add(reader.Item(4))
                Loop
                reader.Close()
                Dim count As Integer = 0
                Dim status As New ArrayList
                For Each item As Integer In ref
                    Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from submittedassignments where assignment = '" & item & "' and student = '" & Session("studentid") & "'", con)
                    Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                    If reader2.HasRows Then
                        ds.Rows.Add(item, Convert.ToDateTime(dates(count)).ToString("dd/MM/yyyy hh:mm tt"), subject(count), title(count), deadline(count), "Done")
                    Else
                        ds.Rows.Add(item, Convert.ToDateTime(dates(count)).ToString("dd/MM/yyyy hh:mm tt"), subject(count), title(count), deadline(count), "Undone")


                    End If
                    reader2.Close()
                    count = count + 1
                Next

                GridView1.DataSource = ds
                GridView1.DataBind()
                If GridView1.PageIndex = GridView1.PageCount - 1 Then
                    btnNext.Visible = False
                Else
                    btnNext.Visible = True
                End If
                If GridView1.PageIndex = 0 Then
                    btnPrevious.Visible = False
                Else
                    btnPrevious.Visible = True
                End If
                If ds.Rows.Count = 0 Then btnNext.Visible = False
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim ref As New ArrayList
                Dim dates As New ArrayList
                Dim subject As New ArrayList
                Dim title As New ArrayList
                Dim deadline As New ArrayList

                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT assignments.id, assignments.date, subjects.subject, assignments.title, assignments.deadline from assignments inner join subjects on subjects.Id = assignments.subject inner join class on class.id = assignments.class where assignments.class = '" & Session("ClassId") & "'  order by assignments.date desc", con)
                Dim ds As New DataTable
                ds.Columns.Add("Reference")
                ds.Columns.Add("Date")
                ds.Columns.Add("Subject")
                ds.Columns.Add("Title")
                ds.Columns.Add("Deadline")
                ds.Columns.Add("status")
                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Do While reader.Read
                    ref.Add(reader.Item(0))
                    dates.Add(reader.Item(1))
                    subject.Add(reader.Item(2))
                    title.Add(reader.Item(3))
                    deadline.Add(reader.Item(4))
                Loop
                reader.Close()
                Dim count As Integer = 0
                Dim status As New ArrayList
                For Each item As Integer In ref
                    Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from submittedassignments where assignment = '" & item & "' and student = '" & Session("studentid") & "'", con)
                    Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                    If reader2.HasRows Then
                        ds.Rows.Add(item, Convert.ToDateTime(dates(count)).ToString("dd/MM/yyyy hh:mm tt"), subject(count), title(count), deadline(count), "Done")
                    Else
                        ds.Rows.Add(item, Convert.ToDateTime(dates(count)).ToString("dd/MM/yyyy hh:mm tt"), subject(count), title(count), deadline(count), "Undone")


                    End If
                    reader2.Close()
                    count = count + 1
                Next

                GridView1.DataSource = ds
                GridView1.DataBind()
                If GridView1.PageIndex = GridView1.PageCount - 1 Then
                    btnNext.Visible = False
                Else
                    btnNext.Visible = True
                End If
                If GridView1.PageIndex = 0 Then
                    btnPrevious.Visible = False
                Else
                    btnPrevious.Visible = True
                End If

                con.close()            End Using


            If GridView1.PageIndex + 1 <= GridView1.PageCount Then
                GridView1.PageIndex = GridView1.PageIndex + 1
                GridView1.DataBind()
                If GridView1.PageIndex = GridView1.PageCount - 1 Then
                    btnNext.Visible = False
                Else
                    btnNext.Visible = True
                End If
                If GridView1.PageIndex = 0 Then
                    btnPrevious.Visible = False
                Else
                    btnPrevious.Visible = True
                End If
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub GridView1_PageIndexChanged(sender As Object, e As EventArgs) Handles GridView1.PageIndexChanged

    End Sub

    Protected Sub GridView1_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim ref As New ArrayList
                Dim dates As New ArrayList
                Dim subject As New ArrayList
                Dim title As New ArrayList
                Dim deadline As New ArrayList

                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT assignments.id, assignments.date, subjects.subject, assignments.title, assignments.deadline from assignments inner join subjects on subjects.Id = assignments.subject inner join class on class.id = assignments.class where assignments.class = '" & Session("ClassId") & "'  order by assignments.date desc", con)
                Dim ds As New DataTable
                ds.Columns.Add("Reference")
                ds.Columns.Add("Date")
                ds.Columns.Add("Subject")
                ds.Columns.Add("Title")
                ds.Columns.Add("Deadline")
                ds.Columns.Add("status")
                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Do While reader.Read
                    ref.Add(reader.Item(0))
                    dates.Add(reader.Item(1))
                    subject.Add(reader.Item(2))
                    title.Add(reader.Item(3))
                    deadline.Add(reader.Item(4))
                Loop
                reader.Close()
                Dim count As Integer = 0
                Dim status As New ArrayList
                For Each item As Integer In ref
                    Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from submittedassignments where assignment = '" & item & "' and student = '" & Session("studentid") & "'", con)
                    Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                    If reader2.HasRows Then
                        ds.Rows.Add(item, Convert.ToDateTime(dates(count)).ToString("dd/MM/yyyy hh:mm tt"), subject(count), title(count), deadline(count), "Done")
                    Else
                        ds.Rows.Add(item, Convert.ToDateTime(dates(count)).ToString("dd/MM/yyyy hh:mm tt"), subject(count), title(count), deadline(count), "Undone")


                    End If
                    reader2.Close()
                    count = count + 1
                Next

                GridView1.DataSource = ds
                GridView1.DataBind()
                If GridView1.PageIndex = GridView1.PageCount - 1 Then
                    btnNext.Visible = False
                Else
                    btnNext.Visible = True
                End If
                If GridView1.PageIndex = 0 Then
                    btnPrevious.Visible = False
                Else
                    btnPrevious.Visible = True
                End If

                con.close()            End Using


            GridView1.PageIndex = e.NewPageIndex
            GridView1.DataBind()

            If GridView1.PageIndex = GridView1.PageCount - 1 Then
                btnNext.Visible = False
            Else
                btnNext.Visible = True
            End If
            If GridView1.PageIndex = 0 Then
                btnPrevious.Visible = False
            Else
                btnPrevious.Visible = True
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub GridView1_SelectedIndexChanged1(sender As Object, e As EventArgs) Handles GridView1.SelectedIndexChanged

    End Sub

    Protected Sub GridView1_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs)

    End Sub



    Protected Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim ref As New ArrayList
                Dim dates As New ArrayList
                Dim subject As New ArrayList
                Dim title As New ArrayList
                Dim deadline As New ArrayList

                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT assignments.id, assignments.date, subjects.subject, assignments.title, assignments.deadline from assignments inner join subjects on subjects.Id = assignments.subject inner join class on class.id = assignments.class where assignments.class = '" & Session("ClassId") & "'  order by assignments.date desc", con)
                Dim ds As New DataTable
                ds.Columns.Add("Reference")
                ds.Columns.Add("Date")
                ds.Columns.Add("Subject")
                ds.Columns.Add("Title")
                ds.Columns.Add("Deadline")
                ds.Columns.Add("status")
                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Do While reader.Read
                    ref.Add(reader.Item(0))
                    dates.Add(reader.Item(1))
                    subject.Add(reader.Item(2))
                    title.Add(reader.Item(3))
                    deadline.Add(reader.Item(4))
                Loop
                reader.Close()
                Dim count As Integer = 0
                Dim status As New ArrayList
                For Each item As Integer In ref
                    Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from submittedassignments where assignment = '" & item & "' and student = '" & Session("studentid") & "'", con)
                    Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                    If reader2.HasRows Then
                        ds.Rows.Add(item, Convert.ToDateTime(dates(count)).ToString("dd/MM/yyyy hh:mm tt"), subject(count), title(count), deadline(count), "Done")
                    Else
                        ds.Rows.Add(item, Convert.ToDateTime(dates(count)).ToString("dd/MM/yyyy hh:mm tt"), subject(count), title(count), deadline(count), "Undone")


                    End If
                    reader2.Close()
                    count = count + 1
                Next

                GridView1.DataSource = ds
                GridView1.DataBind()
                If GridView1.PageIndex = GridView1.PageCount - 1 Then
                    btnNext.Visible = False
                Else
                    btnNext.Visible = True
                End If
                If GridView1.PageIndex = 0 Then
                    btnPrevious.Visible = False
                Else
                    btnPrevious.Visible = True
                End If

                con.close()            End Using


            If GridView1.PageIndex - 1 >= 0 Then
                GridView1.PageIndex = GridView1.PageIndex - 1
                GridView1.DataBind()
                If GridView1.PageIndex = GridView1.PageCount - 1 Then
                    btnNext.Visible = False
                Else
                    btnNext.Visible = True
                End If
                If GridView1.PageIndex = 0 Then
                    btnPrevious.Visible = False
                Else
                    btnPrevious.Visible = True
                End If

            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub lnkSubmitted_Click(sender As Object, e As EventArgs) Handles lnkSubmitted.Click
        Response.Redirect("~/content/student/markedassignments.aspx")

    End Sub
End Class
