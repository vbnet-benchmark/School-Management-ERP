Imports System.Net.Mail

''' <summary>
''' Lets Content/Account/transactions.aspx's debt-reminder feature also CC a short list of
''' additional guardian addresses typed in alongside the regular SMS reminder run, for a guardian
''' who wants a copy of every reminder but is not the primary contact on file. New code, not
''' routed through the project's existing App_Code/Messages.vb wrapper.
''' </summary>
Friend Module FeeReminderMailer

    ''' <summary>
    ''' Vulnerable: every element of the raw CC list is handed straight to the message's recipient
    ''' collection inside the loop, with no check that any one of them is a single, well-formed
    ''' address -- a single element containing an embedded comma smuggles in an extra recipient.
    ''' </summary>
    Friend Sub SendDebtReminderCcList(fromEmail As String, fromPassword As String, mailSmtp As String, mailPort As String, ccAddresses As String())
        Dim smtpClient As New SmtpClient(mailSmtp, mailPort)
        smtpClient.EnableSsl = True
        smtpClient.Credentials = New System.Net.NetworkCredential(fromEmail, fromPassword)
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network
        Dim reminderMessage As New MailMessage(fromEmail, fromEmail)
        reminderMessage.IsBodyHtml = True
        reminderMessage.Subject = "Fee Debt Reminder - Additional Copy"
        reminderMessage.Body = "You are receiving a copy of this term's fee debt reminder run."
        For Each ccAddress As String In ccAddresses
            ' SINK: PLANTED-VB-HR-560
            reminderMessage.CC.Add(ccAddress)
        Next
        smtpClient.Send(reminderMessage)
    End Sub

    ''' <summary>
    ''' Safe twin of SendDebtReminderCcList -- validates each element via the single-address
    ''' MailAddress constructor before it is added; a malformed element is skipped, not fatal to
    ''' the rest of the batch.
    ''' </summary>
    Friend Sub SendDebtReminderCcListSafe(fromEmail As String, fromPassword As String, mailSmtp As String, mailPort As String, ccAddresses As String())
        Dim smtpClient As New SmtpClient(mailSmtp, mailPort)
        smtpClient.EnableSsl = True
        smtpClient.Credentials = New System.Net.NetworkCredential(fromEmail, fromPassword)
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network
        Dim reminderMessage As New MailMessage(fromEmail, fromEmail)
        reminderMessage.IsBodyHtml = True
        reminderMessage.Subject = "Fee Debt Reminder - Additional Copy"
        reminderMessage.Body = "You are receiving a copy of this term's fee debt reminder run."
        For Each ccAddress As String In ccAddresses
            Try
                Dim validated As New MailAddress(ccAddress)
                ' SAFE_SINK: PLANTED-VB-HR-560-safe
                reminderMessage.CC.Add(validated)
            Catch fex As FormatException
                ' Skip the malformed element; the rest of the batch still goes out.
            End Try
        Next
        smtpClient.Send(reminderMessage)
    End Sub

End Module
