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
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("staffid") = Nothing Then            Dim x As ArrayList = check.Check_app_staff(Request.QueryString("username"), Request.QueryString("password"))            If x.Count <> 0 Then                Session("staffid") = x.Item(0)                Session("sessionid") = x.Item(1)            End If        End If
        Try


            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                If Request.QueryString.ToString <> Nothing And Not IsPostBack Then
                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select messages.sender, messages.sendertype, messages.subject, messages.message, messages.receivertype, sentmsgs.sendertype, staffprofile.surname from messages inner join staffprofile on staffprofile.staffid = messages.receiver inner join sentmsgs on sentmsgs.id = messages.id where sentmsgs.id = '" & Request.QueryString("id") & "' and staffprofile.surname = '" & Replace(Request.QueryString("receiver"), "+", " ") & "'", con)
                    Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                    msg.Read()
                    txtSender.Text = msg.Item(0).ToString
                    txtSenderCat.Text = msg.Item(5).ToString
                    Dim sendername As String = msg.Item(1).ToString
                    txtSubject.Text = msg.Item(2).ToString
                    FreeTextBox1.Text = msg.Item(3).ToString
                    txtReceiver.Text = msg.Item(6).ToString
                    msg.Close()
                    If sendername = "Student" Then
                        Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from studentsprofile where admno = '" & txtSender.Text & "'", con)
                        Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                        student.Read()
                        txtSender.Text = student.Item(0).ToString
                        student.Close()
                    ElseIf sendername = "Staff" Then
                        Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & txtSender.Text & "'", con)
                        Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                        student1.Read()
                        txtSender.Text = student1.Item(0).ToString
                        student1.Close()
                    ElseIf sendername = "Parent" Then
                        Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentname from parentprofile where parentid = '" & txtSender.Text & "'", con)
                        Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                        student1.Read()
                        txtSender.Text = student1.Item(0).ToString
                        student1.Close()
                    ElseIf sendername = "Admin" Then
                        Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & txtSender.Text & "'", con)
                        Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                        If student1.Read() Then
                            txtSender.Text = student1.Item(0).ToString
                        Else
                            txtSender.Text = "Super Admin"
                        End If
                        student1.Close()
                    ElseIf sendername = "Accounts" Then
                        Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & txtSender.Text & "'", con)
                        Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                        If student1.Read() Then
                            txtSender.Text = student1.Item(0).ToString
                        Else
                            txtSender.Text = "Super Admin"
                        End If
                        student1.Close()
                    End If
                    Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update messages set status = '" & "Read" & "' where id = '" & Request.QueryString.ToString & "'", con)
                    cmdCheck3.ExecuteNonQuery()
                    Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Select file from attachments where msgId = '" & Request.QueryString.ToString & "'", con)
                    Dim msg2 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck.ExecuteReader
                    Dim c As Integer = 1
                    Do While msg2.Read
                        If c = 1 Then
                            Dim img As New Image
                            img.ImageUrl = ("~/Image/6.png")
                            img1.Controls.Add(img)
                            LinkButton1.Text = Path.GetFileName(msg2.Item(0))
                            Session("pic1") = msg2.Item(0)
                        ElseIf c = 2 Then
                            Dim img As New Image
                            img.ImageUrl = ("~/Image/6.png")
                            img2.Controls.Add(img)
                            LinkButton2.Text = Path.GetFileName(msg2.Item(0))
                            Session("pic2") = msg2.Item(0)
                        ElseIf c = 3 Then
                            Dim img As New Image
                            img.ImageUrl = ("~/Image/6.png")
                            img3.Controls.Add(img)
                            LinkButton3.Text = Path.GetFileName(msg2.Item(0))
                            Session("pic3") = msg2.Item(0)
                        ElseIf c = 4 Then
                            Dim img As New Image
                            img.ImageUrl = ("~/Image/6.png")
                            img4.Controls.Add(img)
                            LinkButton4.Text = Path.GetFileName(msg2.Item(0))
                            Session("pic4") = msg2.Item(0)
                        End If
                        c = c + 1
                    Loop
                    msg2.Close()
                End If
                If Session("msgsuccess") = "Yes" Then
                    lblSuccess.Text = "Message sent successfully"
                End If
                FreeTextBox1.Focus = True
                FreeTextBox1.Toolbars.Clear()
                FreeTextBox1.ReadOnly = True
                con.Close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
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
        Response.Redirect("~/content/app/staff/smessages.aspx")
    End Sub
End Class
