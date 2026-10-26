Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Admin_newsession
    Inherits System.Web.UI.Page
    Dim alert As New Literal
    Dim alertmsg As New Alerts
    Dim logify As New notify
    Dim alertPLC As New PlaceHolder
    Dim check As New CheckUser



    Private Sub Show_Alert(type As Boolean, msg As String)
        alertPLC = Me.Master.FindControl("plcAlert")
        If type = True Then
            alert.Text = alertmsg.success_message(msg)
        Else
            alert.Text = alertmsg.error_message(msg)
        End If
        alertPLC.Controls.Add(alert)
    End Sub
    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Try

            If txtID.Text = "" Then
                Show_Alert(False, "Please enter a Session Name.")
                Exit Sub
            End If
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Sessioncreate where sessionname = ?", con)
                cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", txtID.Text))
                Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                If reader20.Read Then
                    reader20.Close()
                    Show_Alert(False, "Session already exists.")
                Else
                    reader20.Close()
                    Dim enter As New MySql.Data.MySqlClient.MySqlCommand("Insert Into Sessioncreate (Sessionname) Values (?)", con)
                    enter.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", txtID.Text))
                    enter.ExecuteNonQuery()
                    logify.log(Session("StaffId"), "A new session was created name = " & txtID.Text)
                    Show_Alert(True, "Session added successfully")
                    Dim a As New DataTable
                    a.Columns.Add("ID")
                    a.Columns.Add("Session")
                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select ID, sessionname from sessioncreate order by ID", con)
                    Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                    Do While msg.Read()
                        a.Rows.Add(msg.Item(0), msg.Item(1))
                    Loop
                    Gridview1.DataSource = a

                    Gridview1.DataBind()
                    msg.Close()
                End If
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))

        End Try
    End Sub

    Protected Sub GridView1_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles Gridview1.RowCancelingEdit
        GridView1.EditIndex = -1
    End Sub

    Protected Sub Gridview1_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles Gridview1.RowDeleting
        Try
            Dim row As GridViewRow = Gridview1.Rows(e.RowIndex)
            Dim sessions As String = row.Cells(1).Text

            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdCheck2a As New MySql.Data.MySqlClient.MySqlCommand("Select ID from session where session = '" & sessions & "'", con)
                Dim msga As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2a.ExecuteReader
                If msga.Read Then
                Else
                    msga.Close()
                    Dim enter As New MySql.Data.MySqlClient.MySqlCommand("delete from Sessioncreate where sessionname = '" & sessions & "'", con)
                    enter.ExecuteNonQuery()
                End If
                msga.Close()

                Show_Alert(True, "Session deleted successfully.")
                logify.log(Session("staffid"), "Session - " & txtID.Text & " was deleted.")

                Gridview1.EditIndex = -1
                Dim a As New DataTable
                a.Columns.Add("ID")
                a.Columns.Add("Session")
                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select ID, sessionname from sessioncreate order by ID", con)
                Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                Do While msg.Read()
                    a.Rows.Add(msg.Item(0), msg.Item(1))
                Loop
                Gridview1.DataSource = a
                Gridview1.Columns(0).Visible = False
                Gridview1.DataBind()
                msg.Close()
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))

        End Try
    End Sub

    Protected Sub GridView1_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles Gridview1.RowEditing
        Try
            Gridview1.EditIndex = e.NewEditIndex
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim a As New DataTable
                a.Columns.Add("ID")
                a.Columns.Add("Session")
                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select ID, sessionname from sessioncreate order by ID", con)
                Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                Do While msg.Read()
                    a.Rows.Add(msg.Item(0), msg.Item(1))
                Loop
                Gridview1.DataSource = a
                Gridview1.Columns(0).Visible = True
                Gridview1.DataBind()
                msg.Close()
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub GridView1_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles Gridview1.RowUpdating
        Try
            Dim row As GridViewRow = Gridview1.Rows(e.RowIndex)
            Dim ID As String = row.Cells(0).Text
            Dim sessions As String = TryCast(row.Cells(1).Controls(0), TextBox).Text

            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim enter As New MySql.Data.MySqlClient.MySqlCommand("Update Sessioncreate set sessionname = '" & sessions & "' where ID = '" & ID & "'", con)
                enter.ExecuteNonQuery()
                Gridview1.EditIndex = -1
                Show_Alert(True, "Session updated successfully.")
                logify.log(Session("staffid"), "Session - ID =  " & ID & " was updated.")

                Dim a As New DataTable
                a.Columns.Add("ID")
                a.Columns.Add("Session")
                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select ID, sessionname from sessioncreate order by ID", con)
                Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                Do While msg.Read()
                    a.Rows.Add(msg.Item(0), msg.Item(1))
                Loop
                Gridview1.DataSource = a
                Gridview1.Columns(0).Visible = False
                Gridview1.DataBind()
                msg.Close()
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))

        End Try
    End Sub


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("staffid") = Nothing Then            Dim x As ArrayList = check.Check_app_staff(Request.QueryString("username"), Request.QueryString("password"))            If x.Count <> 0 Then                Session("staffid") = x.Item(0)                Session("sessionid") = x.Item(1)            End If        End If

        Try
            If Not IsPostBack Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.open()
                    Dim a As New DataTable
                    a.Columns.Add("ID")
                    a.Columns.Add("Session")
                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select ID, sessionname from sessioncreate order by ID", con)
                    Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                    Do While msg.Read()
                        a.Rows.Add(msg.Item(0), msg.Item(1))
                    Loop
                    Gridview1.DataSource = a

                    Gridview1.DataBind()
                    msg.Close()
                    con.close()                End Using
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))

        End Try
    End Sub

    
   
    Protected Sub linkbterm_Click(sender As Object, e As EventArgs) Handles linkbterm.Click
        Response.Redirect("~/content/App/Admin/newterm.aspx")

    End Sub
End Class
 