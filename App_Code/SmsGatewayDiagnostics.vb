''' <summary>
''' Small collaborator used by the admin SMS reports page's "Run Named Diagnostic" quick action
''' (Content/Admin/smsreports.aspx.vb). Each public method here is a self-contained diagnostic
''' check with no return value and no side effects beyond its own log line -- it exists purely as
''' a realistic dispatch target for that page's own CallByName-based selector, not as a
''' load-bearing part of this project's actual SMS-sending pipeline. Per
''' vbnet-cwe-676-planting-research.md, this project's own organic SMS code (App_Code/notify.vb)
''' never talks to a real gateway at all -- there is no first-party gateway-connectivity code
''' here to extend, so this mirrors the project's own already-established "new,
''' invented-but-realistic admin feature" convention used elsewhere in this CWE-676 round.
''' </summary>
Public Class SmsGatewayDiagnostics

    ''' <summary>In a real deployment this would call the SMS provider's balance-check API.</summary>
    Public Sub CheckBalance()
    End Sub

    ''' <summary>In a real deployment this would call the SMS provider's queue-depth API.</summary>
    Public Sub CheckQueueDepth()
    End Sub

    ''' <summary>In a real deployment this would recycle the SMS provider's HTTP connection.</summary>
    Public Sub ResetConnection()
    End Sub

End Class
