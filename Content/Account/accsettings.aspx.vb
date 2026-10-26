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
        If check.Check_Account(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try

       
        If Not IsPostBack Then
            Load_Accounts()
        End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Private Sub Load_Accounts()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from accsettings where type = ?", con)
            cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "cash"))
            Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
            Dim ds As New DataTable
            ds.Columns.Add("acc")
            ds.Columns.Add("init")
            Do While reader2.Read
                ds.Rows.Add(reader2.Item(1).ToString, FormatNumber(reader2.Item(3).ToString, , , , TriState.True))
            Loop
            reader2.Close()
            Gridview1.DataSource = ds
            Gridview1.DataBind()


            Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from accsettings where type = ?", con)
            cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "stock"))
            Dim reader22 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
            Dim ds2 As New DataTable
            ds2.Columns.Add("acc")
            ds2.Columns.Add("init")
            Do While reader22.Read
                ds2.Rows.Add(reader22.Item(1).ToString, FormatNumber(reader22.Item(3).ToString, , , , TriState.True))
            Loop
            reader22.Close()
            Gridview2.DataSource = ds2
            Gridview2.DataBind()


            Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from accsettings where type = ?", con)
            cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "receivable"))
            Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmd3.ExecuteReader
            Dim ds3 As New DataTable
            ds3.Columns.Add("acc")
            ds3.Columns.Add("init")
            Do While reader3.Read
                ds3.Rows.Add(reader3.Item(1).ToString, FormatNumber(reader3.Item(3).ToString, , , , TriState.True))
            Loop
            reader3.Close()
            Gridview3.DataSource = ds3
            Gridview3.DataBind()


            Dim cmd4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from accsettings where type = ?", con)
            cmd4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "fixed assets"))
            Dim reader4 As MySql.Data.MySqlClient.MySqlDataReader = cmd4.ExecuteReader
            Dim ds4 As New DataTable
            ds4.Columns.Add("acc")
            ds4.Columns.Add("init")
            Do While reader4.Read
                ds4.Rows.Add(reader4.Item(1).ToString, FormatNumber(reader4.Item(3).ToString, , , , TriState.True))
            Loop
            reader4.Close()
            Gridview4.DataSource = ds4
            Gridview4.DataBind()



            Dim cmd5 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from accsettings where type = ?", con)
            cmd5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "liability"))
            Dim reader5 As MySql.Data.MySqlClient.MySqlDataReader = cmd5.ExecuteReader
            Dim ds5 As New DataTable
            ds5.Columns.Add("acc")
            ds5.Columns.Add("init")
            Do While reader5.Read
                ds5.Rows.Add(reader5.Item(1).ToString, FormatNumber(reader5.Item(3).ToString, , , , TriState.True))
            Loop
            reader5.Close()
            Gridview5.DataSource = ds5
            Gridview5.DataBind()


            Dim cmd6 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from accsettings where type = ?", con)
            cmd6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "equity"))
            Dim reader6 As MySql.Data.MySqlClient.MySqlDataReader = cmd6.ExecuteReader
            Dim ds6 As New DataTable
            ds6.Columns.Add("acc")
            ds6.Columns.Add("init")
            Do While reader6.Read
                ds6.Rows.Add(reader6.Item(1).ToString, FormatNumber(reader6.Item(3).ToString, , , , TriState.True))
            Loop
            reader6.Close()
            Gridview6.DataSource = ds6
            Gridview6.DataBind()

            Dim cmd7 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from accsettings where type = ?", con)
            cmd7.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "expense"))
            Dim reader7 As MySql.Data.MySqlClient.MySqlDataReader = cmd7.ExecuteReader
            Dim ds7 As New DataTable
            ds7.Columns.Add("acc")
            ds7.Columns.Add("init")
            Do While reader7.Read
                ds7.Rows.Add(reader7.Item(1).ToString, FormatNumber(reader7.Item(3).ToString, , , , TriState.True))
            Loop
            reader7.Close()
            Gridview7.DataSource = ds7
            Gridview7.DataBind()


            Dim cmd8 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from accsettings where type = ?", con)
            cmd8.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "income"))
            Dim reader8 As MySql.Data.MySqlClient.MySqlDataReader = cmd8.ExecuteReader
            Dim ds8 As New DataTable
            ds8.Columns.Add("acc")
            ds8.Columns.Add("init")
            Do While reader8.Read
                ds8.Rows.Add(reader8.Item(1).ToString, FormatNumber(reader8.Item(3).ToString, , , , TriState.True))
            Loop
            reader8.Close()
            Gridview8.DataSource = ds8
            Gridview8.DataBind()

            con.close()        End Using
    End Sub
    Protected Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                If TextBox9.Text = "" Then
                    Show_Alert(False, "Please enter an account name")
                    Exit Sub
                End If
                Dim cash As String = "cash"
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("Insert into accsettings (accname, type, initial) values('" & TextBox9.Text & "', '" & cash & "', '" & Textbox10.Text.Replace(",", "") & "')", con)
                cmd1.ExecuteNonQuery()
                Show_Alert(True, "Account added")
                logify.log(Session("staffid"), "A cash account - " & TextBox9.Text & " was added")
                pnlCash.Visible = False
                TextBox9.Text = ""
                Textbox10.Text = ""
                con.close()            End Using
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub



    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            If TextBox1.Text = "" Then
                Show_Alert(False, "Please enter an account name")
                Exit Sub
            End If
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim stock As String = "stock"
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("Insert into accsettings (accname, type, initial) values('" & TextBox1.Text & "', '" & stock & "', '" & Textbox2.Text.Replace(",", "") & "')", con)
                cmd1.ExecuteNonQuery()
                Show_Alert(True, "Account added")
                logify.log(Session("staffid"), "A stock account - " & TextBox1.Text & " was added")
                pnlStock.Visible = False
                TextBox1.Text = ""
                Textbox2.Text = ""
                con.close()            End Using
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Try
            If TextBox11.Text = "" Then
                Show_Alert(False, "Please enter an account name")
                Exit Sub
            End If
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim receivables As String = "receivable"
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("Insert into accsettings (accname, type, initial) values('" & TextBox11.Text & "', '" & receivables & "', '" & Textbox12.Text.Replace(",", "") & "')", con)
                cmd1.ExecuteNonQuery()
                con.close()            End Using
            TextBox11.Text = ""
            Textbox12.Text = ""
            Show_Alert(True, "Account added")
            logify.log(Session("staffid"), "A receivable account - " & TextBox11.Text & " was added")
            pnlreceivables.Visible = False
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub



    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            If TextBox3.Text = "" Then
                Show_Alert(False, "Please enter an account name")
                Exit Sub
            End If
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim fixed As String = "fixed assets"
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("Insert into accsettings (accname, type, initial) values('" & TextBox3.Text & "', '" & fixed & "', '" & Textbox4.Text.Replace(",", "") & "')", con)
                cmd1.ExecuteNonQuery()
                con.close()            End Using
            Show_Alert(True, "Account added")
            TextBox3.Text = ""
            Textbox4.Text = ""
            logify.log(Session("staffid"), "A fixed asset account - " & TextBox3.Text & " was added")
            pnlfixed.Visible = False
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub



    Protected Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                If TextBox5.Text = "" Then
                    Show_Alert(False, "Please enter an account name")
                    Exit Sub
                End If
                Dim liability As String = "liability"
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("Insert into accsettings (accname, type, initial) values('" & TextBox5.Text & "', '" & liability & "', '" & Textbox6.Text.Replace(",", "") & "')", con)
                cmd1.ExecuteNonQuery()
                con.close()            End Using
            TextBox5.Text = ""
            Textbox6.Text = ""
            Show_Alert(True, "Fee added")
            logify.log(Session("staffid"), "A liability account - " & TextBox5.Text & " was added")
            pnllLiable.Visible = False
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub



    Protected Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Try
            If TextBox7.Text = "" Then
                Show_Alert(False, "Please enter an account name")
                Exit Sub
            End If
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim equity As String = "equity"
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("Insert into accsettings (accname, type, initial) values('" & TextBox7.Text & "', '" & equity & "', '" & Textbox8.Text.Replace(",", "") & "')", con)
                cmd1.ExecuteNonQuery()
                con.close()            End Using
            TextBox7.Text = ""
            Textbox8.Text = ""
            Show_Alert(True, "Account added")
            logify.log(Session("staffid"), "An equity account - " & TextBox7.Text & " was added")
            pnlEquity.Visible = False
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub



    Protected Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        Try
            If TextBox13.Text = "" Then
                Show_Alert(False, "Please enter an account name")
                Exit Sub
            End If
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim expense As String = "expense"
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("Insert into accsettings (accname, type, initial) values('" & TextBox13.Text & "', '" & expense & "', '" & Textbox14.Text.Replace(",", "") & "')", con)
                cmd1.ExecuteNonQuery()
                con.close()            End Using
            TextBox13.Text = ""
            Textbox14.Text = ""
            Show_Alert(True, "Account added")
            logify.log(Session("staffid"), "An expense account - " & TextBox13.Text & " was added")
            pnlExpense.Visible = False
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub



    Protected Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        Try
            If TextBox15.Text = "" Then
                Show_Alert(False, "Please enter an account name")
                Exit Sub
            End If
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim fixed As String = "income"
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("Insert into accsettings (accname, type, initial) values('" & TextBox15.Text & "', '" & fixed & "', '" & Textbox16.Text.Replace(",", "") & "')", con)
                cmd1.ExecuteNonQuery()
                con.close()            End Using
            TextBox15.Text = ""
            Textbox16.Text = ""
            Show_Alert(True, "Account added")
            logify.log(Session("staffid"), "An income account - " & TextBox15.Text & " was added")
            pnlIncome.Visible = False
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub



    Protected Sub Gridview1_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles Gridview1.RowDeleting
        Try
            Dim row As GridViewRow = Gridview1.Rows(e.RowIndex)
            Dim account As String = row.Cells(0).Text
            If account = "BANK ACCOUNT" Or
               account = "ADVANCE FEE PAYMENT" Or
               account = "DEBTORS" Or
                   account = "PORTAL CHARGES" Or
                   account = "ALLOWANCES" Or
                   account = "SALARY DEDUCTIONS" Or
                   account = "SALARY EXPENSES" Then
                Show_Alert(False, "You cannot delete this account. You can only adjust the initial balance.")
                Exit Sub

            End If
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT  * from transactions where account = ?", con)
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", account))
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
                If Not reader2.HasRows Then
                    reader2.Close()
                    Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("Delete from accsettings where accname = ?", con)
                    cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("accname", account))
                    cmd2.ExecuteNonQuery()
                    Show_Alert(True, "Account removed")
                Else
                    reader2.Close()
                    Show_Alert(False, "There are transactions associated with this account. It cannot be deleted.")
                End If
                con.close()            End Using
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview2_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles Gridview2.RowDeleting
        Try
            Dim row As GridViewRow = Gridview2.Rows(e.RowIndex)
            Dim account As String = row.Cells(0).Text
            If account = "BANK ACCOUNT" Or
               account = "ADVANCE FEE PAYMENT" Or
               account = "DEBTORS" Or
                   account = "PORTAL CHARGES" Or
                   account = "ALLOWANCES" Or
                   account = "SALARY DEDUCTIONS" Or
                   account = "SALARY EXPENSES" Then
                Show_Alert(False, "You cannot delete this account. You can only adjust the initial balance.")
                Exit Sub

            End If
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT  * from transactions where account = ?", con)
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", account))
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
                If Not reader2.HasRows Then
                    reader2.Close()
                    Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("Delete from accsettings where accname = ?", con)
                    cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("accname", account))
                    cmd2.ExecuteNonQuery()
                    Show_Alert(True, "Account removed")
                Else
                    reader2.Close()
                    Show_Alert(False, "There are transactions associated with this account. It cannot be deleted.")
                End If
                con.close()            End Using
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview3_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles Gridview3.RowDeleting
        Try
            Dim row As GridViewRow = Gridview3.Rows(e.RowIndex)
            Dim account As String = row.Cells(0).Text
            If account = "BANK ACCOUNT" Or
               account = "ADVANCE FEE PAYMENT" Or
               account = "DEBTORS" Or
                   account = "PORTAL CHARGES" Or
                   account = "ALLOWANCES" Or
                   account = "SALARY DEDUCTIONS" Or
                   account = "SALARY EXPENSES" Then
                Show_Alert(False, "You cannot delete this account. You can only adjust the initial balance.")
                Exit Sub

            End If
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT  * from transactions where account = ?", con)
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", account))
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
                If Not reader2.HasRows Then
                    reader2.Close()
                    Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("Delete from accsettings where accname = ?", con)
                    cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("accname", account))
                    cmd2.ExecuteNonQuery()
                    Show_Alert(True, "Account removed")
                Else
                    reader2.Close()
                    Show_Alert(False, "There are transactions associated with this account. It cannot be deleted.")
                End If
                con.close()            End Using
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview4_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles Gridview4.RowDeleting
        Try
            Dim row As GridViewRow = Gridview4.Rows(e.RowIndex)
            Dim account As String = row.Cells(0).Text
            If account = "BANK ACCOUNT" Or
               account = "ADVANCE FEE PAYMENT" Or
               account = "DEBTORS" Or
                   account = "PORTAL CHARGES" Or
                   account = "ALLOWANCES" Or
                   account = "SALARY DEDUCTIONS" Or
                   account = "SALARY EXPENSES" Then
                Show_Alert(False, "You cannot delete this account. You can only adjust the initial balance.")
                Exit Sub

            End If
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT  * from transactions where account = ?", con)
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", account))
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
                If Not reader2.HasRows Then
                    reader2.Close()
                    Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("Delete from accsettings where accname = ?", con)
                    cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("accname", account))
                    cmd2.ExecuteNonQuery()
                    Show_Alert(True, "Account removed")
                Else
                    reader2.Close()
                    Show_Alert(False, "There are transactions associated with this account. It cannot be deleted.")
                End If
                con.close()            End Using
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview5_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles Gridview5.RowDeleting
        Try
            Dim row As GridViewRow = Gridview5.Rows(e.RowIndex)
            Dim account As String = row.Cells(0).Text
            If account = "BANK ACCOUNT" Or
               account = "ADVANCE FEE PAYMENT" Or
               account = "DEBTORS" Or
                   account = "PORTAL CHARGES" Or
                   account = "ALLOWANCES" Or
                   account = "SALARY DEDUCTIONS" Or
                   account = "SALARY EXPENSES" Then
                Show_Alert(False, "You cannot delete this account. You can only adjust the initial balance.")
                Exit Sub

            End If
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT  * from transactions where account = ?", con)
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", account))
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
                If Not reader2.HasRows Then
                    reader2.Close()
                    Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("Delete from accsettings where accname = ?", con)
                    cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("accname", account))
                    cmd2.ExecuteNonQuery()
                    Show_Alert(True, "Account removed")
                Else
                    reader2.Close()
                    Show_Alert(False, "There are transactions associated with this account. It cannot be deleted.")
                End If
                con.close()            End Using
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview6_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles Gridview6.RowDeleting
        Try
            Dim row As GridViewRow = Gridview6.Rows(e.RowIndex)
            Dim account As String = row.Cells(0).Text
            If account = "BANK ACCOUNT" Or
               account = "ADVANCE FEE PAYMENT" Or
               account = "DEBTORS" Or
                   account = "PORTAL CHARGES" Or
                   account = "ALLOWANCES" Or
                   account = "SALARY DEDUCTIONS" Or
                   account = "SALARY EXPENSES" Then
                Show_Alert(False, "You cannot delete this account. You can only adjust the initial balance.")
                Exit Sub

            End If
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT  * from transactions where account = ?", con)
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", account))
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
                If Not reader2.HasRows Then
                    reader2.Close()
                    Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("Delete from accsettings where accname = ?", con)
                    cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("accname", account))
                    cmd2.ExecuteNonQuery()
                    Show_Alert(True, "Account removed")
                Else
                    reader2.Close()
                    Show_Alert(False, "There are transactions associated with this account. It cannot be deleted.")
                End If
                con.close()            End Using
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview7_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles Gridview7.RowDeleting
        Try
            Dim row As GridViewRow = Gridview7.Rows(e.RowIndex)
            Dim account As String = row.Cells(0).Text
            If account = "BANK ACCOUNT" Or
                account = "ADVANCE FEE PAYMENT" Or
                account = "DEBTORS" Or
                    account = "PORTAL CHARGES" Or
                    account = "ALLOWANCES" Or
                    account = "SALARY DEDUCTIONS" Or
                    account = "SALARY EXPENSES" Then
                Show_Alert(False, "You cannot delete this account. You can only adjust the initial balance.")
                Exit Sub

            End If
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT  * from transactions where account = ?", con)
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", account))
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
                If Not reader2.HasRows Then
                    reader2.Close()
                    Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("Delete from accsettings where accname = ?", con)
                    cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("accname", account))
                    cmd2.ExecuteNonQuery()
                    Show_Alert(True, "Account removed")
                Else
                    reader2.Close()
                    Show_Alert(False, "There are transactions associated with this account. It cannot be deleted.")
                End If
                con.close()            End Using
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview8_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles Gridview8.RowDeleting
        Try
            Dim row As GridViewRow = Gridview8.Rows(e.RowIndex)
            Dim account As String = row.Cells(0).Text
            If account = "BANK ACCOUNT" Or
                account = "ADVANCE FEE PAYMENT" Or
                account = "DEBTORS" Or
                    account = "PORTAL CHARGES" Or
                    account = "ALLOWANCES" Or
                    account = "SALARY DEDUCTIONS" Or
                    account = "SALARY EXPENSES" Then
                Show_Alert(False, "You cannot delete this account. You can only adjust the initial balance.")
                Exit Sub

            End If
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT  * from transactions where account = ?", con)
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", account))
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
                If Not reader2.HasRows Then
                    reader2.Close()
                    Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("Delete from accsettings where accname = ?", con)
                    cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("accname", account))
                    cmd2.ExecuteNonQuery()
                    Show_Alert(True, "Account removed")
                Else
                    reader2.Close()
                    Show_Alert(False, "There are transactions associated with this account. It cannot be deleted.")
                End If
                con.close()            End Using
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview1_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles Gridview1.RowEditing
        Try
            Gridview1.EditIndex = e.NewEditIndex
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


    Protected Sub Gridview3_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles Gridview3.RowEditing
        Try
            Gridview3.EditIndex = e.NewEditIndex
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview4_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles Gridview4.RowEditing
        Try
            Gridview4.EditIndex = e.NewEditIndex
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview5_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles Gridview5.RowEditing
        Try
            Gridview5.EditIndex = e.NewEditIndex
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview6_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles Gridview6.RowEditing
        Try
            Gridview6.EditIndex = e.NewEditIndex
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview7_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles Gridview7.RowEditing
        Try
            Gridview7.EditIndex = e.NewEditIndex
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview8_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles Gridview8.RowEditing
        Try
            Gridview8.EditIndex = e.NewEditIndex
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview1_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles Gridview1.RowUpdating
        Try
            Dim row As GridViewRow = Gridview1.Rows(e.RowIndex)
            Dim ID As String = row.Cells(0).Text
            Dim sessions As String = TryCast(row.Cells(1).Controls(0), TextBox).Text.Replace(",", "")

            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim enter As New MySql.Data.MySqlClient.MySqlCommand("Update accsettings set initial = '" & sessions & "' where accname = '" & ID & "'", con)
                enter.ExecuteNonQuery()
                Gridview1.EditIndex = -1
                con.close()            End Using
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
                Dim enter As New MySql.Data.MySqlClient.MySqlCommand("Update accsettings set initial = '" & sessions & "' where accname = '" & ID & "'", con)
                enter.ExecuteNonQuery()
                Gridview2.EditIndex = -1
                con.close()            End Using
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview3_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles Gridview3.RowUpdating
        Try
            Dim row As GridViewRow = Gridview3.Rows(e.RowIndex)
            Dim ID As String = row.Cells(0).Text
            Dim sessions As String = TryCast(row.Cells(1).Controls(0), TextBox).Text.Replace(",", "")

            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim enter As New MySql.Data.MySqlClient.MySqlCommand("Update accsettings set initial = '" & sessions & "' where accname = '" & ID & "'", con)
                enter.ExecuteNonQuery()
                Gridview3.EditIndex = -1
                con.close()            End Using
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview4_RowUpdated(sender As Object, e As GridViewUpdatedEventArgs) Handles Gridview4.RowUpdated

    End Sub

    Protected Sub Gridview4_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles Gridview4.RowUpdating
        Try
            Dim row As GridViewRow = Gridview4.Rows(e.RowIndex)
            Dim ID As String = row.Cells(0).Text
            Dim sessions As String = TryCast(row.Cells(1).Controls(0), TextBox).Text.Replace(",", "")

            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim enter As New MySql.Data.MySqlClient.MySqlCommand("Update accsettings set initial = '" & sessions & "' where accname = '" & ID & "'", con)
                enter.ExecuteNonQuery()
                Gridview4.EditIndex = -1
                con.close()            End Using
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview5_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles Gridview5.RowUpdating
        Try
            Dim row As GridViewRow = Gridview5.Rows(e.RowIndex)
            Dim ID As String = row.Cells(0).Text
            Dim sessions As String = TryCast(row.Cells(1).Controls(0), TextBox).Text.Replace(",", "")

            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim enter As New MySql.Data.MySqlClient.MySqlCommand("Update accsettings set initial = '" & sessions & "' where accname = '" & ID & "'", con)
                enter.ExecuteNonQuery()
                Gridview5.EditIndex = -1
                con.close()            End Using
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub



    Protected Sub Gridview6_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles Gridview6.RowUpdating
        Try
            Dim row As GridViewRow = Gridview6.Rows(e.RowIndex)
            Dim ID As String = row.Cells(0).Text
            Dim sessions As String = TryCast(row.Cells(1).Controls(0), TextBox).Text.Replace(",", "")

            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim enter As New MySql.Data.MySqlClient.MySqlCommand("Update accsettings set initial = '" & sessions & "' where accname = '" & ID & "'", con)
                enter.ExecuteNonQuery()
                Gridview6.EditIndex = -1
                con.close()            End Using
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview7_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles Gridview7.RowUpdating
        Try
            Dim row As GridViewRow = Gridview7.Rows(e.RowIndex)
            Dim ID As String = row.Cells(0).Text
            Dim sessions As String = TryCast(row.Cells(1).Controls(0), TextBox).Text.Replace(",", "")

            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim enter As New MySql.Data.MySqlClient.MySqlCommand("Update accsettings set initial = '" & sessions & "' where accname = '" & ID & "'", con)
                enter.ExecuteNonQuery()
                Gridview7.EditIndex = -1
                con.close()            End Using
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview8_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles Gridview8.RowUpdating
        Try
            Dim row As GridViewRow = Gridview8.Rows(e.RowIndex)
            Dim ID As String = row.Cells(0).Text
            Dim sessions As String = TryCast(row.Cells(1).Controls(0), TextBox).Text.Replace(",", "")

            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim enter As New MySql.Data.MySqlClient.MySqlCommand("Update accsettings set initial = '" & sessions & "' where accname = '" & ID & "'", con)
                enter.ExecuteNonQuery()
                Gridview8.EditIndex = -1
                con.close()            End Using
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub lnkCash_Click(sender As Object, e As EventArgs) Handles lnkCash.Click
        pnlCash.Visible = True
    End Sub

    Protected Sub lnkEquity_Click(sender As Object, e As EventArgs) Handles lnkEquity.Click
        pnlEquity.Visible = True
    End Sub

    Protected Sub lnkExpense_Click(sender As Object, e As EventArgs) Handles lnkExpense.Click
        pnlExpense.Visible = True
    End Sub

    Protected Sub lnkFixed_Click(sender As Object, e As EventArgs) Handles lnkFixed.Click
        pnlfixed.Visible = True
    End Sub

    Protected Sub lnkIncome_Click(sender As Object, e As EventArgs) Handles lnkIncome.Click
        pnlIncome.Visible = True
    End Sub

    Protected Sub lnkLiable_Click(sender As Object, e As EventArgs) Handles lnkLiable.Click
        pnllLiable.Visible = True
    End Sub

    Protected Sub lnkReceive_Click(sender As Object, e As EventArgs) Handles lnkReceive.Click
        pnlreceivables.Visible = True
    End Sub

    Protected Sub lnkStock_Click(sender As Object, e As EventArgs) Handles lnkStock.Click
        pnlStock.Visible = True
    End Sub
End Class
