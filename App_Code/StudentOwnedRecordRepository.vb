Imports System.Configuration

''' <summary>
''' Generic record viewer for the student "My Documents" workspace's three owned-record tables
''' (studentdocuments, studentnotes, studentexportjobs) -- all three share the same
''' id/admno/title/detail shape, so one lookup method can serve all of them once a concrete
''' subclass says which table it reads from. Genuine polymorphism (MustInherit/MustOverride),
''' mirroring this project's own App_Code/StudentImportParsers.vb precedent, used here instead of
''' a Select-Case string-built table name.
''' </summary>
Public MustInherit Class StudentOwnedRecordRepositoryBase

    ''' <summary>The workspace table this concrete repository reads from.</summary>
    Public MustOverride Function TableName() As String

    ''' <summary>
    ''' Looks up a single workspace record of this repository's table by its record ID.
    ''' </summary>
    Public Function FetchRecordById(ByVal recordId As Integer) As String
        Using con As New MySql.Data.MySqlClient.MySqlConnection(ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT title, detail FROM " & Me.TableName() & " WHERE id = ?", con)
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("id", recordId))
            Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader()
            Dim result As String
            If reader.Read() Then
                ' SINK: PLANTED-VB-HR-244
                result = "Record: " & reader.Item(0).ToString() & " - " & reader.Item(1).ToString()
            Else
                result = "Record not found"
            End If
            reader.Close()
            con.Close()
            Return result
        End Using
    End Function

    ''' <summary>
    ''' Same generic record lookup as <see cref="FetchRecordById"/>, but only ever returns the
    ''' record if it is actually owned by the requesting student -- regardless of which of the
    ''' three concrete tables this repository resolves to.
    ''' </summary>
    Public Function FetchRecordForOwner(ByVal recordId As Integer, ByVal ownerAdmNo As String) As String
        Using con As New MySql.Data.MySqlClient.MySqlConnection(ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT title, detail FROM " & Me.TableName() & " WHERE id = ? AND admno = ?", con)
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("id", recordId))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("admno", ownerAdmNo))
            Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader()
            Dim result As String
            If reader.Read() Then
                ' SAFE_SINK: PLANTED-VB-HR-244-safe
                result = "Record: " & reader.Item(0).ToString() & " - " & reader.Item(1).ToString()
            Else
                result = "Record not found"
            End If
            reader.Close()
            con.Close()
            Return result
        End Using
    End Function

End Class

''' <summary>Resolves to the "studentdocuments" table.</summary>
Public Class StudentDocumentRepository
    Inherits StudentOwnedRecordRepositoryBase

    Public Overrides Function TableName() As String
        Return "studentdocuments"
    End Function
End Class

''' <summary>Resolves to the "studentnotes" table.</summary>
Public Class StudentNoteRepository
    Inherits StudentOwnedRecordRepositoryBase

    Public Overrides Function TableName() As String
        Return "studentnotes"
    End Function
End Class

''' <summary>Resolves to the "studentexportjobs" table.</summary>
Public Class StudentExportJobRepository
    Inherits StudentOwnedRecordRepositoryBase

    Public Overrides Function TableName() As String
        Return "studentexportjobs"
    End Function
End Class

''' <summary>
''' Resolves which concrete workspace repository to use for a caller-supplied record "type"
''' selector. NOTE: callers choose which table to query purely by this string -- there is no
''' server-side verification that the requested type is one the current page/feature would ever
''' legitimately pass, so a caller can request any of the three repositories below regardless of
''' which form or link actually produced the request.
''' </summary>
Public Class StudentOwnedRecordRepositoryFactory
    Public Shared Function Resolve(ByVal recordType As String) As StudentOwnedRecordRepositoryBase
        Select Case recordType
            Case "note"
                Return New StudentNoteRepository()
            Case "export"
                Return New StudentExportJobRepository()
            Case Else
                Return New StudentDocumentRepository()
        End Select
    End Function
End Class
