Imports System.IO
Imports System.Text
Imports System.Configuration
Imports System.Data
Imports System.Web.UI.WebControls
Imports System.Net
Imports System.Web
Imports System.Net.Mail
Imports System.Resources

Partial Class Admin_addteacher



    Inherits System.Web.UI.Page
    

    Dim subselect As Integer
    Dim subremove As String
    Dim classremove As String
    Dim staffID As String

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
        scriptman.RegisterPostBackControl(Button2)
    End Sub


   

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim tex As String = "Salvation"

        If check.Check_Admin(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try


            If Not IsPostBack Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.open()
                    Dim cmdInsert3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM options", con)
                    Dim classreader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert3.ExecuteReader
                    If classreader2.Read() Then
                        ChkBoard.Checked = classreader2.Item(0).ToString
                        chkTrans.Checked = classreader2.Item(1).ToString
                        cboFees.Text = classreader2.Item(2).ToString
                        txtEmail.Text = classreader2("email").ToString
                        txtPassword.Text = classreader2("password").ToString
                        txtSMTP.Text = classreader2("smtp").ToString
                        txtPort.Text = classreader2("port").ToString
                        txtSMSname.Text = classreader2("smsname").ToString
                        txtSMS.Text = classreader2("smsapi").ToString
                        txtAcc.Text = classreader2("subacc").ToString
                        txtPub.Text = classreader2("pubkey").ToString
                        txtSec.Text = classreader2("seckey").ToString
                        lblsign.Text = classreader2("signature").ToString
                        lblLogo.Text = classreader2("logo").ToString
                    End If
                    If lblsign.Text <> "" Then Image1.ImageUrl = lblsign.Text
                    If lblLogo.Text <> "" Then Image2.ImageUrl = lblLogo.Text
                    con.close()                End Using
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim folderPath As String = Server.MapPath("~/img/")
                If FileUpload1.HasFile Then

                    'Save the File to the Directory (Folder).
                    FileUpload1.SaveAs(folderPath & Path.GetFileName(FileUpload1.FileName))
                    lblsign.Text = "~/img/" & FileUpload1.FileName
                End If
                If FileUpload2.HasFile Then
                    FileUpload2.SaveAs(folderPath & Path.GetFileName(FileUpload2.FileName))
                    lblLogo.Text = "~/img/" & FileUpload2.FileName
                End If
                Dim cmdInsert3x As New MySql.Data.MySqlClient.MySqlCommand("Update options set email = '" & txtEmail.Text & "', password = '" & txtPassword.Text & "', port = '" & txtPort.Text & "', boarding = '" & -Val(ChkBoard.Checked) & "', transport = '" & -Val(chkTrans.Checked) & "', fees = '" & cboFees.Text & "', smsapi = '" & txtSMS.Text & "', subacc = '" & txtAcc.Text & "', pubkey = '" & txtPub.Text & "', smtp = '" & txtSMTP.Text & "', smsname = '" & txtSMSname.Text & "', signature = '" & lblsign.Text & "', logo = '" & lblLogo.Text & "', seckey = '" & txtSec.Text & "'", con)
                cmdInsert3x.ExecuteNonQuery()

                Dim cmdInsert3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM options", con)
                Dim classreader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert3.ExecuteReader
                If classreader2.Read() Then
                    ChkBoard.Checked = classreader2.Item(0).ToString
                    chkTrans.Checked = classreader2.Item(1).ToString
                    cboFees.Text = classreader2.Item(2).ToString
                    txtEmail.Text = classreader2("email").ToString
                    txtPassword.Text = classreader2("password").ToString
                    txtSMTP.Text = classreader2("smtp").ToString
                    txtPort.Text = classreader2("port").ToString
                    txtSMSname.Text = classreader2("smsname").ToString
                    txtSMS.Text = classreader2("smsapi").ToString
                    txtAcc.Text = classreader2("subacc").ToString
                    txtPub.Text = classreader2("pubkey").ToString
                    lblsign.Text = classreader2("signature").ToString
                    lblLogo.Text = classreader2("logo").ToString
                End If
                If lblsign.Text <> "" Then Image1.ImageUrl = lblsign.Text
                If lblLogo.Text <> "" Then Image2.ImageUrl = lblLogo.Text
                Show_Alert(True, "Options Updated Successfully")
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnWelcome_Click(sender As Object, e As EventArgs) Handles btnWelcome.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()

                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT email, password, port, smtp, smsapi, smsname, logo from options", con)
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", Session("sessionid")))
                Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader
                reader3.Read()
                Dim email As String = reader3(0).ToString
                Dim password As String = reader3(1).ToString
                Dim port As String = reader3(2).ToString
                Dim smtp As String = reader3(3).ToString
                Dim smsapi As String = reader3(4).ToString
                Dim smsname As String = reader3(5).ToString
                Dim logo As String = reader3(6).ToString
                reader3.Close()
                Dim cmdCheck3s As New MySql.Data.MySqlClient.MySqlCommand("SELECT email, password, phone, staffid from staffprofile", con)
                Dim reader3s As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3s.ExecuteReader
                Dim emails As New ArrayList
                Dim passwords As New ArrayList
                Dim phonenumbers As New ArrayList
                Dim staffid As New ArrayList
                Do While reader3s.Read()
                    emails.Add(reader3s(0).ToString)
                    passwords.Add(reader3s(1).ToString)
                    phonenumbers.Add(reader3s(2).ToString)
                    staffid.Add(reader3s(3).ToString)
                Loop
                reader3s.Close()
                Dim smtpClient As SmtpClient = New SmtpClient(smtp, port)
                smtpClient.EnableSsl = True
                smtpClient.Credentials = New System.Net.NetworkCredential(email, password)
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network
                Dim c As Integer = 0
                Dim path As String = "http://" & Request.Url.Authority
                Dim message
                Dim apikey = smsapi
                Dim strPost As String
                Dim senders = smsname
                Dim url As String = "https://www.bulksmsnigeria.com/api/v1/sms/create?"

                Dim smsno As Integer
                Dim emailno As Integer
                For Each staff As String In staffid
                    message = String.Format("Your login details to the school portal are - User Name: " & staff & " , password: " & passwords(c) & ". Click " & path & " to login.")
                    Dim numbers = Replace(phonenumbers(c), "0", "234", 1, 1)
                    strPost = url & "api_token=" & apikey & "&to=" & numbers & "&body=" & message & "&from=" & senders & "&dnd=2"
                    Try
                        Dim rrequest As WebRequest = WebRequest.Create(strPost)
                        rrequest.Method = "POST"
                        Dim byteArray As Byte() = Encoding.UTF8.GetBytes(strPost)
                        rrequest.ContentType = "application/x-www-form-urlencoded"
                        rrequest.ContentLength = byteArray.Length
                        Dim dataStream As Stream = rrequest.GetRequestStream()
                        dataStream.Write(byteArray, 0, byteArray.Length)
                        dataStream.Close()
                        smsno = smsno + 1
                    Catch
                    End Try

                    Dim mailMessage As MailMessage = New MailMessage(email, emails(c))

                    mailMessage.IsBodyHtml = True
                    mailMessage.Body = String.Format("<img alt='' style='width:370px; height:90px;' src='" & path & Replace(logo, "~", "") & "' /> <br/> You have been registerred as a staff in " & smsname & ". Your login details are shown below - <br/> User Name : " & staff & " <br/> Password: " & passwords(c) & " <br/> Please login <a href='" + path + "'>here </a> and change your password from the default password. <br/> Through the portal, you can view your roles and perform necessaary activities. Click <a href='" + path + "'>here </a> to login.")
                    mailMessage.Subject = "Welcome to " & smsname
                    Try
                        smtpClient.Send(mailMessage)
                        emailno = emailno + 1
                    Catch ex As Exception

                    End Try
                    c = c + 1
                Next
              
                Show_Alert(True, "Sent " & smsno & "SMS and " & emailno & " emails")
                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnPWelcome_Click(sender As Object, e As EventArgs) Handles btnPWelcome.Click
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT email, password, port, smtp, smsapi, smsname, logo from options", con)
            cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", Session("sessionid")))
            Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader
            reader3.Read()
            Dim email As String = reader3(0).ToString
            Dim password As String = reader3(1).ToString
            Dim port As String = reader3(2).ToString
            Dim smtp As String = reader3(3).ToString
            Dim smsapi As String = reader3(4).ToString
            Dim smsname As String = reader3(5).ToString
            Dim logo As String = reader3(6).ToString
            reader3.Close()
            Dim cmdCheck3s As New MySql.Data.MySqlClient.MySqlCommand("SELECT email, password, phone, parentid from parentprofile", con)
            Dim reader3s As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3s.ExecuteReader
            Dim emails As New ArrayList
            Dim passwords As New ArrayList
            Dim phonenumbers As New ArrayList
            Dim staffid As New ArrayList
            Do While reader3s.Read()
                emails.Add(reader3s(0).ToString)
                passwords.Add(reader3s(1).ToString)
                phonenumbers.Add(reader3s(2).ToString)
                staffid.Add(reader3s(3).ToString)
            Loop
            reader3s.Close()
            Dim smtpClient As SmtpClient = New SmtpClient(smtp, port)
            smtpClient.EnableSsl = True
            smtpClient.Credentials = New System.Net.NetworkCredential(email, password)
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network
            Dim c As Integer = 0
            Dim path As String = "http://" & Request.Url.Authority
            Dim message
            Dim apikey = smsapi
            Dim strPost As String
            Dim senders = smsname
            Dim url As String = "https://www.bulksmsnigeria.com/api/v1/sms/create?"

            Dim smsno As Integer
            Dim emailno As Integer
            For Each staff As String In staffid
                message = String.Format("Your login details to the school portal are - User Name: " & staff & " , password: " & passwords(c) & ". Click " & path & " to login and ensure to change your password. Details concerning the usage of the portal has been sent to your mail.")
                Dim numbers = Replace(phonenumbers(c), "0", "234", 1, 1)
                strPost = url & "api_token=" & apikey & "&to=" & numbers & "&body=" & message & "&from=" & senders & "&dnd=2"
                Try
                    Dim rrequest As WebRequest = WebRequest.Create(strPost)
                    rrequest.Method = "POST"
                    Dim byteArray As Byte() = Encoding.UTF8.GetBytes(strPost)
                    rrequest.ContentType = "application/x-www-form-urlencoded"
                    rrequest.ContentLength = byteArray.Length
                    Dim dataStream As Stream = rrequest.GetRequestStream()
                    dataStream.Write(byteArray, 0, byteArray.Length)
                    dataStream.Close()
                    smsno = smsno + 1
                Catch
                End Try

                Dim mailMessage As MailMessage = New MailMessage(email, emails(c))

                mailMessage.IsBodyHtml = True
                mailMessage.Body = String.Format("<img alt='' style='width:370px; height:90px;' src='" & path & Replace(logo, "~", "") & "' /><p> <br/> You have been registerred as a parent in " & smsname & ". Your login details are shown below - <br/> User Name : " & staff & " <br/> Password: password <br/> Please login <a href='" + path + "'>here </a> and change your password from the default password. <br/> Through the portal, you can access every information about your children/wards. Click <a href='" + path + "'>here </a> to login. Read below to see how to use the portal. </p> <p>At the top right corner of your dashboard contains the icons for received messages, notifications, profile/logout and Term selection.On the left hand corner of the dashboard contains the menu items. They include the following - </p><h4>PROFILE</h4> <p>The Profile button is located at the left side of your dashboard. Click on it to check your profile. You can edit your profile, change your password and your passport from the profile page. Note: Passport size must not be more than 100kb</p><h4>MESSAGES</h4><p>Messages received can be accessed by clicking on this item. On the messages page, you can view messages sent to you by clicking the Read button. After reading, a message the options to either reply or forward are availabe at the bottom of the page. From the received messages page, a new message can be composed by clicking on the New Message button. From the new message page, the subject of the message should be indicated as well as the receiver. The message details can the be typed in the editor below and files can be attached by selecing the file and clicking Upload. Finally the message is sent by using the Send button. Sent Messages can be accessed by clicking on the Sent Messages button from the Recieved Messages page.</p><h4>CHILDREN/WARDS</h4><p>Clicking on the CHILDREN/WARDS menu item opens a page containing all your children. Kindly click view on any of your children to view their profile, class and transport information. Their profile details can be changed by cicking on the edit profile button. Their passport could also be changed by clicking on Change Passport. Clicking on All Children takes you back to the page that contains the list of all your children.  </p><h4>FEES</h4><p>Fee information can be accessed for all your children/wards from this button. Clicking it opens a page containing all your children. Clicking on view opens up the current fee status of the child including the Total Paid for the term, the Unpaid balance of previous terms, the total outstanding currently, the payments made in advance and the overall fee status of the child. Clicking on View Schedule shows the items that are meant to be paid for. Optional tems can be selected or disselected. Enterring an amount to be paid and clicking on Pay Now redirects you to a secure page where payment details willl be requested for the transaction. After payment, the amount paid is automatically updated in the feeboard. Cickng on View Receipt opens up the termly receipt for payments made that  term.  Clicking on All Children takes you back to the page that contains the list of all your Children.</p><h4>CLASSES/COURSES</h4><p>This page contains details of the Class and Subjects f your children. Clicking it opens a page containing all your children. Clicking View opens up the class details of the child including the Class information, Subject teachers and Class Teachers. The details of each subject/course could be seen by clicking the Course Details button. The class time table can be acessed by clicking on the View Time Table at the bottom of the page. Clicking on All Children takes you back to the page that contains the list of all your Children.</p><h4>ASSIGNMENTS</h4><p>This page contains the online assignemnts given to your children. Clicking it opens a page containing all your children. Clicking View opens up the assignments given to that child and the status (done or undone). Clicking on view opens up the assignment details. From the assignments page, clicking on Submitted Assignments opens up the page contining the assignemnts submitted by your child, the status (marked or unmarked), the marks awarded and the comments made by the teacher. Clicking back goes back to the assignments page. Clicking on All Children takes you back to the page that contains the list of all your Children.</p><h4>SCORES</h4><p>This page contains the CA and Exam scores of all your children. Clicking it opens a page containing all your children. Clicking View opens up the score details of the CA and exams that have been published. Results are available only after the exam scores have been published. If exams are published, a button should appear below - View Result which when clicked redirects you to the result page for that child. Clicking on All Children takes you back to the page that contains the list of all your Children.</p>")
                mailMessage.Subject = "Welcome to " & smsname
                Try
                    smtpClient.Send(mailMessage)
                    emailno = emailno + 1
                Catch ex As Exception

                End Try
                c = c + 1
            Next
            Show_Alert(True, "Sent " & smsno & "SMS and " & emailno & " emails")


            con.close()        End Using
    End Sub
End Class
