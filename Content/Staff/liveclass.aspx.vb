Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Imports Google.Apis.Auth.OAuth2
Imports Google.Apis.Calendar.v3
Imports System.IO
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
        If check.Check_Subject(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try


            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT assignments.id, cast(assignments.date as char) as date, subjects.subject, class.class, assignments.title, assignments.deadline from assignments inner join subjects on subjects.Id = assignments.subject inner join class on class.id = assignments.class where assignments.teacher = '" & Session("StaffId") & "'  order by assignments.date desc", con)
                Dim ds As New DataTable
                ds.Columns.Add("Reference")
                ds.Columns.Add("Date")
                ds.Columns.Add("Subject")
                ds.Columns.Add("Class")
                ds.Columns.Add("Title")
                ds.Columns.Add("Deadline")

                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Do While reader.Read
                    ds.Rows.Add(reader.Item(0), Convert.ToDateTime(reader(1)).ToString("dd/MM/yyyy hh:mm tt"), reader.Item(2), reader.Item(3), reader.Item(4), reader.Item(5))
                Loop
                reader.Close()
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
                con.Close()
            End Using

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub


    Protected Sub GridView1_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GridView1.RowDeleting
        Try
            Dim row As GridViewRow = GridView1.Rows(e.RowIndex)
            Dim subject As String = row.Cells(2).Text
            Dim cla As String = row.Cells(3).Text
            Dim title As String = row.Cells(4).Text

            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT  id from class where class = ?", con)
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", cla))
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
                reader2.Read()
                Dim clid As Integer = reader2(0)
                reader2.Close()

                Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT  id from subjects where subject = ?", con)
                cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", subject))
                Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                reader20.Read()
                Dim subjid As Integer = reader20(0)
                reader20.Close()

                Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("Delete from assignments where class = ? and subject = ? and title = ? and session = '" & Session("sessionid") & "'", con)
                cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("accname", clid))
                cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("accna", subjid))
                cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("acame", title))
                cmd2.ExecuteNonQuery()
                Show_Alert(True, "Assignment removed")

                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT assignments.id, cast(assignments.date as char) as date, subjects.subject, class.class, assignments.title, assignments.deadline from assignments inner join subjects on subjects.Id = assignments.subject inner join class on class.id = assignments.class where assignments.teacher = '" & Session("StaffId") & "'  order by assignments.date desc", con)
                Dim ds As New DataTable
                ds.Columns.Add("Reference")
                ds.Columns.Add("Date")
                ds.Columns.Add("Subject")
                ds.Columns.Add("Class")
                ds.Columns.Add("Title")
                ds.Columns.Add("Deadline")

                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Do While reader.Read
                    ds.Rows.Add(reader.Item(0), Convert.ToDateTime(reader(1)).ToString("dd/MM/yyyy hh:mm tt"), reader.Item(2), reader.Item(3), reader.Item(4), reader.Item(5))
                Loop
                reader.Close()
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
                con.Close()
            End Using

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub



    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT assignments.id, cast(assignments.date as char) as date, subjects.subject, class.class, assignments.title, assignments.deadline from assignments inner join subjects on subjects.Id = assignments.subject inner join class on class.id = assignments.class where assignments.teacher = '" & Session("StaffId") & "'  order by assignments.date desc", con)
                Dim ds As New DataTable
                ds.Columns.Add("Reference")
                ds.Columns.Add("Date")
                ds.Columns.Add("Subject")
                ds.Columns.Add("Class")
                ds.Columns.Add("Title")
                ds.Columns.Add("Deadline")

                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Do While reader.Read
                    ds.Rows.Add(reader.Item(0), Convert.ToDateTime(reader(1)).ToString("dd/MM/yyyy hh:mm tt"), reader.Item(2), reader.Item(3), reader.Item(4), reader.Item(5))
                Loop
                reader.Close()
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

                con.Close()
            End Using


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
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT assignments.id, cast(assignments.date as char) as date, subjects.subject, class.class, assignments.title, assignments.deadline from assignments inner join subjects on subjects.Id = assignments.subject inner join class on class.id = assignments.class where assignments.teacher = '" & Session("StaffId") & "'  order by assignments.date desc", con)
                Dim ds As New DataTable
                ds.Columns.Add("Reference")
                ds.Columns.Add("Date")
                ds.Columns.Add("Subject")
                ds.Columns.Add("Class")
                ds.Columns.Add("Title")
                ds.Columns.Add("Deadline")

                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Do While reader.Read
                    ds.Rows.Add(reader.Item(0), Convert.ToDateTime(reader(1)).ToString("dd/MM/yyyy hh:mm tt"), reader.Item(2), reader.Item(3), reader.Item(4), reader.Item(5))
                Loop
                reader.Close()

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

                con.Close()
            End Using


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
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT assignments.id, cast(assignments.date as char) as date, subjects.subject, class.class, assignments.title, assignments.deadline from assignments inner join subjects on subjects.Id = assignments.subject inner join class on class.id = assignments.class where assignments.teacher = '" & Session("StaffId") & "'  order by assignments.date desc", con)
                Dim ds As New DataTable
                ds.Columns.Add("Reference")
                ds.Columns.Add("Date")
                ds.Columns.Add("Subject")
                ds.Columns.Add("Class")
                ds.Columns.Add("Title")
                ds.Columns.Add("Deadline")

                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Do While reader.Read
                    ds.Rows.Add(reader.Item(0), Convert.ToDateTime(reader(1)).ToString("dd/MM/yyyy hh:mm tt"), reader.Item(2), reader.Item(3), reader.Item(4), reader.Item(5))
                Loop
                reader.Close()
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

                con.Close()
            End Using


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

    Protected Sub btnMsg_Click(sender As Object, e As EventArgs) Handles btnMsg.Click
        Dim s As ServiceAccountCredential
        Dim f As New FileStream("client_secret.json", FileMode.Open, FileAccess.Read)
        s = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(f).Secrets, cal
    End Sub

    Protected Sub lnkSubmitted_Click(sender As Object, e As EventArgs) Handles lnkSubmitted.Click
        Response.Redirect("~/content/Staff/submittedassignments.aspx")
    End Sub

    Protected Sub btnCbt_Click(sender As Object, e As EventArgs) Handles btnCbt.Click
        Response.Redirect("~/content/Staff/testlist.aspx")
    End Sub
End Class
