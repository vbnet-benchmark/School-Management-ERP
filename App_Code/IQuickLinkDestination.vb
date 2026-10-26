''' <summary>
''' Pluggable "Quick Links" destination abstraction for the staff dashboard's quick-access
''' shortcut panel. QuickLinkRosterService resolves the concrete implementor at runtime from the
''' caller-supplied link type, so the Dispatch/DispatchSafe call sites on the dashboard are
''' genuinely type/polymorphism-dependent -- whether a given shortcut sends the visitor off this
''' site depends entirely on which concrete class the roster hands back for the requested type.
''' </summary>
Public Interface IQuickLinkDestination
    Sub Go(requestedTarget As String)
    Sub GoSafe(requestedTarget As String)
End Interface
