Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Account_salary
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
        If check.Check_Account(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        If Not IsPostBack Then
            DropDownList3.Items.Add("Year")
            For j As Integer = 2019 To Now.Year + 1
                DropDownList3.Items.Add(j)
            Next
        End If
    End Sub

   
    Sub Compute_tax()

    End Sub

    Protected Sub GridView1_RowEditing(sender As Object, e As GridViewEditEventArgs)
        Try

       
        GridView1.EditIndex = e.NewEditIndex
        Dim ds As New DataTable
        GridView1.Columns(0).Visible = True
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.open()
                Dim comfirm20 As New MySql.Data.MySqlClient.MySqlCommand("Select salschedule.ref, salschedule.staffid as 'staff Id', staffprofile.surname as 'Name', staffprofile.designation, salschedule.amount as 'Amount', salschedule.tax as 'Tax', salschedule.pension as 'Pension', salschedule.bills as 'Bills', salschedule.deductions as 'Deduction', salschedule.increments as 'Increment', salschedule.welfare, salschedule.net as 'Balance'  from salschedule Inner Join staffprofile on salschedule.staffid = staffprofile.staffid where month = ? and year = ?", con)
                comfirm20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("month", Session("month")))
                comfirm20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("year", Session("year")))
                Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
                adapter1.SelectCommand = comfirm20
                ds.Clear()
                adapter1.Fill(ds)
                GridView1.DataSource = ds
                GridView1.DataBind()
                con.close()
            End Using

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub GridView1_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)

    End Sub

    Protected Sub GridView1_RowUpdating(sender As Object, e As GridViewUpdateEventArgs)
        Try
            Dim ds As New DataTable

            Dim row As GridViewRow = GridView1.Rows(e.RowIndex)
            Dim ref As Integer = row.Cells(1).Text
            Dim salary As Integer = row.Cells(5).Text
            Dim tax As Integer = row.Cells(6).Text
            Dim pension As Integer = row.Cells(7).Text
            Dim bills As String = TryCast(row.Cells(8).Controls(0), TextBox).Text.Replace(",", "")
            Dim deduction As String = TryCast(row.Cells(9).Controls(0), TextBox).Text.Replace(",", "")
            Dim increment As String = TryCast(row.Cells(10).Controls(0), TextBox).Text.Replace(",", "")
            Dim welfare As String = TryCast(row.Cells(11).Controls(0), TextBox).Text.Replace(",", "")
            Dim Balance As Integer = (Val(salary) + Val(increment)) - (Val(bills) + Val(deduction) + tax + pension + Val(welfare))
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.open()
                Dim comfirm As New MySql.Data.MySqlClient.MySqlCommand("Update salschedule set bills = ?, deductions = ?, increments = ?, welfare = ?, net = ? where ref = ?", con)
                comfirm.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("bills", bills))
                comfirm.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("deduction", deduction))
                comfirm.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("increment", increment))
                comfirm.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("welfare", welfare))
                comfirm.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("bal", Balance))
                comfirm.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", ref))
                comfirm.ExecuteNonQuery()
                GridView1.EditIndex = -1
                ds.Columns.Add("ref")
                ds.Columns.Add("staff id")
                ds.Columns.Add("Name")
                ds.Columns.Add("Designation")
                ds.Columns.Add("Amount")
                ds.Columns.Add("Tax")
                ds.Columns.Add("Pension")
                ds.Columns.Add("bills")
                ds.Columns.Add("deduction")
                ds.Columns.Add("increment")
                ds.Columns.Add("welfare")
                ds.Columns.Add("balance")
                Dim comfirm20a As New MySql.Data.MySqlClient.MySqlCommand("Select salschedule.ref, salschedule.staffid as 'staff Id', staffprofile.surname as 'Name', staffprofile.designation, salschedule.amount as 'Amount', salschedule.tax as 'Tax', salschedule.pension as 'Pension', salschedule.bills as 'Bills', salschedule.deductions as 'Deduction', salschedule.increments as 'Increment', salschedule.welfare, salschedule.net as 'Balance'  from salschedule Inner Join staffprofile on salschedule.staffid = staffprofile.staffid where month = ? and year = ?", con)
                comfirm20a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("month", Session("month")))
                comfirm20a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("year", Session("year")))
                Dim adapter1a As MySql.Data.MySqlClient.MySqlDataReader = comfirm20a.ExecuteReader
                Do While adapter1a.Read
                    ds.Rows.Add(adapter1a.Item(0), adapter1a.Item(1), adapter1a.Item(2), adapter1a.Item(3), FormatNumber(adapter1a.Item(4), , , , TriState.True), FormatNumber(adapter1a.Item(5), , , , TriState.True), FormatNumber(adapter1a.Item(6), , , , TriState.True), FormatNumber(adapter1a.Item(7), , , , TriState.True), FormatNumber(adapter1a.Item(8), , , , TriState.True), FormatNumber(adapter1a.Item(9), , , , TriState.True), FormatNumber(adapter1a.Item(10), , , , TriState.True), FormatNumber(adapter1a.Item(11), , , , TriState.True))
                Loop
                adapter1a.Close()
                GridView1.Columns(0).Visible = False
                GridView1.DataSource = ds
                GridView1.DataBind()
                con.close()
            End Using

            Dim s As Integer
            Dim t As Integer
            Dim u As Integer
            Dim v As Integer
            Dim w As Integer
            Dim a As Integer
            Dim b As Integer

            For Each r In GridView1.Rows
                s = s + Val(r.Cells(5).Text.replace(",", ""))
                t = t + Val(r.Cells(6).Text.replace(",", ""))
                u = u + Val(r.Cells(7).Text.replace(",", ""))
                v = v + Val(r.Cells(8).Text.replace(",", ""))
                w = w + Val(r.Cells(9).Text.replace(",", ""))
                a = a + Val(r.Cells(10).Text.replace(",", ""))
                b = b + Val(r.Cells(11).Text.replace(",", ""))
            Next
            Label5.Text = FormatNumber(s, , , , TriState.True)
            Label6.Text = FormatNumber(t, , , , TriState.True)
            Label7.Text = FormatNumber(u, , , , TriState.True)
            Label8.Text = FormatNumber(v, , , , TriState.True)
            Label9.Text = FormatNumber(w, , , , TriState.True)
            Label10.Text = FormatNumber(a, , , , TriState.True)
            Label11.Text = FormatNumber(b, , , , TriState.True)
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub


    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.open()
                Dim comfirm As New MySql.Data.MySqlClient.MySqlCommand("Update salschedule set status = ? where month = ? and year = ?", con)
                comfirm.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("status", 1))
                comfirm.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("month", Session("month")))
                comfirm.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("year", Session("year")))
                comfirm.ExecuteNonQuery()
                Dim x As New Random
                Dim ref As New MySql.Data.MySqlClient.MySqlCommand("Select ref from transactions", con)
                Dim readref As MySql.Data.MySqlClient.MySqlDataReader = ref.ExecuteReader
                Dim test As Boolean = False
                Dim refs As New ArrayList
                Do While readref.Read
                    refs.Add(readref.Item(0))
                Loop
                Dim y As Integer
                Do Until test = True
                    y = x.Next(100000, 999999)
                    If refs.Contains(y) Then
                        test = False
                    Else
                        test = True
                    End If
                Loop
                readref.Close()
                Dim s As Double
                Dim g As Double
                Dim w As Double
                For Each r In GridView1.Rows
                    s = s + r.Cells(5).Text.replace(",", "")
                    g = g + r.cells(10).text.replace(",", "")
                    w = w + r.cells(9).text.replace(",", "")
                Next
                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, session) Values (?,?,?,?,?,?,?,?)", con)
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", y))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Expense"))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(s, , , , TriState.True)))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "SALARY EXPENSES"))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Salary paid for " & Session("month") & " " & Session("year")))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pe", Session("sessionid")))

                cmdCheck2.ExecuteNonQuery()
                Dim cmdCheck1 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, session) Values (?,?,?,?,?,?,?,?)", con)
                cmdCheck1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", y))
                cmdCheck1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Cash"))
                cmdCheck1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(s, , , , TriState.True)))
                cmdCheck1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "BANK ACCOUNT"))
                cmdCheck1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Salary paid for " & Session("month") & " " & Session("year")))
                cmdCheck1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Credit"))
                cmdCheck1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pe", Session("sessionid")))

                cmdCheck1.ExecuteNonQuery()
                Dim test21 As Boolean
                Dim f1 As New Random
                Dim ref21 As New MySql.Data.MySqlClient.MySqlCommand("Select ref from transactions", con)
                Dim readref21 As MySql.Data.MySqlClient.MySqlDataReader = ref21.ExecuteReader

                Dim refs21 As New ArrayList
                Do While readref21.Read
                    refs21.Add(readref21.Item(0))
                Loop
                Dim d1 As Integer
                Do Until test21 = True
                    d1 = f1.Next(100000, 999999)
                    If refs21.Contains(d1) Then
                        test21 = False
                    Else
                        test21 = True
                    End If
                Loop
                readref21.Close()
                If Not g = 0 Then
                    Dim cmdCheck0 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, session) Values (?,?,?,?,?,?,?,?)", con)
                    cmdCheck0.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d1))
                    cmdCheck0.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Expense"))
                    cmdCheck0.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(g, , , , TriState.True)))
                    cmdCheck0.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "ALLOWANCES"))
                    cmdCheck0.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Allowances for " & Session("month") & " " & Session("year")))
                    cmdCheck0.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck0.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                    cmdCheck0.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pe", Session("sessionid")))

                    cmdCheck0.ExecuteNonQuery()
                    Dim cmdCheck1a As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, session) Values (?,?,?,?,?,?,?,?)", con)
                    cmdCheck1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d1))
                    cmdCheck1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Cash"))
                    cmdCheck1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(g, , , , TriState.True)))
                    cmdCheck1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "BANK ACCOUNT"))
                    cmdCheck1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Allowances for " & Session("month") & " " & Session("year")))
                    cmdCheck1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Credit"))
                    cmdCheck1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pe", Session("sessionid")))

                    cmdCheck1a.ExecuteNonQuery()
                End If
                Dim test22 As Boolean
                Dim f2 As New Random
                Dim ref22 As New MySql.Data.MySqlClient.MySqlCommand("Select ref from transactions", con)
                Dim readref22 As MySql.Data.MySqlClient.MySqlDataReader = ref22.ExecuteReader

                Dim refs22 As New ArrayList
                Do While readref22.Read
                    refs22.Add(readref22.Item(0))
                Loop
                Dim d2 As Integer
                Do Until test22 = True
                    d2 = f2.Next(100000, 999999)
                    If refs22.Contains(d2) Then
                        test22 = False
                    Else
                        test22 = True
                    End If
                Loop
                readref22.Close()
                If Not w = 0 Then
                    Dim cmdCheck11 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, session) Values (?,?,?,?,?,?,?,?)", con)
                    cmdCheck11.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d2))
                    cmdCheck11.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                    cmdCheck11.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(w, , , , TriState.True)))
                    cmdCheck11.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "SALARY DEDUCTIONS"))
                    cmdCheck11.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Salary deductions for " & Session("month") & " " & Session("year")))
                    cmdCheck11.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck11.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Credit"))
                    cmdCheck11.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pe", Session("sessionid")))

                    cmdCheck11.ExecuteNonQuery()


                    Dim cmdCheck1s As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, session) Values (?,?,?,?,?,?,?,?)", con)
                    cmdCheck1s.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d2))
                    cmdCheck1s.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Cash"))
                    cmdCheck1s.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(w, , , , TriState.True)))
                    cmdCheck1s.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "BANK ACCOUNT"))
                    cmdCheck1s.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Salary deductions for " & Session("month") & " " & Session("year")))
                    cmdCheck1s.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck1s.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                    cmdCheck1s.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pe", Session("sessionid")))

                    cmdCheck1s.ExecuteNonQuery()
                End If
                lblStatus.Text = "Paid"

                lblStatus.ForeColor = Drawing.Color.Green
                Dim ds As New DataTable
                ds.Columns.Add("ref")
                ds.Columns.Add("staff id")
                ds.Columns.Add("Name")
                ds.Columns.Add("Designation")
                ds.Columns.Add("Amount")
                ds.Columns.Add("Tax")
                ds.Columns.Add("Pension")
                ds.Columns.Add("bills")
                ds.Columns.Add("deduction")
                ds.Columns.Add("increment")
                ds.Columns.Add("welfare")
                ds.Columns.Add("balance")
                GridView1.AutoGenerateEditButton = False
                Dim comfirm20a As New MySql.Data.MySqlClient.MySqlCommand("Select salschedule.ref, salschedule.staffid as 'staff Id', staffprofile.surname as 'Name', staffprofile.designation, salschedule.amount as 'Amount', salschedule.tax as 'Tax', salschedule.pension as 'Pension', salschedule.bills as 'Bills', salschedule.deductions as 'Deduction', salschedule.increments as 'Increment', salschedule.welfare, salschedule.net as 'Balance'  from salschedule Inner Join staffprofile on salschedule.staffid = staffprofile.staffid where month = ? and year = ?", con)
                comfirm20a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("month", Session("month")))
                comfirm20a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("year", Session("year")))
                Dim adapter1a As MySql.Data.MySqlClient.MySqlDataReader = comfirm20a.ExecuteReader
                Do While adapter1a.Read
                    ds.Rows.Add(adapter1a.Item(0), adapter1a.Item(1), adapter1a.Item(2), adapter1a.Item(3), FormatNumber(adapter1a.Item(4), , , , TriState.True), FormatNumber(adapter1a.Item(5), , , , TriState.True), FormatNumber(adapter1a.Item(6), , , , TriState.True), FormatNumber(adapter1a.Item(7), , , , TriState.True), FormatNumber(adapter1a.Item(8), , , , TriState.True), FormatNumber(adapter1a.Item(9), , , , TriState.True), FormatNumber(adapter1a.Item(10), , , , TriState.True), FormatNumber(adapter1a.Item(11), , , , TriState.True))
                Loop
                adapter1a.Close()
                GridView1.DataSource = ds
                GridView1.DataBind()
                Dim comfirms As New MySql.Data.MySqlClient.MySqlCommand("Select * from staffprofile where activated = ?", con)
                comfirms.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", 1))
                Dim rcomfirm As MySql.Data.MySqlClient.MySqlDataReader = comfirms.ExecuteReader
                Dim staff As New ArrayList
                Do While rcomfirm.Read
                    If Not Val(rcomfirm("salary").ToString) = 0 Then
                        staff.Add(rcomfirm(0).ToString)
                    End If
                Loop
                rcomfirm.Close()
                Dim xs As New Random
                Dim refs2 As New MySql.Data.MySqlClient.MySqlCommand("Select id from messages", con)
                Dim readrefs As MySql.Data.MySqlClient.MySqlDataReader = refs2.ExecuteReader
                Dim tests As Boolean = False
                Dim refss As New ArrayList
                Do While readrefs.Read
                    refss.Add(readrefs.Item(0))
                Loop
                Dim ys As Integer
                Do Until tests = True
                    ys = xs.Next(100000, 999999)
                    If refss.Contains(ys) Then
                        tests = False
                    Else
                        tests = True
                    End If
                Loop
                readrefs.Close()
                Dim path As String = "https://" & Request.Url.Authority & "/Content/"
                Dim message As String

                message = String.Format("This is to inform you that your salary has been paid for " + Session("month") + " " + Session("year") + ". You can access your payslip using this link.<a target='_blank' href=" + path + "account/payslip.aspx?month=" + Session("Month") + "&year=" + Session("year") + ">View payslip</a>")

                For Each item As String In staff

                    Dim cmdCheck20 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into messages (id, sender, receiver, subject, message, date, session, sendertype, receivertype) Values (?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Id", ys))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sender", Session("staffid")))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("receiver", item))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", "Salary payment"))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("message", message))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("SessionID")))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sendertype", "Accounts"))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("receivertype", "Staff"))
                    cmdCheck20.ExecuteNonQuery()
                    logify.Notifications("You have a new message - " & "Salary payment", item, Session("staffid"), "Accounts", "~/content/staff/readmsg.aspx?" & ys, "Message")
                Next
                Dim cmdcec As New MySql.Data.MySqlClient.MySqlCommand("Insert Into sentmsgs (id, sender, receiver, subject, message, date, session, sendertype, receivertype, receiverreply) Values (?,?,?,?,?,?,?,?,?,?)", con)
                cmdcec.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Id", ys))
                cmdcec.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sender", Session("staffid")))
                cmdcec.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("receiver", "All Staff"))
                cmdcec.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", "Salary payment"))
                cmdcec.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("message", message))
                cmdcec.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdcec.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("SessionID")))
                cmdcec.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sendertype", "Accounts"))
                cmdcec.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("receivertype", "Staff"))
                cmdcec.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("receiverreply", "Staff"))

                cmdcec.ExecuteNonQuery()

                con.close()
            End Using

            Button1.Visible = False
            Show_Alert(True, "Salary paid successfully")
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            GridView1.Columns(0).Visible = False
            lnkPrint.Visible = True
            panel1.Visible = True
            Dim ds As New DataTable
            If DropDownList2.Text = "Month" Or DropDownList3.Text = "Year" Then
                Show_Alert(False, "Please select the month and year for payment.")
                Exit Sub
            End If
            Session("month") = DropDownList2.Text
            Session("year") = DropDownList3.Text
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.open()
                ds.Columns.Add("ref")
                ds.Columns.Add("staff id")
                ds.Columns.Add("Name")
                ds.Columns.Add("Designation")
                ds.Columns.Add("Amount")
                ds.Columns.Add("Tax")
                ds.Columns.Add("Pension")
                ds.Columns.Add("bills")
                ds.Columns.Add("deduction")
                ds.Columns.Add("increment")
                ds.Columns.Add("welfare")
                ds.Columns.Add("balance")
                Dim comfirm20 As New MySql.Data.MySqlClient.MySqlCommand("Select salschedule.ref, salschedule.staffid as 'staff Id', staffprofile.surname as 'Name', staffprofile.designation, salschedule.amount as 'Amount', salschedule.tax as 'Tax', salschedule.pension as 'Pension', salschedule.bills as 'Bills', salschedule.deductions as 'Deduction', salschedule.increments as 'Increment', salschedule.welfare, salschedule.net as 'Balance'  from salschedule Inner Join staffprofile on salschedule.staffid = staffprofile.staffid where month = ? and year = ?", con)
                comfirm20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("month", Session("month")))
                comfirm20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("year", Session("year")))
                Dim adapter1 As MySql.Data.MySqlClient.MySqlDataReader = comfirm20.ExecuteReader
                Dim generated As Boolean = False
                Do While adapter1.Read
                    generated = True
                    ds.Rows.Add(adapter1.Item(0), adapter1.Item(1), adapter1.Item(2), adapter1.Item(3), FormatNumber(adapter1.Item(4), , , , TriState.True), FormatNumber(adapter1.Item(5), , , , TriState.True), FormatNumber(adapter1.Item(6), , , , TriState.True), FormatNumber(adapter1.Item(7), , , , TriState.True), FormatNumber(adapter1.Item(8), , , , TriState.True), FormatNumber(adapter1.Item(9), , , , TriState.True), FormatNumber(adapter1.Item(10), , , , TriState.True), FormatNumber(adapter1.Item(11), , , , TriState.True))
                Loop
                adapter1.Close()
                GridView1.DataSource = ds
                GridView1.DataBind()
                If generated = False Then
                    Dim comfirm As New MySql.Data.MySqlClient.MySqlCommand("Select * from staffprofile where activated = ?", con)
                    comfirm.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", 1))
                    Dim rcomfirm As MySql.Data.MySqlClient.MySqlDataReader = comfirm.ExecuteReader

                    Dim x As New Random
                    Dim d As Date = Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")
                    Dim i As Integer
                    Dim z As Integer
                    Dim y As New ArrayList
                    Dim staff As New ArrayList
                    Dim salary As New ArrayList
                    Dim pension As New ArrayList
                    Dim amount As New ArrayList
                    Do While rcomfirm.Read
                        salary.Add(Val(rcomfirm.Item("salary").ToString))
                        pension.Add(rcomfirm.Item("pension"))
                        y.Add(x.Next(100000, 999999))
                        staff.Add(rcomfirm.Item(0))
                        amount.Add(rcomfirm.Item("salary"))
                        i = i + 1
                    Loop
                    rcomfirm.Close()
                    Dim pensionamt As Double
                    Dim taxable As Double
                    Dim tax As Double
                    For Each q As Integer In salary
                        If pension(z) = True Then

                            pensionamt = 0.08 * q
                            taxable = q - pensionamt - (200000 / 12)

                        Else
                            taxable = q - (200000 / 12)

                        End If

                        If taxable <= 25000 Then
                            tax = 0.07 * taxable
                        Else
                            If taxable <= 50000 Then
                                tax = (0.07 * 25000) + (0.11 * (taxable - 25000))
                            Else
                                If taxable <= (50000 + (500000 / 12)) Then
                                    tax = (0.07 * 25000) + (0.11 * 25000) + 0.15 * (taxable - 50000)
                                Else
                                    If taxable <= (50000 + (500000 / 12) + (500000 / 12)) Then
                                        tax = (0.07 * 25000) + (0.11 * 25000) + 0.15 * (500000 / 12) + 0.19 * (taxable - 50000 - (500000 / 12))
                                    Else
                                        If taxable <= (50000 + (500000 / 12) + (500000 / 12) + (1600000 / 12)) Then
                                            tax = (0.07 * 25000) + (0.11 * 25000) + (0.15 * (500000 / 12) + (0.19 * (500000 / 12)) + (0.21 * (taxable - 50000 - (1000000 / 12))))
                                        End If
                                    End If
                                End If
                            End If
                        End If
                        If tax < 0 Then tax = 0
                        Dim bal As Double = q - tax - pensionamt
                        Dim comfirm2 As New MySql.Data.MySqlClient.MySqlCommand("insert into salschedule (ref, date, month, year, staffId, amount, tax, pension, net) Values (?,?,?,?,?,?,?,?,?)", con)
                        comfirm2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", y(z)))
                        comfirm2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", d))
                        comfirm2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("month", Session("month")))
                        comfirm2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("year", Session("year")))
                        comfirm2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("staff", staff(z)))
                        comfirm2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amount", q))
                        comfirm2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("tax", tax))
                        comfirm2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pension", pensionamt))
                        comfirm2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("balance", bal))
                        comfirm2.ExecuteNonQuery()
                        z = z + 1
                        pensionamt = Nothing
                        tax = Nothing
                    Next

                    Dim comfirm20a As New MySql.Data.MySqlClient.MySqlCommand("Select salschedule.ref, salschedule.staffid as 'staff Id', staffprofile.surname as 'Name', staffprofile.designation, salschedule.amount as 'Amount', salschedule.tax as 'Tax', salschedule.pension as 'Pension', salschedule.bills as 'Bills', salschedule.deductions as 'Deduction', salschedule.increments as 'Increment', salschedule.welfare, salschedule.net as 'Balance'  from salschedule Inner Join staffprofile on salschedule.staffid = staffprofile.staffid where month = ? and year = ?", con)
                    comfirm20a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("month", Session("month")))
                    comfirm20a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("year", Session("year")))
                    Dim adapter1a As MySql.Data.MySqlClient.MySqlDataReader = comfirm20a.ExecuteReader
                    Do While adapter1a.Read
                        ds.Rows.Add(adapter1a.Item(0), adapter1a.Item(1), adapter1a.Item(2), adapter1a.Item(3), FormatNumber(adapter1a.Item(4), , , , TriState.True), FormatNumber(adapter1a.Item(5), , , , TriState.True), FormatNumber(adapter1a.Item(6), , , , TriState.True), FormatNumber(adapter1a.Item(7), , , , TriState.True), FormatNumber(adapter1a.Item(8), , , , TriState.True), FormatNumber(adapter1a.Item(9), , , , TriState.True), FormatNumber(adapter1a.Item(10), , , , TriState.True), FormatNumber(adapter1a.Item(11), , , , TriState.True))
                    Loop
                    adapter1a.Close()
                    GridView1.DataSource = ds
                    GridView1.DataBind()
                Else
                    Dim comfirm21s As New MySql.Data.MySqlClient.MySqlCommand("Select status  from salschedule where month = ? and year = ?", con)
                    comfirm21s.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("month", Session("month")))
                    comfirm21s.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("year", Session("year")))
                    Dim statusreaders As MySql.Data.MySqlClient.MySqlDataReader = comfirm21s.ExecuteReader
                    statusreaders.Read()
                    If statusreaders.Item(0) = 0 Then
                        statusreaders.Close()
                        Dim comfirm As New MySql.Data.MySqlClient.MySqlCommand("Select * from staffprofile where activated = ?", con)
                        comfirm.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", 1))
                        Dim rcomfirm As MySql.Data.MySqlClient.MySqlDataReader = comfirm.ExecuteReader

                        Dim x As New Random
                        Dim d As Date = Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")
                        Dim i As Integer
                        Dim z As Integer
                        Dim y As New ArrayList
                        Dim staff As New ArrayList
                        Dim salary As New ArrayList
                        Dim pension As New ArrayList
                        Dim amount As New ArrayList
                        Do While rcomfirm.Read
                            salary.Add(Val(rcomfirm.Item("salary").ToString))
                            pension.Add(rcomfirm.Item("pension"))
                            y.Add(x.Next(100000, 999999))
                            staff.Add(rcomfirm.Item(0))
                            amount.Add(rcomfirm.Item("salary"))
                            i = i + 1
                        Loop
                        rcomfirm.Close()
                        Dim pensionamt As Double
                        Dim taxable As Double
                        Dim tax As Double
                        For Each q As Integer In salary
                            If pension(z) = True Then

                                pensionamt = 0.08 * q
                                taxable = q - pensionamt - (200000 / 12)

                            Else
                                taxable = q - (200000 / 12)

                            End If

                            If taxable <= 25000 Then
                                tax = 0.07 * taxable
                            Else
                                If taxable <= 50000 Then
                                    tax = (0.07 * 25000) + (0.11 * (taxable - 25000))
                                Else
                                    If taxable <= (50000 + (500000 / 12)) Then
                                        tax = (0.07 * 25000) + (0.11 * 25000) + 0.15 * (taxable - 50000)
                                    Else
                                        If taxable <= (50000 + (500000 / 12) + (500000 / 12)) Then
                                            tax = (0.07 * 25000) + (0.11 * 25000) + 0.15 * (500000 / 12) + 0.19 * (taxable - 50000 - (500000 / 12))
                                        Else
                                            If taxable <= (50000 + (500000 / 12) + (500000 / 12) + (1600000 / 12)) Then
                                                tax = (0.07 * 25000) + (0.11 * 25000) + (0.15 * (500000 / 12) + (0.19 * (500000 / 12)) + (0.21 * (taxable - 50000 - (1000000 / 12))))
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                            If tax < 0 Then tax = 0
                            Dim bal As Double = q - tax - pensionamt
                            Dim comfirm2 As New MySql.Data.MySqlClient.MySqlCommand("Update salschedule set amount = ?, tax = ?, pension = ?, net = ? where staffid = ? and month = ? and year = ?", con)
                            comfirm2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amount", q))
                            comfirm2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("tax", tax))
                            comfirm2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pension", pensionamt))
                            comfirm2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("balance", bal))
                            comfirm2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("staff", staff(z)))
                            comfirm2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("month", Session("month")))
                            comfirm2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("year", Session("year")))

                            comfirm2.ExecuteNonQuery()
                            z = z + 1
                            pensionamt = Nothing
                            tax = Nothing
                        Next
                    End If
                    statusreaders.Close()
                    Dim comfirm20a As New MySql.Data.MySqlClient.MySqlCommand("Select salschedule.ref, salschedule.staffid as 'staff Id', staffprofile.surname as 'Name', staffprofile.designation, salschedule.amount as 'Amount', salschedule.tax as 'Tax', salschedule.pension as 'Pension', salschedule.bills as 'Bills', salschedule.deductions as 'Deduction', salschedule.increments as 'Increment', salschedule.welfare, salschedule.net as 'Balance'  from salschedule Inner Join staffprofile on salschedule.staffid = staffprofile.staffid where month = ? and year = ?", con)
                    comfirm20a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("month", Session("month")))
                    comfirm20a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("year", Session("year")))
                    Dim adapter1a As MySql.Data.MySqlClient.MySqlDataReader = comfirm20a.ExecuteReader
                    Do While adapter1a.Read
                        ds.Rows.Add(adapter1a.Item(0), adapter1a.Item(1), adapter1a.Item(2), adapter1a.Item(3), FormatNumber(adapter1a.Item(4), , , , TriState.True), FormatNumber(adapter1a.Item(5), , , , TriState.True), FormatNumber(adapter1a.Item(6), , , , TriState.True), FormatNumber(adapter1a.Item(7), , , , TriState.True), FormatNumber(adapter1a.Item(8), , , , TriState.True), FormatNumber(adapter1a.Item(9), , , , TriState.True), FormatNumber(adapter1a.Item(10), , , , TriState.True), FormatNumber(adapter1a.Item(11), , , , TriState.True))
                    Loop
                    adapter1a.Close()
                    GridView1.DataSource = ds
                    GridView1.DataBind()







                    End If
                    Label4.Text = "Staff Salary Schedule for " & Session("Month") & " " & Session("Year")
                    Dim s As Integer
                    Dim t As Integer
                    Dim u As Integer
                    Dim v As Integer
                    Dim w As Integer
                    Dim a As Integer
                    Dim b As Integer





                    Dim comfirm21 As New MySql.Data.MySqlClient.MySqlCommand("Select status  from salschedule where month = ? and year = ?", con)
                    comfirm21.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("month", Session("month")))
                    comfirm21.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("year", Session("year")))
                    Dim statusreader As MySql.Data.MySqlClient.MySqlDataReader = comfirm21.ExecuteReader
                    statusreader.Read()
                    If statusreader.Item(0) = 1 Then
                        lblStatus.Text = "Paid"
                        Button1.Visible = False
                        lblStatus.ForeColor = Drawing.Color.Green
                        GridView1.AutoGenerateEditButton = False
                        GridView1.DataBind()
                        For Each r In GridView1.Rows
                            s = s + Val(r.Cells(4).Text.replace(",", ""))
                            t = t + Val(r.Cells(5).Text.replace(",", ""))
                            u = u + Val(r.Cells(6).Text.replace(",", ""))
                            v = v + Val(r.Cells(7).Text.replace(",", ""))
                            w = w + Val(r.Cells(8).Text.replace(",", ""))
                            a = a + Val(r.Cells(9).Text.replace(",", ""))
                            b = b + Val(r.Cells(10).Text.replace(",", ""))
                        Next
                        Label5.Text = FormatNumber(s, , , , TriState.True)
                        Label6.Text = FormatNumber(t, , , , TriState.True)
                        Label7.Text = FormatNumber(u, , , , TriState.True)
                        Label8.Text = FormatNumber(v, , , , TriState.True)
                        Label9.Text = FormatNumber(w, , , , TriState.True)
                        Label10.Text = FormatNumber(a, , , , TriState.True)
                        Label11.Text = FormatNumber(b, , , , TriState.True)


                    Else
                        lblStatus.Text = "Not paid"
                        Button1.Visible = True
                        lblStatus.ForeColor = Drawing.Color.Red
                        GridView1.AutoGenerateEditButton = True
                        GridView1.DataBind()
                        For Each r In GridView1.Rows
                            s = s + Val(r.Cells(5).Text.replace(",", ""))
                            t = t + Val(r.Cells(6).Text.replace(",", ""))
                            u = u + Val(r.Cells(7).Text.replace(",", ""))
                            v = v + Val(r.Cells(8).Text.replace(",", ""))
                            w = w + Val(r.Cells(9).Text.replace(",", ""))
                            a = a + Val(r.Cells(10).Text.replace(",", ""))
                            b = b + Val(r.Cells(11).Text.replace(",", ""))
                        Next
                        Label5.Text = FormatNumber(s, , , , TriState.True)
                        Label6.Text = FormatNumber(t, , , , TriState.True)
                        Label7.Text = FormatNumber(u, , , , TriState.True)
                        Label8.Text = FormatNumber(v, , , , TriState.True)
                        Label9.Text = FormatNumber(w, , , , TriState.True)
                        Label10.Text = FormatNumber(a, , , , TriState.True)
                        Label11.Text = FormatNumber(b, , , , TriState.True)

                    End If
                    con.Close()
            End Using

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub lnkPrint_Click(sender As Object, e As EventArgs) Handles lnkPrint.Click
        Response.Redirect("~/content/account/salprint.aspx")
    End Sub
End Class
