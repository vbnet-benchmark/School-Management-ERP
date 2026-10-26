''' <summary>
''' Interface implemented by any admin page that exposes a small set of named "quick actions"
''' through <see cref="AdminQuickActionDispatcher"/>'s safe, allow-list-based path (see
''' DispatchSafe below). Kept deliberately minimal (two no-argument maintenance actions) -- it
''' exists only so the safe twin has a compiler-checked call to make, not as a reusable
''' abstraction over several interchangeable implementations, so it is not treated as the
''' "genuine reusable polymorphic hierarchy" a type/polymorphism-dependent instance would need
''' (only Content/Admin/erorlog.aspx.vb implements it).
''' </summary>
Public Interface IAdminQuickActionTarget
    Sub ArchiveLog()
    Sub ExportLog()
End Interface

''' <summary>
''' Shared "quick action" dispatcher for small admin-only maintenance operations exposed from
''' admin pages (currently Content/Admin/erorlog.aspx.vb's Archive/Export quick actions). The
''' caller-supplied action NAME selects which member of the calling page object actually runs,
''' via Microsoft.VisualBasic.CallByName -- any public member of the page becomes reachable this
''' way, not only the two maintenance actions the page's own UI exposes.
'''
''' A whole-project grep (per vbnet-cwe-676-planting-research.md) found no genuine, reusable
''' polymorphic hierarchy in this codebase with any natural tie to quick-action/named-dispatch
''' routing (StudentOwnedRecordRepositoryBase dispatches per-table record lookups,
''' LoginCompletionStrategyBase dispatches SSO login completion, IMessagePreviewStrategy
''' dispatches HTML-preview rendering by audience, XmlImportParserBase dispatches XML-import
''' parsing -- none of them gate a Shell/CallByName-style dispatch decision). This mirrors this
''' project's own App_Code/AdminAuditLogger.vb#LogStockAdjustment precedent ("no genuine
''' interface/MustInherit/Overridable dispatch exists... substitutes a hardcoded Select Case"),
''' so PLANTED-VB-HR-369 (Content/Admin/erorlog.aspx.vb) substitutes a loop-batch CallByName
''' shape in place of a type/polymorphism-dependent instance -- disclosed honestly rather than
''' claimed as something it is not.
''' </summary>
Public Class AdminQuickActionDispatcher

    ''' <summary>
    ''' Dispatches a single named quick action against <paramref name="target"/> (the calling
    ''' admin page itself). <paramref name="actionName"/> comes straight from the request with no
    ''' allow-list, so any public method/property of <paramref name="target"/> -- not only the
    ''' two the page's own UI exposes -- is reachable this way.
    ''' </summary>
    Public Shared Sub Dispatch(ByVal target As Object, ByVal actionName As String)
        ' SINK: PLANTED-VB-HR-368
        CallByName(target, actionName, CallType.Method)
    End Sub

    ''' <summary>
    ''' Safe twin of <see cref="Dispatch"/> -- the caller-supplied value is only ever used as an
    ''' allow-list KEY; the Select Case below is the only place CallByName-style dynamic
    ''' dispatch is replaced with a direct, compiler-checked call, so no dynamic-dispatch surface
    ''' remains regardless of what the caller supplies.
    ''' </summary>
    Public Shared Sub DispatchSafe(ByVal target As IAdminQuickActionTarget, ByVal actionKey As String)
        Select Case actionKey
            Case "archive"
                ' SAFE_SINK: PLANTED-VB-HR-368-safe
                target.ArchiveLog()
            Case "export"
                target.ExportLog()
            Case Else
                ' Unrecognized key -- nothing is dispatched.
        End Select
    End Sub

End Class
