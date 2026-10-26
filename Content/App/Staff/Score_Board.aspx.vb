Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Web.UI
Imports System.Security
Imports System.Threading

Public Class Score_Board
    Inherits System.Web.UI.Page

    Dim cmd As New SqlCommand
    Dim cmd2 As New SqlCommand
    Dim da As New SqlDataAdapter
    Dim ds As New DataSet
    Dim dr As SqlDataReader
    Dim dr2 As SqlDataReader





    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub grading()
        Dim sngYearMark, sngExamMark, sngFinalMark As Single
        sngYearMark = Val(txtClass.Text) * 0.3
        sngExamMark = Val(txtExam.Text) * 0.7
        sngFinalMark = sngYearMark + sngExamMark
        lbltot.Text = sngFinalMark
        Select Case sngFinalMark
            Case sngFinalMark = 0 To 44
                lblGrd.Text = "F"
                lblRem.Text = "Fail"
            Case sngFinalMark = 45 To 59
                lblGrd.Text = "E"
                lblRem.Text = "Pass"
            Case sngFinalMark = 60 To 69
                lblGrd.Text = "D"
                lblRem.Text = "Credit"
            Case sngFinalMark = 70 To 79
                lblGrd.Text = "C"
                lblRem.Text = "Good"
            Case sngFinalMark = 80 To 89
                lblGrd.Text = "B"
                lblRem.Text = "Very Good"
            Case sngFinalMark = 90 To 100
                lblGrd.Text = "A"
                lblRem.Text = "Excellent"
            Case Else
                System.Console.WriteLine("Not a valid Student Mark.")
        End Select
    End Sub

    Private Sub CBODisplay()
        Dim ct As String

        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            'Try
            cmd = New SqlCommand("select count (*) from tblEnroll where StudentID='" & DropDownList1.Text & "'", con)

            ct = cmd.ExecuteScalar
            If ct > 0 Then
                cmd = New SqlCommand("select * from tblEnroll where StudentID='" & DropDownList1.Text & "'", con)

                dr = cmd.ExecuteReader()
                dr.Read()
                lblname.Text = dr("Fullname")
                lblHouse.Text = dr("House")
                lblClass.Text = dr("Class")
                lblStudid.Text = dr("StudentID")
                dr.Close()
                Exit Sub


                dr = cmd.ExecuteReader
                dr.Read()

                If dr.HasRows = True Then
                    lblname.Text = dr("Fullname")
                    lblHouse.Text = dr("House")
                    lblClass.Text = dr("Class")
                    lblStudid.Text = dr("StudentID")
                Else
                    Response.Write("Student Number does not exist")
                    dr.Close()

                End If
                dr.Close()
                'dr.Close()

            End If
            cmd.Dispose()
            con.close()        End Using


    End Sub


    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button2.Click
        CBODisplay()

    End Sub

    Protected Sub txtClass_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtClass.TextChanged
        lbltot.Text = Val(txtClass.Text) + Val(txtExam.Text)

    End Sub

    Protected Sub txtExam_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtExam.TextChanged

        If lblname.Text = "" Then
            Response.Write("Please Search Student Name and Proceed")
        Else
            lbltot.Text = Val(txtClass.Text) + Val(txtExam.Text)
            grading()

        End If
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        lblID.Text = DropDownList1.Text
        lblSubject.Text = DropDownList2.Text
        lblScore.Text = txtClass.Text
        lblExm.Text = txtExam.Text
        lblTotal.Text = lbltot.Text
        lblFGrd.Text = lblGrd.Text
        lblFRem.Text = lblRem.Text


        'DropDownList1.SelectedValue = 0
        'DropDownList2.SelectedValue = 0
        txtClass.Text = ""
        txtExam.Text = ""
        lbltot.Text = ""
        lblGrd.Text = ""
        lblRem.Text = ""

    End Sub

    Protected Sub Button3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button3.Click
        Thread.Sleep(3000)
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()

                Dim cmd As New SqlCommand("insert into tblSubject(StudID,Subject,StaffID,StaffName,ClassScore,Exams,Total,Grade,Remarks,Fullname,Class,Term,Yr,Updt)values('" & lblStudid.Text & "','" & DropDownList2.Text & "','" & Session("StaffID") & "','" & Session("Fullname") & "','" & lblScore.Text & "','" & lblExm.Text & "','" & lblTotal.Text & "','" & lblFGrd.Text & "','" & lblFRem.Text & "','" & lblname.Text & "','" & lblClass.Text & "','" & DropDownList3.Text & "','" & DropDownList4.Text & "','" & Today.Date & "')", con)
                cmd.ExecuteNonQuery()
                cmd.Dispose()
                con.close()            End Using

            lblID.Text = String.Empty
            lblSubject.Text = String.Empty
            lblScore.Text = String.Empty
            lblExm.Text = String.Empty
            lblTotal.Text = String.Empty
            lblFGrd.Text = String.Empty
            lblFRem.Text = String.Empty
            GridView1.DataBind()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Instance 1 (Direct): lets a staff member preview how long the save-throttle delay they are
    ''' about to configure will feel, by running it immediately on this request. The submitted
    ''' delay is parsed and handed straight to Thread.Sleep in this same handler -- no helper, no
    ''' branch.
    ''' </summary>
    Protected Sub btnPreviewThrottle_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPreviewThrottle.Click
        Dim previewMs As Integer = CInt(txtThrottlePreviewMs.Text)
        Thread.Sleep(previewMs) ' SINK: PLANTED-VB-HR-741
        lblThrottlePreviewResult.Text = "Previewed a " & previewMs.ToString() & "ms processing delay."
    End Sub

    ''' <summary>
    ''' Safe twin of btnPreviewThrottle_Click: the requested preview delay is floored at zero and
    ''' capped at a small, fixed ceiling before it ever reaches Thread.Sleep, so the attacker's
    ''' only remaining influence is "how close to the ceiling" -- must NOT allow an unbounded
    ''' delay (including the Timeout.Infinite -1 sentinel).
    ''' </summary>
    Protected Sub btnPreviewThrottleSafe_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPreviewThrottleSafe.Click
        Dim previewMs As Integer = CInt(txtThrottlePreviewMsSafe.Text)
        Dim clampedMs As Integer = Math.Min(Math.Max(previewMs, 0), 2000)
        Thread.Sleep(clampedMs) ' SAFE_SINK: PLANTED-VB-HR-741-safe
        lblThrottlePreviewSafeResult.Text = "Previewed a " & clampedMs.ToString() & "ms processing delay (clamped)."
    End Sub

    ''' <summary>
    ''' Instance 2 (Indirect, same-class helper): a staff member configures the pause that should
    ''' be applied before each score save commits (e.g. to spread out DB load during peak
    ''' submission times), persisted to Session for the save flow above to honor. The actual pause
    ''' happens one hop away, in the private ApplySaveThrottle helper.
    ''' </summary>
    Protected Sub btnApplyThrottle_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnApplyThrottle.Click
        Dim requestedMs As Integer = CInt(txtApplyThrottleMs.Text)
        ApplySaveThrottle(requestedMs)
        Session("ScoreSaveThrottleMs") = requestedMs
        lblApplyThrottleResult.Text = "Save throttle applied."
    End Sub

    Private Sub ApplySaveThrottle(ByVal delayMs As Integer)
        Thread.Sleep(delayMs) ' SINK: PLANTED-VB-HR-742
    End Sub

    ''' <summary>
    ''' Safe twin of btnApplyThrottle_Click / ApplySaveThrottle: the requested delay is floored and
    ''' capped before the helper ever calls Thread.Sleep.
    ''' </summary>
    Protected Sub btnApplyThrottleSafe_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnApplyThrottleSafe.Click
        Dim requestedMs As Integer = CInt(txtApplyThrottleMsSafe.Text)
        ApplySaveThrottleSafe(requestedMs)
        Session("ScoreSaveThrottleMsSafe") = requestedMs
        lblApplyThrottleSafeResult.Text = "Save throttle applied (bounded)."
    End Sub

    Private Sub ApplySaveThrottleSafe(ByVal delayMs As Integer)
        Dim clampedMs As Integer = Math.Min(Math.Max(delayMs, 0), 2000)
        Thread.Sleep(clampedMs) ' SAFE_SINK: PLANTED-VB-HR-742-safe
    End Sub

    ''' <summary>
    ''' Instance 3 (Interprocedural, genuine 2-hop cross-file chain): queues a throttled report run
    ''' via App_Code/ReportThrottleService.vb's ScheduleThrottledReportRun (hop 1), which calls
    ''' App_Code/ThrottleDelayExecutor.vb's ExecuteThrottleDelay (hop 2, the actual Thread.Sleep
    ''' call) -- no bound applied at either hop.
    ''' </summary>
    Protected Sub btnQueueReportRun_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnQueueReportRun.Click
        Dim delayMs As Integer = CInt(txtQueueReportDelayMs.Text)
        ReportThrottleService.ScheduleThrottledReportRun(delayMs)
        lblQueueReportResult.Text = "Report run queued."
    End Sub

    ''' <summary>
    ''' Safe twin of btnQueueReportRun_Click: same 2-hop chain, routed to the Safe variants of
    ''' both ScheduleThrottledReportRun and ExecuteThrottleDelay, which floor and cap the delay
    ''' before Thread.Sleep.
    ''' </summary>
    Protected Sub btnQueueReportRunSafe_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnQueueReportRunSafe.Click
        Dim delayMs As Integer = CInt(txtQueueReportDelayMsSafe.Text)
        ReportThrottleService.ScheduleThrottledReportRunSafe(delayMs)
        lblQueueReportSafeResult.Text = "Report run queued (bounded)."
    End Sub

    ''' <summary>
    ''' Instance 4 (genuine type/polymorphism-dependent): resolves a ReportThrottlePolicyBase
    ''' (App_Code/ReportThrottlePolicy.vb) by free-text throttle mode and calls its single
    ''' ApplyDelay method. This call site performs no branching of its own at all -- whether the
    ''' delay ends up bounded depends entirely on which concrete policy the factory resolved.
    ''' </summary>
    Protected Sub btnPreviewPolicy_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPreviewPolicy.Click
        Dim requestedMs As Integer = CInt(txtPolicyDelayMs.Text)
        Dim policy As ReportThrottlePolicyBase = ReportThrottlePolicyFactory.ResolveFor(txtThrottleMode.Text)
        policy.ApplyDelay(requestedMs)
        lblPolicyPreviewResult.Text = "Policy preview complete."
    End Sub

    ''' <summary>
    ''' Instance 5 (Diversity, loop/batch accumulation): a comma-separated list of per-step delays
    ''' is split and each parsed value is slept in a loop, with neither the per-item value nor the
    ''' number of items bounded -- the total accumulated sleep for one request is unbounded even
    ''' beyond what any single value could cause. The accumulated total is genuinely consumed
    ''' downstream (persisted to Session for the report-history view).
    ''' </summary>
    Protected Sub btnRunBatchReport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRunBatchReport.Click
        Dim steps As Array = Split(txtBatchDelays.Text, ",")
        Dim totalMs As Integer = 0
        For Each delayItem As String In steps
            Dim itemMs As Integer = CInt(delayItem.Trim())
            Thread.Sleep(itemMs) ' SINK: PLANTED-VB-HR-745
            totalMs += itemMs
        Next
        Session("LastBatchReportDurationMs") = totalMs
        lblBatchReportResult.Text = "Batch report simulated in " & totalMs.ToString() & "ms across " & steps.Length.ToString() & " step(s)."
    End Sub

    ''' <summary>
    ''' Safe twin of btnRunBatchReport_Click: bounds BOTH the per-item value (clamped to a small
    ''' ceiling) AND the number of items actually processed (capped at 5) -- clamping only the
    ''' per-item duration would be insufficient on its own, since an attacker who cannot make any
    ''' single sleep long could still make the TOTAL sleep for one request unbounded by supplying
    ''' arbitrarily many items.
    ''' </summary>
    Protected Sub btnRunBatchReportSafe_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRunBatchReportSafe.Click
        Dim steps As Array = Split(txtBatchDelaysSafe.Text, ",")
        Dim totalMs As Integer = 0
        Dim processedCount As Integer = 0
        For Each delayItem As String In steps
            If processedCount >= 5 Then
                Exit For
            End If
            Dim itemMs As Integer = CInt(delayItem.Trim())
            Dim clampedMs As Integer = Math.Min(Math.Max(itemMs, 0), 500)
            Thread.Sleep(clampedMs) ' SAFE_SINK: PLANTED-VB-HR-745-safe
            totalMs += clampedMs
            processedCount += 1
        Next
        Session("LastBatchReportDurationMsSafe") = totalMs
        lblBatchReportSafeResult.Text = "Batch report simulated in " & totalMs.ToString() & "ms across " & processedCount.ToString() & " step(s) (bounded)."
    End Sub
End Class
