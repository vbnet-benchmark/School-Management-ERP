Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Imports System
Imports System.Reflection.Emit
Imports System.Collections
Imports Microsoft.Win32
Partial Class Admin_studentprofile
    Inherits System.Web.UI.Page
    Dim pagebefore As String
    Dim studentId As Integer
    Dim termID As Integer
    Dim checkedSubjects As New ArrayList
    Dim uncheckedSubjects As New ArrayList
    Dim loadedsubs As Boolean = False
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
        If check.Check_Class(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try
            If IsPostBack Then
            Dim ct As Integer = 1
                If Session("grades") <> Nothing Then
                    Dim thisarray As New ArrayList
                    thisarray = CType(Session("subs"), ArrayList)
                    

                    For s = 1 To Session("grades")

                        Options_Recreate(s, thisarray(s - 1))

                    Next
                End If
            Else
            Session("grades") = Nothing
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                    con.open()
                    Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.class from classteacher inner join class on class.id = classteacher.class where classteacher .teacher = ?", con)
                    cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("staffid")))
                    Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                    Do While student.Read
                        DropDownList1.Items.Add(student.Item(0).ToString)
                    Loop
                    panel3.Visible = False
                    student.Close()
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
                    con.close()
                End Using

               
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))

        End Try
    End Sub

    Protected Sub gridview1_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles gridview1.SelectedIndexChanging
        Try


            Dim x As Array = Split(gridview1.Rows(e.NewSelectedIndex).Cells(1).Text, " - ")
            Session("studentadd") = RTrim(x(0))
            Student_Details()
            gridview1.SelectedIndex = -1
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))

        End Try
    End Sub

    Protected Sub Student_Details()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport from StudentsProfile where admno = ?", con)
            cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("studentadd")))
            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
            Dim pass As String
            student.Read()
            pass = student.Item("passport").ToString
            student.Close()
            If pass <> "" Then Image1.ImageUrl = pass

            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT pergroups.frequency from indicators inner join (pergroups inner join class on pergroups.id = class.pergrp) on indicators.pergrp = pergroups.id where class.class = '" + dropdownlist1.text & "'", con)
            Dim students As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            cboIndicator.Items.Clear()
            cboIndicator.Items.Add("Select")
            Dim freq As String
            Do While students.Read()
                freq = students.item(0).toString()
            Loop
            Select Case freq
                Case "Daily"
                    For i As Integer = 1 To 100
                        cboIndicator.items.add("Day " & i)
                    Next
                Case "Weekly"
                    For i As Integer = 1 To 14
                        cboIndicator.items.add("Week " & i)
                    Next
                Case "Monthly"
                    For i As Integer = 1 To 3
                        cboIndicator.items.add("Month " & i)
                    Next
            End Select
            cboIndicator.Visible = True
            students.Close()

            pnlAll.Visible = False
            panel3.Visible = True
            con.Close()
        End Using
    End Sub

    Protected Sub cboAspect_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboIndicator.SelectedIndexChanged
        Try
        Dim db As New DB_Interface()
        Dim a As Array = db.Select_Query("SELECT indicators.indicator, pergroups.low, pergroups.high from indicators inner join (pergroups inner join class on pergroups.id = class.pergrp) on indicators.pergrp = pergroups.id where class.class = '" + dropdownlist1.text & "'")
        Dim count As Integer = 1
        Dim subs As New ArrayList
        For j As Integer = 0 To a.GetLength(1) - 2
                Dim value As String = ""
                 subs.Add(count & ". " & a(0,j))
                If db.Select_Single("select value from termind inner join indicators on indicators.id = termind.indicator where student = '" & Session("StudentAdd") & "' and session = '" & Session("SessionId") & "' and indicators.indicator = '" & a(0, j) & "' and period = '" & cboIndicator.Text & "'") = "" Then
                Options_create(count, a(1, j), a(2, j), a(0, j))
            Else
                value = db.Select_Single("select value from termind inner join indicators on indicators.id = termind.indicator where student = '" & Session("StudentAdd") & "' and session = '" & Session("SessionId") & "' and indicators.indicator = '" & a(0, j) & "' and period = '" & cboIndicator.Text & "'")
                Options_create(count, a(1, j), a(2, j), a(0, j), value)
               
            End If

            count += 1
        Next
        Session("grades") = count - 1
        Session("subs") = subs
        Catch ex As Exception
        Show_Alert(False, logify.error_log("Option", Replace(ex.ToString, "'", "")))

        End Try
    End Sub
    Sub Options_create(count As Integer, low As Integer, high As Integer, indicator As String, Optional value As String = "")
        Try
            Dim l As New System.Web.UI.WebControls.Label
            l.ID = "labels" & count & count
            l.Text = count & ". " & indicator
            Dim sx As New LiteralControl
            sx.Text = "<div class='row' id='cf" & count & "' style='padding-left:8.33%;'><div class = 'col-lg-1'></div><br/><div class='col-lg-11'><label id='" & count & "er" & "' style='text-align:left; font-size:16px;' class='login2'>"
            Dim sxend As New LiteralControl
            sxend.Text = "</label></div></div>"


            Dim sxq As New LiteralControl
            sxq.Text = "<div class='row'  id='cf" & count & "' style='padding-left:8.33%;'><div class = 'col-lg-1'></div><br/><div class='col-lg-11'><label id='" & count & "er" & "' style='text-align:left; font-size:14px;' class='login2'>"
            Dim sxqend As New LiteralControl
            sxqend.Text = "</label></div></div>"

            Dim options As New DropDownList
            options.ID = "option" & count
            options.CssClass = "form-control custom-select-value"
            options.Items.Add("Select")
            For i As Integer = Val(low) To val(high)
                options.Items.Add(i)
            Next

            If value <> "" Then
                options.SelectedIndex = val(value + 1)
            End If

            plcC.Controls.Add(sx)
            plcC.Controls.Add(l)
            plcC.Controls.Add(sxend)

            plcC.Controls.Add(sxq)
            plcC.Controls.Add(options)
            plcC.Controls.Add(sxqend)

        Catch ex As Exception
            Show_Alert(False, logify.error_log("Option", Replace(ex.ToString, "'", "")))

        End Try

    End Sub


    Sub Options_Recreate(count As Integer, txt As String)
        Try
            Dim l As New System.Web.UI.WebControls.Label
            l.ID = "labels" & count & count
            l.Text = txt
            Dim sx As New LiteralControl
            sx.Text = "<div class='row'  id='cf" & count & "' style='padding-left:8.33%;'><div class = 'col-lg-1'></div><br/><div class='col-lg-11'><label id='" & count & "er" & "' style='text-align:left; font-size:16px;' class='login2'>"
            Dim sxend As New LiteralControl
            sxend.Text = "</label></div></div>"

            Dim sxq As New LiteralControl
            sxq.Text = "<div class='row'  id='cf" & count & "' style='padding-left:8.33%;'><div class = 'col-lg-1'></div><br/><div class='col-lg-11'><label id='" & count & "er" & "' style='text-align:left; font-size:14px;' class='login2'>"
            Dim sxqend As New LiteralControl
            sxqend.Text = "</label></div></div>"

            Dim options As New DropDownList
            options.CssClass = "form-control custom-select-value"
            options.ID = "option" & count
Dim db As New DB_Interface()
 options.Items.Add("Select")
   for x as integer = db.Select_single("SELECT pergroups.low from indicators inner join (pergroups inner join class on pergroups.id = class.pergrp) on indicators.pergrp = pergroups.id where class.class = '" + dropdownlist1.text & "'") to db.Select_single("SELECT pergroups.high from indicators inner join (pergroups inner join class on pergroups.id = class.pergrp) on indicators.pergrp = pergroups.id where class.class = '" + dropdownlist1.text & "'") 

            options.Items.Add(x)
           
    next
            plcC.Controls.Add(sx)
            plcC.Controls.Add(l)
            plcC.Controls.Add(sxend)

            plcC.Controls.Add(sxq)
            plcC.Controls.Add(options)
            plcC.Controls.Add(sxqend)


        Catch ex As Exception
            Show_Alert(False, logify.error_log("Option", Replace(ex.ToString, "'", "")))

        End Try

    End Sub


    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        Try


            Dim db As New DB_Interface()
            If db.Select_Single("select Count(*) from termind where session = '" & Session("sessionid") & "' and student = '" & session("StudentAdd") & "' and period = '" & cboIndicator.text & "' LIMIT 1") = 0 Then
                    Dim a As ArrayList = db.Select_1D("SELECT indicators.id from indicators inner join (pergroups inner join class on pergroups.id = class.pergrp) on indicators.pergrp = pergroups.id where class.class = '" + dropdownlist1.text & "'")
                    For s = 1 To Session("grades")
                    
                    Dim o As DropDownList = plcC.FindControl("option" & s)
                     
                   if o.SelectedIndex <> 0 Then
                  
                   db.Non_Query("insert into termind (session, student, indicator, period, value) Values ('" & Session("sessionid") & "', '" & session("StudentAdd") & "', '" & a(s -1) & "', '" & cboIndicator.Text & "', '" & o.Text & "')")

                   End If
                   
                Next
                else
                  Dim a As ArrayList = db.Select_1D("SELECT indicators.id from indicators inner join (pergroups inner join class on pergroups.id = class.pergrp) on indicators.pergrp = pergroups.id where class.class = '" + dropdownlist1.text & "'")
                    For s = 1 To Session("grades") 
                    Dim o As DropDownList = plcC.FindControl("option" & s )
                     
                if o.SelectedIndex <> 0 Then

                db.Non_Query("UPDATE termind SET value = '" & o.Text & "' WHERE session = '" & Session("sessionid") & "' AND student = '" & Session("StudentAdd") & "' AND indicator = '" & a(s - 1) & "' AND period = '" & cboIndicator.Text & "'")

            End If
            Next
            End if
                
               
                logify.log(Session("staffid"), "The performance of " & par.getstuname(Session("studentadd")) & " for " & cboIndicator.Text & " was updated")
                Show_Alert(True, "Performance Updated")
              
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))

        End Try
    End Sub
    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        pnlAll.Visible = True
        panel3.Visible = False
        gridview1.SelectedIndex = -1
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
                con.close()
            End Using

            Session("studentadd") = Nothing
            gridview1.SelectedIndex = -1
            panel3.Visible = False
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub



End Class
