Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Admin_allstudents
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


            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id, cast(date as char) as date, sender, sendertype, subject, status from messages where receiver = '" & "Admin" & "'  order by date desc", con)
                Dim ds As New DataTable
                ds.Columns.Add("id")
                ds.Columns.Add("date")
                ds.Columns.Add("sender")
                ds.Columns.Add("sendertype")
                ds.Columns.Add("subject")
                ds.Columns.Add("status")
                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Dim id As New ArrayList
                Dim dates As New ArrayList
                Dim sendere As New ArrayList
                Dim sendertype As New ArrayList
                Dim subject As New ArrayList
                Dim status As New ArrayList
                Dim sendername As New ArrayList
                Dim senderrel As New ArrayList
                Do While reader.Read
                    id.Add(reader.Item(0))
                    dates.Add(reader.Item(1))
                    sendere.Add(reader.Item(2))
                    sendertype.Add(reader.Item(3))
                    subject.Add(reader.Item(4))
                    status.Add(reader.Item(5))
                Loop
                reader.Close()
                Dim x As Integer
                For Each item As String In id
                    If sendertype(x) = "Student" Then
                        Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from studentsprofile where admno = '" & sendere(x) & "'", con)
                        Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                        student.Read()
                        sendername.Add(student.Item(0))
                        student.Close()
                        Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                        Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                        student11.Read()
                        senderrel.Add(student11.Item(0))
                        student11.Close()
                    ElseIf sendertype(x) = "Staff" Then
                        Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                        Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                        student1.Read()
                        sendername.Add(student1.Item(0))
                        student1.Close()
                        Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                        Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                        student11.Read()
                        senderrel.Add(student11.Item(0))
                        student11.Close()
                    ElseIf sendertype(x) = "Parent" Then
                        Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentname from parentprofile where parentid = '" & sendere(x) & "'", con)
                        Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                        student1.Read()
                        sendername.Add(student1.Item(0))
                        student1.Close()
                        Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                        Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                        student11.Read()
                        senderrel.Add(student11.Item(0))
                        student11.Close()

                    ElseIf sendertype(x) = "Accounts" Then
                        Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                        Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                        If student1.Read() Then
                            sendername.Add(student1.Item(0))
                        Else
                            sendername.Add("Super Admin")
                        End If
                        student1.Close()
                        senderrel.Add("Accountant")
                    End If
                    ds.Rows.Add(item, Convert.ToDateTime(dates(x)).ToString("dd/MM/yyyy hh:mm tt"), sendername(x), senderrel(x), subject(x), status(x))
                    x = x + 1
                Next
                GridView1.DataSource = ds
                GridView1.DataBind()
                If GridView1.PageIndex = GridView1.PageCount - 1 Then
                    btnNext.Visible = False
                Else
                    btnNext.Visible = True
                End If
                If GridView1.PageIndex = 0 Then
                    btnPrevious.Visible = False
                Else
                    btnPrevious.Visible = True
                End If

                con.Close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id, cast(date as char) as date, sender, sendertype, subject, status from messages where receivertype = '" & "Admin" & "'  order by date desc", con)
                Dim ds As New DataTable
                ds.Columns.Add("id")
                ds.Columns.Add("date")
                ds.Columns.Add("sender")
                ds.Columns.Add("sendertype")
                ds.Columns.Add("subject")
                ds.Columns.Add("status")
                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Dim id As New ArrayList
                Dim dates As New ArrayList
                Dim sendere As New ArrayList
                Dim sendertype As New ArrayList
                Dim subject As New ArrayList
                Dim status As New ArrayList
                Dim sendername As New ArrayList
                Do While reader.Read
                    id.Add(reader.Item(0))
                    dates.Add(reader.Item(1))
                    sendere.Add(reader.Item(2))
                    sendertype.Add(reader.Item(3))
                    subject.Add(reader.Item(4))
                    status.Add(reader.Item(5))
                Loop
                reader.Close()
                Dim x As Integer
                For Each item As String In id
                    If sendertype(x) = "Student" Then
                        Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from studentsprofile where admno = '" & sendere(x) & "'", con)
                        Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                        student.Read()
                        sendername.Add(student.Item(0).ToString)
                        student.Close()
                    ElseIf sendertype(x) = "Staff" Then
                        Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                        Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                        student1.Read()
                        sendername.Add(student1.Item(0).ToString)
                        student1.Close()
                    ElseIf sendertype(x) = "Parent" Then
                        Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentname from parentprofile where parentid = '" & sendere(x) & "'", con)
                        Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                        student1.Read()
                        sendername.Add(student1.Item(0).ToString)
                        student1.Close()
                    End If
                    ds.Rows.Add(item, Convert.ToDateTime(dates(x)).ToString("dd/MM/yyyy hh:mm tt"), sendername(x), sendertype(x), subject(x), status(x))
                    x = x + 1
                Next
                GridView1.DataSource = ds
                GridView1.DataBind()
                If GridView1.PageIndex = GridView1.PageCount - 1 Then
                    btnNext.Visible = False
                Else
                    btnNext.Visible = True
                End If
                If GridView1.PageIndex = 0 Then
                    btnPrevious.Visible = False
                Else
                    btnPrevious.Visible = True
                End If
                con.close()            End Using


            If GridView1.PageIndex + 1 <= GridView1.PageCount Then
                GridView1.PageIndex = GridView1.PageIndex + 1
                GridView1.DataBind()
                If GridView1.PageIndex = GridView1.PageCount - 1 Then
                    btnNext.Visible = False
                Else
                    btnNext.Visible = True
                End If
                If GridView1.PageIndex = 0 Then
                    btnPrevious.Visible = False
                Else
                    btnPrevious.Visible = True
                End If
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub GridView1_PageIndexChanged(sender As Object, e As EventArgs) Handles GridView1.PageIndexChanged

    End Sub

    Protected Sub GridView1_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id, cast(date as char) as date, sender, sendertype, subject, status from messages where receivertype = '" & "Admin" & "'  order by date desc", con)
                Dim ds As New DataTable
                ds.Columns.Add("id")
                ds.Columns.Add("date")
                ds.Columns.Add("sender")
                ds.Columns.Add("sendertype")
                ds.Columns.Add("subject")
                ds.Columns.Add("status")
                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Dim id As New ArrayList
                Dim dates As New ArrayList
                Dim sendere As New ArrayList
                Dim sendertype As New ArrayList
                Dim subject As New ArrayList
                Dim status As New ArrayList
                Dim sendername As New ArrayList
                Do While reader.Read
                    id.Add(reader.Item(0))
                    dates.Add(reader.Item(1))
                    sendere.Add(reader.Item(2))
                    sendertype.Add(reader.Item(3))
                    subject.Add(reader.Item(4))
                    status.Add(reader.Item(5))
                Loop
                reader.Close()
                Dim x As Integer
                For Each item As String In id
                    If sendertype(x) = "Student" Then
                        Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from studentsprofile where admno = '" & sendere(x) & "'", con)
                        Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                        student.Read()
                        sendername.Add(student.Item(0).ToString)
                        student.Close()
                    ElseIf sendertype(x) = "Staff" Then
                        Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                        Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                        student1.Read()
                        sendername.Add(student1.Item(0).ToString)
                        student1.Close()
                    ElseIf sendertype(x) = "Parent" Then
                        Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentname from parentprofile where parentid = '" & sendere(x) & "'", con)
                        Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                        student1.Read()
                        sendername.Add(student1.Item(0).ToString)
                        student1.Close()
                    End If
                    ds.Rows.Add(item, Convert.ToDateTime(dates(x)).ToString("dd/MM/yyyy hh:mm tt"), sendername(x), sendertype(x), subject(x), status(x))
                    x = x + 1
                Next
                GridView1.DataSource = ds
                GridView1.DataBind()
                If GridView1.PageIndex = GridView1.PageCount - 1 Then
                    btnNext.Visible = False
                Else
                    btnNext.Visible = True
                End If
                If GridView1.PageIndex = 0 Then
                    btnPrevious.Visible = False
                Else
                    btnPrevious.Visible = True
                End If
                con.close()            End Using


            GridView1.PageIndex = e.NewPageIndex
            GridView1.DataBind()

            If GridView1.PageIndex = GridView1.PageCount - 1 Then
                btnNext.Visible = False
            Else
                btnNext.Visible = True
            End If
            If GridView1.PageIndex = 0 Then
                btnPrevious.Visible = False
            Else
                btnPrevious.Visible = True
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub GridView1_SelectedIndexChanged1(sender As Object, e As EventArgs) Handles GridView1.SelectedIndexChanged

    End Sub

    Protected Sub GridView1_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs)

    End Sub

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id, cast(date as char) as date, sender, sendertype, subject, status from messages where receivertype = '" & "Admin" & "' and subject like ? order by date desc", con)
                cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("like", "%" & txtSearch.Text & "%"))
                Dim ds As New DataTable
                ds.Columns.Add("id")
                ds.Columns.Add("date")
                ds.Columns.Add("sender")
                ds.Columns.Add("sendertype")
                ds.Columns.Add("subject")
                ds.Columns.Add("status")
                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Dim id As New ArrayList
                Dim dates As New ArrayList
                Dim sendere As New ArrayList
                Dim sendertype As New ArrayList
                Dim subject As New ArrayList
                Dim status As New ArrayList
                Dim sendername As New ArrayList
                Do While reader.Read
                    id.Add(reader.Item(0))
                    dates.Add(reader.Item(1))
                    sendere.Add(reader.Item(2))
                    sendertype.Add(reader.Item(3))
                    subject.Add(reader.Item(4))
                    status.Add(reader.Item(5))
                Loop
                reader.Close()
                Dim x As Integer
                For Each item As String In id
                    If sendertype(x) = "Student" Then
                        Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from studentsprofile where admno = '" & sendere(x) & "'", con)
                        Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                        student.Read()
                        sendername.Add(student.Item(0))
                        student.Close()
                    ElseIf sendertype(x) = "Staff" Then
                        Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                        Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                        student1.Read()
                        sendername.Add(student1.Item(0))
                        student1.Close()
                    End If
                    ds.Rows.Add(item, Convert.ToDateTime(dates(x)).ToString("dd/MM/yyyy hh:mm tt"), sendername(x), sendertype(x), subject(x), status(x))
                    x = x + 1
                Next
                GridView1.AllowPaging = False
                GridView1.DataSource = ds
                GridView1.DataBind()
                If GridView1.PageIndex = GridView1.PageCount - 1 Then
                    btnNext.Visible = False
                Else
                    btnNext.Visible = True
                End If
                If GridView1.PageIndex = 0 Then
                    btnPrevious.Visible = False
                Else
                    btnPrevious.Visible = True
                End If
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id, cast(date as char) as date, sender, sendertype, subject, status from messages where receivertype = '" & "Admin" & "'  order by date desc", con)
                Dim ds As New DataTable
                ds.Columns.Add("id")
                ds.Columns.Add("date")
                ds.Columns.Add("sender")
                ds.Columns.Add("sendertype")
                ds.Columns.Add("subject")
                ds.Columns.Add("status")
                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Dim id As New ArrayList
                Dim dates As New ArrayList
                Dim sendere As New ArrayList
                Dim sendertype As New ArrayList
                Dim subject As New ArrayList
                Dim status As New ArrayList
                Dim sendername As New ArrayList
                Do While reader.Read
                    id.Add(reader.Item(0))
                    dates.Add(reader.Item(1))
                    sendere.Add(reader.Item(2))
                    sendertype.Add(reader.Item(3))
                    subject.Add(reader.Item(4))
                    status.Add(reader.Item(5))
                Loop
                reader.Close()
                Dim x As Integer
                For Each item As String In id
                    If sendertype(x) = "Student" Then
                        Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from studentsprofile where admno = '" & sendere(x) & "'", con)
                        Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                        student.Read()
                        sendername.Add(student.Item(0).ToString)
                        student.Close()
                    ElseIf sendertype(x) = "Staff" Then
                        Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                        Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                        student1.Read()
                        sendername.Add(student1.Item(0).ToString)
                        student1.Close()
                    ElseIf sendertype(x) = "Parent" Then
                        Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentname from parentprofile where parentid = '" & sendere(x) & "'", con)
                        Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                        student1.Read()
                        sendername.Add(student1.Item(0).ToString)
                        student1.Close()
                    End If
                    ds.Rows.Add(item, Convert.ToDateTime(dates(x)).ToString("dd/MM/yyyy hh:mm tt"), sendername(x), sendertype(x), subject(x), status(x))
                    x = x + 1
                Next
                GridView1.DataSource = ds
                GridView1.DataBind()
                If GridView1.PageIndex = GridView1.PageCount - 1 Then
                    btnNext.Visible = False
                Else
                    btnNext.Visible = True
                End If
                If GridView1.PageIndex = 0 Then
                    btnPrevious.Visible = False
                Else
                    btnPrevious.Visible = True
                End If
                con.close()            End Using

            If GridView1.PageIndex - 1 >= 0 Then
                GridView1.PageIndex = GridView1.PageIndex - 1
                GridView1.DataBind()
                If GridView1.PageIndex = GridView1.PageCount - 1 Then
                    btnNext.Visible = False
                Else
                    btnNext.Visible = True
                End If
                If GridView1.PageIndex = 0 Then
                    btnPrevious.Visible = False
                Else
                    btnPrevious.Visible = True
                End If

            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnMsg_Click(sender As Object, e As EventArgs) Handles btnMsg.Click
        Response.Redirect("~/content/App/App/App/Admin/newmsg.aspx")
    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        Response.Redirect("~/content/App/App/App/Admin/sentmsgs.aspx")
    End Sub

    Protected Sub btnSMS_Click(sender As Object, e As EventArgs) Handles btnSMS.Click
        Response.Redirect("~/content/App/App/App/Admin/smsreports.aspx")
    End Sub
End Class
