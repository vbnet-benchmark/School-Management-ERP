Imports System.IO
Partial Class Admin_addteacher



    Inherits System.Web.UI.Page
    

    Dim subselect As Integer
    Dim subremove As String
    Dim classremove As String
    Dim staffID As String

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
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim cmdloadfs As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.surname, class.class,  studentsummary.class, class.type From Studentsummary INNER JOIN Studentsprofile on studentsummary.student = studentsprofile.admno inner join class on studentsummary.class = class.Id WHERE StudentSummary.student = ? And StudentSummary.Session = ?", con)
            cmdloadfs.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class.ID", Session("StudentAdd")))
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
                cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
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
            cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", Session("StudentAdd")))
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
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
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
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
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
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
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
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
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
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
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
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
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
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()

            Dim cmdInsertv As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where student = ? order by id", con)
            cmdInsertv.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
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
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
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
                cmdInsertx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
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
                cmdInsert225.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                cmdInsert225.ExecuteNonQuery()

                Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("select ref from transactions where account like ? and student = ? and session = ?", con)
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", item & "%"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
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
                cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("Studentadd")))
                Dim feereader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert220.ExecuteReader
                Dim totals As Integer = 0

                Do While feereader2.Read
                    totals = totals + feereader2.Item("amount")
                Loop
                feereader2.Close()
                If totals = 0 Then
                    Dim cmdInsertcv As New MySql.Data.MySqlClient.MySqlCommand("Update StudentSummary Set status = ? where student = ? and session = '" & currentsession & "'", con)
                    cmdInsertcv.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("aveage", True))
                    cmdInsertcv.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("Studentadd")))
                    cmdInsertcv.ExecuteNonQuery()
                End If
                ct = ct + 1
            Next
            con.Close()
        End Using
    End Sub


    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles bnUpdate.Click

        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from StudentsProfile where admno = ?", con)
                cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("studentadd")))
                Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                student.Read()
                Dim dob As TimeSpan = Now.Subtract(DateTime.ParseExact(student.Item("dateofbirth"), "dd/MM/yyyy", Nothing))
                Dim age As Integer = dob.TotalDays \ 365
                student.Close()
                Dim cmdSelect2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT Session, Class, student, age FROM StudentSummary WHERE student = ? And Session = ?", con)
                cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2.ExecuteReader
                Dim cla As Integer
                If reader2.Read() Then cla = reader2.Item(1).ToString()
                reader2.Close()

                Dim cmdSelect2c As New MySql.Data.MySqlClient.MySqlCommand("Update studentsprofile set activated = '" & 1 & "' where admno = '" & Session("studentadd") & "'", con)
                cmdSelect2c.ExecuteNonQuery()

                Dim cmdSelect2a As New MySql.Data.MySqlClient.MySqlCommand("Delete FROM StudentSummary WHERE student = ? And Session = ?", con)
                cmdSelect2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                cmdSelect2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                cmdSelect2a.ExecuteNonQuery()
                Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("Delete from SubjectReg WHERE student = ? And Session = ?", con)
                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                cmd.ExecuteNonQuery()
               
                If cla <> 0 Then
                    Dim cmdSelect2ax As New MySql.Data.MySqlClient.MySqlCommand("SELECT smsno FROM Session WHERE id = ?", con)
                    cmdSelect2ax.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionId")))
                    Dim reader2av As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2ax.ExecuteReader
                    reader2av.Read()
                    Dim smsnos As Integer = reader2av(0)
                    reader2av.Close()
                    Dim cmdInsert2ac As New MySql.Data.MySqlClient.MySqlCommand("Update session set smsno = ? where id = ?", con)
                    cmdInsert2ac.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("aveage", smsnos - 35))
                    cmdInsert2ac.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("No", Session("Sessionid")))
                    cmdInsert2ac.ExecuteNonQuery()


                    Average_Age_previous(cla)
                End If

                Dim cmdSelect2s As New MySql.Data.MySqlClient.MySqlCommand("SELECT id from class where class = '" & cboclass.Text & "'", con)
                Dim reader2s As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2s.ExecuteReader
                reader2s.Read()
                Dim newclass As Integer = reader2s.Item(0)
                reader2s.Close()

                Dim cmdSelect2sa As New MySql.Data.MySqlClient.MySqlCommand("SELECT paid from feeschedule where student = '" & Session("studentadd") & "' and session = '" & Session("sessionid") & "'", con)
                Dim reader2sa As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2sa.ExecuteReader

                Dim paid As Integer
                Do While reader2sa.Read

                    paid = paid + Val(reader2sa.Item(0))
                Loop
                reader2sa.Close()

                Dim cmdSelect2q As New MySql.Data.MySqlClient.MySqlCommand("Delete FROM feeschedule WHERE student = ? And Session = ?", con)
                cmdSelect2q.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                cmdSelect2q.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                cmdSelect2q.ExecuteNonQuery()
                Dim cmdLoadd As New MySql.Data.MySqlClient.MySqlCommand("SELECT ref from transactions where account = '" & "ADVANCE FEE PAYMENT" & "' and student = '" & Session("studentadd") & "' order by session desc", con)
                Dim reads As MySql.Data.MySqlClient.MySqlDataReader = cmdLoadd.ExecuteReader
                Dim ref As Integer
                If reads.Read() Then ref = reads.Item(0)
                reads.Close()
                Dim cmdSelec As New MySql.Data.MySqlClient.MySqlCommand("Delete FROM transactions WHERE student = ? And Session = ? and ref <> '" & ref & "'", con)
                cmdSelec.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                cmdSelec.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                cmdSelec.ExecuteNonQuery()
                con.Close()
                add_traits()
                Re_register(paid, age)
                payment_schedules()
            End Using

           

            logify.log(Session("staffid"), par.getstuname(Session("studentadd")) & " was placed in " & cboclass.Text)
            logify.Notifications("You have been placed in " & cboclass.Text, Session("studentadd"), Session("staffid"), "Admin", "~/content/student/classdetails.aspx", "")
            If par.getparent(Session("studentadd")) <> "" Then logify.Notifications(par.getstuname(Session("Studentadd")) & " has been placed in " & cboclass.Text, par.getparent(Session("studentadd")), Session("staffid"), "Admin", "~/content/parent/classdetails.aspx?" & Session("studentadd"), "")
            If Session("senders") <> Nothing Then
                Dim x As String = Session("senders")
                Session("senders") = Nothing
                Response.Redirect(x)
            Else
                Response.Redirect("~/content/admin/studentprofile.aspx?" & Request.QueryString.ToString)

            End If

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Private Sub add_traits()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.open()
            Dim cmdLoad03 As New MySql.Data.MySqlClient.MySqlCommand("SELECT traits.id from traits inner join class on class.traitgroup = traits.traitgroup where traits.used = '" & 1 & "' and class.class = '" & cboclass.Text & "'", con)
            Dim student03 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad03.ExecuteReader
            Dim traits As New ArrayList
            Do While student03.Read()
                traits.Add(student03(0).ToString)
            Loop
            student03.Close()
            Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("delete from termtraits where student = '" & Session("studentadd") & "' and session = '" & Session("sessionid") & "'", con)
            cmdInsert22.ExecuteNonQuery()
            For Each trait As String In traits
                Dim cmdInsert22a As New MySql.Data.MySqlClient.MySqlCommand("insert into termtraits (session, student, trait) values ('" & Session("sessionid") & "', '" & Session("studentadd") & "', '" & trait & "')", con)
                cmdInsert22a.ExecuteNonQuery()
            Next

            con.close()
        End Using

    End Sub
    Protected Sub Re_register(tpaid As Integer, age As String)
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.open()
            Dim cmdLoad03 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id, type from class where class = '" & cboclass.Text & "'", con)
            Dim student03 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad03.ExecuteReader
            student03.Read()
            Dim cla As String = student03.Item(0).ToString
            Dim clatype As String = student03(1).ToString
            student03.Close()


            Dim total As Double
            Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from classfees where class = ?", con)
            cmdLoad0.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", cla))
            Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
            Dim classfee As New ArrayList

            Dim classamount As New ArrayList
            Dim min As New ArrayList
            Dim monthly As New ArrayList
            Dim quarterly As New ArrayList
            Dim classi As Integer
            Do While student0.Read
                Dim z As Integer
                classfee.Add(student0.Item(2))
                classamount.Add(student0.Item(3))
                min.Add(student0.Item("amount") * (student0.Item("min") / 100))
                monthly.Add(student0.Item(5))
                quarterly.Add(student0(6))
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
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("compulsory", 1))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", min(classi)))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("mon", monthly(classi)))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("mozdfn", quarterly(classi)))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("mozdsfn", classamount(classi)))
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
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
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
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
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
                Dim test21 As Boolean = False
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
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
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
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
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
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
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

            Dim cmdload3xcz As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from classonefees where class = '" & cla & "'", con)
            Dim student3xcz As MySql.Data.MySqlClient.MySqlDataReader = cmdload3xcz.ExecuteReader

            Do While student3xcz.Read

                admfee.Add(student3xcz.Item("fee"))
                admamount.Add(student3xcz.Item("amount"))
                admin.Add(student3xcz.Item("amount") * (student3xcz.Item("min") / 100))
            Loop
            student3xcz.Close()


            Dim hostel As String
            Dim transport As String
            Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT admfees, hostelstay, transport from StudentsProfile where admno = ?", con)
            cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("studentadd")))

            Dim studentadm As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
            studentadm.Read()
            hostel = studentadm.Item(1)
            transport = studentadm.Item(2)
            Dim paid As String = studentadm.Item(0)
            studentadm.Close()
            If paid = "Not Paid" Then
                For Each item As String In admfee
                    Dim test22 As Boolean = False
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
                    total = total + admamount(admi)

                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d2))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(admamount(admi), , , TriState.True)))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", item & " DEBTS"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
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
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
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

                Dim cmdload3xcz1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from classsessionfees where class = '" & cla & "'", con)
                Dim student3xcz1 As MySql.Data.MySqlClient.MySqlDataReader = cmdload3xcz1.ExecuteReader

                Do While student3xcz1.Read

                    sesfee.Add(student3xcz1.Item("fee"))
                    sesamount.Add(student3xcz1.Item("amount"))
                    sesmin.Add(student3xcz1.Item("amount") * (student3xcz1.Item("min") / 100))
                Loop
                student3xcz1.Close()

                For Each item As String In sesfee
                    Dim test23 As Boolean = False
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
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
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
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
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
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))

                    cmdCheck4.ExecuteNonQuery()
                    sesi = sesi + 1
                Next
            End If
            If hostel = True Then

                Dim test24 As Boolean = False
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
                total = total + sesamount

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
                Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
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

            End If
            If transport <> "" Then
                Dim test25 As Boolean = False
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
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
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
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
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
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))

                cmdCheck4.ExecuteNonQuery()
            End If
            add_discount()



            Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO StudentSummary (Session, Class, student, age) Values (?,?,?,?)", con)
            cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
            cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
            cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
            cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("age", age))

            cmdInsert2.ExecuteNonQuery()

           
            If clatype <> "K.G 1 Special" Then
                Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into SubjectReg (Session, Class, Student, SubjectsOfferred) Values (?,?,?,?)", con)

                Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM ClassSubjects WHERE Type= ? And class = ? and subjectnest = '" & 0 & "'", con)
                Dim type As String = "Compulsory"
                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", Session("studentadd")))
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
                    Dim paramc As New MySql.Data.MySqlClient.MySqlParameter

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
                    cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", Session("studentadd")))
                    cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Stdddudent", item))
                    cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Sent", -Val(True)))
                    cmdds.ExecuteNonQuery()

                Next
            Else
                Dim cmdCheck20 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id, subject FROM kcourseoutline where session = '" & Session("sessionid") & "' and class = '" & cla & "'", con)
                Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck20.ExecuteReader()
                Dim topics As New ArrayList
                Dim subjectss As New ArrayList
                Do While reader20.Read()
                    topics.Add(reader20(0))
                    subjectss.Add(reader20(1))
                Loop
                reader20.Close()
                Dim llt As Integer
                For Each topic As Integer In topics
                    Dim cmdceck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into kscoresheet (session, class, subject, topic, student) Values (?,?,?,?,?)", con)
                    cmdceck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", Session("sessionid")))
                    cmdceck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amount", cla))
                    cmdceck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("driver", subjectss(llt)))
                    cmdceck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("busno", topic))
                    cmdceck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("bu", Session("StudentAdd")))
                    cmdceck2.ExecuteNonQuery()
                    llt = llt + 1
                Next
            End If



            Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dr, cr from transactions where account = ? and student = ? order by date desc", con)
            cmdLoad11.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("acc", "ADVANCE FEE PAYMENT"))
            cmdLoad11.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
            Dim balreader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
            Dim dr As Integer
            Dim cr As Integer
            Dim balance As Double = 0
            Do While balreader.Read
                dr = dr + Val(balreader.Item("dr").replace(",", ""))
                cr = cr + Val(balreader.Item("cr").replace(",", ""))
            Loop
            Dim fbalance As Double = cr - dr
            balance = (cr - dr) + tpaid
            balreader.Close()

            cr = Nothing
            dr = Nothing
            Dim init As Double = balance
            If balance > 0 Then
                Dim cmdInsert220 As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where session = ? and student = ? ", con)
                cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
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
                cmdInsert4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))

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
                    Dim test2 As Boolean = False
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
                    If Session("amount" & k) <> Nothing Then
                        Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("Update feeschedule Set paid = ? where fee = ? and student = ? and session = ?", con)
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", Session("amount" & k)))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", Session("fee" & k)))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("ses" & k)))
                        cmdInsert22.ExecuteNonQuery()

                        Dim cmdCheck03 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                        cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d))
                        cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                        cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(Session("amount" & k) - Session("paid" & k), , , TriState.True)))
                        cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", Session("fee" & k) & " DEBTS"))
                        cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees paid"))
                        cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                        cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                        cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                        cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))
                        cmdCheck03.ExecuteNonQuery()

                        Dim cmdCheck02 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                        cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d))
                        cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                        cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(Session("amount" & k) - Session("paid" & k), , , TriState.True)))
                        cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", Session("fee" & k) & " PAID"))
                        cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees paid"))
                        cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                        cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                        cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                        cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))
                        cmdCheck02.ExecuteNonQuery()
                        Session("fee" & k) = Nothing
                        Session("amount" & k) = Nothing
                        Session("paid" & k) = Nothing
                        d = Nothing
                    End If
                Next

                Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where session = ? and student = ?", con)
                cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                Dim feereader As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert220.ExecuteReader
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
                    cmdInsert29.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdInsert29.ExecuteNonQuery()

                  
                End If
                Dim test23 As Boolean = False
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
                Dim diff As Double = fbalance - balance
                Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d3))
                cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(init - balance, , , TriState.True)))
                cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees paid by " & Session("studentadd")))
                cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Credit"))
                cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                cmdCheck.ExecuteNonQuery()

                Dim cmdCheck6 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d3))
                cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Cash"))
                cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(init - balance, , , TriState.True)))
                cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "BANK ACCOUNT"))
                cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees paid by student." & Session("studentadd")))
                cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                cmdCheck6.ExecuteNonQuery()
                If diff > 0 Then
                    Dim test24 As Boolean = False
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
                    Dim advance As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                    advance.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                    advance.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Lability"))
                    advance.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(diff, , , TriState.True)))
                    advance.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "ADVANCE FEE PAYMENT"))
                    advance.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees paid by " & Session("studentadd")))
                    advance.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    advance.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                    advance.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    advance.ExecuteNonQuery()

                    Dim advance6 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                    advance6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                    advance6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                    advance6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(diff, , , TriState.True)))
                    advance6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                    advance6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees paid by student." & Session("studentadd")))
                    advance6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    advance6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Credit"))
                    advance6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    advance6.ExecuteNonQuery()
                ElseIf diff < 0 Then
                    Dim testr3r3 As Boolean
                    Dim fr3 As New Random
                    Dim refr3r3 As New MySql.Data.MySqlClient.MySqlCommand("Select ref from transactions", con)
                    Dim readrefr3r3 As MySql.Data.MySqlClient.MySqlDataReader = refr3r3.ExecuteReader

                    Dim refsr3r3 As New ArrayList
                    Do While readrefr3r3.Read
                        refsr3r3.Add(readrefr3r3.Item(0))
                    Loop
                    Dim dr3 As Integer
                    Do Until testr3r3 = True
                        dr3 = fr3.Next(100000, 999999)
                        If refsr3r3.Contains(dr3) Then
                            testr3r3 = False
                        Else
                            testr3r3 = True
                        End If
                    Loop
                    readrefr3r3.Close()
                    Dim cmdccheck6 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                    cmdccheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", dr3))
                    cmdccheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Liability"))
                    cmdccheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(0 - diff, , , TriState.True)))
                    cmdccheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "ADVANCE FEE PAYMENT"))
                    cmdccheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees paid in advance." & Request.QueryString.ToString))
                    cmdccheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdccheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Credit"))
                    cmdccheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdccheck6.ExecuteNonQuery()

                    Dim cmdCheck5 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", dr3))
                    cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Cash"))
                    cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(0 - diff, , , TriState.True)))
                    cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "BANK ACCOUNT"))
                    cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees paid by " & Request.QueryString.ToString))
                    cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                    cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))
                    cmdCheck5.ExecuteNonQuery()

                End If
            End If

            Average_Age()
            con.close()
        End Using




    End Sub

    Protected Sub Average_Age()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim cmdLoad03 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id, type from class where class = '" & cboclass.Text & "'", con)
            Dim student03 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad03.ExecuteReader
            student03.Read()
            Dim cla As String = student03.Item(0).ToString
            Dim clatype As String = student03(1).ToString
            student03.Close()

            Dim cmdSelect2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT age FROM StudentSummary WHERE class = ? and session = '" & session("sessionid") & "'", con)
            cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", cla))
            Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2.ExecuteReader
            Dim average As Integer
            Dim total As Integer
            Dim count As Integer
            Do While reader2.Read()
                total = total + reader2.Item("age")
                count = count + 1
            Loop
            If Not count = 0 Then
                average = total \ count
            End If
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

            Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update StudentSummary Set Aveage = ?, ClassNo = ? where class = ? and session = '" & session("sessionid") & "'", con)
            cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("aveage", average))
            cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("No", count))
            cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", cla))
            cmdInsert2.ExecuteNonQuery()

          
            
            con.Close()
        End Using
    End Sub

    Private Sub update_class(cla As Integer)

       Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
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

                        Dim param As MySql.Data.MySqlClient.MySqlParameter = cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", studentID))
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

                        Dim cmd22 As New MySql.Data.MySqlClient.MySqlCommand("SELECT grades.lowest From grades inner join (gradingsystem inner join class on class.gradesystem = gradingsystem.id) on grades.system = gradingsystem.id Where class.id = '" & cla & "' order by grades.lowest desc", con)
                        Dim graderead As MySql.Data.MySqlClient.MySqlDataReader = cmd22.ExecuteReader
                        Dim remarks As String = ""
                        Do While graderead.Read
                            If average >= Val(graderead.Item(0)) Then

                                Exit Do
                            End If
                        Loop
                        graderead.Close()

                        Dim cmd2c As New MySql.Data.MySqlClient.MySqlCommand("UPDATE StudentSummary SET Average = ?, principalremarks = ? WHERE student = ? And Session = ? And Class = ?", con)
                        Dim param2 As MySql.Data.MySqlClient.MySqlParameter = cmd2c.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Average", average))
                        Dim paramq As MySql.Data.MySqlClient.MySqlParameter = cmd2c.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Ave", remarks))
                        Dim param3 As MySql.Data.MySqlClient.MySqlParameter = cmd2c.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", studentID))
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


    Protected Sub Average_Age_previous(cla As Integer)
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim cmdSelect2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT age FROM StudentSummary WHERE class = ?", con)
            cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", cla))
            Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2.ExecuteReader
            Dim average As Integer
            Dim total As Integer
            Dim count As Integer
            Do While reader2.Read()
                total = total + reader2.Item("age")
                count = count + 1
            Loop
            If Not count = 0 Then
                average = total \ count
            End If
            reader2.Close()

            Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update StudentSummary Set Aveage = ?, ClassNo = ? where class = ?", con)
            cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("aveage", average))
            cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("No", count))
            cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", cla))
            cmdInsert2.ExecuteNonQuery()

           
           
            con.Close()
        End Using
    End Sub


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If check.Check_Admin(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.open()
                If Not IsPostBack Then

                    Dim comfirm3 As New MySql.Data.MySqlClient.MySqlCommand("Select class from class", con)
                    Dim rcomfirm3 As MySql.Data.MySqlClient.MySqlDataReader = comfirm3.ExecuteReader
                    cboClass.Items.Clear()
                    cboClass.Items.Add("Select new Class")
                    Do While rcomfirm3.Read
                        cboClass.Items.Add(rcomfirm3.Item(0))
                    Loop
                    rcomfirm3.Close()
                End If

                con.close()
            End Using

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub






    Protected Sub bnBack_Click(sender As Object, e As EventArgs) Handles bnBack.Click
        If Session("senders") <> Nothing Then
            Dim x As String = Session("senders")
            Session("senders") = Nothing
            Response.Redirect(x)
        Else
            Response.Redirect("~/content/admin/studentprofile.aspx?" & Request.QueryString.ToString)

        End If
    End Sub
End Class
