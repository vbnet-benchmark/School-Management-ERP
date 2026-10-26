Imports System.Text
Imports System.Configuration
Imports System.Data
Imports System.Web.UI.WebControls
Imports System.IO
Imports System.Net
Imports System.Web
Imports System.Net.Mail
Imports System.Resources
Partial Class Admin_adminpage
    Inherits System.Web.UI.Page
    Public Shared receiver As String
    Public Shared recCategory As String

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
        scriptman.RegisterPostBackControl(btnUpload)
        Dim timer As New Timer
        timer = Me.Master.FindControl("timer1")
        timer.Enabled = False
    End Sub



    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("staffid") = Nothing Then            Dim x As ArrayList = check.Check_app_staff(Request.QueryString("username"), Request.QueryString("password"))            If x.Count <> 0 Then                Session("staffid") = x.Item(0)                Session("sessionid") = x.Item(1)            End If        End If
        cboReceivetype.Text = Session("receivert")
        txtReceiver.Text = Session("receiver")
    End Sub

    Protected Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
        Try

      
        Dim message As String = FreeTextBox1.Text
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                If ChkPortal.Checked = False And chkSendSMS.Checked = False Then
                    Show_Alert(False, "Select an option for either Portal message, SMS or both")
                    Exit Sub
                End If
                Dim x As New Random
                Dim ref As New MySql.Data.MySqlClient.MySqlCommand("Select id from messages", con)
                Dim readref As MySql.Data.MySqlClient.MySqlDataReader = ref.ExecuteReader
                Dim test As Boolean = False
                Dim refs As New ArrayList
                Do While readref.Read
                    refs.Add(readref.Item(0))
                Loop
                Dim y As Integer
                Do Until test = True
                    y = x.Next(100000, 999999)
                    If refs.Contains(y) Then
                        test = False
                    Else
                        test = True
                    End If
                Loop
                readref.Close()
                Dim smsno As Integer
                Dim pages As Integer = Math.Ceiling(txtSMS.Text.Count / 160)
                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT email, password, port, smtp, smsapi, smsname from options", con)
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", Session("sessionid")))
                Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader
                reader3.Read()
                Dim smsapi As String = reader3(4).ToString
                Dim smsname As String = reader3(5).ToString
                reader3.Close()
                Dim inisms As Integer
                If chkSendSMS.Checked = True Then
                   

                    Dim cmdSelect2a As New MySql.Data.MySqlClient.MySqlCommand("SELECT smsno FROM Session WHERE id = ?", con)
                    cmdSelect2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionId")))
                    Dim reader2a As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2a.ExecuteReader
                    reader2a.Read()
                    smsno = reader2a(0)
                    inisms = smsno
                    reader2a.Close()
                    If pages > smsno Then
                        Show_Alert(False, "You don't have enough credits for the SMS operation. Contact developer")
                        Exit Sub
                    End If
                End If
                Dim z As String
                Dim phone As String
                If cboReceivetype.Text = "Student" Then
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT admno, phone from StudentsProfile where surname = ?", con)
                    cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", txtReceiver.Text))
                    Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    student.Read()
                    z = student.Item(0)
                    phone = student.Item(1)
                    smsno = smsno - (1 * pages)
                    student.Close()
                ElseIf cboReceivetype.Text = "Staff" Then
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffid, phone from StaffProfile where surname = ?", con)
                    cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", txtReceiver.Text))
                    Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    student.Read()
                    z = student.Item(0)
                    phone = student.Item(1)
                    smsno = smsno - (1 * pages)
                    student.Close()
                ElseIf cboReceivetype.Text = "Parent" Then
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentid, phone from parentProfile where parentname = ?", con)
                    cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", txtReceiver.Text))
                    Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    student.Read()
                    z = student.Item(0)
                    phone = student.Item(1)
                    smsno = smsno - (1 * pages)
                    student.Close()
                End If
                If ChkPortal.Checked = True Then
                    Dim cmdCheck20 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into messages (id, sender, receiver, subject, message, date, session, sendertype, receivertype) Values (?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Id", y))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sender", Session("StaffID")))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("receiver", z))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", txtSubject.Text))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("message", message))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("SessionID")))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sendertype", "Admin"))
                    cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("receivertype", cboReceivetype.Text))
                    cmdCheck20.ExecuteNonQuery()
                    Dim receiveurl As String
                    If cboReceivetype.Text = "Staff" Then receiveurl = "~/content/staff/readmsg.aspx?" & y Else receiveurl = "~/content/parent/readmsg.aspx?" & y

                    logify.Notifications("You have a new message from Admin - " & txtSubject.Text, z, Session("staffid"), "Admin", receiveurl, "Message")
                End If
                Dim sendsuccess As Boolean = False
                If chkSendSMS.Checked = True Then
                    Dim sms = txtSMS.Text
                    Dim path As String = "http://" & Request.Url.Authority & "/Portal/"
                    Dim apikey = smsapi
                    Dim strPost As String
                    Dim senders = smsname
                    Dim url As String = "https://www.bulksmsnigeria.com/api/v1/sms/create?"
                    strPost = url & "api_token=" & apikey & "&to=" & phone & "&body=" & sms & "&from=" & senders & "&dnd=2"

                    Dim rrequest As WebRequest = WebRequest.Create(strPost)
                    rrequest.Method = "POST"
                    Dim byteArray As Byte() = Encoding.UTF8.GetBytes(strPost)
                    rrequest.ContentType = "application/x-www-form-urlencoded"
                    rrequest.ContentLength = byteArray.Length
                    Dim dataStream As Stream = rrequest.GetRequestStream()
                    dataStream.Write(byteArray, 0, byteArray.Length)
                    dataStream.Close()
                    sendsuccess = True
                   
        Dim cmdInsert2a As New MySql.Data.MySqlClient.MySqlCommand("Update session set smsno = ? where id = ?", con)
        cmdInsert2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("aveage", smsno))
        cmdInsert2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("No", Session("Sessionid")))
        cmdInsert2a.ExecuteNonQuery()

        Dim ceck2Insert2a As New MySql.Data.MySqlClient.MySqlCommand("Insert into sms (subject, message, units, pages, recipientsno, recipienttype, time, sender) values (?,?,?,?,?,?,?,?)", con)
        ceck2Insert2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("aveage", txtSubject.Text))
        ceck2Insert2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("No", sms))
        ceck2Insert2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Nossjs", inisms - smsno))
        ceck2Insert2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Nohdh", pages))
        ceck2Insert2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Nok", 1))
                    ceck2Insert2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Ndo", cboReceivetype.Text))
        ceck2Insert2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Nddo", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
        ceck2Insert2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Ndddho", Session("staffid")))
        ceck2Insert2a.ExecuteNonQuery()
                End If
                If ChkPortal.Checked = True Then
                    If LinkButton1.Text <> "" Then
                        Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Insert into attachments (msgId, file, fileicon) values (?,?,?)", con)
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pass", y))
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", Session("pic1")))
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surme", att1.Text))

                        cmdCheck.ExecuteNonQuery()
                    End If
                    If LinkButton2.Text <> "" Then
                        Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Insert into attachments (msgId, file, fileicon) values (?,?,?)", con)
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pass", y))
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", Session("pic2")))
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surme", att2.Text))
                        cmdCheck.ExecuteNonQuery()
                    End If
                    If LinkButton3.Text <> "" Then
                        Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Insert into attachments (msgId, file, fileicon) values (?,?,?)", con)
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pass", y))
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", Session("pic3")))
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surme", att3.Text))
                        cmdCheck.ExecuteNonQuery()
                    End If
                    If LinkButton4.Text <> "" Then
                        Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Insert into attachments (msgId, file, fileicon) values (?,?,?)", con)
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pass", y))
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", Session("pic4")))
                        cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surme", att4.Text))
                        cmdCheck.ExecuteNonQuery()
                    End If

                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into sentmsgs (id, sender, receiver, subject, message, date, session, sendertype, receivertype, receiverreply) Values (?,?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Id", y))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sender", Session("StaffID")))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("receiver", 1 & " Recipient"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", txtSubject.Text))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("message", message))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("SessionID")))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sendertype", "Admin"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("receivertype", cboReceivetype.Text))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("receiverreply", Session("receivert")))
                    cmdCheck2.ExecuteNonQuery()
                End If
                logify.log(Session("staffid"), "A message was sent to " & txtReceiver.Text & " " & IIf(chkSendSMS.Checked = True, "with SMS.", "without SMS") & ". Message ref = " & y)
                Show_Alert(True, "Message sent. " & IIf(chkSendSMS.Checked = True, IIf(sendsuccess = False, 0, 1) & " SMS sent. Length = " & pages & " pages", ""))

                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If Session("receivert") = "Student" Then
            Response.Redirect("~/content/App/Admin/studentprofile.aspx")
        ElseIf Session("receivert") = "Staff" Then
            Response.Redirect("~/content/App/Admin/staffprofile.aspx")
        ElseIf Session("receivert") = "Parent" Then
            Response.Redirect("~/content/App/Admin/parentprofile.aspx")
        End If

    End Sub

    Protected Sub ChkPortal_CheckedChanged(sender As Object, e As EventArgs) Handles ChkPortal.CheckedChanged
        If ChkPortal.Checked = True Then
            PnlMsg.Visible = True
        Else
            PnlMsg.Visible = False
        End If
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

    Protected Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click
        Try
            Dim folderPath As String = Server.MapPath("~/Content/Uploads/")
            Dim q As String
            If FileUpload1.HasFile Then
                FileUpload1.SaveAs(folderPath & Path.GetFileName(FileUpload1.FileName))
                q = "http://" & Request.Url.Authority & "/Content/Uploads/" & FileUpload1.FileName
                Dim picsource As New fileimage
                If LinkButton1.Text = "" Then
                    att1.Text = FileUpload1.PostedFile.ContentType
                    icon1.Src = picsource.get_image(FileUpload1.PostedFile.ContentType)
                    LinkButton1.Text = Path.GetFileName(q)
                    Session("pic1") = q
                    Panel1.Visible = True
                    del1.Text = "Delete"
                ElseIf LinkButton2.Text = "" Then
                    att2.Text = FileUpload1.PostedFile.ContentType
                    icon2.Src = picsource.get_image(FileUpload1.PostedFile.ContentType)
                    LinkButton2.Text = Path.GetFileName(q)
                    Session("pic2") = q
                    del2.Text = "Delete"

                ElseIf LinkButton3.Text = "" Then
                    att3.Text = FileUpload1.PostedFile.ContentType
                    icon3.Src = picsource.get_image(FileUpload1.PostedFile.ContentType)
                    LinkButton3.Text = Path.GetFileName(q)
                    Session("pic3") = q
                    del3.Text = "Delete"

                ElseIf LinkButton4.Text = "" Then
                    att4.Text = FileUpload1.PostedFile.ContentType
                    icon4.Src = picsource.get_image(FileUpload1.PostedFile.ContentType)
                    LinkButton4.Text = Path.GetFileName(q)
                    Session("pic4") = q
                    del4.Text = "Delete"

                Else
                    Show_Alert(False, "You cannot add more than 4 attachments.")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub del1_Click(sender As Object, e As EventArgs) Handles del1.Click
        LinkButton1.Text = ""
        icon1.Src = ""
        del1.Text = ""
        Session("pic1") = Nothing
        If LinkButton2.Text <> "" Then
            LinkButton1.Text = LinkButton2.Text
            icon1.Src = icon2.Src
            att1.text = att2.text
            del1.Text = "Delete"
            LinkButton2.Text = ""
            icon2.Src = ""
            del2.Text = ""
            Session("pic1") = Session("pic2")
            Session("pic2") = Nothing
        End If
        If LinkButton3.Text <> "" Then
            LinkButton2.Text = LinkButton3.Text
            icon2.Src = icon3.Src
            att2.text = att3.text
            del2.Text = "Delete"
            LinkButton3.Text = ""
            icon3.Src = ""
            del3.Text = ""
            Session("pic2") = Session("pic3")
            Session("pic3") = Nothing
        End If
        If LinkButton4.Text <> "" Then
            LinkButton3.Text = LinkButton4.Text
            icon3.Src = icon4.Src
            att3.text = att4.text
            del3.Text = "Delete"
            LinkButton4.Text = ""
            icon4.Src = ""
            del4.Text = ""
            Session("pic3") = Session("pic4")
            Session("pic4") = Nothing
        End If
        If LinkButton1.Text = "" Then
            Panel1.Visible = False
        End If
    End Sub

    Protected Sub del2_Click(sender As Object, e As EventArgs) Handles del2.Click
        LinkButton2.Text = ""
        icon2.Src = ""
        del2.Text = ""
        Session("pic2") = Nothing
        If LinkButton3.Text <> "" Then
            LinkButton2.Text = LinkButton3.Text
            att2.Text = att3.Text
            icon2.Src = icon3.Src
            del2.Text = "Delete"
            LinkButton3.Text = ""
            icon3.Src = ""
            del3.Text = ""
            Session("pic2") = Session("pic3")
            Session("pic3") = Nothing
        End If
        If LinkButton4.Text <> "" Then
            LinkButton3.Text = LinkButton4.Text
            icon3.Src = icon4.Src
            att3.Text = att4.Text
            del3.Text = "Delete"
            LinkButton4.Text = ""
            icon4.Src = ""
            del4.Text = ""
            Session("pic3") = Session("pic4")
            Session("pic4") = Nothing
        End If
    End Sub

    Protected Sub del3_Click(sender As Object, e As EventArgs) Handles del3.Click
        LinkButton3.Text = ""
        icon3.Src = ""
        del3.Text = ""
        Session("pic3") = Nothing
        If LinkButton4.Text <> "" Then
            LinkButton3.Text = LinkButton4.Text
            icon3.Src = icon4.Src
            att3.Text = att4.Text
            del3.Text = "Delete"
            LinkButton4.Text = ""
            icon4.Src = ""
            del4.Text = ""
            Session("pic3") = Session("pic4")
            Session("pic4") = Nothing
        End If
    End Sub

    Protected Sub del4_Click(sender As Object, e As EventArgs) Handles del4.Click
        LinkButton4.Text = ""
        icon4.Src = ""
        del4.Text = ""
        Session("pic4") = Nothing
    End Sub

    Protected Sub chkSendSMS_CheckedChanged(sender As Object, e As EventArgs) Handles chkSendSMS.CheckedChanged
        If chkSendSMS.Checked = True Then
            pnlsms.Visible = True
        Else
            pnlsms.Visible = False
        End If
    End Sub
End Class
