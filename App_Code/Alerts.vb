Imports Microsoft.VisualBasic

Public Class Alerts


    Public Function success_message(message As String) As String
        Dim html As String = "<div class='alert alert-success alert-st-one' role='alert'><p class='message-mg-rt message-alert-none'><strong>" & message & "</strong></p></div>"
        Return html
    End Function

    Public Function error_message(message As String) As String
        Dim html As String = "<div class='alert alert-danger alert-mg-b alert-success-style4'><p class='message-mg-rt'><strong>" & message & "</strong></p></div> "
        Return html
    End Function








End Class
