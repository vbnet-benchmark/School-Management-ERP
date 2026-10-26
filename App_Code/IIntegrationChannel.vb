''' <summary>
''' Pluggable integration-channel abstraction for the "Integration Channel Dispatch" feature on
''' Content/Admin/externalintegrations.aspx.vb. IntegrationChannelRosterService resolves the
''' concrete implementor at runtime from the caller-supplied channel type, so the Send/SendSafe
''' call sites in that roster are genuinely type/polymorphism-dependent -- whether a given
''' dispatch actually reaches the network depends on which concrete class the roster handed back
''' for the requested channel type. Mirrors this project's own StudentImportParserFactory
''' "resolve to a single shared call site" shape (App_Code/StudentImportParsers.vb).
''' </summary>
Public Interface IIntegrationChannel
    Function Send(destination As String, message As String) As String
    Function SendSafe(destination As String, message As String) As String
End Interface
