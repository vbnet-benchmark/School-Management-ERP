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

                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT accname from accsettings", con)
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    cboDAccount.Items.Clear()
                    cboCAccount.Items.Clear()
                    cboDAccount.Items.Add("Select Account")
                    cboCAccount.Items.Add("Select Account")
                    Do While student0.Read
                        cboDAccount.Items.Add(student0.Item(0))
                        cboCAccount.Items.Add(student0.Item(0))
                    Loop
                    cboDAccount.Items.Add("RETAINED EARNINGS")
                    cboCAccount.Items.Add("RETAINED EARNINGS")
                    student0.Close()
                    con.Close()                End Using
                If Request.QueryString.ToString <> Nothing Then
                    Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                        con.Open()
                        Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from transactions where ref = ?", con)
                        cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", Request.QueryString.ToString))
                        Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                        Do While student.Read
                            If student.Item("trtype").ToString = "credit" Or student.Item("trtype").ToString = "Credit" Then
                                cboCAccount.Text = student.Item("account")
                            Else
                                cboDAccount.Text = student.Item("account")

                            End If
                            If student("dr") = "" Then
                                cboDAmount.Text = student("cr")
                            Else
                                cboDAmount.Text = student("dr")
                            End If

                            cboDDetails.Text = student.Item("details")
                        Loop
                        student.Close()
                        con.Close()                    End Using
                    cboCAccount.Enabled = False
                    cboDAccount.Enabled = False
                    cboDAmount.Enabled = False
                    cboDDetails.Enabled = False
                    lblRef.Text = Request.QueryString.ToString
                    btnReverse.Visible = True
                    lblPost.Visible = False
                Else
                    lblPost.Visible = True
                    btnReverse.Visible = False

                    Generate_Ref()
                End If
            Else

            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Sub Generate_Ref()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim test2 As Boolean
            Dim f As New Random
            Dim ref2 As New MySql.Data.MySqlClient.MySqlCommand("Select ref from transactions", con)
            Dim readref2 As MySql.Data.MySqlClient.MySqlDataReader = ref2.ExecuteReader

            Dim refs2 As New ArrayList
            Do While readref2.Read
                refs2.Add(readref2.Item(0))
            Loop
            Dim d As Integer
            Do Until test2 = True
                d = f.Next(100000, 999999)
                If refs2.Contains(d) Then
                    test2 = False
                Else
                    test2 = True
                End If
            Loop
            readref2.Close()
            cboCAccount.Text = "Select Account"
            cboDAccount.Text = "Select Account"
            cboDAmount.Text = ""
            cboDDetails.Text = ""
            lblRef.Text = d
            con.Close()
        End Using
    End Sub
    Protected Sub lblPost_Click(sender As Object, e As EventArgs) Handles lblPost.Click
        Try
            If cboCAccount.Text = "Select Account" Or
                cboDAccount.Text = "Select Account" Or
                cboDAccount.Text = "" Or
                cboDDetails.Text = "" Then
                Show_Alert(False, "Please enter all transaction details.")
                Exit Sub
            End If
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim Dacctype As String
                If cboDAccount.Text = "RETAINED EARNINGS" Then
                    Dacctype = "Equity"
                Else
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT type from accsettings where accname = ?", con)
                    cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("accname", cboDAccount.Text))
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    student0.Read()
                    Dacctype = student0.Item("type")
                    student0.Close()
                End If


                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype) Values (?,?,?,?,?,?,?)", con)
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", lblRef.Text))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", Dacctype))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(cboDAmount.Text.Replace(",", ""), , , TriState.True)))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", cboDAccount.Text))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", cboDDetails.Text))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                cmdCheck2.ExecuteNonQuery()

                Dim Cacctype As String
                If cboCAccount.Text = "RETAINED EARNINGS" Then
                    Cacctype = "Equity"
                Else
                    Dim cmdLoad2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT type from accsettings where accname = ?", con)
                    cmdLoad2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("accname", cboCAccount.Text))
                    Dim student2 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad2.ExecuteReader
                    student2.Read()
                    Cacctype = student2.Item("type")
                    student2.Close()
                End If
                Dim cmdCheck1 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype) Values (?,?,?,?,?,?,?)", con)
                cmdCheck1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", lblRef.Text))
                cmdCheck1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", Cacctype))
                cmdCheck1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(cboDAmount.Text.Replace(",", ""), , , TriState.True)))
                cmdCheck1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", cboCAccount.Text))
                cmdCheck1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", cboDDetails.Text))
                cmdCheck1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Credit"))
                cmdCheck1.ExecuteNonQuery()

                Show_Alert(True, "Transaction Enterred. Ref = " & lblRef.Text)
                con.close()            End Using
            Generate_Ref()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnReverse_Click(sender As Object, e As EventArgs) Handles btnReverse.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT type from accsettings where accname = ?", con)
                cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("accname", cboDAccount.Text))
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                student0.Read()
                Dim Dacctype As String = student0.Item("type")
                student0.Close()

                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype) Values (?,?,?,?,?,?,?)", con)
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", lblRef.Text & "*"))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", Dacctype))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(0 - Val(cboDAmount.Text.Replace(",", "")), , , TriState.True)))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", cboDAccount.Text))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", cboDDetails.Text & ". (Reversal)"))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                cmdCheck2.ExecuteNonQuery()

                Dim cmdLoad2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT type from accsettings where accname = ?", con)
                cmdLoad2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("accname", cboCAccount.Text))
                Dim student2 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad2.ExecuteReader
                student2.Read()
                Dim Cacctype As String = student2.Item("type")
                student2.Close()

                Dim cmdCheck1 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype) Values (?,?,?,?,?,?,?)", con)
                cmdCheck1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", lblRef.Text & "*"))
                cmdCheck1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", Cacctype))
                cmdCheck1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(0 - Val(cboDAmount.Text.Replace(",", "")), , , TriState.True)))
                cmdCheck1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", cboCAccount.Text))
                cmdCheck1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", cboDDetails.Text & ". (Reversal)"))
                cmdCheck1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Credit"))
                cmdCheck1.ExecuteNonQuery()

                Show_Alert(True, "Transaction Reversed. Ref = " & lblRef.Text & "*")
                cboCAccount.Enabled = True
                cboDAccount.Enabled = True
                cboDAmount.Enabled = True
                cboDDetails.Enabled = True
                lblPost.Visible = True
                btnReverse.Visible = False
                con.close()            End Using
            Generate_Ref()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub lnkBack_Click(sender As Object, e As EventArgs) Handles lnkBack.Click
        Response.Redirect("~/content/app/account/transactions.aspx")

    End Sub
End Class
