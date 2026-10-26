Imports System
Imports System.Collections.Generic
Imports System.Globalization

''' <summary>
''' The shared safe counterpart to every "Custom Formula" sink planted on
''' Content/Admin/customformulas.aspx (and its App_Code collaborators,
''' ReportColumnFormulaCompiler/ReportColumnFormulaOrchestrator, CustomFormulaEvaluator,
''' BulkSurchargeFormulaJob). Evaluates a formula string using a small, fixed, closed grammar --
''' numeric literals, a caller-supplied set of named numeric fields, the four arithmetic operators,
''' unary minus, and parentheses -- and nothing else. There is no code path from this class to
''' Microsoft.VisualBasic.VBCodeProvider, System.CodeDom.Compiler, Activator.CreateInstance, or
''' reflection of any kind: an admin-supplied formula that is not built entirely from the tokens
''' this grammar recognizes simply fails to parse (FormatException), it is never compiled or
''' executed as code.
''' </summary>
Public Class ClosedFormulaExpressionEvaluator

    Private ReadOnly namedFields As Dictionary(Of String, Double)
    Private formulaText As String
    Private position As Integer

    Public Sub New(ByVal namedFields As Dictionary(Of String, Double))
        Me.namedFields = namedFields
    End Sub

    ''' <summary>
    ''' Parses and evaluates <paramref name="formulaText"/> against this evaluator's named fields.
    ''' Throws <see cref="FormatException"/> for any token the closed grammar does not recognize.
    ''' </summary>
    Public Function Evaluate(ByVal formulaText As String) As Double
        Me.formulaText = formulaText
        Me.position = 0
        Dim result As Double = ParseExpression()
        SkipWhitespace()
        If Me.position < Me.formulaText.Length Then
            Throw New FormatException("Unexpected trailing text in formula")
        End If
        Return result
    End Function

    Private Function ParseExpression() As Double
        Dim value As Double = ParseTerm()
        SkipWhitespace()
        Do While Me.position < Me.formulaText.Length AndAlso (Peek() = "+"c OrElse Peek() = "-"c)
            Dim op As Char = Peek()
            Me.position += 1
            Dim rhs As Double = ParseTerm()
            If op = "+"c Then
                value += rhs
            Else
                value -= rhs
            End If
            SkipWhitespace()
        Loop
        Return value
    End Function

    Private Function ParseTerm() As Double
        Dim value As Double = ParseFactor()
        SkipWhitespace()
        Do While Me.position < Me.formulaText.Length AndAlso (Peek() = "*"c OrElse Peek() = "/"c)
            Dim op As Char = Peek()
            Me.position += 1
            Dim rhs As Double = ParseFactor()
            If op = "*"c Then
                value *= rhs
            Else
                value /= rhs
            End If
            SkipWhitespace()
        Loop
        Return value
    End Function

    Private Function ParseFactor() As Double
        SkipWhitespace()
        If Me.position >= Me.formulaText.Length Then
            Throw New FormatException("Unexpected end of formula")
        End If

        If Peek() = "-"c Then
            Me.position += 1
            Return -ParseFactor()
        End If

        If Peek() = "("c Then
            Me.position += 1
            Dim value As Double = ParseExpression()
            SkipWhitespace()
            If Me.position >= Me.formulaText.Length OrElse Peek() <> ")"c Then
                Throw New FormatException("Missing closing parenthesis")
            End If
            Me.position += 1
            Return value
        End If

        If Char.IsDigit(Peek()) OrElse Peek() = "."c Then
            Return ParseNumber()
        End If

        If Char.IsLetter(Peek()) Then
            Return ParseNamedField()
        End If

        Throw New FormatException("Unrecognized character '" & Peek() & "' in formula")
    End Function

    Private Function ParseNumber() As Double
        Dim start As Integer = Me.position
        Do While Me.position < Me.formulaText.Length AndAlso (Char.IsDigit(Peek()) OrElse Peek() = "."c)
            Me.position += 1
        Loop
        Dim literalText As String = Me.formulaText.Substring(start, Me.position - start)
        Return Double.Parse(literalText, CultureInfo.InvariantCulture)
    End Function

    Private Function ParseNamedField() As Double
        Dim start As Integer = Me.position
        Do While Me.position < Me.formulaText.Length AndAlso (Char.IsLetterOrDigit(Peek()) OrElse Peek() = "_"c)
            Me.position += 1
        Loop
        Dim fieldName As String = Me.formulaText.Substring(start, Me.position - start)
        If Not Me.namedFields.ContainsKey(fieldName) Then
            Throw New FormatException("Unrecognized field name '" & fieldName & "' in formula")
        End If
        Return Me.namedFields(fieldName)
    End Function

    Private Function Peek() As Char
        Return Me.formulaText.Chars(Me.position)
    End Function

    Private Sub SkipWhitespace()
        Do While Me.position < Me.formulaText.Length AndAlso Char.IsWhiteSpace(Peek())
            Me.position += 1
        Loop
    End Sub

End Class
