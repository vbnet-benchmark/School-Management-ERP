Imports System.Threading

''' <summary>
''' Hop 2 of the cross-file chain backing Content/App/Staff/Score_Board.aspx's "Queue Report Run"
''' feature (PLANTED-VB-HR-743). Called only from App_Code/ReportThrottleService.vb's
''' ScheduleThrottledReportRun -- this is where the actual Thread.Sleep call lives.
''' </summary>
Public Class ThrottleDelayExecutor

    ''' <summary>
    ''' Instance 3 (Interprocedural, hop 2): the delay forwarded by ReportThrottleService is
    ''' handed straight to Thread.Sleep, with no upper bound.
    ''' </summary>
    Public Shared Sub ExecuteThrottleDelay(ByVal delayMs As Integer)
        Thread.Sleep(delayMs) ' SINK: PLANTED-VB-HR-743
    End Sub

    ''' <summary>
    ''' Safe twin of ExecuteThrottleDelay: floors the delay at zero and caps it at a small, fixed
    ''' ceiling before it reaches Thread.Sleep.
    ''' </summary>
    Public Shared Sub ExecuteThrottleDelaySafe(ByVal delayMs As Integer)
        Dim clampedMs As Integer = Math.Min(Math.Max(delayMs, 0), 2000)
        Thread.Sleep(clampedMs) ' SAFE_SINK: PLANTED-VB-HR-743-safe
    End Sub

End Class
