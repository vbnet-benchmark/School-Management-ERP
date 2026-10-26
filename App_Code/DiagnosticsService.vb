Imports Microsoft.VisualBasic
Imports System.Configuration

''' <summary>
''' Result of a connectivity diagnostic performed by <see cref="DiagnosticsService"/>.
''' On failure, <see cref="Detail"/> always carries the raw exception detail -- this class
''' never decides whether that detail is safe to show to a user; the caller (the UI layer)
''' makes that decision.
''' </summary>
Public Class DiagnosticResult
    Public Property Success As Boolean
    Public Property Detail As String
End Class

''' <summary>
''' Admin-facing connectivity diagnostics (e.g. testing a secondary/backup database
''' connection) used by the Admin > Portal Options page.
''' </summary>
Public Class DiagnosticsService

    ''' <summary>
    ''' Attempts to open a connection using the named connection string and reports whether
    ''' it succeeded. On failure, the caught exception's own message is always carried back
    ''' on the result -- never logged, discarded, or sanitized here -- so the display
    ''' decision belongs entirely to the caller.
    ''' </summary>
    Public Function TestBackupConnection(connectionStringName As String) As DiagnosticResult
        Dim result As New DiagnosticResult()
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(ConfigurationManager.ConnectionStrings(connectionStringName).ConnectionString)
                con.Open()
            End Using
            result.Success = True
            result.Detail = "Connection succeeded."
        Catch ex As Exception
            result.Success = False
            result.Detail = ex.Message
        End Try
        Return result
    End Function

End Class
