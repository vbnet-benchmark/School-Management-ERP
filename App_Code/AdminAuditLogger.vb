Imports System.IO

''' <summary>
''' Shared flat-file admin-activity audit trail used by several admin pages (transport routes,
''' department membership, stock adjustments, bulk messaging). Each caller resolves its own log
''' file path (or base log folder) via its own Server.MapPath and passes the plain string in --
''' this class never depends on HttpContext.Current itself, matching the existing
''' App_Code/TeachingResourceService.vb convention of taking an already-resolved path as a
''' parameter rather than resolving one internally.
''' </summary>
Public Class AdminAuditLogger

    ''' <summary>
    ''' Appends a single admin-activity line to the given flat log file. Never neutralizes
    ''' embedded CR/LF in <paramref name="detail"/>, so an attacker-controlled value (e.g. a
    ''' free-text form field) can forge a fabricated second log line that never happened.
    ''' </summary>
    Public Shared Sub LogAdminActivity(logFilePath As String, category As String, detail As String)
        ' SINK: PLANTED-VB-HR-278
        File.AppendAllText(logFilePath, "[" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "] [" & category & "] " & detail & vbCrLf)
    End Sub

    ''' <summary>
    ''' Safe twin of <see cref="LogAdminActivity"/> -- strips embedded CR/LF from
    ''' <paramref name="detail"/> before it is written, so the written line can never be split
    ''' into a fabricated second entry.
    ''' </summary>
    Public Shared Sub LogAdminActivitySafe(logFilePath As String, category As String, detail As String)
        Dim sanitizedDetail As String = Replace(Replace(detail, vbCr, " "), vbLf, " ")
        ' SAFE_SINK: PLANTED-VB-HR-278-safe
        File.AppendAllText(logFilePath, "[" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "] [" & category & "] " & sanitizedDetail & vbCrLf)
    End Sub

    ''' <summary>
    ''' Dispatches a stock-adjustment audit entry to a per-event-type log file, keyed on
    ''' <paramref name="eventType"/> ("Addition" or "Removal", read from the admin's own dropdown
    ''' selection on Content/Admin/stockadjust.aspx). A whole-project grep found three genuine
    ''' polymorphic hierarchies in this codebase -- App_Code/StudentOwnedRecordRepository.vb's
    ''' StudentOwnedRecordRepositoryBase (CWE-639 round), App_Code/MessagePreviewRenderer.vb's
    ''' IMessagePreviewStrategy (CWE-79 round), and App_Code/StudentImportParsers.vb's
    ''' XmlImportParserBase (CWE-611 round) -- but none has any natural tie to audit-log routing:
    ''' the first dispatches per-table record lookups, the second dispatches HTML-preview
    ''' rendering by audience, and the third dispatches XML-import parsing. None applies here, so
    ''' this substitutes a hardcoded Select Case over the event-type key for genuine polymorphism,
    ''' mirroring this project's own Content/Admin/addteacher.aspx.vb#btnUploadTeacherDocument_Click
    ''' precedent (PLANTED-VB-HR-214, "no genuine interface/MustInherit/Overridable dispatch exists
    ''' ... dispatches on an upload-type key read from a dropdown instead") for the same kind of
    ''' substitution.
    ''' </summary>
    Public Shared Sub LogStockAdjustment(baseLogFolder As String, eventType As String, item As String, details As String)
        Dim logFilePath As String
        Select Case eventType
            Case "Addition"
                logFilePath = baseLogFolder & "stockaudit_in.log"
            Case "Removal"
                logFilePath = baseLogFolder & "stockaudit_out.log"
            Case Else
                logFilePath = baseLogFolder & "stockaudit_other.log"
        End Select
        ' SINK: PLANTED-VB-HR-279
        File.AppendAllText(logFilePath, "[" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "] " & eventType & " - " & item & ": " & details & vbCrLf)
    End Sub

    ''' <summary>
    ''' Safe twin of <see cref="LogStockAdjustment"/> -- same category-key dispatch, but strips
    ''' embedded CR/LF from <paramref name="details"/> before it is written.
    ''' </summary>
    Public Shared Sub LogStockAdjustmentSafe(baseLogFolder As String, eventType As String, item As String, details As String)
        Dim logFilePath As String
        Select Case eventType
            Case "Addition"
                logFilePath = baseLogFolder & "stockaudit_in.log"
            Case "Removal"
                logFilePath = baseLogFolder & "stockaudit_out.log"
            Case Else
                logFilePath = baseLogFolder & "stockaudit_other.log"
        End Select
        Dim sanitizedDetails As String = Replace(Replace(details, vbCr, " "), vbLf, " ")
        ' SAFE_SINK: PLANTED-VB-HR-279-safe
        File.AppendAllText(logFilePath, "[" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "] " & eventType & " - " & item & ": " & sanitizedDetails & vbCrLf)
    End Sub

    ''' <summary>
    ''' Per-recipient audit entry for a bulk admin message dispatch (one call per loop iteration
    ''' over the message's recipient list). The recipient identifier is logged alongside the
    ''' free-text subject/body, which is never neutralized here.
    ''' </summary>
    Public Shared Sub LogMessageDispatch(logFilePath As String, recipient As String, messageText As String)
        ' SINK: PLANTED-VB-HR-280
        File.AppendAllText(logFilePath, "[" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "] Message to " & recipient & ": " & messageText & vbCrLf)
    End Sub

    ''' <summary>
    ''' Safe twin of <see cref="LogMessageDispatch"/> -- strips embedded CR/LF from
    ''' <paramref name="messageText"/> before it is written.
    ''' </summary>
    Public Shared Sub LogMessageDispatchSafe(logFilePath As String, recipient As String, messageText As String)
        Dim sanitizedMessageText As String = Replace(Replace(messageText, vbCr, " "), vbLf, " ")
        ' SAFE_SINK: PLANTED-VB-HR-280-safe
        File.AppendAllText(logFilePath, "[" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "] Message to " & recipient & ": " & sanitizedMessageText & vbCrLf)
    End Sub

End Class
