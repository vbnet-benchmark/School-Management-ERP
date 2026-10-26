Imports System.Web

''' <summary>
''' The "dashboard" Quick Links destination -- always returns the visitor to their own role
''' dashboard, ignoring whatever shortcut target was requested. Selected by
''' QuickLinkRosterService.GetDestination for link type "dashboard" (also the fallback for any
''' unrecognized/unsupplied type). Exists so the Dispatch/DispatchSafe call site is a genuine
''' polymorphic call: not every concrete IQuickLinkDestination implementor leaves the
''' application, and which one runs depends entirely on the caller-supplied link type.
''' </summary>
Public Class InternalQuickLinkDestination
    Implements IQuickLinkDestination

    Public Sub Go(requestedTarget As String) Implements IQuickLinkDestination.Go
        HttpContext.Current.Response.Redirect("~/content/dashboards/staffdashboard.aspx")
    End Sub

    Public Sub GoSafe(requestedTarget As String) Implements IQuickLinkDestination.GoSafe
        HttpContext.Current.Response.Redirect("~/content/dashboards/staffdashboard.aspx")
    End Sub
End Class
