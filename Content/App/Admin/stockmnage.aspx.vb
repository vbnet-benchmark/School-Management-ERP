Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Account_salary
    Inherits System.Web.UI.Page
    Dim feeremove As String
    Dim feeremove2 As String
    Dim feeremove3 As String
    Dim feeremove4 As String
    Dim feeremove5 As String
    Dim feeremove6 As String
    Dim feeremove7 As String
    Dim feeremove8 As String

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


            If Not IsPostBack Then
                Load_Accounts()
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Protected Sub Gridview2_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles Gridview2.RowDeleting
        Try
            Dim row As GridViewRow = Gridview2.Rows(e.RowIndex)
            Dim account As String = row.Cells(0).Text
           
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT  * from stockadjust inner join stock on stock.id = stockadjust.item where stock.item = ?", con)
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", account))
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
                If Not reader2.HasRows Then
                    reader2.Close()
                    Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("Delete from stock where item = ?", con)
                    cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("accname", account))
                    cmd2.ExecuteNonQuery()
                    Show_Alert(True, "Stock Item removed")
                Else
                    reader2.Close()
                    Show_Alert(False, "There are adjustments associated with this stock item. It cannot be deleted.")
                End If
                con.close()            End Using
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Protected Sub Gridview2_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles Gridview2.RowEditing
        Try
            Gridview2.EditIndex = e.NewEditIndex
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview2_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles Gridview2.RowUpdating
        Try
            Dim row As GridViewRow = Gridview2.Rows(e.RowIndex)
            Dim ID As String = row.Cells(0).Text
            Dim sessions As String = TryCast(row.Cells(1).Controls(0), TextBox).Text.Replace(",", "")

            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim enter As New MySql.Data.MySqlClient.MySqlCommand("Update stock set quantity = '" & sessions & "' where item = '" & ID & "'", con)
                enter.ExecuteNonQuery()
                Gridview2.EditIndex = -1
                con.close()            End Using
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Private Sub Load_Accounts()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from stock", con)
            cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "cash"))
            Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
            Dim ds As New DataTable
            ds.Columns.Add("acc")
            ds.Columns.Add("init")
            Do While reader2.Read
                ds.Rows.Add(reader2.Item(1).ToString, reader2.Item(2).ToString)
            Loop
            reader2.Close()
            Gridview2.DataSource = ds
            Gridview2.DataBind()


            con.close()        End Using
    End Sub



    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            If TextBox1.Text = "" Then
                Show_Alert(False, "Please enter a stock name")
                Exit Sub
            End If
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim stock As String = "stock"
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("Insert into stock (item, quantity) values('" & TextBox1.Text & "', '" & Val(Textbox2.Text) & "')", con)
                cmd1.ExecuteNonQuery()
                Show_Alert(True, "Account added")
                logify.log(Session("staffid"), "A stock item - " & TextBox1.Text & " was added")
                pnlStock.Visible = False
                TextBox1.Text = ""
                Textbox2.Text = ""
                con.close()            End Using
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub



    Protected Sub lnkStock_Click(sender As Object, e As EventArgs) Handles lnkStock.Click
        pnlStock.Visible = True
    End Sub

    Protected Sub lnkBack_Click(sender As Object, e As EventArgs) Handles lnkBack.Click
        Response.Redirect("~/content/App/Admin/stock.aspx")
    End Sub
End Class
