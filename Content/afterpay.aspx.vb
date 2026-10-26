Imports Newtonsoft.Json
Imports Paystack.Net.Contstants
Imports Paystack.Net.Interfaces
Imports Paystack.Net.Models
Imports Paystack.Net.Models.Authorizations
Imports Paystack.Net.Models.Exports
Imports Paystack.Net.Models.TransTotal
Imports Paystack.Net.SDK.Transactions
Partial Class Content_pay
    Inherits System.Web.UI.Page

    Dim alert As New Literal
    Dim alertmsg As New Alerts
    Dim logify As New notify
    Dim check As New CheckUser
    Dim alertPLC As New PlaceHolder
    Dim par As New parentcheck
    
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
           
            If Request.QueryString.ToString = Session("tref") Then
              
                After_pay()
                Session("paidfees") = "yes"

            Else
                Session("paidfees") = "no"
            End If
            If Session("staffid") <> "" Then
                logify.log(Session("staffid"), Session("UserName") & " made a school fee payment of " & Session("total") & " for " & par.getstuname(Session("Studentadd")))

                Response.Redirect("~/content/account/feeboard.aspx")
            ElseIf Session("parentid") <> "" Then

                logify.log(Session("parentid"), Session("UserName") & " made a school fee payment of " & Session("total") & " for " & par.getstuname(Session("Studentadd")))
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.open()
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT username from acclogin", con)
                    Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    Do While reader.Read
                        logify.Notifications(Session("UserName") & " made a fee payment of " & FormatNumber(Session("total"), , , , TriState.True) & " for " & par.getstuname(Session("Studentadd")), reader(0), Session("parentid"), "Parent", "~/content/account/transactions.aspx", "")
                    Loop
                    reader.Close()
                    con.close()                End Using
                Response.Redirect("~/content/parent/feeboard.aspx")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Sub After_pay()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdInsertf As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session Order by ID Desc", con)
            Dim reader2f As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertf.ExecuteReader
            reader2f.Read()

            Dim currentSession As String = reader2f(0).ToString
            reader2f.Close()
            Dim cmdInsert220 As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where session = ? and student = ?", con)
            cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", currentSession))
            cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("Studentadd")))
            Dim feereader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert220.ExecuteReader
            Dim total As Integer = 0
            Dim feetype As New ArrayList
            Dim feeamount As New ArrayList
            Dim paid As Double = 0
            Dim min As Double = 0
            feereader2.Close()
            Dim k As Integer
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
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("Studentadd")))
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
                    cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("Studentadd")))
                    cmdCheck03.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentSession))
                    cmdCheck03.ExecuteNonQuery()

                    Dim cmdCheck02 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d))
                    cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                    cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(Session("amount" & k) - Session("paid" & k), , , TriState.True)))
                    cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", Session("fee" & k) & " PAID"))
                    cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees paid"))
                    cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                    cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("Studentadd")))
                    cmdCheck02.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentSession))
                    cmdCheck02.ExecuteNonQuery()
                    Session("fee" & k) = Nothing
                    Session("amount" & k) = Nothing
                    Session("paid" & k) = Nothing
                    d = Nothing
                End If
            Next
            Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update Studentsprofile Set admfees = ? where admno = ? ", con)
            cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("aveage", "Paid"))
            cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("Studentadd")))
            cmdInsert2.ExecuteNonQuery()
            If Session("currenttotal") >= Session("total") Then
                Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Update StudentSummary Set status = ? where student = ?", con)
                cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("aveage", True))
                cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("Studentadd")))
                cmdInsert.ExecuteNonQuery()

            Else
                Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Update StudentSummary Set status = ? where student = ? and session <> '" & currentSession & "'", con)
                cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("aveage", True))
                cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("Studentadd")))
                cmdInsert.ExecuteNonQuery()
            End If
            Dim owing As Double = Session("Total") - Session("over")

            If Not owing = 0 Then

                Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", Session("tref")))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(owing, , , TriState.True)))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees paid by " & Session("Studentadd")))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Credit"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("Studentadd")))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentSession))

                cmdCheck4.ExecuteNonQuery()
                Dim cmdCheck5 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", Session("tref")))
                cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Cash"))
                cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(owing, , , TriState.True)))
                cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "BANK ACCOUNT"))
                cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees paid by " & Session("Studentadd")))
                cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("Studentadd")))
                cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentSession))
                cmdCheck5.ExecuteNonQuery()
            End If





            If Session("over") <> 0 Then
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
                Dim cmdCheck6 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d2))
                cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Liability"))
                cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(Session("over"), , , TriState.True)))
                cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "ADVANCE FEE PAYMENT"))
                cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees paid in advance." & Session("Studentadd")))
                cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Credit"))
                cmdCheck6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("Studentadd")))
                cmdCheck6.ExecuteNonQuery()

                Dim cmdCheck5 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d2))
                cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Cash"))
                cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(Session("over"), , , TriState.True)))
                cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "BANK ACCOUNT"))
                cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees paid by " & Session("Studentadd")))
                cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("Studentadd")))
                cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", currentSession))
                cmdCheck5.ExecuteNonQuery()
            End If


            k = Nothing
            Session("count") = Nothing

            con.close()        End Using

    End Sub
End Class
