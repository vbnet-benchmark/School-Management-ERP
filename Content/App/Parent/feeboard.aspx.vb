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

    Dim alertmsg As New Alerts
    Dim alert As New Literal
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
        If Session("staffid") = Nothing Then 
 Dim x As ArrayList = check.Check_app_staff(Request.QueryString("username"), Request.QueryString("password")) 
 If x.Count <> 0 Then 
 Session("staffid") = x.Item(0) 
 Session("sessionid") = x.Item(1) 
 End If 
 End If 
        Try

            logify.Read_notification("~/content/App/App/parent/feeboard.aspx?" & Request.QueryString.ToString, Session("parentid"))

            If Session("paidfees") = "yes" Then
                Show_Alert(True, "Your Transaction was successful. Payment Updated. Click View Receipt to view termly receipt ")
                Session("paidfees") = Nothing

            ElseIf Session("paidfees") = "no" Then
                Show_Alert(False, "Your Transaction was not successful. Try again.")
                Session("paidfees") = Nothing
            End If

            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                panel3.Visible = False

                Dim ds As New DataTable
                ds.Columns.Add("passport")
                ds.Columns.Add("staffname")
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.passport, studentsprofile.admno, studentsprofile.surname from parentward inner join studentsprofile on parentward.ward = studentsprofile.admno WHERE parentward.parent = ?", con)
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Session("parentID")))
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
                Do While student0.Read
                    ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                Loop
                student0.Close()
                gridview1.DataSource = ds
                gridview1.DataBind()
                con.close()            End Using
            If Request.QueryString.ToString <> Nothing Then
                Session("studentadd") = Request.QueryString.ToString
            End If
            If Session("studentadd") <> Nothing Then
                Student_Details()
            End If

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub


    Protected Sub btnPay_Click(sender As Object, e As EventArgs) Handles btnPay.Click
        Response.Redirect("~/content/App/App/parent/feeschedule.aspx?" & Session("Studentadd"))

    End Sub


    Private Sub Student_Details()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport from StudentsProfile where admno = ?", con)
            cmdLoad4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("studentadd")))
            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
            Dim pass As String = ""
            If student.Read() Then pass = student.Item("passport").ToString
            student.Close()
            If Not pass = "" Then Image1.ImageUrl = pass
            Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.surname, class.class, studentsummary.trans, studentsummary.status, studentsummary.class From Studentsummary INNER JOIN Studentsprofile on studentsummary.student = studentsprofile.admno inner join class on studentsummary.class = class.Id WHERE StudentSummary.student = ? And StudentSummary.Session = ?", con)
            cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class.ID", Session("Studentadd")))
            cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter(" StudentSummary.Session ", Session("SessionID")))
            Dim studentsReader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader()
            If studentsReader.Read() Then
                lblClass.Text = studentsReader.Item(0).ToString
                Session("ClassId") = studentsReader.Item(4)
            Else
                Show_Alert(False, "Student not in a class yet.")
               
        Exit Sub
        End If

        studentsReader.Close()
        Dim cmdInsert220 As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where session = ? and student = ?", con)
        cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
        cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("Studentadd")))
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
        lblPaid.Text = FormatNumber(paid, , , , TriState.True)
        If paid = 0 Then
            lblFStatus.Text = "Not Paid"
            lblFStatus.ForeColor = Drawing.Color.Red
        ElseIf paid < total Then
            lblFStatus.Text = "Payment Incomplete"
            lblFStatus.ForeColor = Drawing.Color.Red
        Else
            lblFStatus.Text = "Paid"
            lblFStatus.ForeColor = Drawing.Color.Green
        End If
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
        If paid <> 0 Then btnReceipt.Visible = True
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
        outstanding = totalall - paidall
        lblOutstanding.Text = FormatNumber(outstanding, , , , TriState.True)
        lblAdvance.Text = FormatNumber(balance, , , , TriState.True)
        lblOut.Text = FormatNumber((total - paid) + outstanding, , , , TriState.True)
        panel3.Visible = True
        pnlAll.Visible = False
        gridview1.SelectedIndex = -1
        con.close()end using

    End Sub

    Protected Sub gridview1_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles gridview1.SelectedIndexChanging
        Try
            Dim x As Array = Split(gridview1.Rows(e.NewSelectedIndex).Cells(1).Text, " - ")
            Session("studentadd") = RTrim(x(0))
            pnlAll.Visible = False
            Student_Details()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        pnlAll.Visible = True
        panel3.Visible = False
    End Sub
    Protected Sub btnReceipt_Click(sender As Object, e As EventArgs) Handles btnReceipt.Click
        Response.Redirect("~/content/App/App/student/receipt.aspx")
    End Sub
End Class
