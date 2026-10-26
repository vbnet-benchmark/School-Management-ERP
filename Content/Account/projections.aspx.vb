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
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If check.Check_Account(Session("roles"), Session("usertype")) = False Then
           if check.Check_oh(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        End If

        Try

       
        If Not IsPostBack Then

            
            Load_Accounts()
                Load_school_stats()
                txtSalary.Text = 4
                txtTerms.Text = 1
                expenses()
                projections()
        Else
                projections()
        End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Private Sub expenses()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim ds As New DataTable
            ds.Columns.Add("EXPENSE")
            ds.Columns.Add("AMOUNT")
            Dim comfirm As New MySql.Data.MySqlClient.MySqlCommand("Select * from staffprofile where activated = ?", con)
            comfirm.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", 1))
            Dim rcomfirm As MySql.Data.MySqlClient.MySqlDataReader = comfirm.ExecuteReader
            Dim salaries As Double = 0
            Do While rcomfirm.Read
                salaries = salaries + Val(rcomfirm("salary").ToString.Replace(",", ""))
            Loop
            salaries = salaries * Val(txtSalary.Text)
            rcomfirm.Close()
            ds.Rows.Add("SALARY EXPENSES", FormatNumber(salaries, , , , TriState.True))
            ds.Rows.Add("ALLOWANCES", FormatNumber(0, , , , TriState.True))
            Dim expense As String = "Expense"
            Dim cmdLoad5 As New MySql.Data.MySqlClient.MySqlCommand("SELECT accname from accsettings where type = '" & expense & "'", con)
            Dim student05 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad5.ExecuteReader
            Do While student05.Read
                If Not student05(0).ToString = "SALARY EXPENSES" And Not student05(0).ToString = "ALLOWANCES" Then
                    ds.Rows.Add(student05(0).ToString, FormatNumber(0, , , , TriState.True))

                End If
            Loop
            student05.Close()
            gidExpenses.DataSource = ds
            gidExpenses.DataBind()
            con.close()        End Using
    End Sub
    Private Sub projections()

        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim ds As New DataTable

            Dim gtotal As Double = 0
            For Each row As GridViewRow In gridClassfees.Rows
                Dim clas As String = row.Cells(0).Text
                For Each row2 As GridViewRow In gridSchool.Rows
                    Dim total As Double = 0
                    If row2.Cells(0).Text = "STUDENTS IN " & clas Then
                        total = Val(Replace(row.Cells(2).Text, ",", "")) * Val(row2.Cells(1).Text) * Val(txtTerms.Text)
                        gtotal = gtotal + total
                    End If
                Next
            Next
            Dim comfirm As New MySql.Data.MySqlClient.MySqlCommand("Select * from class", con)
            Dim readref As MySql.Data.MySqlClient.MySqlDataReader = comfirm.ExecuteReader
            Dim classes As New ArrayList
            Do While readref.Read
                classes.Add(readref.Item(1))
            Loop
            readref.Close()
            For Each row As GridViewRow In gridOtherfees.Rows
                Dim fee As String = row.Cells(0).Text
                Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from optionalfees", con)
                Dim reader22 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                Do While reader22.Read
                    If reader22(1).ToString = fee Then
                        For Each item As String In classes
                            For Each row2 As GridViewRow In gridSchool.Rows
                                Dim total As Double = 0
                                If row2.Cells(0).Text = "STUDENTS IN " & item Then
                                    total = Val(Replace(row.Cells(1).Text, ",", "")) * Val(row2.Cells(1).Text) * Val(txtTerms.Text)
                                    gtotal = gtotal + total
                                End If
                            Next
                        Next
                        reader22.Close()
                        Exit Do
                    End If
                Loop
                reader22.Close()

                Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from sessionalfees", con)
                Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmd3.ExecuteReader
                Dim fterms As Integer
                If Session("term") = "1st Term" Then
                    fterms = Math.Ceiling(Val(txtTerms.Text) / 3)
                ElseIf Session("term") = "2nd Term" Then
                    If Not Val(txtTerms.Text) < 3 Then
                        fterms = Val(txtTerms.Text) \ 3
                    End If
                ElseIf Session("term") = "3rd Term" Then
                    If Not Val(txtTerms.Text) < 2 Then
                        fterms = (Val(txtTerms.Text) + 1) \ 3
                    End If
                End If
                If fterms <> 0 Then
                    Do While reader3.Read
                        If fee = reader3(1).ToString Then
                            For Each item As String In classes
                                For Each row2 As GridViewRow In gridSchool.Rows
                                    Dim total As Double = 0
                                    If row2.Cells(0).Text = "STUDENTS IN " & item Then
                                        total = Val(Replace(row.Cells(1).Text, ",", "")) * Val(row2.Cells(1).Text) * fterms
                                        gtotal = gtotal + total
                                    End If
                                Next
                            Next
                        End If
                    Loop

                End If
                reader3.Close()
                If fee = "BOARDING" Then
                    For Each row2 As GridViewRow In gridSchool.Rows
                        Dim total As Double = 0
                        If row2.Cells(0).Text = "BOARDERS" Then
                            total = Val(Replace(row.Cells(1).Text, ",", "")) * Val(row2.Cells(1).Text) * Val(txtTerms.Text)
                            gtotal = gtotal + total
                        End If
                    Next
                End If
                Dim cmd2x As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from choicefees", con)
                Dim reader22x As MySql.Data.MySqlClient.MySqlDataReader = cmd2x.ExecuteReader
                Do While reader22x.Read
                    If reader22x(1).ToString = fee Then
                        For Each row2 As GridViewRow In gridSchool.Rows
                            Dim total As Double = 0
                            If row2.Cells(0).Text = "STUDENTS PAYING FOR " & fee Then
                                total = Val(Replace(row.Cells(1).Text, ",", "")) * Val(row2.Cells(1).Text) * Val(txtTerms.Text)
                                gtotal = gtotal + total
                            End If
                        Next
                        reader22x.Close()
                        Exit Do
                    End If
                Loop
                reader22x.Close()
                Dim cmd6 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from transportfees", con)
                Dim reader6 As MySql.Data.MySqlClient.MySqlDataReader = cmd6.ExecuteReader
                Do While reader6.Read

                    If fee = reader6.Item(0).ToString & " TRANSPORT ROUTE" Then
                        For Each row2 As GridViewRow In gridSchool.Rows
                            Dim total As Double = 0
                            If row2.Cells(0).Text = "STUDENTS IN " & reader6.Item(0).ToString & " ROUTE" Then

                                total = Val(Replace(row.Cells(1).Text, ",", "")) * Val(row2.Cells(1).Text) * Val(txtTerms.Text)
                                gtotal = gtotal + total
                            End If
                        Next
                        reader6.Close()
                        Exit Do
                    End If
                Loop
                reader6.Close()
                Dim cmd4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from onetimefees", con)
                Dim reader4 As MySql.Data.MySqlClient.MySqlDataReader = cmd4.ExecuteReader

                Do While reader4.Read
                    If fee = reader4.Item(1).ToString Then
                        reader4.Close()
                        Dim comfirmd As New MySql.Data.MySqlClient.MySqlCommand("Select admfees from studentsprofile where activated = '" & 1 & "' and admfees = '" & "Not paid" & "'", con)
                        Dim readrefd As MySql.Data.MySqlClient.MySqlDataReader = comfirmd.ExecuteReader
                        Dim admno As Integer = 0
                        Do While readrefd.Read
                            admno = admno + 1
                        Loop
                        readrefd.Close()

                        Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.class as Class, studentsummary.classNo as 'No of Students' from class left join studentsummary on class.id = studentsummary.class where studentsummary.session = '" & Session("sessionid") & "' order by studentsummary.session desc", con)
                        Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader

                        Dim oristuno As Integer = 0
                        Dim clas As New ArrayList
                        Do While reader2.Read
                            If Not clas.Contains(reader2(0)) Then
                                oristuno = oristuno + reader2(1)
                                clas.Add(reader2(0))
                            End If
                        Loop
                        reader2.Close()
                        Dim newstuno As Integer = 0
                        For Each item As String In clas
                            For Each row2 As GridViewRow In gridSchool.Rows
                                If row2.Cells(0).Text = "STUDENTS IN " & item Then
                                    newstuno = newstuno + Val(row2.Cells(1).Text)
                                End If
                            Next
                        Next
                        Dim studif As Integer = newstuno - oristuno
                        If studif > 0 Then admno = admno + studif

                        Dim total As Double = 0
                        total = Val(Replace(row.Cells(1).Text, ",", "")) * admno
                        gtotal = gtotal + total
                        Exit Do
                    End If
                Loop
                reader4.Close()

            Next
            For Each row As GridViewRow In gridStock.Rows
                gtotal = gtotal + Val(Replace(row.Cells(1).Text, ",", ""))
            Next

            Dim gexpenses As Double = 0
            For Each row As GridViewRow In gidExpenses.Rows
                gexpenses = gexpenses + (Val(Replace(row.Cells(1).Text, ",", "")) * Val(txtTerms.Text))
            Next
            Dim dsincome As New DataTable
            dsincome.Columns.Add("a")
            dsincome.Columns.Add("b")
            dsincome.Rows.Add("ESTIMATED INCOME", FormatNumber(gtotal, , , , TriState.True))
            dsincome.Rows.Add("ESTIMATED EXPENSES", FormatNumber(gexpenses, , , , TriState.True))
            dsincome.Rows.Add("EXPECTED PROFIT/LOSS", FormatNumber(gtotal - gexpenses, , , TriState.True, TriState.True))
            dsincome.Rows.Add("PERCENTAGE PROFIT/LOSS", FormatNumber((gtotal - gexpenses) / gtotal * 100, 2) & IIf((gtotal - gexpenses) > 0, "% PROFIT", "% LOSS"))

            gridIncome.DataSource = dsincome
            gridIncome.DataBind()



            con.close()        End Using
    End Sub
    Private Sub Load_school_stats()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.class as Class, studentsummary.classNo as 'No of Students' from class left join studentsummary on class.id = studentsummary.class where studentsummary.session = '" & Session("sessionid") & "' order by studentsummary.session desc", con)
            Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
            Dim ds As New DataTable
            ds.Columns.Add("PARAMETER")
            ds.Columns.Add("NUMBER")

            Dim clas As New ArrayList
            Do While reader2.Read
                If Not clas.Contains(reader2(0)) Then
                    ds.Rows.Add("STUDENTS IN " & reader2.Item(0), reader2.Item(1).ToString)
                    clas.Add(reader2(0))
                End If
            Loop
            reader2.Close()
            Dim cmd40 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from options", con)
            Dim reader40 As MySql.Data.MySqlClient.MySqlDataReader = cmd40.ExecuteReader
            reader40.Read()
            Dim board As Boolean = reader40.Item(0)
            Dim trans As Boolean = reader40.Item(1)
            reader40.Close()
            If board = True Then
                Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from studentsprofile where hostelstay = '" & True & "'", con)
                Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmd3.ExecuteReader
                Dim hostelcount As Integer = 0
                Do While reader3.Read
                    hostelcount = hostelcount + 1
                Loop
                ds.Rows.Add("BOARDERS", hostelcount)
                reader3.Close()
            End If
            If trans = True Then
                Dim cmd6 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from transportfees", con)
                Dim reader6 As MySql.Data.MySqlClient.MySqlDataReader = cmd6.ExecuteReader
                Dim routes As New ArrayList
                Do While reader6.Read
                    routes.Add(reader6(0))
                Loop
                reader6.Close()
                For Each item As String In routes
                    Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from studentsprofile where transport = '" & item & "'", con)
                    Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmd3.ExecuteReader
                    Dim hostelcount As Integer = 0
                    Do While reader3.Read
                        hostelcount = hostelcount + 1
                    Loop
                    ds.Rows.Add("STUDENTS IN " & item & " ROUTE", hostelcount)
                    reader3.Close()
                Next

            End If
            Dim cmd5 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from choicefees", con)
            Dim reader5 As MySql.Data.MySqlClient.MySqlDataReader = cmd5.ExecuteReader

            Dim optionalfees As New ArrayList
            Do While reader5.Read
                optionalfees.Add(reader5(1))
            Loop
            reader5.Close()
            For Each item As String In optionalfees
                Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from feeschedule where fee = '" & item & "' and session = '" & Session("sessionid") & "'", con)
                Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmd3.ExecuteReader
                Dim hostelcount As Integer = 0
                Do While reader3.Read
                    hostelcount = hostelcount + 1
                Loop
                ds.Rows.Add("STUDENTS PAYING FOR " & item, hostelcount)
                reader3.Close()
            Next
            gridSchool.DataSource = ds
            gridSchool.DataBind()
            con.close()        End Using
    End Sub
    Private Sub Load_Accounts()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT classfees.id, class.class, classfees.fee, classfees.amount, classfees.min from classfees inner join class on classfees.class = class.id", con)
            Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
            Dim ds As New DataTable
            ds.Columns.Add("CLASS")
            ds.Columns.Add("FEE")
            ds.Columns.Add("AMOUNT")

            Do While reader2.Read
                ds.Rows.Add(reader2.Item(1).ToString, reader2.Item(2).ToString, FormatNumber(reader2.Item(3).ToString, , , , TriState.True))
            Loop
            reader2.Close()
            gridClassfees.DataSource = ds
            gridClassfees.DataBind()


            Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from optionalfees", con)
            Dim reader22 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
            Dim ds2 As New DataTable
            ds2.Columns.Add("FEE")
            ds2.Columns.Add("AMOUNT")

            Do While reader22.Read
                ds2.Rows.Add(reader22.Item(1).ToString, FormatNumber(reader22.Item(2).ToString, , , , TriState.True))
            Loop
            reader22.Close()



            Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from sessionalfees", con)
            Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmd3.ExecuteReader



            Do While reader3.Read
                ds2.Rows.Add(reader3.Item(1).ToString, FormatNumber(reader3.Item(2).ToString, , , , TriState.True))
            Loop
            reader3.Close()


            Dim cmd4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from onetimefees", con)
            Dim reader4 As MySql.Data.MySqlClient.MySqlDataReader = cmd4.ExecuteReader

            Do While reader4.Read
                ds2.Rows.Add(reader4.Item(1).ToString, FormatNumber(reader4.Item(2).ToString, , , , TriState.True))
            Loop
            reader4.Close()


            Dim cmd40 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from options", con)
            Dim reader40 As MySql.Data.MySqlClient.MySqlDataReader = cmd40.ExecuteReader
            reader40.Read()
            Dim board As Boolean = reader40.Item(0)
            Dim trans As Boolean = reader40.Item(1)
            reader40.Close()



            If board = True Then
                Dim cmd50 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from boarding", con)
                Dim reader50 As MySql.Data.MySqlClient.MySqlDataReader = cmd50.ExecuteReader
                reader50.Read()
                ds2.Rows.Add("BOARDING", FormatNumber(Val(reader50.Item(0).ToString), , , , TriState.True))
                reader50.Close()
            End If

            Dim cmd5 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from choicefees", con)
            Dim reader5 As MySql.Data.MySqlClient.MySqlDataReader = cmd5.ExecuteReader


            Do While reader5.Read
                ds2.Rows.Add(reader5.Item(1).ToString, FormatNumber(reader5.Item(2).ToString, , , , TriState.True))
            Loop
            reader5.Close()



            If trans = True Then


                Dim cmd6 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from transportfees", con)
                Dim reader6 As MySql.Data.MySqlClient.MySqlDataReader = cmd6.ExecuteReader
                Do While reader6.Read
                    ds2.Rows.Add(reader6.Item(0).ToString & " TRANSPORT ROUTE", FormatNumber(reader6.Item(1).ToString, , , , TriState.True))
                Loop
                reader6.Close()

            End If
            gridOtherfees.DataSource = ds2
            gridOtherfees.DataBind()
            Dim cmd2z As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from accsettings where type = ?", con)
            cmd2z.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "stock"))
            Dim reader22z As MySql.Data.MySqlClient.MySqlDataReader = cmd2z.ExecuteReader
            Dim dss As New DataTable
            dss.Columns.Add("ACCOUNT")
            dss.Columns.Add("AMOUNT")
            Do While reader22z.Read
                dss.Rows.Add(reader22z.Item(1).ToString, FormatNumber(0, , , , TriState.True))
            Loop
            reader22z.Close()
            gridStock.DataSource = dss
            gridStock.DataBind()

            con.close()        End Using
    End Sub



    Protected Sub txtTerms_TextChanged(sender As Object, e As EventArgs) Handles txtTerms.TextChanged

    End Sub

    Protected Sub txtSalary_TextChanged(sender As Object, e As EventArgs) Handles txtSalary.TextChanged
        expenses()
    End Sub

    Protected Sub gridSchool_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gridSchool.RowEditing
        Dim ds As New DataTable
        ds.Columns.Add("PARAMETER")
        ds.Columns.Add("NUMBER")
        For Each row As GridViewRow In gridSchool.Rows
            ds.Rows.Add(row.Cells(0).Text, row.Cells(1).Text)
        Next

        gridSchool.EditIndex = e.NewEditIndex
        gridSchool.DataSource = ds
        gridSchool.DataBind()

    End Sub

    Protected Sub gridSchool_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gridSchool.RowUpdating
        Dim row As GridViewRow = gridSchool.Rows(e.RowIndex)

        Dim amount As String = TryCast(row.Cells(1).Controls(0), TextBox).Text.Replace(",", "")
        Dim ds As New DataTable
        ds.Columns.Add("PARAMETER")
        ds.Columns.Add("NUMBER")
        For Each rows As GridViewRow In gridSchool.Rows
            If rows Is row Then
                ds.Rows.Add(rows.Cells(0).Text, amount)
            Else
                ds.Rows.Add(rows.Cells(0).Text, rows.Cells(1).Text)
            End If

        Next

        gridSchool.DataSource = ds


        gridSchool.EditIndex = -1
        gridSchool.DataBind()
        projections()
    End Sub

    Protected Sub gridClassfees_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gridClassfees.RowEditing



        Dim ds As New DataTable
        ds.Columns.Add("class")
        ds.Columns.Add("fee")
        ds.Columns.Add("amount")
        For Each row As GridViewRow In gridClassfees.Rows
            ds.Rows.Add(row.Cells(0).Text, row.Cells(1).Text, row.Cells(2).Text)
        Next

        gridClassfees.EditIndex = e.NewEditIndex
        gridClassfees.DataSource = ds
        gridClassfees.DataBind()

    End Sub

    Protected Sub gridClassfees_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gridClassfees.RowUpdating
        Dim row As GridViewRow = gridClassfees.Rows(e.RowIndex)

        Dim amount As String = TryCast(row.Cells(2).Controls(0), TextBox).Text.Replace(",", "")
        Dim ds As New DataTable
        ds.Columns.Add("class")
        ds.Columns.Add("fee")
        ds.Columns.Add("amount")
        For Each rows As GridViewRow In gridClassfees.Rows
            If rows Is row Then
                ds.Rows.Add(rows.Cells(0).Text, rows.Cells(1).Text, FormatNumber(amount, , , , TriState.True))
            Else
                ds.Rows.Add(rows.Cells(0).Text, rows.Cells(1).Text, rows.Cells(2).Text)
            End If

        Next

        gridClassfees.DataSource = ds


        gridClassfees.EditIndex = -1
        gridClassfees.DataBind()
        projections()
    End Sub

    Protected Sub gridOtherfees_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gridOtherfees.RowEditing

        Dim ds As New DataTable

        ds.Columns.Add("fee")
        ds.Columns.Add("amount")
        For Each row As GridViewRow In gridOtherfees.Rows
            ds.Rows.Add(row.Cells(0).Text, row.Cells(1).Text)
        Next

        gridOtherfees.EditIndex = e.NewEditIndex
        gridOtherfees.DataSource = ds
        gridOtherfees.DataBind()
    End Sub

    Protected Sub gridOtherfees_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gridOtherfees.RowUpdating
        Dim row As GridViewRow = gridOtherfees.Rows(e.RowIndex)

        Dim amount As String = TryCast(row.Cells(1).Controls(0), TextBox).Text.Replace(",", "")
        Dim ds As New DataTable
        ds.Columns.Add("FEE")
        ds.Columns.Add("AMOUNT")
        For Each rows As GridViewRow In gridOtherfees.Rows
            If rows Is row Then
                ds.Rows.Add(rows.Cells(0).Text, FormatNumber(amount, , , , TriState.True))
            Else
                ds.Rows.Add(rows.Cells(0).Text, rows.Cells(1).Text)
            End If

        Next

        gridOtherfees.DataSource = ds


        gridOtherfees.EditIndex = -1
        gridOtherfees.DataBind()
        projections()
    End Sub

    Protected Sub gridStock_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gridStock.RowEditing
        Dim ds As New DataTable

        ds.Columns.Add("account")
        ds.Columns.Add("amount")
        For Each row As GridViewRow In gridStock.Rows
            ds.Rows.Add(row.Cells(0).Text, row.Cells(1).Text)
        Next

        gridStock.EditIndex = e.NewEditIndex
        gridStock.DataSource = ds
        gridStock.DataBind()
    End Sub

    Protected Sub gridStock_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gridStock.RowUpdating
        Dim row As GridViewRow = gridStock.Rows(e.RowIndex)

        Dim amount As String = TryCast(row.Cells(1).Controls(0), TextBox).Text.Replace(",", "")
        Dim ds As New DataTable
        ds.Columns.Add("ACCOUNT")
        ds.Columns.Add("AMOUNT")
        For Each rows As GridViewRow In gridStock.Rows
            If rows Is row Then
                ds.Rows.Add(rows.Cells(0).Text, FormatNumber(amount, , , , TriState.True))
            Else
                ds.Rows.Add(rows.Cells(0).Text, rows.Cells(1).Text)
            End If

        Next

        gridStock.DataSource = ds


        gridStock.EditIndex = -1
        gridStock.DataBind()
        projections()
    End Sub

    Protected Sub gidExpenses_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gidExpenses.RowEditing
        Dim ds As New DataTable

        ds.Columns.Add("EXPENSE")
        ds.Columns.Add("amount")
        For Each row As GridViewRow In gidExpenses.Rows
            ds.Rows.Add(row.Cells(0).Text, row.Cells(1).Text)
        Next

        gidExpenses.EditIndex = e.NewEditIndex
        gidExpenses.DataSource = ds
        gidExpenses.DataBind()
    End Sub

    Protected Sub gidExpenses_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gidExpenses.RowUpdating
        Dim row As GridViewRow = gidExpenses.Rows(e.RowIndex)

        Dim amount As String = TryCast(row.Cells(1).Controls(0), TextBox).Text.Replace(",", "")
        Dim ds As New DataTable
        ds.Columns.Add("EXPENSE")
        ds.Columns.Add("AMOUNT")
        For Each rows As GridViewRow In gidExpenses.Rows
            If rows Is row Then
                ds.Rows.Add(rows.Cells(0).Text, FormatNumber(amount, , , , TriState.True))
            Else
                ds.Rows.Add(rows.Cells(0).Text, rows.Cells(1).Text)
            End If

        Next

        gidExpenses.DataSource = ds


        gidExpenses.EditIndex = -1
        gidExpenses.DataBind()
        projections()
    End Sub

    Protected Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

    End Sub
End Class
