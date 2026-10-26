Imports System.Text
Imports System.Configuration
Imports System.Data
Imports System.Web.UI.WebControls
Imports System.IO
Imports System.Net
Imports System.Web
Imports System.Net.Mail
Imports System.Resources

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
        If check.Check_Account(Session("roles"), Session("usertype")) = False Then
            If check.Check_oh(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        End If
        Try
            If check.Check_oh(Session("roles"), Session("usertype")) = True And check.Check_Account(Session("roles"), Session("usertype")) = False Then LinkButton2.Visible = False
            If Not IsPostBack Then
                logify.Read_notification("~/content/account/transactions.aspx", Session("staffid"))


                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                    con.open()
                    Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT accname from accsettings", con)
                    Dim student04 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                    cboAcc.Items.Clear()
                    cboAcc.Items.Add("All accounts")
                    cboAcc.Items.Add("Cummulative Fee Payments")
                    Do While student04.Read
                        cboAcc.Items.Add(student04.Item(0))
                    Loop
                    student04.Close()

                    Dim ref2 As New MySql.Data.MySqlClient.MySqlCommand("Select opendate from session where ID = '" & Session("SessionId") & "'", con)
                    Dim readref2 As MySql.Data.MySqlClient.MySqlDataReader = ref2.ExecuteReader
                    readref2.Read()
                    Dim startdate As String = readref2(0).ToString
                    readref2.Close()
                    Dim ref2c As New MySql.Data.MySqlClient.MySqlCommand("Select opendate from session where ID > '" & Session("SessionId") & "'", con)
                    Dim readref2c As MySql.Data.MySqlClient.MySqlDataReader = ref2c.ExecuteReader
                    Dim enddate As String
                    If readref2c.Read() Then
                        Dim endd As String = readref2c(0).ToString
                        Dim a As Array = Split(endd, "/")
                        enddate = IIf(Val(a(0)) - 1 < 10, "0" & Val(a(0)) - 1, Val(a(0)) - 1) & "/" & a(1) & "/" & a(2)
                    Else
                        enddate = Convert.ToDateTime(Now.Date).ToString("dd/MM/yyyy")
                    End If
                    readref2c.Close()
                    DatePicker1.Text = startdate
                    DatePicker2.Text = enddate
                   
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ref, cast(date as char) as date, account, type, details, dr, cr from transactions where date >= ? and date <= ? order by date desc", con)
                    cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dates", Convert.ToDateTime(DateTime.ParseExact(DatePicker1.Text, "dd/MM/yyyy", Nothing)).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("daten", Convert.ToDateTime(DateTime.ParseExact(DatePicker2.Text & " 11:59 PM", "dd/MM/yyyy hh:mm tt", Nothing)).ToString("yyyy-MM-dd HH:mm:ss")))
                    Dim reader1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    Dim ds As New DataTable
                    ds.Columns.Add("ref")
                    ds.Columns.Add("Date")
                    ds.Columns.Add("Account")
                    ds.Columns.Add("Type")
                    ds.Columns.Add("Details")
                    ds.Columns.Add("Dr")
                    ds.Columns.Add("Cr")
                    Dim ct As Integer = 0
                    Do While reader1.Read
                        ds.Rows.Add(reader1(0), Convert.ToDateTime(reader1(1)).ToString("dd/MM/yyyy hh:mm tt"), reader1(2), reader1(3), reader1(4), reader1(5), reader1(6))
                        ct = ct + 1
                    Loop
                    reader1.Close()

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

            End If

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        Try

            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.open()
                Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT accname from accsettings", con)
                Dim student04 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                cboAcc.Items.Clear()
                cboAcc.Items.Add("All accounts")
                cboAcc.Items.Add("Cummulative Fee Payments")
                Do While student04.Read
                    cboAcc.Items.Add(student04.Item(0))
                Loop
                student04.Close()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ref, cast(date as char) as date, account, type, details, dr, cr from transactions where date >= ? and date <= ? order by date desc", con)
                cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dates", Convert.ToDateTime(DateTime.ParseExact(DatePicker1.Text, "dd/MM/yyyy", Nothing)).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("daten", Convert.ToDateTime(DateTime.ParseExact(DatePicker2.Text & " 11:59 PM", "dd/MM/yyyy hh:mm tt", Nothing)).ToString("yyyy-MM-dd HH:mm:ss")))
                Dim reader1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Dim ds As New DataTable
                ds.Columns.Add("ref")
                ds.Columns.Add("Date")
                ds.Columns.Add("Account")
                ds.Columns.Add("Type")
                ds.Columns.Add("Details")
                ds.Columns.Add("Dr")
                ds.Columns.Add("Cr")
                Dim ct As Integer = 0
                Do While reader1.Read
                    ds.Rows.Add(reader1(0), Convert.ToDateTime(reader1(1)).ToString("dd/MM/yyyy hh:mm tt"), reader1(2), reader1(3), reader1(4), reader1(5), reader1(6))
                    ct = ct + 1
                Loop
                reader1.Close()

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

                con.close()
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
                Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT accname from accsettings", con)
                Dim student04 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                cboAcc.Items.Clear()
                cboAcc.Items.Add("All accounts")
                cboAcc.Items.Add("Cummulative Fee Payments")
                Do While student04.Read
                    cboAcc.Items.Add(student04.Item(0))
                Loop
                student04.Close()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ref, cast(date as char) as date, account, type, details, dr, cr from transactions where date >= ? and date <= ? order by date desc", con)
                cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dates", Convert.ToDateTime(DateTime.ParseExact(DatePicker1.Text, "dd/MM/yyyy", Nothing)).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("daten", Convert.ToDateTime(DateTime.ParseExact(DatePicker2.Text & " 11:59 PM", "dd/MM/yyyy hh:mm tt", Nothing)).ToString("yyyy-MM-dd HH:mm:ss")))
                Dim reader1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader

                Dim ds As New DataTable
                ds.Columns.Add("ref")
                ds.Columns.Add("Date")
                ds.Columns.Add("Account")
                ds.Columns.Add("Type")
                ds.Columns.Add("Details")
                ds.Columns.Add("Dr")
                ds.Columns.Add("Cr")
                Dim ct As Integer = 0
                Do While reader1.Read
                    ds.Rows.Add(reader1(0), Convert.ToDateTime(reader1(1)).ToString("dd/MM/yyyy hh:mm tt"), reader1(2), reader1(3), reader1(4), reader1(5), reader1(6))
                    ct = ct + 1
                Loop
                reader1.Close()

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

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ref, date, account, type, details, dr, cr, student from transactions where details like ? or student like ? or ref like ? and date >= ? and date <= ? order by date desc", con)
                cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("like", "%" & txtSearch.Text & "%"))
                cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("2like", "%" & txtSearch.Text & "%"))
                cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("3like", "%" & txtSearch.Text & "%"))
                cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dates", Convert.ToDateTime(DateTime.ParseExact(DatePicker1.Text, "dd/MM/yyyy", Nothing)).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("daten", Convert.ToDateTime(DateTime.ParseExact(DatePicker2.Text & " 11:59 PM", "dd/MM/yyyy hh:mm tt", Nothing)).ToString("yyyy-MM-dd HH:mm:ss")))
                Dim balreader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Dim dr As Integer
                Dim type As String
                Dim cr As Integer
                Dim balance As Double
                Do While balreader.Read
                    dr = dr + Val(balreader.Item("dr").replace(",", ""))
                    cr = cr + Val(balreader.Item("cr").replace(",", ""))
                Loop
                balance = dr - cr
                If balance > 0 Then
                    type = "Dr"
                Else
                    balance = balance * -1
                    type = "Cr"
                End If
                lblBal.Text = "Balance: " & FormatNumber(balance, , , , TriState.True)
                lblType.Text = type
                balreader.Close()
                Dim ds As New DataTable
                GridView1.AllowPaging = False
                Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
                adapter1.SelectCommand = cmdLoad1
                adapter1.Fill(ds)
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

                con.close()
            End Using


        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT accname from accsettings", con)
                Dim student04 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                cboAcc.Items.Clear()
                cboAcc.Items.Add("All accounts")
                cboAcc.Items.Add("Cummulative Fee Payments")
                Do While student04.Read
                    cboAcc.Items.Add(student04.Item(0))
                Loop
                student04.Close()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ref, cast(date as char) as date, account, type, details, dr, cr from transactions where date >= ? and date <= ? order by date desc", con)
                cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dates", Convert.ToDateTime(DateTime.ParseExact(DatePicker1.Text, "dd/MM/yyyy", Nothing)).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("daten", Convert.ToDateTime(DateTime.ParseExact(DatePicker2.Text & " 11:59 PM", "dd/MM/yyyy hh:mm tt", Nothing)).ToString("yyyy-MM-dd HH:mm:ss")))
                Dim reader1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Dim ds As New DataTable
                ds.Columns.Add("ref")
                ds.Columns.Add("Date")
                ds.Columns.Add("Account")
                ds.Columns.Add("Type")
                ds.Columns.Add("Details")
                ds.Columns.Add("Dr")
                ds.Columns.Add("Cr")
                Dim ct As Integer = 0
                Do While reader1.Read
                    ds.Rows.Add(reader1(0), Convert.ToDateTime(reader1(1)).ToString("dd/MM/yyyy hh:mm tt"), reader1(2), reader1(3), reader1(4), reader1(5), reader1(6))
                    ct = ct + 1
                Loop
                reader1.Close()
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

    Protected Sub LinkButton2_Click(sender As Object, e As EventArgs) Handles LinkButton2.Click
        Response.Redirect("~/content/account/transact.aspx")
    End Sub

    Protected Sub cboAcc_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboAcc.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.open()
                btnNext.Visible = False
                btnPrevious.Visible = False
                GridView1.AllowPaging = False
                If cboAcc.Text = "All accounts" Then
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ref, date, account, type, details, dr, cr, student from transactions where date >= ? and date <= ? order by date desc", con)
                    cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dates", Convert.ToDateTime(DateTime.ParseExact(DatePicker1.Text, "dd/MM/yyyy", Nothing)).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("daten", Convert.ToDateTime(DateTime.ParseExact(DatePicker2.Text & " 11:59 PM", "dd/MM/yyyy hh:mm tt", Nothing)).ToString("yyyy-MM-dd HH:mm:ss")))
                    Dim balreader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    Dim dr As Integer
                    Dim type As String
                    Dim cr As Integer
                    Dim balance As Double
                    Dim ds As New DataTable
                    ds.Columns.Add("ref")
                    ds.Columns.Add("Date")
                    ds.Columns.Add("Account")
                    ds.Columns.Add("Type")
                    ds.Columns.Add("Details")
                    ds.Columns.Add("Dr")
                    ds.Columns.Add("Cr")
                    Dim ct As Integer = 0
                    Dim refs As New ArrayList
                    Dim students As New ArrayList
                    Do While balreader.Read

                        dr = dr + Val(balreader.Item("dr").replace(",", ""))
                        cr = cr + Val(balreader.Item("cr").replace(",", ""))
                        ds.Rows.Add(balreader(0), Convert.ToDateTime(balreader(1)).ToString("dd/MM/yyyy hh:mm tt"), balreader(2), balreader(3), balreader(4), balreader(5), balreader(6))
                        ct = ct + 1
                    Loop
                    balance = dr - cr
                    If balance > 0 Then
                        type = "Dr"
                    Else
                        balance = balance * -1
                        type = "Cr"
                    End If
                    balreader.Close()
                    Dim paid As Integer = 0
                    Dim mole As String = ""
                    Dim cmdLoadsz As New MySql.Data.MySqlClient.MySqlCommand("SELECT student from feeschedule  where session = '" & Session("sessionid") & "'", con)
                    Dim balreadersz As MySql.Data.MySqlClient.MySqlDataReader = cmdLoadsz.ExecuteReader

                    Do While balreadersz.Read
                        If Not students.Contains(balreadersz("student")) Then students.Add(balreadersz("student"))
                    Loop
                    balreadersz.Close()
                    Dim cts As Integer = 0
                    For Each student As String In students
                        Dim cmdLoadszw As New MySql.Data.MySqlClient.MySqlCommand("SELECT paid from feeschedule where student = '" & student & "' and session = '" & Session("sessionid") & "'", con)
                        Dim balreaderszw As MySql.Data.MySqlClient.MySqlDataReader = cmdLoadszw.ExecuteReader
                        Dim ca As Integer = 0
                        Do While balreaderszw.Read
                            ca = ca + 1
                            paid = paid + Val(balreaderszw(0))
                        Loop
                        balreaderszw.Close()
                        Dim cmdLoads As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from transactions where student = '" & student & "' and account = 'DEBTORS'", con)
                        Dim balreaders As MySql.Data.MySqlClient.MySqlDataReader = cmdLoads.ExecuteReader
                        Dim exists As Boolean = False
                        Dim crxs As Integer = 0
                        Do While balreaders.Read
                            crxs = crxs + Val(balreaders("cr").ToString.Replace(",", ""))
                        Loop
                        balreaders.Close()
                        If ca > 3 Then
                            mole = student
                            Show_Alert(False, mole & ": " & paid)
                        End If
                        paid = Nothing
                        exists = False
                        cts = cts + 1
                    Next

                    lblBal.Text = "Balance: " & FormatNumber(balance, , , , TriState.True)
                    lblType.Text = type

                    GridView1.DataSource = ds
                    GridView1.DataBind()
                ElseIf cboAcc.Text = "Cummulative Fee Payments" Then
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT transactions.ref, transactions.date, transactions.account, transactions.type, transactions.details, transactions.dr, transactions.cr, studentsprofile.surname from transactions left join studentsprofile on studentsprofile.admno = transactions.student where  transactions.date >= ? and transactions.date <= ? and transactions.account = '" & "DEBTORS" & "' and transactions.dr = '" & "" & "' and transactions.details <> '" & "Student Withdrawal" & "' order by transactions.date desc", con)
                    cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dates", Convert.ToDateTime(DateTime.ParseExact(DatePicker1.Text, "dd/MM/yyyy", Nothing)).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("daten", Convert.ToDateTime(DateTime.ParseExact(DatePicker2.Text & " 11:59 PM", "dd/MM/yyyy hh:mm tt", Nothing)).ToString("yyyy-MM-dd HH:mm:ss")))
                    Dim balreader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    Dim dr As Integer
                    Dim type As String
                    Dim cr As Integer
                    Dim balance As Double
                    Dim ds As New DataTable
                    ds.Columns.Add("ref")
                    ds.Columns.Add("Date")
                    ds.Columns.Add("Account")
                    ds.Columns.Add("Type")
                    ds.Columns.Add("Details")
                    ds.Columns.Add("Dr")
                    ds.Columns.Add("Cr")
                    Dim ct As Integer = 0
                    Do While balreader.Read
                        dr = dr + Val(balreader.Item("dr").replace(",", ""))
                        cr = cr + Val(balreader.Item("cr").replace(",", ""))
                        ds.Rows.Add(balreader(0), Convert.ToDateTime(balreader(1)).ToString("dd/MM/yyyy hh:mm tt"), "FEE PAYMENTS", "Income", "Fees Paid by " & balreader(7), balreader(5), balreader(6))
                        ct = ct + 1
                    Loop
                    balance = dr - cr
                    If balance > 0 Then
                        type = "Dr"
                    Else
                        balance = balance * -1
                        type = "Cr"
                    End If
                    lblBal.Text = "Balance: " & FormatNumber(balance, , , , TriState.True)
                    lblType.Text = type
                    balreader.Close()
                    GridView1.DataSource = ds
                    GridView1.DataBind()

                Else
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ref, date, account, type, details, dr, cr, student from transactions where account = ? and date >= ? and date <= ? order by date desc", con)
                    cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("acc", cboAcc.Text))
                    cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dates", Convert.ToDateTime(DateTime.ParseExact(DatePicker1.Text, "dd/MM/yyyy", Nothing)).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("daten", Convert.ToDateTime(DateTime.ParseExact(DatePicker2.Text & " 11:59 PM", "dd/MM/yyyy hh:mm tt", Nothing)).ToString("yyyy-MM-dd HH:mm:ss")))
                    Dim balreader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    Dim dr As Integer
                    Dim type As String
                    Dim cr As Integer
                    Dim balance As Double
                    Dim ds As New DataTable
                    ds.Columns.Add("ref")
                    ds.Columns.Add("Date")
                    ds.Columns.Add("Account")
                    ds.Columns.Add("Type")
                    ds.Columns.Add("Details")
                    ds.Columns.Add("Dr")
                    ds.Columns.Add("Cr")
                    Dim ct As Integer = 0
                    Dim acctype As String = ""
                    Do While balreader.Read
                        acctype = balreader(3).ToString
                        dr = dr + Val(balreader.Item("dr").replace(",", ""))
                        cr = cr + Val(balreader.Item("cr").replace(",", ""))
                        ds.Rows.Add(balreader(0), Convert.ToDateTime(balreader(1)).ToString("dd/MM/yyyy hh:mm tt"), balreader(2), balreader(3), balreader(4), balreader(5), balreader(6))
                        ct = ct + 1
                    Loop
                    balreader.Close()
                    balance = dr - cr

                    If balance > 0 Then
                        type = "Dr"
                    Else
                        balance = balance * -1
                        type = "Cr"
                    End If

                    lblBal.Text = "Balance: " & FormatNumber(balance, , , , TriState.True)
                    lblType.Text = type

                    GridView1.DataSource = ds
                    GridView1.DataBind()
                End If

                con.close()
            End Using


        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnInstt_Click(sender As Object, e As EventArgs) Handles btnInstt.Click
        Session("start") = DateTime.ParseExact(DatePicker1.Text, "dd/MM/yyyy", Nothing)
        Session("end") = DateTime.ParseExact(DatePicker2.Text & " 11:59 PM", "dd/MM/yyyy hh:mm tt", Nothing)
        Response.Redirect("~/content/account/incomestatement.aspx")
    End Sub

    Protected Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Session("start") = DateTime.ParseExact(DatePicker1.Text, "dd/MM/yyyy", Nothing)
        Session("end") = DateTime.ParseExact(DatePicker2.Text & " 11:59 PM", "dd/MM/yyyy hh:mm tt", Nothing)
        Response.Redirect("~/content/account/balancesheet.aspx")
    End Sub

    Protected Sub LinkButton3_Click(sender As Object, e As EventArgs) Handles LinkButton3.Click
        Response.Redirect("~/content/account/feesreport.aspx")
    End Sub

    Protected Sub LinkButton4_Click(sender As Object, e As EventArgs) Handles LinkButton4.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.open()
                Dim incomeacc As New DataTable
                incomeacc.Columns.Add("name")
                incomeacc.Columns.Add("phone")
                incomeacc.Columns.Add("amount")
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT feeschedule.student, studentsprofile.surname, feeschedule.amount, feeschedule.paid, class.class, parentprofile.phone from feeschedule inner join (parentward inner join parentprofile on parentward.parent = parentprofile.parentid) on feeschedule.student = parentward.ward inner join studentsprofile on feeschedule.student = studentsprofile.admno inner join class on feeschedule.class = class.Id where feeschedule.session = ? order by studentsprofile.surname", con)
                cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("SessionId")))
                Dim balreader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Dim students As New ArrayList
                Dim studentname As New ArrayList
                Dim phone As New ArrayList
                Do While balreader.Read
                    If balreader.Item(2) - balreader.Item(3) > 0 Then
                        If Not students.Contains(balreader(0).ToString) Then
                            students.Add(balreader(0).ToString)
                            phone.Add(balreader(5).ToString)
                            studentname.Add(balreader(1).ToString)
                        End If
                    End If
                Loop
                balreader.Close()
                Dim count As Integer = 0
                Dim msgs As Integer = 0
                Dim cmdSelect2a As New MySql.Data.MySqlClient.MySqlCommand("SELECT smsno FROM Session WHERE id = ?", con)
                cmdSelect2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionId")))
                Dim reader2a As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2a.ExecuteReader
                reader2a.Read()
                Dim smsno = reader2a(0)
                reader2a.Close()
                Dim finished As String = ""
                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT email, password, port, smtp, smsapi, smsname from options", con)
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", Session("sessionid")))
                Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader
                reader3.Read()

                Dim smsapi As String = reader3(4).ToString
                Dim smsname As String = reader3(5).ToString
                reader3.Close()
                For Each student As String In students
                    Dim cmdInsert220 As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where session = ? and student = ?", con)
                    cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                    cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", student))
                    Dim feereader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert220.ExecuteReader
                    Dim total As Integer = 0
                    Dim feetype As New ArrayList
                    Dim feeamount As New ArrayList
                    Dim paid As Double = 0
                    Dim min As Double = 0
                    Do While feereader2.Read
                        feetype.Add(feereader2.Item("fee"))
                        feeamount.Add(feereader2.Item("amount"))
                        total = total + feereader2.Item("amount")
                        paid = paid + feereader2.Item("paid")
                        min = min + feereader2.Item("min")
                    Loop
                    feereader2.Close()
                    Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where student = ? and session <> ?", con)
                    cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                    Dim feereader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                    Dim totalall As Integer = 0
                    Dim paidall As Double = 0
                    Do While feereader3.Read
                        totalall = totalall + feereader3.Item("amount")
                        paidall = paidall + feereader3.Item("paid")
                    Loop
                    feereader3.Close()
                    Dim outstanding As Double

                    outstanding = totalall - paidall
                    Dim debt As String = FormatNumber((total - paid) + outstanding, 0, , , TriState.True)



                    Dim message
                    Dim path As String = "https://" & Request.Url.Authority & "/Content/"
                    Dim apikey = smsapi
                    Dim strPost As String
                    Dim senders = smsname
                    Dim url As String = "https://www.bulksmsnigeria.com/api/v1/sms/create?"
                    message = String.Format("You are reminded of " + studentname(count) + "'S debt of N" & debt & ". Click here to pay. " + path + "parent/feeschedule.aspx?" + student)
                    Dim numbers = Replace(phone(count), "0", "234", 1, 1)
                    strPost = url & "api_token=" & apikey & "&to=" & numbers & "&body=" & message & "&from=" & senders & "&dnd=2"
                    ServicePointManager.SecurityProtocol = 3072
                    Try
                        If smsno > 0 Then
                            Dim rrequest As WebRequest = WebRequest.Create(strPost)
                            rrequest.Method = "POST"
                            Dim byteArray As Byte() = Encoding.UTF8.GetBytes(strPost)
                            rrequest.ContentType = "application/x-www-form-urlencoded"
                            rrequest.ContentLength = byteArray.Length
                            Dim dataStream As Stream = rrequest.GetRequestStream()
                            dataStream.Write(byteArray, 0, byteArray.Length)
                            dataStream.Close()
                            msgs = msgs + 1
                            smsno = smsno - 1
                        Else
                            finished = "SMS Credits exhausted"
                        End If
                    Catch
                    End Try
                    count = count + 1
                Next
                Show_Alert(True, "Sent " & msgs & " SMS notifications." & finished)

                con.close()
            End Using


        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub LinkButton5_Click(sender As Object, e As EventArgs) Handles LinkButton5.Click
        Session("start") = DateTime.ParseExact(DatePicker1.Text, "dd/MM/yyyy", Nothing)
        Session("end") = DateTime.ParseExact(DatePicker2.Text & " 11:59 PM", "dd/MM/yyyy hh:mm tt", Nothing)

        Response.Redirect("~/content/account/incomestatement.aspx?cummulative=yes")

    End Sub

    ''' <summary>
    ''' Share Statement (Instance: Type/polymorphism-dependent) -- resolves a concrete
    ''' INotificationSender at runtime via NotificationSenderFactory.GetSender, based on the
    ''' "legacy format" toggle (a client-compatibility flag, no trust/security claim). The sink is
    ''' reached only by following that interface dispatch to whichever concrete class the factory
    ''' resolved -- RawEmailNotifier (vulnerable, PLANTED-VB-HR-559) when the toggle is checked,
    ''' ValidatedEmailNotifier (safe, PLANTED-VB-HR-559-safe) otherwise.
    ''' </summary>
    Protected Sub btnShareStatement_Click(sender As Object, e As EventArgs) Handles btnShareStatement.Click
        Try
            Dim notifier559 As INotificationSender = NotificationSenderFactory.GetSender(chkLegacyFormat.Checked)
            notifier559.Notify(txtShareEmail.Text, "Account Balance Statement", "Your latest account balance statement is attached to your portal account.")
            Show_Alert(True, "Statement shared.")
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    ''' <summary>
    ''' Email-CC variant of the debt-reminder run (Instance: Interprocedural, loop/array) -- lets a
    ''' guardian who is not the primary contact on file receive a copy by being added via a
    ''' repeated "ccParent" query-string key, routed through a new cross-file Friend Module,
    ''' App_Code/FeeReminderMailer.vb, which performs the actual loop and the sink.
    ''' </summary>
    Protected Sub btnEmailDebtReminders_Click(sender As Object, e As EventArgs) Handles btnEmailDebtReminders.Click
        Try
            Dim ccAddresses As String() = Request.QueryString.GetValues("ccParent")
            If ccAddresses Is Nothing Then ccAddresses = New String() {}
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.open()
                Dim cmdOpts560 As New MySql.Data.MySqlClient.MySqlCommand("SELECT email, password, port, smtp from options", con)
                Dim reader560 As MySql.Data.MySqlClient.MySqlDataReader = cmdOpts560.ExecuteReader
                reader560.Read()
                Dim fromEmail As String = reader560(0).ToString
                Dim fromPassword As String = reader560(1).ToString
                Dim mailPort As String = reader560(2).ToString
                Dim mailSmtp As String = reader560(3).ToString
                reader560.Close()
                con.close()
                FeeReminderMailer.SendDebtReminderCcList(fromEmail, fromPassword, mailSmtp, mailPort, ccAddresses)
            End Using
            Show_Alert(True, "Additional guardians CC'd on the debt reminder.")
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    ''' <summary>
    ''' Safe twin of btnEmailDebtReminders_Click.
    ''' </summary>
    Protected Sub btnEmailDebtRemindersSafe_Click(sender As Object, e As EventArgs) Handles btnEmailDebtRemindersSafe.Click
        Try
            Dim ccAddresses As String() = Request.QueryString.GetValues("ccParentSafe")
            If ccAddresses Is Nothing Then ccAddresses = New String() {}
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.open()
                Dim cmdOpts560s As New MySql.Data.MySqlClient.MySqlCommand("SELECT email, password, port, smtp from options", con)
                Dim reader560s As MySql.Data.MySqlClient.MySqlDataReader = cmdOpts560s.ExecuteReader
                reader560s.Read()
                Dim fromEmail As String = reader560s(0).ToString
                Dim fromPassword As String = reader560s(1).ToString
                Dim mailPort As String = reader560s(2).ToString
                Dim mailSmtp As String = reader560s(3).ToString
                reader560s.Close()
                con.close()
                FeeReminderMailer.SendDebtReminderCcListSafe(fromEmail, fromPassword, mailSmtp, mailPort, ccAddresses)
            End Using
            Show_Alert(True, "Additional guardians CC'd on the debt reminder.")
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
End Class
