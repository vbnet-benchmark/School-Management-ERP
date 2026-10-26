Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Account_Default
    Inherits System.Web.UI.Page

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
        If Session("staffid") = Nothing Then            Dim x As ArrayList = check.Check_app_staff(Request.QueryString("username"), Request.QueryString("password"))            If x.Count <> 0 Then                Session("staffid") = x.Item(0)                Session("sessionid") = x.Item(1)            End If        End If
        Try


            If Not IsPostBack Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.Open()

                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT item from stock", con)
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    cboDAccount.Items.Clear()
                    cboCAccount.Items.Clear()
                    cboDAccount.Items.Add("Select Stock")
                    cboCAccount.Items.Add("Select Event")
                    cboCAccount.Items.Add("Addition")
                    cboCAccount.Items.Add("Removal")
                    Do While student0.Read
                        cboDAccount.Items.Add(student0.Item(0))

                    Loop

                    student0.Close()
                    con.Close()                End Using

            Else

            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub


    Protected Sub lblPost_Click(sender As Object, e As EventArgs) Handles lblPost.Click
        Try
            If cboCAccount.Text = "Select Event" Or
                cboDAccount.Text = "Select Stock" Or
                cboDAccount.Text = "" Or
                cboDDetails.Text = "" Then
                Show_Alert(False, "Please enter all details.")
                Exit Sub
            End If
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()

                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id from stock where item = '" & cboDAccount.Text & "'", con)
                cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("accname", cboDAccount.Text))
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                student0.Read()
                Dim id As Integer = student0.Item("id")
                student0.Close()



                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into stockadjust (date, item, added, removed, details) Values (?,?,?,?,?)", con)
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", id))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", IIf(cboCAccount.Text = "Addition", cboDAmount.Text, "")))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", IIf(cboCAccount.Text = "Removal", cboDAmount.Text, "")))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", cboDDetails.Text))
                cmdCheck2.ExecuteNonQuery()


                Show_Alert(True, "Stock adjusted")
                con.close()            End Using
            cboCAccount.Text = "Select Event"
            cboDAccount.Text = "Select Stock"
            cboDAmount.Text = ""
            cboDDetails.Text = ""
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

  
    Protected Sub lnkBack_Click(sender As Object, e As EventArgs) Handles lnkBack.Click
        Response.Redirect("~/content/App/Admin/stock.aspx")

    End Sub
End Class
