Imports System.Text
Imports System.Web.Services
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Imports System.IO
Partial Class Admin_adminpage
    Inherits System.Web.UI.Page
    Public Shared receiver As String
    Public Shared recCategory As String

    Dim alert As New Literal
    Dim alertmsg As New Alerts
    Dim logify As New notify
    Dim check As New CheckUser
    Dim alertPLC As New PlaceHolder
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
        If Session("staffid") = Nothing Then            Dim x As ArrayList = check.Check_app_staff(Request.QueryString("username"), Request.QueryString("password"))            If x.Count <> 0 Then                Session("staffid") = x.Item(0)                Session("sessionid") = x.Item(1)            End If        End If
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim a As New DataTable
                a.Columns.Add("S/N")
                a.Columns.Add("Staff Id")
                a.Columns.Add("Staff Name")
                a.Columns.Add("Status")

                Dim serial As Integer = 1
                If Request.QueryString.ToString <> Nothing Then
                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select staffprofile.surname, staffprofile.staffId, depts.head, headtitle, depts.dept from staffdept inner join staffprofile on staffdept.staff = staffprofile.staffid inner join depts on depts.id = staffdept.dept where depts.id = '" & Request.QueryString.ToString & "'", con)
                    Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                    Do While msg.Read()
                        If msg.Item(1) = msg.Item(2) Then
                            a.Rows.Add(serial, msg.Item(1), msg.Item(0), msg.Item(3))
                        Else
                            a.Rows.Add(serial, msg.Item(1), msg.Item(0), "Member")
                        End If
                        serial = serial + 1

                    Loop
                    msg.Close()
                End If
                GridView1.DataSource = a

                GridView1.DataBind()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dept, superior from depts where id = '" & Request.QueryString.ToString & "'", con)
                Dim msg2 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                msg2.Read()
                If msg2(1).ToString = "None" Then
                    label1.Text = msg2.Item(0).ToString & " MEMBERS"
                Else
                    label1.Text = msg2.Item(0).ToString & " - " & msg2.Item(1).ToString & " MEMBERS"
                End If

                msg2.Close()
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub


    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                If txtID.Text = "" Then
                    Show_Alert(False, "Please enter a Staff Id")
                Else
                    Dim id As String = ""
                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select staffid from staffprofile where staffid = '" & txtID.Text & "'", con)
                    Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                    If msg.Read Then
                        id = msg.Item(0)
                    End If
                    msg.Close()
                    If id = "" Then
                        Show_Alert(False, "Staff does not exist")
                    Else
                        Dim comfirm As New MySql.Data.MySqlClient.MySqlCommand("Select dept from depts where id = '" & Request.QueryString.ToString & "'", con)
                        Dim rcomfirm As MySql.Data.MySqlClient.MySqlDataReader = comfirm.ExecuteReader
                        rcomfirm.Read()
                        Dim dep As String = rcomfirm(0).ToString
                        rcomfirm.Close()
                        Dim cmdCheck2s As New MySql.Data.MySqlClient.MySqlCommand("Select * from staffdept where staff = '" & txtID.Text & "' and dept = '" & Request.QueryString.ToString & "'", con)
                        Dim msgs As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2s.ExecuteReader
                        If msgs.Read Then
                            Show_Alert(False, "Staff already in department.")
                            Exit Sub
                        End If
                        msgs.Close()
                        Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Insert into staffdept (staff, dept) values ('" & id & "', '" & Request.QueryString.ToString & "')", con)
                        cmdCheck.ExecuteNonQuery()
                        logify.log(Session("staffid"), id & " was added to " & dep & " depatment")
                        logify.Notifications("You are now a member of " & dep & " department", id, Session("staffid"), "Admin", "~/content/staff/staffprofile.aspx", "")
                        Show_Alert(True, "Staff added successfully")
                    End If
                End If
                Dim a As New DataTable
                a.Columns.Add("S/N")
                a.Columns.Add("Staff Id")
                a.Columns.Add("Staff Name")
                a.Columns.Add("Status")
                Dim serial As Integer = 1
                Dim cmdCheck22 As New MySql.Data.MySqlClient.MySqlCommand("Select staffprofile.surname, staffprofile.staffId, depts.head, headtitle from staffdept inner join staffprofile on staffdept.staff = staffprofile.staffid inner join depts on depts.id = staffdept.dept where depts.id = '" & Request.QueryString.ToString & "'", con)
                Dim msg2 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck22.ExecuteReader
                Do While msg2.Read()
                    If msg2.Item(1) = msg2.Item(2) Then
                        a.Rows.Add(serial, msg2.Item(1), msg2.Item(0), msg2.Item(3))
                    Else
                        a.Rows.Add(serial, msg2.Item(1), msg2.Item(0), "Member")
                    End If
                    serial = serial + 1
                Loop
                msg2.Close()
                GridView1.DataSource = a
                GridView1.DataBind()
                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub GridView1_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GridView1.RowDeleting
        Try
            Dim row As GridViewRow = GridView1.Rows(e.RowIndex)
            Dim sessions As String = row.Cells(1).Text
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
            Dim cmdCheck300 As New MySql.Data.MySqlClient.MySqlCommand("Select * from depts where head = '" & sessions & "' and id = '" & Request.QueryString.ToString & "'", con)
            Dim msg20 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck300.ExecuteReader
            If msg20.Read Then
                Show_Alert(False, "You cannot remove the HOD as a member.")
                msg20.Close()
                Exit Sub
            End If
            msg20.Close()
            Dim comfirm As New MySql.Data.MySqlClient.MySqlCommand("Select dept from depts where id = '" & Request.QueryString.ToString & "'", con)
            Dim rcomfirm As MySql.Data.MySqlClient.MySqlDataReader = comfirm.ExecuteReader
            rcomfirm.Read()
            Dim dep As String = rcomfirm(0).ToString
            rcomfirm.Close()
            Dim enter As New MySql.Data.MySqlClient.MySqlCommand("delete from staffdept where staff = '" & sessions & "' and dept = '" & Request.QueryString.ToString & "'", con)
                enter.ExecuteNonQuery()
            GridView1.EditIndex = -1
            logify.log(Session("staffid"), ID & " was removed from " & dep & " depatment")
            logify.Notifications("You are no more a member of " & dep & " department", ID, Session("staffid"), "Admin", "~/content/staff/staffprofile.aspx", "")
            Show_Alert(True, "Staff removed successfully")
            Dim a As New DataTable
            a.Columns.Add("S/N")
            a.Columns.Add("Staff Id")
            a.Columns.Add("Staff Name")
            a.Columns.Add("Status")
            Dim serial As Integer = 1
            Dim cmdCheck22 As New MySql.Data.MySqlClient.MySqlCommand("Select staffprofile.surname, staffprofile.staffId, depts.head, headtitle from staffdept inner join staffprofile on staffdept.staff = staffprofile.staffid inner join depts on depts.id = staffdept.dept where depts.id = '" & Request.QueryString.ToString & "'", con)
            Dim msg2 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck22.ExecuteReader
            Do While msg2.Read()
                If msg2.Item(1) = msg2.Item(2) Then
                    a.Rows.Add(serial, msg2.Item(1), msg2.Item(0), msg2.Item(3))
                Else
                    a.Rows.Add(serial, msg2.Item(1), msg2.Item(0), "Member")
                End If
                serial = serial + 1
            Loop
            msg2.Close()
            GridView1.DataSource = a
                GridView1.DataBind()
                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Unnamed1_Click(sender As Object, e As EventArgs) Handles lnkBack.Click
        Session("currentroute") = Request.QueryString.ToString
        Response.Redirect("~/content/App/Admin/Departments.aspx")
    End Sub
End Class
