''' <summary>
''' Server-side roster resolving a caller-supplied channel type to the concrete
''' IIntegrationChannel implementor that handles it, for the "Integration Channel Dispatch"
''' feature on Content/Admin/externalintegrations.aspx.vb. Mirrors this project's own
''' App_Code/StudentImportParserFactory "resolve to a single shared call site" shape -- the
''' difference here is that the roster resolves a TYPE (which class handles the request) from a
''' caller-supplied STRING, rather than from the uploaded content itself.
''' </summary>
Public Class IntegrationChannelRosterService

    ''' <summary>
    ''' Defaults to the webhook channel for "webhook" and for any unrecognized/unsupplied type,
    ''' so an admin testing a new integration doesn't need to get the type spelling exactly right.
    ''' </summary>
    Public Shared Function GetChannel(ByVal channelType As String) As IIntegrationChannel
        Select Case If(channelType, "").ToUpperInvariant()
            Case "LOG"
                Return New AuditLogIntegrationChannel()
            Case Else
                Return New WebhookIntegrationChannel()
        End Select
    End Function

    ''' <summary>
    ''' Dispatches through the IIntegrationChannel reference returned by GetChannel -- the actual
    ''' concrete type (and therefore whether this reaches the network at all) depends on the
    ''' requested channel type.
    ''' </summary>
    Public Shared Function Dispatch(ByVal channelType As String, ByVal destination As String, ByVal message As String) As String
        Dim channel As IIntegrationChannel = GetChannel(channelType)
        Return channel.Send(destination, message)
    End Function

    ''' <summary>
    ''' Safe counterpart: dispatches to the same polymorphic channel's SendSafe method.
    ''' </summary>
    Public Shared Function DispatchSafe(ByVal channelType As String, ByVal destination As String, ByVal message As String) As String
        Dim channel As IIntegrationChannel = GetChannel(channelType)
        Return channel.SendSafe(destination, message)
    End Function

End Class
