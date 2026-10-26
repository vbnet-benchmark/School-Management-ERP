Imports System
Imports System.Web
Imports System.Collections.Generic

''' <summary>
''' Renders a short "message preview" banner shown to a staff member composing a new message
''' (see Content/Staff/newmsg.aspx.vb's Page_Load) before it is actually sent. Which concrete
''' strategy runs is resolved via a lookup table keyed by the compose-target audience the staff
''' member has selected, rather than a direct branch -- an unrecognized/unlisted audience value
''' falls back to the unencoded internal-memo strategy (a realistic "default open" lookup-table
''' bug, distinct from the already-committed WebGoatVBNET CWE-79 instance's direct if/else
''' dispatch over exactly two branches).
''' </summary>
Public Interface IMessagePreviewStrategy
    Function Render(ByVal subject As String) As String
End Interface

''' <summary>
''' Internal staff-to-staff memo preview: shows the subject exactly as submitted, no encoding.
''' Also the fallback strategy for any audience key the lookup table doesn't recognize.
''' </summary>
Public Class StaffMemoPreviewStrategy
    Implements IMessagePreviewStrategy

    Public Function Render(ByVal subject As String) As String Implements IMessagePreviewStrategy.Render
        ' SINK: PLANTED-VB-HR-28
        Return "<div class='preview-memo'>Internal memo preview: " & subject & "</div>"
    End Function
End Class

''' <summary>
''' Parent-portal message preview: HTML-encodes the subject before inclusion, matching the
''' encoding already applied by the real parent-facing message template.
''' </summary>
Public Class ParentPortalPreviewStrategy
    Implements IMessagePreviewStrategy

    Public Function Render(ByVal subject As String) As String Implements IMessagePreviewStrategy.Render
        ' SAFE_SINK: PLANTED-VB-HR-28-safe
        Return "<div class='preview-parent'>Parent portal preview: " & HttpUtility.HtmlEncode(subject) & "</div>"
    End Function
End Class

''' <summary>
''' School-board digest preview: a second, independently-encoded audience (board members see a
''' monthly digest compiled from message subjects), added so the lookup table has more than one
''' safe entry and isn't just a two-way raw-vs-encoded switch.
''' </summary>
Public Class BoardDigestPreviewStrategy
    Implements IMessagePreviewStrategy

    Public Function Render(ByVal subject As String) As String Implements IMessagePreviewStrategy.Render
        Return "<div class='preview-board'>Board digest preview: " & HttpUtility.HtmlEncode(subject) & "</div>"
    End Function
End Class

''' <summary>
''' Resolves the preview strategy for a given audience key. Built once per call (cheap, 3
''' entries) rather than cached, since this is a low-traffic preview feature, not a hot path.
''' </summary>
Public Class MessagePreviewStrategyFactory
    Public Shared Function Resolve(ByVal audienceKey As String) As IMessagePreviewStrategy
        Dim strategies As New Dictionary(Of String, IMessagePreviewStrategy)(StringComparer.OrdinalIgnoreCase) From {
            {"PARENT", New ParentPortalPreviewStrategy()},
            {"BOARD", New BoardDigestPreviewStrategy()}
        }

        Dim resolved As IMessagePreviewStrategy = Nothing
        If audienceKey IsNot Nothing AndAlso strategies.TryGetValue(audienceKey, resolved) Then
            Return resolved
        End If

        ' Unrecognized or missing audience key (including the default "STAFF" case, which was
        ' never added to the table because internal memos were never expected to need special
        ' handling) falls through to the unencoded strategy.
        Return New StaffMemoPreviewStrategy()
    End Function
End Class
