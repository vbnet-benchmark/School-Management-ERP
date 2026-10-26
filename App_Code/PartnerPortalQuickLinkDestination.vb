Imports System.Web

''' <summary>
''' The "partner" Quick Links destination -- sends the visitor on to an external partner-portal
''' URL (exam board, learning platform, ministry portal, ...) configured as the shortcut's own
''' target. Selected by QuickLinkRosterService.GetDestination whenever the caller requests link
''' type "partner".
''' </summary>
Public Class PartnerPortalQuickLinkDestination
    Implements IQuickLinkDestination

    ''' <summary>
    ''' The target is used exactly as the shortcut supplied it -- reached only through the
    ''' IQuickLinkDestination reference the roster hands back, so which concrete Go this call
    ''' ends up in depends on the requested link type.
    ''' </summary>
    Public Sub Go(requestedTarget As String) Implements IQuickLinkDestination.Go
        HttpContext.Current.Response.Redirect(requestedTarget) ' SINK: PLANTED-VB-HR-519
    End Sub

    ''' <summary>
    ''' Safe counterpart: the target is validated against RedirectTargetGuard before use.
    ''' </summary>
    Public Sub GoSafe(requestedTarget As String) Implements IQuickLinkDestination.GoSafe
        If RedirectTargetGuard.IsLocalRedirectTarget(requestedTarget) Then
            HttpContext.Current.Response.Redirect(requestedTarget) ' SAFE_SINK: PLANTED-VB-HR-519-safe
        Else
            HttpContext.Current.Response.Redirect("~/content/dashboards/staffdashboard.aspx")
        End If
    End Sub
End Class
