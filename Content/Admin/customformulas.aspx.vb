Imports System
Imports Microsoft.VisualBasic
Imports System.CodeDom.Compiler
Imports System.Reflection
Imports System.Collections.Generic

''' <summary>
''' "Custom Formulas" admin screen: lets an admin configure small calculation formulas for three
''' independently plausible areas this project already has organic concepts for -- grade weighting
''' (Content/Admin/addgrades.aspx), fee surcharges (Content/Account/mngfees.aspx,
''' Content/Parent/feeschedule.aspx), and per-row report-column computation (the export-template
''' pattern already used by Content/Admin/departments.aspx/lessonplans.aspx/newtemplate.aspx) --
''' plus a bulk/batch variant. None of these three areas has an existing admin-editable formula
''' concept of its own (see vbnet-cwe-94-planting-research.md §7), so this page and its five
''' PLANTED-VB-HR-671..675 formula actions are new, engineered-host scaffolding, each grounded in
''' one of this project's own real domain concepts.
''' </summary>
Partial Class Admin_customformulas
    Inherits System.Web.UI.Page

    Dim check As New CheckUser

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If check.Check_Admin(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
    End Sub

    ' ---- Direct, in-page straight-line compile+invoke (PLANTED-VB-HR-671) ----

    ''' <summary>
    ''' Computes a weighted final grade from an admin-supplied formula. The formula text is
    ''' expected to be a short expression like "midterm * 0.3 + finalExam * 0.5 + assignments * 0.2"
    ''' but, because it is compiled as a VB.NET function body rather than evaluated as a restricted
    ''' expression, nothing stops it from being an arbitrary multi-statement program instead.
    ''' </summary>
    Protected Sub btnRunGradeFormula_Click(sender As Object, e As EventArgs) Handles btnRunGradeFormula.Click
        Dim formulaCode As String = txtGradeFormulaCode.Text

        Dim fullSource As String =
            "Public Class GradeFormulaHost" & vbCrLf &
            "    Public Function ComputeWeightedGrade(ByVal midterm As Double, ByVal finalExam As Double, ByVal assignments As Double) As Double" & vbCrLf &
            "        " & formulaCode & vbCrLf &
            "    End Function" & vbCrLf &
            "End Class"

        Dim parameters As New CompilerParameters()
        parameters.GenerateInMemory = True
        parameters.GenerateExecutable = False
        parameters.ReferencedAssemblies.Add("System.dll")

        Dim provider As New VBCodeProvider()
        ' SINK: PLANTED-VB-HR-671
        Dim results As CompilerResults = provider.CompileAssemblyFromSource(parameters, fullSource)

        If results.Errors.HasErrors Then
            lblGradeFormulaResult.Text = "Formula could not be compiled"
            Return
        End If

        Dim hostType As Type = results.CompiledAssembly.GetType("GradeFormulaHost")
        Dim hostInstance As Object = Activator.CreateInstance(hostType)
        Dim method As MethodInfo = hostType.GetMethod("ComputeWeightedGrade")
        Dim computedValue As Object = method.Invoke(hostInstance, New Object() {70.0, 80.0, 90.0})
        lblGradeFormulaResult.Text = "Weighted grade: " & computedValue.ToString()
    End Sub

    ''' <summary>
    ''' Safe twin of <see cref="btnRunGradeFormula_Click"/> -- the weighting is always the fixed
    ''' arithmetic "midterm*0.3 + finalExam*0.5 + assignments*0.2", computed directly with no
    ''' compilation step of any kind. The admin can no longer supply arbitrary code, only the three
    ''' sample score values themselves (which this demo fixes the same way as the vulnerable path).
    ''' </summary>
    Protected Sub btnRunGradeFormulaSafe_Click(sender As Object, e As EventArgs) Handles btnRunGradeFormulaSafe.Click
        Dim midterm As Double = 70.0
        Dim finalExam As Double = 80.0
        Dim assignments As Double = 90.0
        ' SAFE_SINK: PLANTED-VB-HR-671-safe
        Dim computedValue As Double = (midterm * 0.3) + (finalExam * 0.5) + (assignments * 0.2)
        lblGradeFormulaResult.Text = "Weighted grade: " & computedValue.ToString()
    End Sub

    ' ---- Indirect, same-class private helper (PLANTED-VB-HR-672) ----

    Protected Sub btnRunFeeSurcharge_Click(sender As Object, e As EventArgs) Handles btnRunFeeSurcharge.Click
        Dim formulaCode As String = txtFeeSurchargeFormulaCode.Text
        Dim baseFee As Double = CDbl(txtFeeSurchargeBaseFee.Text)
        lblFeeSurchargeResult.Text = CompileAndRunSurchargeFormula(formulaCode, baseFee)
    End Sub

    Protected Sub btnRunFeeSurchargeSafe_Click(sender As Object, e As EventArgs) Handles btnRunFeeSurchargeSafe.Click
        Dim formulaCode As String = txtFeeSurchargeFormulaCode.Text
        Dim baseFee As Double = CDbl(txtFeeSurchargeBaseFee.Text)
        lblFeeSurchargeResult.Text = EvaluateSurchargeSafely(formulaCode, baseFee)
    End Sub

    ''' <summary>
    ''' Compiles the admin-supplied fee-surcharge formula text as a VB.NET function body and
    ''' invokes it against the submitted base fee.
    ''' </summary>
    Private Function CompileAndRunSurchargeFormula(ByVal formulaCode As String, ByVal baseFee As Double) As String
        Dim fullSource As String =
            "Public Class SurchargeFormulaHost" & vbCrLf &
            "    Public Function ComputeSurcharge(ByVal baseFee As Double) As Double" & vbCrLf &
            "        " & formulaCode & vbCrLf &
            "    End Function" & vbCrLf &
            "End Class"

        Dim parameters As New CompilerParameters()
        parameters.GenerateInMemory = True
        parameters.GenerateExecutable = False
        parameters.ReferencedAssemblies.Add("System.dll")

        Dim provider As New VBCodeProvider()
        ' SINK: PLANTED-VB-HR-672
        Dim results As CompilerResults = provider.CompileAssemblyFromSource(parameters, fullSource)

        If results.Errors.HasErrors Then
            Return "Formula could not be compiled"
        End If

        Dim hostType As Type = results.CompiledAssembly.GetType("SurchargeFormulaHost")
        Dim hostInstance As Object = Activator.CreateInstance(hostType)
        Dim method As MethodInfo = hostType.GetMethod("ComputeSurcharge")
        Dim computedValue As Object = method.Invoke(hostInstance, New Object() {baseFee})
        Return "Surcharge total: " & computedValue.ToString()
    End Function

    ''' <summary>
    ''' Safe twin of <see cref="CompileAndRunSurchargeFormula"/> -- the formula text is parsed by
    ''' the shared closed-grammar evaluator instead of being compiled and run.
    ''' </summary>
    Private Function EvaluateSurchargeSafely(ByVal formulaText As String, ByVal baseFee As Double) As String
        Dim fields As New Dictionary(Of String, Double)
        fields("baseFee") = baseFee
        Dim evaluator As New ClosedFormulaExpressionEvaluator(fields)
        Try
            ' SAFE_SINK: PLANTED-VB-HR-672-safe
            Dim computedValue As Double = evaluator.Evaluate(formulaText)
            Return "Surcharge total: " & computedValue.ToString()
        Catch ex As Exception
            Return "Formula could not be evaluated"
        End Try
    End Function

    ' ---- Interprocedural, via ReportColumnFormulaOrchestrator -> ReportColumnFormulaCompiler (PLANTED-VB-HR-673) ----

    Protected Sub btnRunReportColumn_Click(sender As Object, e As EventArgs) Handles btnRunReportColumn.Click
        Dim formulaCode As String = txtReportColumnFormulaCode.Text
        Dim sampleValue As Double = CDbl(txtReportColumnSampleValue.Text)
        lblReportColumnResult.Text = ReportColumnFormulaOrchestrator.SubmitCustomReportColumnFormula(formulaCode, sampleValue)
    End Sub

    Protected Sub btnRunReportColumnSafe_Click(sender As Object, e As EventArgs) Handles btnRunReportColumnSafe.Click
        Dim formulaCode As String = txtReportColumnFormulaCode.Text
        Dim sampleValue As Double = CDbl(txtReportColumnSampleValue.Text)
        lblReportColumnResult.Text = ReportColumnFormulaOrchestrator.SubmitCustomReportColumnFormulaSafely(formulaCode, sampleValue)
    End Sub

    ' ---- Interprocedural + polymorphic dispatch (PLANTED-VB-HR-674) ----

    ''' <summary>
    ''' Resolves a formula evaluator purely by the admin-supplied "Evaluator Mode" text and invokes
    ''' it against the submitted base amount -- no branch at this call site decides whether the
    ''' formula text is compiled and run or evaluated safely; that is entirely a property of which
    ''' concrete CustomFormulaEvaluatorBase CustomFormulaEvaluatorFactory.Resolve returns.
    ''' </summary>
    Protected Sub btnRunScholarshipAdjustment_Click(sender As Object, e As EventArgs) Handles btnRunScholarshipAdjustment.Click
        Dim formulaText As String = txtScholarshipFormulaText.Text
        Dim baseAmount As Double = CDbl(txtScholarshipBaseAmount.Text)
        Dim evaluator As CustomFormulaEvaluatorBase = CustomFormulaEvaluatorFactory.Resolve(txtEvaluatorMode.Text)
        Dim computedValue As Double = evaluator.Evaluate(formulaText, baseAmount)
        lblScholarshipResult.Text = "Adjustment: " & computedValue.ToString()
    End Sub

    ' ---- Diversity: interprocedural + batch loop (PLANTED-VB-HR-675) ----

    Protected Sub btnRunBulkSurcharge_Click(sender As Object, e As EventArgs) Handles btnRunBulkSurcharge.Click
        Dim categoryFormulas As Dictionary(Of String, String) = ParseCategoryFormulas(txtBulkCategoryFormulas.Text)
        Dim categoryBaseFees As Dictionary(Of String, Double) = ParseCategoryBaseFees(txtBulkCategoryBaseFees.Text)
        lblBulkSurchargeResult.Text = BulkSurchargeFormulaJob.RunBatch(categoryFormulas, categoryBaseFees)
    End Sub

    Protected Sub btnRunBulkSurchargeSafe_Click(sender As Object, e As EventArgs) Handles btnRunBulkSurchargeSafe.Click
        Dim categoryFormulas As Dictionary(Of String, String) = ParseCategoryFormulas(txtBulkCategoryFormulas.Text)
        Dim categoryBaseFees As Dictionary(Of String, Double) = ParseCategoryBaseFees(txtBulkCategoryBaseFees.Text)
        lblBulkSurchargeResult.Text = BulkSurchargeFormulaJob.RunBatchSafely(categoryFormulas, categoryBaseFees)
    End Sub

    Private Function ParseCategoryFormulas(ByVal rawList As String) As Dictionary(Of String, String)
        Dim result As New Dictionary(Of String, String)
        Dim entries As String() = rawList.Split(";"c)
        For Each entry As String In entries
            If entry.Trim() <> "" Then
                Dim parts As String() = entry.Split(New Char() {"="c}, 2)
                If parts.Length = 2 Then
                    result(parts(0).Trim()) = parts(1).Trim()
                End If
            End If
        Next
        Return result
    End Function

    Private Function ParseCategoryBaseFees(ByVal rawList As String) As Dictionary(Of String, Double)
        Dim result As New Dictionary(Of String, Double)
        Dim entries As String() = rawList.Split(";"c)
        For Each entry As String In entries
            If entry.Trim() <> "" Then
                Dim parts As String() = entry.Split(New Char() {"="c}, 2)
                If parts.Length = 2 Then
                    result(parts(0).Trim()) = CDbl(parts(1).Trim())
                End If
            End If
        Next
        Return result
    End Function

End Class
