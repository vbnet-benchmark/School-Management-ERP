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
                panel3.Visible = False
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.Open()
                    Dim ds As New DataTable
                    ds.Columns.Add("passport")
                    ds.Columns.Add("staffname")
                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport, parentid, parentname from parentprofile", con)
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                    Do While student0.Read
                        ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
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
            If student1.Item("activated") = True Then
                chkActivated.Checked = True
                chkActivated.Text = "Activated"
            Else
                chkActivated.Checked = False
                chkActivated.Text = "Deactivated"
            End If
            student1.Close()

            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentid as 'Parent ID', parentname as 'Name', sex as Sex, phone as 'Phone No', address as 'Address', email as 'Email' from parentprofile where parentid = ?", con)
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
            If Not pass = "" Then Image1.ImageUrl = pass

            Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.surname from parentward inner join studentsprofile on parentward.ward = studentsprofile.admno WHERE parentward.parent = ?", con)
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
            gridview1.SelectedIndex = -1
            con.close()        End Using
        pnlAll.Visible = False
        panel3.Visible = True
    End Sub
    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click

        Session("edit") = "parentprofile"
        Response.Redirect("~/content/App/Admin/addparent.aspx")

    End Sub





    Protected Sub LinkButton4_Click(sender As Object, e As EventArgs) Handles LinkButton4.Click
        Session("edit") = "passport"
        Response.Redirect("~/content/App/Admin/addparent.aspx")
    End Sub


    Protected Sub DetailsView1_PageIndexChanging(sender As Object, e As DetailsViewPageEventArgs) Handles DetailsView1.PageIndexChanging

    End Sub

    Protected Sub gridview1_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gridview1.PageIndexChanging
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim ds As New DataTable
                ds.Columns.Add("passport")
                ds.Columns.Add("staffname")
                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport, parentid, parentname from parentprofile", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                Do While student0.Read
                    ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                Loop
                student0.Close()
                gridview1.DataSource = ds
                gridview1.DataBind()
                con.close()            End Using
            gridview1.PageIndex = e.NewPageIndex
            gridview1.DataBind()
            Session("parentadd") = Nothing
            panel3.Visible = False
            gridview1.SelectedIndex = -1
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try

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


    Protected Sub chkActivated_CheckedChanged(sender As Object, e As EventArgs) Handles chkActivated.CheckedChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim active As Boolean = -Val(chkActivated.Checked)
                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update parentprofile Set activated = ? Where parentID = ?", con)
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("active", active))
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", Session("parentadd")))
                cmdCheck3.ExecuteNonQuery()

                If active = False Then
                    logify.log(Session("staffid"), "Parent - " & Session("parentadd") & " was deactivated")
                Else
                    logify.log(Session("staffid"), "Parent - " & Session("parentadd") & " was deactivated")
                End If
                con.Close()
            End Using
            Staff_Details()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub lnkmsg_Click(sender As Object, e As EventArgs) Handles lnkmsg.Click
        Try
            Session("receivert") = "Parent"
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentname from parentprofile where parentid = ?", con)
                cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("parentadd")))
                Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                student.Read()
                Session("receiver") = student.Item(0).ToString
                student.Close()
                con.close()            End Using
            Response.Redirect("~/content/App/Admin/personnewmg.aspx")
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub lnkPwd_Click(sender As Object, e As EventArgs) Handles lnkPwd.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update parentprofile Set password = ? where parentID = ?", con)
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("password", "password"))
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", Session("parentadd")))
                cmdCheck3.ExecuteNonQuery()
                Show_Alert(True, "Password reset successfully")
                logify.log(Session("staffid"), "The password of " & Session("parentadd") & " was reset")
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        pnlAll.Visible = True
        panel3.Visible = False
    End Sub

    Protected Sub LinkButton5_Click(sender As Object, e As EventArgs) Handles LinkButton5.Click
        Session("edit") = Nothing
        Response.Redirect("~/content/App/Admin/addparent.aspx")
    End Sub

    Protected Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim ds As New DataTable
                ds.Columns.Add("passport")
                ds.Columns.Add("staffname")
                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport, parentid, parentname from parentprofile", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                Do While student0.Read
                    ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                Loop
                student0.Close()
                gridview1.DataSource = ds
                gridview1.DataBind()
                con.close()            End Using
            gridview1.DataBind()
            Session("parentadd") = Nothing
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
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim ds As New DataTable
                ds.Columns.Add("passport")
                ds.Columns.Add("staffname")
                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport, parentid, parentname from parentprofile", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                Do While student0.Read
                    ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                Loop
                student0.Close()
                gridview1.DataSource = ds
                gridview1.DataBind()
                con.close()            End Using
            Session("parentadd") = Nothing
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
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try

    End Sub
End Class
