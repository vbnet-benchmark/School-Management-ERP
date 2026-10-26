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
    Protected Sub radType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles radType.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                Dim currentsession As String
                Dim currentterm As String
                Dim quarter As String
                If Now.Month <= 3 Then
                    quarter = "1st Quarter"
                ElseIf Now.Month > 3 And Now.Month < 7 Then
                    quarter = "2nd Quarter"
                ElseIf Now.Month >= 7 And Now.Month < 10 Then
                    quarter = "3rd Quarter"
                ElseIf Now.Month >= 10 Then
                    quarter = "4th Quarter"
                End If
                If radType.SelectedValue = "Quarterly" Then
                    Dim cmdInsertf As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session where term = '" & IIf(quarter = "1st Quarter" Or quarter = "2nd Quarter", "2nd term", IIf(quarter = "3rd Quarter", "3rd term", "1st term")) & "' Order by ID Desc", con)
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
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentSession))
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

                If radType.SelectedValue = "Monthly" Then
                    Dim x As Integer
                    For Each s As Integer In id
                        If Val(monthly(x)) <> 0 Then
                            Dim cmdInsertf5 As New MySql.Data.MySqlClient.MySqlCommand("update feeschedule set amount = '" & Val(monthly(x)) * 4 & "' where id = '" & s & "'", con)
                            cmdInsertf5.ExecuteNonQuery()

                            Dim cmdCheck44 As New MySql.Data.MySqlClient.MySqlCommand("select ref from transactions where account = ? and student = ? and session = ?", con)
                            cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", fee(x) & " DEBTS"))
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

                            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(IIf(amount(x) <> 0, Val(monthly(x)) * 4, 0), , , TriState.True)))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", fee(x) & " DEBTS"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentSession))
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
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentSession))

                            cmdCheck4.ExecuteNonQuery()

                        End If
                        x += 1
                    Next
                    Session("scheduletype") = "Monthly"
                ElseIf radType.SelectedValue = "Termly" Then
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
                    Session("scheduletype") = "termly"

                ElseIf radType.SelectedValue = "Quarterly" Then
                    Dim x As Integer
                    For Each s As Integer In id
                        If Val(quarterly(x)) <> 0 Then

                            Dim cmdInsertf5 As New MySql.Data.MySqlClient.MySqlCommand("update feeschedule set amount = '" & IIf(amount(x) <> 0 And currentterm = "2nd term", Val(quarterly(x)) * 2, IIf(amount(x) <> 0 And currentterm <> "2nd term", Val(quarterly(x)), IIf(Val(termly(x)) = 0 And currentterm = "2nd term", Val(quarterly(x)) * 2, IIf(Val(termly(x)) = 0 And currentterm <> "2nd term", Val(quarterly(x)), 0)))) & "' where id = '" & s & "'", con)
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
                    Session("scheduletype") = "quarterly"



                End If

                add_discount()
                Student_Details()
                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Sub reverse_all()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim cmdSelect2sa As New MySql.Data.MySqlClient.MySqlCommand("SELECT student, class from studentsummary where session = '" & Session("sessionid") & "'", con)
            Dim reader2sa As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2sa.ExecuteReader

            Dim students As New ArrayList
            Dim cla As New ArrayList
            Do While reader2sa.Read
                If Not students.Contains(reader2sa("student")) Then
                    cla.Add(reader2sa(1))
                    students.Add(reader2sa("student"))
                End If


            Loop
            reader2sa.Close()
            Dim clano As Integer = 0
            For Each item As String In students
                Session("studentadd") = item
                Dim cmdSelect2q As New MySql.Data.MySqlClient.MySqlCommand("Delete FROM feeschedule WHERE student = ? And Session = ?", con)
                cmdSelect2q.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                cmdSelect2q.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                cmdSelect2q.ExecuteNonQuery()
                Dim cmdLoadd As New MySql.Data.MySqlClient.MySqlCommand("SELECT ref from transactions where account = '" & "ADVANCE FEE PAYMENT" & "' and student = '" & Session("studentadd") & "' order by session desc", con)
                Dim reads As MySql.Data.MySqlClient.MySqlDataReader = cmdLoadd.ExecuteReader
                Dim ref As New ArrayList

                Do While reads.Read()
                    ref.Add(reads.Item(0))

                Loop
                reads.Close()
                Dim cmdSelec As New MySql.Data.MySqlClient.MySqlCommand("Delete FROM transactions WHERE student = ? And Session = ?", con)
                cmdSelec.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                cmdSelec.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                cmdSelec.ExecuteNonQuery()

                Dim cmdSelecd As New MySql.Data.MySqlClient.MySqlCommand("update studentsummary set status = '" & 0 & "' WHERE student = ? and session = ?", con)
                cmdSelecd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                cmdSelecd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                cmdSelecd.ExecuteNonQuery()
                For Each r As String In ref



                    Dim cmdSelecx As New MySql.Data.MySqlClient.MySqlCommand("Delete FROM transactions WHERE  ref = '" & r & "'", con)
                    cmdSelecx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdSelecx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                    cmdSelecx.ExecuteNonQuery()



                Next

                Re_register_reverse(cla(clano))
                clano = clano + 1
            Next
            con.Close()
        End Using

    End Sub
    Sub reverse()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim cmdSelect2sa As New MySql.Data.MySqlClient.MySqlCommand("SELECT feeschedule.paid, studentsummary.class from studentsummary left join feeschedule on studentsummary.student = feeschedule.student and feeschedule.session = studentsummary.session where studentsummary.student = '" & Session("studentadd") & "' and studentsummary.session = '" & Session("sessionid") & "'", con)
            Dim reader2sa As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2sa.ExecuteReader

            Dim paid As Integer = 0
            Dim cla As Integer
            Do While reader2sa.Read
                cla = reader2sa(1)
                paid = paid + Val(reader2sa.Item(0).ToString)
            Loop
            reader2sa.Close()

            Dim cmdSelect2q As New MySql.Data.MySqlClient.MySqlCommand("Delete FROM feeschedule WHERE student = ? And Session = ?", con)
            cmdSelect2q.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
            cmdSelect2q.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
            cmdSelect2q.ExecuteNonQuery()
            
            Dim cmdLoadd As New MySql.Data.MySqlClient.MySqlCommand("SELECT ref from transactions where account = '" & "ADVANCE FEE PAYMENT" & "' and student = '" & Session("studentadd") & "' order by session desc", con)
            Dim reads As MySql.Data.MySqlClient.MySqlDataReader = cmdLoadd.ExecuteReader
            Dim ref As New ArrayList

            Do While reads.Read()
                ref.Add(reads.Item(0))

            Loop
            reads.Close()
            Dim cmdSelec As New MySql.Data.MySqlClient.MySqlCommand("Delete FROM transactions WHERE student = ? And Session = ?", con)
            cmdSelec.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
            cmdSelec.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
            cmdSelec.ExecuteNonQuery()

            Dim cmdSelecd As New MySql.Data.MySqlClient.MySqlCommand("update studentsummary set status = '" & 0 & "' WHERE student = ? and session = ?", con)
            cmdSelecd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
            cmdSelecd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
            cmdSelecd.ExecuteNonQuery()
            For Each r As String In ref



                Dim cmdSelecx As New MySql.Data.MySqlClient.MySqlCommand("Delete FROM transactions WHERE  ref = '" & r & "'", con)
                cmdSelecx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                cmdSelecx.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                cmdSelecx.ExecuteNonQuery()



            Next

            Re_register_reverse(cla)

            con.Close()
        End Using

    End Sub
    Sub payment_schedules()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()


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

            Dim currentsession As String
            Dim currentterm As String
            Dim quarter As String
            If Now.Month <= 3 Then
                quarter = "1st Quarter"
            ElseIf Now.Month > 3 And Now.Month < 7 Then
                quarter = "2nd Quarter"
            ElseIf Now.Month >= 7 And Now.Month < 10 Then
                quarter = "3rd Quarter"
            ElseIf Now.Month >= 10 Then
                quarter = "4th Quarter"
            End If
            If Session("scheduletype") = "quarterly" Then
                Dim cmdInsertf As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session where term = '" & IIf(quarter = "1st Quarter" Or quarter = "2nd Quarter", "2nd term", IIf(quarter = "3rd Quarter", "3rd term", "1st term")) & "' Order by ID Desc", con)
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
            ElseIf Session("scheduletype") = "termly" Then
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
                Session("scheduletype") = "termly"

            ElseIf Session("scheduletype") = "quarterly" Then
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
                Session("scheduletype") = "quarterly"



            End If
            add_discount()
            con.Close()
        End Using
    End Sub

    Sub readd_fees()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.open()

            Dim cmdLoadd As New MySql.Data.MySqlClient.MySqlCommand("SELECT ref from transactions where account = '" & "ADVANCE FEE PAYMENT" & "'", con)
            Dim reads As MySql.Data.MySqlClient.MySqlDataReader = cmdLoadd.ExecuteReader
            Dim ref As New ArrayList
            Do While reads.Read()
                ref.Add(reads(0))
            Loop
            reads.Close()
            For Each item As Integer In ref
                Dim cmdSelec As New MySql.Data.MySqlClient.MySqlCommand("Delete FROM transactions where ref = '" & item & "'", con)
                cmdSelec.ExecuteNonQuery()
            Next



            con.close()
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
            Dim quarter As String
            If Now.Month <= 3 Then
                quarter = "1st Quarter"
            ElseIf Now.Month > 3 And Now.Month < 7 Then
                quarter = "2nd Quarter"
            ElseIf Now.Month >= 7 And Now.Month < 10 Then
                quarter = "3rd Quarter"
            ElseIf Now.Month >= 10 Then
                quarter = "4th Quarter"
            End If
            If Session("scheduletype") = "quarterly" Then
                Dim cmdInsertf As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session where term = '" & IIf(quarter = "1st Quarter" Or quarter = "2nd Quarter", "2nd term", IIf(quarter = "3rd Quarter", "3rd term", "1st term")) & "' Order by ID Desc", con)
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
            Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Select * from discount where student = ? and (recurring = '" & -Val(True) & "' or session = '" & currentsession & "')", con)
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
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
               If discounttype(ct) = "Fixed" Then
                    fee = fee - discountamt(ct)
                    min = min - discountamt(ct)
                Else
                    fee = fee - (fee * (discountamt(ct) / 100))
                    min = min - (min * (discountamt(ct) / 100))
                  
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

    Protected Sub Re_register(tpaid As Integer, cla As Integer)
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()

            Dim total As Double = 0

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
                Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, compulsory, min, monthly, termly, quarterly) Values (?,?,?,?,?,?,?,?,?,?)", con)
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("Sessionid")))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", classamount(classi)))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("compulsory", 1))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", min(classi)))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("mon", monthly(classi)))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amebt", classamount(classi)))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amezsfbt", quarterly(classi)))
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
                    ElseIf feereader2.Item("amount") = feereader2.Item("min") And balance <= (feereader2.Item("amount") - feereader2.Item("paid")) And balance > 0 Then
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
                    ElseIf feereader3.Item("amount") <> feereader3.Item("min") And balance <= (feereader3.Item("amount") - feereader3.Item("paid")) And balance > 0 Then
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
                    diff = Nothing
                    fbalance = Nothing
                End If
            End If
            balance = Nothing
            tpaid = Nothing
            total = Nothing
            con.Close()
        End Using


    End Sub

    Protected Sub Re_register_reverse(cla As Integer)
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim currentsession As String
            Dim currentterm As String
            If Session("scheduletype") = "quarterly" Then
                Dim cmdInsertf As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session where term = '" & IIf(lblQuarter.Text = "1st Quarter" Or lblQuarter.Text = "2nd Quarter", "2nd term", IIf(lblQuarter.Text = "3rd Quarter", "3rd term", "1st term")) & "' Order by ID Desc", con)
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
            Dim total As Double = 0

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
                Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, compulsory, min, monthly, termly, quarterly) Values (?,?,?,?,?,?,?,?,?,?)", con)
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", classamount(classi)))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("compulsory", 1))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", min(classi)))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("mon", monthly(classi)))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amebt", classamount(classi)))
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amezsfbt", quarterly(classi)))
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
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))

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
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))

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
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
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
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))

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
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))
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
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
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
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))
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
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))
                    cmdCheck4.ExecuteNonQuery()
                    admi = admi + 1



                Next

            End If
            If currentterm = "1st term" Then




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
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
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
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))

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
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))

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
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
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
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))

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
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))

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
                cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
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
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))

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
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))

                cmdCheck4.ExecuteNonQuery()
            End If
            add_discount()

        End Using

    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        
        If check.Check_Account(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try

            If Session("paidfees") = "yes" Then
                Show_Alert(True, "Your Transaction was successful. Payment Updated.")
                Session("paidfees") = Nothing

            ElseIf Session("paidfees") = "no" Then
                Show_Alert(False, "Your Transaction was not successful. Try again.")
                Session("paidfees") = Nothing
            End If
            If Not IsPostBack Then
                If Now.Month <= 3 Then
                    lblQuarter.Text = "1st Quarter"
                ElseIf Now.Month > 3 And Now.Month < 7 Then
                    lblQuarter.Text = "2nd Quarter"
                ElseIf Now.Month >= 7 And Now.Month < 10 Then
                    lblQuarter.Text = "3rd Quarter"
                ElseIf Now.Month >= 10 Then
                    lblQuarter.Text = "4th Quarter"
                End If

                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                    con.Open()
                    panel3.Visible = False

                    Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT class from class", con)
                    Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                    DropDownList1.Items.Clear()
                    Do While student.Read
                        DropDownList1.Items.Add(student.Item(0).ToString)
                    Loop

                    student.Close()
                    Dim ds As New DataTable
                    ds.Columns.Add("passport")
                    ds.Columns.Add("staffname")
                    Dim cmdload1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.passport, studentsprofile.admno, studentsprofile.surname from studentsummary inner join studentsprofile on studentsummary.student = studentsprofile.admno inner join class on studentsummary.class = class.id WHERE class.class = ? and studentsummary.session = '" & Session("sessionid") & "'", con)
                    cmdload1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class.ID", DropDownList1.Text))
                    cmdload1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter(" StudentSummary.Session ", Session("SessionID")))
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdload1.ExecuteReader
                    Do While student0.Read
                        ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                    Loop
                    student0.Close()
                    gridview1.DataSource = ds
                    gridview1.DataBind()
                    con.Close()
                End Using
                If Session("studentadd") <> Nothing And Session("scheduletype") <> Nothing Then Student_Details()
            End If

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try

    End Sub


    Protected Sub btnPay_Click(sender As Object, e As EventArgs) Handles btnPay.Click
        Response.Redirect("~/content/account/feeschedule.aspx?" & Session("Studentadd"))

    End Sub


    Private Sub Student_Details()

        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            
            
                Session("scheduletype") = "termly"
           


            Dim currentsession As String
            Dim currentterm As String
            If Session("scheduletype") = "quarterly" Then
                Dim cmdInsertf As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session where term = '" & IIf(lblQuarter.Text = "1st Quarter" Or lblQuarter.Text = "2nd Quarter", "2nd term", IIf(lblQuarter.Text = "3rd Quarter", "3rd term", "1st term")) & "' Order by ID Desc", con)
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
            Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport from StudentsProfile where admno = ?", con)
            cmdLoad4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("studentadd")))
            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
            Dim pass As String = ""
            If student.Read() Then pass = student.Item("passport").ToString
            student.Close()
            If pass <> "" Then Image1.ImageUrl = pass
            Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.surname, class.class, studentsummary.trans, studentsummary.status, studentsummary.class From Studentsummary INNER JOIN Studentsprofile on studentsummary.student = studentsprofile.admno inner join class on studentsummary.class = class.Id WHERE StudentSummary.student = ? And StudentSummary.Session = ?", con)
            cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class.ID", Session("Studentadd")))
            cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter(" StudentSummary.Session ", currentsession))
            Dim studentsReader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader()
            studentsReader.Read()
            lblClass.Text = studentsReader.Item(0).ToString
            Session("ClassId") = studentsReader.Item(4)
            studentsReader.Close()
            Dim elapsedmonths As Integer
            If currentterm = "1st term" Then
                elapsedmonths = Now.Month - 9
            ElseIf currentterm = "2nd term" Then
                elapsedmonths = Now.Month - 1
            Else
                elapsedmonths = Now.Month - 5
            End If
            Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where student = ? and session < " & currentsession & "", con)
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
            Dim feereader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
            Dim totalall As Integer = 0
            Dim paidall As Double = 0

            Do While feereader3.Read

                totalall = totalall + feereader3.Item("amount")
                paidall = paidall + feereader3.Item("paid")
            Loop
            feereader3.Close()
            Dim cmdCheck4r As New MySql.Data.MySqlClient.MySqlCommand("select cr from transactions where account like ? and student = ? and session = ? and date >= '" & Convert.ToDateTime(DateTime.ParseExact("01/" & IIf(Now.Month < 10, "0" & Now.Month, Now.Month) & "/" & Now.Year, "dd/MM/yyyy", Nothing)).ToString("yyyy-MM-dd HH:mm:ss") & "'", con)
            cmdCheck4r.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", "%PAID"))
            cmdCheck4r.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
            cmdCheck4r.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))
            Dim readref220r As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck4r.ExecuteReader
            Dim crs As Integer
            Do While readref220r.Read
                crs = crs + readref220r(0)
            Loop
            readref220r.Close()

            Dim cmdCheck44rt As New MySql.Data.MySqlClient.MySqlCommand("select cr from transactions where account like ? and student = ? and session = ? and date >= '" & IIf(lblQuarter.Text = "1st Quarter", Convert.ToDateTime(DateTime.ParseExact("01/01" & "/" & Now.Year, "dd/MM/yyyy", Nothing)).ToString("yyyy-MM-dd HH:mm:ss"), IIf(lblQuarter.Text = "2nd Quarter", Convert.ToDateTime(DateTime.ParseExact("01/04" & "/" & Now.Year, "dd/MM/yyyy", Nothing)).ToString("yyyy-MM-dd HH:mm:ss"), IIf(lblQuarter.Text = "3rd Quarter", Convert.ToDateTime(DateTime.ParseExact("01/07" & "/" & Now.Year, "dd/MM/yyyy", Nothing)).ToString("yyyy-MM-dd HH:mm:ss"), IIf(lblQuarter.Text = "4th Quarter", Convert.ToDateTime(DateTime.ParseExact("01/10" & "/" & Now.Year, "dd/MM/yyyy", Nothing)).ToString("yyyy-MM-dd HH:mm:ss"), "")))) & "'", con)
            cmdCheck44rt.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", "%PAID"))
            cmdCheck44rt.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentAdd")))
            cmdCheck44rt.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentsession))
            Dim readref5567 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck44rt.ExecuteReader
            Dim crsq As Integer
            Do While readref5567.Read
                crsq = crsq + readref5567(0)
            Loop
            readref5567.Close()

            Dim cmdInsert220 As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where session = ? and student = ?", con)
            cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentsession))
            cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("Studentadd")))
            Dim feereader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert220.ExecuteReader
            Dim total As Integer = 0
            Dim feetype As New ArrayList
            Dim feeamount As New ArrayList
            Dim paid As Double = 0
            Dim presentpaid As Double = 0
            Dim min As Double = 0
            Dim currentbal As Double = 0
            Dim payment As String = ""

            Do While feereader2.Read
                payment = feereader2("payment").ToString
                If payment <> "" And Session("scheduletype") = Nothing Then Session("scheduletype") = payment
                If feereader2("amount") <> 0 Then
                    feetype.Add(feereader2.Item("fee"))
                    feeamount.Add(feereader2.Item("amount"))
                    If Session("scheduletype") = "Monthly" And Val(feereader2("monthly")) <> 0 Then
                        If feereader2("paid") <> feereader2("amount") Then
                            total = total + (feereader2.Item("monthly"))
                            Dim diff As Integer = 4 - (feereader2("amount") / feereader2("monthly"))
                            min = min + (feereader2.Item("monthly") * (elapsedmonths + 1 - diff)) - Val(feereader2("paid"))
                            paid = paid + feereader2.Item("paid")
                            presentpaid = presentpaid + (Val(feereader2("paid") - feereader2.Item("monthly") * (elapsedmonths - diff)))
                            If Val(feereader2("paid")) < Val(feereader2("monthly")) * elapsedmonths Then
                                currentbal = currentbal + (Val(feereader2("monthly")) * (elapsedmonths - diff)) - Val(feereader2("paid"))
                            ElseIf Session("scheduletype") = "Monthly" And Val(feereader2("monthly")) = 0 Then
                                currentbal = currentbal + Val(feereader2("amount")) - Val(feereader2("paid"))
                            End If
                        End If
                    Else
                        If Session("scheduletype") = "Monthly" Then
                            total = total + feereader2.Item("amount") - feereader2.Item("paid")
                            min = min + feereader2.Item("min") - feereader2.Item("paid")
                        ElseIf Session("scheduletype") = "quarterly" Then
                            If feereader2("paid") <> feereader2("amount") Then
                                total = total + IIf(currentterm = "2nd term" And Now.Month <= 3, IIf(feereader2("amount") >= feereader2("quarterly"), feereader2("amount") - feereader2("quarterly"), 0), IIf(currentterm = "2nd term" And Now.Month > 3, IIf(feereader2("amount") <= feereader2("quarterly"), feereader2("amount"), feereader2("quarterly")), IIf(feereader2("amount") < feereader2("quarterly"), feereader2("amount"), feereader2.Item("quarterly"))))
                                min = 0
                                paid = paid + IIf(currentterm = "2nd term" And Now.Month > 3, Val(feereader2("paid")) - feereader2.Item("quarterly"), Val(feereader2("paid")))
                                If Val(feereader2("paid")) < Val(feereader2("quarterly")) * IIf(currentterm = "2nd term" And Now.Month > 3, 1, 0) Then
                                    currentbal = currentbal + IIf(feereader2("amount") >= feereader2("quarterly"), feereader2("amount") - feereader2("quarterly"), 0) - IIf(Val(feereader2("paid")) < IIf(feereader2("amount") >= feereader2("quarterly"), feereader2("amount") - feereader2("quarterly"), 0), feereader2("paid"), IIf(feereader2("amount") >= feereader2("quarterly"), feereader2("amount") - feereader2("quarterly"), 0))
                                ElseIf Session("scheduletype") = "quarterly" And Val(feereader2("quarterly")) = 0 Then
                                    currentbal = currentbal + Val(feereader2("amount")) - Val(feereader2("paid"))
                                End If
                            End If

                        Else
                            paid = paid + feereader2.Item("paid")
                            total = total + feereader2.Item("amount")
                            min = min + feereader2.Item("min")
                        End If
                    End If


                End If

            Loop

            If paid <= 0 And crs = 0 And crsq = 0 Then
                If Session("scheduletype") = "quarterly" And lblQuarter.Text = "1st Quarter" Then

                    btnChange.Visible = True
                ElseIf Session("scheduletype") = "quarterly" And lblQuarter.Text <> "1st Quarter" Then
                    btnChange.Visible = False

               
                End If

            Else
                btnChange.Visible = False
            End If
            If paid <> 0 Or Session("scheduletype") <> Nothing Then

                lblPaid.Text = FormatNumber(IIf(Session("scheduletype") = "Monthly", crs, IIf(Session("scheduletype") = "quarterly", crsq, IIf(paid > 0, paid, 0))), , , , TriState.True)
                If paid = 0 Then


                    lblFStatus.Text = "Not Paid"
                    lblFStatus.ForeColor = Drawing.Color.Red

                End If
                If paid < total And paid <> 0 Then
                    lblFStatus.Text = "Payment Incomplete"
                    lblFStatus.ForeColor = Drawing.Color.Red
                    btnReceipt.Visible = True

                ElseIf paid >= total And Session("scheduletype") <> "Monthly" Then
                    lblFStatus.Text = "Paid"
                    lblFStatus.ForeColor = Drawing.Color.Green
                    btnReceipt.Visible = True
                ElseIf presentpaid = 0 And Session("scheduletype") = "Monthly" Then
                    If crs = 0 Then
                        lblFStatus.Text = "Not Paid"
                        lblFStatus.ForeColor = Drawing.Color.Red

                    ElseIf crs > 0 And crs < total Then
                        lblFStatus.Text = "Payment Incomplete"
                        lblFStatus.ForeColor = Drawing.Color.Red
                        btnReceipt.Visible = True

                    Else
                        lblFStatus.Text = "Paid"
                        lblFStatus.ForeColor = Drawing.Color.Green
                        btnReceipt.Visible = True

                    End If

                ElseIf presentpaid < total And Session("scheduletype") = "Monthly" Then
                    lblFStatus.Text = "Payment Incomplete"
                    lblFStatus.ForeColor = Drawing.Color.Red
                    btnReceipt.Visible = True
                ElseIf presentpaid = total And Session("scheduletype") = "Monthly" Then
                    lblFStatus.Text = "Paid"
                    lblFStatus.ForeColor = Drawing.Color.Green
                    btnReceipt.Visible = True
                End If
                feereader2.Close()

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
                balance = cr - dr
                balreader.Close()
                Dim outstanding As Double

                outstanding = totalall - paidall + IIf(currentbal > 0, currentbal, 0)
                lblOutstanding.Text = FormatNumber(outstanding, , , , TriState.True)
                lblAdvance.Text = FormatNumber(balance, , , , TriState.True)

                lblOut.Text = FormatNumber(total - IIf(Session("scheduletype") = "Monthly", IIf(presentpaid > 0, presentpaid, 0), IIf(paid > 0, paid, 0)) + outstanding, , , , TriState.True)
                panel3.Visible = True
                pnlAll.Visible = False
                pnlDetails.Visible = True
                pnlOption.Visible = False
                gridview1.SelectedIndex = -1

                If Session("scheduletype") = "termly" Then
                    pnlQuarter.Visible = False
                ElseIf Session("scheduletype") = "Monthly" Then
                    pnlQuarter.Visible = False
                    Label7.Text = "Total Paid this Month"
                Else
                    pnlQuarter.Visible = True
                    Label7.Text = "Total Paid this Quarter"
                End If

            Else
                panel3.Visible = True
                pnlDetails.Visible = False
                pnlOption.Visible = True
                pnlAll.Visible = False
                btnChange.Visible = False
            End If
            con.Close()
        End Using

    End Sub

    Protected Sub gridview1_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles gridview1.SelectedIndexChanging
        Try
            Dim x As Array = Split(gridview1.Rows(e.NewSelectedIndex).Cells(1).Text, " - ")
            Session("studentadd") = RTrim(x(0))
            Student_Details()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.open()
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
                gridview1.SelectedIndex = -1
                Session("studentadd") = Nothing
                panel3.Visible = False
                pnlAll.Visible = True
                con.close()
            End Using

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        pnlAll.Visible = True
        panel3.Visible = False
        Session("scheduletype") = Nothing
        For Each y As ListItem In radType.Items
            y.Selected = False
        Next
    End Sub

    Protected Sub btnReceipt_Click(sender As Object, e As EventArgs) Handles btnReceipt.Click
        Response.Redirect("~/content/student/receipt.aspx")
    End Sub

    Protected Sub btnReregister_Click(sender As Object, e As EventArgs) Handles btnReregister.Click
        Try
            readd_fees()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub BtnReverse_Click(sender As Object, e As EventArgs) Handles BtnReverse.Click
        Try
            reverse()
            Session("scheduletype") = Nothing
            For Each y As ListItem In radType.Items
                y.Selected = False
            Next
            Student_Details()

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnChange_Click(sender As Object, e As EventArgs) Handles btnChange.Click
        panel3.Visible = True
        pnlDetails.Visible = False
        pnlOption.Visible = True
        pnlAll.Visible = False
        btnChange.Visible = False
        For Each y As ListItem In radType.Items
            y.Selected = False
        Next
    End Sub
End Class
