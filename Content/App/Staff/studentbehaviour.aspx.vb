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
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("staffid") = Nothing Then            Dim x As ArrayList = check.Check_app_staff(Request.QueryString("username"), Request.QueryString("password"))            If x.Count <> 0 Then                Session("staffid") = x.Item(0)                Session("sessionid") = x.Item(1)            End If        End If
        Try

            If Not IsPostBack Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.Open()
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
                    Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.passport, studentsprofile.admno, studentsprofile.surname from studentsummary inner join studentsprofile on studentsummary.student = studentsprofile.admno inner join class on studentsummary.class = class.id WHERE studentsummary.session = '" & Session("sessionid") & "' and class.class = ?", con)
                    cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", DropDownList1.Text))
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
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





    Private Sub Student_Details()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport from StudentsProfile where admno = ?", con)
            cmdLoad4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("studentadd")))
            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
            Dim pass As String = ""
            If student.Read() Then pass = student.Item("passport").ToString
            student.Close()
            If Not pass = "" Then Image1.ImageUrl = pass
            Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.surname, class.class, studentsummary.trans, studentsummary.status, studentsummary.class, studentsprofile.sex From Studentsummary INNER JOIN Studentsprofile on studentsummary.student = studentsprofile.admno inner join class on studentsummary.class = class.Id WHERE StudentSummary.student = ? And StudentSummary.Session = ?", con)
            cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class.ID", Session("Studentadd")))
            cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter(" StudentSummary.Session ", Session("SessionID")))
            Dim studentsReader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader()
            studentsReader.Read()
            lblClass.Text = studentsReader.Item(0).ToString
            Session("ClassId") = studentsReader.Item(4)
            Dim sex As String = studentsReader.Item("sex").ToString
            studentsReader.Close()
            Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from studentsummary where student = ? and class = ? and session = ? ", con)
            cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", Session("Studentadd")))
            cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("ClassId")))
            cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("term", Session("Sessionid")))
            Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader()
            reader2.Read()
            txtRem.Text = reader2("classTeacherRemarks").ToString

            lblPresent.Text = reader2("present").ToString
            lblAbsent.Text = reader2("absent").ToString
            lblOpened.Text = Val(lblPresent.Text) + Val(lblAbsent.Text)
            lblPercent.Text = FormatNumber((Val(lblPresent.Text) / Val(lblOpened.Text)) * 100, 2) & "%"
            reader2.Close()
            Dim a As New DataTable
            a.Columns.Add("ID")
            a.Columns.Add("trait")
            a.Columns.Add("value")
            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select termtraits.ID, traits.trait, termtraits.value from termtraits inner join traits on traits.id = termtraits.trait where termtraits.student = '" & Session("studentadd") & "' and termtraits.session = '" & Session("sessionid") & "'", con)
            Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
            Do While msg.Read()
                a.Rows.Add(msg.Item(0), msg.Item(1), msg.Item(2).ToString)
            Loop
            Gridview2.DataSource = a
            Gridview2.Columns(0).Visible = False
            Gridview2.DataBind()
            msg.Close()
            con.close()        End Using
        panel3.Visible = True
        pnlAll.Visible = False
        
    End Sub

    Protected Sub gridview1_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gridview1.PageIndexChanging
        Try


            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
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
                con.close()            End Using
            gridview1.PageIndex = e.NewPageIndex
            gridview1.DataBind()
            Session("studentadd") = Nothing
            panel3.Visible = False
            gridview1.SelectedIndex = -1
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
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



    Protected Sub Gridview2_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles Gridview2.RowCancelingEdit
        Try
            Gridview2.EditIndex = -1
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim a As New DataTable
                a.Columns.Add("ID")
                a.Columns.Add("trait")
                a.Columns.Add("value")
                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select termtraits.ID, traits.trait, termtraits.value from termtraits inner join traits on traits.id = termtraits.trait where termtraits.student = '" & Session("studentadd") & "' and termtraits.session = '" & Session("sessionid") & "'", con)
                Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                Do While msg.Read()
                    a.Rows.Add(msg.Item(0), msg.Item(1), msg.Item(2).ToString)
                Loop
                Gridview2.DataSource = a
                Gridview2.Columns(0).Visible = False
                Gridview2.DataBind()
                msg.Close()
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview2_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles Gridview2.RowEditing
        Try
            Gridview2.EditIndex = e.NewEditIndex
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim a As New DataTable
                a.Columns.Add("ID")
                a.Columns.Add("trait")
                a.Columns.Add("value")
                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select termtraits.ID, traits.trait, termtraits.value from termtraits inner join traits on traits.id = termtraits.trait where termtraits.student = '" & Session("studentadd") & "' and termtraits.session = '" & Session("sessionid") & "'", con)
                Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                Do While msg.Read()
                    a.Rows.Add(msg.Item(0), msg.Item(1), msg.Item(2).ToString)
                Loop
                Gridview2.DataSource = a
                Gridview2.Columns(0).Visible = True
                Gridview2.DataBind()
                msg.Close()
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try

    End Sub

    Protected Sub Gridview2_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles Gridview2.RowUpdating
        Try
            Dim row As GridViewRow = Gridview2.Rows(e.RowIndex)
            Dim id As String = row.Cells(0).Text
            Dim trait As String = TryCast(row.Cells(2).Controls(0), TextBox).Text
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim enter As New MySql.Data.MySqlClient.MySqlCommand("Update termtraits set value = '" & trait & "' where id = '" & id & "'", con)
                enter.ExecuteNonQuery()
                Gridview2.EditIndex = -1
                Dim a As New DataTable
                a.Columns.Add("ID")
                a.Columns.Add("trait")
                a.Columns.Add("value")
                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select termtraits.ID, traits.trait, termtraits.value from termtraits inner join traits on traits.id = termtraits.trait where termtraits.student = '" & Session("studentadd") & "' and termtraits.session = '" & Session("sessionid") & "'", con)
                Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                Do While msg.Read()
                    a.Rows.Add(msg.Item(0), msg.Item(1), msg.Item(2).ToString)
                Loop
                Gridview2.DataSource = a
                Gridview2.Columns(0).Visible = False
                Gridview2.DataBind()
                msg.Close()
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

    Protected Sub txtRem_SelectedIndexChanged(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("Update studentsummary set classTeacherRemarks = ? where Student = ? and Class = ? and Session = ? ", con)
                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("remarks", txtRem.Text))
                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", Session("Studentadd")))
                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("ClassId")))
                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("term", Session("sessionid")))
                cmd.ExecuteNonQuery()
                con.Close()            End Using
            Show_Alert(True, "Update successful")
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim ds As New DataTable
                ds.Columns.Add("passport")
                ds.Columns.Add("staffname")
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.passport, studentsprofile.admno, studentsprofile.surname from studentsummary inner join studentsprofile on studentsummary.student = studentsprofile.admno inner join class on studentsummary.class = class.id WHERE studentsummary.session = '" & Session("sessionid") & "' and class.class = ?", con)
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", DropDownList1.Text))
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
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
End Class
