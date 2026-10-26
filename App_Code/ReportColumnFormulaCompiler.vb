Imports System
Imports Microsoft.VisualBasic
Imports System.CodeDom.Compiler
Imports System.Reflection
Imports System.Collections.Generic

''' <summary>
''' Second hop of the "Custom Report-Column Formula" feature on
''' Content/Admin/customformulas.aspx: extends this project's existing export-template pattern
''' (Content/Admin/departments.aspx.vb's "Department Profile Export", lessonplans.aspx.vb's
''' "Lesson Plan Template Export", newtemplate.aspx.vb's "Assigned-Classes Export" -- all three
''' already export admin-chosen, pre-stored fields) with a derived, per-row *computed* value: an
''' admin can define a small formula (e.g. a weighted composite score shown as an extra export
''' column) that is evaluated against one sample numeric field per row. Called only from
''' App_Code/ReportColumnFormulaOrchestrator.vb -- this project's PLANTED-VB-HR-534/368-style
''' genuine multi-hop interprocedural shape, not a one-hop helper dressed up as two.
''' </summary>
Public Class ReportColumnFormulaCompiler

    ''' <summary>
    ''' Compiles the admin-supplied report-column formula text as full VB.NET source and invokes
    ''' it against the sample row value. The formula text is wrapped in a minimal class/function
    ''' shell so the admin only has to type the body of a single-expression function, but nothing
    ''' about the wrapping restricts what that body may contain -- a function body is also valid VB
    ''' statement-sequence syntax, so the admin field accepts an arbitrary multi-statement program,
    ''' not just the composite-score arithmetic expression the feature's UI label implies.
    ''' </summary>
    Public Shared Function CompileAndEvaluate(ByVal formulaText As String, ByVal sampleRowValue As Double) As String
        Dim fullSource As String =
            "Public Class ReportColumnFormulaHost" & vbCrLf &
            "    Public Function ComputeColumn(ByVal rowValue As Double) As Double" & vbCrLf &
            "        " & formulaText & vbCrLf &
            "    End Function" & vbCrLf &
            "End Class"

        Dim parameters As New CompilerParameters()
        parameters.GenerateInMemory = True
        parameters.GenerateExecutable = False
        parameters.ReferencedAssemblies.Add("System.dll")

        Dim provider As New VBCodeProvider()
        ' SINK: PLANTED-VB-HR-673
        Dim results As CompilerResults = provider.CompileAssemblyFromSource(parameters, fullSource)

        If results.Errors.HasErrors Then
            Return "Formula could not be compiled"
        End If

        Dim hostType As Type = results.CompiledAssembly.GetType("ReportColumnFormulaHost")
        Dim hostInstance As Object = Activator.CreateInstance(hostType)
        Dim method As MethodInfo = hostType.GetMethod("ComputeColumn")
        Dim computedValue As Object = method.Invoke(hostInstance, New Object() {sampleRowValue})
        Return computedValue.ToString()
    End Function

    ''' <summary>
    ''' Safe twin of <see cref="CompileAndEvaluate"/> -- the admin-supplied formula text is never
    ''' compiled or executed as code. It is parsed by a small, fixed-grammar expression evaluator
    ''' that recognizes only numeric literals, the single named field "rowValue", the four
    ''' arithmetic operators, and parentheses -- no other token can ever be evaluated, so there is
    ''' no code path from here to compiling or invoking anything.
    ''' </summary>
    Public Shared Function EvaluateSafely(ByVal formulaText As String, ByVal sampleRowValue As Double) As String
        Dim fields As New Dictionary(Of String, Double)
        fields("rowValue") = sampleRowValue
        Dim evaluator As New ClosedFormulaExpressionEvaluator(fields)
        Try
            ' SAFE_SINK: PLANTED-VB-HR-673-safe
            Dim computedValue As Double = evaluator.Evaluate(formulaText)
            Return computedValue.ToString()
        Catch ex As Exception
            Return "Formula could not be evaluated"
        End Try
    End Function

End Class
