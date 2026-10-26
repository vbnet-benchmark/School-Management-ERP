''' <summary>
''' Resolves which INotificationSender implementation handles a "Share Statement" request, based
''' on a client-compatibility toggle (legacy plain-text format vs. the modern default) that makes
''' no trust or security claim of any kind -- see App_Code/INotificationSender.vb.
''' </summary>
Public Module NotificationSenderFactory
    Public Function GetSender(useLegacyFormat As Boolean) As INotificationSender
        If useLegacyFormat Then
            Return New RawEmailNotifier
        Else
            Return New ValidatedEmailNotifier
        End If
    End Function
End Module
