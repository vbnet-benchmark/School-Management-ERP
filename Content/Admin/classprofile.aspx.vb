Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Admin_studentprofile
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
    Sub payment_schedules()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.Open()
            Dim cmdloadfs As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.surname, class.class,  studentsummary.class, class.type From Studentsummary INNER JOIN Studentsprofile on studentsummary.student = studentsprofile.admno inner join class on studentsummary.class = class.Id WHERE StudentSummary.student = ? And StudentSummary.Session = ?", con)
            cmdloadfs.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class.ID", textbox1.text))
            cmdloadfs.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter(" StudentSummary.Session ", Session("sessionid")))
            Dim studentsreadsse As MySql.Data.MySqlClient.MySqlDataReader = cmdloadfs.ExecuteReader()
            studentsreadsse.Read()
            Dim classtype As String
            classtype = studentsreadsse.Item(3)
            studentsreadsse.Close()
            If classtype <> "Early Years" Then
                Session("scheduletype") = "Termly"
            Else

                Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where student = ? order by id", con)
                cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", textbox1.text))
                Dim feereader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                Dim totalall As Integer = 0
                Dim paidall As Double = 0

                Do While feereader3.Read
                    If feereader3("payment").ToString <> "" And Session("scheduletype") = Nothing Then Session("scheduletype") = feereader3("payment").ToString
                    totalall = totalall + feereader3.Item("amount")
                    paidall = paidall + feereader3.Item("paid")
                Loop
                feereader3.Close()
            End If
            Dim currentsession As String
            Dim currentterm As String

            Dim cmdInsertf As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session where id = '" & Session("sessionid") & "' Order by ID Desc", con)
            Dim reader2f As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertf.ExecuteReader
            reader2f.Read()
            currentsession = reader2f(0).ToString
            currentterm = reader2f(2).ToString
            reader2f.Close()

            Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where session = ? and student = ?", con)
            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", textbox1.text))
            Dim feereader As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert22.ExecuteReader()
            Dim monthly As New ArrayList
            Dim amount As New ArrayList
            Dim termly As New ArrayList
            Dim quarterly As New ArrayList
            Dim id As New ArrayList
            Dim fee As New ArrayList
            Do While feereader.Read
                id.Add(feereader("id"))
                monthly.Add(feereader("monthly"))
                amount.Add(feereader("amount"))
                termly.Add(feereader("termly"))
                fee.Add(feereader("fee"))
                quarterly.Add(feereader("quarterly"))
            Loop
            feereader.Close()

            If Session("scheduletype") = "Monthly" Then
                Dim x As Integer
                For Each s As Integer In id
                    If Val(monthly(x)) <> 0 Then
                        Dim cmdInsertf5 As New MySql.Data.MySqlClient.MySqlCommand("update feeschedule set amount = '" & IIf(amount(x) <> 0, Val(monthly(x)) * 4, 0) & "' where id = '" & s & "'", con)
                        cmdInsertf5.ExecuteNonQuery()

                        Dim cmdCheck44 As New MySql.Data.MySqlClient.MySqlCommand("select ref from transactions where account = ? and student = ? and session = ?", con)
                        cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", fee(x) & " DEBTS"))
                        cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", textbox1.text))
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
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(IIf(amount(x) <> 0, Val(monthly(x)) * 4, 0), , , TriState.True)))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", fee(x) & " DEBTS"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", textbox1.text))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))
                        cmdCheck2.ExecuteNonQuery()
                        Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(IIf(amount(x) <> 0, Val(monthly(x)) * 4, 0), , , TriState.True)))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", textbox1.text))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))

                        cmdCheck4.ExecuteNonQuery()

                    End If
                    x += 1
                Next
                Session("scheduletype") = "Monthly"
            ElseIf Session("scheduletype") = "Termly" Then
                Dim x As Integer
                For Each s As Integer In id
                    If Val(monthly(x)) <> 0 Then

                        Dim cmdInsertf5 As New MySql.Data.MySqlClient.MySqlCommand("update feeschedule set amount = '" & IIf(amount(x) <> 0, Val(termly(x)), 0) & "' where id = '" & s & "'", con)
                        cmdInsertf5.ExecuteNonQuery()

                        Dim cmdCheck44 As New MySql.Data.MySqlClient.MySqlCommand("select ref from transactions where account = ? and student = ? and session = ?", con)
                        cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", fee(x) & " DEBTS"))
                        cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", textbox1.text))
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
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(IIf(amount(x) <> 0, Val(termly(x)), 0), , , TriState.True)))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", fee(x) & " DEBTS"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", textbox1.text))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))
                        cmdCheck2.ExecuteNonQuery()
                        Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(IIf(amount(x) <> 0, Val(termly(x)), 0), , , TriState.True)))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", textbox1.text))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))

                        cmdCheck4.ExecuteNonQuery()
                    End If
                    x += 1
                Next
                Session("scheduletype") = "Termly"

            ElseIf Session("scheduletype") = "Quarterly" Then
                Dim x As Integer
                For Each s As Integer In id
                    If Val(quarterly(x)) <> 0 Then
                        Dim cmdInsertf5 As New MySql.Data.MySqlClient.MySqlCommand("update feeschedule set amount = '" & IIf(amount(x) <> 0 And currentterm = "2nd term", Val(quarterly(x)) * 2, IIf(amount(x) <> 0 And currentterm <> "2nd term", Val(quarterly(x)), 0)) & "' where id = '" & s & "'", con)
                        cmdInsertf5.ExecuteNonQuery()

                        Dim cmdCheck44 As New MySql.Data.MySqlClient.MySqlCommand("select ref from transactions where account = ? and student = ? and session = ?", con)
                        cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", fee(x) & " DEBTS"))
                        cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", textbox1.text))
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
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(IIf(amount(x) <> 0 And Session("Term") = "2nd term", Val(quarterly(x)) * 2, IIf(amount(x) <> 0 And Session("Term") <> "2nd term", Val(quarterly(x)), 0)), , , TriState.True)))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", fee(x) & " DEBTS"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", textbox1.text))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))
                        cmdCheck2.ExecuteNonQuery()
                        Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(IIf(amount(x) <> 0 And Session("Term") = "2nd term", Val(quarterly(x)) * 2, IIf(amount(x) <> 0 And Session("Term") <> "2nd term", Val(quarterly(x)), 0)), , , TriState.True)))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", textbox1.text))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))

                        cmdCheck4.ExecuteNonQuery()
                    End If
                    x += 1
                Next
                Session("scheduletype") = "Quarterly"



            End If
            add_discount()
            con.Close()
        End Using
    End Sub

    Sub add_discount()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.Open()

            Dim cmdInsertv As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where student = ? order by id", con)
            cmdInsertv.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", textbox1.text))
            Dim feereader3v As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertv.ExecuteReader
            Dim totalall As Integer = 0
            Dim paidall As Double = 0

            Do While feereader3v.Read
                If feereader3v("payment").ToString <> "" And Session("scheduletype") = Nothing Then Session("scheduletype") = feereader3v("payment").ToString
                totalall = totalall + feereader3v.Item("amount")
                paidall = paidall + feereader3v.Item("paid")
            Loop
            feereader3v.Close()

            Dim currentsession As String
            Dim currentterm As String

            Dim cmdInsertf As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session where id = '" & Session("sessionid") & "' Order by ID Desc", con)
            Dim reader2f As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertf.ExecuteReader
            reader2f.Read()
            currentsession = reader2f(0).ToString
            currentterm = reader2f(2).ToString
            reader2f.Close()

            Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Select * from discount where student = ? and recurring = '" & -Val(True) & "' or session = '" & currentsession & "'", con)
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", textbox1.text))
            Dim feereader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
            Dim discountfee As New ArrayList
            Dim discountamt As New ArrayList
            Dim discounttype As New ArrayList
            Do While feereader3.Read()
                discountfee.Add(feereader3(2))
                discountamt.Add(feereader3(4))
                discounttype.Add(feereader3(3))
            Loop
            feereader3.Close()
            Dim ct As Integer
            For Each item As String In discountfee
                Dim cmdInsertx As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where student = ? and session = ? and fee = ?", con)
                cmdInsertx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", textbox1.text))
                cmdInsertx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
                cmdInsertx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fwe", item))
                Dim feereader3x As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertx.ExecuteReader
                feereader3x.Read()
                Dim fee As Integer = feereader3x.Item(2)
                Dim min As Integer = feereader3x.Item("min")
                Dim month As Integer = feereader3x.Item("monthly")
                Dim quarters As Integer = feereader3x.Item("quarterly")
                feereader3x.Close()
                If discounttype(ct) <> "Months" Then
                    If discounttype(ct) = "Fixed" Then
                        fee = fee - discountamt(ct)
                        min = min - discountamt(ct)
                    ElseIf discounttype(ct) = "Percentage" Then
                        fee = fee - (IIf(quarters <> 0 And currentterm = "2nd term", quarters / 2, quarters) * (discountamt(ct).Replace(",", "") / 100))
                        min = min - (IIf(quarters <> 0 And currentterm = "2nd term", quarters / 2, quarters) * (discountamt(ct).Replace(",", "") / 100))
                    End If
                Else
                    fee = fee - (discountamt(ct) * month)
                    min = fee
                End If

                Dim cmdInsert225 As New MySql.Data.MySqlClient.MySqlCommand("Update feeschedule set amount = '" & fee & "', min = '" & IIf(min < 0, 0, min) & "' where session = ? and fee = ? and student = ?", con)
                cmdInsert225.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
                cmdInsert225.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                cmdInsert225.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", textbox1.text))
                cmdInsert225.ExecuteNonQuery()

                Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("select ref from transactions where account like ? and student = ? and session = ?", con)
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", item & "%"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", textbox1.text))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))
                Dim readref220 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck4.ExecuteReader
                readref220.Read()
                Dim refe As Integer = readref220.Item(0).ToString
                readref220.Close()
                Dim cmdCheck04 As New MySql.Data.MySqlClient.MySqlCommand("update transactions set cr = '" & FormatNumber(fee, , , , TriState.True) & "' where ref = ? and account = '" & item & " DEBTS" & "'", con)
                cmdCheck04.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("refa", refe))
                cmdCheck04.ExecuteNonQuery()

                Dim cmdCheck041 As New MySql.Data.MySqlClient.MySqlCommand("update transactions set dr = '" & FormatNumber(fee, , , , TriState.True) & "' where ref = ? and account = '" & "DEBTORS" & "'", con)
                cmdCheck041.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("refa", refe))
                cmdCheck041.ExecuteNonQuery()

                Dim cmdInsert220 As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where session = ? and student = ?", con)
                cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
                cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", textbox1.text))
                Dim feereader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert220.ExecuteReader
                Dim totals As Integer = 0

                Do While feereader2.Read
                    totals = totals + feereader2.Item("amount")
                Loop
                feereader2.Close()
                If totals = 0 Then
                    Dim cmdInsertcv As New MySql.Data.MySqlClient.MySqlCommand("Update StudentSummary Set status = ? where student = ? and session = '" & currentsession & "'", con)
                    cmdInsertcv.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("aveage", True))
                    cmdInsertcv.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", textbox1.text))
                    cmdInsertcv.ExecuteNonQuery()
                End If
                ct = ct + 1
            Next
            con.Close()
        End Using
    End Sub

    Private Sub update_class(cla As Integer)

        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                If Session("sessionid") <> Nothing Then
                    Dim subjects As New ArrayList
                    Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT subject FROM ClassSubjects WHERE  class = '" & cla & "'", con)
                    Dim reader2a As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                    Do While reader2a.Read
                        subjects.Add(reader2a(0))
                    Loop
                    reader2a.Close()
                    For Each subject As Integer In subjects
                        Dim Saverage As New MySql.Data.MySqlClient.MySqlCommand("SELECT Total, student FROM SubjectReg WHERE Session = ? AND Class = ? AND subjectsOfferred = ? Order by total Desc", con)
                        Saverage.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Sessionid")))
                        Saverage.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                        Saverage.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subjectsOffered", subject))
                        Dim readerT As MySql.Data.MySqlClient.MySqlDataReader = Saverage.ExecuteReader
                        Dim STotal As Double = 0
                        Dim SubjectAverage As Double = 0
                        Dim positionz As String = ""
                        Dim pos As New ArrayList
                        Dim posid As New ArrayList
                        Dim count As Integer = 0
                        Dim highest1 As Double = 0
                        Dim lowest1 As Double = 0
                        Dim y As Integer
                        Do While readerT.Read
                            count = count + 1
                            If count = 1 Then
                                highest1 = Val(readerT.Item("Total"))
                            End If
                            positionz = count
                            Select Case positionz
                                Case 1, 21, 31, 41, 51, 61, 71, 81, 91, 101
                                    positionz = positionz.ToString + "st"
                                Case 2, 22, 32, 42, 52, 62, 72, 82, 92, 102
                                    positionz = positionz.ToString + "nd"
                                Case 3, 23, 33, 43, 53, 63, 73, 83, 93, 103
                                    positionz = positionz.ToString + "rd"
                                Case Else
                                    positionz = positionz.ToString + "th"
                            End Select
                            posid.Add(readerT.Item("student"))
                            pos.Add(positionz)
                            lowest1 = Val(readerT.Item("Total"))
                            STotal = STotal + Val(readerT.Item("Total"))
                        Loop
                        SubjectAverage = FormatNumber(STotal / count, 2)
                        readerT.Close()
                        For y = 0 To posid.Count - 1
                            Dim Updatedatabase2 As New MySql.Data.MySqlClient.MySqlCommand("UPDATE SubjectReg SET avg = ?, Highest = ?, Lowest = ?, pos = ? WHERE Student = ? and SubjectsOfferred = ? and Session = ?", con)
                            Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Average", SubjectAverage))
                            Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.highest", highest1))
                            Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.lowest", lowest1))
                            Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.position", pos(y)))
                            Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.ID", posid(y)))
                            Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.subject", subject))
                            Updatedatabase2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.session", Session("sessionid")))
                            Updatedatabase2.ExecuteNonQuery()
                        Next
                        count = Nothing
                    Next




                    Dim studentID As String
                    Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM SubjectReg WHERE Student = ? And Session = ? And Class = ?", con)
                    Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM StudentSummary WHERE Class = ? And Session = ?", con)
                    cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                    cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Sessionid")))
                    Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmd3.ExecuteReader

                    Dim i As Integer = 0
                    Dim students As New ArrayList
                    Do While reader3.Read
                        students.Add(reader3.Item("student"))
                    Loop
                    reader3.Close()
                    Dim gtotals As New ArrayList

                    Dim ct As Integer = 0
                    For Each item As String In students
                        studentID = item

                        Dim param As MySql.Data.MySqlClient.MySqlParameter = cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", studentId))
                        Dim param4 As MySql.Data.MySqlClient.MySqlParameter = cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Sessionid")))
                        Dim param5 As MySql.Data.MySqlClient.MySqlParameter = cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                        Dim readerf As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader()
                        Dim total As Integer = 0
                        Dim average As Double = 0
                        Dim count As Integer = 0
                        Do While readerf.Read()
                            count = count + 1
                            total = total + Val(readerf.Item("Total"))
                        Loop
                        gtotals.Add(total)
                        readerf.Close()
                        average = total / count

                        average = FormatNumber(average, 2)

                        Dim cmd22 As New MySql.Data.MySqlClient.MySqlCommand("SELECT grades.lowest, grades.average From grades inner join (gradingsystem inner join class on class.gradesystem = gradingsystem.id) on grades.system = gradingsystem.id Where class.id = '" & cla & "' order by grades.lowest desc", con)
                        Dim graderead As MySql.Data.MySqlClient.MySqlDataReader = cmd22.ExecuteReader
                        Dim remarks As String = ""
                        Do While graderead.Read
                            If average >= Val(graderead.Item(0)) Then
                                remarks = graderead.Item(1).ToString
                                Exit Do
                            End If
                        Loop
                        graderead.Close()

                        Dim cmd2c As New MySql.Data.MySqlClient.MySqlCommand("UPDATE StudentSummary SET Average = ?, principalremarks = ? WHERE student = ? And Session = ? And Class = ?", con)
                        Dim param2 As MySql.Data.MySqlClient.MySqlParameter = cmd2c.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Average", average))
                        Dim paramq As MySql.Data.MySqlClient.MySqlParameter = cmd2c.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Ave", remarks))
                        Dim param3 As MySql.Data.MySqlClient.MySqlParameter = cmd2c.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", studentId))
                        Dim param6 As MySql.Data.MySqlClient.MySqlParameter = cmd2c.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Sessionid")))
                        Dim param7 As MySql.Data.MySqlClient.MySqlParameter = cmd2c.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))

                        cmd2c.ExecuteNonQuery()


                        cmd2c.Parameters.Remove(paramq)
                        cmd.Parameters.Remove(param)
                        cmd2c.Parameters.Remove(param2)
                        cmd2c.Parameters.Remove(param3)
                        cmd.Parameters.Remove(param4)
                        cmd.Parameters.Remove(param5)
                        cmd2c.Parameters.Remove(param6)
                        cmd2c.Parameters.Remove(param7)
                        total = Nothing
                        count = Nothing
                        ct = ct + 1

                    Next
                    gtotals.Sort()

                    Dim cmd2x As New MySql.Data.MySqlClient.MySqlCommand("UPDATE StudentSummary SET classhigh = '" & gtotals(ct - 1) & "', classlow = '" & gtotals(0) & "' where Session = ? And Class = ?", con)
                    Dim param6aaa As MySql.Data.MySqlClient.MySqlParameter = cmd2x.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Sessionid")))
                    Dim param7aa As MySql.Data.MySqlClient.MySqlParameter = cmd2x.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                    cmd2x.ExecuteNonQuery()
                    Dim positionNo As Integer
                    Dim position As String

                    Dim cmdxx As New MySql.Data.MySqlClient.MySqlCommand("SELECT Average, student from StudentSummary WHERE Session = ? And Class = ? ORDER BY Average DESC", con)
                    Dim cmd2xx As New MySql.Data.MySqlClient.MySqlCommand("UPDATE StudentSummary SET Position = ? WHERE student = ? And Session = ? And Class = ?", con)
                    cmdxx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Sessionid")))
                    cmdxx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                    Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdxx.ExecuteReader
                    Dim ix As Integer = 0
                    Dim studentsx As New ArrayList
                    Do While reader.Read
                        studentsx.Add(reader.Item("student"))
                    Loop
                    reader.Close()
                    For Each item As String In studentsx
                        positionNo = positionNo + 1
                        Select Case positionNo
                            Case 1, 21, 31, 41, 51, 61, 71, 81, 91, 101
                                If positionNo = 1 Then
                                End If
                                position = positionNo.ToString + "st"
                            Case 2, 22, 32, 42, 52, 62, 72, 82, 92, 102
                                position = positionNo.ToString + "nd"
                            Case 3, 23, 33, 43, 53, 63, 73, 83, 93, 103
                                position = positionNo.ToString + "rd"
                            Case Else
                                position = positionNo.ToString + "th"
                        End Select
                        Dim param As MySql.Data.MySqlClient.MySqlParameter = cmd2xx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Position", position))
                        Dim param2 As MySql.Data.MySqlClient.MySqlParameter = cmd2xx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", item))
                        Dim param3 As MySql.Data.MySqlClient.MySqlParameter = cmd2xx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Sessionid")))
                        Dim param4 As MySql.Data.MySqlClient.MySqlParameter = cmd2xx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                        cmd2xx.ExecuteNonQuery()
                        cmd2xx.Parameters.Remove(param)
                        cmd2xx.Parameters.Remove(param2)
                        cmd2xx.Parameters.Remove(param3)
                        cmd2xx.Parameters.Remove(param4)
                    Next
                End If
              
                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log("update class", Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If check.Check_Admin(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try
            If IsPostBack Then
            Else
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.Open()
                    Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT class from class where superior = '" & Request.QueryString("school") & "'", con)
                    Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                    Do While student.Read
                        DropDownList1.Items.Add(student.Item(0).ToString)
                    Loop
                    DropDownList1.Text = Request.QueryString("currentclass")
                    panel3.Visible = False
                    student.Close()
                    Dim cmdLoad03 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from hostel Order by Id", con)
                    Dim student03 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad03.ExecuteReader
                    cboHostel.Items.Clear()
                    cboHostel.Items.Add("Select Hostel")
                    Do While student03.Read
                        cboHostel.Items.Add(student03.Item(0).ToString)
                    Loop
                    student03.Close()

                    Dim cmdLoad01 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from transportfees Order by Id", con)
                    Dim student01 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad01.ExecuteReader
                    cboTransport.Items.Clear()
                    cboTransport.Items.Add("Select Route")
                    Do While student01.Read
                        cboTransport.Items.Add(student01.Item(0).ToString)
                    Loop
                    student01.Close()

                    Dim cmd40 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from options", con)
                    Dim reader40 As MySql.Data.MySqlClient.MySqlDataReader = cmd40.ExecuteReader
                    reader40.Read()
                    Dim board As Boolean = reader40.Item(0)
                    Dim trans As Boolean = reader40.Item(1)
                    reader40.Close()
                    If board = True Then
                        CheckBox1.Visible = True
                        cboHostel.Visible = True
                    End If
                    If trans = True Then
                        CheckBox2.Visible = True
                        cboTransport.Visible = True
                    End If
                    panel3.Visible = False

                    Dim ds As New DataTable
                    ds.Columns.Add("passport")
                    ds.Columns.Add("staffname")
                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.passport, studentsprofile.admno, studentsprofile.surname from studentsummary inner join studentsprofile on studentsummary.student = studentsprofile.admno inner join class  on studentsummary.class = class.id WHERE class.class = '" & DropDownList1.Text & "' and studentsummary.session = '" & Session("sessionid") & "'", con)
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                    Do While student0.Read
                        ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                    Loop
                    student0.Close()
                    gridview1.DataSource = ds
                    gridview1.DataBind()
                    con.Close()                End Using
                If Session("studentadd") <> Nothing Then
                    Student_Details()
                End If
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Student_Details()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.Open()
            Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport, activated, hostelstay, hostel, admfees, transport from StudentsProfile where admno = ?", con)
            cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("StudentAdd")))
            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
            Dim pass As String
            student.Read()
            pass = student.Item("passport").ToString
            If student.Item("activated") = True Then
                chkActivated.Checked = True
                chkActivated.Text = "Activated"
            Else
                chkActivated.Checked = False
                chkActivated.Text = "Deactivated"
            End If
            If student.Item("hostelstay") = True Then
                CheckBox1.Checked = True
                cboHostel.Enabled = True
                If student.Item("hostel").ToString <> "" Then
                    cboHostel.Text = student.Item("hostel").ToString
                End If
            Else
                CheckBox1.Checked = False
                cboHostel.Enabled = False
            End If
            cboAdm.Text = student.Item("admfees").ToString
            If student.Item("transport").ToString <> "" Then
                cboTransport.Text = student.Item("transport").ToString
                CheckBox2.Checked = True
                cboTransport.Enabled = True
            End If
            student.Close()
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.admno as 'Admission number', studentsprofile.surname as Name, studentsprofile.Sex, studentsprofile.dateOfBirth as Birthday, studentsummary.age as 'Age', studentsprofile.phone as 'Phone number' from StudentsProfile left Join (studentsummary left join class on class.id = studentsummary.class) on studentsummary.student = studentsprofile.admno left join (classteacher inner join staffprofile on classteacher.teacher = staffprofile.staffid) on classteacher.class = studentsummary.class left join (parentward inner join parentprofile on parentward.parent = parentprofile.parentid) on parentward.ward = studentsprofile.admno where studentsprofile.admno = ? order by studentsummary.session desc", con)
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("studentadd")))
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("SessionID")))
            Dim ds As New DataTable
            Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
            adapter1.SelectCommand = cmdLoad1
            adapter1.Fill(ds)

            DetailsView1.DataSource = ds
            DetailsView1.DataBind()

            Dim cmdLoad01 As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentprofile.parentname as 'Parents/Guardians', parentprofile.phone as 'Parents Phone number', parentprofile.address as Address from StudentsProfile left Join (studentsummary left join class on class.id = studentsummary.class) on studentsummary.student = studentsprofile.admno left join (classteacher inner join staffprofile on classteacher.teacher = staffprofile.staffid) on classteacher.class = studentsummary.class left join (parentward inner join parentprofile on parentward.parent = parentprofile.parentid) on parentward.ward = studentsprofile.admno where studentsprofile.admno = ? order by studentsummary.session desc", con)
            cmdLoad01.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("studentadd")))
            cmdLoad01.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("SessionID")))
            Dim ds2 As New DataTable
            Dim adapter2 As New MySql.Data.MySqlClient.MySqlDataAdapter
            adapter2.SelectCommand = cmdLoad01
            adapter2.Fill(ds2)

            DetailsView2.DataSource = ds2
            DetailsView2.DataBind()
            Dim cmdLoad1a As New MySql.Data.MySqlClient.MySqlCommand("SELECT  class.class as Class, staffprofile.surname as 'Class Teacher' from studentsprofile left Join (studentsummary left join class on class.id = studentsummary.class) on studentsummary.student = studentsprofile.admno left join (classteacher inner join staffprofile on classteacher.teacher = staffprofile.staffid) on classteacher.class = studentsummary.class left join (parentward inner join parentprofile on parentward.parent = parentprofile.parentid) on parentward.ward = studentsprofile.admno where studentsprofile.admno = ? order by studentsummary.session desc", con)
            cmdLoad1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("studentadd")))
            cmdLoad1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("SessionID")))
            Dim dsa As New DataTable
            Dim adapter1a As New MySql.Data.MySqlClient.MySqlDataAdapter
            adapter1a.SelectCommand = cmdLoad1a
            adapter1a.Fill(dsa)

            DetailsView3.DataSource = dsa
            DetailsView3.DataBind()


            If Not pass = "" Then Image1.ImageUrl = pass
            con.Close()
        End Using
        pnlAll.Visible = False
        panel3.Visible = True

    End Sub




    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        Session("edit") = "studentprofile"
        Session("classselect") = DropDownList1.Text
        Session("schoolselect") = Request.QueryString("school")
        Session("senders") = "~/content/Admin/classprofile.aspx?school=" & Session("schoolselect") & "&currentclass=" & Session("classselect")
        Response.Redirect("~/content/Admin/addstudent.aspx?" & Session("studentadd"))
    End Sub

    Protected Sub LinkButton4_Click(sender As Object, e As EventArgs) Handles LinkButton4.Click
        Session("edit") = "passport"
        Session("classselect") = DropDownList1.Text
        Session("schoolselect") = Request.QueryString("school")
        Session("senders") = "~/content/Admin/classprofile.aspx?school=" & Session("schoolselect") & "&currentclass=" & Session("classselect")
        Response.Redirect("~/content/Admin/addstudent.aspx?" & Session("studentadd"))
    End Sub



    Protected Sub chkActivated_CheckedChanged(sender As Object, e As EventArgs) Handles chkActivated.CheckedChanged
        Try


            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim active As Boolean = -Val(chkActivated.Checked)
                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update StudentsProfile Set activated = ? Where admno = ?", con)
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("active", active))
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", Session("studentadd")))
                cmdCheck3.ExecuteNonQuery()
                If active = False Then
                    Dim cmdSelect2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT Session, Class, student, age FROM StudentSummary WHERE student = ? And Session = ?", con)
                    cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                    Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2.ExecuteReader
                    reader2.Read()
                    Dim cla As Integer = reader2.Item(1).ToString
                    Dim age As String = reader2.Item(3).ToString
                    reader2.Close()
                    Dim cmdSelect2ax1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT smsno FROM Session WHERE id = ?", con)
                    cmdSelect2ax1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionId")))
                    Dim reader2a As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2ax1.ExecuteReader
                    reader2a.Read()
                    Dim smsnos As Integer = reader2a(0)
                    reader2a.Close()
                    Dim cmdInsert2aa As New MySql.Data.MySqlClient.MySqlCommand("Update session set smsno = ? where id = ?", con)
                    cmdInsert2aa.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("aveage", smsnos - 35))
                    cmdInsert2aa.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("No", Session("Sessionid")))
                    cmdInsert2aa.ExecuteNonQuery()
                    Dim cmdSelect2ac As New MySql.Data.MySqlClient.MySqlCommand("Delete FROM StudentSummary WHERE student = ? And Session = ?", con)
                    cmdSelect2ac.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdSelect2ac.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                    cmdSelect2ac.ExecuteNonQuery()

                  

                    Dim cmdSelect2ax As New MySql.Data.MySqlClient.MySqlCommand("Delete FROM kscoresheet WHERE student = ? And Session = ?", con)
                    cmdSelect2ax.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdSelect2ax.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                    cmdSelect2ax.ExecuteNonQuery()
                    Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("Delete from SubjectReg WHERE student = ? And Session = ?", con)
                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                    cmd.ExecuteNonQuery()

                   

                    Dim cmdSelect2s As New MySql.Data.MySqlClient.MySqlCommand("SELECT age FROM StudentSummary WHERE class = ?", con)
                    cmdSelect2s.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", cla))
                    Dim reader2s As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2s.ExecuteReader
                    Dim average As Integer
                    Dim total As Integer
                    Dim count As Integer
                    Do While reader2s.Read()
                        total = total + reader2s.Item("age")
                        count = count + 1
                    Loop
                    If Not count = 0 Then
                        average = total \ count
                    End If
                    reader2s.Close()

                    Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update StudentSummary Set Aveage = ?, ClassNo = ? where class = ?", con)
                    cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("aveage", average))
                    cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("No", count))
                    cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", cla))
                    cmdInsert2.ExecuteNonQuery()

                    Dim cmdckg As New MySql.Data.MySqlClient.MySqlCommand("Update studentsprofile Set hostelstay = ? Where admno = ?", con)
                    cmdckg.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", 0))
                    cmdckg.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sex", Session("studentadd")))
                    cmdckg.ExecuteNonQuery()

                    Dim cmdCheck3d As New MySql.Data.MySqlClient.MySqlCommand("Update studentsprofile Set hostel = ?, transport = '" & "" & "' Where admno = ?", con)
                    cmdCheck3d.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", ""))
                    cmdCheck3d.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sex", Session("studentadd")))
                    cmdCheck3d.ExecuteNonQuery()
                    Dim cmdSelect2q1 As New MySql.Data.MySqlClient.MySqlCommand("Delete FROM discount WHERE student = ?", con)
                    cmdSelect2q1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdSelect2q1.ExecuteNonQuery()

                    Dim cmdSelect2sa As New MySql.Data.MySqlClient.MySqlCommand("SELECT amount, paid from feeschedule where student = '" & Session("studentadd") & "' and session = '" & Session("sessionid") & "'", con)
                    Dim reader2sa As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2sa.ExecuteReader
                    Dim paid As Integer
                    Dim amount As Integer
                    Dim owed As Integer
                    Do While reader2sa.Read
                        amount = amount + reader2sa.Item(0)
                        paid = paid + reader2sa.Item(1)
                    Loop
                    owed = amount - paid
                    reader2sa.Close()
                    Dim cmdSelect2q As New MySql.Data.MySqlClient.MySqlCommand("Delete FROM feeschedule WHERE student = ? And Session = ?", con)
                    cmdSelect2q.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdSelect2q.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                    cmdSelect2q.ExecuteNonQuery()
                    If owed <> 0 Then
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

                        Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(owed, , , TriState.True)))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Student withdrawal"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))

                        cmdCheck2.ExecuteNonQuery()

                        Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Expense"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(owed, , , TriState.True)))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "BAD DEBTS"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Student withdrawal"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))

                        cmdCheck4.ExecuteNonQuery()
                    End If
                    logify.log(Session("staffid"), par.getstuname(Session("studentadd")) & " was deactivated")
                    Session("studentadd") = Nothing
                    panel3.Visible = False
                    Dim ds As New DataTable
                    ds.Rows.Clear()
                    ds.Columns.Add("passport")
                    ds.Columns.Add("staffname")
                    Dim cmdLoad0s As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.passport, studentsprofile.admno, studentsprofile.surname from studentsummary inner join studentsprofile on studentsummary.student = studentsprofile.admno inner join (class inner join classteacher on classteacher.class = class.id) on studentsummary.class = class.id WHERE class.class = '" & DropDownList1.Text & "'", con)
                    Dim student0s As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0s.ExecuteReader
                    Do While student0s.Read
                        ds.Rows.Add(student0s.Item(0).ToString, student0s.Item(1) & " - " & student0s.Item(2).ToString)
                    Loop
                    student0s.Close()
                    gridview1.DataSource = Nothing
                    gridview1.DataSource = ds
                    gridview1.DataBind()


                End If
                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub



    Protected Sub gridview1_PageIndexChanged(sender As Object, e As EventArgs) Handles gridview1.PageIndexChanged

    End Sub

    Protected Sub gridview1_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gridview1.PageIndexChanging
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim ds As New DataTable
                ds.Columns.Add("passport")
                ds.Columns.Add("staffname")
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.passport, studentsprofile.admno, studentsprofile.surname from studentsummary inner join studentsprofile on studentsummary.student = studentsprofile.admno inner join (class inner join classteacher on classteacher.class = class.id) on studentsummary.class = class.id WHERE class.class = ?", con)
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", DropDownList1.Text))
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
                Do While student0.Read
                    ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                Loop
                student0.Close()
                gridview1.DataSource = ds
                gridview1.DataBind()
                con.Close()            End Using
            gridview1.PageIndex = e.NewPageIndex
            gridview1.DataBind()
            Session("studentadd") = Nothing
            panel3.Visible = False
            gridview1.SelectedIndex = -1
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try

    End Sub

    Protected Sub gridview1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gridview1.SelectedIndexChanged

    End Sub

    Protected Sub gridview1_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles gridview1.SelectedIndexChanging
        Dim x As Array = Split(gridview1.Rows(e.NewSelectedIndex).Cells(1).Text, " - ")
        Session("studentadd") = RTrim(x(0))
        Student_Details()
    End Sub

    Protected Sub lnkPwd_Click(sender As Object, e As EventArgs) Handles lnkPwd.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update StudentsProfile Set password = ? where admno = ?", con)
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("password", "password"))
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", Session("Studentadd")))
                cmdCheck3.ExecuteNonQuery()
                Show_Alert(True, "Password reset successfully")
                logify.log(Session("staffid"), "The password of " & par.getstuname(Session("studentadd")) & " was reset")

                con.Close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub



    Protected Sub cboHostel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboHostel.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                If Not cboHostel.Text = "Select Hostel" Then
                    Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update studentsprofile Set hostel = ? Where admno = ?", con)
                    cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", cboHostel.Text))
                    cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sex", Session("studentadd")))
                    cmdCheck3.ExecuteNonQuery()
                    logify.log(Session("staffid"), par.getstuname(Session("studentadd")) & " was made a boarder in " & cboHostel.Text)
                    logify.Notifications("You are now a boarder in " & cboHostel.Text, Session("studentadd"), Session("staffid"), "Admin", "")
                    If par.getparent(Session("studentadd")) <> "" Then logify.Notifications(par.getstuname(Session("studentadd")) & " is now a boarder in " & cboHostel.Text, par.getparent(Session("studentadd")), Session("staffid"), "Admin", "")

                Else
                    Show_Alert(False, "Select a hostel")
                End If
                con.Close()            End Using
            Student_Details()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub cboAdm_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboAdm.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update studentsprofile Set admfees = ? Where admno = ?", con)
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", cboAdm.Text))
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sex", Session("studentadd")))
                cmdCheck3.ExecuteNonQuery()

                Dim cmdSelect2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT Session, Class, student FROM StudentSummary WHERE student = ? And Session = ?", con)
                cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2.ExecuteReader
                reader2.Read()
                Dim cla As Integer = reader2.Item(1).ToString
                reader2.Close()
                Dim cmdLoad3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from onetimefees", con)
                Dim student3 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad3.ExecuteReader
                Dim admfee As New ArrayList
                Dim admamount As New ArrayList
                Dim admin As New ArrayList
                Dim admi As Integer
                Do While student3.Read

                    admfee.Add(student3.Item(1))
                    admamount.Add(student3.Item(2))
                    admin.Add(student3.Item("amount") * (student3.Item("min") / 100))
                Loop
                student3.Close()
                Dim cmdLoad3z As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from classonefees where class = '" & cla & "'", con)
                Dim student3z As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad3z.ExecuteReader

                Do While student3z.Read

                    admfee.Add(student3z.Item(2))
                    admamount.Add(student3z.Item(3))
                    admin.Add(student3z.Item("amount") * (student3z.Item("min") / 100))
                Loop
                student3z.Close()
                If cboAdm.Text = "Not Paid" Then
                    For Each item As String In admfee
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
                        Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, compulsory, min) Values (?,?,?,?,?,?,?)", con)
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", admamount(admi)))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("compulsory", 1))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", admin(admi)))
                        cmdInsert22.ExecuteNonQuery()

                        Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d2))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(admamount(admi), , , TriState.True)))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", item & " DEBTS"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                        cmdCheck2.ExecuteNonQuery()

                        Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d2))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(admamount(admi), , , TriState.True)))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                        cmdCheck4.ExecuteNonQuery()
                        admi = admi + 1
                    Next


                Else
                    For Each item As String In admfee

                        Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("Delete from feeschedule where session = ? and fee = ? and student = ?", con)
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                        cmdInsert22.ExecuteNonQuery()
                        Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("select ref from transactions where account like ? and student = ?", con)
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", item & "%"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                        Dim readref220 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck4.ExecuteReader
                        readref220.Read()
                        Dim refe As Integer = readref220.Item(0).ToString
                        readref220.Close()
                        Dim cmdCheck04 As New MySql.Data.MySqlClient.MySqlCommand("delete from transactions where ref = ?", con)
                        cmdCheck04.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("refa", refe))
                        cmdCheck04.ExecuteNonQuery()
                    Next

                End If

                con.Close()            End Using
            Student_Details()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim board As Boolean = CheckBox1.Checked
                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update studentsprofile Set hostelstay = ? Where admno = ?", con)
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", -Val(board)))
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sex", Session("studentadd")))
                cmdCheck3.ExecuteNonQuery()

                Dim cmdSelect2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT Session, Class, student FROM StudentSummary WHERE student = ? And Session = ?", con)
                cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2.ExecuteReader
                reader2.Read()
                Dim cla As Integer = reader2.Item(1).ToString
                reader2.Close()
                If board = True Then
                    cboHostel.Enabled = True
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
                    Dim cmdLoad24 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from boarding", con)
                    Dim student24 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad24.ExecuteReader
                    student24.Read()

                    Dim sesamount As String = student24.Item(0)
                    Dim sesmin As Integer = (student24.Item("cost") * (student24.Item("min") / 100))
                    student24.Close()
                    Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, min) Values (?,?,?,?,?,?)", con)
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", "BOARDING"))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", sesamount))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", sesmin))
                    cmdInsert22.ExecuteNonQuery()

                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(sesamount, , , TriState.True)))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "BOARDING DEBTS"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))
                    cmdCheck2.ExecuteNonQuery()
                    Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(sesamount, , , TriState.True)))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))

                    cmdCheck4.ExecuteNonQuery()
                Else
                    cboHostel.Enabled = False
                    Dim cmdCheck32 As New MySql.Data.MySqlClient.MySqlCommand("Update studentsprofile Set hostel = ? Where admno = ?", con)
                    cmdCheck32.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", ""))
                    cmdCheck32.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sex", Session("studentadd")))
                    cmdCheck32.ExecuteNonQuery()
                    Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("Delete from feeschedule where session = ? and fee = ? and student = ?", con)
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", "BOARDING"))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdInsert22.ExecuteNonQuery()
                    Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("select ref from transactions where account like ? and student = ? and session = ?", con)
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", "BOARDING" & "%"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))
                    Dim readref220 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck4.ExecuteReader
                    readref220.Read()
                    Dim refe As Integer = readref220.Item(0).ToString
                    readref220.Close()
                    Dim cmdCheck04 As New MySql.Data.MySqlClient.MySqlCommand("delete from transactions where ref = ?", con)
                    cmdCheck04.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("refa", refe))
                    cmdCheck04.ExecuteNonQuery()
                    logify.log(Session("staffid"), par.getstuname(Session("studentadd")) & " was removed as a boarder in " & cboHostel.Text)
                    logify.Notifications("You have been removed from being a boarder in " & cboHostel.Text, Session("studentadd"), Session("staffid"), "Admin", "")
                    If par.getparent(Session("studentadd")) <> "" Then logify.Notifications(par.getstuname(Session("studentadd")) & " has been removed from being a boarder in " & cboHostel.Text, par.getparent(Session("studentadd")), Session("staffid"), "Admin", "")

                End If
                con.Close()            End Using
            Student_Details()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        Try
            Dim trans As Boolean = CheckBox2.Checked
            If trans = True Then
                cboTransport.Enabled = True
                cboTransport.Text = "Select Route"
            Else
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.Open()
                    cboTransport.Enabled = False
                    Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update studentsprofile Set transport = ? Where admno = ?", con)
                    cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", ""))
                    cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sex", Session("studentadd")))
                    cmdCheck3.ExecuteNonQuery()

                    Dim cmdSelect2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT Session, Class, student FROM StudentSummary WHERE student = ? And Session = ?", con)
                    cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                    Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2.ExecuteReader
                    reader2.Read()
                    Dim cla As Integer = reader2.Item(1).ToString
                    reader2.Close()


                    Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("Delete from feeschedule where session = ? and fee = ? and student = ?", con)
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", "TRANSPORT"))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdInsert22.ExecuteNonQuery()
                    Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("select ref from transactions where account like ? and student = ? and session = ?", con)
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", "TRANSPORT" & "%"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))
                    Dim readref220 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck4.ExecuteReader
                    readref220.Read()
                    Dim refe As Integer = readref220.Item(0).ToString
                    readref220.Close()
                    Dim cmdCheck04 As New MySql.Data.MySqlClient.MySqlCommand("delete from transactions where ref = ?", con)
                    cmdCheck04.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("refa", refe))
                    cmdCheck04.ExecuteNonQuery()
                    logify.log(Session("staffid"), par.getstuname(Session("studentadd")) & " was removed from transport")
                    logify.Notifications("You have been removed from transport", Session("studentadd"), Session("staffid"), "Admin", "")
                    If par.getparent(Session("studentadd")) <> "" Then logify.Notifications(par.getstuname(Session("studentadd")) & " has been removed from transport", par.getparent(Session("studentadd")), Session("staffid"), "Admin", "")
                    con.Close()
                End Using
            End If

            Student_Details()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try

    End Sub

    Protected Sub cboTransport_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTransport.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()





                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update studentsprofile Set transport = ? Where admno = ?", con)
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", cboTransport.Text))
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sex", Session("studentadd")))
                cmdCheck3.ExecuteNonQuery()

                Dim cmdSelect2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT Session, Class, student FROM StudentSummary WHERE student = ? And Session = ?", con)
                cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2.ExecuteReader
                reader2.Read()
                Dim cla As Integer = reader2.Item(1).ToString
                reader2.Close()


                Dim cmdInsert222 As New MySql.Data.MySqlClient.MySqlCommand("Delete from feeschedule where session = ? and fee = ? and student = ?", con)
                cmdInsert222.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                cmdInsert222.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", "TRANSPORT"))
                cmdInsert222.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                cmdInsert222.ExecuteNonQuery()
                Dim cmdCheck44 As New MySql.Data.MySqlClient.MySqlCommand("select ref from transactions where account like ? and student = ? and session = ?", con)
                cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", "TRANSPORT" & "%"))
                cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))
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
                cmdLoad24.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", cboTransport.Text))
                Dim student24 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad24.ExecuteReader
                student24.Read()

                Dim sesamount As String = student24.Item(1)
                Dim sesmin As Integer = (student24.Item("amount") * (student24.Item("min") / 100))
                student24.Close()
                Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, min) Values (?,?,?,?,?,?)", con)
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", "TRANSPORT"))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", sesamount))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
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
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))
                cmdCheck2.ExecuteNonQuery()
                Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(sesamount, , , TriState.True)))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))

                cmdCheck4.ExecuteNonQuery()
                con.Close()            End Using
            Student_Details()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub


    Protected Sub lnkClass_Click(sender As Object, e As EventArgs) Handles lnkClass.Click
        Session("senders") = Request.RawUrl
        Response.Redirect("~/content/Admin/newclass.aspx")


    End Sub
    Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim ds As New DataTable
                ds.Columns.Add("passport")
                ds.Columns.Add("staffname")
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.passport, studentsprofile.admno, studentsprofile.surname from studentsummary inner join studentsprofile on studentsummary.student = studentsprofile.admno inner join class on studentsummary.class = class.id WHERE class.class = ? and studentsummary.session = '" & Session("sessionid") & "'", con)
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", DropDownList1.Text))
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
                Do While student0.Read
                    ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                Loop
                student0.Close()
                gridview1.DataSource = ds
                gridview1.DataBind()
                Session("studentadd") = Nothing
                panel3.Visible = False
                gridview1.SelectedIndex = -1
                con.Close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from StudentsProfile where admno = ?", con)
                cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", TextBox1.Text))
                Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                If Not student.Read() Then
                    Show_Alert(False, "Admission number invalid")
                    student.Close()
                    Exit Sub
                Else
                    Dim dob As TimeSpan = Now.Subtract(DateTime.ParseExact(student.Item("dateofbirth"), "dd/MM/yyyy", Nothing))
                    Dim age As Integer = dob.TotalDays \ 365
                    student.Close()
                    Dim cmdLoad10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id, type from class where class = ?", con)
                    cmdLoad10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", DropDownList1.Text))
                    Dim student04 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10.ExecuteReader
                    student04.Read()
                    Dim cla As Integer = student04.Item(0)
                    Dim clatype As String = student04(1).ToString
                    student04.Close()
                    Dim cmdSelect2c As New MySql.Data.MySqlClient.MySqlCommand("Update studentsprofile set activated = '" & 1 & "' where admno = '" & TextBox1.Text & "'", con)
                    cmdSelect2c.ExecuteNonQuery()
                    Dim cmdSelect2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT Session, Class, student FROM StudentSummary WHERE student = ? And Session = ?", con)
                    cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                    cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                    Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2.ExecuteReader
                    reader2.Read()
                    If reader2.HasRows Then
                        reader2.Close()
                        Show_Alert(False, "Student previously registerred in a class")
                        Exit Sub
                    Else
                        reader2.Close()
                        Dim total As Double
                        Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from classfees where class = ?", con)
                        cmdLoad0.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", cla))
                        Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                        Dim classfee As New ArrayList
                        Dim quarterly As New ArrayList
                        Dim classamount As New ArrayList
                        Dim min As New ArrayList
                        Dim monthly As New ArrayList
                        Dim classi As Integer
                        Do While student0.Read
                            Dim z As Integer
                            classfee.Add(student0.Item(2))
                            classamount.Add(student0.Item(3))
                            min.Add(student0.Item("amount") * (student0.Item("min") / 100))
                            monthly.Add(student0.Item(5))
                            quarterly.Add(student0.Item(6))
                        Loop
                        student0.Close()
                        For Each item As String In classfee
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
                            Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, compulsory, min, monthly, quarterly, termly) Values (?,?,?,?,?,?,?,?,?,?)", con)
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Sessionid")))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", classamount(classi)))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("compulsory", 1))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", min(classi)))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("mon", monthly(classi)))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amgsdgt", quarterly(classi)))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amsat", classamount(classi)))
                            cmdInsert22.ExecuteNonQuery()
                            total = total + classamount(classi)



                            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(classamount(classi), , , TriState.True)))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", item & " DEBTS"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))

                            cmdCheck2.ExecuteNonQuery()

                            Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(classamount(classi), , , TriState.True)))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))

                            cmdCheck4.ExecuteNonQuery()

                            classi = classi + 1
                        Next





                        Dim cmdLoad2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from optionalfees", con)
                        Dim student2 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad2.ExecuteReader
                        Dim genfee As New ArrayList
                        Dim genamount As New ArrayList
                        Dim geni As Integer
                        Dim genmin As New ArrayList
                        Do While student2.Read
                            genfee.Add(student2.Item(1))
                            genamount.Add(student2.Item(2))
                            genmin.Add(student2.Item("amount") * (student2.Item("min") / 100))
                        Loop
                        student2.Close()
                        For Each item As String In genfee
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
                            Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, compulsory, min) Values (?,?,?,?,?,?,?)", con)
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", genamount(geni)))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("compulsory", 1))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", genmin(geni)))
                            cmdInsert22.ExecuteNonQuery()
                            total = total + genamount(geni)

                            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d1))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(genamount(geni), , , TriState.True)))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", item & " DEBTS"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))

                            cmdCheck2.ExecuteNonQuery()


                            Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d1))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(genamount(geni), , , TriState.True)))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))
                            cmdCheck4.ExecuteNonQuery()

                            geni = geni + 1
                        Next





                        Dim cmdLoad3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from onetimefees", con)
                        Dim student3 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad3.ExecuteReader
                        Dim admfee As New ArrayList
                        Dim admamount As New ArrayList
                        Dim admin As New ArrayList
                        Dim admi As Integer
                        Do While student3.Read

                            admfee.Add(student3.Item(1))
                            admamount.Add(student3.Item(2))
                            admin.Add(student3.Item("amount") * (student3.Item("min") / 100))
                        Loop
                        student3.Close()
                        Dim hostel As String
                        Dim transport As String
                        Dim feeding As String
                        Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT admfees, hostelstay, transport from StudentsProfile where admno = ?", con)
                        cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", TextBox1.Text))

                        Dim studentadm As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                        studentadm.Read()
                        hostel = studentadm.Item(1)
                        transport = studentadm.Item(2)
                        Dim paid As String = studentadm.Item(0)
                        studentadm.Close()
                        If paid = "Not Paid" Then
                            For Each item As String In admfee
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
                                Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, compulsory, min) Values (?,?,?,?,?,?,?)", con)
                                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", admamount(admi)))
                                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("compulsory", 1))
                                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", admin(admi)))
                                cmdInsert22.ExecuteNonQuery()
                                total = total + admamount(admi)

                                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d2))
                                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(admamount(admi), , , TriState.True)))
                                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", item & " DEBTS"))
                                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))
                                cmdCheck2.ExecuteNonQuery()

                                Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d2))
                                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(admamount(admi), , , TriState.True)))
                                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))
                                cmdCheck4.ExecuteNonQuery()
                                admi = admi + 1



                            Next

                        End If
                        If Session("term") = "1st term" Then




                            Dim cmdLoad24 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from sessionalfees", con)
                            Dim student24 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad24.ExecuteReader
                            Dim sesfee As New ArrayList
                            Dim sesamount As New ArrayList
                            Dim sesi As Integer
                            Dim sesmin As New ArrayList
                            Do While student24.Read

                                sesfee.Add(student24.Item(1))
                                sesamount.Add(student24.Item(2))
                                sesmin.Add(student24.Item("amount") * (student24.Item("min") / 100))
                            Loop
                            student24.Close()
                            For Each item As String In sesfee
                                Dim test23 As Boolean
                                Dim f3 As New Random
                                Dim ref23 As New MySql.Data.MySqlClient.MySqlCommand("Select ref from transactions", con)
                                Dim readref23 As MySql.Data.MySqlClient.MySqlDataReader = ref23.ExecuteReader

                                Dim refs23 As New ArrayList
                                Do While readref23.Read
                                    refs23.Add(readref23.Item(0))
                                Loop
                                Dim d3 As Integer
                                Do Until test23 = True
                                    d3 = f3.Next(100000, 999999)
                                    If refs23.Contains(d3) Then
                                        test23 = False
                                    Else
                                        test23 = True
                                    End If
                                Loop
                                readref23.Close()
                                Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, compulsory, min) Values (?,?,?,?,?,?,?)", con)
                                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", sesfee(sesi)))
                                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", sesamount(sesi)))
                                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("compulsory", 1))
                                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", sesmin(sesi)))
                                cmdInsert22.ExecuteNonQuery()
                                total = total + sesamount(sesi)


                                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d3))
                                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(sesamount(sesi), , , TriState.True)))
                                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", sesfee(sesi) & " DEBTS"))
                                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))

                                cmdCheck2.ExecuteNonQuery()
                                Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d3))
                                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(sesamount(sesi), , , TriState.True)))
                                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))

                                cmdCheck4.ExecuteNonQuery()
                                sesi = sesi + 1
                            Next
                        End If
                        If hostel = True Then

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
                            Dim cmdLoad24 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from boarding", con)
                            Dim student24 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad24.ExecuteReader
                            student24.Read()

                            Dim sesamount As String = student24.Item(0)
                            Dim sesmin As Integer = (student24.Item("cost") * (student24.Item("min") / 100))
                            student24.Close()
                            Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, min) Values (?,?,?,?,?,?)", con)
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", "BOARDING"))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", sesamount))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", sesmin))
                            cmdInsert22.ExecuteNonQuery()
                            total = total + sesamount

                            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(sesamount, , , TriState.True)))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "BOARDING DEBTS"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))

                            cmdCheck2.ExecuteNonQuery()
                            Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(sesamount, , , TriState.True)))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))

                            cmdCheck4.ExecuteNonQuery()

                        End If
                        If transport <> "" Then
                            Dim test25 As Boolean
                            Dim f5 As New Random
                            Dim ref25 As New MySql.Data.MySqlClient.MySqlCommand("Select ref from transactions", con)
                            Dim readref25 As MySql.Data.MySqlClient.MySqlDataReader = ref25.ExecuteReader

                            Dim refs25 As New ArrayList
                            Do While readref25.Read
                                refs25.Add(readref25.Item(0))
                            Loop
                            Dim d5 As Integer
                            Do Until test25 = True
                                d5 = f5.Next(100000, 999999)
                                If refs25.Contains(d5) Then
                                    test25 = False
                                Else
                                    test25 = True
                                End If
                            Loop
                            readref25.Close()
                            Dim cmdLoad24 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from transportfees where route = ?", con)
                            cmdLoad24.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", transport))
                            Dim student24 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad24.ExecuteReader
                            student24.Read()
                            Dim sesfee As String = student24.Item(0)
                            Dim sesamount As String = student24.Item(1)
                            Dim sesmin As Integer = (student24.Item("amount") * (student24.Item("min") / 100))
                            student24.Close()
                            Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, min) Values (?,?,?,?,?,?)", con)
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", "TRANSPORT"))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", sesamount))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", sesmin))
                            cmdInsert22.ExecuteNonQuery()
                            total = total + sesamount

                            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d5))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(sesamount, , , TriState.True)))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "TRANSPORT DEBTS"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))

                            cmdCheck2.ExecuteNonQuery()

                            Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d5))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(sesamount, , , TriState.True)))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))

                            cmdCheck4.ExecuteNonQuery()
                        End If




                        Dim cmdInsert2a As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO StudentSummary (Session, Class, student, age) Values (?,?,?,?)", con)
                        cmdInsert2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                        cmdInsert2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                        cmdInsert2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                        cmdInsert2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("age", age))

                        cmdInsert2a.ExecuteNonQuery()

                        payment_schedules()


                       
                        Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dr, cr from transactions where account = ? and student = ? order by date desc", con)
                        cmdLoad11.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("acc", "ADVANCE FEE PAYMENT"))
                        cmdLoad11.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                        Dim balreader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                        Dim dr As Integer
                        Dim cr As Integer
                        Dim balance As Double = 0
                        Do While balreader.Read
                            dr = dr + Val(balreader.Item("dr").replace(",", ""))
                            cr = cr + Val(balreader.Item("cr").replace(",", ""))
                        Loop
                        balance = cr - dr
                        balreader.Close()
                        cr = Nothing
                        dr = Nothing
                        Dim init As Double = balance
                        If balance > 0 Then
                            Dim cmdInsert220 As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where session = ? and student = ? ", con)
                            cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                            cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                            Dim feereader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert220.ExecuteReader
                            Dim total2 As Integer = 0
                            Dim feetype As New ArrayList
                            Dim feeamount As New ArrayList
                            Dim paid2 As Double = 0
                            Dim min2 As Double = 0
                            Dim k As Integer = 0
                            Do While feereader2.Read
                                If feereader2.Item("amount") = feereader2.Item("min") And balance > (feereader2.Item("amount") - feereader2.Item("paid")) Then
                                    Session("ses" & k) = feereader2.Item("session")
                                    Session("paid" & k) = feereader2.Item("paid")
                                    Session("fee" & k) = feereader2.Item("fee")
                                    Session("amount" & k) = feereader2.Item("amount")
                                    balance = balance - (Session("amount" & k) - Session("paid" & k))
                                    k = k + 1
                                ElseIf feereader2.Item("amount") = feereader2.Item("min") And balance < (feereader2.Item("amount") - feereader2.Item("paid")) And balance > 0 Then
                                    Session("fee" & k) = feereader2.Item("fee")
                                    Session("amount" & k) = balance
                                    Session("ses" & k) = feereader2.Item("session")
                                    Session("paid" & k) = feereader2.Item("paid")
                                    balance = balance - (Session("amount" & k) - Session("paid" & k))
                                    Exit Do
                                End If
                            Loop
                            feereader2.Close()

                            Dim cmdInsert4 As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where session = ? and student = ? ", con)
                            cmdInsert4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                            cmdInsert4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))

                            Dim feereader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert4.ExecuteReader
                            Do While feereader3.Read
                                If feereader3.Item("amount") <> feereader3.Item("min") And balance > (feereader3.Item("amount") - feereader3.Item("paid")) Then
                                    Session("ses" & k) = feereader3.Item("session")
                                    Session("paid" & k) = feereader3.Item("paid")
                                    Session("fee" & k) = feereader3.Item("fee")
                                    Session("amount" & k) = feereader3.Item("amount")
                                    balance = balance - (Session("amount" & k) - Session("paid" & k))
                                    k = k + 1
                                ElseIf feereader3.Item("amount") <> feereader3.Item("min") And balance < (feereader3.Item("amount") - feereader3.Item("paid")) And balance > 0 Then
                                    Session("fee" & k) = feereader3.Item("fee")
                                    Session("amount" & k) = balance
                                    Session("ses" & k) = feereader3.Item("session")
                                    Session("paid" & k) = feereader3.Item("paid")
                                    balance = balance - (Session("amount" & k) - Session("paid" & k))
                                    Exit Do
                                End If

                            Loop
                            feereader3.Close()
                            Session("count") = k



                            For k = 0 To Session("count")
                                Dim test25 As Boolean
                                Dim f5 As New Random
                                Dim ref25 As New MySql.Data.MySqlClient.MySqlCommand("Select ref from transactions", con)
                                Dim readref25 As MySql.Data.MySqlClient.MySqlDataReader = ref25.ExecuteReader

                                Dim refs25 As New ArrayList
                                Do While readref25.Read
                                    refs25.Add(readref25.Item(0))
                                Loop
                                Dim d5 As Integer
                                Do Until test25 = True
                                    d5 = f5.Next(100000, 999999)
                                    If refs25.Contains(d5) Then
                                        test25 = False
                                    Else
                                        test25 = True
                                    End If
                                Loop
                                readref25.Close()
                                If Session("amount" & k) <> Nothing Then
                                    Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("Update feeschedule Set paid = ? where fee = ? and student = ? and session = ?", con)
                                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", Session("amount" & k)))
                                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", Session("fee" & k)))
                                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("ses" & k)))
                                    cmdInsert22.ExecuteNonQuery()

                                    Dim cmdCheck03 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                                    cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d5))
                                    cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                                    cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(Session("amount" & k) - Session("paid" & k), , , TriState.True)))
                                    cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", Session("fee" & k) & " DEBTS"))
                                    cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees paid"))
                                    cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                                    cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                                    cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                                    cmdCheck03.ExecuteNonQuery()

                                    Dim cmdCheck02 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                                    cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d5))
                                    cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                                    cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(Session("amount" & k) - Session("paid" & k), , , TriState.True)))
                                    cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", Session("fee" & k) & " PAID"))
                                    cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees paid"))
                                    cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                                    cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                                    cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                                    cmdCheck02.ExecuteNonQuery()
                                    Session("fee" & k) = Nothing
                                    Session("amount" & k) = Nothing
                                    Session("paid" & k) = Nothing
                                End If
                            Next
                            Dim test26 As Boolean
                            Dim f6 As New Random
                            Dim ref26 As New MySql.Data.MySqlClient.MySqlCommand("Select ref from transactions", con)
                            Dim readref26 As MySql.Data.MySqlClient.MySqlDataReader = ref26.ExecuteReader

                            Dim refs26 As New ArrayList
                            Do While readref26.Read
                                refs26.Add(readref26.Item(0))
                            Loop
                            Dim d6 As Integer
                            Do Until test26 = True
                                d6 = f6.Next(100000, 999999)
                                If refs26.Contains(d6) Then
                                    test26 = False
                                Else
                                    test26 = True
                                End If
                            Loop
                            readref26.Close()
                            Dim cmdInserta As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where session = ? and student = ?", con)
                            cmdInserta.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                            cmdInserta.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                            Dim feereader As MySql.Data.MySqlClient.MySqlDataReader = cmdInserta.ExecuteReader
                            Dim mini As Double
                            Dim paid3 As Double
                            Do While feereader.Read
                                mini = mini + feereader.Item("min")
                                paid3 = paid3 + feereader.Item("paid")
                            Loop
                            feereader.Close()
                            If paid3 >= mini Then
                                Dim cmdInsert29 As New MySql.Data.MySqlClient.MySqlCommand("Update StudentSummary Set status = ? where student = ? ", con)
                                cmdInsert29.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("aveage", True))
                                cmdInsert29.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                                cmdInsert29.ExecuteNonQuery()

                               
                            End If
                            Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                            cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d6))
                            cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                            cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(init - balance, , , TriState.True)))
                            cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                            cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees paid by " & TextBox1.Text))
                            cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                            cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Credit"))
                            cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                            cmdCheck.ExecuteNonQuery()

                            Dim cmdCheck6 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                            cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d6))
                            cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Liability"))
                            cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(init - balance, , , TriState.True)))
                            cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "ADVANCE FEE PAYMENT"))
                            cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees paid in advance." & TextBox1.Text))
                            cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                            cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                            cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", TextBox1.Text))
                            cmdCheck6.ExecuteNonQuery()

                        End If
                    End If
                    If clatype <> "K.G 1 Special" Then
                        If Session("term") <> "1st term" Then
                            Dim lastterm As Integer
                            Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session Order by ID Desc", con)
                            Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                            For i As Integer = 1 To 2
                                If reader20.Read() Then
                                    If reader20.Item(0) <> Session("SessionID") Then
                                        lastterm = reader20.Item(0)
                                        i = i + 1
                                    End If
                                End If
                            Next
                            reader20.Close()
                            If lastterm = 0 Then
                                Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into SubjectReg (Session, Class, Student, SubjectsOfferred) Values (?,?,?,?)", con)

                                Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM ClassSubjects WHERE Type= ? And class = ? and subjectnest = '" & 0 & "'", con)
                                Dim type As String = "Compulsory"
                                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", TextBox1.Text))
                                cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Type", type))
                                cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", cla))
                                Dim reader1 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                                Dim subjectsID As New ArrayList
                                Dim i As Integer = 0
                                Do While reader1.Read
                                    subjectsID.Add(Val(reader1.Item("subject")))

                                Loop
                                reader1.Close()
                                For Each item As String In subjectsID
                                    Dim param As New MySql.Data.MySqlClient.MySqlParameter

                                    param = cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subjects", item))
                                    cmd.ExecuteNonQuery()
                                    cmd.Parameters.Remove(param)
                                Next
                                Dim cmd2s As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjectnest FROM ClassSubjects WHERE class = '" & cla & "' and subjectnest <> '" & "" & "'", con)
                                Dim reader1a As MySql.Data.MySqlClient.MySqlDataReader = cmd2s.ExecuteReader
                                Dim subjectsIDnest As New ArrayList
                                Dim inest As Integer = 0
                                Do While reader1a.Read
                                    If Not subjectsIDnest.Contains(Val(reader1a.Item(0))) Then
                                        subjectsIDnest.Add(Val(reader1a.Item(0)))
                                    End If
                                Loop
                                reader1a.Close()
                                For Each item As String In subjectsIDnest
                                    Dim cmdds As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into SubjectReg (Session, Class, Student, SubjectsOfferred, nested) Values (?,?,?,?,?)", con)
                                    cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                                    cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                                    cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", TextBox1.Text))
                                    cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Stdddudent", item))
                                    cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Sent", -Val(True)))
                                    cmdds.ExecuteNonQuery()

                                Next
                            Else
                                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT Subjectsofferred, nested FROM SubjectReg WHERE SubjectReg.Student = ? And SubjectReg.Session = ? And SubjectReg.Class = ?", con)
                                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", TextBox1.Text))
                                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Session", lastterm))
                                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Class", cla))
                                Dim reader21 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
                                Dim subjectsofferred As New ArrayList
                                Dim nested As New ArrayList
                                Do While reader21.Read
                                    subjectsofferred.Add(reader21.Item(0))
                                    nested.Add(reader21(1))
                                Loop
                                reader21.Close()
                                If subjectsofferred.Count = 0 Then
                                    Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into SubjectReg (Session, Class, Student, SubjectsOfferred) Values (?,?,?,?)", con)

                                    Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM ClassSubjects WHERE Type= ? And class = ? and subjectnest = '" & 0 & "' order by subject", con)
                                    Dim type As String = "Compulsory"
                                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", TextBox1.Text))

                                    cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Type", type))
                                    cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", cla))
                                    Dim reader1 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                                    Dim subjectsID As New ArrayList
                                    Dim i As Integer = 0
                                    Do While reader1.Read
                                        subjectsID.Add(Val(reader1.Item("subject")))

                                    Loop
                                    reader1.Close()
                                    For Each item As String In subjectsID
                                        Dim param As New MySql.Data.MySqlClient.MySqlParameter

                                        param = cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subjects", item))


                                        cmd.ExecuteNonQuery()
                                        cmd.Parameters.Remove(param)

                                        Dim paramcx As New MySql.Data.MySqlClient.MySqlParameter

                                    Next
                                    Dim cmd2s As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjectnest FROM ClassSubjects WHERE class = '" & cla & "' and subjectnest <> '" & "" & "'", con)
                                    Dim reader1a As MySql.Data.MySqlClient.MySqlDataReader = cmd2s.ExecuteReader
                                    Dim subjectsIDnest As New ArrayList
                                    Dim inest As Integer = 0
                                    Do While reader1a.Read
                                        If Not subjectsIDnest.Contains(Val(reader1a.Item(0))) Then
                                            subjectsIDnest.Add(Val(reader1a.Item(0)))
                                        End If
                                    Loop
                                    reader1a.Close()
                                    For Each item As String In subjectsIDnest
                                        Dim cmdds As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into SubjectReg (Session, Class, Student, SubjectsOfferred, nested) Values (?,?,?,?,?)", con)
                                        cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                                        cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                                        cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", TextBox1.Text))
                                        cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Stdddudent", item))
                                        cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Sent", -Val(True)))
                                        cmdds.ExecuteNonQuery()

                                       
                                    Next

                                Else
                                    Dim nestno As Integer
                                    For Each item As Integer In subjectsofferred
                                        Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into SubjectReg (Session, Class, Student, SubjectsOfferred, nested) Values (?,?,?,?,?)", con)
                                        cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                                        cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                                        cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", TextBox1.Text))
                                        cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subjects", item))
                                        cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("bjects", nested(nestno)))
                                        cmd.ExecuteNonQuery()

                                       
                                        nestno = nestno + 1
                                    Next
                                End If
                            End If
                        Else
                            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into SubjectReg (Session, Class, Student, SubjectsOfferred) Values (?,?,?,?)", con)

                            Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM ClassSubjects WHERE Type= ? And class = ? and subjectnest = '" & 0 & "' order by subject", con)
                            Dim type As String = "Compulsory"
                            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", TextBox1.Text))

                            cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Type", type))
                            cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", cla))
                            Dim reader1 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                            Dim subjectsID As New ArrayList
                            Dim i As Integer = 0
                            Do While reader1.Read
                                subjectsID.Add(Val(reader1.Item("subject")))

                            Loop
                            reader1.Close()
                            For Each item As String In subjectsID
                                Dim param As New MySql.Data.MySqlClient.MySqlParameter

                                param = cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subjects", item))


                                cmd.ExecuteNonQuery()
                                cmd.Parameters.Remove(param)


                            Next
                            Dim cmd2s As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjectnest FROM ClassSubjects WHERE class = '" & cla & "' and subjectnest <> '" & "" & "'", con)
                            Dim reader1a As MySql.Data.MySqlClient.MySqlDataReader = cmd2s.ExecuteReader
                            Dim subjectsIDnest As New ArrayList
                            Dim inest As Integer = 0
                            Do While reader1a.Read
                                If Not subjectsIDnest.Contains(Val(reader1a.Item(0))) Then
                                    subjectsIDnest.Add(Val(reader1a.Item(0)))
                                End If
                            Loop
                            reader1a.Close()
                            For Each item As String In subjectsIDnest
                                Dim cmdds As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into SubjectReg (Session, Class, Student, SubjectsOfferred, nested) Values (?,?,?,?,?)", con)
                                cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                                cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                                cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", TextBox1.Text))
                                cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Stdddudent", item))
                                cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Sent", -Val(True)))
                                cmdds.ExecuteNonQuery()

                               
                            Next

                        End If

                    Else
                        Dim cmdCheck20 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id, subject FROM kcourseoutline where session = '" & Session("sessionid") & "' and class = '" & cla & "'", con)
                        Dim reader202 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck20.ExecuteReader()
                        Dim topics As New ArrayList
                        Dim subjectss As New ArrayList
                        Do While reader202.Read()
                            topics.Add(reader202(0))
                            subjectss.Add(reader202(1))
                        Loop
                        reader202.Close()
                        Dim llt As Integer
                        For Each topic As Integer In topics
                            Dim cmdceck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into kscoresheet (session, class, subject, topic, student) Values (?,?,?,?,?)", con)
                            cmdceck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", Session("sessionid")))
                            cmdceck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amount", cla))
                            cmdceck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("driver", subjectss(llt)))
                            cmdceck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("busno", topic))
                            cmdceck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("bu", TextBox1.Text))
                            cmdceck2.ExecuteNonQuery()
                            llt = llt + 1
                        Next
                    End If
                End If
                Average_Age()
                logify.log(Session("staffid"), par.getstuname(Session("studentadd")) & " was placed in " & DropDownList1.Text)
                logify.Notifications("You have been placed in " & DropDownList1.Text, Session("studentadd"), Session("staffid"), "Admin", "")
                If par.getparent(Session("studentadd")) <> "" Then logify.Notifications(par.getstuname(Session("Studentadd")) & " has been placed in " & DropDownList1.Text, Session("studentadd"), Session("staffid"), "Admin", "")

                Show_Alert(True, "Student successfully registerred in class")
                Dim ds As New DataTable
                ds.Rows.Clear()
                ds.Columns.Add("passport")
                ds.Columns.Add("staffname")
                Dim cmdLoad0s As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.passport, studentsprofile.admno, studentsprofile.surname from studentsummary inner join studentsprofile on studentsummary.student = studentsprofile.admno inner join (class inner join classteacher on classteacher.class = class.id) on studentsummary.class = class.id WHERE class.class = '" & DropDownList1.Text & "'", con)
                Dim student0s As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0s.ExecuteReader
                Do While student0s.Read
                    ds.Rows.Add(student0s.Item(0).ToString, student0s.Item(1) & " - " & student0s.Item(2).ToString)
                Loop
                student0s.Close()
                gridview1.DataSource = Nothing
                gridview1.DataSource = ds
                gridview1.DataBind()

                con.Close()            End Using
            add_traits()
            Session("StudentAdd") = TextBox1.Text
            Student_Details()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try


    End Sub
    Private Sub add_traits()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.Open()
            Dim cmdLoad03 As New MySql.Data.MySqlClient.MySqlCommand("SELECT traits.id from traits inner join class on class.traitgroup = traits.traitgroup where traits.used = '" & 1 & "' and class.class = '" & DropDownList1.Text & "'", con)
            Dim student03 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad03.ExecuteReader
            Dim traits As New ArrayList
            Do While student03.Read()
                traits.Add(student03(0).ToString)
            Loop
            student03.Close()
            Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("delete from termtraits where student = '" & TextBox1.Text & "' and session = '" & Session("sessionid") & "'", con)
            cmdInsert22.ExecuteNonQuery()
            For Each trait As String In traits
                Dim cmdInsert22a As New MySql.Data.MySqlClient.MySqlCommand("insert into termtraits (session, student, trait) values ('" & Session("sessionid") & "', '" & TextBox1.Text & "', '" & trait & "')", con)
                cmdInsert22a.ExecuteNonQuery()
            Next

            con.Close()        End Using
    End Sub
    Protected Sub Average_Age()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.Open()

            Dim cmdLoad10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id, type from class where class = ?", con)
            cmdLoad10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", DropDownList1.Text))
            Dim student04 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10.ExecuteReader
            student04.Read()
            Dim cla As Integer = student04.Item(0)
            Dim clatype As String = student04(1).ToString
            student04.Close()

            Dim cmdSelect2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT age FROM StudentSummary WHERE class = ? and session = '" & Session("sessionid") & "'", con)
            cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", cla))
            Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2.ExecuteReader
            Dim average As Integer
            Dim total As Integer
            Dim count As Integer
            Do While reader2.Read()
                total = total + reader2.Item("age")
                count = count + 1
            Loop
            Try
                average = total \ count
            Catch ex As Exception

            End Try

            reader2.Close()

            Dim cmdSelect2a As New MySql.Data.MySqlClient.MySqlCommand("SELECT smsno FROM Session WHERE id = ?", con)
            cmdSelect2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionId")))
            Dim reader2a As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2a.ExecuteReader
            reader2a.Read()
            Dim smsno As Integer = reader2a(0)
            reader2a.Close()
            Dim cmdInsert2a As New MySql.Data.MySqlClient.MySqlCommand("Update session set smsno = ? where id = ?", con)
            cmdInsert2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("aveage", smsno + 35))
            cmdInsert2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("No", Session("Sessionid")))
            cmdInsert2a.ExecuteNonQuery()

            Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update StudentSummary Set Aveage = ?, ClassNo = ? where class = ?", con)
            cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("aveage", average))
            cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("No", count))
            cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", cla))
            cmdInsert2.ExecuteNonQuery()

          
            If clatype <> "K.G 1 Special" Then update_class(cla)
            con.Close()
        End Using
    End Sub




    Protected Sub lnkclrpt_Click(sender As Object, e As EventArgs) Handles lnkclrpt.Click
        Session("classselect") = DropDownList1.Text
        Session("schoolselect") = Nothing
        Response.Redirect("~/content/Admin/classlist.aspx")
    End Sub

    Protected Sub lnkSchRpt_Click(sender As Object, e As EventArgs) Handles lnkschrpt.Click
        Session("schoolselect") = Request.QueryString("school")
        Response.Redirect("~/content/Admin/classlist.aspx")
    End Sub



    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        pnlAll.Visible = True
        panel3.Visible = False
    End Sub


    Protected Sub lnkAdd_Click(sender As Object, e As EventArgs) Handles lnkAdd.Click
        pnlAdd.Visible = True
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim ds As New DataTable
                ds.Columns.Add("passport")
                ds.Columns.Add("staffname")
                Dim s As String = "%" & txtSearch.Text & "%"
                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.passport, studentsprofile.admno, studentsprofile.surname from studentsummary inner join studentsprofile on studentsummary.student = studentsprofile.admno inner join class  on studentsummary.class = class.id WHERE class.class = '" & DropDownList1.Text & "' and studentsummary.session = '" & Session("sessionid") & "' and studentsprofile.surname like '" & s & "' or studentsprofile.admno = '" & txtSearch.Text & "'", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                Do While student0.Read
                    ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                Loop
                student0.Close()
                gridview1.DataSource = ds
                gridview1.DataBind()
                con.Close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
End Class
