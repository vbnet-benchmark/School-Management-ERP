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
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()


                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ttname.name, depts.dept, class.class, ttname.default, ttname.id, ttname.school, ttname.class from ttname left join depts on ttname.school = depts.id left join class on ttname.class = class.id", con)
                Dim ds As New DataTable
                ds.Columns.Add("name")
                ds.Columns.Add("default")
                ds.Columns.Add("id")
                ds.Columns.Add("school")
                ds.Columns.Add("class")
                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Do While reader.Read

                    If reader(5) <> 0 Then
                        ds.Rows.Add(reader.Item(0) & " - " & reader.Item(1), IIf(reader(3) = 1, True, False), reader.Item(4), reader(5), reader(6))
                    Else
                        ds.Rows.Add(reader.Item(0) & " - " & reader.Item(2), IIf(reader(3) = 1, True, False), reader.Item(4), reader(5), reader(6))
                    End If

                Loop
                reader.Close()
                GridView1.DataSource = ds
                GridView1.DataBind()
                con.Close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub


    Protected Sub GridView1_PageIndexChanged(sender As Object, e As EventArgs) Handles GridView1.PageIndexChanged

    End Sub

    Protected Sub GridView1_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()



                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ttname.name, depts.dept, class.class, ttname.default, ttname.id, ttname.school, ttname.class from ttname left join depts on ttname.school = depts.id left join class on ttname.class = class.id", con)
                Dim ds As New DataTable
                ds.Columns.Add("name")
                ds.Columns.Add("default")
                ds.Columns.Add("id")
                ds.Columns.Add("school")
                ds.Columns.Add("class")
                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Do While reader.Read

                    If reader(5) <> 0 Then
                        ds.Rows.Add(reader.Item(0) & " - " & reader.Item(1), IIf(reader(3) = 1, True, False), reader.Item(4), reader(5), reader(6))
                    Else
                        ds.Rows.Add(reader.Item(0) & " - " & reader.Item(2), IIf(reader(3) = 1, True, False), reader.Item(4), reader(5), reader(6))
                    End If

                Loop
                reader.Close()
                GridView1.DataSource = ds
                GridView1.DataBind()
                con.close()            End Using

            GridView1.PageIndex = e.NewPageIndex
            GridView1.DataBind()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try


    End Sub

    Protected Sub GridView1_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub GridView1_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GridView1.RowDeleting
        Try
            Dim row As GridViewRow = GridView1.Rows(e.RowIndex)
            Dim a As Array = Split(row.Cells(0).Text, " - ")
            Dim name As String = a(0)
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdf0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from ttname Where name = ?", con)
                cmdf0.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", name))
                Dim readerf0 As MySql.Data.MySqlClient.MySqlDataReader = cmdf0.ExecuteReader
                readerf0.Read()
                Dim sid As Integer = readerf0.Item(0)
                readerf0.Close()

                Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("delete from tperiods where timetable = ?", con)
                cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", sid))
                cmd3.ExecuteNonQuery()
                Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("delete from ttname Where id = '" & sid & "'", con)
                cmd10.ExecuteNonQuery()
                Show_Alert(True, "Time table deleted successfully")
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ttname.name, depts.dept, class.class, ttname.default, ttname.id, ttname.school, ttname.class from ttname left join depts on ttname.school = depts.id left join class on ttname.class = class.id", con)
                Dim ds As New DataTable
                ds.Columns.Add("name")
                ds.Columns.Add("default")
                ds.Columns.Add("id")
                ds.Columns.Add("school")
                ds.Columns.Add("class")
                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Do While reader.Read

                    If reader(5) <> 0 Then
                        ds.Rows.Add(reader.Item(0) & " - " & reader.Item(1), IIf(reader(3) = 1, True, False), reader.Item(4), reader(5), reader(6))
                    Else
                        ds.Rows.Add(reader.Item(0) & " - " & reader.Item(2), IIf(reader(3) = 1, True, False), reader.Item(4), reader(5), reader(6))
                    End If

                Loop
                reader.Close()
                GridView1.DataSource = ds
                GridView1.DataBind()
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub GridView1_SelectedIndexChanged1(sender As Object, e As EventArgs) Handles GridView1.SelectedIndexChanged

    End Sub

    Protected Sub GridView1_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs)

    End Sub

    

    

    Protected Sub lnkSubmitted_Click(sender As Object, e As EventArgs) Handles lnknEW.Click
        Response.Redirect("~/content/App/Admin/newtimetable.aspx")

    End Sub
End Class
