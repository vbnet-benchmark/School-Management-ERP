Imports System.Web
Imports System.Web.Security

''' <summary>
''' Completion strategy for an SSO-style admin login hand-off (Content/Account/acclogin.aspx's
''' "SSO Login" action): the caller-supplied "Mode" query-string selector chooses which concrete
''' strategy actually finishes the login. Genuine polymorphism (MustInherit/MustOverride),
''' mirroring this project's own App_Code/StudentOwnedRecordRepository.vb precedent ("Genuine
''' polymorphism (MustInherit/MustOverride) ... used here instead of a Select-Case string-built
''' table name"), used here instead of a Select-Case dispatch written inline in the page.
''' </summary>
Public MustInherit Class LoginCompletionStrategyBase
    Public MustOverride Sub CompleteLogin(ByVal loginId As String)
End Class

''' <summary>Standard SSO completion -- the existing, unmodified default behavior.</summary>
Public Class StandardLoginCompletion
    Inherits LoginCompletionStrategyBase

    Public Overrides Sub CompleteLogin(ByVal loginId As String)
        ' SINK: PLANTED-VB-HR-324
        FormsAuthentication.RedirectFromLoginPage(loginId, False)
    End Sub
End Class

''' <summary>Hardened SSO completion -- discards any pre-existing session before completing.</summary>
Public Class SecureLoginCompletion
    Inherits LoginCompletionStrategyBase

    Public Overrides Sub CompleteLogin(ByVal loginId As String)
        HttpContext.Current.Session.Abandon()
        ' SAFE_SINK: PLANTED-VB-HR-324-safe
        FormsAuthentication.RedirectFromLoginPage(loginId, False)
    End Sub
End Class

''' <summary>
''' Resolves which concrete completion strategy to use for a caller-supplied SSO "Mode" selector.
''' NOTE: callers choose the completion strategy purely by this string -- there is no server-side
''' verification that the requested mode is one the current request would ever legitimately use
''' (same disclosed, honest limitation as this project's other caller-selected-dispatch plants).
''' </summary>
Public Class LoginCompletionFactory
    Public Shared Function Resolve(ByVal mode As String) As LoginCompletionStrategyBase
        Select Case mode
            Case "secure"
                Return New SecureLoginCompletion()
            Case Else
                Return New StandardLoginCompletion()
        End Select
    End Function
End Class
