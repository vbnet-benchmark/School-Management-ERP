Imports System.Text
Imports System.Configuration
Imports System.Data
Imports System.Web.UI.WebControls
Imports System.IO
Imports System.Net
Imports System.Web
Imports System.Net.Mail
Imports System.Resources
Partial Class Admin_results
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

    Sub Cummulative()
        Dim db As New DB_Interface
        Dim classes As ArrayList = db.Select_1D("select id from class")
        For Each c In classes
            Dim a As Array = db.Select_Query("select lastscore, lastscore2, total, id from subjectreg where class =  '" & c & "' and session = '" & Session("sessionid") & "'")
            For j = 0 To a.GetLength(1) - 2
                Dim remarks As String = ""
                Dim grade As String = ""
                Dim first As Double = a(1, j)
                Dim second As Double = a(0, j)
                Dim third As Double = a(2, j)
                Dim total As Double = 0
                Dim count As Integer = 0
                If first <> 0 Then
                    total += first
                    count += 1
                End If
                If second <> 0 Then
                    total += second
                    count += 1
                End If
                If third <> 0 Then
                    total += third
                    count += 1
                End If
                Dim avg As Double = total / count
                Dim k As Array = db.Select_Query("SELECT grades.lowest, grades.grade, grades.subject From grades inner join (gradingsystem inner join class on class.gradesystem = gradingsystem.id) on grades.system = gradingsystem.id Where class.id = '" & c & "' order by grades.lowest desc")
                For l = 0 To k.GetLength(1) - 2
                    If avg >= k(0, l) Then
                        grade = k(1, l)
                        remarks = k(2, l)
                        Exit For
                    End If
                Next
                db.Non_Query("update subjectreg set cumaverage = '" & FormatNumber(avg, 2) & "', cumgrade = '" & grade & "', cumremarks = '" & remarks & "' where id = '" & a(3, j) & "'")
            Next
        Next

    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If check.Check_Admin(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try
            If Not IsPostBack Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.Open()



                    Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT scorespublish.* From scorespublish where term = '" & Session("Sessionid") & "'", con)

                    Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader
                    reader3.Read()
                    If reader3.Item("CA1") = True Then
                        lblpublishCA1.Text = "Published"
                        lblpublishCA1.ForeColor = Drawing.Color.Green
                        btnCA1.Text = "Unpublish"
                    Else
                        lblpublishCA1.Text = "Not Published"
                        lblpublishCA1.ForeColor = Drawing.Color.Red
                        btnCA1.Text = "Publish"
                    End If
                    If reader3.Item("CA2") = True Then
                        lblPublishCA2.Text = "Published"
                        lblPublishCA2.ForeColor = Drawing.Color.Green
                        btnCA2.Text = "Unpublish"
                    Else
                        lblPublishCA2.Text = "Not Published"
                        lblPublishCA2.ForeColor = Drawing.Color.Red
                        btnCA2.Text = "Publish"
                    End If
                    If reader3.Item("CA3") = True Then
                        lblPublishCA3.Text = "Published"
                        lblPublishCA3.ForeColor = Drawing.Color.Green
                        btnCA3.Text = "Unpublish"
                    Else
                        lblPublishCA3.Text = "Not Published"
                        lblPublishCA3.ForeColor = Drawing.Color.Red
                        btnCA3.Text = "Publish"
                    End If
                    If reader3.Item("Project") = True Then
                        lblPublishProject.Text = "Published"
                        lblPublishProject.ForeColor = Drawing.Color.Green
                        btnProject.Text = "Unpublish"
                    Else
                        lblPublishProject.Text = "Not Published"
                        lblPublishProject.ForeColor = Drawing.Color.Red
                        btnProject.Text = "Publish"
                    End If
                    If reader3.Item("Exams") = True Then
                        lblPublisgExams.Text = "Published"
                        lblPublisgExams.ForeColor = Drawing.Color.Green
                        btnExams.Text = "Unpublish"
                    Else
                        lblPublisgExams.Text = "Not Published"
                        lblPublisgExams.ForeColor = Drawing.Color.Red
                        btnExams.Text = "Publish"
                    End If


                    con.Close()                End Using
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub





    Protected Sub btnCA1_Click(sender As Object, e As EventArgs) Handles btnCA1.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()

                Dim topublished As Boolean
                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT scorespublish.* From scorespublish where term = ?", con)
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", Session("Sessionid")))
                Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader
                reader3.Read()
                Dim i As Integer
                topublished = reader3.Item("CA1")
                If topublished = True Then
                    i = 0
                Else
                    i = 1
                End If
                reader3.Close()
                Dim cmdCheck30 As New MySql.Data.MySqlClient.MySqlCommand("Update scorespublish set CA1 = '" & i & "' where term = '" & Session("sessionid") & "'", con)
                cmdCheck30.ExecuteNonQuery()
                If i = 1 Then
                    btnCA1.Text = "Unpublish"
                Else
                    btnCA1.Text = "Publish"
                End If
               
            After_Publish()
            Dim a As String = "CA 1"

                If i = 1 Then Mass_Mail(a)
                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Private Sub After_Publish()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT scorespublish.* From  scorespublish where term = ?", con)
            cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", session("sessionid")))
            Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader
            reader3.Read()
            If reader3.Item("CA1") = True Then
                lblpublishCA1.Text = "Published"
                lblpublishCA1.ForeColor = Drawing.Color.Green
            Else
                lblpublishCA1.Text = "Not Published"
                lblpublishCA1.ForeColor = Drawing.Color.Red
            End If
            If reader3.Item("CA2") = True Then
                lblpublishCA2.Text = "Published"
                lblpublishCA2.ForeColor = Drawing.Color.Green
            Else
                lblpublishCA2.Text = "Not Published"
                lblpublishCA2.ForeColor = Drawing.Color.Red
            End If
            If reader3.Item("CA3") = True Then
                lblPublishCA3.Text = "Published"
                lblPublishCA3.ForeColor = Drawing.Color.Green
            Else
                lblPublishCA3.Text = "Not Published"
                lblPublishCA3.ForeColor = Drawing.Color.Red
            End If
            If reader3.Item("Project") = True Then
                lblPublishProject.Text = "Published"
                lblPublishProject.ForeColor = Drawing.Color.Green
            Else
                lblPublishProject.Text = "Not Published"
                lblPublishProject.ForeColor = Drawing.Color.Red
            End If
            If reader3.Item("Exams") = True Then
                lblPublisgExams.Text = "Published"
                lblPublisgExams.ForeColor = Drawing.Color.Green
            Else
                lblPublisgExams.Text = "Not Published"
                lblPublisgExams.ForeColor = Drawing.Color.Red
            End If
            con.close()        End Using
    End Sub

    Protected Sub btnCA2_Click(sender As Object, e As EventArgs) Handles btnCA2.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()

                Dim topublished As Boolean
                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT scorespublish.* From scorespublish where term = ?", con)
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", Session("sessionid")))
                Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader
                reader3.Read()
                topublished = reader3.Item("CA2")
                Dim i As Integer
                If topublished = True Then
                    i = 0
                Else
                    i = 1
                End If
                reader3.Close()

                Dim cmdCheck30 As New MySql.Data.MySqlClient.MySqlCommand("Update scorespublish set CA2 = '" & i & "' where term = '" & Session("sessionid") & "'", con)
                cmdCheck30.ExecuteNonQuery()
                If i = 1 Then
                    btnCA2.Text = "Unpublish"
                Else
                    btnCA2.Text = "Publish"
                End If
               
            After_Publish()
            Dim a As String = "CA 2"
                If i = 1 Then Mass_Mail(a)
                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnCA3_Click(sender As Object, e As EventArgs) Handles btnCA3.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim topublished As Boolean
                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT scorespublish.* From scorespublish where term = ?", con)
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", Session("sessionid")))
                Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader
                reader3.Read()
                topublished = reader3.Item("CA3")
                Dim i As Integer
                If topublished = True Then
                    i = 0
                Else
                    i = 1
                End If
                reader3.Close()

                Dim cmdCheck30 As New MySql.Data.MySqlClient.MySqlCommand("Update scorespublish set CA3 = '" & i & "' where term = '" & Session("sessionid") & "'", con)
                cmdCheck30.ExecuteNonQuery()
                If i = 1 Then
                    btnCA3.Text = "Unpublish"
                Else
                    btnCA3.Text = "Publish"
                End If
               

            After_Publish()
            Dim a As String = "CA 3"
                If i = 1 Then Mass_Mail(a)
                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnProject_Click(sender As Object, e As EventArgs) Handles btnProject.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim topublished As Boolean
                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT scorespublish.* From scorespublish where term = ?", con)
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", Session("sessionid")))
                Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader
                reader3.Read()
                topublished = reader3.Item("Project")
                Dim i As Integer
                If topublished = True Then
                    i = 0
                Else
                    i = 1
                End If
                reader3.Close()
                Dim cmdCheck30 As New MySql.Data.MySqlClient.MySqlCommand("Update scorespublish set CA1 = '" & i & "', CA2 = '" & i & "', CA3 = '" & i & "', project = '" & i & "' where term = '" & Session("sessionid") & "'", con)
                cmdCheck30.ExecuteNonQuery()
                If i = 1 Then
                    btnProject.Text = "Unpublish"
                Else
                    btnProject.Text = "Publish"
                End If
              
            After_Publish()
            Dim a As String = "Project"
                If i = 1 Then Mass_Mail(a)
                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnExams_Click(sender As Object, e As EventArgs) Handles btnExams.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim topublished As Boolean
                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT scorespublish.* From scorespublish where term = ?", con)
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", Session("sessionid")))
                Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader
                reader3.Read()
                topublished = reader3.Item("Exams")
                Dim i As Integer
                If topublished = True Then
                    i = 0
                Else
                    i = 1
                End If
                reader3.Close()
                Dim cmdCheck30 As New MySql.Data.MySqlClient.MySqlCommand("Update scorespublish set CA1 = '" & i & "', CA2 = '" & i & "', CA3 = '" & i & "',  Exams = '" & i & "' where term = '" & Session("sessionid") & "'", con)
                cmdCheck30.ExecuteNonQuery()
                If i = 1 Then
                    btnExams.Text = "Unpublish"
                Else
                    btnExams.Text = "Publish"
                End If
              
            After_Publish()
            Dim a As String = "Exams"
                If i = 1 Then Mass_Mail(a)
                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Private Sub Mass_Mail(assessment As String)
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdSelect2a As New MySql.Data.MySqlClient.MySqlCommand("SELECT smsno FROM Session WHERE id = ?", con)
            cmdSelect2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionId")))
            Dim reader2a As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2a.ExecuteReader
            reader2a.Read()
            Dim smsno = reader2a(0)
            reader2a.Close()

            Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT email, password, port, smtp, smsapi, smsname from options", con)
            cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", Session("sessionid")))
            Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader
            reader3.Read()
            Dim email As String = reader3(0).ToString
            Dim password As String = reader3(1).ToString
            Dim port As String = reader3(2).ToString
            Dim smtp As String = reader3(3).ToString
            Dim smsapi As String = reader3(4).ToString
            Dim smsname As String = reader3(5).ToString
            reader3.Close()
            Dim smtpClient As SmtpClient = New SmtpClient(smtp, port)
            smtpClient.EnableSsl = True
            smtpClient.Credentials = New System.Net.NetworkCredential(email, password)
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network
            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentprofile.email, studentsprofile.surname, studentsprofile.admno, parentprofile.phone, parentprofile.parentid, class.type  from parentward inner join parentprofile on parentprofile.parentid = parentward.parent inner join (studentsummary inner join class on class.id = studentsummary.class) on parentward.ward = studentsummary.student inner join studentsprofile on parentward.ward = studentsprofile.admno where studentsummary.status = '" & 1 & "' and studentsummary.session = '" & Session("Sessionid") & "'", con)
            Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
            Dim msgs As Integer
            Dim emails As Integer
            Dim message
            Dim path As String = "http://" & Request.Url.Authority & "/Content/"
            Dim strPost As String
            Dim apikey = smsapi
            Dim first As Array = Split(smsname, " ")
            Dim sender = first(0)
            Dim url As String = "http://api.ebulksms.com:8080/sendsms?"
            Dim finished As String = ""
            Dim unsent As Integer = 0
            Dim unsentmail As Integer = 0
            Do While reader2.Read()
                Try
                    Dim mailMessage As MailMessage = New MailMessage(email, reader2(0))

                    mailMessage.IsBodyHtml = True
                    Dim notpath As String = ""
                    Select Case assessment
                        Case "CA 1"
                            notpath = "~/content/student/myscores.aspx?" + reader2(2).ToString + "," + Session("Sessionid")
                            message = "1st CA published for " + reader2(1) + ". Assess it through this link. " + path + "student/myscores.aspx?" + reader2(2).ToString + "," + Session("Sessionid")
                            mailMessage.Body = String.Format("This is to inform you 1st CA have been published for " + reader2(1) + ". Assess it through this link <a href=" + path + "student/myscores.aspx?" + reader2(2).ToString + "," + Session("Sessionid") + "> Check Assessment</a>")
                            mailMessage.Subject = "Assessment Notification"
                        Case "CA 2"
                            notpath = "~/content/student/myscores.aspx?" + reader2(2).ToString + "," + Session("Sessionid")
                            message = "2nd CA published for " + reader2(1) + ". Assess it through this link. " & path & "student/myscores.aspx?" + reader2(2).ToString + "," + Session("Sessionid")
                            mailMessage.Body = String.Format("This is to inform you 2nd CA have been published for " + reader2(1) + ". Assess it through this link <a href=" + path + "student/myscores.aspx?" + reader2(2).ToString + "," + Session("Sessionid") + "> Check Assessment</a>")
                            mailMessage.Subject = "Assessment Notification"
                        Case "CA 3"
                            notpath = "~/content/student/myscores.aspx?" + reader2(2).ToString + "," + Session("Sessionid")
                            message = String.Format("3rd CA published for " + reader2(1) + ". Assess it through this link. " + path + "student/myscores.aspx?" + reader2(2).ToString + "," + Session("Sessionid"))
                            mailMessage.Body = String.Format("This is to inform you 3rd CA have been published for " + reader2(1) + ". Assess it through this link <a href=" + path + "student/myscores.aspx?" + reader2(2).ToString + "," + Session("Sessionid") + "> Check Assessment</a>")
                            mailMessage.Subject = "Assessment Notification"
                        Case "Project"
                            notpath = "~/content/student/myscores.aspx?" + reader2(2).ToString + "," + Session("Sessionid")
                            message = String.Format("Continuous assessments published for " + reader2(1) + ". Assess it through this link. " + path + "student/myscores.aspx?" + reader2(2).ToString + "," + Session("Sessionid"))
                            mailMessage.Body = String.Format("This is to inform you that continuous assessments have been published for " + reader2(1) + ". Assess it through this link <a href=" + path + "student/myscores.aspx?" + reader2(2).ToString + "," + Session("Sessionid") + "> Check Assessment</a>")
                            mailMessage.Subject = "Assessment Notification"
                        Case "Exams"
                            notpath = "~/content/student/result.aspx?" + reader2(2).ToString + "," + Session("Sessionid")
                            mailMessage.Body = String.Format("This is to inform you that Results have been published for " + reader2(1) + ". Assess it through this link <a href=" + path + "student/result.aspx?" + reader2(2).ToString + "," + Session("Sessionid") + "> Check Result</a>")
                            mailMessage.Subject = "Assessment Notification"
                            message = String.Format("Results published for " + reader2(1) + ". Assess it through this link. " + path + "student/result.aspx?" + reader2(2).ToString + "," + Session("Sessionid"))
                    End Select

                    Try
                        If reader2(5).ToString <> "K.G 1 Special" Then
                            smtpClient.Send(mailMessage)
                            emails = emails + 1
                        Else
                            If assessment = "Exams" Then
                                smtpClient.Send(mailMessage)
                                emails = emails + 1
                            End If
                        End If
                    Catch
                        unsentmail = unsentmail + 1
                    End Try

                    Dim numbers = reader2(3)
                    logify.Notifications(assessment & " has been published for " + reader2(1), reader2(4), Session("staffid"), "Admin", notpath, "")
                    logify.Notifications(assessment & " has been published.", reader2(2), Session("staffid"), "Admin", notpath, "")
                    strPost = url & "username=simplehopee@yahoo.com&apikey=" & apikey & "&sender=" & sender & "&messagetext=" & message & "&flash=0&recipients=" & numbers & ""
                    Try
                        
                            If assessment = "Exams" Then
                                Dim fr As System.Net.HttpWebRequest
                                Dim targetURI As New Uri(strPost)

                                fr = DirectCast(HttpWebRequest.Create(targetURI), System.Net.HttpWebRequest)
                                If (fr.GetResponse().ContentLength > 0) Then
                                    Dim str As New System.IO.StreamReader(fr.GetResponse().GetResponseStream())
                                    str.ReadToEnd()
                                    str.Close()
                                End If
                                smsno = smsno - 1
                                msgs = msgs + 1
                            End If

                    Catch ex As Exception
                        unsent = unsent + 1
                    End Try
                   
                Catch

                End Try
            Loop
            reader2.Close()

          

            Show_Alert(True, "Assesment Published. Sent " & msgs & " SMS notifications and " & emails & " email notifications." & finished & IIf(unsentmail > 0, "Could not send " & unsentmail & " emails. ", "") & IIf(unsent > 0, "Could not send " & unsent & " SMS.", ""))
            logify.log(Session("staffid"), assessment & " was published")

            con.close()        End Using

    End Sub
    Sub send_Sms()
        
    End Sub

    ''' <summary>
    ''' External-moderator notification (Instance: Indirect) -- lets Admin let an external
    ''' examination moderator (not a portal user) know the latest CA batch was published, routed
    ''' through a private same-class helper that performs the actual send.
    ''' </summary>
    Protected Sub btnNotifyModerator_Click(sender As Object, e As EventArgs) Handles btnNotifyModerator.Click
        Try
            NotifyExternalModerator(txtModeratorEmail.Text)
            Show_Alert(True, "Moderator notified.")
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    ''' <summary>
    ''' Safe twin of btnNotifyModerator_Click.
    ''' </summary>
    Protected Sub btnNotifyModeratorSafe_Click(sender As Object, e As EventArgs) Handles btnNotifyModeratorSafe.Click
        Try
            NotifyExternalModeratorSafe(txtModeratorEmailSafe.Text)
            Show_Alert(True, "Moderator notified.")
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    ''' <summary>
    ''' Builds and sends a short notice to an external examination moderator who has no account on
    ''' the portal. The caller's raw text reaches this helper and is handed straight to the
    ''' message's recipient collection with no check that it is a single, well-formed address.
    ''' </summary>
    Private Sub NotifyExternalModerator(moderatorEmail As String)
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.open()
            Dim cmdOpts557 As New MySql.Data.MySqlClient.MySqlCommand("SELECT email, password, port, smtp from options", con)
            Dim reader557 As MySql.Data.MySqlClient.MySqlDataReader = cmdOpts557.ExecuteReader
            reader557.Read()
            Dim fromEmail As String = reader557(0).ToString
            Dim fromPassword As String = reader557(1).ToString
            Dim mailPort As String = reader557(2).ToString
            Dim mailSmtp As String = reader557(3).ToString
            reader557.Close()

            Dim smtpClient557 As New SmtpClient(mailSmtp, mailPort)
            smtpClient557.EnableSsl = True
            smtpClient557.Credentials = New System.Net.NetworkCredential(fromEmail, fromPassword)
            smtpClient557.DeliveryMethod = SmtpDeliveryMethod.Network
            Dim moderatorMessage As New MailMessage(fromEmail, fromEmail)
            ' SINK: PLANTED-VB-HR-557
            moderatorMessage.To.Add(moderatorEmail)
            moderatorMessage.IsBodyHtml = True
            moderatorMessage.Subject = "Assessment Batch Published"
            moderatorMessage.Body = "The latest continuous-assessment batch has just been published to parents and students."
            smtpClient557.Send(moderatorMessage)
            con.close()
        End Using
    End Sub

    ''' <summary>
    ''' Safe twin of NotifyExternalModerator -- validates via the single-address MailAddress
    ''' constructor (rejects a comma/semicolon-bearing string with a FormatException) and adds the
    ''' already-parsed object, never the raw string.
    ''' </summary>
    Private Sub NotifyExternalModeratorSafe(moderatorEmail As String)
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.open()
            Dim cmdOpts557s As New MySql.Data.MySqlClient.MySqlCommand("SELECT email, password, port, smtp from options", con)
            Dim reader557s As MySql.Data.MySqlClient.MySqlDataReader = cmdOpts557s.ExecuteReader
            reader557s.Read()
            Dim fromEmail As String = reader557s(0).ToString
            Dim fromPassword As String = reader557s(1).ToString
            Dim mailPort As String = reader557s(2).ToString
            Dim mailSmtp As String = reader557s(3).ToString
            reader557s.Close()

            Dim smtpClient557s As New SmtpClient(mailSmtp, mailPort)
            smtpClient557s.EnableSsl = True
            smtpClient557s.Credentials = New System.Net.NetworkCredential(fromEmail, fromPassword)
            smtpClient557s.DeliveryMethod = SmtpDeliveryMethod.Network
            Dim moderatorMessage As New MailMessage(fromEmail, fromEmail)
            Try
                Dim validated As New MailAddress(moderatorEmail)
                ' SAFE_SINK: PLANTED-VB-HR-557-safe
                moderatorMessage.To.Add(validated)
            Catch fex As FormatException
                con.close()
                Exit Sub
            End Try
            moderatorMessage.IsBodyHtml = True
            moderatorMessage.Subject = "Assessment Batch Published"
            moderatorMessage.Body = "The latest continuous-assessment batch has just been published to parents and students."
            smtpClient557s.Send(moderatorMessage)
            con.close()
        End Using
    End Sub

    ''' <summary>
    ''' Share-with-external-reviewer (Instance: Interprocedural) -- routes through a new cross-file
    ''' service class, App_Code/ResultNotificationService.vb, which performs the actual send.
    ''' </summary>
    Protected Sub btnShareWithReviewer_Click(sender As Object, e As EventArgs) Handles btnShareWithReviewer.Click
        Try
            Dim svc558 As New ResultNotificationService
            svc558.ShareResultsWithExternalReviewer(txtReviewerEmail.Text, "Latest Results")
            Show_Alert(True, "Reviewer notified.")
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    ''' <summary>
    ''' Safe twin of btnShareWithReviewer_Click.
    ''' </summary>
    Protected Sub btnShareWithReviewerSafe_Click(sender As Object, e As EventArgs) Handles btnShareWithReviewerSafe.Click
        Try
            Dim svc558s As New ResultNotificationService
            svc558s.ShareResultsWithExternalReviewerSafe(txtReviewerEmailSafe.Text, "Latest Results")
            Show_Alert(True, "Reviewer notified.")
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

   
End Class
