Imports System
Imports Microsoft.VisualBasic
Imports System.CodeDom.Compiler
Imports System.Reflection
Imports System.Collections.Generic

''' <summary>
''' Resolves which concrete formula evaluator handles the "Custom Scholarship/Concession
''' Adjustment Formula" action on Content/Admin/customformulas.aspx, by caller-supplied free-text
''' evaluator mode. Genuine polymorphism (MustInherit/MustOverride), mirroring this project's own
''' App_Code/LoginCompletionStrategy.vb precedent ("Genuine polymorphism (MustInherit/MustOverride)
''' ... used here instead of a Select-Case dispatch written inline in the page") and
''' App_Code/PartnerCertificateTrustPolicy.vb, used here instead of branching directly at the
''' call site on the evaluator mode. The call site in customformulas.aspx.vb resolves an
''' evaluator and invokes Evaluate with no branch of its own -- whether that call compiles and
''' runs the admin's formula text or evaluates it safely depends entirely on which concrete type
''' CustomFormulaEvaluatorFactory.Resolve returns.
''' </summary>
Public MustInherit Class CustomFormulaEvaluatorBase
    Public MustOverride Function Evaluate(ByVal formulaText As String, ByVal baseAmount As Double) As Double
End Class

''' <summary>
''' Legacy/default formula evaluator -- compiles the admin-supplied formula text as full VB.NET
''' source and executes it. Selected by CustomFormulaEvaluatorFactory.Resolve for any evaluator
''' mode not explicitly recognized as "closed".
''' </summary>
Public Class ScriptedFormulaEvaluator
    Inherits CustomFormulaEvaluatorBase

    Public Overrides Function Evaluate(ByVal formulaText As String, ByVal baseAmount As Double) As Double
        Dim fullSource As String =
            "Public Class ScholarshipFormulaHost" & vbCrLf &
            "    Public Function ComputeAdjustment(ByVal baseAmount As Double) As Double" & vbCrLf &
            "        " & formulaText & vbCrLf &
            "    End Function" & vbCrLf &
            "End Class"

        Dim parameters As New CompilerParameters()
        parameters.GenerateInMemory = True
        parameters.GenerateExecutable = False
        parameters.ReferencedAssemblies.Add("System.dll")

        Dim provider As New VBCodeProvider()
        ' SINK: PLANTED-VB-HR-674
        Dim results As CompilerResults = provider.CompileAssemblyFromSource(parameters, fullSource)

        If results.Errors.HasErrors Then
            Throw New InvalidOperationException("Formula could not be compiled")
        End If

        Dim hostType As Type = results.CompiledAssembly.GetType("ScholarshipFormulaHost")
        Dim hostInstance As Object = Activator.CreateInstance(hostType)
        Dim method As MethodInfo = hostType.GetMethod("ComputeAdjustment")
        Dim computedValue As Object = method.Invoke(hostInstance, New Object() {baseAmount})
        Return CDbl(computedValue)
    End Function
End Class

''' <summary>
''' Hardened formula evaluator -- never compiles or executes the admin-supplied formula text as
''' code. Parses it through the shared closed-grammar evaluator instead.
''' </summary>
Public Class ClosedExpressionFormulaEvaluator
    Inherits CustomFormulaEvaluatorBase

    Public Overrides Function Evaluate(ByVal formulaText As String, ByVal baseAmount As Double) As Double
        Dim fields As New Dictionary(Of String, Double)
        fields("baseAmount") = baseAmount
        Dim evaluator As New ClosedFormulaExpressionEvaluator(fields)
        ' SAFE_SINK: PLANTED-VB-HR-674-safe
        Return evaluator.Evaluate(formulaText)
    End Function
End Class

''' <summary>
''' Resolves which concrete evaluator to use for a caller-supplied evaluator-mode string.
''' NOTE: callers choose the evaluator purely by this string -- there is no server-side
''' verification that the requested mode is one the current request would ever legitimately use
''' (same disclosed, honest limitation as this project's other caller-selected-dispatch plants,
''' e.g. App_Code/LoginCompletionStrategy.vb's LoginCompletionFactory and
''' App_Code/PartnerCertificateTrustPolicy.vb's PartnerCertificateTrustPolicyFactory).
''' </summary>
Public Class CustomFormulaEvaluatorFactory
    Public Shared Function Resolve(ByVal evaluatorMode As String) As CustomFormulaEvaluatorBase
        ' Case-sensitive (VB default Option Compare Binary, not set otherwise anywhere in this
        ' project) -- e.g. "Closed" falls through to Case Else, not the "closed" branch below.
        ' Same default comparison this project's own LoginCompletionStrategy.vb's
        ' LoginCompletionFactory and PartnerCertificateTrustPolicy.vb's
        ' PartnerCertificateTrustPolicyFactory rely on for their own Select Case.
        Select Case evaluatorMode
            Case "closed"
                Return New ClosedExpressionFormulaEvaluator()
            Case Else
                Return New ScriptedFormulaEvaluator()
        End Select
    End Function
End Class
