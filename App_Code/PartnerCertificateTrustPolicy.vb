Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates

''' <summary>
''' Resolves which certificate-trust policy governs an outbound call to a partner exam-board /
''' government-portal API, by partner code. Genuine polymorphism (MustInherit/MustOverride),
''' mirroring this project's own App_Code/LoginCompletionStrategy.vb precedent ("Genuine
''' polymorphism (MustInherit/MustOverride) ... used here instead of a Select-Case dispatch
''' written inline in the page"), used here instead of branching directly at the network-call
''' site on the partner code. Backs the "Partner Certificate Trust" admin feature on
''' Content/Admin/externalintegrations.aspx.vb -- the call site there resolves a policy and wires
''' its ValidateServerCertificate straight into ServicePointManager.ServerCertificateValidationCallback,
''' with no branch of its own.
''' </summary>
Public MustInherit Class PartnerCertificateTrustPolicyBase
    Public MustOverride Function ValidateServerCertificate(ByVal certSender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
End Class

''' <summary>
''' Legacy partner trust policy -- several older partner integrations present a self-signed or
''' mis-chained certificate, so this policy accepts the connection regardless of sslPolicyErrors.
''' Selected by PartnerCertificateTrustPolicyFactory.Resolve for any partner code not explicitly
''' recognized as "standard".
''' </summary>
Public Class LegacyPartnerCertificateTrustPolicy
    Inherits PartnerCertificateTrustPolicyBase

    Public Overrides Function ValidateServerCertificate(ByVal certSender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
        ' SINK: PLANTED-VB-HR-644
        Return True
    End Function
End Class

''' <summary>
''' Standard partner trust policy -- accepts the connection only when the certificate chain
''' validated cleanly.
''' </summary>
Public Class StandardPartnerCertificateTrustPolicy
    Inherits PartnerCertificateTrustPolicyBase

    Public Overrides Function ValidateServerCertificate(ByVal certSender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
        ' SAFE_SINK: PLANTED-VB-HR-644-safe
        Return sslPolicyErrors = SslPolicyErrors.None
    End Function
End Class

''' <summary>
''' Resolves which concrete trust policy to use for a caller-supplied partner code.
''' NOTE: callers choose the trust policy purely by this string -- there is no server-side
''' verification that the requested partner code is one the current request would ever
''' legitimately use (same disclosed, honest limitation as this project's other
''' caller-selected-dispatch plants, e.g. App_Code/LoginCompletionStrategy.vb's
''' LoginCompletionFactory).
''' </summary>
Public Class PartnerCertificateTrustPolicyFactory
    Public Shared Function Resolve(ByVal partnerCode As String) As PartnerCertificateTrustPolicyBase
        ' Case-sensitive (VB default Option Compare Binary, not set otherwise anywhere in this
        ' project) -- e.g. "Standard" falls through to Case Else, not the "standard" branch below.
        ' Same default comparison this project's own LoginCompletionStrategy.vb's
        ' LoginCompletionFactory relies on for its own Select Case.
        Select Case partnerCode
            Case "standard"
                Return New StandardPartnerCertificateTrustPolicy()
            Case Else
                Return New LegacyPartnerCertificateTrustPolicy()
        End Select
    End Function
End Class
