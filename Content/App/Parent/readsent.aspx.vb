Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Imports System.IO
Partial Class Admin_adminpage
    Inherits System.Web.UI.Page

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
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Dim scriptman As New ScriptManager
        scriptman = Me.Master.FindControl("ScriptManager1")
        scriptman.RegisterPostBackControl(LinkButton1)
        scriptman.RegisterPostBackControl(LinkButton2)
        scriptman.RegisterPostBackControl(LinkButton3)
        scriptman.RegisterPostBackControl(LinkButton4)
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

            Dim ds As New DataTable
            ds.Columns.Add("name")
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                If Request.QueryString.ToString <> Nothing And Not IsPostBack Then
                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select messages.subject, messages.message, messages.receiver, messages.receivertype, sentmsgs.receivertype from messages left join sentmsgs on messages.id = sentmsgs.id where messages.id = '" & Request.QueryString.ToString & "'", con)
                    Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                    Dim recipienttype As String
                    Dim recipients As New ArrayList
                    Do While msg.Read
                        txtSubject.Text = msg.Item(0)
                        FreeTextBox1.Text = msg.Item(1)
                        recipients.Add(msg.Item(2))
                        recipienttype = msg.Item(4)
                    Loop
                    Dim rectype As String = msg.Item(3)
                    txtRecType.Text = recipienttype
                    msg.Close()
                    For Each item As String In recipients
                        If rectype = "Student" Then
                            Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from studentsprofile where admno = '" & item & "'", con)
                            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                            student.Read()
                            ds.Rows.Add(student(0))
                            student.Close()
                        ElseIf rectype = "Staff" Then
                            Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & item & "'", con)
                            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                            student1.Read()
                            ds.Rows.Add(student1(0))
                            student1.Close()
                        ElseIf rectype = "Admin" Then
                            ds.Rows.Add("Admin")
                        ElseIf rectype = "Accounts" Then
                            ds.Rows.Add("Accounts")
                        End If
                    Next
                    Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Select file, fileicon from attachments where msgId = '" & Request.QueryString.ToString & "'", con)
                    Dim msg2 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck.ExecuteReader
                    Dim c As Integer = 1
                    Do While msg2.Read
                        Dim imagesrc As New fileimage
                        If c = 1 Then
                            Panel1.Visible = True
                            icon1.Src = imagesrc.get_image(msg2(1).ToString)
                            LinkButton1.Text = Path.GetFileName(msg2.Item(0))
                            Session("pic1") = msg2.Item(0)
                        ElseIf c = 2 Then
                            icon2.Src = imagesrc.get_image(msg2(1).ToString)
                            LinkButton2.Text = Path.GetFileName(msg2.Item(0))
                            Session("pic2") = msg2.Item(0)
                        ElseIf c = 3 Then
                            icon3.Src = imagesrc.get_image(msg2(1).ToString)
                            LinkButton3.Text = Path.GetFileName(msg2.Item(0))
                            Session("pic3") = msg2.Item(0)
                        ElseIf c = 4 Then
                            icon4.Src = imagesrc.get_image(msg2(1).ToString)
                            LinkButton4.Text = Path.GetFileName(msg2.Item(0))
                            Session("pic4") = msg2.Item(0)
                        End If
                        c = c + 1
                    Loop
                    msg2.Close()
                End If

                FreeTextBox1.Focus = True
                FreeTextBox1.Toolbars.Clear()
                FreeTextBox1.ReadOnly = True
                gridRecipients.DataSource = ds
                gridRecipients.DataBind()
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    

    Protected Sub btnForward_Click(sender As Object, e As EventArgs) Handles btnForward.Click
        Session("responsetype") = "Forward"
        Response.Redirect("~/content/App/App/Parent/newmsg.aspx?" & Request.QueryString.ToString)

    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        Response.Redirect(Session("pic1"))
    End Sub

    Protected Sub LinkButton2_Click(sender As Object, e As EventArgs) Handles LinkButton2.Click
        Response.Redirect(Session("pic2"))
    End Sub

    Protected Sub LinkButton3_Click(sender As Object, e As EventArgs) Handles LinkButton3.Click
        Response.Redirect(Session("pic3"))

    End Sub

    Protected Sub LinkButton4_Click(sender As Object, e As EventArgs) Handles LinkButton4.Click
        Response.Redirect(Session("pic4"))

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Response.Redirect("~/content/App/App/parent/sentmsgs.aspx")
    End Sub
End Class
