''' <summary>
''' Server-side roster resolving a caller-supplied link type to the concrete
''' IQuickLinkDestination implementor that handles it, for the staff dashboard's "Quick Links"
''' shortcut panel.
''' </summary>
Public Class QuickLinkRosterService

    ''' <summary>
    ''' Defaults to the dashboard destination for "dashboard" and for any unrecognized/unsupplied
    ''' type, so a shortcut with a missing or misspelled link type still lands the visitor
    ''' somewhere inside the application rather than failing outright.
    ''' </summary>
    Public Shared Function GetDestination(ByVal linkType As String) As IQuickLinkDestination
        Select Case If(linkType, "").ToUpperInvariant()
            Case "PARTNER"
                Return New PartnerPortalQuickLinkDestination()
            Case Else
                Return New InternalQuickLinkDestination()
        End Select
    End Function

    ''' <summary>
    ''' Dispatches through the IQuickLinkDestination reference returned by GetDestination -- the
    ''' actual concrete type (and therefore whether this leaves the application at all) depends
    ''' on the requested link type.
    ''' </summary>
    Public Shared Sub Dispatch(ByVal linkType As String, ByVal requestedTarget As String)
        Dim destination As IQuickLinkDestination = GetDestination(linkType)
        destination.Go(requestedTarget)
    End Sub

    ''' <summary>
    ''' Safe counterpart: dispatches to the same polymorphic destination's GoSafe method.
    ''' </summary>
    Public Shared Sub DispatchSafe(ByVal linkType As String, ByVal requestedTarget As String)
        Dim destination As IQuickLinkDestination = GetDestination(linkType)
        destination.GoSafe(requestedTarget)
    End Sub

End Class
