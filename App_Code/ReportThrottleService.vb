Imports System.Threading

''' <summary>
''' Cross-file service backing Content/App/Staff/Score_Board.aspx's "Queue Report Run" feature
''' (PLANTED-VB-HR-743). A staff member can schedule a throttled report-generation run from the
''' score board; this is hop 1 of a genuine two-hop chain -- the actual pause happens one file
''' further away, in App_Code/ThrottleDelayExecutor.vb's ExecuteThrottleDelay.
''' </summary>
Public Class ReportThrottleService

    ''' <summary>
    ''' Instance 3 (Interprocedural, hop 1): forwards the requested delay unchanged to
    ''' ThrottleDelayExecutor.ExecuteThrottleDelay, with no bound applied here or there.
    ''' </summary>
    Public Shared Sub ScheduleThrottledReportRun(ByVal delayMs As Integer)
        ThrottleDelayExecutor.ExecuteThrottleDelay(delayMs)
    End Sub

    ''' <summary>
    ''' Safe twin of ScheduleThrottledReportRun: forwards to the Safe variant of
    ''' ExecuteThrottleDelay, which floors and caps the delay before Thread.Sleep.
    ''' </summary>
    Public Shared Sub ScheduleThrottledReportRunSafe(ByVal delayMs As Integer)
        ThrottleDelayExecutor.ExecuteThrottleDelaySafe(delayMs)
    End Sub

End Class
