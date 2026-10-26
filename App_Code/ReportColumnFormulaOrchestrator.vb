Imports System

''' <summary>
''' First hop of the "Custom Report-Column Formula" feature on
''' Content/Admin/customformulas.aspx -- receives the admin's submitted formula text and sample
''' row value from the page's code-behind, records which export template is being extended
''' (reserved for a future audit-log tie-in, not implemented in this round), and hands off to
''' App_Code/ReportColumnFormulaCompiler.vb (second hop) to actually compile/evaluate the formula.
''' Deliberately kept thin: this is this round's interprocedural tier, so the call chain is
''' page -> orchestrator -> compiler (two genuine hops), not a single compile call dressed up with
''' an extra wrapper method.
''' </summary>
Public Class ReportColumnFormulaOrchestrator

    Public Shared Function SubmitCustomReportColumnFormula(ByVal formulaText As String, ByVal sampleRowValue As Double) As String
        Return ReportColumnFormulaCompiler.CompileAndEvaluate(formulaText, sampleRowValue)
    End Function

    ''' <summary>
    ''' Safe twin of <see cref="SubmitCustomReportColumnFormula"/> -- hands off to the compiler's
    ''' closed-evaluator path instead of its compile-and-run path.
    ''' </summary>
    Public Shared Function SubmitCustomReportColumnFormulaSafely(ByVal formulaText As String, ByVal sampleRowValue As Double) As String
        Return ReportColumnFormulaCompiler.EvaluateSafely(formulaText, sampleRowValue)
    End Function

End Class
