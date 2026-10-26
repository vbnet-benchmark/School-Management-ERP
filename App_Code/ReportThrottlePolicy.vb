Imports System.Threading

''' <summary>
''' Resolves which processing-throttle policy governs the "Preview Throttle Policy" feature on
''' Content/App/Staff/Score_Board.aspx.vb (PLANTED-VB-HR-744). Genuine polymorphism
''' (MustInherit/MustOverride), mirroring this project's own App_Code/LoginCompletionStrategy.vb
''' precedent, used here instead of branching directly at the delay-preview call site on the
''' requested mode. The page's own call site resolves a policy and calls its single ApplyDelay
''' method, with no branch of its own -- whether the delay ends up bounded depends entirely on
''' which concrete policy the factory resolved.
''' </summary>
Public MustInherit Class ReportThrottlePolicyBase
    Public MustOverride Sub ApplyDelay(ByVal requestedMs As Integer)
End Class

''' <summary>
''' The default throttle policy -- applies whatever delay was requested with no bound at all.
''' Selected by ReportThrottlePolicyFactory.ResolveFor for any mode not explicitly recognized as
''' "bounded" (including an empty string, Nothing, or an unrecognized value).
''' </summary>
Public Class UnboundedReportThrottle
    Inherits ReportThrottlePolicyBase

    Public Overrides Sub ApplyDelay(ByVal requestedMs As Integer)
        Thread.Sleep(requestedMs) ' SINK: PLANTED-VB-HR-744
    End Sub
End Class

''' <summary>
''' The bounded throttle policy -- floors the requested delay at zero and caps it at a small,
''' fixed ceiling before applying it. Selected by ReportThrottlePolicyFactory.ResolveFor when the
''' requested mode is exactly "bounded" (case-insensitive).
''' </summary>
Public Class BoundedReportThrottle
    Inherits ReportThrottlePolicyBase

    Public Overrides Sub ApplyDelay(ByVal requestedMs As Integer)
        Dim clampedMs As Integer = Math.Min(Math.Max(requestedMs, 0), 2000)
        Thread.Sleep(clampedMs) ' SAFE_SINK: PLANTED-VB-HR-744-safe
    End Sub
End Class

''' <summary>
''' Resolves a ReportThrottlePolicyBase from a free-text throttle-mode value. The dispatch key is
''' read directly from a TextBox.Text value on the page, never a postback-bound list control, so
''' this call site's reachability does not depend on ListControl.LoadPostData.
''' </summary>
Public Class ReportThrottlePolicyFactory
    Public Shared Function ResolveFor(ByVal throttleMode As String) As ReportThrottlePolicyBase
        If String.Equals(throttleMode, "bounded", StringComparison.OrdinalIgnoreCase) Then
            Return New BoundedReportThrottle()
        Else
            Return New UnboundedReportThrottle()
        End If
    End Function
End Class
