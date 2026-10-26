''' <summary>
''' The "log" integration channel -- writes the message to the application activity log instead
''' of making any outbound network call. Selected by IntegrationChannelRosterService.GetChannel
''' whenever the caller requests channel type "log". Exists so the Dispatch/DispatchSafe call
''' site is a genuine polymorphic call: not every concrete IIntegrationChannel implementor
''' touches the network, and which one runs depends entirely on the caller-supplied channel type.
''' </summary>
Public Class AuditLogIntegrationChannel
    Implements IIntegrationChannel

    Public Function Send(destination As String, message As String) As String Implements IIntegrationChannel.Send
        Dim logger As New notify()
        logger.log("integration-dispatch", "[" & destination & "] " & message)
        Return "logged"
    End Function

    Public Function SendSafe(destination As String, message As String) As String Implements IIntegrationChannel.SendSafe
        Dim logger As New notify()
        logger.log("integration-dispatch", "[" & destination & "] " & message)
        Return "logged"
    End Function
End Class
