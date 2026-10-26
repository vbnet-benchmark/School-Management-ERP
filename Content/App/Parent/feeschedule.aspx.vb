Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Account_Default
    Inherits System.Web.UI.Page
    Dim optionalfeeb As String
    Dim optionalfeef As String
    Dim optionalfeet As String
    Dim optionalfeec As New ArrayList
    Dim optionalfeenc As New ArrayList
    Dim hostel As Boolean
    Dim transport As String
    Dim feeding As String
    Dim minimum As Double
    Dim i As Integer
    Dim discountedfees As New ArrayList
    Dim discountedvalues As New ArrayList

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
    Sub load_mandatory()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()

            Dim cmdInsertf As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session Order by ID Desc", con)
            Dim reader2f As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertf.ExecuteReader
            reader2f.Read()

            Dim currentSession As String = reader2f(0).ToString
            reader2f.Close()

            Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where session = ? and student = ?", con)
            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentSession))
            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", Session("StudentAdd")))
            Dim feereader As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert22.ExecuteReader()
            BulletedList1.Items.Clear()
            BulletedList1.BulletStyle = BulletStyle.NotSet

            Do While feereader.Read
                If feereader.Item("compulsory") = 1 Then
                    BulletedList1.Items.Add(feereader.Item(1))


                    Dim label5 As New Label
                    label5.ID = "label1" & i
                    label5.Text = FormatNumber(feereader.Item(2), , , , TriState.True)
                    ammcol.Controls.Add(label5)
                    Dim literal5 As New LiteralControl
                    literal5.Text = "<br/>"
                    ammcol.Controls.Add(literal5)
                    i = i + 1
                End If

            Loop
            feereader.Close()
            Dim cmdInsert220 As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where session = ? and student = ?", con)
            cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentSession))
            cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
            Dim feereader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert220.ExecuteReader
            Dim total As Integer = 0
            Dim feetype As New ArrayList
            Dim feeamount As New ArrayList
            Dim paid As Double = 0
            Dim min As Double = 0
            Dim mon As Double = 0
            Do While feereader2.Read
                feetype.Add(feereader2.Item("fee"))
                feeamount.Add(feereader2.Item("amount"))
                total = total + feereader2.Item("amount")
                paid = paid + feereader2.Item("paid")
                min = min + feereader2.Item("min")
                mon = mon + feereader2.Item("monthly")
                If feereader2.Item("fee") = "BOARDING" And feereader2.Item("paid") > 0 Then
                    CheckBoxList1.Enabled = False
                End If
                If feereader2.Item("fee") = "TRANSPORT" And feereader2.Item("paid") > 0 Then
                    CheckBoxList2.Enabled = False
                    RadioButtonList1.Enabled = False
                End If
                For Each item As ListItem In chkChoice.Items
                    If feereader2.Item("fee") = item.Text Then
                        item.Selected = True
                        If feereader2.Item("paid") > 0 Then
                            item.Enabled = False
                        End If
                    End If
                Next
            Loop

            lblTotal.Text = FormatNumber(total, , , , TriState.True)
            lblPaid.Text = FormatNumber(paid, , , , TriState.True)
            If mon <> 0 Then
                lblMonthly.Text = FormatNumber(mon, , , , TriState.True)
            Else
                lblMonthly.Text = "N/A"
            End If
            feereader2.Close()
            Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where student = ? and session <> ?", con)
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentSession))
            Dim feereader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
            Dim totalall As Integer = 0
            Dim paidall As Double = 0

            Do While feereader3.Read

                totalall = totalall + feereader3.Item("amount")
                paidall = paidall + feereader3.Item("paid")


            Loop
            feereader3.Close()
            Dim outstanding As Double
            Dim due As Double

            outstanding = totalall - paidall
            Session("currenttotal") = (total - paid) + outstanding
            lblOutstanding.Text = FormatNumber(outstanding, , , , TriState.True)
            due = (outstanding + (total - paid))
            lblDue.Text = FormatNumber(due, , , , TriState.True)
            If mon <> 0 Then
                lblInstall.Text = FormatNumber(mon, , , , TriState.True)
            Else
                If paid <> 0 Then
                    lblInstall.Text = FormatNumber(total - paid, , , , TriState.True)
                Else
                    lblInstall.Text = FormatNumber(min + outstanding, , , , TriState.True)
                End If
            End If

            If outstanding > 0 Then lblOutstanding.ForeColor = Drawing.Color.Red Else lblOutstanding.ForeColor = Drawing.Color.Green
            If outstanding > total Then
                Session("Owing") = outstanding - total
            End If
            Dim cmdLoad25 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from transportfees", con)
            Dim student25 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad25.ExecuteReader
            Dim literal30 As New LiteralControl
            literal30.Text = "<table style='width: 100%; text-align:right;'>"
            Td6.Controls.Add(literal30)
            Do While student25.Read()
                Dim literal40 As New LiteralControl
                literal40.Text = "<tr><td>"
                Td6.Controls.Add(literal40)
                Dim label3 As New Label
                label3.ID = "label2" & i
                label3.Text = FormatNumber(student25(1), , , , TriState.True)
                Td6.Controls.Add(label3)
                Dim literal4 As New LiteralControl
                literal4.Text = "</td></tr>"
                Td6.Controls.Add(literal4)
                i = i + 1
            Loop
            Dim literal300 As New LiteralControl
            literal300.Text = "</table>"
            Td6.Controls.Add(literal300)
            student25.Close()

            Dim cmdLoad250 As New MySql.Data.MySqlClient.MySqlCommand("SELECT choicefees.*, class.class from choicefees left join class on class.id = choicefees.class", con)
            Dim student250 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad250.ExecuteReader
            Dim literal301 As New LiteralControl
            literal301.Text = "<table style='width: 100%; text-align:right;' >"
            Td10.Controls.Add(literal301)

            Do While student250.Read()
                If student250(5).ToString <> "" And Session("ClassId") = student250(5).ToString Then
                    Dim literal400 As New LiteralControl
                    literal400.Text = "<tr><td>"
                    Td10.Controls.Add(literal400)
                    Dim label4 As New Label
                    label4.ID = "label4" & i
                    label4.Text = FormatNumber(student250(2), , , , TriState.True)
                    Td10.Controls.Add(label4)
                    Dim literal42 As New LiteralControl
                    literal42.Text = "</td></tr>"
                    Td10.Controls.Add(literal42)
                    i = i + 1
                End If
            Loop
            Dim literal3002 As New LiteralControl
            literal3002.Text = "</table>"
            Td10.Controls.Add(literal3002)
            student250.Close()
            con.close()        End Using

    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("staffid") = Nothing Then 
 Dim x As ArrayList = check.Check_app_staff(Request.QueryString("username"), Request.QueryString("password")) 
 If x.Count <> 0 Then 
 Session("staffid") = x.Item(0) 
 Session("sessionid") = x.Item(1) 
 End If 
 End If 
        Try


            If IsPostBack Then
                If Not CheckBoxList1.Items.Count = 0 Then
                    If CheckBoxList1.Items(0).Selected = True And CheckBoxList1.Items(0).Enabled = True Then
                        optionalfeeb = lblBoarding.Text.Replace(",", "")
                    End If
                End If
                For Each item As ListItem In chkChoice.Items
                    If item.Selected = True And item.Enabled = True Then
                        optionalfeec.Add(item.Text)
                    ElseIf item.Selected = False And item.Enabled = True Then
                        optionalfeenc.Add(item.Text)
                    End If
                Next
                If CheckBoxList2.Items(0).Selected = True And CheckBoxList2.Items(0).Enabled = True Then
                    For Each item As ListItem In RadioButtonList1.Items
                        If item.Selected = True Then
                            optionalfeet = item.Text
                        End If
                    Next

                End If
            Else
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.open()
                    Dim cmdInsertf As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session Order by ID Desc", con)
                    Dim reader2f As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertf.ExecuteReader
                    reader2f.Read()

                    Dim currentSession As String = reader2f(0).ToString
                    reader2f.Close()
                    Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.surname, class.class,  studentsummary.class, class.type From Studentsummary INNER JOIN Studentsprofile on studentsummary.student = studentsprofile.admno inner join class on studentsummary.class = class.Id WHERE StudentSummary.student = ? And StudentSummary.Session = ?", con)
                    cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class.ID", Session("StudentAdd")))
                    cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter(" StudentSummary.Session ", currentSession))
                    Dim studentsReader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader()
                    studentsReader.Read()

                    lblClass.Text = studentsReader.Item(0).ToString
                    Session("ClassId") = studentsReader.Item(1)
                    Session("ClassType") = studentsReader.Item(3)
                    studentsReader.Close()

                    Dim cmdLoad6 As New MySql.Data.MySqlClient.MySqlCommand("SELECT admfees, hostelstay, transport from StudentsProfile where admno = ?", con)
                    cmdLoad6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("StudentAdd")))

                    Dim studentadm As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad6.ExecuteReader
                    studentadm.Read()
                    hostel = studentadm.Item(1)
                    transport = studentadm.Item(2)
                    studentadm.Close()

                    Dim cmd40 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from options", con)
                    Dim reader40 As MySql.Data.MySqlClient.MySqlDataReader = cmd40.ExecuteReader
                    reader40.Read()
                    Dim board As Boolean = reader40.Item(0)
                    Dim trans As Boolean = reader40.Item(1)
                    reader40.Close()
                    If board = True Then
                        Dim cmdLoad24 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from boarding", con)
                        Dim student24 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad24.ExecuteReader
                        student24.Read()
                        CheckBoxList1.Items.Clear()
                        CheckBoxList1.Items.Add("BOARDING")
                        lblBoarding.Text = FormatNumber(student24.Item(0), , , , TriState.True)
                        i = i + 1

                        student24.Close()
                        If hostel <> False Then
                            CheckBoxList1.Items(0).Selected = True
                        Else
                            CheckBoxList1.Items(0).Selected = False
                        End If
                    End If


                    If trans = True Then
                        CheckBoxList2.Items.Clear()
                        CheckBoxList2.Items.Add("TRANSPORT")
                        Dim cmdLoad25 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from transportfees", con)
                        Dim student25 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad25.ExecuteReader
                        RadioButtonList1.Items.Clear()
                        Do While student25.Read()

                            RadioButtonList1.Items.Add(student25.Item(0))

                        Loop
                        student25.Close()
                    End If

                    Dim cmdLoad250 As New MySql.Data.MySqlClient.MySqlCommand("SELECT choicefees.*, class.class from choicefees left join class on class.id = choicefees.class", con)
                    Dim student250 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad250.ExecuteReader
                    chkChoice.Items.Clear()

                    Do While student250.Read()
                        If student250(5).ToString <> "" And Session("ClassId") = student250(5).ToString Then

                            chkChoice.Items.Add(student250.Item(1))
                        End If
                    Loop
                    student250.Close()

                    If transport <> "" Then
                        CheckBoxList2.Items(0).Selected = True
                        For Each item In RadioButtonList1.Items
                            If item.text = transport Then
                                item.Selected = True
                            Else
                                item.selected = False
                            End If
                        Next

                    Else
                        RadioButtonList1.Enabled = False
                    End If

                    con.close()                End Using
                load_mandatory()


            End If

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnPay_Click(sender As Object, e As EventArgs) Handles btnPay.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdInsertf As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session Order by ID Desc", con)
                Dim reader2f As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertf.ExecuteReader
                reader2f.Read()

                Dim currentSession As String = reader2f(0).ToString
                reader2f.Close()
                Dim k As Integer
                Session("studentadd") = Session("StudentAdd")
                Dim amount As Integer = Val(txtAmount.Text.Replace(",", ""))
                Session("total") = amount
                If amount >= Val(lblInstall.Text.Replace(",", "")) Then
                    If lblOutstanding.Text.Replace(",", "") <> 0.0 And amount >= lblOutstanding.Text.Replace(",", "") Then
                        Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where student = ? and session <> ?", con)
                        cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                        cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentSession))
                        Dim feereader0 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                        Dim ses As New ArrayList
                        Dim outfee As New ArrayList
                        Dim outamount As New ArrayList
                        Dim outpay As New ArrayList
                        Do While feereader0.Read
                            If feereader0.Item("amount") <> feereader0.Item("paid") Then
                                Session("fee" & k) = feereader0.Item("fee")
                                Session("amount" & k) = feereader0.Item("amount")
                                Session("ses" & k) = feereader0.Item("session")
                                Session("paid") = feereader0.Item("paid")
                                amount = amount - (Session("amount" & k) - Session("paid"))
                                k = k + 1
                            End If
                        Loop
                        feereader0.Close()
                    End If
                    If Val(lblPaid.Text.Replace(",", "")) = 0.0 Then
                        Dim cmdInsert220 As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where session = ? and student = ? ", con)
                        cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentSession))
                        cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                        Dim feereader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert220.ExecuteReader
                        Dim total As Integer = 0
                        Dim feetype As New ArrayList
                        Dim feeamount As New ArrayList
                        Dim paid As Double = 0
                        Dim min As Double = 0

                        Do While feereader2.Read
                            If feereader2.Item("amount") = feereader2.Item("min") Then
                                Session("ses" & k) = feereader2.Item("session")
                                Session("paid" & k) = feereader2.Item("paid")
                                Session("fee" & k) = feereader2.Item("fee")
                                Session("amount" & k) = feereader2.Item("amount")
                                amount = amount - (Session("amount" & k) - Session("paid" & k))
                                k = k + 1
                            End If




                        Loop
                        feereader2.Close()

                        Dim cmdInsert4 As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where session = ? and student = ? ", con)
                        cmdInsert4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentSession))
                        cmdInsert4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))

                        Dim feereader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert4.ExecuteReader
                        Do While feereader3.Read
                            If feereader3.Item("amount") <> feereader3.Item("min") Then
                                If amount > feereader3.Item("amount") Then

                                    Session("ses" & k) = feereader3.Item("session")
                                    Session("paid" & k) = feereader3.Item("paid")
                                    Session("fee" & k) = feereader3.Item("fee")
                                    Session("amount" & k) = feereader3.Item("amount")
                                    amount = amount - (Session("amount" & k) - Session("paid" & k))
                                    k = k + 1

                                Else
                                    Session("fee" & k) = feereader3.Item("fee")
                                    Session("amount" & k) = amount
                                    Session("ses" & k) = feereader3.Item("session")
                                    Session("paid" & k) = feereader3.Item("paid")
                                    amount = amount - (Session("amount" & k) - Session("paid" & k))
                                    Exit Do
                                End If

                            End If

                        Loop
                        feereader3.Close()

                    Else
                        Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where student = ? and session = ?", con)
                        cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                        cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentSession))
                        Dim feereader0 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                        Dim ses As New ArrayList
                        Dim outfee As New ArrayList
                        Dim outamount As New ArrayList
                        Dim outpay As New ArrayList
                        Do While feereader0.Read
                            If feereader0.Item("amount") <> feereader0.Item("paid") And amount >= (feereader0.Item("amount") - feereader0.Item("paid")) Then
                                Session("fee" & k) = feereader0.Item("fee")
                                Session("amount" & k) = feereader0.Item("amount")
                                Session("ses" & k) = feereader0.Item("session")
                                Session("paid" & k) = feereader0.Item("paid")
                                amount = amount - (Session("amount" & k) - Session("paid" & k))
                                k = k + 1
                            ElseIf feereader0.Item("amount") <> feereader0.Item("paid") And amount < (feereader0.Item("amount") - feereader0.Item("paid")) Then
                                Session("fee" & k) = feereader0.Item("fee")
                                Session("amount" & k) = amount
                                Session("ses" & k) = feereader0.Item("session")
                                Session("paid" & k) = feereader0.Item("paid")
                                amount = amount - (Session("amount" & k) - Session("paid" & k))
                                Exit Do
                            End If
                        Loop
                        feereader0.Close()
                    End If

                    Session("count") = k
                    Session("over") = amount
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
                    Session("tref") = d1
                    Response.Redirect("~/content/App/App/pay.aspx")
                Else
                    Show_Alert(False, "The amount enterred is not up to the minimum installment")
                    Exit Sub
                End If


                con.Close()
            End Using

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub


    Protected Sub CheckBoxList1_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim cmdInsertf As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session Order by ID Desc", con)
                Dim reader2f As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertf.ExecuteReader
                reader2f.Read()

                Dim currentSession As String = reader2f(0).ToString
                reader2f.Close()
                If optionalfeeb <> Nothing Then
                    Dim cmdSelect2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT Session, Class, student FROM StudentSummary WHERE student = ? And Session = ?", con)
                    cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                    cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentSession))
                    Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2.ExecuteReader
                    reader2.Read()
                    Dim cla As Integer = reader2.Item(1).ToString
                    reader2.Close()

                    Dim cmdLoad10 As New MySql.Data.MySqlClient.MySqlCommand("Update studentsprofile set hostelstay = ? where admno = ?", con)
                    cmdLoad10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("hostel", True))
                    cmdLoad10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("admno", Session("StudentAdd")))

                    cmdLoad10.ExecuteNonQuery()

                    Dim cmdLoad24 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from boarding", con)
                    Dim student24 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad24.ExecuteReader
                    student24.Read()
                    Dim sesamount As String = student24.Item(0)
                    student24.Close()
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
                    Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, min) Values (?,?,?,?,?,?)", con)
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentSession))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", "BOARDING"))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", sesamount))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", sesamount))
                    cmdInsert22.ExecuteNonQuery()

                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d2))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(sesamount, , , TriState.True)))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "BOARDING DEBTS"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentSession))
                    cmdCheck2.ExecuteNonQuery()

                    Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d2))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(sesamount, , , , TriState.True)))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentSession))
                    cmdCheck4.ExecuteNonQuery()
                Else
                    Dim cmdLoad10 As New MySql.Data.MySqlClient.MySqlCommand("Update studentsprofile set hostelstay = ? where admno = ?", con)
                    cmdLoad10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("hostel", False))
                    cmdLoad10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("admno", Session("StudentAdd")))

                    cmdLoad10.ExecuteNonQuery()
                    Dim cmdLoad24 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from boarding", con)
                    Dim student24 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad24.ExecuteReader
                    student24.Read()
                    Dim sesamount As String = student24.Item(0)
                    student24.Close()

                    Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("Delete from feeschedule where session = ? and fee = ? and student = ?", con)
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentSession))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", "BOARDING"))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                    cmdInsert22.ExecuteNonQuery()



                    Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("select ref from transactions where account = ? and student = ? and session = ?", con)
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", "BOARDING DEBTS"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentSession))
                    Dim readref220 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck4.ExecuteReader
                    readref220.Read()
                    Dim refe As Integer = readref220.Item(0).ToString
                    readref220.Close()
                    Dim cmdCheck04 As New MySql.Data.MySqlClient.MySqlCommand("delete from transactions where ref = ?", con)
                    cmdCheck04.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("refa", refe))
                    cmdCheck04.ExecuteNonQuery()
                End If

                con.close()            End Using
            load_mandatory()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub



    Protected Sub CheckBoxList2_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                If CheckBoxList2.Items(0).Selected = True Then
                    RadioButtonList1.Enabled = True
                Else
                    Dim cmdInsertf As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session Order by ID Desc", con)
                    Dim reader2f As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertf.ExecuteReader
                    reader2f.Read()

                    Dim currentSession As String = reader2f(0).ToString
                    reader2f.Close()
                    Dim cmdLoad10 As New MySql.Data.MySqlClient.MySqlCommand("Update studentsprofile set transport = ? where admno = ?", con)
                    cmdLoad10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("hostel", ""))
                    cmdLoad10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("admno", Session("StudentAdd")))

                    cmdLoad10.ExecuteNonQuery()
                    For Each item As ListItem In RadioButtonList1.Items

                        If item.Selected = True Then

                            Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("Delete from feeschedule where session = ? and fee = ? and student = ?", con)
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentSession))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", "TRANSPORT"))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                            cmdInsert22.ExecuteNonQuery()
                            Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("select ref from transactions where account = ? and student = ? and session = ?", con)
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", "TRANSPORT DEBTS"))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentSession))
                            Dim readref220 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck4.ExecuteReader
                            readref220.Read()
                            Dim refe As Integer = readref220.Item(0).ToString
                            readref220.Close()
                            Dim cmdCheck04 As New MySql.Data.MySqlClient.MySqlCommand("delete from transactions where ref = ?", con)
                            cmdCheck04.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("refa", refe))
                            cmdCheck04.ExecuteNonQuery()

                            item.Selected = False

                        End If
                    Next
                    RadioButtonList1.Enabled = False
                End If
                con.close()            End Using
            load_mandatory()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub RadioButtonList1_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()

                Dim cmdInsertf As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session Order by ID Desc", con)
                Dim reader2f As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertf.ExecuteReader
                reader2f.Read()

                Dim currentSession As String = reader2f(0).ToString
                reader2f.Close()
                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update studentsprofile Set transport = ? Where admno = ?", con)
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", optionalfeet))
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sex", Session("StudentAdd")))
                cmdCheck3.ExecuteNonQuery()

                Dim cmdSelect2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT Session, Class, student FROM StudentSummary WHERE student = ? And Session = ?", con)
                cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentSession))
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2.ExecuteReader
                reader2.Read()
                Dim cla As Integer = reader2.Item(1).ToString
                reader2.Close()


                Dim cmdInsert222 As New MySql.Data.MySqlClient.MySqlCommand("Delete from feeschedule where session = ? and fee = ? and student = ?", con)
                cmdInsert222.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentSession))
                cmdInsert222.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", "TRANSPORT"))
                cmdInsert222.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                cmdInsert222.ExecuteNonQuery()
                Dim cmdCheck44 As New MySql.Data.MySqlClient.MySqlCommand("select ref from transactions where account = ? and student = ? and session = ?", con)
                cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", "TRANSPORT DEBTS"))
                cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentSession))
                Dim readref220 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck44.ExecuteReader
                Dim refe As Integer
                If readref220.Read() Then
                    refe = readref220.Item(0).ToString
                End If
                readref220.Close()
                Dim cmdCheck04 As New MySql.Data.MySqlClient.MySqlCommand("delete from transactions where ref = ?", con)
                cmdCheck04.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("refa", refe))
                cmdCheck04.ExecuteNonQuery()

                Dim test24 As Boolean
                Dim f4 As New Random
                Dim ref24 As New MySql.Data.MySqlClient.MySqlCommand("Select ref from transactions", con)
                Dim readref24 As MySql.Data.MySqlClient.MySqlDataReader = ref24.ExecuteReader

                Dim refs24 As New ArrayList
                Do While readref24.Read
                    refs24.Add(readref24.Item(0))
                Loop
                Dim d4 As Integer
                Do Until test24 = True
                    d4 = f4.Next(100000, 999999)
                    If refs24.Contains(d4) Then
                        test24 = False
                    Else
                        test24 = True
                    End If
                Loop
                readref24.Close()
                Dim cmdLoad24 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from transportfees where route = ?", con)
                cmdLoad24.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", optionalfeet))
                Dim student24 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad24.ExecuteReader
                student24.Read()

                Dim sesamount As String = student24.Item(1)
                Dim sesmin As Integer = (student24.Item("amount") * (student24.Item("min") / 100))
                student24.Close()
                Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, min) Values (?,?,?,?,?,?)", con)
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentSession))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", "TRANSPORT"))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", sesamount))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", sesmin))
                cmdInsert22.ExecuteNonQuery()

                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(sesamount, , , TriState.True)))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "TRANSPORT DEBTS"))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentSession))
                cmdCheck2.ExecuteNonQuery()
                Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(sesamount, , , TriState.True)))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentSession))

                cmdCheck4.ExecuteNonQuery()

                con.close()            End Using
            load_mandatory()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub chkChoice_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim cmdInsertf As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session Order by ID Desc", con)
                Dim reader2f As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertf.ExecuteReader
                reader2f.Read()

                Dim currentSession As String = reader2f(0).ToString
                reader2f.Close()
                For Each item As String In optionalfeec

                    Dim cmdSelect2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT Session, Class, student FROM StudentSummary WHERE student = ? And Session = ?", con)
                    cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentSession))
                    Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2.ExecuteReader
                    reader2.Read()
                    Dim cla As Integer = reader2.Item(1).ToString
                    reader2.Close()
                    Dim cmdInsert222 As New MySql.Data.MySqlClient.MySqlCommand("Delete from feeschedule where session = ? and fee = ? and student = ?", con)
                    cmdInsert222.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentSession))
                    cmdInsert222.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                    cmdInsert222.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                    cmdInsert222.ExecuteNonQuery()
                    Dim cmdCheck44 As New MySql.Data.MySqlClient.MySqlCommand("select ref from transactions where account = ? and student = ? and session = ?", con)
                    cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", item & " DEBTS"))
                    cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                    cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentSession))
                    Dim readref220 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck44.ExecuteReader
                    Dim refe As Integer
                    If readref220.Read() Then
                        refe = readref220.Item(0).ToString
                    End If
                    readref220.Close()
                    Dim cmdCheck04 As New MySql.Data.MySqlClient.MySqlCommand("delete from transactions where ref = ?", con)
                    cmdCheck04.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("refa", refe))
                    cmdCheck04.ExecuteNonQuery()

                    Dim test24 As Boolean
                    Dim f4 As New Random
                    Dim ref24 As New MySql.Data.MySqlClient.MySqlCommand("Select ref from transactions", con)
                    Dim readref24 As MySql.Data.MySqlClient.MySqlDataReader = ref24.ExecuteReader

                    Dim refs24 As New ArrayList
                    Do While readref24.Read
                        refs24.Add(readref24.Item(0))
                    Loop
                    Dim d4 As Integer
                    Do Until test24 = True
                        d4 = f4.Next(100000, 999999)
                        If refs24.Contains(d4) Then
                            test24 = False
                        Else
                            test24 = True
                        End If
                    Loop
                    readref24.Close()
                    Dim cmdLoad24 As New MySql.Data.MySqlClient.MySqlCommand("SELECT choicefees.*, class.class from choicefees left join class on class.id = choicefees.class where choicefees.fee = ?", con)
                    cmdLoad24.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", item))
                    Dim student24 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad24.ExecuteReader
                    Dim sesamount As String = ""
                    Dim sesmin As Integer
                    Dim sesmon As Integer
                    Do While student24.Read()
                        If student24(6).ToString <> "" And Session("ClassId") = student24(6).ToString Then
                            sesamount = student24.Item(2)
                            sesmin = (student24.Item("amount") * (student24.Item("min") / 100))
                            sesmon = student24.Item(5)
                        End If
                    Loop
                    student24.Close()
                    Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, min, monthly) Values (?,?,?,?,?,?,?)", con)
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentSession))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", sesamount))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", sesmin))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("mon", sesmon))
                    cmdInsert22.ExecuteNonQuery()


                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(sesamount, , , TriState.True)))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", item & " DEBTS"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentSession))
                    cmdCheck2.ExecuteNonQuery()
                    Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(sesamount, , , TriState.True)))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentSession))

                    cmdCheck4.ExecuteNonQuery()

                Next
                For Each item As String In optionalfeenc


                    Dim cmdSelect2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT Session, Class, student FROM StudentSummary WHERE student = ? And Session = ?", con)
                    cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentSession))
                    Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2.ExecuteReader
                    reader2.Read()
                    Dim cla As Integer = reader2.Item(1).ToString
                    reader2.Close()
                    Dim cmdInsert222 As New MySql.Data.MySqlClient.MySqlCommand("Delete from feeschedule where session = ? and fee = ? and student = ?", con)
                    cmdInsert222.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentSession))
                    cmdInsert222.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                    cmdInsert222.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                    cmdInsert222.ExecuteNonQuery()
                    Dim cmdCheck44 As New MySql.Data.MySqlClient.MySqlCommand("select ref from transactions where account = ? and student = ? and session = ?", con)
                    cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", item & " DEBTS"))
                    cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                    cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentSession))
                    Dim readref220 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck44.ExecuteReader
                    Dim refe As Integer
                    If readref220.Read() Then
                        refe = readref220.Item(0).ToString
                    End If
                    readref220.Close()
                    Dim cmdCheck04 As New MySql.Data.MySqlClient.MySqlCommand("delete from transactions where ref = ?", con)
                    cmdCheck04.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("refa", refe))
                    cmdCheck04.ExecuteNonQuery()

                Next
                con.close()            End Using
            load_mandatory()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
End Class
