Imports System.Text
Imports System.Configuration
Imports System.Data
Imports System.Web.UI.WebControls
Imports System.Collections.Generic

''' <summary>
''' The student's own "My Documents" workspace: a small set of per-student-owned records
''' (uploaded personal documents, private notes, and export-job requests such as a transcript
''' download) a student can look back up by the record's own ID once they've created it -- the
''' first per-student-owned-record store in this project outside the organic
''' assignments/messages/fee areas. Every workspace table is tagged with the owning student's
''' admission number (admno), mirroring the admno-ownership convention already used throughout
''' studentsprofile/submittedassignments/feeschedule/transactions. Reached from Content/Student/
''' assignments.aspx's "My Documents" button.
''' </summary>
Partial Class Student_MyDocuments
    Inherits System.Web.UI.Page

    Dim check As New CheckUser

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If check.Check_Student(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
    End Sub

    Protected Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Response.Redirect("~/content/student/assignments.aspx")
    End Sub

    ' ---- Direct, in-page straight-line query (PLANTED-VB-HR-241) ----

    Protected Sub btnViewDocument_Click(sender As Object, e As EventArgs) Handles btnViewDocument.Click
        Dim requestedDocId As Integer = CInt(txtDocumentId.Text)
        Using con As New MySql.Data.MySqlClient.MySqlConnection(ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT title, detail FROM studentdocuments WHERE id = ?", con)
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("id", requestedDocId))
            Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader()
            If reader.Read() Then
                ' SINK: PLANTED-VB-HR-241
                lblDocumentResult.Text = "Document: " & reader.Item(0).ToString() & " - " & reader.Item(1).ToString()
            Else
                lblDocumentResult.Text = "Document not found"
            End If
            reader.Close()
            con.Close()
        End Using
    End Sub

    ''' <summary>
    ''' Safe twin of <see cref="btnViewDocument_Click"/> -- only ever returns a document actually
    ''' owned by the current student; any other student's document ID resolves as "not found".
    ''' </summary>
    Protected Sub btnViewDocumentSafe_Click(sender As Object, e As EventArgs) Handles btnViewDocumentSafe.Click
        Dim requestedDocId As Integer = CInt(txtDocumentId.Text)
        Dim currentAdmNo As String = Session("StudentID").ToString()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT title, detail FROM studentdocuments WHERE id = ? AND admno = ?", con)
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("id", requestedDocId))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("admno", currentAdmNo))
            Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader()
            If reader.Read() Then
                ' SAFE_SINK: PLANTED-VB-HR-241-safe
                lblDocumentResult.Text = "Document: " & reader.Item(0).ToString() & " - " & reader.Item(1).ToString()
            Else
                lblDocumentResult.Text = "Document not found"
            End If
            reader.Close()
            con.Close()
        End Using
    End Sub

    ' ---- Indirect, same-class private helper (PLANTED-VB-HR-242) ----

    Protected Sub btnViewNote_Click(sender As Object, e As EventArgs) Handles btnViewNote.Click
        Dim requestedNoteId As Integer = CInt(txtNoteId.Text)
        lblNoteResult.Text = FetchNoteTextUnchecked(requestedNoteId)
    End Sub

    Protected Sub btnViewNoteSafe_Click(sender As Object, e As EventArgs) Handles btnViewNoteSafe.Click
        Dim requestedNoteId As Integer = CInt(txtNoteId.Text)
        lblNoteResult.Text = FetchNoteTextForCurrentStudent(requestedNoteId)
    End Sub

    ''' <summary>
    ''' Looks up a single private-note record's title and content by its record ID, for the
    ''' "View Note" action above.
    ''' </summary>
    Private Function FetchNoteTextUnchecked(ByVal noteId As Integer) As String
        Using con As New MySql.Data.MySqlClient.MySqlConnection(ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT title, detail FROM studentnotes WHERE id = ?", con)
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("id", noteId))
            Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader()
            Dim result As String
            If reader.Read() Then
                ' SINK: PLANTED-VB-HR-242
                result = "Note: " & reader.Item(0).ToString() & " - " & reader.Item(1).ToString()
            Else
                result = "Note not found"
            End If
            reader.Close()
            con.Close()
            Return result
        End Using
    End Function

    ''' <summary>
    ''' Safe twin of <see cref="FetchNoteTextUnchecked"/> -- only ever returns a note actually
    ''' owned by the current student.
    ''' </summary>
    Private Function FetchNoteTextForCurrentStudent(ByVal noteId As Integer) As String
        Dim currentAdmNo As String = Session("StudentID").ToString()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT title, detail FROM studentnotes WHERE id = ? AND admno = ?", con)
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("id", noteId))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("admno", currentAdmNo))
            Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader()
            Dim result As String
            If reader.Read() Then
                ' SAFE_SINK: PLANTED-VB-HR-242-safe
                result = "Note: " & reader.Item(0).ToString() & " - " & reader.Item(1).ToString()
            Else
                result = "Note not found"
            End If
            reader.Close()
            con.Close()
            Return result
        End Using
    End Function

    ' ---- Interprocedural, via StudentWorkspaceService (PLANTED-VB-HR-243) ----

    Protected Sub btnViewExportJob_Click(sender As Object, e As EventArgs) Handles btnViewExportJob.Click
        Dim requestedJobId As Integer = CInt(txtExportJobId.Text)
        lblExportResult.Text = StudentWorkspaceService.GetExportJobRecordByID(requestedJobId)
    End Sub

    Protected Sub btnViewExportJobSafe_Click(sender As Object, e As EventArgs) Handles btnViewExportJobSafe.Click
        Dim requestedJobId As Integer = CInt(txtExportJobId.Text)
        Dim currentAdmNo As String = Session("StudentID").ToString()
        lblExportResult.Text = StudentWorkspaceService.GetExportJobRecordForOwner(requestedJobId, currentAdmNo)
    End Sub

    ' ---- Interprocedural + polymorphic dispatch (PLANTED-VB-HR-244) ----

    Protected Sub btnViewRecordByType_Click(sender As Object, e As EventArgs) Handles btnViewRecordByType.Click
        Dim requestedRecordId As Integer = CInt(txtRecordId.Text)
        Dim repo As StudentOwnedRecordRepositoryBase = StudentOwnedRecordRepositoryFactory.Resolve(ddlRecordType.SelectedValue)
        lblRecordResult.Text = repo.FetchRecordById(requestedRecordId)
    End Sub

    Protected Sub btnViewRecordByTypeSafe_Click(sender As Object, e As EventArgs) Handles btnViewRecordByTypeSafe.Click
        Dim requestedRecordId As Integer = CInt(txtRecordId.Text)
        Dim currentAdmNo As String = Session("StudentID").ToString()
        Dim repo As StudentOwnedRecordRepositoryBase = StudentOwnedRecordRepositoryFactory.Resolve(ddlRecordType.SelectedValue)
        lblRecordResult.Text = repo.FetchRecordForOwner(requestedRecordId, currentAdmNo)
    End Sub

    ' ---- Interprocedural + batch loop (PLANTED-VB-HR-245) ----

    Protected Sub btnViewBatch_Click(sender As Object, e As EventArgs) Handles btnViewBatch.Click
        Dim parsedIds As List(Of Integer) = ParseBatchIds(txtBatchIds.Text)
        lblBatchResult.Text = StudentWorkspaceService.GetDocumentRecordsBatchText(parsedIds)
    End Sub

    Protected Sub btnViewBatchSafe_Click(sender As Object, e As EventArgs) Handles btnViewBatchSafe.Click
        Dim parsedIds As List(Of Integer) = ParseBatchIds(txtBatchIds.Text)
        Dim currentAdmNo As String = Session("StudentID").ToString()
        lblBatchResult.Text = StudentWorkspaceService.GetDocumentRecordsBatchTextForOwner(parsedIds, currentAdmNo)
    End Sub

    Private Function ParseBatchIds(ByVal rawList As String) As List(Of Integer)
        Dim requestedIds As String() = rawList.Split(","c)
        Dim parsedIds As New List(Of Integer)
        For Each rawId As String In requestedIds
            If rawId.Trim() <> "" Then
                parsedIds.Add(CInt(rawId.Trim()))
            End If
        Next
        Return parsedIds
    End Function

End Class
