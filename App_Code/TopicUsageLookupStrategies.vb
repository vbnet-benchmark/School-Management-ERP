Imports System.Data
Imports System.Configuration

''' <summary>
''' Resolves an early-years topic usage lookup for the "Manage Early Years Scores"
''' admin screen. Two concrete strategies exist because topics recorded before the
''' numeric TopicID column was introduced are still only identifiable by their
''' original free-text topic name -- both eras need to stay queryable from the same
''' screen until the historical rows are migrated.
''' </summary>
Public MustInherit Class TopicUsageLookupStrategy
    Public MustOverride Function LookupUsage(ByVal rawKey As String) As DataTable

    Protected Function NewUsageTable() As DataTable
        Dim ds As New DataTable
        ds.Columns.Add("student")
        ds.Columns.Add("session")
        ds.Columns.Add("grade")
        Return ds
    End Function
End Class

''' <summary>
''' Current-era lookup: TopicID is always the server-assigned integer shown in the
''' topic list, bound as a query parameter.
''' </summary>
Public Class NumericTopicIdLookup
    Inherits TopicUsageLookupStrategy

    Public Overrides Function LookupUsage(ByVal rawKey As String) As DataTable
        Dim ds As DataTable = NewUsageTable()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT elscores.student, elscores.session, elscores.grade FROM elscores WHERE elscores.topic = @topicKey", con)
            cmd.Parameters.AddWithValue("@topicKey", rawKey)
            Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader() ' SAFE_SINK: PLANTED-VB-HR-534-safe
            Do While reader.Read()
                ds.Rows.Add(reader(0).ToString(), reader(1).ToString(), reader(2).ToString())
            Loop
            reader.Close()
        End Using
        Return ds
    End Function
End Class

''' <summary>
''' Legacy lookup for topics recorded before TopicID existed: the topic is still
''' identified by its original free-text name in those old rows.
''' </summary>
Public Class LegacyTopicNameLookup
    Inherits TopicUsageLookupStrategy

    Public Overrides Function LookupUsage(ByVal rawKey As String) As DataTable
        Dim ds As DataTable = NewUsageTable()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT elscores.student, elscores.session, elscores.grade FROM elscores WHERE elscores.topic = '" & rawKey & "'", con)
            Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader() ' SINK: PLANTED-VB-HR-534
            Do While reader.Read()
                ds.Rows.Add(reader(0).ToString(), reader(1).ToString(), reader(2).ToString())
            Loop
            reader.Close()
        End Using
        Return ds
    End Function
End Class
