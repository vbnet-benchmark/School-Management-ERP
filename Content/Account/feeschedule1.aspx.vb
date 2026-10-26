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
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.Open()

            Dim currentsession As String
            Dim currentterm As String
            If Session("scheduletype") = "quarterly" Then
                Dim cmdInsertf As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session where term = '" & IIf(lblScheduleType.Text = "1st Quarter" Or lblScheduleType.Text = "2nd Quarter", "2nd term", IIf(lblScheduleType.Text = "3rd Quarter", "3rd term", "1st term")) & "' Order by ID Desc", con)
                Dim reader2f As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertf.ExecuteReader
                reader2f.Read()
                currentsession = reader2f(0).ToString
                currentterm = reader2f(2).ToString
                reader2f.Close()
            Else
                Dim cmdInsertf As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session Order by ID Desc", con)
                Dim reader2f As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertf.ExecuteReader
                reader2f.Read()
                currentterm = reader2f(2).ToString
                currentsession = reader2f(0).ToString
                reader2f.Close()

            End If

            Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where session = ? and student = ?", con)
            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", Session("StudentAdd")))
            Dim feereader As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert22.ExecuteReader()
            BulletedList1.Items.Clear()
            BulletedList1.BulletStyle = BulletStyle.NotSet

            Do While feereader.Read
                If feereader.Item("compulsory") = 1 Then
                    BulletedList1.Items.Add(feereader.Item(1))


                    Dim label5 As New Label
                    label5.ID = "label1" & i
                    If Session("scheduletype") = "Monthly" Then
                        label5.Text = FormatNumber(feereader.Item("monthly"), , , , TriState.True)
                    ElseIf Session("scheduletype") = "quarterly" Then
                        label5.Text = FormatNumber(feereader.Item("quarterly"), , , , TriState.True)
                    Else
                        label5.Text = FormatNumber(feereader.Item(2), , , , TriState.True)
                    End If
                    ammcol.Controls.Add(label5)
                    Dim literal5 As New LiteralControl
                    literal5.Text = "<br/>"
                    ammcol.Controls.Add(literal5)
                    i = i + 1
                End If

            Loop
            feereader.Close()
            Dim cmdInsert220 As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where session = ? and student = ?", con)
            cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
            cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
            Dim feereader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert220.ExecuteReader
            Dim total As Integer = 0
            Dim feetype As New ArrayList
            Dim feeamount As New ArrayList
            Dim paid As Double = 0
            Dim min As Double = 0
            Dim currentbal As Double = 0
            Dim elapsedmonths As Integer
            Dim presentpaid As Double = 0
            If currentterm = "1st term" Then
                elapsedmonths = Now.Month - 9
            ElseIf currentterm = "2nd term" Then
                elapsedmonths = Now.Month - 1
            Else
                elapsedmonths = Now.Month - 5

            End If
            Dim x As String = ""
            Do While feereader2.Read
                If feereader2("amount") <> 0 Then
                    feetype.Add(feereader2.Item("fee"))
                    feeamount.Add(feereader2.Item("amount"))
                    total = total + feereader2.Item("amount")
                    paid = paid + feereader2.Item("paid")
                    min = min + feereader2.Item("min")

                    x = x & feereader2.Item("amount").ToString

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
                End If

            Loop
            Show_Alert(True, x)
            Exit Sub

            lblTotal.Text = FormatNumber(total, , , , TriState.True)
            lblPaid.Text = FormatNumber(IIf(paid >= 0, paid, 0), , , , TriState.True)

            feereader2.Close()
            Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where student = ? and session < " & currentsession & "", con)
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
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

            outstanding = totalall - paidall + currentbal
            Session("currenttotal") = (total - IIf(paid >= 0, paid, 0)) + outstanding
            lblOutstanding.Text = FormatNumber(outstanding, , , , TriState.True)
            due = (outstanding + (total - IIf(paid >= 0, paid, 0)))
            lblDue.Text = FormatNumber(due, , , , TriState.True)




            If outstanding > 0 Then lblOutstanding.ForeColor = Drawing.Color.Red Else lblOutstanding.ForeColor = Drawing.Color.Green
            If outstanding > total Then
                Session("Owing") = outstanding - total
            End If
            Dim cmd40 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from options", con)
            Dim reader40 As MySql.Data.MySqlClient.MySqlDataReader = cmd40.ExecuteReader
            reader40.Read()
            Dim board As Boolean = reader40.Item(0)
            Dim trans As Boolean = reader40.Item(1)
            reader40.Close()
            If trans = True Then
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
            End If
            Dim cmdLoad250 As New MySql.Data.MySqlClient.MySqlCommand("SELECT choicefees.*, class.class from choicefees left join class on class.id = choicefees.class", con)
            Dim student250 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad250.ExecuteReader
            Dim literal301 As New LiteralControl
            literal301.Text = "<table style='width: 100%; text-align:right;' >"
            Td10.Controls.Add(literal301)

            Do While student250.Read()
                If (student250(7).ToString <> "" And Session("ClassId") = student250(7).ToString) Or student250(7).ToString = "" Then
                    Dim literal400 As New LiteralControl
                    literal400.Text = "<tr><td>"
                    Td10.Controls.Add(literal400)
                    Dim label4 As New Label
                    label4.ID = "label4" & i
                    If Session("scheduletype") = "Monthly" Then
                        label4.Text = FormatNumber(student250("monthly"), , , , TriState.True)
                    ElseIf Session("scheduletype") = "quarterly" Then
                        label4.Text = FormatNumber(student250("quarterly"), , , , TriState.True)
                    Else
                        label4.Text = FormatNumber(student250(2), , , , TriState.True)
                    End If
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
            con.Close()        End Using

    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If check.Check_Account(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
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
                Try
                    If CheckBoxList2.Items(0).Selected = True And CheckBoxList2.Items(0).Enabled = True Then
                        For Each item As ListItem In RadioButtonList1.Items
                            If item.Selected = True Then
                                optionalfeet = item.Text
                            End If
                        Next

                    End If
                Catch
                End Try
            Else
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.Open()
                    If Now.Month <= 3 Then
                        lblScheduleType.Text = "1st Quarter"
                    ElseIf Now.Month > 3 And Now.Month < 7 Then
                        lblScheduleType.Text = "2nd Quarter"
                    ElseIf Now.Month >= 7 And Now.Month < 10 Then
                        lblScheduleType.Text = "3rd Quarter"
                    ElseIf Now.Month >= 10 Then
                        lblScheduleType.Text = "4th Quarter"
                    End If
                    Dim currentsession As String
                    If Session("scheduletype") = "quarterly" Then
                        Dim cmdInsertf As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session where term = '" & IIf(lblScheduleType.Text = "1st Quarter" Or lblScheduleType.Text = "2nd Quarter", "2nd term", IIf(lblScheduleType.Text = "3rd Quarter", "3rd term", "1st term")) & "' Order by ID desc", con)
                        Dim reader2f As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertf.ExecuteReader
                        reader2f.Read()
                        currentsession = reader2f(0).ToString
                        reader2f.Close()
                    Else
                        Dim cmdInsertf As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session Order by ID Desc", con)
                        Dim reader2f As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertf.ExecuteReader
                        reader2f.Read()

                        currentsession = reader2f(0).ToString
                        reader2f.Close()

                    End If
                    Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.surname, class.class,  studentsummary.class, class.type From Studentsummary INNER JOIN Studentsprofile on studentsummary.student = studentsprofile.admno inner join class on studentsummary.class = class.Id WHERE StudentSummary.student = ? And StudentSummary.Session = ?", con)
                    cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class.ID", Session("StudentAdd")))
                    cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter(" StudentSummary.Session ", currentSession))
                    Dim studentsReader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader()
                    studentsReader.Read()
                    lblClassMonth.Text = studentsReader.Item(0).ToString
                    lblClass.Text = studentsReader.Item(0).ToString
                    Session("ClassId") = studentsReader.Item(1)
                    Session("ClassType") = studentsReader.Item(3)
                    studentsReader.Close()
                    lblMonth.Text = Now.Month.ToString.ToUpper

                    
                        lblScheduleType.Text = Session("sessionname").ToString.ToUpper & "  " & Session("Term").ToString.ToUpper
                        lblPayable.Text = "Total Payable this term"
                        Label7.Text = "Total Payed this term"
                        Label3.Text = "Balance from other terms"
                  
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
                        If (student250(7).ToString <> "" And Session("ClassId") = student250(7).ToString) Or student250(7).ToString = "" Then
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

                    con.Close()                End Using
                load_mandatory()

            End If

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnPay_Click(sender As Object, e As EventArgs) Handles btnPay.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim currentsession As String
                Dim currentterm As String
                If Session("scheduletype") = "quarterly" Then
                    Dim cmdInsertf As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session where term = '" & IIf(lblScheduleType.Text = "1st Quarter" Or lblScheduleType.Text = "2nd Quarter", "2nd term", IIf(lblScheduleType.Text = "3rd Quarter", "3rd term", "1st term")) & "' Order by ID desc", con)
                    Dim reader2f As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertf.ExecuteReader
                    reader2f.Read()
                    currentsession = reader2f(0).ToString
                    currentterm = reader2f(2).ToString
                    reader2f.Close()
                Else
                    Dim cmdInsertf As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session Order by ID Desc", con)
                    Dim reader2f As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertf.ExecuteReader
                    reader2f.Read()
                    currentterm = reader2f(2).ToString
                    currentsession = reader2f(0).ToString
                    reader2f.Close()

                End If
                Dim k As Integer
                Dim l As Integer
                Dim lnum As Integer
                Session("studentadd") = Session("StudentAdd")
                Dim amount As Integer = Val(txtAmount.Text.Replace(",", ""))
                If amount < lblOutstanding.Text.Replace(",", "") Then
                    Show_Alert(False, "Previous balances must be cleared up.")
                    load_mandatory()
                    Exit Sub
                End If
                Session("total") = amount
                If Session("scheduletype") <> "Monthly" Then
                    If lblOutstanding.Text.Replace(",", "") <> 0.0 And amount >= lblOutstanding.Text.Replace(",", "") Then
                        If Session("scheduletype") = "quarterly" And Now.Month > 3 And currentterm = "2nd term" Then

                            Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where student = ? and session = ?", con)
                            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
                            Dim feereader0 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                            Dim ses As New ArrayList
                            Dim outfee As New ArrayList
                            Dim outamount As New ArrayList
                            Dim outpay As New ArrayList
                            Do While feereader0.Read
                                If IIf(feereader0.Item("amount") - feereader0("quarterly") < 0, 0, feereader0.Item("amount") - feereader0("quarterly")) <> feereader0.Item("paid") Then

                                    Session("fee" & l) = feereader0.Item("fee")
                                    Session("amount" & l) = IIf(feereader0.Item("amount") - feereader0("quarterly") < 0, 0, feereader0.Item("amount") - feereader0("quarterly"))
                                    Session("ses" & l) = feereader0.Item("session")
                                    Session("paid" & l) = feereader0.Item("paid")
                                    amount = amount - (Session("amount" & l) - Session("paid" & l))

                                End If
                                l = l + 1
                            Loop
                            feereader0.Close()
                        ElseIf Session("scheduletype") = "quarterly" And Now.Month > 3 And currentterm <> "2nd term" Then
                            Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where student = ? and session < ?", con)
                            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
                            Dim feereader0 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                            Dim ses As New ArrayList
                            Dim outfee As New ArrayList
                            Dim outamount As New ArrayList
                            Dim outpay As New ArrayList
                            Do While feereader0.Read
                                If feereader0.Item("amount") <> feereader0.Item("paid") Then
                                    Session("fee" & l) = feereader0.Item("fee")
                                    Session("amount" & l) = feereader0.Item("amount")
                                    Session("ses" & l) = feereader0.Item("session")
                                    Session("paid" & l) = feereader0.Item("paid")
                                    amount = amount - (Session("amount" & l) - Session("paid" & l))

                                End If
                                l = l + 1
                            Loop
                            feereader0.Close()

                        Else
                            Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where student = ? and session < ?", con)
                            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
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
                                    Session("paid" & k) = feereader0.Item("paid")
                                    amount = amount - (Session("amount" & k) - Session("paid" & k))
                                    k = k + 1
                                End If
                            Loop
                            feereader0.Close()
                        End If

                    End If
                    If Val(lblPaid.Text.Replace(",", "")) = 0.0 Then
                        Dim cmdInsert220 As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where session = ? and student = ? ", con)
                        cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
                        cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                        Dim feereader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert220.ExecuteReader
                        Dim total As Integer = 0
                        Dim feetype As New ArrayList
                        Dim feeamount As New ArrayList
                        Dim paid As Double = 0
                        Dim min As Double = 0
                        Dim temp As Integer
                        lnum = l
                        l = 0


                        Do While feereader2.Read
                            If Session("scheduletype") = "quarterly" Then

                                If amount > IIf(currentterm = "2nd term" And Now.Month <= 3, IIf(feereader2.Item("amount") - feereader2("quarterly") < 0, 0, feereader2.Item("amount") - feereader2("quarterly")), IIf(currentterm = "2nd term" And Now.Month > 3, IIf(feereader2("amount") < feereader2("quarterly"), feereader2("amount"), feereader2("quarterly")), feereader2("amount"))) - IIf(currentterm = "2nd term" And Now.Month < 3, IIf(feereader2.Item("paid") - feereader2("quarterly") < 0, 0, feereader2.Item("paid") - feereader2("quarterly")), feereader2("paid")) Then
                                    temp = Session("amount" & l)
                                    If currentterm = "2nd term" And Now.Month > 3 Then

                                        Session("ses" & l) = feereader2.Item("session")
                                        Session("paid" & l) = feereader2.Item("paid")
                                        Session("fee" & l) = feereader2.Item("fee")

                                        Session("amount" & l) = IIf(temp = 0, feereader2.Item("paid"), temp) + IIf(currentterm = "2nd term" And Now.Month <= 3, IIf(feereader2.Item("amount") - feereader2("quarterly") < 0, 0, feereader2.Item("amount") - feereader2("quarterly")), IIf(currentterm = "2nd term" And Now.Month > 3, IIf(feereader2("amount") < feereader2("quarterly"), feereader2("amount"), feereader2("quarterly")), feereader2("amount"))) - IIf(currentterm = "2nd term" And Now.Month > 3, IIf(feereader2.Item("paid") - feereader2("quarterly") < 0, 0, feereader2.Item("paid") - feereader2("quarterly")), feereader2("paid"))
                                        amount = amount - (Session("amount" & l) - IIf(temp = 0, feereader2.Item("paid"), temp))

                                        l = l + 1
                                    Else
                                        If temp <> 0 Then
                                            l = l + 1
                                            Session("fee" & l) = feereader2.Item("fee")

                                            Session("amount" & l) = feereader2.Item("paid") + IIf(currentterm = "2nd term" And Now.Month <= 3, IIf(feereader2.Item("amount") - feereader2("quarterly") < 0, 0, feereader2.Item("amount") - feereader2("quarterly")), IIf(currentterm = "2nd term" And Now.Month > 3, IIf(feereader2("amount") < feereader2("quarterly"), feereader2("amount"), feereader2("quarterly")), feereader2("amount"))) - IIf(currentterm = "2nd term" And Now.Month > 3, IIf(feereader2.Item("paid") - feereader2("quarterly") < 0, 0, feereader2.Item("paid") - feereader2("quarterly")), feereader2("paid"))
                                            Session("ses" & l) = feereader2.Item("session")
                                            Session("paid" & l) = feereader2.Item("paid")
                                            amount = amount - (Session("amount" & l) - Session("paid" & l))
                                            l = l + 1
                                        Else
                                            Session("fee" & l) = feereader2.Item("fee")

                                            Session("amount" & l) = feereader2.Item("paid") + IIf(currentterm = "2nd term" And Now.Month <= 3, IIf(feereader2.Item("amount") - feereader2("quarterly") < 0, 0, feereader2.Item("amount") - feereader2("quarterly")), IIf(currentterm = "2nd term" And Now.Month > 3, IIf(feereader2("amount") < feereader2("quarterly"), feereader2("amount"), feereader2("quarterly")), feereader2("amount"))) - IIf(currentterm = "2nd term" And Now.Month > 3, IIf(feereader2.Item("paid") - feereader2("quarterly") < 0, 0, feereader2.Item("paid") - feereader2("quarterly")), feereader2("paid"))
                                            Session("ses" & l) = feereader2.Item("session")
                                            Session("paid" & l) = feereader2.Item("paid")
                                            amount = amount - (Session("amount" & l) - Session("paid" & l))
                                            l = l + 1
                                        End If

                                    End If
                                Else
                                    temp = Session("amount" & l)
                                    If currentterm = "2nd term" And Now.Month > 3 Then
                                        Session("fee" & l) = feereader2.Item("fee")

                                        Session("amount" & l) = IIf(temp = 0, feereader2.Item("paid"), temp) + amount
                                        Session("ses" & l) = feereader2.Item("session")
                                        Session("paid" & l) = feereader2.Item("paid")
                                        amount = amount - (Session("amount" & l) - IIf(temp = 0, feereader2.Item("paid"), temp))
                                        Exit Do
                                    Else
                                        If temp <> 0 Then
                                            l = l + 1
                                            Session("fee" & l) = feereader2.Item("fee")

                                            Session("amount" & l) = feereader2.Item("paid") + amount
                                            Session("ses" & l) = feereader2.Item("session")
                                            Session("paid" & l) = feereader2.Item("paid")
                                            amount = amount - (Session("amount" & l) - Session("paid" & l))
                                            Exit Do
                                        Else
                                            Session("fee" & l) = feereader2.Item("fee")

                                            Session("amount" & l) = feereader2.Item("paid") + amount
                                            Session("ses" & l) = feereader2.Item("session")
                                            Session("paid" & l) = feereader2.Item("paid")
                                            amount = amount - (Session("amount" & l) - Session("paid" & l))
                                            Exit Do
                                        End If
                                    End If
                                End If
                            Else
                                If feereader2.Item("amount") = feereader2.Item("min") Then
                                    Session("ses" & k) = feereader2.Item("session")
                                    Session("paid" & k) = feereader2.Item("paid")
                                    Session("fee" & k) = feereader2.Item("fee")
                                    Session("amount" & k) = feereader2.Item("amount")
                                    amount = amount - (Session("amount" & k) - Session("paid" & k))
                                    k = k + 1
                                End If

                            End If
                        Loop
                        feereader2.Close()

                        Dim cmdInsert4 As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where session = ? and student = ? ", con)
                        cmdInsert4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
                        cmdInsert4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                        Dim feereader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert4.ExecuteReader
                        Do While feereader3.Read
                            If feereader3.Item("amount") <> feereader3.Item("min") And Session("scheduletype") <> "quarterly" Then
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
                        If Session("scheduletype") = "quarterly" Then
                            Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where student = ? and session = ?", con)
                            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
                            Dim feereader0 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                            Dim ses As New ArrayList
                            Dim outfee As New ArrayList
                            Dim outamount As New ArrayList
                            Dim outpay As New ArrayList
                            Dim temp As Integer
                            Do While feereader0.Read
                                If feereader0.Item("amount") <> feereader0.Item("paid") And amount >= (IIf(currentterm = "2nd term" And Now.Month <= 3, IIf(feereader0.Item("amount") - feereader0("quarterly") < 0, 0, feereader0.Item("amount") - feereader0("quarterly")), IIf(currentterm = "2nd term" And Now.Month > 3, IIf(feereader0("amount") < feereader0("quarterly"), feereader0("amount"), feereader0("quarterly")), feereader0("amount"))) - IIf(currentterm = "2nd term" And Now.Month > 3, feereader0.Item("paid") - feereader0("quarterly"), feereader0("paid"))) Then
                                    temp = Session("amount" & l)
                                    Session("fee" & l) = feereader0.Item("fee")
                                    Session("amount" & l) = feereader0("paid") + IIf(currentterm = "2nd term" And Now.Month <= 3, IIf(feereader0.Item("amount") - feereader0("quarterly") < 0, 0, feereader0.Item("amount") - feereader0("quarterly")), IIf(currentterm = "2nd term" And Now.Month > 3, IIf(feereader0("amount") < feereader0("quarterly"), feereader0("amount"), feereader0("quarterly")), feereader0("amount"))) - IIf(currentterm = "2nd term" And Now.Month > 3, feereader0.Item("paid") - feereader0("quarterly"), feereader0("paid"))
                                    Session("ses" & l) = feereader0.Item("session")
                                    Session("paid" & l) = feereader0.Item("paid")
                                    amount = amount - (Session("amount" & l) - Session("paid" & l))
                                    Session("amount" & l) = Session("amount" & l) + temp
                                    l = l + 1
                                ElseIf feereader0.Item("amount") <> feereader0.Item("paid") And amount < (IIf(currentterm = "2nd term" And Now.Month <= 3, IIf(feereader0.Item("amount") - feereader0("quarterly") < 0, 0, feereader0.Item("amount") - feereader0("quarterly")), IIf(currentterm = "2nd term" And Now.Month > 3, IIf(feereader0("amount") < feereader0("quarterly"), feereader0("amount"), feereader0("quarterly")), feereader0("amount"))) - IIf(currentterm = "2nd term" And Now.Month > 3, feereader0.Item("paid") - feereader0("quarterly"), feereader0("paid"))) Then
                                    temp = Session("amount" & l)
                                    Session("fee" & l) = feereader0.Item("fee")
                                    Session("amount" & l) = amount + feereader0("paid")
                                    Session("ses" & l) = feereader0.Item("session")
                                    Session("paid" & l) = feereader0.Item("paid")
                                    amount = amount - (Session("amount" & l) - Session("paid" & l))
                                    Session("amount" & l) = Session("amount" & l) + temp

                                    Exit Do
                                End If
                            Loop
                            feereader0.Close()
                        Else
                            Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where student = ? and session = ?", con)
                            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
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

                    End If
                    If l > lnum Then lnum = l
                    Session("count") = IIf(lnum <> 0, lnum, k)
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
                    Session("tcurrentsession") = currentsession
                    Session("tref") = d1
                    Response.Redirect("~/content/afterpay.aspx?" & Session("tref"))
                Else

                    Dim cmdInsert220 As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where session = ? and student = ? ", con)
                    cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
                    cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                    Dim feereader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert220.ExecuteReader
                    Dim total As Integer = 0
                    Dim feetype As New ArrayList
                    Dim feeamount As New ArrayList
                    Dim paid As Double = 0
                    Dim min As Double = 0

                    Do While feereader2.Read
                        If feereader2("monthly") = 0 Then
                            Session("ses" & k) = feereader2.Item("session")
                            Session("paid" & k) = feereader2.Item("paid")
                            Session("fee" & k) = feereader2.Item("fee")
                            Session("amount" & k) = feereader2.Item("amount")
                            Session("actualamount" & k) = feereader2.Item("amount")
                            amount = amount - (Session("amount" & k) - Session("paid" & k))
                            k = k + 1
                        End If




                    Loop
                    feereader2.Close()
                    Dim elapsedmonths As Integer
                    Dim cmdInsert4 As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where session = ? and student = ? ", con)
                    cmdInsert4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
                    cmdInsert4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                    If currentterm = "1st term" Then
                        elapsedmonths = Now.Month - 9
                    ElseIf currentterm = "2nd term" Then
                        elapsedmonths = Now.Month - 1
                    Else
                        elapsedmonths = Now.Month - 5
                    End If
                    Dim feereader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert4.ExecuteReader
                    Do While feereader3.Read
                        If feereader3("monthly") <> 0 Then
                            If feereader3("paid") <> feereader3("amount") Then
                                Dim diff As Integer = 4 - (feereader3("amount") / feereader3("monthly"))
                                Session("ses" & k) = feereader3.Item("session")
                                Session("paid" & k) = feereader3.Item("paid")
                                Session("fee" & k) = feereader3.Item("fee")
                                Session("amount" & k) = Val(feereader3.Item("monthly")) * (elapsedmonths + 1 - diff)
                                Session("actualamount" & k) = (Val(feereader3.Item("monthly")) * (elapsedmonths + 1 - diff)) - feereader3.Item("paid")
                                amount = amount - Session("actualamount" & k)
                                k = k + 1
                            End If
                        End If
                    Loop
                    feereader3.Close()

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
                    Response.Redirect("~/content/afterpay.aspx?" & Session("tref"))



                End If
                Dim cmdInsert4s As New MySql.Data.MySqlClient.MySqlCommand("update feeschedule set payment = '" & Session("scheduletype") & "' where session = ? and student = ? ", con)
                cmdInsert4s.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentSession))
                cmdInsert4s.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                cmdInsert4s.ExecuteNonQuery()



                con.Close()
            End Using

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub


    Protected Sub CheckBoxList1_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim currentsession As String
                If Session("scheduletype") = "quarterly" Then
                    Dim cmdInsertf As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session where term = '" & IIf(lblScheduleType.Text = "1st Quarter" Or lblScheduleType.Text = "2nd Quarter", "2nd term", IIf(lblScheduleType.Text = "3rd Quarter", "3rd term", "1st term")) & "' Order by ID desc", con)
                    Dim reader2f As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertf.ExecuteReader
                    reader2f.Read()
                    currentsession = reader2f(0).ToString
                    reader2f.Close()
                Else
                    Dim cmdInsertf As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session Order by ID Desc", con)
                    Dim reader2f As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertf.ExecuteReader
                    reader2f.Read()

                    currentsession = reader2f(0).ToString
                    reader2f.Close()

                End If
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
                    Dim currentsession As String
                    If Session("scheduletype") = "quarterly" Then
                        Dim cmdInsertf As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session where term = '" & IIf(lblScheduleType.Text = "1st Quarter" Or lblScheduleType.Text = "2nd Quarter", "2nd term", IIf(lblScheduleType.Text = "3rd Quarter", "3rd term", "1st term")) & "' Order by ID desc", con)
                        Dim reader2f As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertf.ExecuteReader
                        reader2f.Read()
                        currentsession = reader2f(0).ToString
                        reader2f.Close()
                    Else
                        Dim cmdInsertf As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session Order by ID Desc", con)
                        Dim reader2f As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertf.ExecuteReader
                        reader2f.Read()

                        currentsession = reader2f(0).ToString
                        reader2f.Close()

                    End If
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
                Dim currentsession As String
                If Session("scheduletype") = "quarterly" Then
                    Dim cmdInsertf As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session where term = '" & IIf(lblScheduleType.Text = "1st Quarter" Or lblScheduleType.Text = "2nd Quarter", "2nd term", IIf(lblScheduleType.Text = "3rd Quarter", "3rd term", "1st term")) & "' Order by ID desc", con)
                    Dim reader2f As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertf.ExecuteReader
                    reader2f.Read()
                    currentsession = reader2f(0).ToString
                    reader2f.Close()
                Else
                    Dim cmdInsertf As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session Order by ID Desc", con)
                    Dim reader2f As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertf.ExecuteReader
                    reader2f.Read()

                    currentsession = reader2f(0).ToString
                    reader2f.Close()

                End If

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
                Dim currentsession As String
                Dim currentterm As String
                If Session("scheduletype") = "quarterly" Then
                    Dim cmdInsertf As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session where term = '" & IIf(lblScheduleType.Text = "1st Quarter" Or lblScheduleType.Text = "2nd Quarter", "2nd term", IIf(lblScheduleType.Text = "3rd Quarter", "3rd term", "1st term")) & "' Order by ID desc", con)
                    Dim reader2f As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertf.ExecuteReader
                    reader2f.Read()
                    currentsession = reader2f(0).ToString
                    currentterm = reader2f(2).ToString
                    reader2f.Close()
                Else
                    Dim cmdInsertf As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session Order by ID Desc", con)
                    Dim reader2f As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertf.ExecuteReader
                    reader2f.Read()
                    currentterm = reader2f(2).ToString
                    currentsession = reader2f(0).ToString
                    reader2f.Close()
                End If
                For Each item As String In optionalfeec
                    Dim cmdSelect2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT Session, Class, student FROM StudentSummary WHERE student = ? And Session = ?", con)
                    cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentSession))
                    Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2.ExecuteReader
                    reader2.Read()
                    Dim cla As Integer = reader2.Item(1).ToString
                    reader2.Close()
                    If Session("scheduletype") <> "Monthly" Then
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
                        Dim elapsedmonths As Integer
                        If currentterm = "1st term" Then
                            elapsedmonths = Now.Month - 9
                        ElseIf currentterm = "2nd term" Then
                            elapsedmonths = Now.Month - 1
                        Else
                            elapsedmonths = Now.Month - 5

                        End If

                        Dim cmdLoad24 As New MySql.Data.MySqlClient.MySqlCommand("SELECT choicefees.*, class.class from choicefees left join class on class.id = choicefees.class where choicefees.fee = ?", con)
                        cmdLoad24.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", item))
                        Dim student24 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad24.ExecuteReader
                        Dim sesamount As String = ""
                        Dim sesmin As Integer
                        Dim sesmon As Integer
                        Dim sesqua As Integer
                        Do While student24.Read()
                            If (student24(7).ToString <> "" And Session("ClassId") = student24(7).ToString) Or student24(7).ToString = "" Then

                                sesamount = student24.Item(2)
                                sesmin = (student24.Item("amount") * (student24.Item("min") / 100))
                                sesmon = student24.Item(5)
                                sesqua = student24(6)
                            End If
                        Loop
                        student24.Close()




                        Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, min, monthly, termly, quarterly) Values (?,?,?,?,?,?,?,?,?)", con)
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", IIf(Session("scheduletype") = "quarterly", IIf(currentterm = "2nd term", sesqua * 2, sesqua), sesamount)))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", sesmin))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("mon", sesmon))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("adsvmt", sesamount))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("adbcsvmt", sesqua))
                        cmdInsert22.ExecuteNonQuery()

                        Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(IIf(Session("scheduletype") = "quarterly", IIf(currentterm = "2nd term", sesqua * 2, sesqua), sesamount), , , TriState.True)))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", item & " DEBTS"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))
                        cmdCheck2.ExecuteNonQuery()
                        Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(IIf(Session("scheduletype") = "quarterly", IIf(currentterm = "2nd term", sesqua * 2, sesqua), sesamount), , , TriState.True)))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))

                        cmdCheck4.ExecuteNonQuery()

                    ElseIf Session("scheduletype") = "Monthly" Then
                        Dim exists As Boolean = False
                        Dim cmdCheck44 As New MySql.Data.MySqlClient.MySqlCommand("select fee, paid from feeschedule where fee = ? and student = ? and session = ?", con)
                        cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", item))
                        cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                        cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))
                        Dim readref220 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck44.ExecuteReader
                        Dim paid As Integer
                        If readref220.Read Then
                            exists = True
                            paid = readref220("paid")
                        End If
                        readref220.Close()
                        If exists = True Then
                            Dim elapsedmonths As Integer
                            If currentterm = "1st term" Then
                                elapsedmonths = Now.Month - 9
                            ElseIf currentterm = "2nd term" Then
                                elapsedmonths = Now.Month - 1
                            Else
                                elapsedmonths = Now.Month - 5
                            End If
                            Dim cmdLoad24 As New MySql.Data.MySqlClient.MySqlCommand("SELECT choicefees.*, class.class from choicefees left join class on class.id = choicefees.class where choicefees.fee = ?", con)
                            cmdLoad24.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", item))
                            Dim student24 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad24.ExecuteReader
                            Dim sesamount As String = ""
                            Dim sesmin As Integer
                            Dim sesmon As Integer
                            Do While student24.Read()
                                If student24(7).ToString <> "" And Session("ClassId") = student24(7).ToString Then
                                    sesamount = student24.Item("monthly") * (4 - elapsedmonths) + paid
                                    sesmin = (student24.Item("monthly") * (4 - elapsedmonths) + paid) * (student24.Item("min") / 100)
                                    sesmon = student24.Item(5)
                                End If
                            Loop
                            student24.Close()

                            Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("Update feeschedule set amount = ?, min = ?, termly = ? where session = ? and student = ? and fee = ?", con)
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", sesamount))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", sesmin))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amsdt", sesamount))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                            cmdInsert22.ExecuteNonQuery()

                            Dim cmdCheck4r As New MySql.Data.MySqlClient.MySqlCommand("select ref from transactions where account = ? and student = ? and session = ?", con)
                            cmdCheck4r.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", item & " DEBTS"))
                            cmdCheck4r.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                            cmdCheck4r.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))
                            Dim readref220r As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck4r.ExecuteReader
                            Dim refe As Integer
                            If readref220r.Read() Then
                                refe = readref220r.Item(0).ToString
                            End If
                            readref220r.Close()
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

                            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(sesamount, , , TriState.True)))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", item & " DEBTS"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))
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
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))

                            cmdCheck4.ExecuteNonQuery()

                        Else
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
                            Dim elapsedmonths As Integer
                            If currentterm = "1st term" Then
                                elapsedmonths = Now.Month - 9
                            ElseIf currentterm = "2nd term" Then
                                elapsedmonths = Now.Month - 1
                            Else
                                elapsedmonths = Now.Month - 5

                            End If

                            Dim cmdLoad24 As New MySql.Data.MySqlClient.MySqlCommand("SELECT choicefees.*, class.class from choicefees left join class on class.id = choicefees.class where choicefees.fee = ?", con)
                            cmdLoad24.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", item))
                            Dim student24 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad24.ExecuteReader
                            Dim sesamount As String = ""
                            Dim sesmin As Integer
                            Dim sesmon As Integer
                            Dim sesqua As Integer
                            Do While student24.Read()
                                If student24(7).ToString <> "" And Session("ClassId") = student24(7).ToString Then

                                    sesamount = student24.Item("monthly") * 4
                                    sesmin = student24.Item("monthly") * 4 * (student24.Item("min") / 100)
                                    sesmon = student24.Item(5)
                                    sesqua = student24(6)
                                End If
                            Loop
                            student24.Close()




                            Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, min, monthly, termly, quarterly) Values (?,?,?,?,?,?,?,?,?)", con)
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", sesamount))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", sesmin))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("mon", sesmon))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("adsvmt", sesamount))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("adsvmjsdt", sesqua))
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
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))
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
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))

                            cmdCheck4.ExecuteNonQuery()

                        End If



                    End If
                Next
                For Each item As String In optionalfeenc
                    If Session("scheduletype") = "Monthly" Then
                        Dim elapsedmonths As Integer
                        If currentterm = "1st term" Then
                            elapsedmonths = Now.Month - 9
                        ElseIf currentterm = "2nd term" Then
                            elapsedmonths = Now.Month - 1
                        Else
                            elapsedmonths = Now.Month - 5

                        End If
                        Dim cmdCheck44 As New MySql.Data.MySqlClient.MySqlCommand("select fee, paid, amount from feeschedule where fee = ? and student = ? and session = ?", con)
                        cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", item))
                        cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                        cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))
                        Dim readref220 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck44.ExecuteReader
                        Dim paid As Integer
                        Dim amount As Integer
                        If readref220.Read Then
                            paid = readref220("paid")
                            amount = readref220("amount")
                        End If
                        readref220.Close()

                        Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("Update feeschedule set amount = ?, min = ?, termly = ? where session = ? and student = ? and fee = ?", con)
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", paid))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", paid))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amgst", paid))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                        cmdInsert22.ExecuteNonQuery()



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

                        Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(amount - paid, , , TriState.True)))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", item & " DEBTS"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees removed"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))
                        cmdCheck2.ExecuteNonQuery()
                        Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(amount - paid, , , TriState.True)))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees removal"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))

                        cmdCheck4.ExecuteNonQuery()
                    ElseIf Session("scheduletype") = "quarterly" Then
                        Dim elapsedmonths As Integer
                        If currentterm = "1st term" Then
                            elapsedmonths = Now.Month - 9
                        ElseIf currentterm = "2nd term" Then
                            elapsedmonths = Now.Month - 1
                        Else
                            elapsedmonths = Now.Month - 5
                        End If
                        Dim cmdCheck44 As New MySql.Data.MySqlClient.MySqlCommand("select fee, paid, amount from feeschedule where fee = ? and student = ? and session = ?", con)
                        cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", item))
                        cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                        cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))
                        Dim readref220 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck44.ExecuteReader
                        Dim paid As Integer
                        Dim amount As Integer
                        If readref220.Read Then
                            paid = readref220("paid")
                            amount = readref220("amount")
                        End If
                        readref220.Close()

                        Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("Update feeschedule set amount = ?, min = ?, termly = ? where session = ? and student = ? and fee = ?", con)
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", paid))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", paid))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amgst", paid))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                        cmdInsert22.ExecuteNonQuery()



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

                        Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(amount - paid, , , TriState.True)))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", item & " DEBTS"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees removed"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))
                        cmdCheck2.ExecuteNonQuery()
                        Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(amount - paid, , , TriState.True)))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees removal"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))

                        cmdCheck4.ExecuteNonQuery()

                    Else

                        Dim cmdSelect2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT Session, Class, student FROM StudentSummary WHERE student = ? And Session = ?", con)
                        cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                        cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
                        Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2.ExecuteReader
                        reader2.Read()
                        Dim cla As Integer = reader2.Item(1).ToString
                        reader2.Close()
                        Dim cmdInsert222 As New MySql.Data.MySqlClient.MySqlCommand("Delete from feeschedule where session = ? and fee = ? and student = ?", con)
                        cmdInsert222.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
                        cmdInsert222.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                        cmdInsert222.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                        cmdInsert222.ExecuteNonQuery()
                        Dim cmdCheck44 As New MySql.Data.MySqlClient.MySqlCommand("select ref from transactions where account = ? and student = ? and session = ?", con)
                        cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", item & " DEBTS"))
                        cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                        cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))
                        Dim readref220 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck44.ExecuteReader
                        Dim refe As Integer
                        If readref220.Read() Then
                            refe = readref220.Item(0).ToString
                        End If
                        readref220.Close()
                        Dim cmdCheck04 As New MySql.Data.MySqlClient.MySqlCommand("delete from transactions where ref = ?", con)
                        cmdCheck04.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("refa", refe))
                        cmdCheck04.ExecuteNonQuery()
                    End If
                Next
                con.close()            End Using
            load_mandatory()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

   
End Class
