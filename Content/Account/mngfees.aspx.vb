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
    Dim db As New DB_Interface
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

    Sub remove_student_fees(fee As String, cla As String, Optional transport As Boolean = False)
        Dim students As ArrayList = db.Select_1D("select student from studentsummary inner join class on class.id = studentsummary.class where class.class = '" & cla & "' and studentsummary.session = '" & Session("sessionid") & "'")
        For Each s In students
            Dim clas As Integer = db.Select_single("select id from class where class = '" & cla & "'")
            Dim paid As Integer = db.Select_single("select paid from feeschedule where student = '" & s & "' and fee = '" & IIf(transport = True, "TRANSPORT", fee) & "' and session = '" & Session("sessionid") & "'")            
            db.Non_Query("delete from feeschedule where student = '" & s & "' and class = '" & clas & "' and fee = '" & IIf(transport = True, "TRANSPORT", fee) & "'")
            Dim ref As Integer = db.Select_single("select ref from transactions where  account = '" & IIf(transport = True, "TRANSPORT", fee) & " DEBTS" & "' and student = '" & s & "' and session = '" & Session("sessionid") & "'")
            db.Non_Query("Delete FROM transactions WHERE ref = '" & ref & "'")
            Dim test2 As Boolean
            Dim f As New Random
            Dim refs2 As ArrayList = db.Select_1D("Select ref from transactions")
            Dim d As Integer
            Do Until test2 = True
                d = f.Next(100000, 999999)
                If refs2.Contains(d) Then
                    test2 = False
                Else
                    test2 = True
                End If
            Loop
            If paid > 0 Then
                db.Non_Query("Insert Into transactions (ref, type, cr, account, details, date, trtype, student) Values ('" & d & "','Liability','" & FormatNumber(paid, , , TriState.True) & "', 'ADVANCE FEE PAYMENT', 'Payment Advance' , '" & Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss") & "','Credit', '" & s & "')")                db.Non_Query("Insert Into transactions (ref, type, dr, account, details, date, trtype, student) Values ('" & d & "','Income','" & FormatNumber(paid, , , TriState.True) & "', '" & IIf(transport = True, "TRANSPORT", fee) & " PAID" & "', 'Temporary refresh' , '" & Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss") & "','Debit', '" & s & "')")            End If
        Next
    End Sub

    Sub get_students(cla As String, fee As String, amount As String, min As String, compulsory As Boolean, Optional admission As Boolean = False, Optional transport As Boolean = False)
        Dim students As ArrayList = db.Select_1D("select student from studentsummary inner join class on class.id = studentsummary.class where class.class = '" & cla & "' and studentsummary.session = '" & Session("sessionid") & "'")
        For Each s In students
            If admission = True And db.Select_single("select admfees from studentsprofile where admno = '" & s & "'") = "Paid" Then Exit Sub
            If transport = True And db.Select_single("select transport from studentsprofile where admno = '" & s & "'") <> fee Then Exit Sub
            Dim clas As Integer = db.Select_single("select id from class where class = '" & cla & "'")
            Dim test2 As Boolean
            Dim f As New Random
            Dim refs2 As ArrayList = db.Select_1D("Select ref from transactions")
            Dim d As Integer
            Do Until test2 = True
                d = f.Next(100000, 999999)
                If refs2.Contains(d) Then
                    test2 = False
                Else
                    test2 = True
                End If
            Loop            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dr, cr from transactions where account = ? and student = ? order by date desc", con)
                cmdLoad11.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("acc", "ADVANCE FEE PAYMENT"))
                cmdLoad11.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", s))
                Dim balreader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                Dim dr As Integer
                Dim cr As Integer
                Dim balance As Double = 0
                Do While balreader.Read
                    dr = dr + Val(balreader.Item("dr").replace(",", ""))
                    cr = cr + Val(balreader.Item("cr").replace(",", ""))
                Loop
                Dim fbalance As Double = cr - dr
                balance = (cr - dr)
                balreader.Close()

                cr = Nothing
                dr = Nothing
                con.Close()

                db.Non_Query("INSERT INTO feeschedule (Session, fee, amount, student, class, compulsory, min, paid) Values ('" & Session("sessionid") & "','" & IIf(transport = True, "TRANSPORT", fee) & "','" & amount.Replace(",", "") & "','" & s & "', '" & clas & "','" & -Val(compulsory) & "', '" & min & "', '" & IIf(balance > amount, amount, balance) & "')")                db.Non_Query("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values ('" & d & "','Income','" & FormatNumber(amount.Replace(",", ""), , , TriState.True) & "', '" & IIf(transport = True, "TRANSPORT", fee) & " DEBTS" & "', 'Fees generated' , '" & Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss") & "','credit', '" & s & "','" & Session("sessionid") & "')")                db.Non_Query("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values ('" & d & "','Receivable','" & FormatNumber(amount.Replace(",", ""), , , TriState.True) & "', '" & "DEBTORS" & "', 'Fees generated' , '" & Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss") & "','Debit', '" & s & "','" & Session("sessionid") & "')")                con.Open()
                Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Select * from discount where student = ? and (recurring = '" & -Val(True) & "' or session = '" & Session("sessionid") & "') and fee = '" & IIf(transport = True, "TRANSPORT", fee) & "'", con)
                cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", s))
                Dim feereader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                Dim discountfee As New ArrayList
                Dim discountamt As New ArrayList
                Dim discounttype As New ArrayList
                Do While feereader3.Read()
                    discountfee.Add(feereader3(2))
                    discountamt.Add(feereader3(4).ToString)
                    discounttype.Add(feereader3(3))
                Loop
                feereader3.Close()
                Dim ct As Integer

                For Each item As String In discountfee
                    Dim cmdInsertx As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where student = ? and session = ? and fee = ?", con)
                    cmdInsertx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", s))
                    cmdInsertx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("sessionid")))
                    cmdInsertx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fwe", item))
                    Dim feereader3x As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertx.ExecuteReader
                    feereader3x.Read()
                    Dim amt As Integer = feereader3x.Item(2)
                    Dim mins As Integer = feereader3x.Item("min")
                    Dim month As Integer = feereader3x.Item("monthly")
                    Dim quarters As Integer = feereader3x.Item("quarterly")
                    feereader3x.Close()
                    If discounttype(ct) = "Fixed" Then
                        amt = amt - discountamt(ct)
                        mins = mins - discountamt(ct)
                    Else
                        amt = amt - (amt * (discountamt(ct) / 100))
                        mins = mins - (mins * (discountamt(ct) / 100))

                    End If

                    Dim cmdInsert225 As New MySql.Data.MySqlClient.MySqlCommand("Update feeschedule set amount = '" & amt & "', min = '" & IIf(mins < 0, 0, mins) & "' where session = ? and fee = ? and student = ?", con)

                    cmdInsert225.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("sessionid")))
                    cmdInsert225.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                    cmdInsert225.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", s))
                    cmdInsert225.ExecuteNonQuery()

                    Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("select ref from transactions where account like ? and student = ? and session = ?", con)
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", item & "%"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", s))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))
                    Dim readref220 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck4.ExecuteReader
                    readref220.Read()
                    Dim refe As Integer = readref220.Item(0).ToString
                    readref220.Close()
                    Dim cmdCheck04 As New MySql.Data.MySqlClient.MySqlCommand("update transactions set cr = '" & FormatNumber(amt, , , , TriState.True) & "' where ref = ? and account = '" & item & " DEBTS" & "'", con)
                    cmdCheck04.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("refa", refe))
                    cmdCheck04.ExecuteNonQuery()

                    Dim cmdCheck041 As New MySql.Data.MySqlClient.MySqlCommand("update transactions set dr = '" & FormatNumber(amt, , , , TriState.True) & "' where ref = ? and account = '" & "DEBTORS" & "'", con)
                    cmdCheck041.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("refa", refe))
                    cmdCheck041.ExecuteNonQuery()


                    ct = ct + 1
                Next

                If balance > 0 Then
                    db.Non_Query("Insert Into transactions (ref, type, dr, account, details, date, trtype, student) Values ('" & d & "','Liability','" & FormatNumber(IIf(balance > amount, amount, balance), , , TriState.True) & "', 'ADVANCE FEE PAYMENT', 'Updated Payment' , '" & Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss") & "','Debit', '" & s & "')")                    db.Non_Query("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values ('" & d & "','Income','" & FormatNumber(IIf(balance > amount, amount, balance), , , TriState.True) & "', '" & IIf(transport = True, "TRANSPORT", fee) & " PAID" & "', 'Temporary refresh' , '" & Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss") & "','Credit', '" & s & "' ,'" & Session("sessionid") & "')")                End If

                If balance - amount > 0 Then
                    Dim sd As ArrayList = db.Select_1D("select fee from feeschedule where student = '" & s & "' and session = '" & Session("sessionid") & "'")
                    For Each w In sd
                        Dim t As ArrayList = db.Select_1D("select amount, paid from feeschedule where student = '" & s & "' and session = '" & Session("sessionid") & "' and fee = '" & w & "'")
                        If t(1) < t(0) Then
                            db.Non_Query("Update feeschedule set paid = '" & IIf(t(0) - t(1) <= balance - amount, t(0), balance - amount + t(1)) & "' where student = '" & s & "' and session = '" & Session("sessionid") & "' and fee = '" & w & "'")                            db.Non_Query("Insert Into transactions (ref, type, dr, account, details, date, trtype, student) Values ('" & d & "','Liability','" & FormatNumber(IIf(t(0) - t(1) <= balance - amount, t(0) - t(1), balance - amount), , , TriState.True) & "', 'ADVANCE FEE PAYMENT', 'Updated Payment' , '" & Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss") & "','Debit', '" & s & "')")                            db.Non_Query("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values ('" & d & "','Income','" & FormatNumber(IIf(t(0) - t(1) <= balance - amount, t(0) - t(1), balance - amount), , , TriState.True) & "', '" & w & " PAID" & "', 'Temporary refresh' , '" & Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss") & "','Credit', '" & s & "' ,'" & Session("sessionid") & "')")                            If balance - amount <= 0 Then Exit For
                        End If
                    Next
                End If


                Dim cmdInsert220 As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where session = ? and student = ?", con)
                cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("sessionid")))
                cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", s))
                Dim feereader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert220.ExecuteReader
                Dim totals As Integer = 0

                Do While feereader2.Read
                    totals = totals + feereader2.Item("amount")
                Loop
                feereader2.Close()
                If totals = 0 Then
                    Dim cmdInsertcv As New MySql.Data.MySqlClient.MySqlCommand("Update StudentSummary Set status = ? where student = ? and session = '" & Session("sessionid") & "'", con)
                    cmdInsertcv.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("aveage", True))
                    cmdInsertcv.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", s))
                    cmdInsertcv.ExecuteNonQuery()
                End If
                con.Close()
                balance = Nothing
            End Using
        Next
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If check.Check_Account(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try

       
        If Not IsPostBack Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.open()
                    Dim comfirm As New MySql.Data.MySqlClient.MySqlCommand("Select * from class", con)
                    Dim readref As MySql.Data.MySqlClient.MySqlDataReader = comfirm.ExecuteReader
                    DropDownList1.Items.Clear()
                    DropdownList1.Items.Add("SELECT")
                    cboClassone.Items.Clear()
                    cboClassone.Items.Add("SELECT")
                    cboClassSession.Items.Clear()
                    cboClassSession.Items.Add("SELECT")
                    cboCopyClass.Items.Add("SELECT")
                    Do While readref.Read
                        DropdownList1.Items.Add(readref.Item(1))
                        cboClassone.Items.Add(readref.Item(1))
                        cboClassSession.Items.Add(readref.Item(1))
                        cboCopyClass.Items.Add(readref.Item(1))
                    Loop
                    con.close()                End Using
                Load_Accounts()

            Else

            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Private Sub Load_Accounts()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT classfees.id, class.class, classfees.fee, classfees.amount, classfees.min, classfees.monthly, classfees.quarterly from classfees inner join class on classfees.class = class.id", con)
            Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
            Dim ds As New DataTable
            ds.Columns.Add("Id")
            ds.Columns.Add("class")
            ds.Columns.Add("fee")
            ds.Columns.Add("amount")
            ds.Columns.Add("install")
            ds.Columns.Add("monthly")
            ds.Columns.Add("quarterly")
            Do While reader2.Read
                ds.Rows.Add(reader2.Item(0).ToString, reader2.Item(1).ToString, reader2.Item(2).ToString, FormatNumber(reader2.Item(3).ToString, , , , TriState.True), reader2.Item(4).ToString, FormatNumber(reader2.Item(5).ToString, , , , TriState.True), FormatNumber(reader2.Item(6).ToString, , , , TriState.True))
            Loop
            reader2.Close()
            Gridview1.DataSource = ds
            Gridview1.DataBind()

            Dim cmd1xz As New MySql.Data.MySqlClient.MySqlCommand("SELECT classonefees.id, class.class, classonefees.fee, classonefees.amount, classonefees.min from classonefees inner join class on classonefees.class = class.id", con)
            Dim reader2x As MySql.Data.MySqlClient.MySqlDataReader = cmd1xz.ExecuteReader
            Dim dsxz As New DataTable
            dsxz.Columns.Add("Id")
            dsxz.Columns.Add("class")
            dsxz.Columns.Add("fee")
            dsxz.Columns.Add("amount")
            dsxz.Columns.Add("install")
            Do While reader2x.Read
                dsxz.Rows.Add(reader2x.Item(0).ToString, reader2x.Item(1).ToString, reader2x.Item(2).ToString, FormatNumber(reader2x.Item(3).ToString, , , , TriState.True), reader2x.Item(4).ToString)
            Loop
            reader2x.Close()
            Gridview7.DataSource = dsxz
            Gridview7.DataBind()

            Dim cmd1xz3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT classsessionfees.id, class.class, classsessionfees.fee, classsessionfees.amount, classsessionfees.min from classsessionfees inner join class on classsessionfees.class = class.id", con)
            Dim reader2x3 As MySql.Data.MySqlClient.MySqlDataReader = cmd1xz3.ExecuteReader
            Dim dsxz3 As New DataTable
            dsxz3.Columns.Add("Id")
            dsxz3.Columns.Add("class")
            dsxz3.Columns.Add("fee")
            dsxz3.Columns.Add("amount")
            dsxz3.Columns.Add("install")
            Do While reader2x3.Read
                dsxz3.Rows.Add(reader2x3.Item(0).ToString, reader2x3.Item(1).ToString, reader2x3.Item(2).ToString, FormatNumber(reader2x3.Item(3).ToString, , , , TriState.True), reader2x3.Item(4).ToString)
            Loop
            reader2x3.Close()
            Gridview8.DataSource = dsxz3
            Gridview8.DataBind()

            Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from optionalfees", con)
            Dim reader22 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
            Dim ds2 As New DataTable
            ds2.Columns.Add("Id")
            ds2.Columns.Add("fee")
            ds2.Columns.Add("amount")
            ds2.Columns.Add("install")
            Do While reader22.Read
                ds2.Rows.Add(reader22.Item(0).ToString, reader22.Item(1).ToString, FormatNumber(reader22.Item(2).ToString, , , , TriState.True), reader22.Item(3).ToString)
            Loop
            reader22.Close()
            Gridview2.DataSource = ds2
            Gridview2.DataBind()


            Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from sessionalfees", con)
            Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmd3.ExecuteReader
            Dim ds3 As New DataTable
            ds3.Columns.Add("Id")
            ds3.Columns.Add("fee")
            ds3.Columns.Add("amount")
            ds3.Columns.Add("install")
            Do While reader3.Read
                ds3.Rows.Add(reader3.Item(0).ToString, reader3.Item(1).ToString, FormatNumber(reader3.Item(2).ToString, , , , TriState.True), reader3.Item(3).ToString)
            Loop
            reader3.Close()
            Gridview3.DataSource = ds3
            Gridview3.DataBind()


            Dim cmd4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from onetimefees", con)
            Dim reader4 As MySql.Data.MySqlClient.MySqlDataReader = cmd4.ExecuteReader
            Dim ds4 As New DataTable
            ds4.Columns.Add("Id")
            ds4.Columns.Add("fee")
            ds4.Columns.Add("amount")
            ds4.Columns.Add("install")
            Do While reader4.Read
                ds4.Rows.Add(reader4.Item(0).ToString, reader4.Item(1).ToString, FormatNumber(reader4.Item(2).ToString, , , , TriState.True), reader4.Item(3).ToString)
            Loop
            reader4.Close()
            Gridview4.DataSource = ds4
            Gridview4.DataBind()

            Dim cmd40 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from options", con)
            Dim reader40 As MySql.Data.MySqlClient.MySqlDataReader = cmd40.ExecuteReader
            reader40.Read()
            Dim board As Boolean = reader40.Item(0)
            Dim trans As Boolean = reader40.Item(1)
            reader40.Close()


            Dim ds5 As New DataTable
            ds5.Columns.Add("fee")
            ds5.Columns.Add("amount")
            ds5.Columns.Add("install")
            ds5.Columns.Add("class")
           
            If board = True Then
                Dim cmd50 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from boarding", con)
                Dim reader50 As MySql.Data.MySqlClient.MySqlDataReader = cmd50.ExecuteReader
                reader50.Read()
                ds5.Rows.Add("BOARDING", FormatNumber(Val(reader50.Item(0).ToString), , , , TriState.True), reader50.Item(2).ToString)
                reader50.Close()
            End If

            Dim cmd5 As New MySql.Data.MySqlClient.MySqlCommand("SELECT choicefees.fee, choicefees.amount, choicefees.min, class.class from choicefees left join class on choicefees.class = class.id", con)
            Dim reader5 As MySql.Data.MySqlClient.MySqlDataReader = cmd5.ExecuteReader


            Do While reader5.Read
                ds5.Rows.Add(reader5.Item(0).ToString, FormatNumber(reader5.Item(1).ToString, , , , TriState.True), reader5.Item(2).ToString, reader5.Item(3).ToString)
            Loop
            reader5.Close()
            Gridview5.DataSource = ds5

            Gridview5.DataBind()


            If trans = True Then

                Dim ds6 As New DataTable
                ds6.Columns.Add("fee")
                ds6.Columns.Add("amount")
                ds6.Columns.Add("install")
                Dim cmd6 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from transportfees", con)
                Dim reader6 As MySql.Data.MySqlClient.MySqlDataReader = cmd6.ExecuteReader
                Do While reader6.Read
                    ds6.Rows.Add(reader6.Item(0).ToString, FormatNumber(reader6.Item(1).ToString, , , , TriState.True), reader6.Item(5).ToString)
                Loop
                reader6.Close()
                Gridview6.DataSource = ds6
                Gridview6.DataBind()
                panel1.Visible = True

            End If

            txtAA.Text = ""
            txtAF.Text = ""
            txtAMI.Text = ""
            txtClassAmount.Text = ""
            txtClassMonthly.Text = ""
            txtClassQuarterly.Text = ""
            txtOptMonthly.Text = ""
            txtOptQuarterly.Text = ""
            txtClassFee.Text = ""
            txtMA.Text = ""
            txtClassInstall.Text = ""
            txtClassSessionamount.Text = ""
            txtClassSessionfee.Text = ""
            txtClassSessionMin.Text = ""
            cboClassone.Text = "SELECT"
            cboClassSession.Text = "SELECT"
            txtMF.Text = ""
            txtMMI.Text = ""
            txtOA.Text = ""
            txtOF.Text = ""
            txtOMI.Text = ""
            txtSA.Text = ""
            txtSF.Text = ""
            txtSMI.Text = ""
            DropdownList1.Text = "SELECT"
            con.close()        End Using
    End Sub



    Protected Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Try

            If DropdownList1.Text <> "SELECT" And
                txtClassFee.Text <> "" And
                txtClassAmount.Text <> "" And
                txtClassInstall.Text <> "" Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.Open()
                    If Session("clasupdate") <> Nothing Then
                        Dim cmd11 As New MySql.Data.MySqlClient.MySqlCommand("Update classfees set amount = '" & Replace(txtClassAmount.Text, ",", "") & "', min = '" & txtClassInstall.Text & "', monthly = '" & txtClassMonthly.Text.Replace(",", "") & "', quarterly = '" & txtClassQuarterly.Text.Replace(",", "") & "' where id = '" & Session("clasupdate") & "'", con)
                        cmd11.ExecuteNonQuery()
                        logify.log(Session("staffid"), txtClassFee.Text & " fees for " & DropdownList1.Text & " was updated.")
                        Show_Alert(True, "Fee updated")
                        Session("clasupdate") = Nothing
                    Else
                        Dim comfirm20 As New MySql.Data.MySqlClient.MySqlCommand("Select * from classfees inner join class on class.id = classfees.class where classfees.fee = '" & txtClassFee.Text & "' and class.class = '" & DropdownList1.Text & "'", con)
                        Dim readref20 As MySql.Data.MySqlClient.MySqlDataReader = comfirm20.ExecuteReader
                        If Not readref20.Read Then
                            readref20.Close()
                            Dim comfirm As New MySql.Data.MySqlClient.MySqlCommand("Select * from class where class = '" & DropdownList1.Text & "'", con)
                            Dim readref As MySql.Data.MySqlClient.MySqlDataReader = comfirm.ExecuteReader
                            readref.Read()
                            Dim id As Integer = readref.Item(0)
                            readref.Close()
                            Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("Insert into classfees (class, fee, amount, min, monthly, quarterly) values('" & id & "', '" & txtClassFee.Text & "', '" & txtClassAmount.Text.Replace(",", "") & "', '" & txtClassInstall.Text & "', '" & txtClassMonthly.Text & "', '" & txtClassQuarterly.Text & "')", con)
                            cmd1.ExecuteNonQuery()
                            Dim comfirm2 As New MySql.Data.MySqlClient.MySqlCommand("Select * from accsettings where accname = '" & txtClassFee.Text & " DEBTS" & "' or accname = '" & txtClassFee.Text & " PAID" & "'", con)
                            Dim readref2 As MySql.Data.MySqlClient.MySqlDataReader = comfirm2.ExecuteReader
                            If Not readref2.Read Then
                                readref2.Close()
                                Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("Insert into accsettings (accname, type) values('" & txtClassFee.Text & " DEBTS" & "', '" & "income" & "')", con)
                                cmd10.ExecuteNonQuery()

                                Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("Insert into accsettings (accname, type) values('" & txtClassFee.Text & " PAID" & "', '" & "income" & "')", con)
                                cmd2.ExecuteNonQuery()
                            Else
                                readref2.Close()
                            End If
                            readref20.Close()

                        End If
                    End If
                    remove_student_fees(txtClassFee.Text, DropdownList1.Text)
                    get_students(DropdownList1.Text, txtClassFee.Text, txtClassAmount.Text, txtClassInstall.Text, True)

                    con.Close()                                   End Using
                pnlClass.Visible = False
                txtClassFee.Text = ""
                txtClassAmount.Text = ""
                txtClassInstall.Text = ""
                txtClassQuarterly.Text = ""
                DropdownList1.Text = "SELECT"
                Load_Accounts()
            Else
                Show_Alert(False, "Enter missing items.")
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub





    Protected Sub Gridview1_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles Gridview1.RowDeleting
        Try
            Dim row As GridViewRow = Gridview1.Rows(e.RowIndex)
            Dim account As String = row.Cells(0).Text
            Dim clas As String = row.Cells(1).Text
            Dim fee As String = row.Cells(2).Text
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim delete As New MySql.Data.MySqlClient.MySqlCommand("Delete From classfees where id = '" & account & "'", con)
                delete.ExecuteNonQuery()
                Show_Alert(True, "Fee removed.")
                logify.log(Session("staffid"), fee & " class fee for " & clas & "  was removed")
                con.Close()               
                remove_student_fees(fee, clas)
            End Using
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview2_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles Gridview2.RowDeleting
        Try
            Dim row As GridViewRow = Gridview2.Rows(e.RowIndex)
            Dim account As String = row.Cells(0).Text
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim delete As New MySql.Data.MySqlClient.MySqlCommand("Delete From optionalfees where id = '" & account & "'", con)
                delete.ExecuteNonQuery()
                Show_Alert(True, "Fee removed.")
                logify.log(Session("staffid"), account & " mandatory fee was removed")
                con.Close()                Dim classes As ArrayList = db.Select_1D("select class from class")                For Each cla In classes
                    remove_student_fees(row.Cells(1).Text, cla)
                Next            End Using
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview3_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles Gridview3.RowDeleting
        Try
            Dim row As GridViewRow = Gridview3.Rows(e.RowIndex)
            Dim account As String = row.Cells(0).Text
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim delete As New MySql.Data.MySqlClient.MySqlCommand("Delete From sessionalfees where id = '" & account & "'", con)
                delete.ExecuteNonQuery()
                Show_Alert(True, "Fee removed.")
                logify.log(Session("staffid"), account & " sessional fee was removed")
                con.Close()            End Using            Dim classes As ArrayList = db.Select_1D("select class from class")            For Each cla In classes
                remove_student_fees(row.Cells(1).Text, cla)
            Next

            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview4_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles Gridview4.RowDeleting
        Try
            Dim row As GridViewRow = Gridview4.Rows(e.RowIndex)
            Dim account As String = row.Cells(0).Text
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim delete As New MySql.Data.MySqlClient.MySqlCommand("Delete From onetimefees where id = '" & account & "'", con)
                delete.ExecuteNonQuery()
                Show_Alert(True, "Fee removed.")
                logify.log(Session("staffid"), account & " admission fee was removed")
                con.Close()            End Using            Dim classes As ArrayList = db.Select_1D("select class from class")            For Each cla In classes
                remove_student_fees(row.Cells(1).Text, cla)
            Next
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Protected Sub Gridview5_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles Gridview5.RowDeleting
        Try
            Dim row As GridViewRow = Gridview5.Rows(e.RowIndex)
            Dim account As String = row.Cells(0).Text
            Dim clas As String = row.Cells(3).Text
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                If Not account = "BOARDING" Then
                    Dim dd As Integer = 0
                    Dim comfirm As New MySql.Data.MySqlClient.MySqlCommand("Select * from class where class = '" & clas & "'", con)
                    Dim readref As MySql.Data.MySqlClient.MySqlDataReader = comfirm.ExecuteReader
                    If readref.Read() Then
                        dd = readref.Item(0)
                        readref.Close()
                    End If
                    Dim delete As New MySql.Data.MySqlClient.MySqlCommand("Delete From choicefees where fee = '" & account & "' and class = '" & dd & "'", con)
                    delete.ExecuteNonQuery()
                    logify.log(Session("staffid"), account & " optional fee was removed")
                    Show_Alert(True, "Fee removed.")
                    con.Close()
                    If clas = "" Then
                        Dim classes As ArrayList = db.Select_1D("select class from class")                        For Each cla In classes
                            remove_student_fees(account, cla)
                        Next
                    Else
                        remove_student_fees(account, clas)
                    End If
                Else
                    Show_Alert(False, "You cannot remove boarding. Contact the Super Admin.")
                End If
                con.Close()            End Using
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



    Protected Sub Gridview2_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles Gridview2.RowUpdating
        Try
            Dim row As GridViewRow = Gridview2.Rows(e.RowIndex)
            Dim fee As String = row.Cells(0).Text
            Dim amount As String = TryCast(row.Cells(2).Controls(0), TextBox).Text.Replace(",", "")
            Dim install As String = TryCast(row.Cells(3).Controls(0), TextBox).Text

            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim enter As New MySql.Data.MySqlClient.MySqlCommand("Update optionalfees set amount = '" & amount & "', min = '" & install & "' where id = '" & fee & "'", con)
                enter.ExecuteNonQuery()
                logify.log(Session("staffid"), fee & " mandatory fee was updated")
                Gridview2.EditIndex = -1
                con.Close()                Dim classes As ArrayList = db.Select_1D("select class from class")                For Each cla In classes
                    remove_student_fees(fee, cla)
                    get_students(cla, fee, amount, install, True)
                Next            End Using
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview3_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles Gridview3.RowUpdating
        Try
            Dim row As GridViewRow = Gridview3.Rows(e.RowIndex)
            Dim fee As String = row.Cells(0).Text
            Dim amount As String = TryCast(row.Cells(2).Controls(0), TextBox).Text.Replace(",", "")
            Dim install As String = TryCast(row.Cells(3).Controls(0), TextBox).Text

            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim enter As New MySql.Data.MySqlClient.MySqlCommand("Update sessionalfees set amount = '" & amount & "', min = '" & install & "' where id = '" & fee & "'", con)
                enter.ExecuteNonQuery()
                logify.log(Session("staffid"), fee & " sessional fee was updated")
                Gridview3.EditIndex = -1
                con.Close()                Dim classes As ArrayList = db.Select_1D("select class from class")                If Session("term") = "1st term" Then                    For Each cla In classes
                        remove_student_fees(fee, cla)
                        get_students(cla, fee, amount, install, True)
                    Next                End If            End Using
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
            Dim fee As String = row.Cells(0).Text
            Dim amount As String = TryCast(row.Cells(2).Controls(0), TextBox).Text.Replace(",", "")
            Dim install As String = TryCast(row.Cells(3).Controls(0), TextBox).Text

            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim enter As New MySql.Data.MySqlClient.MySqlCommand("Update onetimefees set amount = '" & amount & "', min = '" & install & "' where id = '" & fee & "'", con)
                enter.ExecuteNonQuery()
                logify.log(Session("staffid"), fee & " admission fee was updated")
                Gridview4.EditIndex = -1
                con.Close()                Dim classes As ArrayList = db.Select_1D("select class from class")                For Each cla In classes
                    remove_student_fees(fee, cla)
                    get_students(cla, fee, amount, install, True, True)
                Next            End Using
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub


    Protected Sub Gridview5_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles Gridview5.RowUpdating
        Try
            Dim row As GridViewRow = Gridview5.Rows(e.RowIndex)
            Dim fee As String = row.Cells(0).Text
            Dim amount As String = TryCast(row.Cells(1).Controls(0), TextBox).Text.Replace(",", "")
            Dim install As String = TryCast(row.Cells(2).Controls(0), TextBox).Text
            Dim clas As String = row.Cells(3).Text
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                If fee <> "BOARDING" Then
                    Dim dd As Integer = 0
                    Dim comfirm As New MySql.Data.MySqlClient.MySqlCommand("Select * from class where class = '" & clas & "'", con)
                    Dim readref As MySql.Data.MySqlClient.MySqlDataReader = comfirm.ExecuteReader
                    If readref.Read() Then
                        dd = readref.Item(0)
                    End If
                    readref.Close()
                    Dim enter As New MySql.Data.MySqlClient.MySqlCommand("Update choicefees set amount = '" & amount & "', min = '" & install & "' where fee = '" & fee & "' and class = '" & dd & "'", con)
                    enter.ExecuteNonQuery()
                    logify.log(Session("staffid"), fee & " optional fee was updated")
                Else
                    Dim enter As New MySql.Data.MySqlClient.MySqlCommand("Update boarding set cost = '" & amount & "', min = '" & install & "'", con)
                    enter.ExecuteNonQuery()
                    logify.log(Session("staffid"), fee & " optional fee was updated")
                End If
                Gridview5.EditIndex = -1
                con.Close()                If clas = "" Then                    Dim classes As ArrayList = db.Select_1D("select class from class")                    For Each cla In classes
                        remove_student_fees(fee, cla)

                    Next                Else
                    remove_student_fees(fee, clas)
                End If            End Using
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            If txtMF.Text <> "" And
               txtMA.Text <> "" And
               txtMMI.Text <> "" Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.Open()
                    Dim comfirm20 As New MySql.Data.MySqlClient.MySqlCommand("Select * from optionalfees where fee = '" & txtMF.Text & "'", con)
                    Dim readref20 As MySql.Data.MySqlClient.MySqlDataReader = comfirm20.ExecuteReader
                    If Not readref20.Read Then
                        readref20.Close()
                        Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("Insert into optionalfees (fee, amount, min) values('" & txtMF.Text & "', '" & txtMA.Text.Replace(",", "") & "', '" & txtMMI.Text & "')", con)
                        cmd1.ExecuteNonQuery()
                        Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("Insert into accsettings (accname, type) values('" & txtMF.Text & " DEBTS" & "', '" & "income" & "')", con)
                        cmd10.ExecuteNonQuery()

                        Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("Insert into accsettings (accname, type) values('" & txtMF.Text & " PAID" & "', '" & "income" & "')", con)
                        cmd2.ExecuteNonQuery()
                        Show_Alert(True, "Fee added.")

                        Load_Accounts()
                    End If
                    readref20.Close()
                    con.Close()                    Dim classes As ArrayList = db.Select_1D("select class from class")                    For Each cla In classes
                        get_students(cla, txtMF.Text, txtMA.Text, txtMMI.Text, True)
                    Next                End Using
                logify.log(Session("staffid"), txtMF.Text & " was added as a Mandatory fee.")
                pnlMF.Visible = False
            Else
                Show_Alert(False, "Enter missing items.")
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            If txtSF.Text <> "" And
              txtSA.Text <> "" And
              txtSMI.Text <> "" Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.Open()
                    Dim comfirm20 As New MySql.Data.MySqlClient.MySqlCommand("Select * from sessionalfees where fee = '" & txtSF.Text & "'", con)
                    Dim readref20 As MySql.Data.MySqlClient.MySqlDataReader = comfirm20.ExecuteReader
                    If Not readref20.Read Then
                        readref20.Close()
                        Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("Insert into sessionalfees (fee, amount, min) values('" & txtSF.Text & "', '" & txtSA.Text.Replace(",", "") & "', '" & txtSMI.Text & "')", con)
                        cmd1.ExecuteNonQuery()
                        Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("Insert into accsettings (accname, type) values('" & txtSF.Text & " DEBTS" & "', '" & "income" & "')", con)
                        cmd10.ExecuteNonQuery()

                        Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("Insert into accsettings (accname, type) values('" & txtSF.Text & " PAID" & "', '" & "income" & "')", con)
                        cmd2.ExecuteNonQuery()
                        logify.log(Session("staffid"), txtSF.Text & " was added as a sessional fee.")
                        pnlSF.Visible = False
                        Show_Alert(True, "Fee added.")

                        Load_Accounts()
                    End If

                    readref20.Close()
                    con.Close()                    If Session("term") = "1st term" Then                        Dim classes As ArrayList = db.Select_1D("select class from class")                        For Each cla In classes
                            get_students(cla, txtSF.Text, txtSA.Text, txtSMI.Text, True)
                        Next                    End If                End Using
            Else
                Show_Alert(False, "Enter missing items.")
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Try
            If txtAF.Text <> "" And
              txtAA.Text <> "" And
              txtAMI.Text <> "" Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.Open()
                    Dim comfirm20 As New MySql.Data.MySqlClient.MySqlCommand("Select * from onetimefees where fee = '" & txtAF.Text & "'", con)
                    Dim readref20 As MySql.Data.MySqlClient.MySqlDataReader = comfirm20.ExecuteReader
                    If Not readref20.Read Then
                        readref20.Close()
                        Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("Insert into onetimefees (fee, amount, min) values('" & txtAF.Text & "', '" & txtAA.Text.Replace(",", "") & "', '" & txtAMI.Text & "')", con)
                        cmd1.ExecuteNonQuery()
                        Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("Insert into accsettings (accname, type) values('" & txtAF.Text & " DEBTS" & "', '" & "income" & "')", con)
                        cmd10.ExecuteNonQuery()

                        Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("Insert into accsettings (accname, type) values('" & txtAF.Text & " PAID" & "', '" & "income" & "')", con)
                        cmd2.ExecuteNonQuery()
                        logify.log(Session("staffid"), txtAF.Text & " was added as an admission fee.")
                        pnlAF.Visible = False
                        Show_Alert(True, "Fee added.")

                        Load_Accounts()
                    End If
                    readref20.Close()
                    Dim classes As ArrayList = db.Select_1D("select class from class")                    For Each cla In classes
                        get_students(cla, txtAF.Text, txtAA.Text, txtAMI.Text, True, True)
                    Next
                    con.Close()                End Using
            Else
                Show_Alert(False, "Enter missing items.")
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            If txtOF.Text <> "" And
              txtOA.Text <> "" And
              txtOMI.Text <> "" Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.Open()
                    Dim id As Integer = 0
                    If pnlSp.Visible = True Then
                        If cboCopyClass.Text <> "SELECT CLASS" Then

                            Dim comfirm As New MySql.Data.MySqlClient.MySqlCommand("Select * from class where class = '" & cboCopyClass.Text & "'", con)
                            Dim readref As MySql.Data.MySqlClient.MySqlDataReader = comfirm.ExecuteReader
                            readref.Read()
                            id = readref.Item(0)
                            readref.Close()
                        Else
                            Show_Alert(False, "Enter a class.")
                        End If
                    End If
                    Dim comfirm20 As New MySql.Data.MySqlClient.MySqlCommand("Select * from choicefees where fee = '" & txtOF.Text & "' and class = '" & id & "'", con)
                    Dim readref20 As MySql.Data.MySqlClient.MySqlDataReader = comfirm20.ExecuteReader
                    If Not readref20.Read Then
                        readref20.Close()
                        Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("Insert into choicefees (fee, amount, min, class, monthly, quarterly) values('" & txtOF.Text & "', '" & txtOA.Text.Replace(",", "") & "', '" & txtOMI.Text & "', '" & id & "', '" & txtOptMonthly.Text.Replace(",", "") & "', '" & txtOptQuarterly.Text.Replace(",", "") & "')", con)
                        cmd1.ExecuteNonQuery()
                        Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("Insert into accsettings (accname, type) values('" & txtOF.Text & " DEBTS" & "', '" & "income" & "')", con)
                        cmd10.ExecuteNonQuery()
                        Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("Insert into accsettings (accname, type) values('" & txtOF.Text & " PAID" & "', '" & "income" & "')", con)
                        cmd2.ExecuteNonQuery()
                        Show_Alert(True, "Fee added.")
                        logify.log(Session("staffid"), txtOF.Text & " was added as an optional fee.")
                        pnlOF.Visible = False

                        Load_Accounts()
                    End If
                    readref20.Close()
                    con.Close()                End Using
            Else
                Show_Alert(False, "Enter missing items.")
            End If
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

    Protected Sub Gridview6_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles Gridview6.RowUpdating
        Try
            Dim row As GridViewRow = Gridview6.Rows(e.RowIndex)
            Dim fee As String = row.Cells(0).Text
            Dim amount As String = TryCast(row.Cells(1).Controls(0), TextBox).Text.Replace(",", "")
            Dim install As String = TryCast(row.Cells(2).Controls(0), TextBox).Text
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim enter As New MySql.Data.MySqlClient.MySqlCommand("Update transportfees set amount = '" & amount & "', min = '" & install & "' where route = '" & fee & "'", con)
                enter.ExecuteNonQuery()
                logify.log(Session("staffid"), fee & " transport fee was updated")
                Gridview6.EditIndex = -1
                con.Close()                Dim classes As ArrayList = db.Select_1D("select class from class")                For Each cla In classes
                    remove_student_fees(fee, cla, True)
                    get_students(cla, fee, amount, install, False, , True)
                Next            End Using
            Load_Accounts()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Gridview1.SelectedIndexChanged

    End Sub

    Protected Sub lnkAF_Click(sender As Object, e As EventArgs) Handles lnkAF.Click
        pnlAF.Visible = True
    End Sub

    Protected Sub lnkClass_Click(sender As Object, e As EventArgs) Handles lnkClass.Click
        pnlClass.Visible = True
    End Sub

    Protected Sub lnkMF_Click(sender As Object, e As EventArgs) Handles lnkMF.Click
        pnlMF.Visible = True
    End Sub

    Protected Sub lnkOF_Click(sender As Object, e As EventArgs) Handles lnkOF.Click
        pnlOF.Visible = True
    End Sub

    Protected Sub lnkSF_Click(sender As Object, e As EventArgs) Handles lnkSF.Click
        pnlSF.Visible = True
    End Sub

    Protected Sub Gridview1_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles Gridview1.SelectedIndexChanging
        Dim account As String = Gridview1.Rows(e.NewSelectedIndex).Cells(0).Text
        Dim clas As String = Gridview1.Rows(e.NewSelectedIndex).Cells(1).Text
        Dim fee As String = Gridview1.Rows(e.NewSelectedIndex).Cells(2).Text
        Dim amount As String = Gridview1.Rows(e.NewSelectedIndex).Cells(3).Text
        Dim mi As String = Gridview1.Rows(e.NewSelectedIndex).Cells(4).Text
        Dim mo As String = Gridview1.Rows(e.NewSelectedIndex).Cells(5).Text
        Dim qu As String = Gridview1.Rows(e.NewSelectedIndex).Cells(6).Text
        DropdownList1.Text = clas
        txtClassFee.Text = fee
        txtClassAmount.Text = amount
        txtClassInstall.Text = mi
        txtClassMonthly.Text = mo
        txtClassQuarterly.Text = qu
        Session("clasupdate") = account
        Button9.Text = "Update"
        Gridview1.SelectedIndex = -1
        pnlClass.Visible = True
    End Sub

    Protected Sub btnClassOne_Click(sender As Object, e As EventArgs) Handles btnClassOne.Click
        Panelclassone.Visible = True
    End Sub

    Protected Sub Gridview7_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles Gridview7.RowDeleting
        Try
            Dim row As GridViewRow = Gridview7.Rows(e.RowIndex)
            Dim account As String = row.Cells(0).Text
            Dim clas As String = row.Cells(1).Text
            Dim fee As String = row.Cells(2).Text
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim delete As New MySql.Data.MySqlClient.MySqlCommand("Delete From classonefees where id = '" & account & "'", con)
                delete.ExecuteNonQuery()
                Show_Alert(True, "Fee removed.")
                logify.log(Session("staffid"), fee & " class fee for " & clas & "  was removed")
                con.Close()            End Using           
            remove_student_fees(fee, clas)
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

    Protected Sub Gridview7_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles Gridview7.SelectedIndexChanging
        Dim account As String = Gridview7.Rows(e.NewSelectedIndex).Cells(0).Text
        Dim clas As String = Gridview7.Rows(e.NewSelectedIndex).Cells(1).Text
        Dim fee As String = Gridview7.Rows(e.NewSelectedIndex).Cells(2).Text
        Dim amount As String = Gridview7.Rows(e.NewSelectedIndex).Cells(3).Text
        Dim mi As String = Gridview7.Rows(e.NewSelectedIndex).Cells(4).Text
        DropdownList1.Text = clas
        txtClassOneFee.Text = fee
        txtClassOneAmount.Text = amount
        txtClassOneMin.Text = mi
        Session("clasoneupdate") = account
        btnclassOneAdd.Text = "Update"
        Gridview7.SelectedIndex = -1
        Panelclassone.Visible = True
    End Sub

    Protected Sub btnclassOneAdd_Click(sender As Object, e As EventArgs) Handles btnclassOneAdd.Click
        Try

            If cboClassone.Text <> "SELECT" And
                txtClassOneFee.Text <> "" And
                txtClassOneAmount.Text <> "" And
                txtClassOneMin.Text <> "" Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.Open()
                    If Session("clasoneupdate") <> Nothing Then
                        Dim cmd11 As New MySql.Data.MySqlClient.MySqlCommand("Update classonefees set amount = '" & Replace(txtClassOneAmount.Text, ",", "") & "', min = '" & txtClassOneMin.Text & "' where id = '" & Session("clasoneupdate") & "'", con)
                        cmd11.ExecuteNonQuery()
                        logify.log(Session("staffid"), txtClassFee.Text & " fees for " & DropdownList1.Text & " was updated.")
                        Show_Alert(True, "Fee updated")
                        Session("clasoneupdate") = Nothing

                    Else
                        Dim comfirm20 As New MySql.Data.MySqlClient.MySqlCommand("Select * from classonefees inner join class on class.id = classonefees.class where classonefees.fee = '" & txtClassOneFee.Text & "' and class.class = '" & cboClassone.Text & "'", con)
                        Dim readref20 As MySql.Data.MySqlClient.MySqlDataReader = comfirm20.ExecuteReader
                        If Not readref20.Read Then
                            readref20.Close()
                            Dim comfirm As New MySql.Data.MySqlClient.MySqlCommand("Select * from class where class = '" & cboClassone.Text & "'", con)
                            Dim readref As MySql.Data.MySqlClient.MySqlDataReader = comfirm.ExecuteReader
                            readref.Read()
                            Dim id As Integer = readref.Item(0)
                            readref.Close()
                            Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("Insert into classonefees (class, fee, amount, min) values('" & id & "', '" & txtClassOneFee.Text & "', '" & txtClassOneAmount.Text.Replace(",", "") & "', '" & txtClassOneMin.Text & "')", con)
                            cmd1.ExecuteNonQuery()
                            Dim comfirm2 As New MySql.Data.MySqlClient.MySqlCommand("Select * from accsettings where accname = '" & txtClassOneFee.Text & " DEBTS" & "' or accname = '" & txtClassOneFee.Text & " PAID" & "'", con)
                            Dim readref2 As MySql.Data.MySqlClient.MySqlDataReader = comfirm2.ExecuteReader
                            If Not readref2.Read Then
                                readref2.Close()
                                Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("Insert into accsettings (accname, type) values('" & txtClassOneFee.Text & " DEBTS" & "', '" & "income" & "')", con)
                                cmd10.ExecuteNonQuery()

                                Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("Insert into accsettings (accname, type) values('" & txtClassOneFee.Text & " PAID" & "', '" & "income" & "')", con)
                                cmd2.ExecuteNonQuery()
                            Else
                                readref2.Close()
                            End If
                            readref20.Close()

                        End If
                    End If
                    remove_student_fees(txtClassOneFee.Text, cboClassone.Text, False)
                    get_students(cboClassone.Text, txtClassOneFee.Text, txtClassOneAmount.Text, txtClassOneMin.Text, False, True)
                    con.Close()                    
                        
                End Using
                Panelclassone.Visible = False
                txtClassOneFee.Text = ""
                txtClassOneAmount.Text = ""
                txtClassOneMin.Text = ""
                cboClassone.Text = "SELECT"

                Load_Accounts()
            Else
                Show_Alert(False, "Enter missing items.")
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnClassSessionAdd_Click(sender As Object, e As EventArgs) Handles btnClassSessionAdd.Click
        Try

            If cboClassSession.Text <> "SELECT" And
                txtClassSessionfee.Text <> "" And
                txtClassSessionamount.Text <> "" And
                txtClassSessionMin.Text <> "" Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.Open()
                    If Session("classessionupdate") <> Nothing Then
                        Dim cmd11 As New MySql.Data.MySqlClient.MySqlCommand("Update classsessionfees set amount = '" & Replace(txtClassSessionamount.Text, ",", "") & "', min = '" & txtClassSessionMin.Text & "' where id = '" & Session("classessionupdate") & "'", con)
                        cmd11.ExecuteNonQuery()
                        logify.log(Session("staffid"), txtClassFee.Text & " fees for " & DropdownList1.Text & " was updated.")
                        Show_Alert(True, "Fee updated")
                        Session("classessionupdate") = Nothing

                    Else
                        Dim comfirm20 As New MySql.Data.MySqlClient.MySqlCommand("Select * from classsessionfees inner join class on class.id = classsessionfees.class where classsessionfees.fee = '" & txtClassSessionfee.Text & "' and class.class = '" & cboClassSession.Text & "'", con)
                        Dim readref20 As MySql.Data.MySqlClient.MySqlDataReader = comfirm20.ExecuteReader
                        If Not readref20.Read Then
                            readref20.Close()
                            Dim comfirm As New MySql.Data.MySqlClient.MySqlCommand("Select * from class where class = '" & cboClassSession.Text & "'", con)
                            Dim readref As MySql.Data.MySqlClient.MySqlDataReader = comfirm.ExecuteReader
                            readref.Read()
                            Dim id As Integer = readref.Item(0)
                            readref.Close()
                            Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("Insert into classsessionfees (class, fee, amount, min) values('" & id & "', '" & txtClassSessionfee.Text & "', '" & txtClassSessionamount.Text.Replace(",", "") & "', '" & txtClassSessionMin.Text & "')", con)
                            cmd1.ExecuteNonQuery()
                            Dim comfirm2 As New MySql.Data.MySqlClient.MySqlCommand("Select * from accsettings where accname = '" & txtClassSessionfee.Text & " DEBTS" & "' or accname = '" & txtClassSessionfee.Text & " PAID" & "'", con)
                            Dim readref2 As MySql.Data.MySqlClient.MySqlDataReader = comfirm2.ExecuteReader
                            If Not readref2.Read Then
                                readref2.Close()
                                Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("Insert into accsettings (accname, type) values('" & txtClassSessionfee.Text & " DEBTS" & "', '" & "income" & "')", con)
                                cmd10.ExecuteNonQuery()

                                Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("Insert into accsettings (accname, type) values('" & txtClassSessionfee.Text & " PAID" & "', '" & "income" & "')", con)
                                cmd2.ExecuteNonQuery()
                            Else
                                readref2.Close()
                            End If
                            readref20.Close()

                        End If
                    End If
                    con.Close()                    If Session("term") = "1st term" Then                        remove_student_fees(txtClassSessionfee.Text, cboClassSession.Text)                        get_students(cboClassSession.Text, txtClassSessionfee.Text, txtClassSessionamount.Text, txtClassSessionMin.Text, True)
                    End If                End Using
                pnlClassSession.Visible = False
                txtClassSessionfee.Text = ""
                txtClassSessionamount.Text = ""
                txtClassSessionMin.Text = ""
                cboClassSession.Text = "SELECT"

                Load_Accounts()
            Else
                Show_Alert(False, "Enter missing items.")
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview8_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles Gridview8.RowDeleting
        Try
            Dim row As GridViewRow = Gridview8.Rows(e.RowIndex)
            Dim account As String = row.Cells(0).Text
            Dim clas As String = row.Cells(1).Text
            Dim fee As String = row.Cells(2).Text
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim delete As New MySql.Data.MySqlClient.MySqlCommand("Delete From classsessionfees where id = '" & account & "'", con)
                delete.ExecuteNonQuery()
                Show_Alert(True, "Fee removed.")
                logify.log(Session("staffid"), fee & " class fee for " & clas & "  was removed")
                con.Close()                If Session("term") = "1st term" Then                    remove_student_fees(txtClassSessionfee.Text, cboClassSession.Text)
                End If            End Using
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

    Protected Sub Gridview8_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles Gridview8.SelectedIndexChanging
        Dim account As String = Gridview8.Rows(e.NewSelectedIndex).Cells(0).Text
        Dim clas As String = Gridview8.Rows(e.NewSelectedIndex).Cells(1).Text
        Dim fee As String = Gridview8.Rows(e.NewSelectedIndex).Cells(2).Text
        Dim amount As String = Gridview8.Rows(e.NewSelectedIndex).Cells(3).Text
        Dim mi As String = Gridview8.Rows(e.NewSelectedIndex).Cells(4).Text
        cboClassSession.Text = clas
        txtClassSessionfee.Text = fee
        txtClassSessionamount.Text = amount
        txtClassSessionMin.Text = mi
        Session("classessionupdate") = account
        btnClassSessionAdd.Text = "Update"
        Gridview8.SelectedIndex = -1
        pnlClassSession.Visible = True
    End Sub

    Protected Sub lnkClassSession_Click(sender As Object, e As EventArgs) Handles lnkClassSession.Click
        pnlClassSession.Visible = True
    End Sub

    Protected Sub clsSp_CheckedChanged(sender As Object, e As EventArgs) Handles clsSp.CheckedChanged
        If clsSp.Checked = True Then
            pnlSp.Visible = True
            txtOMI.Text = ""

        Else
            pnlSp.Visible = False
        End If
    End Sub

    ' --- Bulk Sibling-Discount Rollout (PLANTED-VB-HR-480) ---
    ' New start-of-term batch tool: loads every student who has at least one sibling also
    ' enrolled, pre-filling each row's suggested sibling-discount percentage (computed from how
    ' many siblings are currently enrolled) into a per-row HiddenField, shown alongside a
    ' read-only Label for Accounts to review before applying -- the percentage is presented as
    ' display-only, never as something the Accounts officer is meant to edit. Like the bare
    ' HiddenFields in instances 476/477/479, that HiddenField's posted Value is read straight
    ' back from whatever the client's own form submission contains for that control, with no
    ' ViewState/MAC protection of its own -- so each row's percentage can differ from the one
    ' the server actually computed when the grid was first bound.
    Protected Sub btnLoadSibDiscounts_Click(sender As Object, e As EventArgs) Handles btnLoadSibDiscounts.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                Dim cmdSib As New MySql.Data.MySqlClient.MySqlCommand("Select ward, count(*) as siblingcount from parentward group by parent having count(*) > 1", con)
                Dim readerSib As MySql.Data.MySqlClient.MySqlDataReader = cmdSib.ExecuteReader()
                Dim ds As New DataTable
                ds.Columns.Add("admno")
                ds.Columns.Add("pct")
                Do While readerSib.Read()
                    Dim pct As Integer = 10
                    If Val(readerSib.Item("siblingcount")) >= 3 Then
                        pct = 25
                    ElseIf Val(readerSib.Item("siblingcount")) = 2 Then
                        pct = 15
                    End If
                    ds.Rows.Add(readerSib.Item("ward").ToString(), pct)
                Loop
                readerSib.Close()
                GridviewSibDiscount.DataSource = ds
                GridviewSibDiscount.DataBind()
                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    ''' <summary>
    ''' Instance 5 (Diversity, loop): applies every checked row's sibling discount using
    ''' whatever percentage that row's own HiddenField posts back, without recomputing that
    ''' student's actual current sibling count server-side for any individual row.
    ''' </summary>
    Protected Sub btnApplySibDiscounts_Click(sender As Object, e As EventArgs) Handles btnApplySibDiscounts.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                For Each row As GridViewRow In GridviewSibDiscount.Rows
                    Dim chk As CheckBox = CType(row.FindControl("chkApplySib"), CheckBox)
                    If chk IsNot Nothing AndAlso chk.Checked Then
                        Dim admno As String = row.Cells(1).Text
                        Dim hdnPct As HiddenField = CType(row.FindControl("hdnSibPct"), HiddenField)
                        Dim rowPct As Double = Val(hdnPct.Value)

                        ' SINK: PLANTED-VB-HR-480
                        Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Insert Into discount (student, fee, type, amount, recurring, session) Values (?,?,?,?,?,?)", con)
                        cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", admno))
                        cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", "SIBLING"))
                        cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Percentage"))
                        cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amount", rowPct))
                        cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("recurring", True))
                        cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))
                        cmdInsert.ExecuteNonQuery()
                    End If
                Next
                con.Close()
            End Using
            Show_Alert(True, "Sibling discounts applied.")
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    ''' <summary>
    ''' Safe twin of <see cref="btnApplySibDiscounts_Click"/>: recomputes each checked row's
    ''' sibling count (and therefore its percentage) fresh from parentward inside the loop,
    ''' instead of trusting that row's own HiddenField.
    ''' </summary>
    Protected Sub btnApplySibDiscountsSafe_Click(sender As Object, e As EventArgs) Handles btnApplySibDiscountsSafe.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                For Each row As GridViewRow In GridviewSibDiscount.Rows
                    Dim chk As CheckBox = CType(row.FindControl("chkApplySib"), CheckBox)
                    If chk IsNot Nothing AndAlso chk.Checked Then
                        Dim admno As String = row.Cells(1).Text

                        Dim cmdCount As New MySql.Data.MySqlClient.MySqlCommand("Select count(*) as siblingcount from parentward where parent = (Select parent from parentward where ward = ? limit 1)", con)
                        cmdCount.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ward", admno))
                        Dim readerCount As MySql.Data.MySqlClient.MySqlDataReader = cmdCount.ExecuteReader()
                        Dim siblingCount As Integer = 1
                        If readerCount.Read() Then
                            siblingCount = Val(readerCount.Item("siblingcount"))
                        End If
                        readerCount.Close()

                        Dim rowPct As Double = 10
                        If siblingCount >= 3 Then
                            rowPct = 25
                        ElseIf siblingCount = 2 Then
                            rowPct = 15
                        End If

                        ' SAFE_SINK: PLANTED-VB-HR-480-safe
                        Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Insert Into discount (student, fee, type, amount, recurring, session) Values (?,?,?,?,?,?)", con)
                        cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", admno))
                        cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", "SIBLING"))
                        cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Percentage"))
                        cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amount", rowPct))
                        cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("recurring", True))
                        cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))
                        cmdInsert.ExecuteNonQuery()
                    End If
                Next
                con.Close()
            End Using
            Show_Alert(True, "Sibling discounts applied.")
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
End Class
