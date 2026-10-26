Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls



Partial Class Admin_staffprofile
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
                    Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.class from classteacher inner join class on class.id = classteacher.class where classteacher.teacher = ?", con)
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
                    Dim parents As New ArrayList
                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentprofile.passport, parentprofile.parentid, parentprofile.parentname from parentward inner join parentprofile on parentward.parent = parentprofile.parentid inner join (studentsprofile inner join (studentsummary inner join class on studentsummary.class = class.id) on studentsprofile.admno = studentsummary.student) on parentward.ward = studentsprofile.admno  where class.class = '" & DropDownList1.Text & "' and studentsummary.session = '" & Session("sessionid") & "'", con)
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                    Do While student0.Read
                        If Not parents.Contains(student0(1).ToString) Then
                            parents.Add(student0(1).ToString)
                            ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                        End If
                    Loop
                    student0.Close()
                    gridview1.DataSource = ds
                    gridview1.DataBind()
                    con.Close()                End Using
                If Session("parentadd") <> Nothing Then
                    Staff_Details()
                End If
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try

    End Sub
    Protected Sub Staff_Details()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdLoad10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport, activated from parentprofile where parentid = ?", con)
            cmdLoad10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("parentadd")))
            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10.ExecuteReader
            Dim pass As String
            student1.Read()
            pass = student1.Item("passport").ToString

            student1.Close()

            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentprofile.parentid as 'Parent ID', parentprofile.parentname as 'Name', parentprofile.sex as Sex, parentprofile.phone as 'Phone No', parentprofile.address as 'Address', parentprofile.email as 'Email' from parentward inner join parentprofile on parentward.parent = parentprofile.parentid inner join (studentsprofile inner join (studentsummary inner join class on studentsummary.class = class.id) on studentsprofile.admno = studentsummary.student) on parentward.ward = studentsprofile.admno  where class.class = '" & DropDownList1.Text & "' and parentprofile.parentid = ?", con)
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("parentadd")))
            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            student.Read()
            student.Close()
            Dim ds As New DataTable
            Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
            adapter1.SelectCommand = cmdLoad1
            adapter1.Fill(ds)
            DetailsView1.DataSource = ds
            DetailsView1.DataBind()
            If pass = "" Then
                pass = "~/image/noimage.jpg"
            End If
            Image1.ImageUrl = pass
            Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.surname from parentward inner join parentprofile on parentward.parent = parentprofile.parentid inner join (studentsprofile inner join (studentsummary inner join class on studentsummary.class = class.id) on studentsprofile.admno = studentsummary.student) on parentward.ward = studentsprofile.admno  where class.class = '" & DropDownList1.Text & "' and parentprofile.parentid = ? and studentsummary.session = '" & Session("sessionid") & "'", con)
            cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Session("parentAdd")))
            Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
            Dim count As Integer = 1
            Do While reader2.Read
                Dim subjects As New Label
                If FindControl("cbosubject" & count) Is Nothing Then
                    subjects.ID = "cbosubject" & count
                End If
                subjects.Text = count & ".   " & reader2.Item(0)
                taught.Controls.Add(subjects)
                Dim MyLiteral = New LiteralControl
                MyLiteral.Text = "<BR/>"
                taught.Controls.Add(MyLiteral)
                count = count + 1
            Loop
            reader2.Close()


            con.close()        End Using
        pnlAll.Visible = False
        panel3.Visible = True
    End Sub

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        pnlAll.Visible = True
        panel3.Visible = False
        gridview1.SelectedIndex = -1
    End Sub





    Protected Sub DetailsView1_PageIndexChanging(sender As Object, e As DetailsViewPageEventArgs) Handles DetailsView1.PageIndexChanging

    End Sub

    Protected Sub gridview1_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles gridview1.SelectedIndexChanging
        Try


            Dim x As Array = Split(gridview1.Rows(e.NewSelectedIndex).Cells(1).Text, " - ")
            Session("parentadd") = RTrim(x(0))
            Staff_Details()
            pnlAll.Visible = False
            gridview1.SelectedIndex = -1
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
                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport, parentid, parentname from parentprofile where parentname like '" & s & "' or parentid = '" & txtSearch.Text & "'", con)
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



    Protected Sub lnkmsg_Click(sender As Object, e As EventArgs) Handles lnkmsg.Click
        Try
            Session("receivert") = DropDownList1.Text & " Parent"
            Session("msgclass") = DropDownList1.Text
            Session("receive") = "Parent"
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentname from parentprofile where parentid = ?", con)
                cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("parentadd")))
                Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                student.Read()
                Session("receiver") = student.Item(0).ToString
                student.Close()
                con.close()            End Using
            Response.Redirect("~/content/app/staff/personnewmg.aspx")
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub


    Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            panel3.Visible = False
            Dim ds As New DataTable
            ds.Columns.Add("passport")
            ds.Columns.Add("staffname")
            Dim parents As New ArrayList
            Dim cmdLoad0x As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentprofile.passport, parentprofile.parentid, parentprofile.parentname from parentward inner join parentprofile on parentward.parent = parentprofile.parentid inner join (studentsprofile inner join (studentsummary inner join class on studentsummary.class = class.id) on studentsprofile.admno = studentsummary.student) on parentward.ward = studentsprofile.admno  where class.class = '" & DropDownList1.Text & "'", con)
            Dim student0x As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0x.ExecuteReader
            Do While student0x.Read
                If Not parents.Contains(student0x(1).ToString) Then
                    parents.Add(student0x(1).ToString)
                    ds.Rows.Add(student0x.Item(0).ToString, student0x.Item(1) & " - " & student0x.Item(2).ToString)
                End If
            Loop
            student0x.Close()
            gridview1.DataSource = ds
            gridview1.DataBind()
            con.close()        End Using
    End Sub
End Class
