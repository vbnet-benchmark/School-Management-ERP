''' <summary>
''' Implemented by any class that can deliver an account-statement notification to a single
''' recipient. Content/Account/transactions.aspx's "Share Statement" feature resolves a concrete
''' implementation at runtime via NotificationSenderFactory.GetSender, based on a client-
''' compatibility toggle ("Use legacy plain-text format") that makes no trust or security claim
''' of any kind -- both implementations are described purely in terms of how they build the
''' outgoing message, never in terms of who is allowed to request which.
''' </summary>
Public Interface INotificationSender
    Sub Notify(recipientEmail As String, subjectText As String, bodyText As String)
End Interface
