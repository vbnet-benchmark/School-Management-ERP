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
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("staffid") = Nothing Then            Dim x As ArrayList = check.Check_app_staff(Request.QueryString("username"), Request.QueryString("password"))            If x.Count <> 0 Then                Session("staffid") = x.Item(0)                Session("sessionid") = x.Item(1)            End If        End If
        Try


            If IsPostBack Then

            Else
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.Open()



                    panel3.Visible = False

                    Dim ds As New DataTable
                    ds.Columns.Add("passport")
                    ds.Columns.Add("staffname")
                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport, admno, surname from studentsprofile where activated = '" & 1 & "'", con)
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
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT fee from feeschedule where student = '" & Session("studentadd") & "' and session = '" & Session("sessionid") & "'", con)
            Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
            cboHostel.Items.Clear()
            cboHostel.Items.Add("Select Fee")
            Do While student0.Read
                cboHostel.Items.Add(student0.Item(0).ToString)
            Loop
            student0.Close()

            Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport, surname from StudentsProfile where admno = ?", con)
            cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("studentadd")))
            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
            Dim pass As String
            student.Read()
            pass = student.Item("passport").ToString
            lblName.Text = student.Item(1).ToString

            student.Close()
            Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id, fee, type, amount from discount where student = '" & Session("studentadd") & "'", con)
            Dim reader22 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
            Dim ds2 As New DataTable
            ds2.Columns.Add("Id")
            ds2.Columns.Add("fee")
            ds2.Columns.Add("type")
            ds2.Columns.Add("amount")
            Do While reader22.Read
                If reader22.Item("type").ToString = "Percentage" Then
                    ds2.Rows.Add(reader22.Item(0).ToString, reader22.Item(1).ToString, reader22.Item(2).ToString, reader22.Item(3).ToString & "%")
                Else
                    ds2.Rows.Add(reader22.Item(0).ToString, reader22.Item(1).ToString, reader22.Item(2).ToString, FormatNumber(reader22.Item(3).ToString, , , , TriState.True))
                End If
            Loop
            reader22.Close()
            Gridview2.DataSource = ds2
            Gridview2.DataBind()
            If pass <> "" Then Image1.ImageUrl = pass
            gridview1.SelectedIndex = -1
            panel3.Visible = True
            pnlAll.Visible = False
            con.close()        End Using
    End Sub






    Protected Sub LinkButton4_Click(sender As Object, e As EventArgs) Handles LinkButton4.Click
        panel2.Visible = True
        cboHostel.Text = "Select Fee"
        cboTransport.SelectedIndex = 0
        txtValue.Text = ""
    End Sub



    Protected Sub gridview1_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles gridview1.SelectedIndexChanging
        Try
            Dim x As Array = Split(gridview1.Rows(e.NewSelectedIndex).Cells(1).Text, " - ")
            Session("studentadd") = RTrim(x(0))
            Student_Details()
            gridview1.SelectedIndex = -1
            pnlAll.Visible = False
            panel3.Visible = True
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub



    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim ds As New DataTable
                ds.Columns.Add("passport")
                ds.Columns.Add("staffname")
                Dim s As String = "%" & txtSearch.Text & "%"
                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport, admno, surname from studentsprofile where surname like '" & s & "' or admno = '" & txtSearch.Text & "' and activated = '" & 1 & "'", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                Do While student0.Read
                    ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                Loop
                student0.Close()
                gridview1.DataSource = ds
                gridview1.DataBind()
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub



    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                If cboHostel.Text = "Select Fee" Or cboTransport.Text = "Select Type" Or txtValue.Text = "" Then
                    Show_Alert(False, "Enter missing values")
                    Exit Sub
                End If


                If Session("editdiscount") <> Nothing Then
                    Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT type, amount from discount where id = '" & Session("editdiscount") & "'", con)
                    Dim reader22 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                    reader22.Read()
                    Dim previousdiscount As Integer = reader22.Item(1)
                    Dim previoustype As String = reader22.Item(0)
                    reader22.Close()

                    Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("Update discount set fee = ?, type = ?, amount = ? where id = ?", con)
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fwe", cboHostel.Text))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", cboTransport.Text))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", txtValue.Text.Replace(",", "")))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("editdiscount")))
                    cmdInsert22.ExecuteNonQuery()
                    Session("editdiscount") = Nothing

                    Dim cmdSelect2s As New MySql.Data.MySqlClient.MySqlCommand("SELECT fee, paid from feeschedule where student = '" & Session("Studentadd") & "' and session = '" & Session("sessionid") & "' and paid <> '" & 0 & "'", con)
                    Dim param As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2s.ExecuteReader

                    Dim dsfees As New DataTable
                    dsfees.Columns.Add("fee")
                    dsfees.Columns.Add("amount")
                    Dim nextmsg As String = ""

                    If param.Read Then
                        nextmsg = "Discount takes effect from next term"
                        param.Close()
                    Else
                        param.Close()
                        Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where student = ? and session = ? and fee = ?", con)
                        cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                        cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                        cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fwe", cboHostel.Text))
                        Dim feereader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                        feereader3.Read()
                        Dim fee As Integer = feereader3.Item(2)
                        Dim min As Integer = feereader3.Item("min")

                        feereader3.Close()
                        If previoustype = "Fixed" Then
                            fee = fee + previousdiscount
                            min = min + previousdiscount
                        Else
                            fee = fee / (1 - (previousdiscount / 100))
                            min = min / (1 - (previousdiscount / 100))
                        End If

                        If cboTransport.Text = "Fixed" Then
                            fee = fee - txtValue.Text.Replace(",", "")
                            min = min - txtValue.Text.Replace(",", "")
                        Else
                            fee = fee - (fee * (txtValue.Text.Replace(",", "") / 100))
                            min = min - (min * (txtValue.Text.Replace(",", "") / 100))
                        End If
                        Dim cmdInsert225 As New MySql.Data.MySqlClient.MySqlCommand("Update feeschedule set amount = '" & fee & "', min = '" & min & "' where session = ? and fee = ? and student = ?", con)
                        cmdInsert225.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                        cmdInsert225.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", cboHostel.Text))
                        cmdInsert225.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                        cmdInsert225.ExecuteNonQuery()

                        Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("select ref from transactions where account like ? and student = ? and session = ?", con)
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", cboHostel.Text & "%"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))
                        Dim readref220 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck4.ExecuteReader
                        readref220.Read()
                        Dim refe As Integer = readref220.Item(0).ToString
                        readref220.Close()
                        Dim cmdCheck04 As New MySql.Data.MySqlClient.MySqlCommand("update transactions set cr = '" & FormatNumber(fee, , , , TriState.True) & "' where ref = ? and account = '" & cboHostel.Text & " DEBTS" & "'", con)
                        cmdCheck04.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("refa", refe))
                        cmdCheck04.ExecuteNonQuery()

                        Dim cmdCheck041 As New MySql.Data.MySqlClient.MySqlCommand("update transactions set dr = '" & FormatNumber(fee, , , , TriState.True) & "' where ref = ? and account = '" & "DEBTORS" & "'", con)
                        cmdCheck041.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("refa", refe))
                        cmdCheck041.ExecuteNonQuery()
                    End If



                    logify.log(Session("staffid"), "Discount on " & cboHostel.Text & " was changed to " & txtValue.Text & " - " & cboTransport.Text & " for " & par.getstuname(Session("studentadd")))
                    logify.Notifications("Discount on " & cboHostel.Text & " was changed to " & txtValue.Text & " - " & cboTransport.Text & " for " & par.getstuname(Session("studentadd")) & " on " & cboHostel.Text & ". " & nextmsg, par.getparent(Session("studentadd")), Session("staffid"), "Accounts", "")
                    Show_Alert(True, "Discount Updated successfully. " & nextmsg)
                Else
                    Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO discount (student, fee, type, amount) Values (?,?,?,?)", con)
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fwe", cboHostel.Text))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", cboTransport.Text))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", txtValue.Text.Replace(",", "")))
                    cmdInsert22.ExecuteNonQuery()
                    Dim cmdSelect2s As New MySql.Data.MySqlClient.MySqlCommand("SELECT fee, paid from feeschedule where student = '" & Session("Studentadd") & "' and session = '" & Session("sessionid") & "' and paid <> '" & 0 & "'", con)
                    Dim param As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2s.ExecuteReader

                    Dim dsfees As New DataTable
                    dsfees.Columns.Add("fee")
                    dsfees.Columns.Add("amount")
                    Dim nextmsg As String = ""

                    If param.Read Then
                        nextmsg = "Discount takes effect from next term"
                        param.Close()
                    Else
                        param.Close()

                        Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where student = ? and session = ? and fee = ?", con)
                        cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                        cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                        cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fwe", cboHostel.Text))
                        Dim feereader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                        feereader3.Read()
                        Dim fee As Integer = feereader3.Item(2)
                        Dim min As Integer = feereader3.Item("min")
                        feereader3.Close()
                        If cboTransport.Text = "Fixed" Then
                            fee = fee - txtValue.Text.Replace(",", "")
                            min = min - txtValue.Text.Replace(",", "")
                        Else
                            fee = fee - (fee * (txtValue.Text.Replace(",", "") / 100))
                            min = min - (min * (txtValue.Text.Replace(",", "") / 100))
                        End If

                        Dim cmdInsert225 As New MySql.Data.MySqlClient.MySqlCommand("Update feeschedule set amount = '" & fee & "', min = '" & IIf(min < 0, 0, min) & "' where session = ? and fee = ? and student = ?", con)
                        cmdInsert225.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                        cmdInsert225.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", cboHostel.Text))
                        cmdInsert225.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                        cmdInsert225.ExecuteNonQuery()

                        Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("select ref from transactions where account like ? and student = ? and session = ?", con)
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", cboHostel.Text & "%"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))
                        Dim readref220 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck4.ExecuteReader
                        readref220.Read()
                        Dim refe As Integer = readref220.Item(0).ToString
                        readref220.Close()
                        Dim cmdCheck04 As New MySql.Data.MySqlClient.MySqlCommand("update transactions set cr = '" & FormatNumber(fee, , , , TriState.True) & "' where ref = ? and account = '" & cboHostel.Text & " DEBTS" & "'", con)
                        cmdCheck04.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("refa", refe))
                        cmdCheck04.ExecuteNonQuery()

                        Dim cmdCheck041 As New MySql.Data.MySqlClient.MySqlCommand("update transactions set dr = '" & FormatNumber(fee, , , , TriState.True) & "' where ref = ? and account = '" & "DEBTORS" & "'", con)
                        cmdCheck041.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("refa", refe))
                        cmdCheck041.ExecuteNonQuery()
                    End If
                    Dim cmdInsert220 As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where session = ? and student = ?", con)
                    cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                    cmdInsert220.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("Studentadd")))
                    Dim feereader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert220.ExecuteReader
                    Dim totals As Integer = 0

                    Do While feereader2.Read
                        totals = totals + feereader2.Item("amount")
                    Loop
                    feereader2.Close()
                    If totals = 0 Then
                        Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Update StudentSummary Set status = ? where student = ? and session = '" & Session("SessionID") & "'", con)
                        cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("aveage", True))
                        cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("Studentadd")))
                        cmdInsert.ExecuteNonQuery()
                    End If

                    logify.log(Session("staffid"), "A " & cboTransport.Text & " discount of " & txtValue.Text & " was given to " & par.getstuname(Session("studentadd")) & " on " & cboHostel.Text)
                    logify.Notifications("A " & cboTransport.Text & " discount of " & txtValue.Text & " was given to " & par.getstuname(Session("studentadd")) & " on " & cboHostel.Text & ". " & nextmsg, par.getparent(Session("studentadd")), Session("staffid"), "Accounts", "~/content/app/parent/feeboard.aspx?" & Session("studentadd"), "")
                    Show_Alert(True, "Discount added successfully. " & nextmsg)
                End If
                panel2.Visible = False
                con.close()            End Using
            Student_Details()


          
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview2_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles Gridview2.RowDeleting
        Try
            Dim row As GridViewRow = Gridview2.Rows(e.RowIndex)
            Dim fees As String = row.Cells(1).Text
            Dim previoustype As String = row.Cells(2).Text
            Dim previousdiscount As String = row.Cells(3).Text.Replace("%", "")
            previousdiscount = previousdiscount.Replace(",", "")
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim enter As New MySql.Data.MySqlClient.MySqlCommand("delete from discount where fee = '" & fees & "' and student = '" & Session("studentadd") & "'", con)
                enter.ExecuteNonQuery()

                Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Select * from feeschedule where student = ? and session = ? and fee = ?", con)
                cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fwe", fees))
                Dim feereader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                feereader3.Read()
                Dim fee As Integer = feereader3.Item(2)
                Dim min As Integer = feereader3.Item("min")

                feereader3.Close()
                If previoustype = "Fixed" Then
                    fee = fee + Val(previousdiscount)
                    min = min + Val(previousdiscount)
                Else
                    fee = fee / (1 - (previousdiscount / 100))
                    min = min / (1 - (previousdiscount / 100))
                End If


                Dim cmdInsert225 As New MySql.Data.MySqlClient.MySqlCommand("Update feeschedule set amount = '" & fee & "', min = '" & min & "' where session = ? and fee = ? and student = ?", con)
                cmdInsert225.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                cmdInsert225.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", fees))
                cmdInsert225.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                cmdInsert225.ExecuteNonQuery()

                Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("select ref from transactions where account like ? and student = ? and session = ?", con)
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", fees & "%"))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))
                Dim readref220 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck4.ExecuteReader
                readref220.Read()
                Dim refe As Integer = readref220.Item(0).ToString
                readref220.Close()
                Dim cmdCheck04 As New MySql.Data.MySqlClient.MySqlCommand("update transactions set cr = '" & FormatNumber(fee, , , , TriState.True) & "' where ref = ? and account = '" & fees & " DEBTS" & "'", con)
                cmdCheck04.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("refa", refe))
                cmdCheck04.ExecuteNonQuery()

                Dim cmdCheck041 As New MySql.Data.MySqlClient.MySqlCommand("update transactions set dr = '" & FormatNumber(fee, , , , TriState.True) & "' where ref = ? and account = '" & "DEBTORS" & "'", con)
                cmdCheck041.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("refa", refe))
                cmdCheck041.ExecuteNonQuery()
                gridview1.EditIndex = -1
                con.Close()                logify.log(Session("staffid"), fee & " discount has been removed from " & par.getstuname(Session("studentadd")))
                logify.Notifications(fee & " discount has been removed from " & par.getstuname(Session("studentadd")), par.getparent(Session("studentadd")), Session("staffid"), "Accounts", "")
            End Using
           
            Student_Details()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview2_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles Gridview2.SelectedIndexChanging
        Try
            Dim fee As String = Gridview2.Rows(e.NewSelectedIndex).Cells(1).Text
            Dim type As String = Gridview2.Rows(e.NewSelectedIndex).Cells(2).Text
            panel2.Visible = True
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmd25 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id,  amount from discount where fee  = '" & fee & "' and type = '" & type & "'", con)
                Dim reader225 As MySql.Data.MySqlClient.MySqlDataReader = cmd25.ExecuteReader
                reader225.Read()

                cboHostel.Text = fee
                cboTransport.Text = type
                Session("editdiscount") = reader225(0).ToString

                If type = "Fixed" Then
                    txtValue.Text = FormatNumber(reader225.Item(1).ToString, , , , TriState.True)
                Else
                    txtValue.Text = reader225.Item(1).ToString
                End If
                reader225.Close()
                con.close()            End Using

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        pnlAll.Visible = True
        panel3.Visible = False
        gridview1.SelectedIndex = -1
    End Sub

    Protected Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim ds As New DataTable
                ds.Columns.Add("passport")
                ds.Columns.Add("staffname")
                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport, admno, surname from studentsprofile", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                Do While student0.Read
                    ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                Loop
                student0.Close()
                gridview1.DataSource = ds
                gridview1.DataBind()
                con.close()            End Using
            gridview1.DataBind()
            Session("studentadd") = Nothing
            panel3.Visible = False
            gridview1.SelectedIndex = -1
           
            If gridview1.PageIndex = gridview1.PageCount - 1 Then
                btnNext.Visible = False
            Else
                btnNext.Visible = True
            End If
            If gridview1.PageIndex = 0 Then
                btnPrevious.Visible = False
            Else
                btnPrevious.Visible = True
            End If


            If gridview1.PageIndex + 1 <= gridview1.PageCount Then
                gridview1.PageIndex = gridview1.PageIndex + 1
                gridview1.DataBind()
                If gridview1.PageIndex = gridview1.PageCount - 1 Then
                    btnNext.Visible = False
                Else
                    btnNext.Visible = True
                End If
                If gridview1.PageIndex = 0 Then
                    btnPrevious.Visible = False
                Else
                    btnPrevious.Visible = True
                End If
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim ds As New DataTable
            ds.Columns.Add("passport")
            ds.Columns.Add("staffname")
            Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport, admno, surname from studentsprofile", con)
            Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
            Do While student0.Read
                ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
            Loop
            student0.Close()
            gridview1.DataSource = ds
            gridview1.DataBind()
            con.close()        End Using
        gridview1.DataBind()
        Session("studentadd") = Nothing
        panel3.Visible = False
        gridview1.SelectedIndex = -1
       
        If gridview1.PageIndex = gridview1.PageCount - 1 Then
            btnNext.Visible = False
        Else
            btnNext.Visible = True
        End If
        If gridview1.PageIndex = 0 Then
            btnPrevious.Visible = False
        Else
            btnPrevious.Visible = True
        End If

       
        If gridview1.PageIndex - 1 >= 0 Then
            gridview1.PageIndex = gridview1.PageIndex - 1
            gridview1.DataBind()
            If gridview1.PageIndex = gridview1.PageCount - 1 Then
                btnNext.Visible = False
            Else
                btnNext.Visible = True
            End If
            If gridview1.PageIndex = 0 Then
                btnPrevious.Visible = False
            Else
                btnPrevious.Visible = True
            End If

        End If
    End Sub
End Class
