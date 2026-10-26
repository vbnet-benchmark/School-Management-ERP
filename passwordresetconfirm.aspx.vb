Imports System.Configuration

''' <summary>
''' Second hop of the password-reset confirmation-code flow: passwordreset.aspx.vb writes
''' Session("PendingResetCode_" &amp; id) once it has verified the requester owns a real
''' staff/parent account and emailed them a confirmation code; this page verifies that code and
''' delegates to the shared App_Code/AccountResetCompletion.vb class to actually complete the
''' login.
''' </summary>
Partial Class _PasswordResetConfirm
    Inherits System.Web.UI.Page

    Protected Sub btnConfirm_Click(sender As Object, e As EventArgs)
        Dim pendingCode As String = CStr(Session("PendingResetCode_" & txtConfirmId.Text))
        If Not String.IsNullOrEmpty(pendingCode) AndAlso pendingCode = txtConfirmCode.Text Then
            AccountResetCompletion.CompleteReset(txtConfirmId.Text)
        Else
            lblConfirmError.Text = "The confirmation code you entered is incorrect."
        End If
    End Sub

    Protected Sub btnConfirmSecure_Click(sender As Object, e As EventArgs)
        Dim pendingCode As String = CStr(Session("PendingResetCode_" & txtConfirmId.Text))
        If Not String.IsNullOrEmpty(pendingCode) AndAlso pendingCode = txtConfirmCode.Text Then
            AccountResetCompletion.CompleteResetSecure(txtConfirmId.Text)
        Else
            lblConfirmError.Text = "The confirmation code you entered is incorrect."
        End If
    End Sub

    ''' <summary>
    ''' Password-Reset Continue Link, safe counterpart (PLANTED-VB-HR-517-safe): same "dest" page
    ''' link as passwordreset.aspx.vb's reset-request step, but reached on this confirmation step
    ''' instead, with the requested destination checked before use.
    ''' </summary>
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not String.IsNullOrEmpty(Request.QueryString("dest")) Then
            CompleteConfirmAndContinue(Request.QueryString("dest"))
        End If
    End Sub

    Private Sub CompleteConfirmAndContinue(ByVal destination As String)
        If RedirectTargetGuard.IsLocalRedirectTarget(destination) Then
            Response.Redirect(destination) ' SAFE_SINK: PLANTED-VB-HR-517-safe
        Else
            Response.Redirect("~/default.aspx")
        End If
    End Sub
End Class
