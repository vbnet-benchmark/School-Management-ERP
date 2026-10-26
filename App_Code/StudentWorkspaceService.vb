Imports System.Configuration
Imports System.Collections.Generic
Imports System.Text

''' <summary>
''' Backs two actions of the student "My Documents" workspace (Content/Student/mydocuments.aspx):
''' the export-job viewer (e.g. re-checking a previously requested transcript download) and the
''' "my documents" list's bulk-refresh action. The workspace tables (studentdocuments,
''' studentnotes, studentexportjobs) are a new per-student-owned-record store -- each row is
''' tagged with the owning student's admission number (admno), mirroring the admno-ownership
''' convention already used throughout studentsprofile/submittedassignments/feeschedule/
''' transactions.
''' </summary>
Public Class StudentWorkspaceService

    ''' <summary>
    ''' Looks up a single export-job record (e.g. a transcript or report-card download request)
    ''' by its record ID.
    ''' </summary>
    Public Shared Function GetExportJobRecordByID(ByVal jobId As Integer) As String
        Using con As New MySql.Data.MySqlClient.MySqlConnection(ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT title, detail FROM studentexportjobs WHERE id = ?", con)
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("id", jobId))
            Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader()
            Dim result As String
            If reader.Read() Then
                ' SINK: PLANTED-VB-HR-243
                result = "Export job: " & reader.Item(0).ToString() & " - " & reader.Item(1).ToString()
            Else
                result = "Export job not found"
            End If
            reader.Close()
            con.Close()
            Return result
        End Using
    End Function

    ''' <summary>
    ''' Safe twin of <see cref="GetExportJobRecordByID"/> -- only ever returns an export-job
    ''' record that is actually owned by the requesting student; any other student's job ID
    ''' resolves as "not found".
    ''' </summary>
    Public Shared Function GetExportJobRecordForOwner(ByVal jobId As Integer, ByVal ownerAdmNo As String) As String
        Using con As New MySql.Data.MySqlClient.MySqlConnection(ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT title, detail FROM studentexportjobs WHERE id = ? AND admno = ?", con)
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("id", jobId))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("admno", ownerAdmNo))
            Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader()
            Dim result As String
            If reader.Read() Then
                ' SAFE_SINK: PLANTED-VB-HR-243-safe
                result = "Export job: " & reader.Item(0).ToString() & " - " & reader.Item(1).ToString()
            Else
                result = "Export job not found"
            End If
            reader.Close()
            con.Close()
            Return result
        End Using
    End Function

    ''' <summary>
    ''' Looks up several uploaded-document records in one call, for the "my documents" list's
    ''' bulk-refresh action (re-reading a whole batch of previously-seen document IDs at once
    ''' instead of one request per row).
    ''' </summary>
    Public Shared Function GetDocumentRecordsBatchText(ByVal documentIds As List(Of Integer)) As String
        Dim combined As New StringBuilder()
        If documentIds IsNot Nothing Then
            For Each requestedId As Integer In documentIds
                Using con As New MySql.Data.MySqlClient.MySqlConnection(ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                    con.Open()
                    Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT title, detail FROM studentdocuments WHERE id = ?", con)
                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("id", requestedId))
                    Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        ' SINK: PLANTED-VB-HR-245
                        combined.Append(reader.Item(0).ToString()).Append(" - ").Append(reader.Item(1).ToString()).Append("; ")
                    End If
                    reader.Close()
                    con.Close()
                End Using
            Next
        End If
        Return combined.ToString()
    End Function

    ''' <summary>
    ''' Safe twin of <see cref="GetDocumentRecordsBatchText"/> -- every requested ID in the batch
    ''' is individually checked against the requesting student's own ownership before being
    ''' included, so a single batch request can never return a mix of the caller's own documents
    ''' and someone else's.
    ''' </summary>
    Public Shared Function GetDocumentRecordsBatchTextForOwner(ByVal documentIds As List(Of Integer), ByVal ownerAdmNo As String) As String
        Dim combined As New StringBuilder()
        If documentIds IsNot Nothing Then
            For Each requestedId As Integer In documentIds
                Using con As New MySql.Data.MySqlClient.MySqlConnection(ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                    con.Open()
                    Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT title, detail FROM studentdocuments WHERE id = ? AND admno = ?", con)
                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("id", requestedId))
                    cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("admno", ownerAdmNo))
                    Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        ' SAFE_SINK: PLANTED-VB-HR-245-safe
                        combined.Append(reader.Item(0).ToString()).Append(" - ").Append(reader.Item(1).ToString()).Append("; ")
                    End If
                    reader.Close()
                    con.Close()
                End Using
            Next
        End If
        Return combined.ToString()
    End Function

End Class
