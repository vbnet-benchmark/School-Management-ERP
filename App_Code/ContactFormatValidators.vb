Imports System
Imports System.Text.RegularExpressions

''' <summary>
''' Shared free-text contact-field format-validation helpers, called from the parent self-service
''' profile pages (Content/Parent/editprofile.aspx.vb's Wizard1_NextButtonClick1, and
''' Content/Parent/editchild.aspx.vb's Wizard1_NextButtonClick). This project's own App_Code
''' classes are otherwise all page-independent services taking an already-resolved value as a
''' parameter (see App_Code/AdminAuditLogger.vb's own doc comment on the same convention); this
''' module follows the same shape for format validation instead of audit logging.
''' </summary>
Friend Module ContactFormatValidators

    ''' <summary>
    ''' Validates an email-address format. Both the local part and the domain-label clause nest a
    ''' quantified group inside another quantified group (the OWASP ReDoS cheat sheet's own
    ''' canonical shape, applied to both halves of the address) -- catastrophic on a crafted input
    ''' that is a long run of class-matching characters with no valid suffix.
    ''' </summary>
    Friend Function IsValidEmailFormat(value As String) As Boolean
        ' SINK: PLANTED-VB-HR-288
        Return Regex.IsMatch(value, "^([a-zA-Z0-9_.+-]+)+@(([a-zA-Z0-9-]+)+\.)+[a-zA-Z]{2,}$")
    End Function

    ''' <summary>
    ''' Safe twin's risk class applied to the phone field instead (a sibling free-text contact
    ''' field, called from a different host page) -- bounded with an explicit match timeout so a
    ''' pathological input cannot hang the request thread; a timeout is caught and treated as
    ''' "did not match" (fail closed).
    ''' </summary>
    Friend Function IsValidPhoneFormatSafe(value As String) As Boolean
        Try
            ' SAFE_SINK: PLANTED-VB-HR-288-safe
            Return Regex.IsMatch(value, "^\+?(([0-9])+[\s\-\.]?)+$", RegexOptions.None, TimeSpan.FromMilliseconds(200))
        Catch tex As RegexMatchTimeoutException
            Return False
        End Try
    End Function

End Module
