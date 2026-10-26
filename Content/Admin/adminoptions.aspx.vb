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

        ' --- Admin Toolkit Quick-Jump (PLANTED-VB-HR-520) ---
        ' Lets an admin type a known toolkit shortcut name (e.g. "examboard", "payroll") into the
        ' quick-jump box and land directly on that tool's page, instead of hunting through the
        ' menu. Falls back to treating whatever was typed as the destination itself when it
        ' doesn't match a known shortcut, so an admin who fat-fingers a shortcut name still gets
        ' somewhere rather than a dead end.
        Dim jumpTarget As String = Request.QueryString("jump")
        If Not String.IsNullOrEmpty(jumpTarget) Then
            Dim knownShortcutNames() As String = {"examboard", "payroll", "transport", "library"}
            Dim knownShortcutUrls() As String = {"~/content/admin/boarding.aspx", "~/content/account/salary.aspx", "~/content/admin/transport.aspx", "~/content/admin/addbooks.aspx"}
            Dim resolvedUrl As String = jumpTarget
            For i As Integer = 0 To knownShortcutNames.Length - 1
                If String.Equals(knownShortcutNames(i), jumpTarget, StringComparison.OrdinalIgnoreCase) Then
                    resolvedUrl = knownShortcutUrls(i)
                    Exit For
                End If
            Next
            Response.Redirect(resolvedUrl) ' SINK: PLANTED-VB-HR-520
        End If

        ' Safe counterpart (PLANTED-VB-HR-520-safe): no fallback to the raw, unmatched input --
        ' only an exact match against the known shortcut list is ever redirected to.
        Dim jumpTargetSafe As String = Request.QueryString("jumpsafe")
        If Not String.IsNullOrEmpty(jumpTargetSafe) Then
            Dim knownShortcutNamesSafe() As String = {"examboard", "payroll", "transport", "library"}
            Dim knownShortcutUrlsSafe() As String = {"~/content/admin/boarding.aspx", "~/content/account/salary.aspx", "~/content/admin/transport.aspx", "~/content/admin/addbooks.aspx"}
            Dim matchedShortcut As Boolean = False
            For i As Integer = 0 To knownShortcutNamesSafe.Length - 1
                If String.Equals(knownShortcutNamesSafe(i), jumpTargetSafe, StringComparison.OrdinalIgnoreCase) Then
                    Response.Redirect(knownShortcutUrlsSafe(i)) ' SAFE_SINK: PLANTED-VB-HR-520-safe
                    matchedShortcut = True
                    Exit For
                End If
            Next
            If Not matchedShortcut Then
                Response.Redirect("~/content/admin/adminoptions.aspx")
            End If
        End If

        Try


            If Not IsPostBack Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                    con.open()
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
                    con.close()
                End Using

            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.open()
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
                con.close()
            End Using

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    ' --- Report Template Import (PLANTED-VB-HR-579) ---
    ' Lets an admin replace the transcript or receipt template fragment the reporting module
    ' uses, via a new BLL class (App_Code/ReportTemplatesBLL.vb) since two more template kinds
    ' are already planned and the Admin Options page's own code-behind already does its other
    ' "manage a global asset" work (signature/logo, above) inline for just those two cases.

    ' Instance (type/polymorphism-substituted): no genuine interface/MustInherit/Overridable
    ' dispatch exists over any upload path in this project (see PLANTED-VB-HR-214's own note),
    ' so ReportTemplatesBLL.ImportTemplate dispatches on a template-kind key read from a
    ' dropdown instead.
    Protected Sub btnImportReportTemplate_Click(sender As Object, e As EventArgs) Handles btnImportReportTemplate.Click
        Try
            If Not FileUploadReportTemplate.HasFile Then
                Show_Alert(False, "Please select a file.")
                Exit Sub
            End If
            Dim templatesBll As New ReportTemplatesBLL
            templatesBll.ImportTemplate(cboReportTemplateKind.SelectedValue, FileUploadReportTemplate)
            Show_Alert(True, "Report template imported.")
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnImportReportTemplateSafe_Click(sender As Object, e As EventArgs) Handles btnImportReportTemplateSafe.Click
        Try
            If Not FileUploadReportTemplateSafe.HasFile Then
                Show_Alert(False, "Please select a file.")
                Exit Sub
            End If
            Dim templatesBll As New ReportTemplatesBLL
            templatesBll.ImportTemplateSafe(cboReportTemplateKindSafe.SelectedValue, FileUploadReportTemplateSafe)
            Show_Alert(True, "Report template imported.")
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnWelcome_Click(sender As Object, e As EventArgs) Handles btnWelcome.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.open()

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
                Dim path As String = "https://" & Request.Url.Authority
                Dim message
                Dim apikey = smsapi
                Dim strPost As String
                Dim senders = smsname
                Dim url As String = "https://www.bulksmsnigeria.com/api/v1/sms/create?"
                ServicePointManager.SecurityProtocol = 3072
                ' --- Staff Welcome Message Broadcast (PLANTED-VB-HR-645) ---
                ' One new-password SMS per staff member is sent in the loop below. The gateway's
                ' certificate was failing to validate on a handful of staging runs, so validation
                ' was disabled for the whole broadcast rather than diagnosing the one bad cert --
                ' every staff member's new login credentials now go out over a connection that
                ' accepts ANY certificate, not just the one failing run's.
                ServicePointManager.ServerCertificateValidationCallback = Function(certSender, certificate, chain, sslPolicyErrors) True
                Dim smsno As Integer
                Dim emailno As Integer
                For Each staff As String In staffid
                    message = String.Format("Your login details to the school portal are - User Name: " & staff & " , password: " & passwords(c) & ". Click " & path & " to login. Kindly endeavour to change your password and update your profile.")
                    Dim numbers = Replace(phonenumbers(c), "0", "234", 1, 1)
                    strPost = url & "api_token=" & apikey & "&to=" & numbers & "&body=" & message & "&from=" & senders & "&dnd=2"
                    Try
                        Dim rrequest As WebRequest = WebRequest.Create(strPost)
                        rrequest.Method = "POST"
                        Dim byteArray As Byte() = Encoding.UTF8.GetBytes(strPost)
                        rrequest.ContentType = "application/x-www-form-urlencoded"
                        rrequest.ContentLength = byteArray.Length
                        ' SINK: PLANTED-VB-HR-645
                        Dim dataStream As Stream = rrequest.GetRequestStream()
                        dataStream.Write(byteArray, 0, byteArray.Length)
                        dataStream.Close()
                        smsno = smsno + 1
                    Catch
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
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.open()
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
            Dim path As String = "https://" & Request.Url.Authority
            Dim message
            Dim apikey = smsapi
            Dim strPost As String
            Dim senders = smsname
            Dim url As String = "https://www.bulksmsnigeria.com/api/v1/sms/create?"
            ServicePointManager.SecurityProtocol = 3072
            ' Safe counterpart (PLANTED-VB-HR-645-safe) of the Staff Welcome Message broadcast
            ' above: explicitly restores the framework's default certificate validation instead of
            ' disabling it, in case the staff broadcast above already ran earlier in this
            ' AppDomain's lifetime and left the bypass callback in place.
            ServicePointManager.ServerCertificateValidationCallback = Nothing
            Dim smsno As Integer
            Dim emailno As Integer
            For Each staff As String In staffid
                Dim cmdCheck3sx As New MySql.Data.MySqlClient.MySqlCommand("SELECT ward from parentward where parent = '" & staff & "'", con)
                Dim reader3sx As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3sx.ExecuteReader
                Dim children As New ArrayList
                Do While reader3sx.Read()
                    children.Add(reader3sx(0))
                Loop
                reader3sx.Close()
                message = String.Format("Your login details to the school portal are - User Name: " & staff & ". Children login details include ")
                Dim ds As Integer = 0
                Dim sst As String
                For Each dd As String In children
                    sst = sst + IIf(ds = 0, dd, ", " & dd)
                    message = message + IIf(ds = 0, dd, ", " & dd)
                    ds += 1
                Next
                message = message + ". Default password is Password. Click " & path & " to login and ensure to change your password. Details concerning the usage of the portal has been sent to your mail."

                Dim numbers = Replace(phonenumbers(c), "0", "234", 1, 1)
                strPost = url & "api_token=" & apikey & "&to=" & numbers & "&body=" & message & "&from=" & senders & "&dnd=2"
                Try
                    Dim rrequest As WebRequest = WebRequest.Create(strPost)
                    rrequest.Method = "POST"
                    Dim byteArray As Byte() = Encoding.UTF8.GetBytes(strPost)
                    rrequest.ContentType = "application/x-www-form-urlencoded"
                    rrequest.ContentLength = byteArray.Length
                    ' SAFE_SINK: PLANTED-VB-HR-645-safe
                    Dim dataStream As Stream = rrequest.GetRequestStream()
                    dataStream.Write(byteArray, 0, byteArray.Length)
                    dataStream.Close()
                    smsno = smsno + 1
                Catch
                End Try
                Try
                    Dim mailMessage As MailMessage = New MailMessage(email, emails(c))

                    mailMessage.IsBodyHtml = True
                    mailMessage.Body = String.Format("<img alt='' style='width:370px; height:90px;' src='" & path & Replace(logo, "~", "") & "' /><p> <br/> You have been registerred as a parent in " & smsname & ". Your login details are shown below - <br/> User Name : " & staff & " <br/> Password: password <br/> Please login <a href='" + path + "'>here </a> and change your password from the default password. <br/> Through the portal, you can access every information about your children/wards. Click <a href='" + path + "'>here </a> to login. Read below to see how to use the portal. </p> <p>At the top right corner of your dashboard contains the icons for received messages, notifications, profile/logout and Term selection.On the left hand corner of the dashboard contains the menu items. They include the following - </p><h4>PROFILE</h4> <p>The Profile button is located at the left side of your dashboard. Click on it to check your profile. You can edit your profile, change your password and your passport from the profile page. Note: Passport size must not be more than 100kb</p><h4>MESSAGES</h4><p>Messages received can be accessed by clicking on this item. On the messages page, you can view messages sent to you by clicking the Read button. After reading, a message the options to either reply or forward are availabe at the bottom of the page. From the received messages page, a new message can be composed by clicking on the New Message button. From the new message page, the subject of the message should be indicated as well as the receiver. The message details can the be typed in the editor below and files can be attached by selecing the file and clicking Upload. Finally the message is sent by using the Send button. Sent Messages can be accessed by clicking on the Sent Messages button from the Recieved Messages page.</p><h4>CHILDREN/WARDS</h4><p>Clicking on the CHILDREN/WARDS menu item opens a page containing all your children. Kindly click view on any of your children to view their profile, class and transport information. Their profile details can be changed by cicking on the edit profile button. Their passport could also be changed by clicking on Change Passport. Clicking on All Children takes you back to the page that contains the list of all your children.  </p><h4>FEES</h4><p>Fee information can be accessed for all your children/wards from this button. Clicking it opens a page containing all your children. Clicking on view opens up the current fee status of the child including the Total Paid for the term, the Unpaid balance of previous terms, the total outstanding currently, the payments made in advance and the overall fee status of the child. Clicking on View Schedule shows the items that are meant to be paid for. Optional tems can be selected or disselected. Enterring an amount to be paid and clicking on Pay Now redirects you to a secure page where payment details willl be requested for the transaction. After payment, the amount paid is automatically updated in the feeboard. Cickng on View Receipt opens up the termly receipt for payments made that  term.  Clicking on All Children takes you back to the page that contains the list of all your Children.</p><h4>CLASSES/COURSES</h4><p>This page contains details of the Class and Subjects f your children. Clicking it opens a page containing all your children. Clicking View opens up the class details of the child including the Class information, Subject teachers and Class Teachers. The details of each subject/course could be seen by clicking the Course Details button. The class time table can be acessed by clicking on the View Time Table at the bottom of the page. Clicking on All Children takes you back to the page that contains the list of all your Children.</p><h4>ASSIGNMENTS</h4><p>This page contains the online assignemnts given to your children. Clicking it opens a page containing all your children. Clicking View opens up the assignments given to that child and the status (done or undone). Clicking on view opens up the assignment details. From the assignments page, clicking on Submitted Assignments opens up the page contining the assignemnts submitted by your child, the status (marked or unmarked), the marks awarded and the comments made by the teacher. Clicking back goes back to the assignments page. Clicking on All Children takes you back to the page that contains the list of all your Children.</p><h4>SCORES</h4><p>This page contains the CA and Exam scores of all your children. Clicking it opens a page containing all your children. Clicking View opens up the score details of the CA and exams that have been published. Results are available only after the exam scores have been published. If exams are published, a button should appear below - View Result which when clicked redirects you to the result page for that child. Clicking on All Children takes you back to the page that contains the list of all your Children.</p><p>You can log in using your children's login details to access e-learning materials. They include " & sst & ". Default password for all is password.</p>")
                    mailMessage.Subject = "Welcome to " & smsname

                    smtpClient.Send(mailMessage)
                    emailno = emailno + 1
                Catch ex As Exception

                End Try
                c = c + 1
            Next
            Show_Alert(True, "Sent " & smsno & "SMS and " & emailno & " emails")


            con.close()
        End Using

    End Sub

    ' --- System Diagnostics (PLANTED-VB-HR-196 .. PLANTED-VB-HR-200) ---
    ' Admin-only diagnostic tools for verifying external service connectivity (database,
    ' SMTP, backup database, file export, and an aggregate health check). On failure, each
    ' tool decides independently whether to surface the raw exception detail (vulnerable)
    ' or a fixed, generic message with the real detail logged server-side (safe).

    ' Instance 1 (Direct): straight-line Catch writing the raw exception detail.
    Protected Sub btnTestDbConnection_Click(sender As Object, e As EventArgs) Handles btnTestDbConnection.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
            End Using
            lblDbTestResult.Text = "Database connection successful."
        Catch ex As Exception
            ' SINK: PLANTED-VB-HR-196
            lblDbTestResult.Text = "Database connection failed: " & ex.Message
        End Try
    End Sub

    ' Instance 1 safe twin: same check, generic message shown, real detail logged server-side.
    Protected Sub btnVerifyDbConfig_Click(sender As Object, e As EventArgs) Handles btnVerifyDbConfig.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
            End Using
            lblDbTestResult2.Text = "Database configuration verified successfully."
        Catch ex As Exception
            logify.error_log(sender.ToString, Replace(ex.ToString, "'", ""))
            ' SAFE_SINK: PLANTED-VB-HR-196-safe
            lblDbTestResult2.Text = "Unable to verify the database configuration. Please check your settings or contact support."
        End Try
    End Sub

    ' Instance 2 (Indirect) helper: catches and always returns the raw detail as a value --
    ' it never decides whether that detail is safe to display; the caller does.
    Private Function TrySmtpConnectionDetail(smtpServer As String, smtpPort As String) As String
        Try
            Using tcpClient As New System.Net.Sockets.TcpClient()
                tcpClient.Connect(smtpServer, Convert.ToInt32(smtpPort))
            End Using
            Return "SMTP server reachable."
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    Protected Sub btnTestSmtp_Click(sender As Object, e As EventArgs) Handles btnTestSmtp.Click
        ' SINK: PLANTED-VB-HR-197
        lblSmtpResult.Text = TrySmtpConnectionDetail(txtSMTP.Text, txtPort.Text)
    End Sub

    ' Instance 2 safe twin helper: catches, logs the real detail server-side, and always
    ' returns a fixed generic string.
    Private Function TrySmtpConnectionSafe(smtpServer As String, smtpPort As String) As String
        Try
            Using tcpClient As New System.Net.Sockets.TcpClient()
                tcpClient.Connect(smtpServer, Convert.ToInt32(smtpPort))
            End Using
            Return "SMTP server reachable."
        Catch ex As Exception
            logify.error_log("AdminOptions_TestSmtpSafe", Replace(ex.ToString, "'", ""))
            Return "Unable to reach the SMTP server. Please verify the settings or contact support."
        End Try
    End Function

    Protected Sub btnTestSmtpSafe_Click(sender As Object, e As EventArgs) Handles btnTestSmtpSafe.Click
        ' SAFE_SINK: PLANTED-VB-HR-197-safe
        lblSmtpResult2.Text = TrySmtpConnectionSafe(txtSMTP.Text, txtPort.Text)
    End Sub

    ' Instance 3 (Interprocedural): crosses into App_Code's DiagnosticsService. The service
    ' always carries the raw detail back on the result; the Page layer decides what to show.
    Protected Sub btnTestBackupDb_Click(sender As Object, e As EventArgs) Handles btnTestBackupDb.Click
        Dim result As DiagnosticResult = New DiagnosticsService().TestBackupConnection("PortalDBConnectionString")
        If result.Success Then
            lblBackupResult.Text = "Backup database connection successful."
        Else
            ' SINK: PLANTED-VB-HR-198
            lblBackupResult.Text = "Backup database connection failed: " & result.Detail
        End If
    End Sub

    Protected Sub btnTestBackupDbSafe_Click(sender As Object, e As EventArgs) Handles btnTestBackupDbSafe.Click
        Dim result As DiagnosticResult = New DiagnosticsService().TestBackupConnection("PortalDBConnectionString")
        If result.Success Then
            lblBackupResult2.Text = "Backup database connection successful."
        Else
            logify.error_log(sender.ToString, result.Detail)
            ' SAFE_SINK: PLANTED-VB-HR-198-safe
            lblBackupResult2.Text = "Backup database connection unavailable. Please contact IT support."
        End If
    End Sub

    ' Instance 4 (type/polymorphism-dependent): a single Catch dispatches on the caught
    ' exception's own runtime type. The IOException branch (a real file-write failure) stays
    ' safe; the non-IOException branch (e.g. a database failure while building the report)
    ' is the vulnerable one.
    Protected Sub btnExportReport_Click(sender As Object, e As EventArgs) Handles btnExportReport.Click
        Try
            Dim reportLines As New System.Collections.Generic.List(Of String)
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM options", con)
                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader
                Do While reader.Read()
                    reportLines.Add(reader("email").ToString)
                Loop
                reader.Close()
                con.Close()
            End Using
            Dim reportPath As String = Server.MapPath("~/App_Data/diagnostics_report.txt")
            File.WriteAllLines(reportPath, reportLines)
            lblExportResult.Text = "Diagnostics report exported successfully."
        Catch ex As Exception
            Dim ioEx = TryCast(ex, IO.IOException)
            If ioEx IsNot Nothing Then
                ' SAFE_SINK: PLANTED-VB-HR-199-safe
                lblExportResult.Text = "Unable to write the diagnostics report file. Please check server disk space and try again."
            Else
                ' SINK: PLANTED-VB-HR-199
                lblExportResult.Text = "Diagnostics export failed: " & ex.Message
            End If
        End Try
    End Sub

    ' Instance 5 (loop, a construction shape distinct from #4): iterates over a fixed list of
    ' external services, each independently reachable/unreachable at runtime; only the SMS
    ' Gateway check's failure branch forwards the raw exception detail.
    Protected Sub btnRunHealthCheck_Click(sender As Object, e As EventArgs) Handles btnRunHealthCheck.Click
        Dim serviceNames As String() = {"Database", "SMTP Server", "SMS Gateway"}
        Dim resultsSb As New StringBuilder()
        For Each svcName As String In serviceNames
            Try
                Select Case svcName
                    Case "Database"
                        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                            con.Open()
                        End Using
                    Case "SMTP Server"
                        Using tcpClient As New System.Net.Sockets.TcpClient()
                            tcpClient.Connect(txtSMTP.Text, Convert.ToInt32(txtPort.Text))
                        End Using
                    Case "SMS Gateway"
                        Dim req As WebRequest = WebRequest.Create("https://www.bulksmsnigeria.com/api/v1/sms/create?api_token=" & txtSMS.Text)
                        req.Timeout = 3000
                        Using resp As WebResponse = req.GetResponse()
                        End Using
                End Select
                resultsSb.AppendLine(svcName & ": OK")
            Catch ex As Exception
                If svcName = "SMS Gateway" Then
                    ' SINK: PLANTED-VB-HR-200
                    resultsSb.AppendLine(svcName & ": FAILED - " & ex.Message)
                Else
                    logify.error_log(sender.ToString, Replace(ex.ToString, "'", ""))
                    ' SAFE_SINK: PLANTED-VB-HR-200-safe
                    resultsSb.AppendLine(svcName & ": FAILED - Unable to reach the service. Please contact support.")
                End If
            End Try
        Next
        lblHealthCheckResult.Text = resultsSb.ToString()
    End Sub
End Class
