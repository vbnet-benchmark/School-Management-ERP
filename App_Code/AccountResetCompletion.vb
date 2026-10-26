Imports System.Web
Imports System.Web.Security

''' <summary>
''' Shared completion helper for the password-reset confirmation-code flow: passwordreset.aspx.vb
''' writes Session("PendingResetCode_" &amp; id) once it has verified the requester owns a real
''' staff/parent account and emailed them a confirmation code; passwordresetconfirm.aspx(.vb)
''' later verifies that code and delegates here to actually complete the login. Standalone
''' vulnerable/safe-twin carrier, mirroring this project's own App_Code service-class convention
''' (StudentWorkspaceService.vb, TeachingResourceService.vb) of a shared class taking an
''' already-resolved value in rather than resolving one itself.
''' </summary>
Public Class AccountResetCompletion

    ''' <summary>
    ''' Completes the password-reset login hand-off for <paramref name="loginId"/> without first
    ''' discarding whatever session the requester already held.
    ''' </summary>
    Public Shared Sub CompleteReset(ByVal loginId As String)
        HttpContext.Current.Session("StaffID") = loginId
        HttpContext.Current.Session("staff") = loginId
        ' SINK: PLANTED-VB-HR-323
        FormsAuthentication.RedirectFromLoginPage(loginId, False)
    End Sub

    ''' <summary>
    ''' Same password-reset login hand-off as <see cref="CompleteReset"/>, but discards the
    ''' requester's pre-existing session (and therefore its SessionID) before completing.
    ''' </summary>
    Public Shared Sub CompleteResetSecure(ByVal loginId As String)
        HttpContext.Current.Session.Abandon()
        HttpContext.Current.Session("StaffID") = loginId
        HttpContext.Current.Session("staff") = loginId
        ' SAFE_SINK: PLANTED-VB-HR-323-safe
        FormsAuthentication.RedirectFromLoginPage(loginId, False)
    End Sub

End Class
