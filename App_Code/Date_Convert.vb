Imports Microsoft.VisualBasic

Public Class Date_Convert
    Inherits Gen_Functions

    Public Function ToVb(value)
        Return Convert.ToDateTime(value).ToString("dd/MM/yyyy hh:mm tt")
    End Function

    Public Function ToSQL(value)
        Return Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm:ss")
    End Function

    Public Function Validate_Date(Value As String) As Boolean
        If Value = "" Then
            Return False
        Else
            Dim a As Array = Split(Value, "/")
            If a(0).ToString.Count <> 2 Or a(1).ToString.Count <> 2 Or a(2).ToString.Count <> 4 Then
                Return False
            Else
                Return True
            End If
        End If
    End Function

    Public Function Validate_Time(Value As String) As Boolean
        If Value = "" Then
            Return False
        Else
            Dim a As Array = Split(Value, ":")
            If a.Length <> 2 Then
                Return False
            Else
                Return True
            End If
        End If
    End Function

    Public Function Time_Dff(value As String) As String
        Dim dob As Date = ToVb(value)
        Dim sage As TimeSpan = Now.Subtract(dob)
        Dim timelag As String = ""
        If sage.Days < 1 Then
            If sage.Hours < 1 Then
                If sage.Minutes < 1 Then
                    timelag = "Just now"
                ElseIf sage.Minutes = 1 Then
                    timelag = 1 & " Min ago"
                Else
                    timelag = sage.Minutes & " Mins ago"
                End If
            ElseIf sage.Hours = 1 Then
                timelag = "1 hour ago"
            Else
                timelag = sage.Hours & " Hrs ago"
            End If
        ElseIf sage.Days = 1 Then
            timelag = "1 Day ago"
        Else
            timelag = sage.Days & " Days ago"
        End If
        Return timelag
    End Function

End Class
