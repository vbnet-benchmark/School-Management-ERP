Imports System.Net.Mail

''' <summary>
''' Legacy-format notifier, resolved when the caller asks for the older plain-text-compatible
''' format (NotificationSenderFactory.GetSender(True)). The recipient address reaches the
''' message's recipient collection with no check that it resolves to a single, well-formed
''' address.
''' </summary>
Public Class RawEmailNotifier
    Implements INotificationSender

    Public Sub Notify(recipientEmail As String, subjectText As String, bodyText As String) Implements INotificationSender.Notify
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
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
            Dim statementMessage As New MailMessage(fromEmail, fromEmail)
            ' SINK: PLANTED-VB-HR-559
            statementMessage.To.Add(recipientEmail)
            statementMessage.Subject = subjectText
            statementMessage.Body = bodyText
            smtpClient.Send(statementMessage)
            con.Close()
        End Using
    End Sub
End Class
