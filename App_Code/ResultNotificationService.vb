Imports System.Net.Mail
Imports System.Configuration

''' <summary>
''' Lets Admin_results (Content/Admin/publishca.aspx) share the latest published assessment
''' results with an external examination-board reviewer who has no account on the portal. A
''' small, standalone service class -- new code, not routed through the project's existing
''' App_Code/Messages.vb wrapper -- added for this one quick admin-facing notification feature.
''' </summary>
Public Class ResultNotificationService

    ''' <summary>
    ''' Vulnerable: the caller's raw reviewer-address string is handed straight to the message's
    ''' recipient collection, with no check that it resolves to a single, well-formed address.
    ''' </summary>
    Public Sub ShareResultsWithExternalReviewer(reviewerEmail As String, assessmentLabel As String)
        Using con As New MySql.Data.MySqlClient.MySqlConnection(ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT email, password, port, smtp from options", con)
            Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader
            reader.Read()
            Dim fromEmail As String = reader(0).ToString
            Dim fromPassword As String = reader(1).ToString
            Dim mailPort As String = reader(2).ToString
            Dim mailSmtp As String = reader(3).ToString
            reader.Close()

            Dim smtpClient As New SmtpClient(mailSmtp, mailPort)
            smtpClient.EnableSsl = True
            smtpClient.Credentials = New System.Net.NetworkCredential(fromEmail, fromPassword)
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network
            Dim reviewMessage As New MailMessage(fromEmail, fromEmail)
            ' SINK: PLANTED-VB-HR-558
            reviewMessage.To.Add(reviewerEmail)
            reviewMessage.IsBodyHtml = True
            reviewMessage.Subject = "Results Published For External Review"
            reviewMessage.Body = assessmentLabel & " has just been published and is ready for your review."
            smtpClient.Send(reviewMessage)
            con.Close()
        End Using
    End Sub

    ''' <summary>
    ''' Safe twin of ShareResultsWithExternalReviewer -- validates via the single-address
    ''' MailAddress constructor and adds the already-parsed object, never the raw string.
    ''' </summary>
    Public Sub ShareResultsWithExternalReviewerSafe(reviewerEmail As String, assessmentLabel As String)
        Using con As New MySql.Data.MySqlClient.MySqlConnection(ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT email, password, port, smtp from options", con)
            Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader
            reader.Read()
            Dim fromEmail As String = reader(0).ToString
            Dim fromPassword As String = reader(1).ToString
            Dim mailPort As String = reader(2).ToString
            Dim mailSmtp As String = reader(3).ToString
            reader.Close()

            Dim smtpClient As New SmtpClient(mailSmtp, mailPort)
            smtpClient.EnableSsl = True
            smtpClient.Credentials = New System.Net.NetworkCredential(fromEmail, fromPassword)
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network
            Dim reviewMessage As New MailMessage(fromEmail, fromEmail)
            Try
                Dim validated As New MailAddress(reviewerEmail)
                ' SAFE_SINK: PLANTED-VB-HR-558-safe
                reviewMessage.To.Add(validated)
            Catch fex As FormatException
                con.Close()
                Exit Sub
            End Try
            reviewMessage.IsBodyHtml = True
            reviewMessage.Subject = "Results Published For External Review"
            reviewMessage.Body = assessmentLabel & " has just been published and is ready for your review."
            smtpClient.Send(reviewMessage)
            con.Close()
        End Using
    End Sub

End Class
