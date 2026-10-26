Imports System
Imports Microsoft.VisualBasic
Imports System.CodeDom.Compiler
Imports System.Reflection
Imports System.Collections.Generic
Imports System.Text

''' <summary>
''' "Bulk Fee-Category Surcharge Rollout" action on Content/Admin/customformulas.aspx: an admin
''' submits one surcharge formula per fee category in a single batch (e.g. a different percentage
''' adjustment for "Tuition", "Transport", "Boarding"), and every submitted formula is compiled and
''' run once, against that category's own base fee, in a loop -- a distinct construction from the
''' single-call direct/indirect/interprocedural tiers above, mirroring this project's own
''' loop-batch precedent (App_Code/AdminQuickActionDispatcher.vb's PLANTED-VB-HR-369,
''' App_Code/StudentWorkspaceService.vb's PLANTED-VB-HR-245).
''' </summary>
Public Class BulkSurchargeFormulaJob

    ''' <summary>
    ''' Compiles and runs every (formulaText, baseFee) pair in the batch, one at a time, and
    ''' returns the combined per-category results.
    ''' </summary>
    Public Shared Function RunBatch(ByVal categoryFormulas As Dictionary(Of String, String), ByVal categoryBaseFees As Dictionary(Of String, Double)) As String
        Dim combined As New StringBuilder()
        For Each categoryName As String In categoryFormulas.Keys
            Dim formulaText As String = categoryFormulas(categoryName)
            Dim baseFee As Double = categoryBaseFees(categoryName)

            Dim fullSource As String =
                "Public Class SurchargeFormulaHost" & vbCrLf &
                "    Public Function ComputeSurcharge(ByVal baseFee As Double) As Double" & vbCrLf &
                "        " & formulaText & vbCrLf &
                "    End Function" & vbCrLf &
                "End Class"

            Dim parameters As New CompilerParameters()
            parameters.GenerateInMemory = True
            parameters.GenerateExecutable = False
            parameters.ReferencedAssemblies.Add("System.dll")

            Dim provider As New VBCodeProvider()
            ' SINK: PLANTED-VB-HR-675
            Dim results As CompilerResults = provider.CompileAssemblyFromSource(parameters, fullSource)

            If results.Errors.HasErrors Then
                combined.Append(categoryName).Append(": formula error; ")
            Else
                Dim hostType As Type = results.CompiledAssembly.GetType("SurchargeFormulaHost")
                Dim hostInstance As Object = Activator.CreateInstance(hostType)
                Dim method As MethodInfo = hostType.GetMethod("ComputeSurcharge")
                Dim computedValue As Object = method.Invoke(hostInstance, New Object() {baseFee})
                combined.Append(categoryName).Append(": ").Append(computedValue.ToString()).Append("; ")
            End If
        Next
        Return combined.ToString()
    End Function

    ''' <summary>
    ''' Safe twin of <see cref="RunBatch"/> -- every submitted formula in the batch is evaluated
    ''' through the shared closed-grammar evaluator instead of being compiled and run; a formula
    ''' that uses any token outside the closed grammar fails for that one category only, without
    ''' ever reaching a compile/execute step.
    ''' </summary>
    Public Shared Function RunBatchSafely(ByVal categoryFormulas As Dictionary(Of String, String), ByVal categoryBaseFees As Dictionary(Of String, Double)) As String
        Dim combined As New StringBuilder()
        For Each categoryName As String In categoryFormulas.Keys
            Dim formulaText As String = categoryFormulas(categoryName)
            Dim baseFee As Double = categoryBaseFees(categoryName)

            Dim fields As New Dictionary(Of String, Double)
            fields("baseFee") = baseFee
            Dim evaluator As New ClosedFormulaExpressionEvaluator(fields)
            Try
                ' SAFE_SINK: PLANTED-VB-HR-675-safe
                Dim computedValue As Double = evaluator.Evaluate(formulaText)
                combined.Append(categoryName).Append(": ").Append(computedValue.ToString()).Append("; ")
            Catch ex As Exception
                combined.Append(categoryName).Append(": formula error; ")
            End Try
        Next
        Return combined.ToString()
    End Function

End Class
